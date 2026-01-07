using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleController : MonoBehaviour, IGameStateRequestable
{
    TitleSceneState currentState = TitleSceneState.Initializing;
    public event Action<GameState> OnNextRequested;
    [SerializeField] GameStartText gameStartText;
    [SerializeField] FadeController fadeController;
    void Start()
    {
        SetState(currentState);
        fadeController.OnFadeInEnd += SetIdleState;
        gameStartText.OnBlinkEnd += fadeController.StartAnimation;
        fadeController.OnFadeOutEnd += NextSceneRequest;
    }

    void Update()
    {
        
    }
    void SetState(TitleSceneState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case TitleSceneState.Initializing:
                fadeController.StartAnimation();
                break;
            case TitleSceneState.Idle:
                break;
            case TitleSceneState.Menu:
                break;
            case TitleSceneState.Transitioning:
                gameStartText.StartBlink();
                break;
        }
    }
    void SetIdleState(){ SetState(TitleSceneState.Idle); }
    void SetMenuState() { SetState(TitleSceneState.Menu); }
    void SetTransitioningState() { SetState(TitleSceneState.Transitioning); }
    void NextSceneRequest()
    {
        OnNextRequested?.Invoke(GameState.Select);
    }
    public void ProceedToSelect(InputAction.CallbackContext context)
    {
        if(currentState != TitleSceneState.Idle)
        {
            return;
        }
        if (context.performed)
        {
            SetTransitioningState();
        }
    }
}
