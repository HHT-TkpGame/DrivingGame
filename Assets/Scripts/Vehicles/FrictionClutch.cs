using UnityEngine;

public class FrictionClutch
{
    public float Engagement {  get; private set; }
    //クラッチの接続状態(1が接続, 0が切断)
    //InputHandlerからの入力は押されていない時が0を返す
    //本来は入力がない時に接続状態が1になるべきなので値を反対にする
    public void SetEngagement(float clutchInput)
    {
        float raw = 1f - clutchInput;
        float target = curve.Evaluate(raw);

        // FixedUpdateから呼ばれる前提でTime.fixedDeltaTimeを使う
        float dt = Time.fixedDeltaTime;

        float maxDelta = engagementRiseRate * dt;

        Engagement = Mathf.MoveTowards(Engagement, target, maxDelta);
        //Debug.Log("Engagement" + Engagement);
    }

    AnimationCurve curve;
    float maxTorqueCap; // Nm
    float maxCap { get { return maxTorqueCap * Engagement; } }

    //調整値：回転差の吸収を強める（大きいほどロックが早いが、強すぎると振動の原因）
    //単位は「1.0 = 1フレームで回転差を消す」の目安として使う係数。
    float slipResponse = 1.0f;

    //調整値：Engagementの追従速度 [1/s]
    float engagementRiseRate = 10f;

    public FrictionClutch(
        AnimationCurve curve,
        float maxTorqueCap
    )
    {
        this.curve = curve;
        this.maxTorqueCap = maxTorqueCap;
    }

    /// <summary>
    /// クラッチを通過するトルクを計算する（エンジンとトランスミッション間で相互作用させる）
    /// </summary>
    /// <param name="engineTorque">エンジンが出力したいトルク</param>
    /// <param name="loadTorque">トランスミッション側からエンジンに戻る負荷トルク</param>
    /// <param name="engineOmega">エンジン角速度</param>
    /// <param name="transInputOmega">トランスミッション入力軸角速度</param>
    /// <returns>実際にクラッチを通過するトルク</returns>
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
        if (Engagement <= 0.01f)
        {
            return 0f;
        }

        // ここでは「回転差を減らす向きの連続トルク」を作り、
        // 最大伝達トルク(maxCap)でクランプすることで安定化させる。
        float omegaDiff = engineOmega - transInputOmega;
        float inertiaSum = engineInertia + vehicleInertia;

        //エンジン側/負荷側の釣り合いから必要な伝達トルクを推定
        float requiredTorque =
            (engineTorque * vehicleInertia + loadTorque * engineInertia) / inertiaSum;

        //回転差を吸収する
        //2つの慣性がクラッチで結合しているとみなし、等価慣性で回転差を減らす方向のトルクを計算する。
        //1フレームでomegaDiffを打ち消す量を基準にしているので、deltaTimeでスケールする。
        float effectiveInertia = (engineInertia * vehicleInertia) / inertiaSum;
        float viscousGain = effectiveInertia * slipResponse; // Nm / (rad/s) 相当（目安）
        float slipTorque = omegaDiff * viscousGain;

        //目的の伝達トルク
        float desiredTorque = requiredTorque + slipTorque;
        return Mathf.Clamp(desiredTorque, -maxCap, maxCap);
    }
}
