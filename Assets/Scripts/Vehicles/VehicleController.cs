using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] VehicleInputHandler inputHandler;
    [SerializeField] CarSpec carSpec;
    //タイヤ1つ1つのクラス
    //見やすくするため配列でまとめない
    [SerializeField] WheelController wheelFL;
    [SerializeField] WheelController wheelFR;
    [SerializeField] WheelController wheelRL;
    [SerializeField] WheelController wheelRR;
    WheelController[] wheels;
    EngineModel engine;
    TransmissionModel transmission;
    ClutchModel clutch;

    int drivenWheelCount;
    float acceleratorAxis;
    float clutchAxis;
    float brakeAxis;
    float steerAxis;


    void Awake()
    {
        engine = new EngineModel(carSpec);
        transmission = new TransmissionModel(carSpec);
        clutch = new ClutchModel(inputHandler);
        inputHandler.OnGearPressed += transmission.SetGear;
        wheels = new WheelController[]{
            wheelFR,
            wheelFL,
            wheelRL,
            wheelRR
        };
        foreach (var wheel in wheels)
        {
            if(wheel.IsDrivenWheel)
            {
                drivenWheelCount++;
            }
        }
    }
    void OnDestroy()
    {
        inputHandler.OnGearPressed -= transmission.SetGear;
    }
    void Start()
    {
        transmission.SetGear(0);
        InitWheels(carSpec.IsFrontDriven);
    }

    // Update is called once per frame
    void Update()
    {
        acceleratorAxis = inputHandler.AccelerationAxis;
        clutchAxis = inputHandler.ClutchAxis;
        brakeAxis = inputHandler.BrakeAxis;
        steerAxis = inputHandler.SteerAxis;
    }
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float averageDrivenWheelRpm = 0f;
        float summedRpm = 0f;
        foreach (WheelController wheel in wheels)
        {
            if (wheel.IsDrivenWheel)
            {
                summedRpm += wheel.WheelRPM;
            }
        }
        averageDrivenWheelRpm = summedRpm / drivenWheelCount;

        float engineTorque = engine.OutputTorque;
        float returnTorque = 
        transmission.CalculateReturnTorque(
            engine.CurrentRPM,
            averageDrivenWheelRpm,
            clutchAxis,
            dt
        );
        float outputTorque = 
        transmission.CalculateDrivenTorque(
            engineTorque, 
            clutchAxis,
            drivenWheelCount
        );

        engine.UpdateEngine(
            acceleratorAxis, 
            returnTorque, 
            dt
        );
        float drivenTorque = outputTorque;
        foreach ( WheelController wheel in wheels )
        {
            wheel.ApplyInput(drivenTorque, brakeAxis, steerAxis);
        }
    }

    void InitWheels(bool isFrontDriven)
    {
        wheelFL.Init(carSpec, true, isFrontDriven);
        wheelFR.Init(carSpec, true, isFrontDriven);
        wheelRL.Init(carSpec, false, !isFrontDriven);
        wheelRR.Init(carSpec, false, !isFrontDriven);
    }
}
