using UnityEngine;

public class FrictionClutch
{
    AnimationCurve curve;
    float maxStaticFriction;//Nm
    float maxDynamicFriction;//Nm
    float slipThreshold;//rad/s
    public FrictionClutch(
        AnimationCurve curve,
        float maxStaticFriction,
        float maxDynamicFriction,
        float slipThreshold
    )
    {
        this.curve = curve;
        this.maxStaticFriction = maxStaticFriction;
        this.maxDynamicFriction = maxDynamicFriction;
        this.slipThreshold = slipThreshold;
    }
    /// <summary>
    /// クラッチが発生させるトルク（符号付き）
    /// engineOmega - wheelOmega は呼び出し側で計算
    /// </summary>
    /// <param name="deltaOmega">回転差(rad/s)</param>
    /// <param name="input">クラッチ入力 0～1</param>
    /// <returns>伝達トルク[Nm]</returns>
    public float GetTransmittedTorque(float deltaOmega, float input)
    {
        // クラッチ未接触
        if (input <= 0f) return 0f;

        float k = curve.Evaluate(input);

        float absDiff = Mathf.Abs(deltaOmega);
        float sign = Mathf.Sign(deltaOmega);

        if (absDiff < slipThreshold)
        {
            // 同調させる方向に最大静摩擦
            // → 本当に止められるかは呼び出し側の慣性次第
            return -sign * k * maxStaticFriction;
        }
        float slipTorque = Mathf.Lerp(k * maxDynamicFriction, k * maxStaticFriction, slipThreshold / absDiff);

        return -sign * slipTorque;
    }
}
