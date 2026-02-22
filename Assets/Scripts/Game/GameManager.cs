using UnityEngine;
using UnityEngine.SceneManagement;

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
        instance = this;
        DontDestroyOnLoad(gameObject);
        sceneChanger = new SceneChanger();
        // èââÒçwì«
        Subscribe();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        Debug.Log("[GM] OnDestroy");

        SceneManager.sceneLoaded -= OnSceneLoaded;
        Unsubscribe();

        if (instance == this) instance = null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ìÒèdìoò^ñhé~ÇµÇ¬Ç¬çwì«ÇµíºÇ∑
        Subscribe();
        Debug.Log($"[GM] Re-Subscribe on sceneLoaded: {scene.name}");
    }

    void Subscribe()
    {
        // ìÒèdìoò^âÒî
        stateRequestChannel.OnRequested -= ChangeState;
        modeRequestChannel.OnModeRequested -= modeContext.SetMode;
        modeRequestChannel.OnTypeRequested -= modeContext.SetType;

        stateRequestChannel.OnRequested += ChangeState;
        modeRequestChannel.OnModeRequested += modeContext.SetMode;
        modeRequestChannel.OnTypeRequested += modeContext.SetType;
    }

    void Unsubscribe()
    {
        stateRequestChannel.OnRequested -= ChangeState;
        modeRequestChannel.OnModeRequested -= modeContext.SetMode;
        modeRequestChannel.OnTypeRequested -= modeContext.SetType;
    }

    void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        sceneChanger.Change(newState);
    }
}