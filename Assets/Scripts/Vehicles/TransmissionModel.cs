using UnityEngine;

/// <summary>
/// エンジンの出力（トルク・回転数）を、ギア比を通して車輪側に伝える
/// </summary>
public class TransmissionModel
{
    enum GearState
    {
        Neutral,
        Forward,
        Reverse
    }
    CarSpec spec;
    float currentGearRatio = 0;
    public int CurrentGear {  get; private set; }
    GearState currentState = GearState.Neutral;

    float[] gearRatios;// 1速～5速
    float reverseGearRatios;
    float finalDriveRatio;

    public TransmissionModel(CarSpec spec)
    {
        this.spec = spec;
        SetSpec(this.spec);
    }
    void SetSpec(CarSpec spec)
    {
        gearRatios = spec.GearRatios;
        reverseGearRatios = spec.ReverseRatio;
        finalDriveRatio = spec.FinalDriveRatio;
    }
    /// <summary>
    /// ギアを切り替える（ギア番号の妥当性は呼び出し側で保証する）
    /// </summary>
    /// <param name="newGear">ギア数（0はN,-1はR）</param>
    public void SetGear(int newGear)
    {
        CurrentGear = newGear;
        switch(newGear)
        {
            case -1:
                currentGearRatio = reverseGearRatios;
                currentState = GearState.Reverse;
                break;
            case 0:
                currentGearRatio = 0;
                currentState = GearState.Neutral;
                break;
            default: 
                currentGearRatio = gearRatios[newGear-1];
                currentState = GearState.Forward;
                break;
        }
    }
    public float CalculateDrivenTorque(float engineTorque, float clutchEngagement, int drivenWheelCount)
    {
        if(currentState == GearState.Neutral || clutchEngagement < 0.01) { return 0f; }
        float driveTorque = engineTorque * clutchEngagement;
        float totalRatio = currentGearRatio * finalDriveRatio;
        float wheelTorque = driveTorque * totalRatio / drivenWheelCount;
        return wheelTorque;
    }

    /// <summary>
    /// エンジンとホイールの回転数から戻りトルクを計算する
    /// </summary>
    /// <param name="engineRPM"></param>
    /// <param name="wheelRPM"></param>
    /// <param name="flywheelInertia"></param>
    /// <returns></returns>
    public float CalculateReturnTorque(
        float engineRPM,
        float wheelRPM,
        float clutchEngagement
    ){
        //Debug.Log($"e:{engineRPM},w:{wheelRPM},c:{clutchEngagement}");
        //回転の速度をrad/sに変換
        float engineOmega = engineRPM * Mathf.PI * 2f / 60f; //rad/s
        float wheelOmega = wheelRPM * Mathf.PI * 2f / 60f;   //rad/s

        if (currentState == GearState.Neutral || clutchEngagement < 0.01)
        {
            return 0f;
        }
        //ギア比から伝達側の回転速度を算出
        float ratio = currentGearRatio * finalDriveRatio;
        float omegaDifference = engineOmega - (wheelOmega / ratio);

        //float drivenInertia = 2.5f; 
        float dampingCoeff = 2f; //調整値（0.1～5くらい）
        float baseInertia = 0.5f;
        float drivenInertia = baseInertia * ratio * ratio;
        float returnTorque = drivenInertia * omegaDifference * dampingCoeff * clutchEngagement;
        return returnTorque;
    }
}
