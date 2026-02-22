using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectController : MonoBehaviour, IGameStateRequestable, IGameModeRequestable
{
    public event Action<GameState> OnNextRequested;
    public event Action<GameModeState> OnModeRequested;
    public event Action<DriveType> OnDriveTypeRequested;
    SelectSceneState currentState = SelectSceneState.Initializing;
    [SerializeField] FadeController fadeController;
    [SerializeField] SelectUIController uiController;
    [SerializeField] SelectUIInputHandler inputHandler;
    [SerializeField] BGMSetter BGM;
    void Start()
    {
        SetState(currentState);
        fadeController.OnFadeInEnd += SetModeSelectState;
        uiController.OnModeSelected += RequestGameMode;
        uiController.OnModeSelectEnd += SetDriveModeSelectState;
        uiController.OnModeDriveTypeSelected += RequestDriveType;
        uiController.OnDriveTypeSelectEnd += SetCustomizeState;
        uiController.OnCustomizeEnd += SetTransitioningState;
        fadeController.OnFadeOutEnd += NextSceneRequest;
        inputHandler.OnModeSelect += SetGameMode;
        inputHandler.OnClickButton += SelectGameMode;
        inputHandler.OnModeSelect += SetDriveType;
        inputHandler.OnClickButton += SelectDriveType;
        inputHandler.OnVerticalButton += SetColorCursorVert;
        inputHandler.OnHorizontalButton += SetSliderVal;
        inputHandler.OnSelectEnd += SelectEnd;
        inputHandler.OnResetCarColor += ResetCarColor;
        inputHandler.OnStartCancelButton += CancelStart;
    }

    void ResetCarColor()
    {
        if(currentState != SelectSceneState.Customize) { return; }
        uiController.SetDefaultCarColor();
    }

    void SelectEnd()
    {
        if (currentState != SelectSceneState.Customize) { return; }
        uiController.FinishSelectScene();
    }

    void SetColorCursorVert(float f)
    {
		if (currentState != SelectSceneState.Customize) { return; }
		uiController.SetColorVert(f);
    }
    void SetSliderVal(float f)
    {
		if (currentState != SelectSceneState.Customize) { return; }
		uiController.SetSliderVal(f);
    }


    void SelectGameMode()
    {
		if (currentState != SelectSceneState.ModeSelect) { return; }

        uiController.SelectGameMode();
	}

    void SetGameMode()
    {
        if (currentState != SelectSceneState.ModeSelect) { return; }
        
        uiController.SetGameMode();
    }

    void SelectDriveType()
    {
        if (currentState != SelectSceneState.DriveTypeSelect) { return; }

        uiController.SelectDriveType();
    }

    void SetDriveType()
    {
        if (currentState != SelectSceneState.DriveTypeSelect) { return; }

        uiController.SetDriveType();
    }
    void CancelStart()
    {
        uiController.CancelStart();
    }

    ///やる流れ
    ///モードセレクトのUIが出てくる
    ///ステートを見て左右入力を受け取ってもいいかを見る
    ///SelectControllerが受け取った値をUIControllerに渡す
    ///UIを動かしてModeを受け取る
    ///Clickに代わる入力(Customizeのほうのやつでも)を作ってそれが押されたらSelectControllerでステートを変える
    ///
    ///これでカラーのところまでいってるはず
    ///上下もボタンのScaleの値からいけると思う
    ///左右はChatGPTのやつ
    ///決定はCustomizeのほうのやつ
    ///受け取ったものをUIコントローラー

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
            case SelectSceneState.DriveTypeSelect:
                uiController.StartDriveTypeSelect();
                break;
            case SelectSceneState.Customize:
                uiController.StartCustomize();
                break;
            case SelectSceneState.Transitioning:
                fadeController.StartAnimation();
                BGM.StartBGMDown();
                break;
        }
    }
    void SetModeSelectState() { SetState(SelectSceneState.ModeSelect); }
    void SetDriveModeSelectState() { SetState(SelectSceneState.DriveTypeSelect); }
    void SetCustomizeState(){ SetState(SelectSceneState.Customize); }
    void SetTransitioningState() { SetState(SelectSceneState.Transitioning); }
    void RequestGameMode(GameModeState mode) { OnModeRequested?.Invoke(mode); }   
    void RequestDriveType(DriveType type) {  OnDriveTypeRequested?.Invoke(type); }
    
    void NextSceneRequest()
    {
        OnNextRequested?.Invoke(GameState.InGame);
    }
}
