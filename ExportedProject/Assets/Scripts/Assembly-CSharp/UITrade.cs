using System.Collections;
using UnityEngine;

public class UITrade : UIMessageHandler
{
	private static UITrade _instance;

	public UITrade_SellContainer sellContainer;

	public UITrade_BuyContainer buyContainer;

	[SerializeField]
	private UITrade_AuctionBox auctionBox;

	public COMA_PlayerSelfCharacter playerOnShowCom;

	private int framesToLate;

	private UITrade_RefreshEffect refreshEffect;

	[SerializeField]
	private GameObject _objRefreshBtn;

	private UITrade_SellBoxSlot curSlot;

	private UITrade_EquipedBoxSlot curSel;

	private UITrade_EquipedBoxSlot preSel;

	public static UITrade Instance
	{
		get
		{
			return _instance;
		}
	}

	public UITrade_EquipedBoxSlot CurSelSlot
	{
		get
		{
			return curSel;
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	private void Awake()
	{
		if (sellContainer == null)
		{
			Debug.Log("sellContainer==null");
			GameObject gameObject = GameObject.Find("SellContainer");
			if (gameObject != null)
			{
				sellContainer = gameObject.GetComponent<UITrade_SellContainer>();
				if (sellContainer == null)
				{
					sellContainer = gameObject.AddComponent<UITrade_SellContainer>();
				}
			}
		}
		if (!(buyContainer == null))
		{
			return;
		}
		Debug.Log("buyContainer==null");
		GameObject gameObject2 = GameObject.Find("BuyContainer");
		if (gameObject2 != null)
		{
			buyContainer = gameObject2.GetComponent<UITrade_BuyContainer>();
			if (buyContainer == null)
			{
				buyContainer = gameObject2.AddComponent<UITrade_BuyContainer>();
			}
		}
	}

	private void Start()
	{
		ProceSelBuy();
		RefreshGoldAndCrystal();
		if (COMA_HTTP_TextureManager.Instance != null && COMA_TexBuyBuffer.Instance.items.Count <= 0)
		{
			RequestItemsOnBuyShell();
		}
	}

	private new void RefreshGoldAndCrystal()
	{
		if (_goldLabel != null)
		{
			_goldLabel.Text = COMA_Pref.Instance.GetGold().ToString();
		}
		if (_gemLabel != null)
		{
			_gemLabel.Text = COMA_Pref.Instance.GetCrystal().ToString();
		}
	}

	private void RequestItemsOnBuyShell()
	{
	}

	public void SetItemInfoOnShell(Texture2D tex, int kind, int gold, int num, float leftTime, bool isOfficial)
	{
		if (kind < 0)
		{
			Debug.LogWarning("kind must be 0 - 3 : " + kind);
			return;
		}
		string fileName = string.Empty;
		switch (kind)
		{
		case 0:
			fileName = "Head01";
			break;
		case 1:
			fileName = "Body01";
			break;
		case 2:
			fileName = "Leg01";
			break;
		}
		StartCoroutine(SetItemInfoOnShell(tex, fileName, gold, num, leftTime, isOfficial));
	}

	public IEnumerator SetItemInfoOnShell(Texture2D tex, string fileName, int gold, int num, float leftTime, bool isOfficial)
	{
		for (int i = 0; i < framesToLate; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		framesToLate++;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/" + fileName)) as GameObject;
		tarObj.transform.GetChild(0).renderer.material.mainTexture = tex;
		Texture2D iconTexture = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		COMA_ItemInTradeHall item = new COMA_ItemInTradeHall
		{
			isOfficial = isOfficial,
			serialName = fileName,
			gold = gold,
			num = num,
			leftTime = leftTime,
			texture = tex,
			iconTexture = iconTexture
		};
		if (item.isOfficial)
		{
			COMA_TexBuyBuffer.Instance.items.Insert(0, item);
			for (int j = 0; j < COMA_TexBuyBuffer.Instance.items.Count; j++)
			{
				UITrade_BuyBoxData data = (UITrade_BuyBoxData)buyContainer.LstBoxDatas[j];
				data.Price = COMA_TexBuyBuffer.Instance.items[j].gold;
				UITrade_BuyBoxSlot slot = (UITrade_BuyBoxSlot)data.Ower;
				slot.SetIconPic(COMA_TexBuyBuffer.Instance.items[j].iconTexture);
			}
		}
		else
		{
			COMA_TexBuyBuffer.Instance.items.Add(item);
			UITrade_BuyBoxData data2 = (UITrade_BuyBoxData)buyContainer.LstBoxDatas[COMA_TexBuyBuffer.Instance.items.Count - 1];
			data2.Price = gold;
			UITrade_BuyBoxSlot slot2 = (UITrade_BuyBoxSlot)data2.Ower;
			slot2.SetIconPic(iconTexture);
		}
		if (COMA_TexBuyBuffer.Instance.items.Count >= 15)
		{
			ReadyToRefresh();
		}
		yield return 0;
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.Trade", _aniControl);
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	private void ProceSelBuy()
	{
		sellContainer.ExitContainer();
		sellContainer.gameObject.SetActiveRecursively(false);
		buyContainer.gameObject.SetActiveRecursively(true);
		_objRefreshBtn.SetActive(true);
		int num = 5;
		int num2 = buyContainer.CreateBox(num, 0);
		Debug.Log("Trade Data Num: " + num2);
		for (int i = 0; i < num; i++)
		{
			UI_Box uI_Box = buyContainer.LstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				if (!uI_BoxSlot.IsLocked())
				{
					UITrade_BuyBoxData uITrade_BuyBoxData = new UITrade_BuyBoxData();
					buyContainer.AddBoxData(uI_BoxSlot.GetID(), uITrade_BuyBoxData);
					int num3 = i * uI_Box.Slots.Length + j;
					if (num3 < COMA_TexBuyBuffer.Instance.items.Count)
					{
						uITrade_BuyBoxData.Price = COMA_TexBuyBuffer.Instance.items[num3].gold;
						UITrade_BuyBoxSlot uITrade_BuyBoxSlot = (UITrade_BuyBoxSlot)uI_BoxSlot;
						uITrade_BuyBoxSlot.SetIconPic(COMA_TexBuyBuffer.Instance.items[num3].iconTexture);
					}
				}
			}
		}
		buyContainer.RefreshContainer();
	}

	private void ProceSelSell()
	{
		int i = 0;
		buyContainer.ExitContainer();
		sellContainer.gameObject.SetActiveRecursively(true);
		buyContainer.gameObject.SetActiveRecursively(false);
		_objRefreshBtn.SetActive(false);
		_objRefreshBtn.GetComponent<UITrade_RefreshEffect>().EndRefresh();
		int num = 4;
		int num2 = sellContainer.CreateBox(num, 0);
		Debug.Log("Trade Data Num: " + num2);
		for (int j = 0; j < num; j++)
		{
			UI_Box uI_Box = sellContainer.LstBoxs[j];
			for (int k = 0; k < uI_Box.Slots.Length; k++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[k];
				if (uI_BoxSlot.IsLocked())
				{
					continue;
				}
				UITrade_SellBoxData uITrade_SellBoxData = new UITrade_SellBoxData();
				sellContainer.AddBoxData(uI_BoxSlot.GetID(), uITrade_SellBoxData);
				int num3 = j * uI_Box.Slots.Length + k;
				for (; i < COMA_Pref.Instance.package.pack.Length; i++)
				{
					if (COMA_Pref.Instance.package.pack[i] != null && COMA_Pref.Instance.package.pack[i].state == COMA_PackageItem.PackageItemStatus.Sale)
					{
						Debug.Log(uI_BoxSlot.gameObject.GetInstanceID() + " " + uI_BoxSlot.GetID());
						UITrade_SellBoxSlot uITrade_SellBoxSlot = (UITrade_SellBoxSlot)uI_BoxSlot;
						uITrade_SellBoxSlot.SetIconPic(COMA_Pref.Instance.package.pack[i].iconTexture);
						uITrade_SellBoxData.DoodleState = UITrade_SellBoxData.EDoodleState.Selling;
						COMA_HTTP_TextureManager.Instance.TextureSell_LeftTimeFromServer(COMA_Pref.Instance.package.pack[i].tid, uITrade_SellBoxSlot.GetID());
						i++;
						break;
					}
				}
			}
		}
		sellContainer.RefreshContainer();
	}

	public void UpdateSlotToSellShell(int slotID, int gold, int num, float leftTime)
	{
		int num2 = sellContainer.LstBoxs[0].Slots.Length;
		int index = slotID / num2;
		int num3 = slotID % num2;
		UI_Box uI_Box = sellContainer.LstBoxs[index];
		UITrade_SellBoxSlot uITrade_SellBoxSlot = (UITrade_SellBoxSlot)uI_Box.Slots[num3];
		UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)uITrade_SellBoxSlot.BoxData;
		Debug.Log("slotID:" + slotID + "  gold:" + gold + "  num:" + num + "  leftTime:" + leftTime);
	}

	public void HandleEventButtonBack(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonBack-CommandClick");
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

	public void HandleEventButtonBuy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			Debug.Log("Button_Buy-CommandSelect");
			ProceSelBuy();
		}
	}

	public void HandleEventButtonSell(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			Debug.Log("Button_Sell-CommandSelect");
			ProceSelSell();
		}
	}

