using UnityEngine;

public class ATClutch : IClutchModel
{
    public float Engagement { get; private set; }

    // 無制限に近い値（数値爆発を避けるため無限大は使わない）
    readonly float maxTorqueCap;
    readonly float slipResponse;

    public ATClutch(float maxTorqueCap = 999999f, float slipResponse = 1.0f)
    {
        this.maxTorqueCap = maxTorqueCap;
        this.slipResponse = slipResponse;
    }

    public void SetEngagement(float clutchInput, bool isPlaying)
    {
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
        float inertiaSum = engineInertia + vehicleInertia;

        float requiredTorque =
            (engineTorque * vehicleInertia + loadTorque * engineInertia) / inertiaSum;

        float effectiveInertia = (engineInertia * vehicleInertia) / inertiaSum;
        float viscousGain = effectiveInertia * slipResponse;
        float slipTorque = omegaDiff * viscousGain;

        float desiredTorque = requiredTorque + slipTorque;
        return Mathf.Clamp(desiredTorque, -maxTorqueCap, maxTorqueCap);
    }
}
