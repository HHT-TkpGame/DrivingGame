using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleInputHandler : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    InputAction clutchAct;
    InputAction clutchKeyboardAct;
    InputAction accelAct;
    InputAction brakeAct;
    InputAction steerAct;
    public float SteerAxis {  get; private set; }
    public float AccelerationAxis {  get; private set; }
    public float BrakeAxis {  get; private set; }
    public float ClutchAxis {  get; private set; }
    float mainClutchAxis;
    float keyboardClutchAxis;
    public event Action<int> OnGearPressed;

    public event Action OnMenuButtonPressed;
    public event Action InMenuClick;
    public event Action<float> MenuArrows;
    public void MenuButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMenuButtonPressed?.Invoke();
        }
    }
    public void InMenuUIClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InMenuClick?.Invoke();    
        }
    } 

    public void MenuArrowPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuArrows?.Invoke(context.ReadValue<float>());
			
		}
    }

    private void Awake()
    {
        clutchAct = input.actions["Clutch"];
        clutchKeyboardAct = input.actions["ClutchByKeyboard"];
        accelAct = input.actions["Accelerator"];
        brakeAct = input.actions["Brake"];
        steerAct = input.actions["Steering"];
    }
    void Update()
    {
        AccelerationAxis = ProcessingHandleControllerInput(
            true,
            accelAct.ReadValue<float>(),
            accelAct.activeControl?.device
        );
        BrakeAxis = ProcessingHandleControllerInput(
            false,
            brakeAct.ReadValue<float>(),
            brakeAct.activeControl?.device
        );
        SteerAxis = steerAct.ReadValue<float>();
        ClutchAxis = ProcessingClutchInput();
    }

    /// <summary>
    /// ハンコン・キーボード入力を考慮し加工した入力を返す
    /// </summary>
    /// <returns></returns>
    float ProcessingClutchInput()
    {
        mainClutchAxis = ProcessingHandleControllerInput(
            false,
            clutchAct.ReadValue<float>(),
            clutchAct.activeControl?.device
        );
        keyboardClutchAxis = clutchKeyboardAct.ReadValue<float>();

        //キーボードのみの入力で、0か1しかないので0比較でOK
        if (keyboardClutchAxis == 0f)
        {
            return mainClutchAxis;
        }
        return Mathf.Clamp01(mainClutchAxis + keyboardClutchAxis);
    }

    /// <summary>
    /// ハンコンの入力だった場合、ペダルの入力値が-1～1なので0～1に変換する
    /// 現在だとアクセル・ブレーキ・クラッチの3つ
    /// </summary>
    /// <param name="negate">反転が必要かどうか</param>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    float ProcessingHandleControllerInput(bool negate, float inputValue, InputDevice device)
    {
        if(device == null) { return inputValue; }
        if (device.name != "44F B677") { return inputValue; }
        int v = negate? -1 : 1;
        inputValue = (inputValue * v + 1) / 2; 
        return inputValue;
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
