using UnityEngine;
[CreateAssetMenu(fileName = "NewClutchCurve", menuName = "CarSpecs/ClutchCurve")]
public class ClutchCurve : ScriptableObject
{
    [SerializeField] AnimationCurve curve;
    public AnimationCurve Curve => curve;
}
