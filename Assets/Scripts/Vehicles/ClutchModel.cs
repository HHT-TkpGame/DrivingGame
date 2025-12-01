
using UnityEngine;
/// <summary>
/// クラッチの繋がり具合（0～1）に応じて Engine と Transmission を接続
/// </summary>
public class ClutchModel
{
    VehicleInputHandler inputHandler;
    AnimationCurve clutchCurve;
    //クラッチの接続状態(1が接続, 0が切断)
    //InputHandlerからの入力は押されていない時が0を返す
    //本来は入力がない時に接続状態が1になるべきなので値を反対にする
    public float Engagement { 
        get 
        {
            float raw = 1 - inputHandler.ClutchAxis;
            Debug.Log(raw);
            return clutchCurve.Evaluate(raw); 
        }
    } 
    public ClutchModel(VehicleInputHandler inputHandler, AnimationCurve clutchCurve)
    {
        this.inputHandler = inputHandler;
        this.clutchCurve = clutchCurve;
    }
}
