using UnityEngine;

/// <summary>
/// エンジンの出力（トルク・回転数）を、ギア比を通して車輪側に伝える
/// </summary>
public class TransmissionModel: ITransmissionModel
{
    enum GearState
    {
        Neutral,
        Forward,
        Reverse
    }
    CarSpec spec;
    float currentGearRatio;
    /// <summary>
    /// デフ比とギア比を乗算した倍率
    /// </summary>
    public float CurrentRatio { get { return currentGearRatio * finalDriveRatio; }}
    public int CurrentGear {  get; private set; }

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
                break;
            case 0:
                currentGearRatio = 0;
                break;
            default: 
                currentGearRatio = gearRatios[CurrentGear-1];
                break;
        }
    }
    // MTは速度上限など無いので空実装
    public void Tick(float speedKph, float deltaTime) { }

    // MTは常に1
    public float GetDriveTorqueScale(float speedKph) => 1f;
}
