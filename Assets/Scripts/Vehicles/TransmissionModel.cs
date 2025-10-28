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
    public float CurrentGearRatio => currentGearRatio * finalDriveRatio;
    GearState currentState = GearState.Neutral;

    float[] gearRatios;// 1速～5速
    float reverseGearRatios;
    float finalDriveRatio;
    float flywheelInertia;

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
        flywheelInertia = spec.FlywheelInertia;
    }
    /// <summary>
    /// ギアを切り替える（ギア番号の妥当性は呼び出し側で保証する）
    /// </summary>
    /// <param name="newGear">ギア数（0はN,-1はR）</param>
    public void SetGear(int newGear)
    {
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
        if(currentState == GearState.Neutral) { return 0f; }
        return engineTorque * CurrentGearRatio * clutchEngagement / drivenWheelCount;
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
        float clutchEngagement,
        float deltaTime
    ){
        //回転の速度をrad/sに変換
        float engineOmega = engineRPM * Mathf.PI * 2f / 60f; // rad/s
        float wheelOmega = wheelRPM * Mathf.PI * 2f / 60f;   // rad/s

        if (currentState == GearState.Neutral)
        {
            //ニュートラルでも多少は抵抗があるので少しだけ負荷トルクを返す
            float resistanceCoeff = 0.002f; // 調整用係数
            return -engineOmega * resistanceCoeff;
        }        
        // ギア比（正の値）
        float ratio = currentGearRatio * finalDriveRatio;
        // 回転差を求める
        float omegaDifference = engineOmega - (wheelOmega * ratio);
        // クラッチ接続率に応じて回転差を比例的に減衰させ、滑らかな戻りトルクを作る
        float smoothedDifference = Mathf.Lerp(0f, omegaDifference, clutchEngagement);
        float angularAccel = smoothedDifference / deltaTime;

        float returnTorque = -flywheelInertia * angularAccel;

        return returnTorque;
    }
}
