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
        tracker = new LapTracker();
        fade.OnFadeInEnd += countdown.StartCountdown;
        countdown.OnCountdownEnd += EndInitialize;
        tracker.OnGoal += EndGame;
        uIController.OnExitRequested += EndResult;
        fade.OnFadeOutEnd += EndTransition;
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
        uIController.StopInGameUI();
    }
    void EndResult()
    {
        OnResultEnd?.Invoke();
    }
    void EndTransition()
    {
        OnTransitionEnd?.Invoke();
    }
    public void StartGame()
    {
        uIController.Initialize(tracker);
        uIController.StartInGameUI();
    }
    public void StartResult()
    {
        uIController.StartResultUI();
    }
    public void StartTransition()
    {
        fade.StartAnimation();
    }
    public void OpenMenu()
    {

    }
}
