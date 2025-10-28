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

    public EngineModel(CarSpec spec)
    {
        this.spec = spec;
        updateMethods = new Dictionary<EngineState, Action<float, float, float>>
        {
            { EngineState.Stalled, UpdateStalled },
            { EngineState.Running, UpdateRunning }
        };
        maxRPM = spec.MaxRPM;
        idleRPM = spec.IdleRPM;
        torqueCurve = spec.TorqueCurve;
        flywheelInertia = spec.FlywheelInertia;
        pumpingLossFactor = spec.PumpingLossFactor;
    }
    /// <summary>
    /// エンジン更新
    /// </summary>
    /// <param name="throttle">0～1</param>
    /// <param name="externalTorque">外部から伝わるトルク（クラッチやタイヤからの戻り）</param>
    /// <param name="deltaTime">時間差分</param>
    public void UpdateEngine(float throttle, float externalTorque, float deltaTime)
    {
        updateMethods[currentState].Invoke(throttle, externalTorque, deltaTime);
    }
    void UpdateRunning(float throttle, float externalTorque, float deltaTime)
    {
        //抵抗などを考慮しない場合のトルク
        float engineTorque = torqueCurve.Evaluate(currentRPM) * throttle;
        //吸排気抵抗ぶんで引かれるトルク
        float lossTorque = engineTorque * pumpingLossFactor;
        //実際のトルク値
        float netTorque = engineTorque + externalTorque - lossTorque;

        //トルクから角加速度を求める（慣性モーメントに反比例）
        //単位：rad/s^2（ラジアン毎秒毎秒）
        //→ トルクが大きい or 慣性が小さいほど回転が速く変化する
        float angularAcceleration = netTorque / flywheelInertia;

        //角加速度 * 経過時間で角速度の変化量を求める
        //単位はrad/s（ラジアン毎秒）
        float angularVelocityChange = angularAcceleration * deltaTime;

        //角速度の変化量(rad/s)を回転数(RPM)の変化量に変換
        //角速度の単位は秒あたり、回転数は分あたりなので時間単位を合わせる（×60）
        //また1回転 = 2π[rad] なので角度単位を回転に変換（÷2π）
        //よって、1 [rad/s] = 60 / (2π) [RPM]
        float rpmChange = angularVelocityChange * (60f / (2f * Mathf.PI));

        // 現在の回転数に加算して更新
        currentRPM += rpmChange;

        currentRPM = Mathf.Clamp(currentRPM, 0, maxRPM);

        OutputTorque = netTorque;

        if (currentRPM < idleRPM * 0.7f)
        {
            currentState = EngineState.Stalled;
        }
    }
    void UpdateStalled(float throttle, float externalTorque, float deltaTime)
    {
        Mathf.MoveTowards(currentRPM, 0, 2000f * deltaTime);
    }
}
