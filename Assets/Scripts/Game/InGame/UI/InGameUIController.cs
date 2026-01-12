using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] TimeAttackTimer timer;
    [SerializeField] LapUI lapUI;
    public void Initialize
    (
        ICheckPointReceiver receiver
    )
    {
        receiver.OnCheckPointUpdated += lapUI.UpdateDisplay;
        lapUI.Initialize(receiver.MaxCheckPoint);
    }
    public void StartUI()
    {
        timer.StartTimer();
    }
    public void StopUI()
    {
        timer.StopTimer();
    }
}
