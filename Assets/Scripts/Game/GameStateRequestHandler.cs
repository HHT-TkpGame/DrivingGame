using UnityEngine;

public class GameStateRequestHandler : MonoBehaviour
{
    [SerializeField] GameStateRequestChannel stateRequestChannel;
    [SerializeField, Header("IGameStateRequestable‚ðŽÀ‘•‚µ‚½ƒNƒ‰ƒX‚Å‚ ‚é‚±‚Æ")]
    MonoBehaviour monoBehaviour;
    IGameStateRequestable iRequestable;
    void Awake()
    {
        iRequestable = monoBehaviour as IGameStateRequestable;
        iRequestable.OnNextRequested += OnStateChangeRequested;
    }
    void OnDestroy()
    {
        iRequestable.OnNextRequested -= OnStateChangeRequested;
    }
    public void OnStateChangeRequested(GameState newState)
    {
        stateRequestChannel.Raise(newState);
    }
}
