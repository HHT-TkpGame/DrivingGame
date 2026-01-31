using UnityEngine;

[CreateAssetMenu(fileName = "SaveCarRGB", menuName = "Scriptable Objects/SaveCarRGB")]
public class SaveCarRGB : ScriptableObject
{
    Color carColor;
    public Color CarColor=>carColor;
    public void SetCarColor(Color c)
    {
        carColor = c;
    }
}
