using System;
using UnityEngine;
using UnityEngine.UI;

public class CarColorSetter : MonoBehaviour
{
	[SerializeField] Slider[] scrollbarObj;
	[SerializeField] Transform sliderCursor;

	Transform[] scrollbarPositions;

	[SerializeField] Material bodyMaterial;

	//[SerializeField] SaveCarRGB carRGB;

	//最初のオレンジ
	Color defaultColor = new Color(0.766f, 0.445f,0,1);
	Color carColor;

	int maxItems;
	public enum ScrollBarItem
	{
		RedBar,
		GreenBar,
		BlueBar,
	}

	ScrollBarItem currentSetting=ScrollBarItem.RedBar;


	//inputからのメソッドにもする
	public void SetStandardColor()
	{
		bodyMaterial.color = defaultColor;
		scrollbarObj[0].value = defaultColor.r;
		scrollbarObj[1].value = defaultColor.g;
		scrollbarObj[2].value = defaultColor.b;
		carColor = defaultColor;
	}

	private void Start()
	{
		SetStandardColor();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void Initialize()
    {
		//スタートからステートが変わってからのメソッドにする

		
		scrollbarPositions=new Transform[scrollbarObj.Length];
		maxItems = Enum.GetValues(typeof(ScrollBarItem)).Length;

		for(int i = 0; i < scrollbarObj.Length;i++)
		{
			scrollbarPositions[i] = scrollbarObj[i].gameObject.transform;
		}

		UpdateCursor(currentSetting);

	}
	void UpdateCursor(ScrollBarItem m)
	{
		sliderCursor.localPosition = scrollbarPositions[(int)m].localPosition;
	}

	public void CursorLift(float f)
	{
		//カーソルとステートの変更
		//Debug.Log("リフトした"+f);

		int nextMenu = (int)currentSetting + (f < 0 ? -1 : 1);

		if (nextMenu < 0)
		{
			nextMenu = maxItems - 1;
		}
		else if (nextMenu >= maxItems)
		{
			nextMenu = 0;
		}

		currentSetting = (ScrollBarItem)nextMenu;

		UpdateCursor(currentSetting);
	}

	public void MoveSlider(float f)
	{
		scrollbarObj[(int)currentSetting].value += f;
		SetCarColor(scrollbarObj);
	}

	void SetCarColor(Slider[] slider)
	{
		Color c = new Color(scrollbarObj[0].value, scrollbarObj[1].value, scrollbarObj[2].value, 1);
		bodyMaterial.color = c;
		carColor = c;
	}

	//スペース押されたときこれ
	public void SaveCarColor()
	{
		//carRGB.SetCarColor(carColor);
	}
	///メソッドで
	//carRGB.SetCarColor(c);を保管

}


