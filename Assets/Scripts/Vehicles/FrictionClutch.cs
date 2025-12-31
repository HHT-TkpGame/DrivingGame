using UnityEngine;

public class FrictionClutch
{
    public float Engagement {  get; private set; }
    //クラッチの接続状態(1が接続, 0が切断)
    //InputHandlerからの入力は押されていない時が0を返す
    //本来は入力がない時に接続状態が1になるべきなので値を反対にする
    public void SetEngagement(float clutchInput)
    {
        float raw = 1 - clutchInput;
        Engagement = curve.Evaluate(raw);
    }

    AnimationCurve curve;
    float maxTorqueCap;//Nm　900ぐらい
    float maxCap { get { return maxTorqueCap * Engagement; } }
    public FrictionClutch(
        AnimationCurve curve,
        float maxTorqueCap
    ){
        this.curve = curve;
        this.maxTorqueCap = maxTorqueCap;
    }

    /// <summary>
    /// クラッチを通過するトルクを計算する（エンジンとトランスミッション間で相互作用させる）
    /// </summary>
    /// <param name="engineTorque">エンジンが出力したいトルク</param>
    /// <param name="loadTorque">トランスミッション側からエンジンに戻る負荷トルク　常にengineOmegaの符号と逆になる</param>
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

        float threshold = 5f * deltaTime;
        float omegaDiff = engineOmega - transInputOmega;
        float requiredTorque = 
            (engineTorque * vehicleInertia + loadTorque * engineInertia)
            / (engineInertia + vehicleInertia); 

        //許容トルクを超えたら上限を返す
        if(Mathf.Abs(requiredTorque) > maxCap)
        {
            Debug.Log("トルクオーバー");
            return Mathf.Sign(requiredTorque) * maxCap;
        }
        if(Mathf.Abs(omegaDiff) < threshold)
        {
            Debug.Log("許容回転差");
            return requiredTorque;
        }
        Debug.Log("Slip");
        return Mathf.Sign(omegaDiff) * maxCap;
    }
}
