using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleInputHandler : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public float SteerAxis {  get; private set; }
    public float AccelerationAxis {  get; private set; }
    public float BrakeAxis {  get; private set; }
    public float ClutchAxis {  get; private set; }
    public event Action<int> OnGearPressed;
    
    public void OnSteerInput(InputAction.CallbackContext context)
    {
        SteerAxis = context.ReadValue<float>();
    }
    public void OnAccelerationInput(InputAction.CallbackContext context)
    {
        AccelerationAxis = context.ReadValue<float>();
    }
    public void OnBrakeInput(InputAction.CallbackContext context)
    {
        BrakeAxis = context.ReadValue<float>();
    }
    public void OnClutchInput(InputAction.CallbackContext context)
    {
        ClutchAxis = context.ReadValue<float>();
    }
    void HandleGearInput(InputAction.CallbackContext context, int gear)
    {
        if (context.performed)
        {
            OnGearPressed?.Invoke(gear);
        }
    }
    public void OnNeutralInput(InputAction.CallbackContext context) => HandleGearInput(context, 0);
    public void OnGear1Input(InputAction.CallbackContext context) => HandleGearInput(context, 1);
    public void OnGear2Input(InputAction.CallbackContext context) => HandleGearInput(context, 2);
    public void OnGear3Input(InputAction.CallbackContext context) => HandleGearInput(context, 3);
    public void OnGear4Input(InputAction.CallbackContext context) => HandleGearInput(context, 4);
    public void OnGear5Input(InputAction.CallbackContext context) => HandleGearInput(context, 5);
    public void OnGear6Input(InputAction.CallbackContext context) => HandleGearInput(context, 6);
    public void OnReverseInput(InputAction.CallbackContext context) => HandleGearInput(context, -1);
}
