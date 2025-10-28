using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �G���W���P�̃N���X�i��]���E�g���N�v�Z�j
/// �ԑ���M�A�ɂ͈ˑ������AThrottle���͂ƊO���g���N�����]���E�o�̓g���N���v�Z
/// </summary>
public class EngineModel
{
    enum EngineState
    {
        Stalled,
        Running
    }
    EngineState currentState = EngineState.Running;
    Dictionary<EngineState, Action<float, float, float>> updateMethods;

    [Header("�Ԏ�ɂ�钲���l")]
    float maxRPM;           // ���b�h���C��
    float idleRPM;          // �A�C�h�����O��]��

    // RPM�ɑ΂���g���N�Ȑ���Inspector����Č�����
    AnimationCurve torqueCurve;
    float flywheelInertia;  // �������[�����g
    float pumpingLossFactor;// �z�r�C�E��@����

    [Header("���")]
    float currentRPM = 1000f;       // ���݂̉�]��

    /// <summary>
    /// ���݂̃G���W���g���N
    /// </summary>
    public float OutputTorque { get; private set; }

    /// <summary>
    /// ���݂̉�]��
    /// </summary>
    public float CurrentRPM => currentRPM;
    CarSpec spec;

    public EngineModel(CarSpec spec)
    {
        this.spec = spec;
        updateMethods = new Dictionary<EngineState, Action<float, float, float>>
        {
            { EngineState.Stalled, UpdateStalled },
            { EngineState.Running, UpdateRunning }
        };
        maxRPM = spec.MaxRPM;
        idleRPM = spec.IdleRPM;
        torqueCurve = spec.TorqueCurve;
        flywheelInertia = spec.FlywheelInertia;
        pumpingLossFactor = spec.PumpingLossFactor;
    }
    /// <summary>
    /// �G���W���X�V
    /// </summary>
    /// <param name="throttle">0�`1</param>
    /// <param name="externalTorque">�O������`���g���N�i�N���b�`��^�C������̖߂�j</param>
    /// <param name="deltaTime">���ԍ���</param>
    public void UpdateEngine(float throttle, float externalTorque, float deltaTime)
    {
        updateMethods[currentState].Invoke(throttle, externalTorque, deltaTime);
    }
    void UpdateRunning(float throttle, float externalTorque, float deltaTime)
    {
        //��R�Ȃǂ��l�����Ȃ��ꍇ�̃g���N
        float engineTorque = torqueCurve.Evaluate(currentRPM) * throttle;
        //�z�r�C��R�Ԃ�ň������g���N
        float lossTorque = engineTorque * pumpingLossFactor;
        //���ۂ̃g���N�l
        float netTorque = engineTorque + externalTorque - lossTorque;

        //�g���N����p�����x�����߂�i�������[�����g�ɔ����j
        //�P�ʁFrad/s^2�i���W�A�����b���b�j
        //�� �g���N���傫�� or �������������قǉ�]�������ω�����
        float angularAcceleration = netTorque / flywheelInertia;

        //�p�����x * �o�ߎ��ԂŊp���x�̕ω��ʂ����߂�
        //�P�ʂ�rad/s�i���W�A�����b�j
        float angularVelocityChange = angularAcceleration * deltaTime;

        //�p���x�̕ω���(rad/s)����]��(RPM)�̕ω��ʂɕϊ�
        //�p���x�̒P�ʂ͕b������A��]���͕�������Ȃ̂Ŏ��ԒP�ʂ����킹��i�~60�j
        //�܂�1��] = 2��[rad] �Ȃ̂Ŋp�x�P�ʂ���]�ɕϊ��i��2�΁j
        //����āA1 [rad/s] = 60 / (2��) [RPM]
        float rpmChange = angularVelocityChange * (60f / (2f * Mathf.PI));

        // ���݂̉�]���ɉ��Z���čX�V
        currentRPM += rpmChange;

        currentRPM = Mathf.Clamp(currentRPM, 0, maxRPM);

        OutputTorque = netTorque;

        if (currentRPM < idleRPM * 0.7f)
        {
            currentState = EngineState.Stalled;
        }
    }
    void UpdateStalled(float throttle, float externalTorque, float deltaTime)
    {
        Mathf.MoveTowards(currentRPM, 0, 2000f * deltaTime);
    }
}
