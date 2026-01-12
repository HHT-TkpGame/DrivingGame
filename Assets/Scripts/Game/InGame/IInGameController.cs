using System;
using UnityEngine;

public interface IInGameController
{
    void Initialize();
    void StartGame();
    void OpenMenu();
    void StartResult();
    void StartTransition();
    event Action OnInitializeEnd;
    event Action OnGameEnd;
    event Action OnResultEnd;
    event Action OnMenuClosed;
    event Action OnTransitionEnd;
}
