using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class VehicleInputHandler : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public float SteerAxis {  get; private set; }
    public float AccelerationAxis {  get; private set; }
    public float BrakeAxis {  get; private set; }
    public float ClutchAxis {  get; private set; }
    float clutchAxis;
    float keyboardClutchAxis;
    public readonly int CLUTCH_MAX_INPUT = 1;
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
    /// <summary>
    /// クラッチの入力を取得する
    /// ハンコン・キーボード両方が使う
    /// ハンコンだと0～1,キーボードは0 or 0.5
    /// </summary>
    /// <param name="context"></param>
    public void OnClutchInput(InputAction.CallbackContext context)
    {
        clutchAxis = context.ReadValue<float>();
        CombineClutchAxis(context.control.device);
    }
    /// <summary>
    /// キーボードで0.5の入力を扱うためのメソッド
    /// </summary>
    /// <param name="context"></param>
    public void OnClutchInputByKeyboard(InputAction.CallbackContext context)
    {
        keyboardClutchAxis = context.ReadValue<float>();
        CombineClutchAxis(context.control.device);
    }
    /// <summary>
    /// ハンコンの場合はそのままの値を使い
    /// キーボードの場合に2ボタンの入力を合算する
    /// </summary>
    /// <param name="device"></param>
    void CombineClutchAxis(InputDevice device)
    {
        if (device is Keyboard)
        {
            ClutchAxis = Mathf.Clamp01(clutchAxis + keyboardClutchAxis);
        }
        else
        {
            ClutchAxis = clutchAxis;
        }
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
