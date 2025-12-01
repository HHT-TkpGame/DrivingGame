using UnityEngine;

public class TransmissionModelTest : MonoBehaviour
{
    [SerializeField] CarSpec carSpec;
    TransmissionModel model;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model = new TransmissionModel(carSpec);
        model.SetGear(1);
        TestReturnTorque(engineRPM: 2000, wheelRPM: 1000, clutch: 1f);
        TestReturnTorque(engineRPM: 4000, wheelRPM: 3000, clutch: 1f);
        TestReturnTorque(engineRPM: 4000, wheelRPM: 3500, clutch: 0.5f);
        TestReturnTorque(engineRPM: 2000, wheelRPM: 2500, clutch: 1f);
        TestReturnTorque(engineRPM: 1000, wheelRPM: 3000, clutch: 1f);
    }

    void TestReturnTorque(float engineRPM, float wheelRPM, float clutch)
    {
        float torque = model.CalculateReturnTorque(engineRPM, wheelRPM, clutch);
        Debug.Log($"[Test] Gear={model.CurrentGear}, eRPM={engineRPM:F0}, wRPM={wheelRPM:F0}, clutch={clutch:F2} Å® ReturnTorque={torque:F2}");
    }
}
