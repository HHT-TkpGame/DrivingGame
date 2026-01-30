using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    public enum MenuItem
    {
        Controls,
		Exit,
		Back
    }

	MenuItem currentMenu;
	int maxMenu;
	bool isVisible=false;

	[SerializeField] Sprite[] infoTexts;
	[SerializeField] RectTransform[] settingUIs;
	[SerializeField] MenuVisual[] settingUIContents;
	[SerializeField] RectTransform cursorUI;
	[SerializeField] GameObject[] menuContents;
	[SerializeField] Sprite[] menuPanelSprites;
	[SerializeField] Sprite[] selectUISprites;
 

	Dictionary<MenuItem, Action> menuActions;

	//menuImageの操作説明が書いてある部分の画像のこと
	//関係があるのはControlsの時のみ
	//それ以外の時は半透明の画像になると思う
	[SerializeField] Image menuImage;
	[SerializeField] Image InfoText;

	Image image;
	Image cursorImage;

	public event Action OnMenuClosed;
	public event Action OnEndDrive;

	private void Awake()
	{
		menuActions = new Dictionary<MenuItem, Action>()
		{
			{ MenuItem.Controls, () => SetControlsImage()},
			{ MenuItem.Exit, () => ClickEndDrive()},
			{ MenuItem.Back, () => ClosedMenu()}
		};
	}

	void ClickEndDrive()
	{
		OnEndDrive?.Invoke();
	}

	public void Initialize()
	{
		currentMenu = MenuItem.Controls;
		maxMenu = Enum.GetValues(typeof(MenuItem)).Length;

		image = GetComponent<Image>();
		cursorImage = cursorUI.gameObject.GetComponent<Image>();

		UpdateMenuContent(currentMenu);
		UpdateCursor(currentMenu);
		UpdateUIContent(currentMenu);
		UpdateInfoText(currentMenu);
		UpdateMenuPanelIllst(currentMenu);
		UpdateSettingSelectUIIllst(currentMenu);

		gameObject.SetActive(false);
		//今いる場所の記録や設定など
	}

	public void OpenMenu()
	{
		//開くだけにして初期値を設定とかを書くとOpenMenu→OpenMenuでリセット注意
		ToggleMenuVisibility();
	}

	void ToggleMenuVisibility()
	{
		isVisible = !isVisible;
		gameObject.SetActive(isVisible);

		if (!isVisible)
		{
			MenuEnd();
		}
	}

	///<summary>
	/// Action OnMenuEnd
	///void menuEnd{
	///onmenuEnd?.invoke
	///
	/// TimeAttackController
	/// 
	/// Action OnMenuClosed(すでにある)
	/// 
	/// menuUIController.OnMenuEnd += menuEnd 
	/// void menuEnd{
	/// OnMenuClosed?.invoke
	/// }
	/// 
	/// 
	/// }
	/// </summary>
	public void MenuArrow(float value)
	{
		if (!isVisible) { return; }


		int nextMenu = (int)currentMenu + (value < 0 ? -1 : 1);

		if (nextMenu < 0)
		{
			nextMenu = maxMenu - 1;
		}
		else if (nextMenu >= maxMenu)
		{
			nextMenu = 0;
		}

		currentMenu = (MenuItem)nextMenu;

		UpdateMenuContent(currentMenu);
		UpdateCursor(currentMenu);
		UpdateUIContent(currentMenu);
		UpdateInfoText(currentMenu);
		UpdateMenuPanelIllst(currentMenu);
		UpdateSettingSelectUIIllst(currentMenu);
	}

	public void ClickAnyUI()
	{
		//isVisibleがTrueなら現在Menuは表示されているということ
		if (!isVisible) { return; }

		menuActions[currentMenu]?.Invoke();
	}
	void UpdateCursor(MenuItem m)
	{
		cursorUI.localPosition = settingUIs[(int)m].localPosition;
	}
	void UpdateUIContent(MenuItem m)
	{
		MenuVisual visual = settingUIContents[(int)m];
		menuImage.sprite = visual.mainSprite;
	}
	void UpdateInfoText(MenuItem m)
	{
		InfoText.sprite = infoTexts[(int)m];
	}
	void UpdateMenuContent(MenuItem m)
	{
		for(int i = 0; i < menuContents.Length; i++)
		{
			menuContents[i].SetActive(i==(int)m);
		}
	}
    void UpdateMenuPanelIllst(MenuItem m)
	{
		image.sprite = menuPanelSprites[(int)m];
	}
	void UpdateSettingSelectUIIllst(MenuItem m)
	{
		cursorImage.sprite = selectUISprites[(int)m];
	} 

	bool controlsActive;
	void SetControlsImage()
	{
		controlsActive = !controlsActive;
		MenuVisual v = settingUIContents[(int)MenuItem.Controls];
		menuImage.sprite = controlsActive ? v.mainSprite : v.subSprite;
	}	


	//戻るボタンが押されたときも呼ぶ
	void MenuEnd()
	{
		OnMenuClosed?.Invoke();
	}

	//menuを閉じるボタンからMenuを閉じたときのメソッド
	void ClosedMenu()
	{
		isVisible = false;
		gameObject.SetActive(false);
		MenuEnd();
	}
}
