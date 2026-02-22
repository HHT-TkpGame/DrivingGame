using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/GameModeRequestChannel")]
public class GameModeRequestChannel : ScriptableObject
{
    public event Action<GameModeState> OnModeRequested;
    public event Action<DriveType> OnTypeRequested;

    public void Raise(GameModeState mode)
    {
        OnModeRequested?.Invoke(mode);
    }
    public void Raise(DriveType type)
    {
        OnTypeRequested?.Invoke(type);
    }
}
