using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �G���W���P�̃N���X�i��]���E�g���N�v�Z�j
/// �ԑ���M�A�ɂ͈ˑ������AThrottle���͂ƊO���g���N�����]���E�o�̓g���N���v�Z
/// </summary>
public class TmpEngineModel
{
    enum TmpEngineState
    {
        Stalled,
        Running
    }
    TmpEngineState currentState = TmpEngineState.Running;
    Dictionary<TmpEngineState, Action<float, float, float>> updateMethods;

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
    float engineAngularVelocity;

    public TmpEngineModel(CarSpec spec)
    {
        maxRPM = spec.MaxRPM;
        idleRPM = spec.IdleRPM;
        torqueCurve = spec.TorqueCurve;
        flywheelInertia = spec.FlywheelInertia;
        pumpingLossFactor = spec.PumpingLossFactor;

        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
    }
    
    /// <summary>
    /// �O���g���N���l�����Ȃ����g���N�v�Z
    /// </summary>
    /// <param name="throttle"></param>
    public void UpdateTorque(float throttle)
    {
        if (currentState == TmpEngineState.Stalled)
        {
            OutputTorque = 0f;
            return;
        }
        throttle += UpdateIdleControl(throttle);
        // �X���b�g���ɉ��������_�g���N
        float baseTorque = torqueCurve.Evaluate(currentRPM) * throttle;
        
        // ����������������
        float lossTorque = baseTorque * pumpingLossFactor;

        // �G���W���̏��o�̓g���N
        OutputTorque = baseTorque - lossTorque;
    }

    /// <summary>
    /// �g�����X�~�b�V�����̔��̓g���N��K�p���A��]�����X�V����
    /// </summary>
    /// <param name="externalTorque"></param>
    /// <param name="deltaTime"></param>
    public void ApplyExternalTorque(float externalTorque, float deltaTime)
    {
        if (currentState == TmpEngineState.Stalled)
        {
            currentRPM = Mathf.MoveTowards(currentRPM, 0f, 2000f * deltaTime);
            return;
        }
        engineAngularVelocity = currentRPM * 2f * Mathf.PI / 60f;
        // �o�̓g���N�ƊO���g���N�i���́j�����킹�Ċ����������v�Z
        float netTorque = OutputTorque - externalTorque;
        float viscousLoss = 0.000003f * currentRPM * currentRPM; // ��C��R�I�ȑ���
        float frictionLoss = 6f * (currentRPM / maxRPM);      // �@�B���C
        float mechanicalLoss = viscousLoss + frictionLoss;
        

        //float mechanicalLoss = 0.02f * currentRPM;
        netTorque -= mechanicalLoss;
        // �p�����x(rad/s^2)
        float angularAcceleration = netTorque / flywheelInertia;

        // �p���x�X�V
        engineAngularVelocity += angularAcceleration * deltaTime;

        //Debug.Log($"External:{externalTorque},\n RPM:{currentRPM},\n Torque:{OutputTorque},\n MechaLos:{mechanicalLoss},\n NetTorque:{netTorque},\n AngularVelocity:{engineAngularVelocity}");

        // RPM�X�V
        currentRPM = Mathf.Clamp(
            engineAngularVelocity * 60f / (2f * Mathf.PI),
            0f,
            maxRPM
        );

        // �G���W����~����
        if (currentRPM < idleRPM * 0.7f &&
             netTorque < 0)
        {
            currentState = TmpEngineState.Stalled;
        }
    }
    /// <summary>
    /// �A�C�h������
    /// </summary>
    /// <param name="throttle"></param>
    /// <returns>�X���b�g���␳�l</returns>
    float UpdateIdleControl(float throttle)
    {
        float effectiveThrottle = 0f;
        if (throttle < 0.1f)
        {
            float rpmDiff = idleRPM - currentRPM;

            // ��]�����������炳��ɊJ����iP����j
            float correction = Mathf.Clamp(rpmDiff * 0.005f, 0f, 0.2f);

            effectiveThrottle += correction;
        }
        return effectiveThrottle;
    }
}
