using System;
using UnityEngine;

public class InGameController : MonoBehaviour, IGameStateRequestable
{
    public event Action <GameState> OnNextRequested;
    //GameModeContextからGameModeを受け取ってインゲームの初期化をする
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
        SetState(InGameSceneState.Transitioning);
    }
    void SetMenuState()
    {
        SetState(InGameSceneState.Menu);
    }
    void NextSceneRequest()
    {
        OnNextRequested?.Invoke(GameState.Title);
    }

    //timeAttackControllerにHandlerを直接渡すパターンだとStateを見れないと思った
    //Stateを見れないとMenuを開いてないのにブラインド状態でゲームを終わらせたりできそう
    
    //なのでHandlerを直接渡すパターンだとOpenMenuしたときにMenuUIControllerでBoolの値を持っておいて
    //MenuUIClickでその値を見て動かしてもいいか判断
    
    //もしくはIInGameControllerでClickのメソッドとかActionを追加してInGameControllerの中でStateを見てMenuなら
    //IInGameController.なんとかでTimeAttackControllerやFreeMoveに渡す
    
    //一旦上の方法で動かす
    //void MenuUIClick()
    //{
    //    if (currentState != InGameSceneState.Menu) { return; }


    //}
}
