using UnityEngine;

/// <summary>
/// �G���W���̏o�́i�g���N�E��]���j���A�M�A���ʂ��Ďԗ֑��ɓ`����
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

    float[] gearRatios;// 1���`5��
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
    /// �M�A��؂�ւ���i�M�A�ԍ��̑Ó����͌Ăяo�����ŕۏ؂���j
    /// </summary>
    /// <param name="newGear">�M�A���i0��N,-1��R�j</param>
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
        /// �G���W���ƃz�C�[���̉�]������߂�g���N���v�Z����
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
        //��]�̑��x��rad/s�ɕϊ�
        float engineOmega = engineRPM * Mathf.PI * 2f / 60f; // rad/s
        float wheelOmega = wheelRPM * Mathf.PI * 2f / 60f;   // rad/s

        if (currentState == GearState.Neutral)
        {
            //�j���[�g�����ł������͒�R������̂ŏ����������׃g���N��Ԃ�
            float resistanceCoeff = 0.002f; // �����p�W��
            return -engineOmega * resistanceCoeff;
        }        
        // �M�A��i���̒l�j
        float ratio = currentGearRatio * finalDriveRatio;
        // ��]�������߂�
        float omegaDifference = engineOmega - (wheelOmega * ratio);
        // �N���b�`�ڑ����ɉ����ĉ�]������I�Ɍ��������A���炩�Ȗ߂�g���N�����
        float smoothedDifference = Mathf.Lerp(0f, omegaDifference, clutchEngagement);
        float angularAccel = smoothedDifference / deltaTime;

        float returnTorque = -flywheelInertia * angularAccel;

        return returnTorque;
    }
}
