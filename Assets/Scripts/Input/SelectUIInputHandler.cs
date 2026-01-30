using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class SelectUIInputHandler : MonoBehaviour
{
    public event Action OnModeSelect;
    public event Action OnClickButton;
    public event Action<float> OnVerticalButton;//èc
    public event Action<float> OnHorizontalButton;//â°
    public event Action OnSelectEnd;
    public event Action OnResetCarColor;

    float horizontalValue;
    Coroutine horizontalCoroutine;

    const float slidePower = 0.8f;

    public void ResetCarColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnResetCarColor?.Invoke();
        }
    }

    public void SelectEndButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSelectEnd?.Invoke();
        }
    }

    public void ModeButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnModeSelect?.Invoke();
        }
    }

    public void InSelectClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnClickButton?.Invoke();
        }
    }

    public void VerticalArrowPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnVerticalButton?.Invoke(context.ReadValue<float>());
        }
    }

    public void HorizontalArrowPressed(InputAction.CallbackContext context)
    {
        //if (context.started)
        //{
        //    float v = context.ReadValue<float>();
        //    v*=Time.deltaTime*slidePower;
        //    horizontalCoroutine = StartCoroutine(HorizontalCoroutine(v));
        //}
        //else if (context.canceled)
        //{
        //    if (horizontalCoroutine != null)
        //    {
        //        StopCoroutine(horizontalCoroutine);
        //        horizontalCoroutine = null;
        //    }
        //}

        float value = context.ReadValue<float>();
        if (Mathf.Approximately(value, 0f))
        {
            horizontalValue = 0;
            return;
        }
        horizontalValue = value * slidePower;

    }

    private void Update()
    {
        if (Mathf.Approximately(horizontalValue, 0f)) { return; }

        OnHorizontalButton?.Invoke(horizontalValue * Time.deltaTime);
    }

    //  IEnumerator HorizontalCoroutine(float v)
    //  {
    //      while (true)
    //      {
    //	OnHorizontalButton?.Invoke(v);
    //          yield return null;
    //}
    //  }
}
