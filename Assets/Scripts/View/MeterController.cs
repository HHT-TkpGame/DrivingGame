using UnityEditor.ShaderGraph;
using UnityEngine;

public class MeterController
{
    RectTransform needleRect;
    float maxRot = -300f;
    float maxRpm;
    public MeterController(RectTransform needleRect, float maxRpm)
    {
        this.needleRect = needleRect;
        this.maxRpm = maxRpm;
    }
    public void UpdateNeedle(float currentRpm)
    {
        // ílÇ0~1ÇÃäÑçáÇ…ïœä∑
        float t = currentRpm / maxRpm;

        // Zé≤âÒì]Ç…îΩâf
        needleRect.localEulerAngles = new Vector3(
            needleRect.localEulerAngles.x,
            needleRect.localEulerAngles.y,
            t * maxRot
        );
    }
}
