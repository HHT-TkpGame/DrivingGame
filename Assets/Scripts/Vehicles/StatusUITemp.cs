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
    //FrictionClutch clutchModel;
    IClutchModel clutchModel;
    ITransmissionModel transmission;
    EngineModel engine;
    VehicleController vehicleController;
    DriveType driveType;
    public void Initialize(
        IClutchModel clutchModel,
        ITransmissionModel transmission,
        EngineModel engine,
        VehicleController vehicleController,
        float maxRpm,
        DriveType driveType
    ){
        this.clutchModel = clutchModel;
        this.transmission = transmission;
        this.engine = engine;
        this.vehicleController = vehicleController;
        meterController = new MeterController(needleRect, maxRpm);
        this.driveType = driveType;
    }
    
    string GetGearLabel()
    {
        // 表示だけの責務なので、ここでラベル変換してOK
        if (driveType == DriveType.Automatic)
        {
            // AT: -1 = R, 0 = N, 1 = D
            return transmission.CurrentGear switch
            {
                -1 => "R",
                0 => "N",
                1 => "D",
                _ => "N" // 想定外は安全側
            };
        }

        // MT: -1 = R, 0 = N, 1.. = 1..
        return transmission.CurrentGear switch
        {
            -1 => "R",
            0 => "N",
            _ => transmission.CurrentGear.ToString()
        };
    }
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        gear.text = $"Gear\n{GetGearLabel()}";
        meterController.UpdateNeedle(engine.CurrentRPM);
        velocity.text = vehicleController.SpeedKPH.ToString("f0");
    }
}
