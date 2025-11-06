using UnityEngine;

public class ExecuteTester : MonoBehaviour
{
    [SerializeField] CarSpec carSpec;
    EngineModel engine;
    void Start()
    {
        // エンジン生成
        engine = new EngineModel(carSpec);
    }

    void Update()
    {
        // テスト条件
        float throttle = 0.3f;      // スロットル開度（30%）
        float externalTorque = 50f; // 仮の抵抗トルク [Nm]
        float deltaTime = Time.deltaTime;

        // エンジントルクを更新
        engine.UpdateTorque(throttle);

        // 外部トルク（負荷）を適用
        engine.ApplyExternalTorque(externalTorque, deltaTime);

        // 状態をログ出力
        Debug.Log($"RPM: {engine.CurrentRPM:F1}, Torque: {engine.OutputTorque:F1}");
    }
}
