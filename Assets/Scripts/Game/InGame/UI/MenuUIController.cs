using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEditor.Timeline;
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

	[SerializeField] RectTransform[] settingUIs;
	[SerializeField] GameObject[] settingUIContents;
	[SerializeField] RectTransform cursorUI;
	Dictionary<MenuItem, Action> menuActions;

	private void Awake()
	{
		menuActions = new Dictionary<MenuItem, Action>()
		{
			{ MenuItem.Controls, () => Debug.Log("コントロールでクリックされた")},
			{ MenuItem.Exit, () => Debug.Log("ゲーム終了")},
			//{ MenuItem.Back, () => Debug.Log("バックでクリック")},
			{ MenuItem.Back, () => ClosedMenu()}
		};
	}

	public void Initialize()
	{
		currentMenu = MenuItem.Controls;
		maxMenu = Enum.GetValues(typeof(MenuItem)).Length;
		UpdateCursor(currentMenu);
		UpdateUIContent(currentMenu);

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

		UpdateCursor(currentMenu);
		UpdateUIContent(currentMenu);
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
		for (int i = 0; i < settingUIContents.Length; i++)
		{
			settingUIContents[i].SetActive(i == (int)m);
		}
	}

	public event Action OnMenuClosed;

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
