using UnityEngine;

public class ExecuteTester : MonoBehaviour
{
    [SerializeField] VehicleInputHandler handler;
    [SerializeField] ClutchCurve clutch;
    AnimationCurve curve;
    ClutchModel model;

    void Start()
    {
        curve = clutch.Curve;
        model = new ClutchModel(handler, curve);
    }

    void Update()
    {
        //Debug.Log($"eng: {model.Engagement}");
    }
}
