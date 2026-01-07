using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIController : MonoBehaviour
{
    public event Action<GameModeState> OnModeSelected;
    public event Action OnModeSelectEnd;
    public event Action OnCustomizeEnd;
    [SerializeField] CustomizeController customizeController;
    [SerializeField] Button taModeButton;
    [SerializeField] Button freeModeButton;
    [SerializeField] UIMover modeMover;
    [SerializeField] UIMover customMover;
    void Start()
    {
        modeMover.OnReturnEnd += EndModeSelect;
      
        //ここのボタン群は後からコントローラ入力に変えるのでクリック制御は一旦しない
        taModeButton.onClick.AddListener(
            () => SelectGameMode(GameModeState.TimeAttack)
        );
        freeModeButton.onClick.AddListener(
            () => SelectGameMode(GameModeState.FreeDrive)
        );
        customMover.OnMoveEnd += customizeController.SetEnable;
        customizeController.OnCustomEnd += customMover.Return;
        customMover.OnReturnEnd += EndCustomize;
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
