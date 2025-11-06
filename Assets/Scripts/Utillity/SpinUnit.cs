using UnityEngine;
public static class SpinUnit
{
    const float RPM_TO_RAD = 2f * Mathf.PI / 60f;
    const float RAD_TO_RPM = 60f / (2f * Mathf.PI);

    public static float RpmToRad(float rpm) => rpm * RPM_TO_RAD;
    public static float RadToRpm(float radPerSec) => radPerSec * RAD_TO_RPM;
}
