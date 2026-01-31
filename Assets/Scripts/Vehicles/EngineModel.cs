using System.Collections.Generic;
using System;
using UnityEngine;


public class EngineModel
{
    enum EngineState
    {
        Stalled,
        Running
    }
    EngineState currentState = EngineState.Running;
    Dictionary<EngineState, Action<float, float, float>> updateMethods;

    [Header("車種による調整値")]
    float maxRPM;           // レッドライン
    float idleRPM;          // アイドリング回転数

    // RPMに対するトルク曲線をInspectorから再現する
    AnimationCurve torqueCurve;
    float flywheelInertia;  // 慣性モーメント
    float pumpingLossFactor;// 吸排気・補機損失

    [Header("状態")]
    float currentRPM = 1000f;       // 現在の回転数
    float engineBrakeTorqueCap = 120f;


    /// <summary>
    /// 現在のエンジントルク
    /// </summary>
    public float OutputTorque { get; private set; }

    /// <summary>
    /// 現在の回転数
    /// </summary>
    public float CurrentRPM => currentRPM;
    float engineAngularVelocity;

    public EngineModel(CarSpec spec)
    {
        maxRPM = spec.MaxRPM;
        idleRPM = spec.IdleRPM;
        torqueCurve = spec.TorqueCurve;
        flywheelInertia = spec.FlywheelInertia;
        pumpingLossFactor = spec.PumpingLossFactor;

        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
    }
    
    /// <summary>
    /// 外部トルクを考慮しない純トルク計算
    /// </summary>
    /// <param name="inputThrottle"></param>
    public void UpdateTorque(float inputThrottle)
    {
        if (currentState == EngineState.Stalled)
        {
            OutputTorque = 0f;
            return;
        }
        //1以上にはならないはずだが1を超えると計算が壊れるのでクランプする
        inputThrottle = Mathf.Clamp01(inputThrottle);
        float idleThrottle = UpdateIdleControl(inputThrottle);
        float throttle = Mathf.Clamp01(inputThrottle + idleThrottle);

        // スロットルに応じた理論トルク
        float baseTorque = torqueCurve.Evaluate(currentRPM) * throttle;
        
        // 損失分を差し引く
        float lossTorque = baseTorque * pumpingLossFactor;

        float rpmPer = Mathf.Clamp01(currentRPM / maxRPM);

        float idleFade = Mathf.Clamp01((currentRPM - idleRPM) / Mathf.Max(1f, idleRPM * 0.5f));

        float rpmFactor = Mathf.Pow(rpmPer, 3);

        float engineBrakeTorque = (1f - inputThrottle) * engineBrakeTorqueCap * rpmFactor * idleFade;

        // エンジンの純出力トルク
        OutputTorque = baseTorque - lossTorque - engineBrakeTorque;
    }

    /// <summary>
    /// トランスミッションの反力トルクを適用し、回転数を更新する
    /// </summary>
    /// <param name="externalTorque">抵抗がある場合は-方向の値を期待する</param>
    /// <param name="deltaTime"></param>
    public void ApplyExternalTorque(float externalTorque, float deltaTime)
    {
        if (currentState == EngineState.Stalled)
        {
            currentRPM = Mathf.MoveTowards(currentRPM, 0f, 2000f * deltaTime);
            return;
        }
        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
        // 出力トルクと外部トルク（反力）を合わせて慣性応答を計算
        float netTorque = OutputTorque + externalTorque;
        float viscousLoss = 0.000003f * currentRPM * currentRPM; // 空気抵抗的な損失
        float frictionLoss = 6f * (currentRPM / maxRPM);      // 機械摩擦
        float mechanicalLoss = viscousLoss + frictionLoss;
        

        //float mechanicalLoss = 0.02f * currentRPM;
        netTorque -= mechanicalLoss;
        // 角加速度(rad/s^2)
        float angularAcceleration = netTorque / flywheelInertia;

        // 角速度更新
        engineAngularVelocity += angularAcceleration * deltaTime;

        // RPM更新
        currentRPM = Mathf.Clamp(
            engineAngularVelocity * 60f / (2f * Mathf.PI),
            0f,
            maxRPM
        );

        // エンジン停止判定
        if (currentRPM < idleRPM * 0.7f &&
             netTorque < 0)
        {
            currentState = EngineState.Stalled;
        }
    }
    /// <summary>
    /// アイドル制御
    /// </summary>
    /// <param name="throttle"></param>
    /// <returns>スロットル補正値</returns>
    float UpdateIdleControl(float throttle)
    {
        float effectiveThrottle = 0f;
        if (throttle < 0.1f)
        {
            float rpmDiff = idleRPM - currentRPM;

            // 回転が下がったらさらに開ける（P制御）
            float correction = Mathf.Clamp(rpmDiff * 0.008f, 0f, 0.03f);

            effectiveThrottle += correction;
        }
        return effectiveThrottle;
    }
}
