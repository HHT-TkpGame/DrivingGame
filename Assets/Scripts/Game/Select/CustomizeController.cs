using NUnit.Framework.Internal.Filters;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomizeController : MonoBehaviour
{
    bool enable;
    public event Action OnCustomEnd;
    public void SetEnable()
    {
        enable = true;
    }
    public void EndCustomize(InputAction.CallbackContext context)
    {
        if (!enable) { return; }
        if (context.performed)
        {
            OnCustomEnd?.Invoke();
            enable = false;
        }
    }
}
