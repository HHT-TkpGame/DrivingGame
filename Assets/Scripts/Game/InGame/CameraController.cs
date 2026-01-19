using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //todo âºé¿ëïÅ@å„Ç≈HandlerÇâÓÇµÇƒéÛÇØéÊÇÈÇÊÇ§Ç…Ç∑ÇÈ
    bool isFacingForward = true;
    bool isFirstPerson = true;
    [SerializeField] Camera firstPersonCamera;
    [SerializeField] Camera thirdPersonCamera;
    [SerializeField] Camera firstPersonBehindCamera;
    [SerializeField] Camera thirdPersonBehindCamera;

    void Start()
    {
        UpdateCameraState();
    }
    public void TogglePerspective(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFirstPerson = !isFirstPerson;
            UpdateCameraState();
        }
    }
    public void ToggleFacingDirection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFacingForward = !isFacingForward;
            UpdateCameraState();
        }
    }
    void UpdateCameraState()
    {
        SetActiveCamera(firstPersonCamera, false);
        SetActiveCamera(thirdPersonCamera, false);
        SetActiveCamera(firstPersonBehindCamera, false);
        SetActiveCamera(thirdPersonBehindCamera, false);
        Camera c = GetActiveCamera();
        SetActiveCamera(c, true);
    }
    Camera GetActiveCamera()
    {
        if (isFirstPerson)
        {
            return isFacingForward
                ? firstPersonCamera
                : firstPersonBehindCamera;
        }
        return isFacingForward
            ? thirdPersonCamera
            : thirdPersonBehindCamera;
    }
    void SetActiveCamera(Camera camera ,bool enable)
    {
        camera.depth = enable ? 1 : 0;
    }
}
