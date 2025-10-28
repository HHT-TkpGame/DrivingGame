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

    [SerializeField, Tooltip("デフ比")]
    float finalDriveRatio;//大体4ぐらい
    //配列の個数は1速から最大ギア数まで
    [SerializeField, Tooltip("前進用ギア比")] 
    float[] gearRatios;
    //stateで管理するので後退を別に定義する
    [SerializeField, Tooltip("後退用ギア比")] 
    float reverseRatio;
    public float FinalDriveRatio => finalDriveRatio;
    public float[] GearRatios => gearRatios;
    public float ReverseRatio => reverseRatio;


    [Header("WheelSpec")]

    [SerializeField, Tooltip("駆動方式　trueだとFF,falseだとFR")]
    bool isFrontDriven;
    [SerializeField, Tooltip("フロント1輪あたりの最大ブレーキトルク [Nm]")]
    float maxBrakeTorqueFront;//レクサスLFAだと大体1800
    [SerializeField, Tooltip("リア1輪あたりの最大ブレーキトルク [Nm]")]
    float maxBrakeTorqueRear;//レクサスLFAだと大体900
    public bool IsFrontDriven => isFrontDriven;
    public float MaxBrakeTorqueFront => maxBrakeTorqueFront;
    public float MaxBrakeTorqueRear => maxBrakeTorqueRear;
}

