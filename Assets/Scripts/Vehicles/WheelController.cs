using UnityEngine;

public class WheelController : MonoBehaviour
{
    WheelCollider wheel;
    [SerializeField] Transform wheelVisual;
    Vector3 wheelPos;
    Quaternion wheelRot;
    CarSpec spec;
    bool isFrontWheel;
    bool isDrivenWheel;
    public float WheelRPM { get { return wheel.rpm; } }
    public bool IsDrivenWheel => isDrivenWheel;
    float maxSteerAngle = 30f;
    float maxBrakeTorque;
    void Awake()
    {
        wheel = GetComponent<WheelCollider>();
    }
    void Update()
    {
        SyncVisual();
    }

    /// <summary>
    /// ホイールのデータ初期化
    /// </summary>
    /// <param name="isFrontWheel">前輪かどうか</param>
    /// <param name="isDrivenWheel">駆動輪かどうか</param>
    public void Init(CarSpec spec, bool isFrontWheel, bool isDrivenWheel)
    {
        this.spec = spec;
        this.isFrontWheel = isFrontWheel;
        this.isDrivenWheel = isDrivenWheel;
        maxBrakeTorque = isFrontWheel ? spec.MaxBrakeTorqueFront : spec.MaxBrakeTorqueRear;
    }

    public void ApplyInput(float torque, float brakeTorque, float steer)
    {
        // RPMに応じて単調に増える
        wheel.wheelDampingRate = Mathf.Lerp(0.05f, 0.3f, Mathf.InverseLerp(0f, 1000f, Mathf.Abs(wheel.rpm)));
        // 駆動トルク制御
        wheel.motorTorque = isDrivenWheel ? torque : 0f;
        // ステアリング制御
        wheel.steerAngle = isFrontWheel ? steer * maxSteerAngle : 0f;
        // ブレーキ制御
        wheel.brakeTorque = brakeTorque * maxBrakeTorque / 2;
    }
    void SyncVisual()
    {
        wheel.GetWorldPose(out wheelPos, out wheelRot);
        wheelVisual.SetPositionAndRotation(wheelPos, wheelRot);
    }
}
