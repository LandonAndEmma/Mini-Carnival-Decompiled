using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using Protocol.Shop.C2S;
using Protocol.Shop.S2C;
using UIGlobal;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UIMarket : UIEntity
	{
		public enum ETabTopButtonsType
		{
			None = -1,
			Featured = 0,
			Market = 1
		}

		public enum ETabLeftButtonsType
		{
			None = 0,
			Avatar_Best = 1,
			Avatar = 2,
			Accessory = 3,
			Template = 4,
			Favourites_Icon = 5,
			Favourites_Avatar = 6,
			Gem = 7
		}

		[SerializeField]
		private ETabTopButtonsType _curTabTopBtnType;

		private ETabTopButtonsType _preTabTopBtnType = ETabTopButtonsType.None;

		[SerializeField]
		private ETabLeftButtonsType _curTabLeftBtnType;

		private ETabLeftButtonsType _preTabLeftBtnType;

		[SerializeField]
		private GameObject _menu_content_Favourites;

		[SerializeField]
		private GameObject _menu_content_Market;

		[SerializeField]
		private GameObject _uishoppingCart;

		private bool bEquipedAfterBuy;

		private int _buyNum;

		private bool _bEquipedAfterBuy;

		private int _unlockBagNum;

		[SerializeField]
		private GameObject _uiFittingRoom;

		[SerializeField]
		private GameObject _menu_tab_left;

		[SerializeField]
		private GameObject _caption_AlternativeBtn;

		[SerializeField]
		private GameObject _favourites_Content;

		[SerializeField]
		private GameObject _favourites_Author_Detail;

		[SerializeField]
		private GameObject _market_Content;

		[SerializeField]
		private UIMarket_Container _favourites_Container;

		private Dictionary<uint, UIMarket_BoxData> _lstShoppingCart = new Dictionary<uint, UIMarket_BoxData>();

		private Dictionary<string, UIMarket_BoxData> _lstShoppingCart_System = new Dictionary<string, UIMarket_BoxData>();

		private bool _bNeedRefreshCharcProfile;

		[SerializeField]
		private UILabel _label_favourites_player;

		[SerializeField]
		private UILabel _label_favourites_avatar;

		[SerializeField]
		private List<UIMarket_BoxData> _lstBestAvatarCache = new List<UIMarket_BoxData>();

		[SerializeField]
		private List<UIMarket_BoxData> _lstAdAvatarCache = new List<UIMarket_BoxData>();

		[SerializeField]
		private List<UIMarket_BoxData> _lstRandomAvatarCache = new List<UIMarket_BoxData>();

		[SerializeField]
		private UIMarket_Container _uiMarketContainer;

		private int _gemShopDay;

		[SerializeField]
		private UIMarket_ShoppingCartContainer _uiMarketShoppingCartContainer;

		[SerializeField]
		private UIMarket_FittingRoomContainer _uiMarketFittingRoomContainer;

		public ETabTopButtonsType CurTabTopBtnType
		{
			get
			{
				return _curTabTopBtnType;
			}
			set
			{
				_curTabTopBtnType = value;
			}
		}

		public ETabLeftButtonsType CurTabLeftBtnType
		{
			get
			{
				return _curTabLeftBtnType;
			}
			set
			{
				_curTabLeftBtnType = value;
			}
		}

		protected override void Load()
		{
			RegisterMessage(EUIMessageID.UIMarket_TabTopButtonSelChanged, this, TabTopButtonSelChanged);
			RegisterMessage(EUIMessageID.UIMarket_TabLeftButtonSelChanged, this, TabLeftButtonSelChanged);
			RegisterMessage(EUIMessageID.UI_ButtonAlternativeChanged, this, ButtonAlternativeChanged);
			RegisterMessage(EUIMessageID.UIMarket_GotoShoppingCart, this, GotoShoppingCart);
			RegisterMessage(EUIMessageID.UIMarket_GotoFittingRoom, this, GotoFittingRoom);
			RegisterMessage(EUIMessageID.UIMarket_SelNewShoppingItem, this, SelectNewShoppingItem);
			RegisterMessage(EUIMessageID.UIMarket_DelNewShoppingItem, this, DelNewShoppingItem);
			RegisterMessage(EUIMessageID.UIMarket_PreviewPersonalFavorites, this, PreviewPersonalFavorites);
			RegisterMessage(EUIMessageID.UIMarket_CaptionClose, this, CaptionClose);
			RegisterMessage(EUIMessageID.UIMarket_PurchaseShopItems, this, PurchaseShopItems);
			RegisterMessage(EUIMessageID.UIMarket_PurchaseShopItemsSuccess, this, PurchaseShopItemsSuccess);
			RegisterMessage(EUIMessageID.UIMarket_PurchaseShopItemsFailure, this, PurchaseShopItemsFailure);
			RegisterMessage(EUIMessageID.UIBackpack_NewItemInBag, this, NewItemInBag);
			RegisterMessage(EUIMessageID.UIMarket_BoxRefresh_Free, this, BoxRefresh_Free);
			RegisterMessage(EUIMessageID.UIMarket_BoxRefresh_Charge, this, BoxRefresh_Charge);
			RegisterMessage(EUIMessageID.UIMarket_UnfollowSuccess, this, InFavUnfollowSuccess);
			RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
			RegisterMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess1, this, UnlockBagCellSuccess);
			RegisterMessage(EUIMessageID.UIMarket_UncollectAvatarSuccess, this, UncollectAvatarSuccess);
			RegisterMessage(EUIMessageID.UIRPG_NotifyGetShopGemResult, this, NotifyGetShopGemResult);
			_bNeedRefreshCharcProfile = true;
			bEquipedAfterBuy = false;
		}

		protected override void UnLoad()
		{
			UnregisterMessage(EUIMessageID.UIMarket_TabTopButtonSelChanged, this);
			UnregisterMessage(EUIMessageID.UIMarket_TabLeftButtonSelChanged, this);
			UnregisterMessage(EUIMessageID.UI_ButtonAlternativeChanged, this);
			UnregisterMessage(EUIMessageID.UIMarket_GotoShoppingCart, this);
			UnregisterMessage(EUIMessageID.UIMarket_GotoFittingRoom, this);
			UnregisterMessage(EUIMessageID.UIMarket_SelNewShoppingItem, this);
			UnregisterMessage(EUIMessageID.UIMarket_DelNewShoppingItem, this);
			UnregisterMessage(EUIMessageID.UIMarket_PreviewPersonalFavorites, this);
			UnregisterMessage(EUIMessageID.UIMarket_CaptionClose, this);
			UnregisterMessage(EUIMessageID.UIMarket_PurchaseShopItems, this);
			UnregisterMessage(EUIMessageID.UIMarket_PurchaseShopItemsSuccess, this);
			UnregisterMessage(EUIMessageID.UIMarket_PurchaseShopItemsFailure, this);
			UnregisterMessage(EUIMessageID.UIBackpack_NewItemInBag, this);
			UnregisterMessage(EUIMessageID.UIMarket_BoxRefresh_Free, this);
			UnregisterMessage(EUIMessageID.UIMarket_BoxRefresh_Charge, this);
			UnregisterMessage(EUIMessageID.UIMarket_UnfollowSuccess, this);
			UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
			UnregisterMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess1, this);
			UnregisterMessage(EUIMessageID.UIMarket_UncollectAvatarSuccess, this);
			UnregisterMessage(EUIMessageID.UIRPG_NotifyGetShopGemResult, this);
			_bNeedRefreshCharcProfile = false;
			bEquipedAfterBuy = false;
			ClearShoppingCart();
		}

		private bool TabTopButtonSelChanged(TUITelegram msg)
		{
			CurTabTopBtnType = (ETabTopButtonsType)(int)msg._pExtraInfo;
			if (_preTabTopBtnType != CurTabTopBtnType)
			{
				RefreshMarketContainer();
			}
			_preTabTopBtnType = CurTabTopBtnType;
			return true;
		}

		private bool TabLeftButtonSelChanged(TUITelegram msg)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click_Sort);
			CurTabLeftBtnType = (ETabLeftButtonsType)(int)msg._pExtraInfo;
			if (_preTabLeftBtnType != CurTabLeftBtnType)
			{
				RefreshMarketContainer();
			}
			_preTabLeftBtnType = CurTabLeftBtnType;
			GameObject gameObject = (GameObject)msg._pExtraInfo2;
			gameObject.GetComponent<UICheckbox>().isChecked = true;
			if (CurTabLeftBtnType == ETabLeftButtonsType.Template && COMA_Pref.Instance.NG2_FirstEnterMarketTmp)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstEnterMarketTmp, null, null);
			}
			return true;
		}

		private bool ButtonAlternativeChanged(TUITelegram msg)
		{
			UI_ButtonAlternativeGroup uI_ButtonAlternativeGroup = msg._pSender as UI_ButtonAlternativeGroup;
			if (uI_ButtonAlternativeGroup.GroupId != 0)
			{
				return false;
			}
			switch ((ETabTopButtonsType)(int)msg._pExtraInfo)
			{
			case ETabTopButtonsType.Market:
				_menu_content_Market.SetActive(true);
				_menu_content_Favourites.SetActive(false);
				break;
			case ETabTopButtonsType.Featured:
				_menu_content_Market.SetActive(false);
				_menu_content_Favourites.SetActive(true);
				break;
			}
			return true;
		}

		private bool GotoShoppingCart(TUITelegram msg)
		{
			if (CurTabLeftBtnType == ETabLeftButtonsType.Gem)
			{
				if (_uiMarketContainer.CurSelBox != null && _uiMarketContainer.CurSelBox.BoxData.DataType == 4)
				{
					NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
					bool flag = false;
					UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_uiMarketContainer.CurSelBox.BoxData;
					if (uIMarket_BoxData.CurrencyType == ECurrencyType.Crystal)
					{
						if (notifyRoleDataCmd.m_info.m_crystal < uIMarket_BoxData.Price)
						{
							flag = true;
						}
					}
					else if (uIMarket_BoxData.CurrencyType == ECurrencyType.Gold && notifyRoleDataCmd.m_info.m_gold < uIMarket_BoxData.Price)
					{
						flag = true;
					}
					if (flag)
					{
						UIGolbalStaticFun.PopMsgBox_LackMoney();
					}
					else
					{
						UIGolbalStaticFun.PopBlockOnlyMessageBox();
						BuyGemShopItemCmd buyGemShopItemCmd = new BuyGemShopItemCmd();
						buyGemShopItemCmd.m_day = (byte)_gemShopDay;
						buyGemShopItemCmd.m_item_id = (uint)uIMarket_BoxData.InListID;
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, buyGemShopItemCmd);
						COMA_HTTP_DataCollect.Instance.SendBuyGemCount("1");
					}
				}
				return true;
			}
			int num = _lstShoppingCart.Count + _lstShoppingCart_System.Count;
			if (num <= 0)
			{
				string str = Localization.instance.Get("shangdianjiemian_desc31");
				UIGolbalStaticFun.PopupTipsBox(str);
				return true;
			}
			_uiMarketContainer.InitContainer(UI_Container.EBoxSelType.Single);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, null);
			_uishoppingCart.SetActive(true);
			RefreshMarketShoppingCartContainer();
			return true;
		}

		private void SendPurchaseMsgToSrv()
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_BuyItem);
			int buyNum = _buyNum;
			Debug.Log("--------------------------------------------PopNum:" + buyNum);
			UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(buyNum);
			bEquipedAfterBuy = buyNum > 0 && _bEquipedAfterBuy;
			int count = _lstShoppingCart.Count;
			if (count > 0)
			{
				foreach (uint key in _lstShoppingCart.Keys)
				{
					UIMarket_BoxData uIMarket_BoxData = _lstShoppingCart[key];
					Debug.Log("-------PurchaseShopItems = " + uIMarket_BoxData.ItemId);
					BuyAvatarCmd buyAvatarCmd = new BuyAvatarCmd();
					buyAvatarCmd.m_id = (uint)uIMarket_BoxData.ItemId;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, buyAvatarCmd);
				}
			}
			count = _lstShoppingCart_System.Count;
			if (count <= 0)
			{
				return;
			}
			foreach (string key2 in _lstShoppingCart_System.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData2 = _lstShoppingCart_System[key2];
				Debug.Log("-------PurchaseShopItems = " + uIMarket_BoxData2.Unit);
				BuySysShopCmd buySysShopCmd = new BuySysShopCmd();
				buySysShopCmd.m_id = uIMarket_BoxData2.Unit;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, buySysShopCmd);
			}
		}

		private bool OnPopBoxClick_Yes(TUITelegram msg)
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
			Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
			switch (uIMessage_CommonBoxData.Mark)
			{
			case "UnlockBagCellToBuy":
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				BuyBagCellCmd buyBagCellCmd = new BuyBagCellCmd();
				buyBagCellCmd.m_buy_num = (byte)_unlockBagNum;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, buyBagCellCmd);
				break;
			}
			}
			return true;
		}

		private bool UnlockBagCellSuccess(TUITelegram msg)
		{
			SendPurchaseMsgToSrv();
			return true;
		}

		private bool UncollectAvatarSuccess(TUITelegram msg)
		{
			if (IsFavouritesLabel() && CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Avatar)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, null);
				RefreshMarketContainer_Favourites();
			}
			return true;
		}

		private bool NotifyGetShopGemResult(TUITelegram msg)
		{
			BuyGemShopItemResultCmd buyGemShopItemResultCmd = msg._pExtraInfo as BuyGemShopItemResultCmd;
			if (buyGemShopItemResultCmd.m_result == 0)
			{
				UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Gem, 1, buyGemShopItemResultCmd.m_gem_id);
				UIGolbalStaticFun.PopGetItemBox(data);
			}
			return true;
		}

		private bool PurchaseShopItems(TUITelegram msg)
		{
			int num = _lstShoppingCart.Count + _lstShoppingCart_System.Count;
			if (num <= 0)
			{
				string str = Localization.instance.Get("shangdianjiemian_desc31");
				UIGolbalStaticFun.PopupTipsBox(str);
				return true;
			}
			if (!_uiMarketShoppingCartContainer.IsEnoughMoneyToBuy())
			{
				UIGolbalStaticFun.PopMsgBox_LackMoney();
				return true;
			}
			Debug.Log("PurchaseShopItems--- :" + bEquipedAfterBuy);
			_buyNum = num;
			_bEquipedAfterBuy = (bool)msg._pExtraInfo;
			int num2 = 0;
			foreach (string key in _lstShoppingCart_System.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData = _lstShoppingCart_System[key];
				if (uIMarket_BoxData.Unit == "HBL01")
				{
					num2 += 2;
					break;
				}
			}
			bool flag = false;
			foreach (uint key2 in _lstShoppingCart.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData2 = _lstShoppingCart[key2];
				for (int i = 0; i < 3; i++)
				{
					if (uIMarket_BoxData2.Units[i] != string.Empty)
					{
						num2++;
						flag = true;
					}
				}
			}
			if (flag)
			{
				num2 -= _lstShoppingCart.Count;
			}
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			int num3 = notifyRoleDataCmd.m_bag_data.m_bag_capacity - notifyRoleDataCmd.m_bag_data.m_bag_list.Count;
			Debug.LogWarning("roleData.m_bag_data.m_bag_capacity=" + notifyRoleDataCmd.m_bag_data.m_bag_capacity + "  roleData.m_bag_data.m_bag_list.Count=" + notifyRoleDataCmd.m_bag_data.m_bag_list.Count);
			if (num3 < 0)
			{
				Debug.LogWarning("roleData.m_bag_data.m_bag_list.Count>roleData.m_bag_data.m_bag_capacity");
				num3 = 0;
			}
			int num4 = COMA_Package.maxCount - notifyRoleDataCmd.m_bag_data.m_bag_capacity;
			if (num3 >= num + num2)
			{
				SendPurchaseMsgToSrv();
			}
			else if (num3 + num4 >= num + num2)
			{
				Debug.LogWarning("buyNum=" + num + "  nOffset=" + num2 + " bagAvailableNum=" + num3);
				_unlockBagNum = num + num2 - num3;
				string des = TUITool.StringFormat(Localization.instance.Get("beibaojiemian_desc7"), _unlockBagNum, _unlockBagNum * COMA_DataConfig.Instance._sysConfig.Bag.unlock_cost);
				UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
				uIMessage_CommonBoxData.Mark = "UnlockBagCellToBuy";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			}
			else
			{
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("beibaojiemian_desc21"));
				UIGolbalStaticFun.PopCommonMessageBox(data);
			}
			return true;
		}

		private bool PurchaseShopItemsSuccess(TUITelegram msg)
		{
			UIAutoDelBlockOnlyMessageBoxMgr.Instance.ReleaseAutoDelBlockOnlyMessageBox();
			if ((int)msg._pExtraInfo2 == 0)
			{
				uint num = (uint)msg._pExtraInfo;
				if (_lstShoppingCart.ContainsKey(num))
				{
					UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Other, _lstShoppingCart[num].Tex);
					UIGolbalStaticFun.PopGetItemBox(data);
					Debug.Log("Shop Cart Remove : " + num);
					_lstShoppingCart.Remove(num);
					RefreshMarketShoppingCartContainer();
				}
			}
			else if ((int)msg._pExtraInfo2 == 1)
			{
				string text = (string)msg._pExtraInfo;
				if (_lstShoppingCart_System.ContainsKey(text))
				{
					UIMarket_BoxData uIMarket_BoxData = _lstShoppingCart_System[text];
					Debug.Log("SYS MARKET:" + uIMarket_BoxData.Unit);
					if (UIGolbalStaticFun.IsSystemShopTmp(uIMarket_BoxData.Unit))
					{
						UIGetItemBoxData data2 = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Other, _lstShoppingCart_System[text].Tex);
						UIGolbalStaticFun.PopGetItemBox(data2);
						RefreshMarketContainer();
					}
					else
					{
						UIGetItemBoxData data3 = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Deco, _lstShoppingCart_System[text].SpriteName);
						UIGolbalStaticFun.PopGetItemBox(data3);
					}
					Debug.Log("Shop Cart Remove : " + text);
					_lstShoppingCart_System.Remove(text);
					RefreshMarketShoppingCartContainer();
				}
			}
			if (_lstShoppingCart.Count == 0 && _lstShoppingCart_System.Count == 0)
			{
				_uishoppingCart.SetActive(false);
			}
			return true;
		}

		private bool NewItemInBag(TUITelegram msg)
		{
			BagItem bagItem = (BagItem)msg._pExtraInfo;
			if (bEquipedAfterBuy)
			{
				MountBagItemCmd mountBagItemCmd = new MountBagItemCmd();
				mountBagItemCmd.m_unique_id = bagItem.m_unique_id;
				NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
				RoleInfo info = notifyRoleDataCmd.m_info;
				if (bagItem.m_part == 1)
				{
					info.m_head = bagItem.m_unique_id;
					Debug.Log("NewItemInBag:Head->" + info.m_head);
				}
				else if (bagItem.m_part == 2)
				{
					info.m_body = bagItem.m_unique_id;
				}
				else if (bagItem.m_part == 3)
				{
					info.m_leg = bagItem.m_unique_id;
				}
				else if (bagItem.m_part == 4)
				{
					int num = COMA_PackageItem.NameToPart(bagItem.m_unit);
					switch (num)
					{
					case 1:
						info.m_head_top = bagItem.m_unique_id;
						break;
					case 2:
						info.m_head_front = bagItem.m_unique_id;
						break;
					case 3:
						info.m_head_back = bagItem.m_unique_id;
						break;
					case 4:
						info.m_head_left = bagItem.m_unique_id;
						break;
					case 5:
						info.m_head_right = bagItem.m_unique_id;
						break;
					case 6:
						info.m_chest_front = bagItem.m_unique_id;
						break;
					case 7:
						info.m_chest_back = bagItem.m_unique_id;
						break;
					}
					mountBagItemCmd.m_mount_part = (byte)(num - 1);
				}
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Charc);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd);
			}
			return true;
		}

		private bool PurchaseShopItemsFailure(TUITelegram msg)
		{
			UIAutoDelBlockOnlyMessageBoxMgr.Instance.ReleaseAutoDelBlockOnlyMessageBox();
			if ((int)msg._pExtraInfo2 == 0)
			{
				switch ((BuyAvatarResultCmd.Code)(int)msg._pExtraInfo)
				{
				case BuyAvatarResultCmd.Code.kMiss:
				{
					UIMessage_CommonBoxData data3 = new UIMessage_CommonBoxData(1, Localization.instance.Get("shangdianjiemian_desc27"));
					UIGolbalStaticFun.PopCommonMessageBox(data3);
					break;
				}
				case BuyAvatarResultCmd.Code.kNoEnough:
					UIGolbalStaticFun.PopMsgBox_LackMoney();
					break;
				case BuyAvatarResultCmd.Code.kSellOut:
				{
					UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("shangdianjiemian_desc21"));
					UIGolbalStaticFun.PopCommonMessageBox(data2);
					break;
				}
				case BuyAvatarResultCmd.Code.kNoSpace:
				{
					UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("jiaoyijiemian_desc3"));
					UIGolbalStaticFun.PopCommonMessageBox(data);
					break;
				}
				}
			}
			else if ((int)msg._pExtraInfo2 == 1)
			{
				switch ((BuySysShopResultCmd.Code)(int)msg._pExtraInfo)
				{
				case BuySysShopResultCmd.Code.kNoEnough:
					UIGolbalStaticFun.PopMsgBox_LackMoney();
					break;
				case BuySysShopResultCmd.Code.kNoSpace:
				{
					UIMessage_CommonBoxData data4 = new UIMessage_CommonBoxData(1, Localization.instance.Get("jiaoyijiemian_desc3"));
					UIGolbalStaticFun.PopCommonMessageBox(data4);
					break;
				}
				}
			}
			return true;
		}

		private void ForceRefreshMarket(bool bFree)
		{
			if (COMA_Pref.Instance.GetCrystal() > 0 || bFree)
			{
				Debug.Log("ForceRefreshMarket====" + CurTabLeftBtnType);
				if (CurTabLeftBtnType == ETabLeftButtonsType.Avatar_Best)
				{
					_lstBestAvatarCache.Clear();
				}
				else
				{
					_lstAdAvatarCache.Clear();
					_lstRandomAvatarCache.Clear();
				}
				RefreshMarketContainer();
			}
		}

		private bool BoxRefresh_Free(TUITelegram msg)
		{
			ForceRefreshMarket(true);
			return true;
		}

		private bool BoxRefresh_Charge(TUITelegram msg)
		{
			ForceRefreshMarket(false);
			return true;
		}

		private bool InFavUnfollowSuccess(TUITelegram msg)
		{
			if (CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Icon)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_CaptionClose, null, 1);
			}
			return true;
		}

		private bool SelectNewShoppingItem(TUITelegram msg)
		{
			UIMarket_BoxData uIMarket_BoxData = msg._pExtraInfo as UIMarket_BoxData;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Wear, uIMarket_BoxData);
			if (uIMarket_BoxData.DataType == 1)
			{
				uint key = (uint)uIMarket_BoxData.ItemId;
				List<uint> list = new List<uint>();
				foreach (uint key2 in _lstShoppingCart.Keys)
				{
					UIMarket_BoxData uIMarket_BoxData2 = _lstShoppingCart[key2];
					Debug.LogWarning("data.AvatarAttribute=" + uIMarket_BoxData2.AvatarAttribute + ",boxData.AvatarAttribute=" + uIMarket_BoxData.AvatarAttribute);
					if (IsShopCartAvatarReplace(uIMarket_BoxData2.AvatarAttribute, uIMarket_BoxData.AvatarAttribute))
					{
						list.Add(key2);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					Debug.Log("----------------Del to Shopping Cart----Avatar:" + list[i]);
					_lstShoppingCart.Remove(list[i]);
				}
				Debug.Log("----------------Add to Shopping Cart----Avatar!");
				UIMarket_BoxData value = new UIMarket_BoxData(uIMarket_BoxData);
				if (!_lstShoppingCart.ContainsKey(key))
				{
					_lstShoppingCart.Add(key, value);
				}
			}
			else if (uIMarket_BoxData.DataType == 2)
			{
				string unit = uIMarket_BoxData.Unit;
				if (!_lstShoppingCart_System.ContainsKey(unit))
				{
					foreach (KeyValuePair<string, UIMarket_BoxData> item in _lstShoppingCart_System)
					{
						if (COMA_PackageItem.SysNameToPart(item.Key) == COMA_PackageItem.SysNameToPart(unit))
						{
							_lstShoppingCart_System.Remove(item.Key);
							Debug.Log("----------------Remove Shopping Cart---System! = " + item.Key);
							break;
						}
					}
					Debug.Log("_lstShoppingCart_System.Count=" + _lstShoppingCart_System.Count);
					Debug.Log("----------------Add to Shopping Cart---System!");
					UIMarket_BoxData value2 = new UIMarket_BoxData(uIMarket_BoxData);
					_lstShoppingCart_System.Add(unit, value2);
				}
			}
			Debug.Log("Select New Shop Item Type=" + (UIMarket_BoxData.EDataType)uIMarket_BoxData.DataType);
			if (uIMarket_BoxData.DataType == 1)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, uIMarket_BoxData, CurTabLeftBtnType);
				if (COMA_Pref.Instance.NG2_FirstEnterMarket)
				{
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstWatchShopItem, this, null);
				}
			}
			return true;
		}

		private bool DelNewShoppingItem(TUITelegram msg)
		{
			UIMarket_BoxData uIMarket_BoxData = msg._pExtraInfo as UIMarket_BoxData;
			Debug.Log("DelNewShoppingItem:ID=" + uIMarket_BoxData.ItemId);
			_lstShoppingCart.Remove((uint)uIMarket_BoxData.ItemId);
			Debug.Log("DelNewShoppingItem:Name=" + uIMarket_BoxData.Unit);
			if (uIMarket_BoxData.Unit != string.Empty)
			{
				_lstShoppingCart_System.Remove(uIMarket_BoxData.Unit);
			}
			bool isChecked = IsShopCartAvatarCanEI();
			_uiMarketShoppingCartContainer._equipedCheck.isChecked = isChecked;
			_uiMarketShoppingCartContainer._equipedCheck.gameObject.SetActive(isChecked);
			return true;
		}

		private bool GotoFittingRoom(TUITelegram msg)
		{
			_uiFittingRoom.SetActive(true);
			RefreshMarketFittingRoomContainer();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, this, UIMarketFittingRoom3DMgr.EOperType.Show_Charc);
			return true;
		}

		private bool PreviewPersonalFavorites(TUITelegram msg)
		{
			_menu_tab_left.SetActive(false);
			_caption_AlternativeBtn.SetActive(false);
			_market_Content.SetActive(false);
			_favourites_Content.SetActive(true);
			_favourites_Author_Detail.SetActive(true);
			_favourites_Container.InitContainer(UI_Container.EBoxSelType.Single);
			_favourites_Container.ClearContainer();
			UIMarket_BoxData uIMarket_BoxData = msg._pExtraInfo as UIMarket_BoxData;
			Debug.Log("Fetch Player:" + (uint)uIMarket_BoxData.ItemId + " 's ShopList!");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyEnterAuthorShop, this, (uint)uIMarket_BoxData.ItemId);
			UIDataBufferCenter.Instance.FetchPlayerSellList((uint)uIMarket_BoxData.ItemId, delegate(List<ShopItem> lstShopList)
			{
				Debug.Log("FetchPlayerSellList Avatar Count=" + lstShopList.Count);
				_favourites_Container.InitBoxs(lstShopList.Count, true);
				for (int i = 0; i < lstShopList.Count; i++)
				{
					UIMarket_BoxData uIMarket_BoxData2 = new UIMarket_BoxData
					{
						DataType = 1,
						ItemId = lstShopList[i].m_id,
						Price = (int)lstShopList[i].m_price,
						CurrencyType = (ECurrencyType)lstShopList[i].m_price_type,
						AuthorId = lstShopList[i].m_author,
						RemainNum = lstShopList[i].m_remain_num,
						PraiseNum = lstShopList[i].m_praise,
						Units = lstShopList[i].m_unit
					};
					UIGolbalStaticFun.GetAvatarSuitTex(new CSuitMD5((uint)uIMarket_BoxData2.ItemId, lstShopList[i].m_unit), uIMarket_BoxData2);
					_favourites_Container.SetBoxData(i, uIMarket_BoxData2);
				}
			});
			return true;
		}

		private bool CaptionClose(TUITelegram msg)
		{
			if (_favourites_Content.activeSelf)
			{
				_menu_tab_left.SetActive(true);
				_caption_AlternativeBtn.SetActive(true);
				_market_Content.SetActive(true);
				_favourites_Content.SetActive(false);
				_favourites_Author_Detail.SetActive(false);
				_uiMarketContainer.InitContainer(UI_Container.EBoxSelType.Single);
				if (msg._pExtraInfo != null && (int)msg._pExtraInfo == 1)
				{
					RefreshMarketContainer_Favourites();
				}
			}
			else
			{
				UIGolbalStaticFun.PopBlockForTUIMessageBox();
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquareScene);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
				if (COMA_Scene_PlayerController.Instance != null)
				{
					COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
				}
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_UpdateSelfAvatarOnSquare, null, null);
			return true;
		}

		private bool GotoSquareScene(object obj)
		{
			Debug.Log("GotoSquareScene");
			UIGolbalStaticFun.CloseBlockForTUIMessageBox();
			TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.AddHidePre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			return true;
		}

		private void ClearShoppingCart()
		{
			_lstShoppingCart.Clear();
			_lstShoppingCart_System.Clear();
		}

		private bool IsNeedRefreshBtn()
		{
			return CurTabLeftBtnType == ETabLeftButtonsType.Avatar_Best || CurTabLeftBtnType == ETabLeftButtonsType.Avatar;
		}

		private bool IsNeedShowMoreInfo()
		{
			return CurTabLeftBtnType == ETabLeftButtonsType.Avatar_Best || CurTabLeftBtnType == ETabLeftButtonsType.Avatar || CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Avatar;
		}

		private bool IsFavouritesLabel()
		{
			return CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Icon || CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Avatar;
		}

		protected void GetShopListByType(byte type, byte size, int startBox, bool needSetRefreshBtn)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox(type.ToString());
			UIDataBufferCenter.Instance.FetchShopList(type, size, delegate(List<ShopItem> lstShopList)
			{
				UIGolbalStaticFun.CloseBlockOnlyMessageBox(type.ToString());
				if (type == 2)
				{
					_lstBestAvatarCache.Clear();
				}
				else if (type == 1)
				{
					_lstAdAvatarCache.Clear();
				}
				else if (type == 0)
				{
					_lstRandomAvatarCache.Clear();
				}
				Debug.Log("========================\nlstShopList.Count=" + lstShopList.Count);
				for (int i = 0; i < lstShopList.Count; i++)
				{
					UIMarket_BoxData uIMarket_BoxData = new UIMarket_BoxData(lstShopList[i]);
					if (type == 1)
					{
						uIMarket_BoxData.IsAdItem = true;
					}
					_uiMarketContainer.SetBoxData(_uiMarketContainer.AddBox(), uIMarket_BoxData);
					UIGolbalStaticFun.GetAvatarSuitTex(new CSuitMD5((uint)uIMarket_BoxData.ItemId, lstShopList[i].m_unit), uIMarket_BoxData);
					uIMarket_BoxData.SetDirty();
					if (type == 2)
					{
						_lstBestAvatarCache.Add(uIMarket_BoxData);
					}
					else if (type == 1)
					{
						_lstAdAvatarCache.Add(uIMarket_BoxData);
					}
					else if (type == 0)
					{
						_lstRandomAvatarCache.Add(uIMarket_BoxData);
					}
				}
				if (lstShopList.Count > 0 && needSetRefreshBtn)
				{
					int num = _uiMarketContainer.AddBox();
					UIMarket_BoxData data = new UIMarket_BoxData
					{
						DataType = 0,
						BoxOwerInCaptionType = type
					};
					_uiMarketContainer.SetBoxData(num, data);
					LimitContainerMove(num + 1);
				}
			});
		}

		protected void GetCacheShopListByType(byte type, bool needSetRefreshBtn)
		{
			List<UIMarket_BoxData> list = null;
			switch (type)
			{
			case 2:
				list = _lstBestAvatarCache;
				break;
			case 1:
				list = _lstAdAvatarCache;
				break;
			case 0:
				list = _lstRandomAvatarCache;
				break;
			}
			for (int i = 0; i < list.Count; i++)
			{
				UIMarket_BoxData uIMarket_BoxData = new UIMarket_BoxData(list[i]);
				if (type == 1)
				{
					uIMarket_BoxData.IsAdItem = true;
				}
				_uiMarketContainer.SetBoxData(_uiMarketContainer.AddBox(), uIMarket_BoxData);
				uIMarket_BoxData.SetDirty();
			}
			if (list.Count > 0 && needSetRefreshBtn)
			{
				int num = _uiMarketContainer.AddBox();
				UIMarket_BoxData uIMarket_BoxData2 = new UIMarket_BoxData();
				uIMarket_BoxData2.DataType = 0;
				uIMarket_BoxData2.BoxOwerInCaptionType = type;
				_uiMarketContainer.SetBoxData(num, uIMarket_BoxData2);
				LimitContainerMove(num + 1);
			}
			else
			{
				LimitContainerMove(0);
			}
		}

		private void RefreshMarketContainer()
		{
			_label_favourites_player.enabled = false;
			_label_favourites_avatar.enabled = false;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, null);
			if (IsFavouritesLabel())
			{
				RefreshMarketContainer_Favourites();
				return;
			}
			byte b = 31;
			if (CurTabLeftBtnType == ETabLeftButtonsType.Accessory)
			{
				b = (byte)COMA_DataConfig.Instance.GetSysShopCount_Accessories();
			}
			else if (CurTabLeftBtnType == ETabLeftButtonsType.Template)
			{
				b = (byte)COMA_DataConfig.Instance.GetSysShopCount_Model();
			}
			byte boxCount = b;
			if (IsNeedRefreshBtn())
			{
				boxCount++;
			}
			_uiMarketContainer.InitContainer(UI_Container.EBoxSelType.Single);
			if (CurTabLeftBtnType == ETabLeftButtonsType.Avatar_Best)
			{
				Debug.Log("FetchShopList");
				_uiMarketContainer.ClearContainer();
				if (_lstBestAvatarCache.Count <= 0)
				{
					GetShopListByType(2, b, 0, true);
				}
				else
				{
					GetCacheShopListByType(2, true);
				}
				return;
			}
			if (CurTabLeftBtnType == ETabLeftButtonsType.Avatar)
			{
				_uiMarketContainer.ClearContainer();
				Debug.Log("GetShopListByType:kAd");
				if (_lstAdAvatarCache.Count <= 0)
				{
					GetShopListByType(1, 8, 0, false);
				}
				else
				{
					GetCacheShopListByType(1, false);
				}
				Debug.Log("GetShopListByType:kRand");
				if (_lstRandomAvatarCache.Count <= 0)
				{
					GetShopListByType(0, (byte)(b - 8), 8, true);
				}
				else
				{
					GetCacheShopListByType(0, true);
				}
				return;
			}
			if (CurTabLeftBtnType == ETabLeftButtonsType.Gem)
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
				{
					UIGolbalStaticFun.CloseBlockOnlyMessageBox();
					int dayOfWeek = UIGolbalStaticFun.GetDayOfWeek(time);
					_gemShopDay = dayOfWeek;
					Debug.Log("Day=" + dayOfWeek);
					Debug.Log("RpgGemShop=" + RPGGlobalData.Instance.RpgGemShop._gemShopUnitPool.Count);
					List<RPGGemShopUnit> list = RPGGlobalData.Instance.RpgGemShop._gemShopUnitPool[dayOfWeek];
					boxCount = (byte)list.Count;
					_uiMarketContainer.InitBoxs(boxCount, false);
					for (int i = 0; i < boxCount; i++)
					{
						UIMarket_BoxData uIMarket_BoxData = new UIMarket_BoxData
						{
							DataType = 4,
							ItemId = (ulong)list[i]._gemId,
							InListID = list[i]._itemId,
							CurrencyType = (ECurrencyType)(list[i]._currencyType + 1),
							Price = list[i]._totalPrice
						};
						RPGGemDefineUnit.EGemColor gemColorByID = RPGGemDefineUnit.GetGemColorByID((int)uIMarket_BoxData.ItemId);
						int gemGradeByID = RPGGemDefineUnit.GetGemGradeByID((int)uIMarket_BoxData.ItemId);
						uIMarket_BoxData.SpriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel((int)gemColorByID, gemGradeByID);
						_uiMarketContainer.SetBoxData(i, uIMarket_BoxData);
					}
					LimitContainerMove(boxCount);
				});
				return;
			}
			_uiMarketContainer.InitBoxs(boxCount, false);
			for (int num = 0; num < boxCount; num++)
			{
				UIMarket_BoxData data = new UIMarket_BoxData();
				if (CurTabLeftBtnType == ETabLeftButtonsType.Accessory)
				{
					COMA_DataConfig.Instance.GetSysShopData_Accessories(num, ref data);
					data.SpriteName = "deco_" + data.Unit;
				}
				else if (CurTabLeftBtnType == ETabLeftButtonsType.Template)
				{
					Debug.Log("---------------Refresh");
					COMA_DataConfig.Instance.GetSysShopData_Model(num, ref data);
					if (data.Unit == "Head01")
					{
						UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Head, string.Empty, data);
					}
					else if (data.Unit == "Body01")
					{
						UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Body, string.Empty, data);
					}
					else if (data.Unit == "Leg01")
					{
						UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Leg, string.Empty, data);
					}
					else if (data.Unit == "HBL01")
					{
						UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_None, string.Empty, data);
					}
				}
				_uiMarketContainer.SetBoxData(num, data);
			}
			LimitContainerMove(boxCount);
		}

		private void LimitContainerMove(int nBoxCount)
		{
			if (nBoxCount > 16)
			{
				_uiMarketContainer.SetMoveForce(new Vector3(0f, 1f, 0f));
			}
			else
			{
				_uiMarketContainer.SetMoveForce(Vector3.zero);
			}
		}

		private void RefreshMarketContainer_Favourites()
		{
			List<uint> follow_list = ((NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role)).m_follow_list;
			int count = follow_list.Count;
			List<uint> lstFav = ((NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role)).m_collect_list;
			int boxCountFav = lstFav.Count;
			_uiMarketContainer.InitContainer(UI_Container.EBoxSelType.Single);
			if (CurTabLeftBtnType == ETabLeftButtonsType.Favourites_Icon)
			{
				_label_favourites_player.enabled = ((count <= 0) ? true : false);
				_label_favourites_avatar.enabled = false;
				_uiMarketContainer.InitBoxs(count, false);
				for (int i = 0; i < count; i++)
				{
					UIMarket_BoxData data = new UIMarket_BoxData();
					data.DataType = 3;
					data.ItemId = follow_list[i];
					UIDataBufferCenter.Instance.FetchPlayerProfile(follow_list[i], delegate(WatchRoleInfo playerInfo)
					{
						Debug.Log("player facebook icon md5 : " + playerInfo.m_face_image_md5);
						data.Name = playerInfo.m_name;
						UIDataBufferCenter.Instance.FetchFacebookIconByTID(playerInfo.m_player_id, delegate(Texture2D tex2D)
						{
							data.Tex = tex2D;
							data.SetDirty();
						});
						data.SetDirty();
					});
					_uiMarketContainer.SetBoxData(i, data);
				}
				LimitContainerMove(count);
			}
			else
			{
				if (CurTabLeftBtnType != ETabLeftButtonsType.Favourites_Avatar)
				{
					return;
				}
				_uiMarketContainer.ClearContainer();
				_label_favourites_player.enabled = false;
				_label_favourites_avatar.enabled = ((boxCountFav <= 0) ? true : false);
				Debug.Log("Collect Avatar Count=" + boxCountFav);
				UIDataBufferCenter.Instance.FetchSelfCollectAvatarLst(delegate(List<ShopItem> lstShopList)
				{
					Debug.Log("Fetch Collect Avatar Count=" + lstShopList.Count);
					int num = 0;
					List<uint> list = new List<uint>();
					for (int j = 0; j < lstShopList.Count; j++)
					{
						list.Add(lstShopList[j].m_id);
					}
					for (int k = 0; k < boxCountFav; k++)
					{
						if (!list.Contains(lstFav[k]))
						{
							num++;
							UncollectAvatarCmd extraInfo = new UncollectAvatarCmd
							{
								m_id = lstFav[k],
								m_param = 0
							};
							UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
						}
					}
					if (num > 0)
					{
						string str = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc30"), num);
						UIGolbalStaticFun.PopupTipsBox(str);
					}
					for (int l = 0; l < Mathf.Min(boxCountFav, lstShopList.Count); l++)
					{
						UIMarket_BoxData uIMarket_BoxData = new UIMarket_BoxData
						{
							DataType = 1,
							ItemId = lstShopList[l].m_id,
							Price = (int)lstShopList[l].m_price,
							CurrencyType = (ECurrencyType)lstShopList[l].m_price_type,
							AuthorId = lstShopList[l].m_author,
							RemainNum = lstShopList[l].m_remain_num,
							PraiseNum = lstShopList[l].m_praise,
							Units = lstShopList[l].m_unit
						};
						UIGolbalStaticFun.GetAvatarSuitTex(new CSuitMD5((uint)uIMarket_BoxData.ItemId, lstShopList[l].m_unit), uIMarket_BoxData);
						_uiMarketContainer.SetBoxData(_uiMarketContainer.AddBox(), uIMarket_BoxData);
					}
					LimitContainerMove(lstShopList.Count);
				});
			}
		}

		private void RefreshMarketShoppingCartContainer()
		{
			int num = _lstShoppingCart.Count + _lstShoppingCart_System.Count;
			Debug.Log("boxCount=" + num);
			_uiMarketShoppingCartContainer.InitContainer(UI_Container.EBoxSelType.Single);
			_uiMarketShoppingCartContainer.InitBoxs(num, true);
			int num2 = 0;
			foreach (uint key in _lstShoppingCart.Keys)
			{
				UIMarket_CartBoxData data = new UIMarket_CartBoxData(_lstShoppingCart[key]);
				_uiMarketShoppingCartContainer.SetBoxData(num2++, data);
			}
			foreach (string key2 in _lstShoppingCart_System.Keys)
			{
				UIMarket_CartBoxData data2 = new UIMarket_CartBoxData(_lstShoppingCart_System[key2]);
				_uiMarketShoppingCartContainer.SetBoxData(num2++, data2);
			}
			_uiMarketShoppingCartContainer.RefreshPrice();
			if (num > 10)
			{
				_uiMarketShoppingCartContainer.SetMoveForce(new Vector3(0f, 1f, 0f));
			}
			else
			{
				_uiMarketShoppingCartContainer.SetMoveForce(Vector3.zero);
			}
			bool isChecked = IsShopCartAvatarCanEI();
			_uiMarketShoppingCartContainer._equipedCheck.isChecked = isChecked;
			_uiMarketShoppingCartContainer._equipedCheck.gameObject.SetActive(isChecked);
		}

		private void RefreshMarketFittingRoomContainer()
		{
			int num = _lstShoppingCart.Count + _lstShoppingCart_System.Count;
			Debug.Log("Fitting room boxCount=" + num);
			_uiMarketFittingRoomContainer.InitContainer(UI_Container.EBoxSelType.Multi);
			_uiMarketFittingRoomContainer.ClearContainer();
			List<UIMarket_FittingBoxData> list = new List<UIMarket_FittingBoxData>();
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			RoleInfo info = notifyRoleDataCmd.m_info;
			BagData bag_data = notifyRoleDataCmd.m_bag_data;
			if (info.m_head != 0L)
			{
				Debug.Log(info.m_head);
				BagItem bagItemByID = UIGolbalStaticFun.GetBagItemByID(info.m_head);
				if (bagItemByID != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData.Unit = bagItemByID.m_unit;
					uIMarket_FittingBoxData.SingleAvatar = 0;
					UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Head, uIMarket_FittingBoxData.Unit, uIMarket_FittingBoxData);
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData);
					list.Add(uIMarket_FittingBoxData);
				}
			}
			if (info.m_body != 0L)
			{
				BagItem bagItemByID2 = UIGolbalStaticFun.GetBagItemByID(info.m_body);
				if (bagItemByID2 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData2 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData2.Unit = bagItemByID2.m_unit;
					uIMarket_FittingBoxData2.SingleAvatar = 1;
					UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Body, uIMarket_FittingBoxData2.Unit, uIMarket_FittingBoxData2);
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData2);
					list.Add(uIMarket_FittingBoxData2);
				}
			}
			if (info.m_leg != 0L)
			{
				BagItem bagItemByID3 = UIGolbalStaticFun.GetBagItemByID(info.m_leg);
				if (bagItemByID3 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData3 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData3.Unit = bagItemByID3.m_unit;
					uIMarket_FittingBoxData3.SingleAvatar = 2;
					UIGolbalStaticFun.GetAvatarPartTex(EAvatarPart.Avatar_Leg, uIMarket_FittingBoxData3.Unit, uIMarket_FittingBoxData3);
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData3);
					list.Add(uIMarket_FittingBoxData3);
				}
			}
			if (info.m_head_top != 0L)
			{
				BagItem bagItemByID4 = UIGolbalStaticFun.GetBagItemByID(info.m_head_top);
				if (bagItemByID4 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData4 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData4.Unit = bagItemByID4.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData4);
					list.Add(uIMarket_FittingBoxData4);
				}
			}
			if (info.m_head_front != 0L)
			{
				BagItem bagItemByID5 = UIGolbalStaticFun.GetBagItemByID(info.m_head_front);
				if (bagItemByID5 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData5 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData5.Unit = bagItemByID5.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData5);
					list.Add(uIMarket_FittingBoxData5);
				}
			}
			if (info.m_head_back != 0L)
			{
				BagItem bagItemByID6 = UIGolbalStaticFun.GetBagItemByID(info.m_head_back);
				if (bagItemByID6 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData6 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData6.Unit = bagItemByID6.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData6);
					list.Add(uIMarket_FittingBoxData6);
				}
			}
			if (info.m_head_left != 0L)
			{
				BagItem bagItemByID7 = UIGolbalStaticFun.GetBagItemByID(info.m_head_left);
				if (bagItemByID7 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData7 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData7.Unit = bagItemByID7.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData7);
					list.Add(uIMarket_FittingBoxData7);
				}
			}
			if (info.m_head_right != 0L)
			{
				BagItem bagItemByID8 = UIGolbalStaticFun.GetBagItemByID(info.m_head_right);
				if (bagItemByID8 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData8 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData8.Unit = bagItemByID8.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData8);
					list.Add(uIMarket_FittingBoxData8);
				}
			}
			if (info.m_chest_front != 0L)
			{
				BagItem bagItemByID9 = UIGolbalStaticFun.GetBagItemByID(info.m_chest_front);
				if (bagItemByID9 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData9 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData9.Unit = bagItemByID9.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData9);
					list.Add(uIMarket_FittingBoxData9);
				}
			}
			if (info.m_chest_back != 0L)
			{
				BagItem bagItemByID10 = UIGolbalStaticFun.GetBagItemByID(info.m_chest_back);
				if (bagItemByID10 != null)
				{
					UIMarket_FittingBoxData uIMarket_FittingBoxData10 = new UIMarket_FittingBoxData();
					uIMarket_FittingBoxData10.Unit = bagItemByID10.m_unit;
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), uIMarket_FittingBoxData10);
					list.Add(uIMarket_FittingBoxData10);
				}
			}
			foreach (uint key in _lstShoppingCart.Keys)
			{
				UIMarket_FittingBoxData data = new UIMarket_FittingBoxData(_lstShoppingCart[key]);
				_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), data);
			}
			foreach (string key2 in _lstShoppingCart_System.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData = _lstShoppingCart_System[key2];
				Debug.Log("Fitting room  Unit=" + uIMarket_BoxData.Unit);
				if (!UIGolbalStaticFun.IsSystemShopTmp(uIMarket_BoxData.Unit))
				{
					UIMarket_FittingBoxData data2 = new UIMarket_FittingBoxData(_lstShoppingCart_System[key2]);
					_uiMarketFittingRoomContainer.SetBoxData(_uiMarketFittingRoomContainer.AddBox(), data2);
				}
			}
			_uiMarketFittingRoomContainer.InitFittingRoom(list);
		}

		private void Awake()
		{
			ClearMarketCache();
		}

		private void Start()
		{
		}

		protected override void Tick()
		{
			if (_bNeedRefreshCharcProfile)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Charc);
				_bNeedRefreshCharcProfile = false;
			}
		}

		private void ClearMarketCache()
		{
			_lstBestAvatarCache.Clear();
			_lstAdAvatarCache.Clear();
			_lstRandomAvatarCache.Clear();
		}

		private bool IsShopCartAvatarReplace(byte avatar1, byte avatar2)
		{
			int num = avatar1 & 4;
			int num2 = avatar1 & 2;
			int num3 = avatar1 & 1;
			int num4 = avatar2 & 4;
			int num5 = avatar2 & 2;
			int num6 = avatar2 & 1;
			return (num4 >= num && num5 >= num2 && num6 >= num3) ? true : false;
		}

		private bool IsShopCartAvatarCanEI()
		{
			int num = 0;
			foreach (uint key in _lstShoppingCart.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData = _lstShoppingCart[key];
				num += uIMarket_BoxData.AvatarAttribute;
			}
			bool flag = false;
			foreach (string key2 in _lstShoppingCart_System.Keys)
			{
				UIMarket_BoxData uIMarket_BoxData2 = _lstShoppingCart_System[key2];
				if (UIGolbalStaticFun.IsSystemShopTmp(uIMarket_BoxData2.Unit))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return false;
			}
			return (num <= 7) ? true : false;
		}
	}
}
