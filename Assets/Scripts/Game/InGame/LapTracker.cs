using System;
using UnityEngine;

public class LapTracker : ICheckPointReceiver
{
    CheckPoint[] checkPoints;
    public int MaxCheckPoint => checkPoints.Length;
    int currentIndex;
    public event Action OnGoal;
    public event Action<int> OnCheckPointUpdated;
    public void Initialize
    (
        CheckPoint[] checkPoints
    )
    {
        this.checkPoints = checkPoints;
        checkPoints[0].SetVisible(true);
    }
    public void OnCheckPointPassed(int index)
    {
        if (currentIndex != index) { return; }
        UpdateCheckPoint();
        Debug.Log($"currentIndex:{currentIndex}");
    }
    public void UpdateCheckPoint()
    {
        checkPoints[currentIndex].SetVisible(false);
        currentIndex++;
        OnCheckPointUpdated?.Invoke(currentIndex);
        if (currentIndex < checkPoints.Length) 
        {
            checkPoints[currentIndex].SetVisible(true); 
        }
        else
        {
            OnGoal?.Invoke();
            Debug.Log("goal");
        }
    }
}
