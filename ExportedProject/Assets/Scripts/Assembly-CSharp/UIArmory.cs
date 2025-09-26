using UnityEngine;

public class UIArmory : UIMessageHandler
{
	public UIArmory_Container armoryContainer;

	public Transform playerOnShowTrs;

	private void Awake()
	{
		if (!(armoryContainer == null))
		{
			return;
		}
		Debug.Log("armoryContainer==null");
		GameObject gameObject = GameObject.Find("ArmoryContainer");
		if (gameObject != null)
		{
			armoryContainer = gameObject.GetComponent<UIArmory_Container>();
			if (armoryContainer == null)
			{
				armoryContainer = gameObject.AddComponent<UIArmory_Container>();
			}
		}
	}

	public void HandleEventButtonWeapon(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 1)
		{
			return;
		}
		Debug.Log("Button_Weapon-CommandSelect");
		if (armoryContainer != null)
		{
			armoryContainer.ExitContainer();
			armoryContainer.CreateBox(10, 0);
			for (int i = 0; i < 10; i++)
			{
				Random.seed = ((int)Time.timeSinceLevelLoad + i) * 10245;
				UIArmory_WeaponBoxData boxData = new UIArmory_WeaponBoxData();
				armoryContainer.AddBoxData(i, boxData);
			}
			armoryContainer.RefreshContainer();
		}
	}

	public void HandleEventButtonHelmet(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 1)
		{
			return;
		}
		Debug.Log("Button_Helmet-CommandSelect");
		if (armoryContainer != null)
		{
			armoryContainer.ExitContainer();
			armoryContainer.CreateBox(8, 1);
			for (int i = 0; i < 8; i++)
			{
				Random.seed = ((int)Time.timeSinceLevelLoad + i) * 20245;
				UIArmory_HelmentBoxData boxData = new UIArmory_HelmentBoxData();
				armoryContainer.AddBoxData(i, boxData);
			}
			armoryContainer.RefreshContainer();
		}
	}

	public void HandleEventButtonPaint(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 1)
		{
			return;
		}
		Debug.Log("Button_Paint-CommandSelect");
		if (armoryContainer != null)
		{
			armoryContainer.ExitContainer();
			armoryContainer.CreateBox(6, 2);
			for (int i = 0; i < 6; i++)
			{
				Random.seed = ((int)Time.timeSinceLevelLoad + i) * 30245;
				UIArmory_PaintBoxData uIArmory_PaintBoxData = new UIArmory_PaintBoxData();
				uIArmory_PaintBoxData.PaintColor = COMA_Color.colors[i + 2];
				armoryContainer.AddBoxData(i, uIArmory_PaintBoxData);
			}
			armoryContainer.RefreshContainer();
		}
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_back-CommandClick");
			if (_fadeMgr != null)
			{
				_fadeMgr.FadeOut();
			}
			_aniControl.PlayExitAni("UI.MainMenu");
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.Armory", _aniControl);
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void ProcessWeaponBuy(TUIControl control, int eventType, float wparam, float lparam, object data, UI_BoxSlot boxSlot)
	{
		UIArmory_WeaponBoxSlot uIArmory_WeaponBoxSlot = (UIArmory_WeaponBoxSlot)boxSlot;
		((UIArmory_WeaponBoxData)uIArmory_WeaponBoxSlot.BoxData).OwnNum++;
	}

	public void ProcessHelmetBuy(TUIControl control, int eventType, float wparam, float lparam, object data, UI_BoxSlot boxSlot)
	{
		UIArmory_HelmentBoxSlot uIArmory_HelmentBoxSlot = (UIArmory_HelmentBoxSlot)boxSlot;
		UIArmory_HelmentBoxData uIArmory_HelmentBoxData = (UIArmory_HelmentBoxData)uIArmory_HelmentBoxSlot.BoxData;
		bool flag = false;
		if (!COMA_Pref.Instance.IsPackageFull())
		{
			if (uIArmory_HelmentBoxData.PurchaseType == 0)
			{
				if ((float)COMA_Pref.Instance.GetCrystal() >= uIArmory_HelmentBoxData.Price)
				{
					COMA_Pref.Instance.AddCrystal(-Mathf.FloorToInt(uIArmory_HelmentBoxData.Price));
					COMA_Pref.Instance.Save(true);
					flag = true;
				}
			}
			else if (uIArmory_HelmentBoxData.PurchaseType == 1 && (float)COMA_Pref.Instance.GetGold() >= uIArmory_HelmentBoxData.Price)
			{
				COMA_Pref.Instance.AddGold(-Mathf.FloorToInt(uIArmory_HelmentBoxData.Price));
				COMA_Pref.Instance.Save(true);
				flag = true;
			}
		}
		if (flag)
		{
			Debug.Log(uIArmory_HelmentBoxData.Serial);
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = uIArmory_HelmentBoxData.Serial;
			cOMA_PackageItem.itemName = uIArmory_HelmentBoxData.Name;
			cOMA_PackageItem.num = uIArmory_HelmentBoxData.Num;
			cOMA_PackageItem.part = uIArmory_HelmentBoxData.Part;
			cOMA_PackageItem.hpAdd = uIArmory_HelmentBoxData.HP;
			cOMA_PackageItem.spdAdd = uIArmory_HelmentBoxData.AGI;
			if (cOMA_PackageItem.part <= 0)
			{
				cOMA_PackageItem.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem.serialName);
				cOMA_PackageItem.LoadPNG();
			}
			cOMA_PackageItem.CreateIconTexture();
			cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
		}
	}

	public void ProcessPaintBuy(TUIControl control, int eventType, float wparam, float lparam, object data, UI_BoxSlot boxSlot)
	{
		UIArmory_PaintBoxSlot uIArmory_PaintBoxSlot = (UIArmory_PaintBoxSlot)boxSlot;
		((UIArmory_PaintBoxData)uIArmory_PaintBoxSlot.BoxData).OwnNum++;
	}

	public void HandleEventButton_PlayerRotate(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		float y = (0f - wparam) * COMA_Sys.Instance.sensitivity * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		playerOnShowTrs.rotation *= quaternion;
	}
}
