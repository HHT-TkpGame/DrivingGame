using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneChanger sceneChanger;
    [SerializeField] GameStateRequestChannel stateRequestChannel;
    [SerializeField] GameModeRequestChannel modeRequestChannel;
    [SerializeField] GameModeContext modeContext;
    GameState currentState = GameState.Title;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        sceneChanger = new SceneChanger();
        stateRequestChannel.OnRequested += ChangeState;
        modeRequestChannel.OnRequested += modeContext.SetMode;
    }
    void OnDestroy()
    {
        stateRequestChannel.OnRequested -= ChangeState;
        modeRequestChannel.OnRequested -= modeContext.SetMode;
    }
    void ChangeState(GameState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
            sceneChanger.Change(newState);
        }
    }
}

