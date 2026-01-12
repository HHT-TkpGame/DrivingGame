using System;
using UnityEngine;

public class TimeAttackController : MonoBehaviour, IInGameController
{
    [SerializeField] InGameUIController uIController;
    [SerializeField] CheckPointGenerator generator;
    LapTracker tracker;
    public event Action OnInitializeEnd;
    public event Action OnGameEnd;
    public event Action OnResultEnd;
    public event Action OnMenuClosed;
    public event Action OnTransitionEnd;
    [SerializeField] FadeController fade;
    [SerializeField] Countdown countdown;
    public void Initialize()
    {
        fade.OnFadeInEnd += countdown.StartCountdown;
        countdown.OnCountdownEnd += EndInitialize;
        tracker = new LapTracker();
        tracker.OnGoal += EndGame;
        generator.Initialize(tracker);
        tracker.Initialize(generator.Generate());
        fade.StartAnimation();
    }
    void EndInitialize()
    {
        OnInitializeEnd?.Invoke();
    }
    void EndGame()
    {
        OnGameEnd?.Invoke();
        uIController.StopUI();
    }
    public void StartGame()
    {
        uIController.Initialize(tracker);
        uIController.StartUI();
    }
    public void StartResult()
    {

    }
    public void StartTransition()
    {

    }
    public void OpenMenu()
    {

    }
}
