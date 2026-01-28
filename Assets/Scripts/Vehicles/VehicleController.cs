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
    FrictionClutch clutch;
    Rigidbody rb;
    float vehicleInertiaSmoothed = 0.05f;

    public float SpeedKPH
    {
        get
        {
            float mps = Mathf.Abs(Vector3.Dot(rb.linearVelocity, transform.forward));
            return mps * 3.6f;
        }
    }

    int drivenWheelCount;

    void Awake()
    {
        engine = new EngineModel(carSpec);
        transmission = new TransmissionModel(carSpec);
        clutch = new FrictionClutch(
            clutchCurve.Curve,
            800f
        );
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
    }
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        clutch.SetEngagement(inputHandler.ClutchAxis);
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
        engine.UpdateTorque(inputHandler.AccelerationAxis);
        //rad/sに変換し、回転差を計算
        float engineOmega = engine.CurrentRPM * Mathf.PI * 2 / 60f;
        float transOmega = avgDrivenWheelRpm * Mathf.PI * 2 / 60f * transmission.CurrentRatio;
        float absRatio = Mathf.Abs(transmission.CurrentRatio);
        float vehicleInertia = 0.05f;
        float smoothRate = 10f; // 追従速度[1/s]
        vehicleInertiaSmoothed = Mathf.Lerp(vehicleInertiaSmoothed, vehicleInertia, 1f - Mathf.Exp(-smoothRate * deltaTime));
        vehicleInertia = vehicleInertiaSmoothed;
        if (absRatio > 0.01f
            && clutch.Engagement > 0.01f)
        {
            float v = (rb.mass * (carSpec.WheelRadius * carSpec.WheelRadius))/drivenWheelCount;
            vehicleInertia = v / (absRatio * absRatio);
        }

        //loadTorqueの計算
        float speed = rb.linearVelocity.magnitude;
        float rollingResistance = 0.015f * rb.mass * 9.81f;

        float loadTorque = (rollingResistance * carSpec.WheelRadius) / (absRatio == 0 ? 1 : absRatio);
        // 車輪側(=transOmega)も参照しつつ、0近傍はスムーズに0へフェードさせる
        float threshold = 0.1f;

        float signOmega = Mathf.Abs(transOmega) > threshold ? transOmega : engineOmega;

        // 0近傍で線形に減衰させる
        float fade = Mathf.InverseLerp(0f, threshold, Mathf.Abs(signOmega));
        if (fade <= 0.0001f)
        {
            loadTorque = 0f;
        }
        else
        {
            loadTorque *= -Mathf.Sign(signOmega) * fade;
        }

        float clutchTorque = transmission.CurrentRatio == 0f ? 0f :
            clutch.ComputeTorque(
                engine.OutputTorque,
                engineOmega,
                carSpec.FlywheelInertia,
                loadTorque,
                transOmega,
                vehicleInertia,
                deltaTime
            );
        //Debug.Log(clutchTorque);
        engine.ApplyExternalTorque(-clutchTorque, deltaTime);

        float drivenTorque = clutchTorque * transmission.CurrentRatio / drivenWheelCount;//駆動輪の数で割る
        foreach (WheelController wheel in wheels)
        {
            wheel.ApplyInput(drivenTorque, inputHandler.BrakeAxis, inputHandler.SteerAxis);
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
