using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
    VehicleController vehicle;   // 車の状態取得（RPM/Throttle）
    CarSpec spec;                 // Idle/MaxRPM参照用（なければ手入力でも可）
    EngineModel engineModel;
    VehicleInputHandler inputHandler;

    [SerializeField] AudioSource lowSource;        // 低音（基音ループ）
    [SerializeField] AudioSource highSource;       // 高音（鋭さループ）
    [SerializeField] AudioSource starterSe;

    [SerializeField] float minPitch = 0.85f;
    [SerializeField] float maxPitch = 2.0f;

    [SerializeField] float referenceRpm = 4000f;

    [SerializeField] float lowMaxVolume = 1.3f;
    [SerializeField] float highMaxVolume = 0.4f;

    [Header("HF Emphasis")]
    [Tooltip("高音が出始めるRPM正規化（0..1）")]
    [SerializeField] float highStartRpm01 = 0.25f;

    [Tooltip("高音が強くなるRPM正規化（0..1）")]
    [SerializeField] float highFullRpm01 = 0.9f;

    [Tooltip("高音は負荷（スロットル）でどれだけ増えるか")]
    [SerializeField] float highLoadBoost = 1.0f;

    [Header("Smoothing")]
    [SerializeField] float rpmSmoothTime = 0.06f;
    [SerializeField] float throttleSmoothTime = 0.08f;

    float rpm01Smoothed;
    float rpm01Vel;
    float throttleSmoothed;
    float throttleVel;

    float idleRpm;
    float maxRpm;
    public void Initialize(
        VehicleController vehicleController,
        CarSpec spec,
        EngineModel engineModel,
        VehicleInputHandler inputHandler
    ){
        this.vehicle = vehicleController;
        this.spec = spec;
        this.engineModel = engineModel;
        this.inputHandler = inputHandler;
        idleRpm = spec.IdleRPM;
        maxRpm = spec.MaxRPM;
        engineModel.OnEngineStarted += PlayStarterSe;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Update()
    {
        if (vehicle == null) { return; }

        // 音に必要な最小情報
        float rpm = Mathf.Max(engineModel.CurrentRPM, 0f);
        float throttle01 = Mathf.Clamp01(inputHandler.AccelerationAxis);

        // RPMを0..1化（アイドル～レッドライン）
        float rpm01 = Mathf.InverseLerp(idleRpm, maxRpm, rpm);

        // ガタつき防止（音はUpdateで平滑化した方が自然）
        rpm01Smoothed = Mathf.SmoothDamp(rpm01Smoothed, rpm01, ref rpm01Vel, rpmSmoothTime);
        throttleSmoothed = Mathf.SmoothDamp(throttleSmoothed, throttle01, ref throttleVel, throttleSmoothTime);

        ApplyPitch(rpm);
        ApplyVolumes(rpm01Smoothed, throttleSmoothed);
    }

    void ApplyPitch(float rpm)
    {
        // 正規化→ピッチ
        float rpm01 = Mathf.InverseLerp(idleRpm, maxRpm, rpm);
        float pitch = Mathf.Lerp(minPitch, maxPitch, rpm01);

        // referenceRpmでpitch~1になるよう補正（素材が特定回転で録られている場合に有効）
        float ref01 = Mathf.InverseLerp(idleRpm, maxRpm, referenceRpm);
        float refPitch = Mathf.Lerp(minPitch, maxPitch, ref01);
        float corrected = pitch / Mathf.Max(0.0001f, refPitch);

        if (lowSource != null) lowSource.pitch = corrected;
        if (highSource != null) highSource.pitch = corrected;
    }

    void ApplyVolumes(float rpm01, float throttle01)
    {
        float rpm = Mathf.Max(engineModel.CurrentRPM, 0f);
        if (rpm <= 10f)
        {
            if (lowSource != null) lowSource.volume = 0f;
            if (highSource != null) highSource.volume = 0f;
            return;
        }

        float lowRpmGain = Mathf.SmoothStep(0.0f, 1.0f, rpm01);

        float lowIdleFloor = 0.4f;

        float lowVol = lowMaxVolume * Mathf.Lerp(lowIdleFloor, 1.0f, lowRpmGain);

        float hfRpmGain = Mathf.InverseLerp(highStartRpm01, highFullRpm01, rpm01);
        hfRpmGain = Mathf.Clamp01(hfRpmGain);

        float hfLoadGain = Mathf.Clamp01(throttle01 * highLoadBoost);

        float highVol = highMaxVolume * hfRpmGain * Mathf.Lerp(0.2f, 1.0f, hfLoadGain);

        if (lowSource != null) lowSource.volume = lowVol;
        if (highSource != null) highSource.volume = highVol;
    }
    void PlayStarterSe()
    {
        starterSe.Play();
    }
}
