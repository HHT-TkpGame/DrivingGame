using System;
using UnityEngine;

public interface ICheckPointReceiver
{
    event Action<int> OnCheckPointUpdated;
    void OnCheckPointPassed(int index);
    int MaxCheckPoint { get; }
}
