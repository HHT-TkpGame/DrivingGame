using System;
using UnityEngine;

public class TimeAttackController : MonoBehaviour, IInGameController
{
    [SerializeField] VehicleController vehicleController;
    [SerializeField] InGameUIController uIController;
    [SerializeField] CheckPointGenerator generator;
    LapTracker tracker;
    public event Action OnInitializeEnd;
    public event Action OnGameEnd;
    public event Action OnResultEnd;
    public event Action OnMenuClosed;
    public event Action OnMenuDriveEnd;
    public event Action OnTransitionEnd;
    [SerializeField] FadeController fade;
    [SerializeField] Countdown countdown;

    [SerializeField] MenuUIController menuUIController;
    VehicleInputHandler handler;

    public void Initialize(VehicleInputHandler handler)
    {
        this.handler = handler; 
        tracker = new LapTracker();
        fade.OnFadeInEnd += countdown.StartCountdown;
        countdown.OnCountdownEnd += EndInitialize;
        tracker.OnGoal += EndGame;
        uIController.OnExitRequested += EndResult;
        fade.OnFadeOutEnd += EndTransition;
        generator.Initialize(tracker);
        tracker.Initialize(generator.Generate());
        fade.StartAnimation();
        menuUIController.Initialize();
        menuUIController.OnMenuClosed += MenuClosed;
        menuUIController.OnEndDrive += MenuDriveEnd;

        this.handler.InMenuClick += InMenuClick;
        this.handler.MenuArrows += MenuScroll;
    }

    void MenuDriveEnd()
    {
        OnMenuDriveEnd?.Invoke();
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
        vehicleController.SetIsPlaying();
        uIController.Initialize(tracker, handler);
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
        menuUIController.OpenMenu();
    }
    void InMenuClick()
    {
        menuUIController.ClickAnyUI();
    }
    void MenuScroll(float value)
    {
        //Debug.Log(value);
        menuUIController.MenuArrow(value);
	}
    void MenuClosed()
    {
        OnMenuClosed?.Invoke();
    }
}
