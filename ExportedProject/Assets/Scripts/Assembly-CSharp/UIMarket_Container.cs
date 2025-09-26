using UnityEngine;

public class UIMarket_Container : UI_Container
{
	private GameObject _objSellingInfoPerfab;

	private GameObject _objSellingBoxPerfab;

	private int nSimulateBoxCount = 6;

	[SerializeField]
	private GameObject _infoLabel;

	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/Market/Market_AvatarShopBox") as GameObject;
		_objSellingInfoPerfab = Resources.Load("UI/Market/Market_SellingInfoBox") as GameObject;
		_objSellingBoxPerfab = Resources.Load("UI/Market/Market_SellingBox") as GameObject;
	}

	private void Start()
	{
	}

	public void SimulateCall_AvatarInit()
	{
		Debug.Log("SimulateCall_AvatarInit");
		nSimulateBoxCount = 5;
		int num = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIMarket_AvatarShopData boxData = null;
				AddBoxData(uI_BoxSlot.GetID(), boxData);
			}
		}
		RefreshContainer();
		SimulateCall_AvatarRefresh();
	}

	public void SimulateCall_AvatarRefresh()
	{
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UI_Box uI_Box = base.LstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIMarket_AvatarShopData uIMarket_AvatarShopData = (UIMarket_AvatarShopData)uI_BoxSlot.BoxData;
				int num = i * 4 + j;
				if (num == 0)
				{
					uIMarket_AvatarShopData = COMA_Scene_Trade.Instance.officialTex.data;
				}
				else if (num < 1 + COMA_Scene_Trade.suitCount)
				{
					int num2 = num - 1;
					uIMarket_AvatarShopData = ((num2 >= COMA_Scene_Trade.Instance.suitTex.Count) ? null : COMA_Scene_Trade.Instance.suitTex[num2].data);
				}
				else if (num < 1 + COMA_Scene_Trade.suitCount + COMA_Scene_Trade.partCount)
				{
					int num3 = num - 1 - COMA_Scene_Trade.suitCount;
					uIMarket_AvatarShopData = ((num3 >= COMA_Scene_Trade.Instance.partTex.Count) ? null : COMA_Scene_Trade.Instance.partTex[num3].data);
				}
				uI_BoxSlot.BoxData = uIMarket_AvatarShopData;
				base.LstBoxDatas[uI_BoxSlot.GetID()] = uIMarket_AvatarShopData;
			}
		}
	}

	public void SimulateCall_MoldsInit()
	{
		Debug.Log("SimulateCall_MoldsInit");
		nSimulateBoxCount = COMA_Scene_Shop.Instance.shopData_Molds.Count / 4;
		if (COMA_Scene_Shop.Instance.shopData_Molds.Count % 4 != 0)
		{
			nSimulateBoxCount++;
		}
		int num = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				if (i * 4 + j < COMA_Scene_Shop.Instance.shopData_Molds.Count)
				{
					UIMarket_AvatarShopData uIMarket_AvatarShopData = COMA_Scene_Shop.Instance.shopData_Molds[i * 4 + j];
					uIMarket_AvatarShopData.CanSell = true;
					AddBoxData(uI_BoxSlot.GetID(), uIMarket_AvatarShopData);
				}
			}
		}
		RefreshContainer();
	}

	public void SimulateCall_AccessoriesInit()
	{
		Debug.Log("SimulateCall_AccessoriesInit");
		nSimulateBoxCount = COMA_Scene_Shop.Instance.shopData_Accessories.Count / 4;
		if (COMA_Scene_Shop.Instance.shopData_Accessories.Count % 4 != 0)
		{
			nSimulateBoxCount++;
		}
		if (nSimulateBoxCount < 4)
		{
			nSimulateBoxCount = 4;
		}
		int num = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				if (i * 4 + j < COMA_Scene_Shop.Instance.shopData_Accessories.Count)
				{
					UIMarket_AvatarShopData boxData = COMA_Scene_Shop.Instance.shopData_Accessories[i * 4 + j];
					AddBoxData(uI_BoxSlot.GetID(), boxData);
				}
			}
		}
		for (int num2 = nSimulateBoxCount - 1; num2 >= nSimulateBoxCount; num2--)
		{
			UI_Box uI_Box2 = lstBoxs[num2];
			for (int k = 0; k < uI_Box2.Slots.Length; k++)
			{
				UI_BoxSlot uI_BoxSlot2 = uI_Box2.Slots[k];
				AddBoxData(uI_BoxSlot2.GetID(), null);
				uI_BoxSlot2.gameObject.SetActive(false);
			}
		}
		RefreshContainer();
	}

	public void SimulateCall_SellingSingle()
	{
		Debug.Log("SimulateCall_SellingSingle");
		nSimulateBoxCount = COMA_Package.maxCount / 4;
		GameObject gameObject = objBoxPerfab;
		objBoxPerfab = _objSellingBoxPerfab;
		int num = CreateBox(nSimulateBoxCount, 0);
		objBoxPerfab = gameObject;
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIMarket_AvatarShopData uIMarket_AvatarShopData = null;
				int num2 = i * 4 + j;
				if (COMA_Pref.Instance.package.pack[num2] != null && COMA_Pref.Instance.package.pack[num2].part <= 0 && COMA_Pref.Instance.package.pack[num2].num > 1 && COMA_Pref.Instance.package.pack[num2].state != COMA_PackageItem.PackageItemStatus.Equiped)
				{
					uIMarket_AvatarShopData = new UIMarket_AvatarShopData();
					uIMarket_AvatarShopData.OfficalIcon = false;
					uIMarket_AvatarShopData.AvatarPrice = 0;
					uIMarket_AvatarShopData.Suited = false;
					uIMarket_AvatarShopData.PartType = COMA_Pref.Instance.package.pack[num2].serialName;
					uIMarket_AvatarShopData.itemName = COMA_Pref.Instance.package.pack[num2].itemName;
					uIMarket_AvatarShopData.AvatarIcon = COMA_Pref.Instance.package.pack[num2].iconTexture;
				}
				AddBoxData(uI_BoxSlot.GetID(), uIMarket_AvatarShopData);
			}
		}
		RefreshContainer();
	}

	public void SimulateCall_SellingBatch()
	{
		Debug.Log("SimulateCall_SellingBatch");
		nSimulateBoxCount = COMA_Package.maxCount / 4;
		GameObject gameObject = objBoxPerfab;
		objBoxPerfab = _objSellingBoxPerfab;
		int num = CreateBox(nSimulateBoxCount, 0);
		objBoxPerfab = gameObject;
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIMarket_AvatarShopData uIMarket_AvatarShopData = null;
				int num2 = i * 4 + j;
				if (COMA_Pref.Instance.package.pack[num2] != null && COMA_Pref.Instance.package.pack[num2].part <= 0 && COMA_Pref.Instance.package.pack[num2].num > 1 && COMA_Pref.Instance.package.pack[num2].state != COMA_PackageItem.PackageItemStatus.Equiped)
				{
					uIMarket_AvatarShopData = new UIMarket_AvatarShopData();
					uIMarket_AvatarShopData.OfficalIcon = false;
					uIMarket_AvatarShopData.AvatarPrice = 0;
					uIMarket_AvatarShopData.Suited = false;
					uIMarket_AvatarShopData.PartType = COMA_Pref.Instance.package.pack[num2].serialName;
					uIMarket_AvatarShopData.itemName = COMA_Pref.Instance.package.pack[num2].itemName;
					uIMarket_AvatarShopData.AvatarIcon = COMA_Pref.Instance.package.pack[num2].iconTexture;
				}
				AddBoxData(uI_BoxSlot.GetID(), uIMarket_AvatarShopData);
			}
		}
		RefreshContainer();
	}

	public void UnActiveInfoLabel()
	{
		_infoLabel.SetActive(false);
	}

	public void SimulateCall_SellingInfo()
	{
		UIMarket.Instance.framesToLate = 0;
		UIMarket.forSellingExit++;
		Debug.Log("SimulateCall_SellingInfo");
		GameObject gameObject = objBoxPerfab;
		objBoxPerfab = _objSellingInfoPerfab;
		nSimulateBoxCount = COMA_TexOnSale.Instance.items.Count;
		int num = CreateBox(nSimulateBoxCount, 0);
		objBoxPerfab = gameObject;
		if (nSimulateBoxCount == 0)
		{
			_infoLabel.SetActive(true);
		}
		else
		{
			UnActiveInfoLabel();
		}
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				UIMarket_SellingInfoData uIMarket_SellingInfoData = new UIMarket_SellingInfoData();
				uIMarket_SellingInfoData.AvatarIcon = null;
				uIMarket_SellingInfoData.DayNum = 0;
				uIMarket_SellingInfoData.HourNum = 0;
				uIMarket_SellingInfoData.MinuteNum = 0;
				uIMarket_SellingInfoData.SoldNum = 0;
				uIMarket_SellingInfoData.BalanceNum = 0;
				uIMarket_SellingInfoData.Cliamed = false;
				AddBoxData(uI_BoxSlot.GetID(), uIMarket_SellingInfoData);
				string tid = COMA_TexOnSale.Instance.items[i].tid;
				string param = "OnSale_" + i.ToString() + "_" + UIMarket.forSellingExit + "_" + COMA_TexOnSale.Instance.items[i].tid;
				if (COMA_TexOnSale.Instance.items[i].isSuit)
				{
					COMA_Server_Texture.Instance.TextureSuit_GetTextureFromServer(tid, param);
				}
				else
				{
					COMA_Server_Texture.Instance.TextureSell_GetTextureFromServer(tid, param);
				}
			}
		}
		RefreshContainer();
	}

	private new void Update()
	{
	}
}
