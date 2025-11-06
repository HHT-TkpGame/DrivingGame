using TMPro;
using UnityEngine;

public class StatusUITemp : MonoBehaviour
{
    [SerializeField] TMP_Text rpm;
    [SerializeField] TMP_Text gear;
    [SerializeField] TMP_Text torque;
    [SerializeField] TMP_Text clutch;
    [SerializeField] TMP_Text velocity;
    ClutchModel clutchModel;
    TransmissionModel transmission;
    EngineModel engine;

    public void Initialize(
        ClutchModel clutchModel,
        TransmissionModel transmission,
        EngineModel engine
    ){
        this.clutchModel = clutchModel;
        this.transmission = transmission;
        this.engine = engine;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        rpm.text = $"Rpm : {engine.CurrentRPM}";
        gear.text = $"GearRatio : {transmission.CurrentGearRatio}";
        torque.text = $"Torque : {engine.OutputTorque}";
        clutch.text = $"Engagement : {clutchModel.Engagement}";
    }
}
