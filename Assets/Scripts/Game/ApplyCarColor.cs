using UnityEngine;

public class ApplyCarColor : MonoBehaviour
{
	[SerializeField] SaveCarRGB carScriptable;
	[SerializeField] Material carColor;
    private void Start()
    {
		SetCarColor();
    }
    public void SetCarColor()
	{
		//carScriptable.SetCarColor(carColor);
		carColor.color = carScriptable.CarColor;
		
	}
}
