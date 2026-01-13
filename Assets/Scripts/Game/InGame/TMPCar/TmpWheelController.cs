    using UnityEngine;

    public class TmpWheelController : MonoBehaviour
    {
        WheelCollider wheel;
        [SerializeField] Transform wheelVisual;
        Vector3 wheelPos;
        Quaternion wheelRot;
        bool isFrontWheel;
        bool isDrivenWheel;
        public float WheelRPM { 
            get 
            {
                float rpm = wheel.rpm;
                if (float.IsNaN(rpm)|| float.IsInfinity(rpm)|| Mathf.Abs(rpm) < 0.1f)
                {
                    rpm = 0f;
                }
                return rpm; 
            }
        }
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
        /// �z�C�[���̃f�[�^������
        /// </summary>
        /// <param name="isFrontWheel">�O�ւ��ǂ���</param>
        /// <param name="isDrivenWheel">�쓮�ւ��ǂ���</param>
        /// <param name="radius">�z�C�[�����a</param>
        public void Init(CarSpec spec, bool isFrontWheel, bool isDrivenWheel, float radius)
        {
            wheel.radius = radius;
            this.isFrontWheel = isFrontWheel;
            this.isDrivenWheel = isDrivenWheel;
            maxBrakeTorque = isFrontWheel ? spec.MaxBrakeTorqueFront : spec.MaxBrakeTorqueRear;
        }

        public void ApplyInput(float torque, float brakeTorque, float steer)
        {
            // RPM�ɉ����ĒP���ɑ������]��R
            wheel.wheelDampingRate = Mathf.Lerp(0.05f, 0.3f, Mathf.InverseLerp(0f, 1000f, Mathf.Abs(wheel.rpm)));
            // �쓮�g���N����
            wheel.motorTorque = isDrivenWheel ? torque : 0f;
            // �X�e�A�����O����
            wheel.steerAngle = isFrontWheel ? steer * maxSteerAngle : 0f;
            // �u���[�L����
            wheel.brakeTorque = brakeTorque * maxBrakeTorque / 2;
        }
        void SyncVisual()
        {
            wheel.GetWorldPose(out wheelPos, out wheelRot);
            wheelVisual.SetPositionAndRotation(wheelPos, wheelRot);
        }
    }
