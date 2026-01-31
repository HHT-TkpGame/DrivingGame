using System;
using UnityEngine;

public class InGameController : MonoBehaviour, IGameStateRequestable
{
    public event Action <GameState> OnNextRequested;
    //GameModeContextÇ©ÇÁGameModeÇéÛÇØéÊÇ¡ÇƒÉCÉìÉQÅ[ÉÄÇÃèâä˙âªÇÇ∑ÇÈ
    [SerializeField] GameModeContext modeContext;
    [SerializeField] TimeAttackController timeAttackController;
    [SerializeField] VehicleInputHandler vehicleInputHandler;
    IInGameController iGameController;
    InGameSceneState currentState = InGameSceneState.Initializing;
    void Start()
    {
        SetState(InGameSceneState.Initializing);
    }
    void SetState(InGameSceneState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case InGameSceneState.Initializing:
                SetupGame();
                break;
            case InGameSceneState.Play:
                iGameController.StartGame();
                break;
            case InGameSceneState.Menu:
                iGameController.OpenMenu();
				break;
            case InGameSceneState.Result:
                iGameController.StartResult();
                break;
            case InGameSceneState.Transitioning:
                iGameController.StartTransition();
                break;
        }
    }
    void SetupGame()
    {
        switch (modeContext.currentMode)
        {
            case GameModeState.TimeAttack:
                iGameController = timeAttackController;
                break;
            case GameModeState.FreeDrive:
                break;
        }
        iGameController.Initialize(vehicleInputHandler);
        SetSubscribers();
    }
    void SetSubscribers()
    {
        iGameController.OnInitializeEnd += SetPlayState;
        iGameController.OnGameEnd += SetResultState;
        iGameController.OnResultEnd += SetTransitionState;
        iGameController.OnMenuClosed += SetPlayState;
        iGameController.OnMenuDriveEnd += NextSceneRequest;
        iGameController.OnTransitionEnd += NextSceneRequest;
        vehicleInputHandler.OnMenuButtonPressed += SetMenuState;
        //vehicleInputHandler.InMenuClick += MenuUIClick;
    }
    void SetPlayState()
    {
        SetState(InGameSceneState.Play);
    }
    void SetResultState()
    {
        SetState(InGameSceneState.Result);
    }
    void SetTransitionState()
    {
        if(currentState != InGameSceneState.Result) { return; }
        SetState(InGameSceneState.Transitioning);
    }
    void SetMenuState()
    {
        if(currentState != InGameSceneState.Play 
            && currentState != InGameSceneState.Menu) { return; }
        SetState(InGameSceneState.Menu);
    }
    void NextSceneRequest()
    {
        OnNextRequested?.Invoke(GameState.Title);
    }
}
