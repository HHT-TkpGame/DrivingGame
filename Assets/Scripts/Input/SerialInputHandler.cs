using System;
using System.IO.Ports;
using UnityEngine;

/// <summary>
/// シフトレバーとのシリアル通信を扱う
/// </summary>
public class SerialInputHandler : MonoBehaviour
{
    const string PORT_NAME = "COM4";
    int baudRate = 115200;
    SerialPort port;
    const int SENSOR_COUNT = 7;
    bool[] sensorState = new bool[SENSOR_COUNT];
    //VehicleInputHandlerに購読してもらう
    public event Action<int> OnSensorStateChanged;
    int currentGear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //port = new SerialPort(PORT_NAME, baudRate);
        //port.ReadTimeout = 50;
        //port.DtrEnable = true;
        //port.Open();
        try
        {
            port = new SerialPort(PORT_NAME, baudRate)
            {
                ReadTimeout = 50,
                DtrEnable = true
            };

            port.Open();
            Debug.Log("Serial port opened");
        }
        catch (Exception e)
        {
            Debug.Log($"Serial open failed: {e.Message}");
            port = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (port == null || !port.IsOpen)
            return;

        try
        {
            string line = port.ReadLine(); // Picoからの1行 "0" or "1"
            if (int.TryParse(line, out int value))
            {
                //Debug.Log($"val {value}");
                UnpackBits(value);
            }
        }
        catch (TimeoutException)
        {
            //Debug.Log("timeout");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    void OnDestroy()
    {
        if (port != null && port.IsOpen)
            port.Close();
    }
    void UnpackBits(int v)
    {
        for (int i = 0; i < SENSOR_COUNT; i++)
        {
            bool b = ((v >> i) & 1) == 1;
            if(b != sensorState[i])
            {
                sensorState[i] = b;
                Debug.Log($"{i}:{sensorState[i]}");
            }
        }
        ChangeGear();
    }
    void ChangeGear()
    {
        int detectedGear = 0;
        for (int i = 0; i < SENSOR_COUNT; i++)
        {
            if (!sensorState[i])
            {
                continue;
            }
            detectedGear = SensorIndexToGear(i);
            break;//最初に見つかったものでbreak;
        }
        if(detectedGear == currentGear)
        {
            return;
        }
        currentGear = detectedGear;
        OnSensorStateChanged?.Invoke(currentGear);
    }
    int SensorIndexToGear(int index)
    {
        return index switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 6,
            6 => -1,
            _ => 0
        };
    }
}
