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
    public void EndCustomize()
    {
        if (!enable) { return; }
            OnCustomEnd?.Invoke();
		enable = false;
        
    }
}
