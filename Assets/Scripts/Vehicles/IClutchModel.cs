using UnityEngine;

public interface IClutchModel
{
    float Engagement { get; }
    void SetEngagement(float clutchInput, bool isPlaying);

    float ComputeTorque(
        float engineTorque,
        float engineOmega,
        float engineInertia,
        float loadTorque,
        float transInputOmega,
        float vehicleInertia,
        float deltaTime
    );
}
