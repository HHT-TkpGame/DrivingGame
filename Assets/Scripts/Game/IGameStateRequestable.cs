using System;
using UnityEngine;

public interface IGameStateRequestable
{
    public event Action<GameState> OnNextRequested;
}