	public void ProcessAvatarShopBuy(TUIControl control, int eventType, float wparam, float lparam, object data, UITrade_BuyBoxSlot slot)
	{
		UITrade_BuyBoxData uITrade_BuyBoxData = (UITrade_BuyBoxData)slot.BoxData;
		if (uITrade_BuyBoxData.Price > 0)
		{
			if (COMA_Pref.Instance.GetGold() < uITrade_BuyBoxData.Price)
			{
				UI_MsgBox uI_MsgBox = MessageBox("Lack of gold", "Need More?");
				uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
				return;
			}
			COMA_Pref.Instance.AddGold(-uITrade_BuyBoxData.Price);
			RefreshGoldAndCrystal();
			Debug.Log(slot.GetID());
			COMA_ItemInTradeHall cOMA_ItemInTradeHall = COMA_TexBuyBuffer.Instance.items[slot.GetID()];
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = cOMA_ItemInTradeHall.serialName;
			cOMA_PackageItem.itemName = string.Empty;
			cOMA_PackageItem.num = 0;
			cOMA_PackageItem.part = 0;
			cOMA_PackageItem.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem.serialName);
			cOMA_PackageItem.texture = cOMA_ItemInTradeHall.texture;
			cOMA_PackageItem.SavePNG();
			cOMA_PackageItem.iconTexture = cOMA_ItemInTradeHall.iconTexture;
			cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
			COMA_Pref.Instance.Save(true);
		}
	}

	public void ProcessAvatarShopPreview(TUIControl control, int eventType, float wparam, float lparam, object data, UITrade_BuyBoxSlot slot)
	{
		Debug.Log("ProcessAvatarShopPreview-CommandClick");
		UITrade_BuyBoxData uITrade_BuyBoxData = (UITrade_BuyBoxData)slot.BoxData;
		if (uITrade_BuyBoxData != null)
		{
			Debug.Log(slot.GetID());
			if (COMA_TexBuyBuffer.Instance.items[slot.GetID()].serialName.StartsWith("Head"))
			{
				playerOnShowCom.bodyObjs[0].renderer.material.mainTexture = COMA_TexBuyBuffer.Instance.items[slot.GetID()].texture;
			}
			else if (COMA_TexBuyBuffer.Instance.items[slot.GetID()].serialName.StartsWith("Body"))
			{
				playerOnShowCom.bodyObjs[1].renderer.material.mainTexture = COMA_TexBuyBuffer.Instance.items[slot.GetID()].texture;
			}
			else if (COMA_TexBuyBuffer.Instance.items[slot.GetID()].serialName.StartsWith("Leg"))
			{
				playerOnShowCom.bodyObjs[2].renderer.material.mainTexture = COMA_TexBuyBuffer.Instance.items[slot.GetID()].texture;
			}
			else
			{
				Debug.LogError("Can't be!!");
			}
		}
	}

	public void NeedMoreCoin(string param)
	{
	}

	public void ProcessAvatarShopSellButton_gold(TUIControl control, int eventType, float wparam, float lparam, object data, UITrade_SellBoxSlot slot)
	{
		UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)slot.BoxData;
		if (uITrade_SellBoxData != null)
		{
			uITrade_SellBoxData.DoodleState = UITrade_SellBoxData.EDoodleState.Blank;
		}
	}

	public void ProcessAvatarShopSellButton_Slot(TUIControl control, int eventType, float wparam, float lparam, object data, UITrade_SellBoxSlot slot)
	{
		curSlot = slot;
		UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)slot.BoxData;
		if (slot.IsLocked())
		{
			UI_MsgBox uI_MsgBox = MessageBox("Would you like to Unlock\nthis item?");
			uI_MsgBox.AddProceYesHandler(UnLockedSlot);
		}
		else
		{
			if (uITrade_SellBoxData == null)
			{
				return;
			}
			if (uITrade_SellBoxData.DoodleState == UITrade_SellBoxData.EDoodleState.Selling)
			{
				UI_MsgBox uI_MsgBox2 = MessageBox("Would you like to take down\nthis item?");
				uI_MsgBox2.AddProceYesHandler(TakeDownItem);
			}
			else if (uITrade_SellBoxData.DoodleState == UITrade_SellBoxData.EDoodleState.Blank)
			{
				Debug.Log("弹出物品选择界面");
				if (!(auctionBox != null))
				{
					return;
				}
				int num = 0;
				UITrade_EquipedContainer equipedContainer = auctionBox.equipedContainer;
				equipedContainer.ExitContainer();
				int num2 = 5;
				equipedContainer.CreateBox(num2, 0);
				for (int i = 0; i < num2; i++)
				{
					UI_Box uI_Box = equipedContainer.LstBoxs[i];
					for (int j = 0; j < uI_Box.Slots.Length; j++)
					{
						if (COMA_Pref.Instance.package.pack[num] != null && COMA_Pref.Instance.package.pack[num].part <= 0 && COMA_Pref.Instance.package.pack[num].num > 1 && COMA_Pref.Instance.package.pack[num].state != COMA_PackageItem.PackageItemStatus.Sale)
						{
							UITrade_EquipedBoxSlot uITrade_EquipedBoxSlot = (UITrade_EquipedBoxSlot)uI_Box.Slots[j];
							uITrade_EquipedBoxSlot.SetIconPic(COMA_Pref.Instance.package.pack[num].iconTexture);
							UITrade_EquipedBoxData uITrade_EquipedBoxData = new UITrade_EquipedBoxData();
							uITrade_EquipedBoxData.idInPackage = num;
							uITrade_EquipedBoxData.number = COMA_Pref.Instance.package.pack[num].num;
							equipedContainer.AddBoxData(uITrade_EquipedBoxSlot.GetID(), uITrade_EquipedBoxData);
						}
						num++;
					}
				}
				equipedContainer.RefreshContainer();
				auctionBox.Show();
			}
			else if (uITrade_SellBoxData.DoodleState != UITrade_SellBoxData.EDoodleState.Confirming)
			{
			}
		}
	}

	private void UnLockedSlot(string param)
	{
		curSlot.SetSlot(UI_BoxSlot.ESlotState.UnLocked);
		curSlot.BoxData = new UITrade_SellBoxData();
		((UITrade_SellBoxData)curSlot.BoxData).DoodleState = UITrade_SellBoxData.EDoodleState.Blank;
	}

	private void TakeDownItem(string param)
	{
		UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)curSlot.BoxData;
		uITrade_SellBoxData.DoodleState = UITrade_SellBoxData.EDoodleState.Blank;
	}

	public void ProcessSelChange(UITrade_EquipedBoxSlot boxSlot)
	{
		curSel = boxSlot;
		if (curSel != preSel)
		{
			if (preSel != null)
			{
				preSel.ProcessSelected(false);
			}
			preSel = curSel;
		}
		if (curSel != null)
		{
			auctionBox.BoxData = (UITrade_EquipedBoxData)curSel.BoxData;
		}
	}

	public void HandleEventAuctionButton_OK(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventAuctionButton_OK-CommandClick");
			int price = auctionBox.Price;
			UITrade_EquipedBoxData boxData = auctionBox.BoxData;
			if (boxData != null)
			{
				int idInPackage = boxData.idInPackage;
				string tex = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[idInPackage].texture.EncodeToPNG());
				int kind = -1;
				if (COMA_Pref.Instance.package.pack[idInPackage].serialName.StartsWith("Head"))
				{
					kind = 0;
				}
				else if (COMA_Pref.Instance.package.pack[idInPackage].serialName.StartsWith("Body"))
				{
					kind = 1;
				}
				else if (COMA_Pref.Instance.package.pack[idInPackage].serialName.StartsWith("Leg"))
				{
					kind = 2;
				}
				else
				{
					Debug.LogError(COMA_Pref.Instance.package.pack[idInPackage].itemName + " can't be sold!!");
				}
				COMA_Pref.Instance.package.pack[idInPackage].state = COMA_PackageItem.PackageItemStatus.Sale;
				COMA_Pref.Instance.package.pack[idInPackage].gold = price;
				COMA_HTTP_TextureManager.Instance.TextureSell_InitToServer(tex, kind, price, COMA_Pref.Instance.package.pack[idInPackage].num, this);
				curSlot.SetIconPic(COMA_Pref.Instance.package.pack[idInPackage].iconTexture);
				UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)curSlot.BoxData;
				uITrade_SellBoxData.DoodleState = UITrade_SellBoxData.EDoodleState.Selling;
				COMA_Pref.Instance.Save(true);
			}
			UITrade_EquipedContainer equipedContainer = auctionBox.equipedContainer;
			equipedContainer.ExitContainer();
			auctionBox.Hide();
			break;
		}
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventAuctionButton_Add(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			auctionBox.Price += 500;
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventAuctionButton_Subtract(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			auctionBox.Price -= 500;
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButtonRefresh(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (refreshEffect == null)
			{
				Debug.Log("HandleEventButtonRefresh-CommandClick");
				refreshEffect = control.gameObject.GetComponent<UITrade_RefreshEffect>();
				if (refreshEffect != null)
				{
					refreshEffect.StartRefresh();
					SceneTimerInstance.Instance.Add(10f, ReadyToRefresh);
					COMA_TexBuyBuffer.Instance.items.Clear();
					RequestItemsOnBuyShell();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public bool ReadyToRefresh()
	{
		if (refreshEffect == null)
		{
			return false;
		}
		refreshEffect.EndRefresh();
		refreshEffect = null;
		return false;
	}

	public void HandleEventButton_PlayerRotate(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		float y = (0f - wparam) * COMA_Sys.Instance.sensitivity * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		playerOnShowCom.transform.rotation *= quaternion;
	}
}
