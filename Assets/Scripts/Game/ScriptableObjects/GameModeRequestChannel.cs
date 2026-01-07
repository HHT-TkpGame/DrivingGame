using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/GameModeRequestChannel")]
public class GameModeRequestChannel : ScriptableObject
{
    public event Action<GameModeState> OnRequested;
    public void Raise(GameModeState mode)
    {
        OnRequested?.Invoke(mode);
    }
}
