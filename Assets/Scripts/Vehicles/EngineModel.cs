using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// エンジン単体クラス（回転数・トルク計算）
/// 車速やギアには依存せず、Throttle入力と外部トルクから回転数・出力トルクを計算
/// </summary>
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

    /// <summary>
    /// 現在のエンジントルク
    /// </summary>
    public float OutputTorque { get; private set; }

    /// <summary>
    /// 現在の回転数
    /// </summary>
    public float CurrentRPM => currentRPM;
    CarSpec spec;
    float engineAngularVelocity;

    public EngineModel(CarSpec spec)
    {
        this.spec = spec;
        maxRPM = spec.MaxRPM;
        idleRPM = spec.IdleRPM;
        torqueCurve = spec.TorqueCurve;
        flywheelInertia = spec.FlywheelInertia;
        pumpingLossFactor = spec.PumpingLossFactor;
        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
    }
    
    public void UpdateTorque(float throttle)
    {
        if (currentState == EngineState.Stalled)
        {
            OutputTorque = 0f;
            return;
        }
        throttle += UpdateIdleControl(throttle);
        // スロットルに応じた理論トルク
        float baseTorque = torqueCurve.Evaluate(currentRPM) * throttle;
        
        // 損失分を差し引く
        float lossTorque = baseTorque * pumpingLossFactor;

        // エンジンの純出力トルク
        OutputTorque = baseTorque - lossTorque;
    }
    public void ApplyExternalTorque(float externalTorque, float deltaTime)
    {
        if (currentState == EngineState.Stalled)
        {
            currentRPM = Mathf.MoveTowards(currentRPM, 0f, 2000f * deltaTime);
            return;
        }
        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
        // 出力トルクと外部トルク（反力）を合わせて慣性応答を計算
        float netTorque = OutputTorque - externalTorque;
        float mechanicalLoss = 0.02f * currentRPM; // 調整値（実験でチューニング）
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
        if (currentRPM < idleRPM * 0.7f)
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

            // 最低限開けておくアイドルベース
            float baseIdleThrottle = 0.07f;

            // 回転が下がったらさらに開ける（P制御）
            float correction = Mathf.Clamp(rpmDiff * 0.005f, 0f, 0.2f);

            effectiveThrottle = baseIdleThrottle + correction;
        }
        return effectiveThrottle;
    }
}
