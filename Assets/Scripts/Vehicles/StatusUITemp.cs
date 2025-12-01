using TMPro;
using UnityEngine;

public class StatusUITemp : MonoBehaviour
{
    [SerializeField] TMP_Text rpm;
    [SerializeField] TMP_Text gear;
    [SerializeField] TMP_Text torque;
    [SerializeField] TMP_Text clutch;
    [SerializeField] TMP_Text velocity;
    [SerializeField] RectTransform needleRect;
    MeterController meterController;
    ClutchModel clutchModel;
    TransmissionModel transmission;
    EngineModel engine;
    VehicleController vehicleController;
    public void Initialize(
        ClutchModel clutchModel,
        TransmissionModel transmission,
        EngineModel engine,
        VehicleController vehicleController,
        float maxRpm
    ){
        this.clutchModel = clutchModel;
        this.transmission = transmission;
        this.engine = engine;
        this.vehicleController = vehicleController;
        meterController = new MeterController(needleRect, maxRpm);
    }
    string[] gears = { "R", "N", "1", "2", "3", "4", "5", "6" };
    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        rpm.text = $"Rpm : {engine.CurrentRPM}";
        gear.text = $"Gear\n{gears[transmission.CurrentGear+1]}";
        torque.text = $"Torque : {engine.OutputTorque.ToString("f1")}";
        clutch.text = $"Engagement : {clutchModel.Engagement}";
        meterController.UpdateNeedle(engine.CurrentRPM);
        velocity.text = vehicleController.SpeedKPH.ToString("f1");
    }
}
