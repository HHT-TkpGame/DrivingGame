using System;
using UnityEngine;

public interface IGameModeRequestable
{
    public event Action<GameModeState> OnModeRequested;
}
