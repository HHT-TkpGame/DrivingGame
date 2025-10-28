using UnityEngine;

/// <summary>
/// クラッチの繋がり具合（0～1）に応じて Engine と Transmission を接続
/// </summary>
public class ClutchModel
{
    VehicleInputHandler inputHandler;
    //ここに入る値がInputHandlerから渡される入力値
    public float Engagement { get { return inputHandler.ClutchAxis; }  } //クラッチの繋がり具合
    public ClutchModel(VehicleInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
    }
}
