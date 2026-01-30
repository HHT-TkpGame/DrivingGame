using UnityEngine;

[CreateAssetMenu(fileName = "SaveCarRGB", menuName = "Scriptable Objects/SaveCarRGB")]
public class SaveCarRGB : ScriptableObject
{
    [SerializeField]Color carColor;
    public Color CarColor=>carColor;
    public void SetCarColor(Color c)
    {
        carColor = c;
    }
}
