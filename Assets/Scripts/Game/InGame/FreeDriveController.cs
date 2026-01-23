using System;
using UnityEngine;

public class FreeDriveController : MonoBehaviour, IInGameController
{
    public event Action OnInitializeEnd;
    public event Action OnGameEnd;
    public event Action OnResultEnd;
    public event Action OnMenuClosed;
    public event Action OnTransitionEnd;
    [SerializeField] VehicleInputHandler vehicleInputHandler;
    public void Initialize(VehicleInputHandler handler)
    {
        
    }
    public void StartGame()
    {

    }
    public void StartResult()
    {

    }
    public void StartTransition()
    {

    }
    public void OpenMenu()
    {

    }
}
