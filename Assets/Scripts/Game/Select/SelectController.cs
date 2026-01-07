using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectController : MonoBehaviour, IGameStateRequestable, IGameModeRequestable
{
    public event Action<GameState> OnNextRequested;
    public event Action<GameModeState> OnModeRequested;
    SelectSceneState currentState = SelectSceneState.Initializing;
    [SerializeField] FadeController fadeController;
    [SerializeField] SelectUIController uiController;
    void Start()
    {
        SetState(currentState);
        fadeController.OnFadeInEnd += SetModeSelectState;
        uiController.OnModeSelected += RequestGameMode;
        uiController.OnModeSelectEnd += SetCustomizeState;
        uiController.OnCustomizeEnd += SetTransitioningState;
        fadeController.OnFadeOutEnd += NextSceneRequest;
    }

    void Update()
    {

    }
    void SetState(SelectSceneState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case SelectSceneState.Initializing:
                fadeController.StartAnimation();
                break;
            case SelectSceneState.ModeSelect:
                uiController.StartModeSelect();
                break;
            case SelectSceneState.Customize:
                uiController.StartCustomize();
                break;
            case SelectSceneState.Transitioning:
                fadeController.StartAnimation();
                break;
        }
    }
    void SetModeSelectState() { SetState(SelectSceneState.ModeSelect); }
    void SetCustomizeState(){ SetState(SelectSceneState.Customize); }
    void SetTransitioningState() { SetState(SelectSceneState.Transitioning); }
    void RequestGameMode(GameModeState mode) { OnModeRequested?.Invoke(mode); }    
    void NextSceneRequest()
    {
        OnNextRequested?.Invoke(GameState.InGame);
    }
}
