using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIController : MonoBehaviour
{
    public event Action<GameModeState> OnModeSelected;
    public event Action OnModeSelectEnd;
    public event Action OnCustomizeEnd;
    [SerializeField] CustomizeController customizeController;
    [SerializeField] CarColorSetter carColorSetter;
    [SerializeField] Button taModeButton;
    [SerializeField] Button freeModeButton;
    [SerializeField] UIMover modeMover;
    [SerializeField] UIMover customMover;
    [SerializeField] Transform[] modeButtons;

    GameModeState currentMode = GameModeState.FreeDrive;
    [SerializeField] GameObject cursorImage;
    
    //ゲームモードを選ぶときに選ぶ前にクリックされるの防止
    bool isModeSelecting;

    void Start()
    {
        modeMover.OnReturnEnd += EndModeSelect;
      
        //ここのボタン群は後からコントローラ入力に変えるのでクリック制御は一旦しない
        //taModeButton.onClick.AddListener(
        //    () => SelectGameMode(GameModeState.TimeAttack)
        //);
        //freeModeButton.onClick.AddListener(
        //    () => SelectGameMode(GameModeState.FreeDrive)
        //);
        customMover.OnMoveEnd += customizeController.SetEnable;
        customMover.OnMoveEnd += ColorSetterInit;
        customizeController.OnCustomEnd += customMover.Return;
        customizeController.OnCustomEnd += carColorSetter.SaveCarColor;
        customMover.OnReturnEnd += EndCustomize;
        
        cursorImage.SetActive( false );
    }

	void ColorSetterInit()
	{
        carColorSetter.Initialize();
	}
    public void SetDefaultCarColor()
    {
       carColorSetter.SetStandardColor();
    }
    public void FinishSelectScene()
    {
        customizeController.EndCustomize();
    }

	public void SetColorVert(float f)
    {
        carColorSetter.CursorLift( f );
    }

    public void SetSliderVal(float f)
    {
        carColorSetter.MoveSlider( f );
    }
    public void SetGameMode()
    {
        //currentModeを反転
        currentMode = currentMode == GameModeState.TimeAttack?
            GameModeState.FreeDrive : GameModeState.TimeAttack;
        cursorImage.transform.localPosition = modeButtons[(int)currentMode].localPosition;

        if (!isModeSelecting)
        {
			isModeSelecting = true;
            cursorImage.SetActive(isModeSelecting);
		}
	}

    public void SelectGameMode()
    {
        //CursorUIが表示されていないならGameModeを選択できないようにする
        if (!isModeSelecting) { return; }

        SelectGameMode(currentMode);
    }

    void SelectGameMode(GameModeState mode)
    {
        Debug.Log("StartReturn");
        OnModeSelected?.Invoke(mode);
        modeMover.Return();
    }
    public void StartModeSelect()
    {
        modeMover.StartMove();
    }
    void EndModeSelect()
    {
        OnModeSelectEnd?.Invoke();
    }
    public void StartCustomize()
    {
        customMover.StartMove();

    }
    void EndCustomize()
    {
        OnCustomizeEnd?.Invoke();
    }
}
