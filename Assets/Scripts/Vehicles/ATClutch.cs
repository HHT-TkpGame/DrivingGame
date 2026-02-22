using UnityEngine;

public class ATClutch : IClutchModel
{
    public float Engagement { get; private set; }

    // 低速クリープで伝える最大トルク（Nm）
    readonly float creepTorqueCap;

    // 回転差(rad/s)に対して上限トルクを増やす係数（Nm / (rad/s)）
    // 大きいほど「回転差があると強く押す」= トルク増幅っぽくなる
    readonly float slipToTorqueGain;

    // 入力軸の回転(rad/s)がこの値を超えるとロックアップ寄りになる
    readonly float lockupTransOmega;

    // ロックアップ時の最大伝達トルク（Nm）
    readonly float lockupTorqueCap;

    // ロックアップ時に回転差を潰す強さ（大きいほど直結に近い）
    readonly float lockupSlipResponse;

    // 低速時の回転差を潰す弱い粘性（直結にしないため小さめ）
    readonly float lowSlipResponse;

    /// <summary>
    /// デフォルト値は「とりあえずエンストしにくく、ATっぽく発進できる」寄りの値
    /// 車重やエンジントルクに合わせて調整してください
    /// </summary>
    public ATClutch(
        float creepTorqueCap = 80f,
        float slipToTorqueGain = 0.25f,
        float lockupTransOmega = 80f,     // rad/s ≒ 764rpm
        float lockupTorqueCap = 2000f,
        float lockupSlipResponse = 1.0f,
        float lowSlipResponse = 0.15f
    )
    {
        this.creepTorqueCap = Mathf.Max(0f, creepTorqueCap);
        this.slipToTorqueGain = Mathf.Max(0f, slipToTorqueGain);
        this.lockupTransOmega = Mathf.Max(0f, lockupTransOmega);
        this.lockupTorqueCap = Mathf.Max(0f, lockupTorqueCap);
        this.lockupSlipResponse = Mathf.Max(0f, lockupSlipResponse);
        this.lowSlipResponse = Mathf.Max(0f, lowSlipResponse);
    }

    public void SetEngagement(float clutchInput, bool isPlaying)
    {
        // ATなので入力クラッチは使わず、プレイ中は常時ON
        Engagement = isPlaying ? 1f : 0f;
    }

    public float ComputeTorque(
        float engineTorque,
        float engineOmega,
        float engineInertia,
        float loadTorque,
        float transInputOmega,
        float vehicleInertia,
        float deltaTime
    )
    {
        if (Engagement <= 0.01f) return 0f;

        float omegaDiff = engineOmega - transInputOmega;
        float absDiff = Mathf.Abs(omegaDiff);

        // 入力軸が回ってくるほどロックアップ寄り（0..1）
        float lockup = (lockupTransOmega <= 0.01f)
            ? 0f
            : Mathf.Clamp01(transInputOmega / lockupTransOmega);

        // 低速側：回転差があるほど上限トルクを増やす（トルクコンバータっぽさ）
        float capLow = creepTorqueCap + absDiff * slipToTorqueGain;

        // 高速側：ロックアップ（直結寄り）
        float capHigh = lockupTorqueCap;

        // 最終的な伝達上限
        float cap = Mathf.Lerp(capLow, capHigh, lockup);

        // 「負荷を受けつつ、回転差も少しだけ潰す」狙いの目標トルク
        float inertiaSum = engineInertia + vehicleInertia;
        float requiredTorque =
            (engineTorque * vehicleInertia + loadTorque * engineInertia) / inertiaSum;

        float effectiveInertia = (engineInertia * vehicleInertia) / inertiaSum;

        // 低速は滑るので粘性弱め、高速はロックアップで強め
        float viscousGain = effectiveInertia * Mathf.Lerp(lowSlipResponse, lockupSlipResponse, lockup);
        float slipTorque = omegaDiff * viscousGain;

        float desiredTorque = requiredTorque + slipTorque;

        // 上限でクリップされる分が「滑り」になる
        return Mathf.Clamp(desiredTorque, -cap, cap);
    }
}
