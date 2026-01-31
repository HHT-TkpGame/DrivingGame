using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] TimeAttackTimer timer;
    [SerializeField] LapUI lapUI;
    [SerializeField] ResultUIController resultUI;
    public event Action OnExitRequested;
    public void Initialize
    (
        ICheckPointReceiver receiver,
        VehicleInputHandler handler
    )
    {
        receiver.OnCheckPointUpdated += lapUI.UpdateDisplay;
        timer.OnTimerStopped += resultUI.SetResult;
        lapUI.Initialize(receiver.MaxCheckPoint);
        resultUI.Initialize(handler);
        resultUI.exitButtonPressed += EndResultRequest;
    }
    public void StartInGameUI()
    {
        timer.StartTimer();
    }
    public void StopInGameUI()
    {
        timer.StopTimer();
    }
    public void StartResultUI()
    {
        resultUI.StartAnimation();
    }
    void EndResultRequest()
    {
        OnExitRequested?.Invoke();
    }
}
