using UnityEngine;

public class CameraController : MonoBehaviour
{
    //todo 仮実装　後でHandlerを介して受け取るようにする
    bool isFacingForward = true;
    bool isFirstPerson = true;
    bool isLookLeftPressing;
    bool isLookRightPressing;
    [SerializeField] float maxYAngle = 45f;
    [SerializeField] float rotSpeed = 20f;
    float currentY;
    Quaternion baseFirstFront;
    Quaternion baseFirstBack;
    Quaternion baseThird;

    [SerializeField] GameObject firstFrontPivot;
    [SerializeField] GameObject firstBackPivot;
    [SerializeField] GameObject thirdPivot;
    [SerializeField] Camera firstPersonCamera;
    [SerializeField] Camera thirdPersonCamera;
    [SerializeField] Camera firstPersonBehindCamera;
    [SerializeField] Camera thirdPersonBehindCamera;
    public void Initialize(VehicleInputHandler handler)
    {
        handler.OnFacingDirectionButtonPressed += ToggleFacingDirection;
        handler.OnPerspectiveButtonPressed += TogglePerspective;
        handler.OnLookLeftChanged += SetLookLeft;
        handler.OnLookRightChanged += SetLookRight;
    }

    void Start()
    {
        UpdateCameraState();
        //初期角度キャッシュ
        baseFirstFront = firstFrontPivot.transform.localRotation;
        baseFirstBack = firstBackPivot.transform.localRotation;
        baseThird = thirdPivot.transform.localRotation;
    }
    void Update()
    {
        UpdateRot();    
    }
    void SetLookLeft(bool isLeft)
    {
        isLookLeftPressing = isLeft;
    }
    void SetLookRight(bool isRight)
    {
        isLookRightPressing = isRight;
    }

    void UpdateRot()
    {
        //両方入力or未入力は回さない
        float target =
            (isLookLeftPressing == isLookRightPressing) ? 0f
            : (isLookLeftPressing ? -maxYAngle : maxYAngle);

        currentY = Mathf.MoveTowards(currentY, target, rotSpeed * Time.deltaTime);
        GameObject targetPivot = GetActivePivot();
        targetPivot.transform.localRotation = GetBaseRot(targetPivot) * Quaternion.Euler(0, currentY, 0);
    }
    Quaternion GetBaseRot(GameObject pivot)
    {
        if (pivot == firstFrontPivot) { return baseFirstFront; }
        if (pivot == firstBackPivot) { return baseFirstBack; }
        return baseThird;
    }
    GameObject GetActivePivot()
    {
        if (isFirstPerson)
        {
            return isFacingForward ? firstFrontPivot : firstBackPivot;
        }
        return thirdPivot;
    }

    //ここから視点切り替え
    void TogglePerspective()
    {
        isFirstPerson = !isFirstPerson;
        UpdateCameraState();
    }
    void ToggleFacingDirection()
    {
        isFacingForward = !isFacingForward;
        UpdateCameraState();
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
