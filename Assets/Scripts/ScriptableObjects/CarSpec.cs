using UnityEngine;

[CreateAssetMenu(fileName = "NewCarSpecs", menuName = "CarSpecs")]
public class CarSpec : ScriptableObject
{
    [Header("EngineSpec")]
    [SerializeField] float maxRPM;
    [SerializeField] float idleRPM;
    [SerializeField] float flywheelInertia;
    [SerializeField] AnimationCurve torqueCurve;
    [SerializeField] float pumpingLossFactor;

    public float MaxRPM => maxRPM;
    public float IdleRPM => idleRPM;
    public float FlywheelInertia => flywheelInertia;
    public AnimationCurve TorqueCurve => torqueCurve;
    public float PumpingLossFactor => pumpingLossFactor;

    [Header("TransmissionSpec")]

    [SerializeField, Tooltip("�f�t��")]
    float finalDriveRatio;//���4���炢
    //�z��̌���1������ő�M�A���܂�
    [SerializeField, Tooltip("�O�i�p�M�A��")] 
    float[] gearRatios;
    //state�ŊǗ�����̂Ō�ނ�ʂɒ�`����
    [SerializeField, Tooltip("��ޗp�M�A��")] 
    float reverseRatio;
    public float FinalDriveRatio => finalDriveRatio;
    public float[] GearRatios => gearRatios;
    public float ReverseRatio => reverseRatio;


    [Header("WheelSpec")]

    [SerializeField, Tooltip("�쓮�����@true����FF,false����FR")]
    bool isFrontDriven;
    [SerializeField, Tooltip("�t�����g1�ւ�����̍ő�u���[�L�g���N [Nm]")]
    float maxBrakeTorqueFront;//���N�T�XLFA���Ƒ��1800
    [SerializeField, Tooltip("���A1�ւ�����̍ő�u���[�L�g���N [Nm]")]
    float maxBrakeTorqueRear;//���N�T�XLFA���Ƒ��900
    public bool IsFrontDriven => isFrontDriven;
    public float MaxBrakeTorqueFront => maxBrakeTorqueFront;
    public float MaxBrakeTorqueRear => maxBrakeTorqueRear;
}

