using UnityEngine;

public class ATTransmission : ITransmissionModel
{
    readonly float driveRatio;
    readonly float reverseRatio;
    readonly float finalDriveRatio;

    readonly float maxForwardKph;
    readonly float maxReverseKph;

    // -1:R, 0:N, 1:D
    public int CurrentGear { get; private set; } = 0;

    public float CurrentRatio
    {
        get
        {
            float baseRatio = CurrentGear switch
            {
                1 => driveRatio,
                -1 => reverseRatio,
                _ => 0f
            };
            return baseRatio * finalDriveRatio;
        }
    }

    public ATTransmission(
        float driveRatio,
        float reverseRatio,
        float finalDriveRatio,
        float maxForwardKph,
        float maxReverseKph
    )
    {
        this.driveRatio = driveRatio;
        this.reverseRatio = reverseRatio;
        this.finalDriveRatio = finalDriveRatio;
        this.maxForwardKph = Mathf.Max(0f, maxForwardKph);
        this.maxReverseKph = Mathf.Max(0f, maxReverseKph);
    }

    public void SetGear(int newGear)
    {
        // 入力は -1/0/1 だけ採用（それ以外は無視）
        if (newGear < -1 || newGear > 1) return;
        CurrentGear = newGear;
    }

    // 必要なら将来ここでDレンジ中の挙動拡張（疑似変速など）
    public void Tick(float speedKph, float deltaTime) { }

    public float GetDriveTorqueScale(float speedKph)
    {
        if (CurrentGear == 0) return 0f;

        float limit = (CurrentGear == 1) ? maxForwardKph : maxReverseKph;
        if (limit <= 0.01f) return 1f;

        // 上限に近づいたら滑らかに絞る（急に0にすると挙動が硬くなる）
        // 例：limitの95%から絞り始め、limitで0になる
        float start = limit * 0.95f;
        if (speedKph <= start) return 1f;
        if (speedKph >= limit) return 0f;

        return Mathf.InverseLerp(limit, start, speedKph);
    }
}
