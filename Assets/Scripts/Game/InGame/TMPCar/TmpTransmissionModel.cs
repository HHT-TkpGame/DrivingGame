using System.Xml;
using UnityEngine;

/// <summary>
/// �G���W���̏o�́i�g���N�E��]���j���A�M�A���ʂ��Ďԗ֑��ɓ`����
/// </summary>
public class TmpTransmissionModel
{
    enum TmpGearState
    {
        Neutral,
        Forward,
        Reverse
    }
    CarSpec spec;
    float currentGearRatio = 0;
    public int CurrentGear {  get; private set; }
    TmpGearState currentState = TmpGearState.Neutral;
    float flywheelInertia;

    float[] gearRatios;// 1���`5��
    float reverseGearRatios;
    float finalDriveRatio;

    public TmpTransmissionModel(CarSpec spec)
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
    /// �M�A��؂�ւ���i�M�A�ԍ��̑Ó����͌Ăяo�����ŕۏ؂���j
    /// </summary>
    /// <param name="newGear">�M�A���i0��N,-1��R�j</param>
    public void SetGear(int newGear)
    {
        CurrentGear = newGear;
        switch(newGear)
        {
            case -1:
                currentGearRatio = reverseGearRatios;
                currentState = TmpGearState.Reverse;
                break;
            case 0:
                currentGearRatio = 0;
                currentState = TmpGearState.Neutral;
                break;
            default: 
                currentGearRatio = gearRatios[newGear-1];
                currentState = TmpGearState.Forward;
                break;
        }
    }
    public float CalculateDrivenTorque(float engineTorque, float clutchEngagement, int drivenWheelCount)
    {
        if(currentState == TmpGearState.Neutral || clutchEngagement < 0.01) { return 0f; }
        float driveTorque = engineTorque * clutchEngagement;
        float totalRatio = currentGearRatio * finalDriveRatio;
        float wheelTorque = driveTorque * totalRatio / drivenWheelCount;
        return wheelTorque;
    }

    /// <summary>
    /// �G���W���ƃz�C�[���̉�]������߂�g���N���v�Z����
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

        if (currentState == TmpGearState.Neutral || clutchEngagement < 0.01)
        {
            return 0f;
        }
        // ギア比から伝達側の回転速度を算出
        float ratio = currentGearRatio * finalDriveRatio;
        float omegaDifference = engineOmega - (wheelOmega * ratio);

        float dampingCoeff = 0.5f; // 調整値（0.1～5くらいでチューニング）
        float returnTorque = flywheelInertia * omegaDifference * dampingCoeff * clutchEngagement;
        //Debug.Log(returnTorque);
        return returnTorque;
    }
}
