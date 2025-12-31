using UnityEngine;

public class ExecuteTester : MonoBehaviour
{
    [SerializeField] VehicleInputHandler handler;
    [SerializeField] ClutchCurve clutch;
    AnimationCurve curve;
    

    void Start()
    {
        curve = clutch.Curve;
    }

    void Update()
    {
        //Debug.Log($"eng: {model.Engagement}");
    }
}
