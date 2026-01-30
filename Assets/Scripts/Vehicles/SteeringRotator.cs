using System;
using UnityEngine;

public class SteeringRotator : MonoBehaviour
{
    [SerializeField] GameObject steerObj;
    VehicleInputHandler handler;
    float maxRotAngle;
    public void Initialize(VehicleInputHandler handler)
    {
        this.handler = handler;
    }

    void Update()
    {
        float zAngle = handler.SteerAxis * maxRotAngle;
        // ¶‰ñ‚è^‰E‰ñ‚è‚ª‹t‚È‚ç•„†‚ğ”½“]‚·‚é
        steerObj.transform.localRotation =
            Quaternion.Euler(0f, 0f, zAngle);
    }
}
