using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] StatusUITemp tempUI;//デバッグ用　後々削除
    [SerializeField] VehicleInputHandler inputHandler;
    [SerializeField] CarSpec carSpec;
    [SerializeField] ClutchCurve clutchCurve;
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
    Rigidbody rb;
    public float SpeedKPH
    {
        get
        {
            float mps = Mathf.Abs(Vector3.Dot(rb.linearVelocity, transform.forward));
            return mps * 3.6f;
        }
    }

    int drivenWheelCount;
    float acceleratorAxis;
    float clutchAxis;
    float brakeAxis;
    float steerAxis;


    void Awake()
    {
        engine = new EngineModel(carSpec);
        transmission = new TransmissionModel(carSpec);
        clutch = new ClutchModel(inputHandler, clutchCurve.Curve);
        inputHandler.OnGearPressed += transmission.SetGear;
        wheels = new WheelController[]{
            wheelFR,
            wheelFL,
            wheelRL,
            wheelRR
        };
        tempUI.Initialize(
            clutch,
            transmission,
            engine,
            this,
            carSpec.MaxRPM
        );
        rb = GetComponent<Rigidbody>();
    }

    void OnDestroy()
    {
        inputHandler.OnGearPressed -= transmission.SetGear;
    }
    void Start()
    {
        transmission.SetGear(0);
        InitWheels(carSpec.IsFrontDriven, carSpec.WheelRadius);
        foreach (var wheel in wheels)
        {
            if (wheel.IsDrivenWheel)
            {
                drivenWheelCount++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        acceleratorAxis = inputHandler.AccelerationAxis;
        clutchAxis = clutch.Engagement;
        brakeAxis = inputHandler.BrakeAxis;
        steerAxis = inputHandler.SteerAxis;
        Debug.Log($"accel{acceleratorAxis}\nclutch{clutchAxis}\nbrake{brakeAxis}\nsteer{steerAxis}");
    }
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float averageDrivenWheelRpm = GetAverageDrivenWheelRpm();
        UpdateDrivetrain(averageDrivenWheelRpm, dt);
        //簡易的な空気抵抗再現
        float speed = rb.linearVelocity.magnitude;
        float airDrag = 0.4f * speed * speed;
        Vector3 dragForce = -rb.linearVelocity.normalized * airDrag;
        rb.AddForce(dragForce);
    }

    float GetAverageDrivenWheelRpm()
    {
        float sum = 0f;
        foreach (var wheel in wheels)
        {
            if (wheel.IsDrivenWheel)
            {
                sum += wheel.WheelRPM;
            }
        }
        return sum / drivenWheelCount;
    }
    void UpdateDrivetrain(float avgDrivenWheelRpm, float deltaTime)
    {
        engine.UpdateTorque(acceleratorAxis);
        float returnTorque = transmission.CalculateReturnTorque(
            engine.CurrentRPM,
            avgDrivenWheelRpm,
            clutchAxis
        );
        engine.ApplyExternalTorque(returnTorque, deltaTime);
        float drivenTorque =
        transmission.CalculateDrivenTorque(
            engine.OutputTorque,
            clutchAxis,
            drivenWheelCount
        );
        foreach (WheelController wheel in wheels)
        {
            wheel.ApplyInput(drivenTorque, brakeAxis, steerAxis);
        }
    }

    void InitWheels(bool isFrontDriven, float wheelRadius)
    {
        wheelFL.Init(carSpec, true, isFrontDriven, wheelRadius);
        wheelFR.Init(carSpec, true, isFrontDriven, wheelRadius);
        wheelRL.Init(carSpec, false, !isFrontDriven, wheelRadius);
        wheelRR.Init(carSpec, false, !isFrontDriven, wheelRadius);
    }
}
