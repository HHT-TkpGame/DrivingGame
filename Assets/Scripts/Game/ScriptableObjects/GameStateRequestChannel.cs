using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event/GameStateRequestChannel")]
public class GameStateRequestChannel : ScriptableObject
{
    public event Action<GameState> OnRequested;
    public void Raise(GameState state)
    {
        OnRequested?.Invoke(state);
    }
}
