using UnityEngine;

public class GameModeRequestHandler : MonoBehaviour
{
    [SerializeField] GameModeRequestChannel modeRequestChannel;
    [SerializeField, Header("IGameModeRequestable‚ðŽÀ‘•‚µ‚½ƒNƒ‰ƒX‚Å‚ ‚é‚±‚Æ")]
    MonoBehaviour monoBehaviour;
    IGameModeRequestable iRequestable;
    void Awake()
    {
        iRequestable = monoBehaviour as IGameModeRequestable;
        iRequestable.OnModeRequested += OnModeChangeRequested;
        iRequestable.OnDriveTypeRequested += OnDriveTypeChangeRequested;
    }
    void OnDestroy()
    {
        iRequestable.OnModeRequested -= OnModeChangeRequested;
        iRequestable.OnDriveTypeRequested -= OnDriveTypeChangeRequested;
    }
    public void OnModeChangeRequested(GameModeState mode)
    {
        modeRequestChannel.Raise(mode);
    }
    public void OnDriveTypeChangeRequested(DriveType type)
    {
        modeRequestChannel.Raise(type);
    }
}
