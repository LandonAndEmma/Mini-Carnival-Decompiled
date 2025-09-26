using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using Protocol.Shop.C2S;
using UIGlobal;
using UnityEngine;

public class UIBackpack : UIEntity
{
	private enum EAfterSyncOperType
	{
		None = 0,
		Goto_Market = 1,
		Goto_SellItem = 2,
		DelItem = 3,
		Goto_Square = 4
	}

	public enum EItemType
	{
		None = 0,
		All = 1,
		Avatar = 2,
		Decoration = 3
	}

	public enum ECaptionType
	{
		Backpack = 0,
		SellList = 1
	}

	private RoleInfo _enterBackpackBeforeRoleInfo = new RoleInfo();

	[SerializeField]
	private EItemType _curItemTypeBtnType;

	private EItemType _preItemTypeBtnType;

	private EAfterSyncOperType _curAfterSyncOperType;

	private ECaptionType _curCaptionType;

	[SerializeField]
	private GameObject _pageBackpack;

	[SerializeField]
	private GameObject _pageBackpack_Btns;

	[SerializeField]
	private GameObject _pageSell;

	[SerializeField]
	private GameObject _pageSell_Btns;

	[SerializeField]
	private GameObject _backpackBtns;

	[SerializeField]
	private GameObject _sellListBtns;

	[SerializeField]
	private UILabel _labelTips;

	[SerializeField]
	private UIBackpack_Container _uiBackpackContainer;

	[SerializeField]
	private UIBackpack_SellInfoContainer _uiSellContainer;

	public EItemType CurItemTypeBtnType
	{
		get
		{
			return _curItemTypeBtnType;
		}
		set
		{
			_curItemTypeBtnType = value;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIBackpack_ItemTypeButtonSelChanged, this, ItemTypeButtonSelChanged);
		RegisterMessage(EUIMessageID.UIBackpack_CaptionTypeSwitched, this, CaptionTypeSwitched);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleDataChanged, this, RoleDataChanged);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this, RoleDataBagDataChanged);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, RoleDataRoleInfoChanged);
		RegisterMessage(EUIMessageID.UIBackpack_GotoMarketClick, this, GotoMarketClick);
		RegisterMessage(EUIMessageID.UIBackpack_GotoSellItems, this, GotoSellItems);
		RegisterMessage(EUIMessageID.UI_CloseBlockBox, this, CloseBlockBox);
		RegisterMessage(EUIMessageID.UIBackpack_DelItem, this, DelItem);
		RegisterMessage(EUIMessageID.UIBackpack_ButtonGotoSquare, this, ButtonGotoSquare);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess, this, UnlockBagCellSuccess);
		RegisterMessage(EUIMessageID.UIBackpack_DelAvatarList, this, DelAvatarList);
		RegisterMessage(EUIMessageID.UIBackpack_Reshelf, this, Reshelf);
		RegisterMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, RefreshAvatarList);
		RegisterMessage(EUIMessageID.UIBackpack_AdAvatar, this, AdAvatar);
		CopySrvRoleInfoDataToCache();
		UIDataBufferCenter.Instance.FetchSelfExtInfoByID(delegate(ExtInfo extInfo)
		{
			if (extInfo != null)
			{
				Debug.Log("卖出avatar数:" + extInfo.m_sells_num);
				int sells_num = (int)extInfo.m_sells_num;
				COMA_Achievement.Instance.GetRich = sells_num;
				COMA_Achievement.Instance.Drawer = sells_num;
				COMA_Achievement.Instance.Artist = sells_num;
				COMA_Achievement.Instance.DrawingExpert = sells_num;
				COMA_Achievement.Instance.DrawingMaster = sells_num;
				COMA_Achievement.Instance.DrawingGrandMaster = sells_num;
			}
		});
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIBackpack_ItemTypeButtonSelChanged, this);
		UnregisterMessage(EUIMessageID.UIBackpack_CaptionTypeSwitched, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleDataChanged, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this);
		UnregisterMessage(EUIMessageID.UIBackpack_GotoMarketClick, this);
		UnregisterMessage(EUIMessageID.UIBackpack_GotoSellItems, this);
		UnregisterMessage(EUIMessageID.UI_CloseBlockBox, this);
		UnregisterMessage(EUIMessageID.UIBackpack_DelItem, this);
		UnregisterMessage(EUIMessageID.UIBackpack_ButtonGotoSquare, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess, this);
		UnregisterMessage(EUIMessageID.UIBackpack_DelAvatarList, this);
		UnregisterMessage(EUIMessageID.UIBackpack_Reshelf, this);
		UnregisterMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this);
		UnregisterMessage(EUIMessageID.UIBackpack_AdAvatar, this);
		InitItemTypeBtnType();
	}

	private void CopySrvRoleInfoDataToCache()
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		_enterBackpackBeforeRoleInfo.m_head = info.m_head;
		_enterBackpackBeforeRoleInfo.m_body = info.m_body;
		_enterBackpackBeforeRoleInfo.m_leg = info.m_leg;
		_enterBackpackBeforeRoleInfo.m_head_top = info.m_head_top;
		_enterBackpackBeforeRoleInfo.m_head_front = info.m_head_front;
		_enterBackpackBeforeRoleInfo.m_head_back = info.m_head_back;
		_enterBackpackBeforeRoleInfo.m_head_left = info.m_head_left;
		_enterBackpackBeforeRoleInfo.m_head_right = info.m_head_right;
		_enterBackpackBeforeRoleInfo.m_chest_front = info.m_chest_front;
		_enterBackpackBeforeRoleInfo.m_chest_back = info.m_chest_back;
		Debug.Log("......_enterBackpackBeforeRoleInfo.m_head=" + _enterBackpackBeforeRoleInfo.m_head);
	}

	private int SynEquipedDataToSrv()
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		int num = 0;
		int num2 = 0;
		RoleInfo roleInfo = new RoleInfo();
		roleInfo.m_head = ulong.MaxValue;
		roleInfo.m_body = ulong.MaxValue;
		roleInfo.m_leg = ulong.MaxValue;
		roleInfo.m_head_top = ulong.MaxValue;
		roleInfo.m_head_front = ulong.MaxValue;
		roleInfo.m_head_back = ulong.MaxValue;
		roleInfo.m_head_left = ulong.MaxValue;
		roleInfo.m_head_right = ulong.MaxValue;
		roleInfo.m_chest_front = ulong.MaxValue;
		roleInfo.m_chest_back = ulong.MaxValue;
		Debug.Log("_enterBackpackBeforeRoleInfo.m_head=" + _enterBackpackBeforeRoleInfo.m_head + ",CurHead=" + info.m_head);
		if (_enterBackpackBeforeRoleInfo.m_head != info.m_head)
		{
			roleInfo.m_head = info.m_head;
			if (info.m_head != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_body != info.m_body)
		{
			roleInfo.m_body = info.m_body;
			if (info.m_body != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_leg != info.m_leg)
		{
			roleInfo.m_leg = info.m_leg;
			if (info.m_leg != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_head_top != info.m_head_top)
		{
			roleInfo.m_head_top = info.m_head_top;
			if (info.m_head_top != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_head_front != info.m_head_front)
		{
			roleInfo.m_head_front = info.m_head_front;
			if (info.m_head_front != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_head_back != info.m_head_back)
		{
			roleInfo.m_head_back = info.m_head_back;
			if (info.m_head_back != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_head_left != info.m_head_left)
		{
			roleInfo.m_head_left = info.m_head_left;
			if (info.m_head_left != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_head_right != info.m_head_right)
		{
			roleInfo.m_head_right = info.m_head_right;
			if (info.m_head_right != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_chest_front != info.m_chest_front)
		{
			roleInfo.m_chest_front = info.m_chest_front;
			if (info.m_chest_front != 0L)
			{
				num2++;
			}
			num++;
		}
		if (_enterBackpackBeforeRoleInfo.m_chest_back != info.m_chest_back)
		{
			roleInfo.m_chest_back = info.m_chest_back;
			if (info.m_chest_back != 0L)
			{
				num2++;
			}
			num++;
		}
		if (num > 0)
		{
			num2 = 0;
			if (roleInfo.m_head != 0L && roleInfo.m_head != ulong.MaxValue)
			{
				Debug.Log("Equiped Head = " + roleInfo.m_head);
				MountBagItemCmd mountBagItemCmd = new MountBagItemCmd();
				mountBagItemCmd.m_unique_id = roleInfo.m_head;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd);
				num2++;
			}
			else if (roleInfo.m_head == 0L && _enterBackpackBeforeRoleInfo.m_head != 0L)
			{
				Debug.Log("UnEquiped Head = " + roleInfo.m_head);
				MountBagItemCmd mountBagItemCmd2 = new MountBagItemCmd();
				mountBagItemCmd2.m_unique_id = roleInfo.m_head;
				mountBagItemCmd2.m_mount_part = 7;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd2);
				num2++;
			}
			if (roleInfo.m_body != 0L && roleInfo.m_body != ulong.MaxValue)
			{
				MountBagItemCmd mountBagItemCmd3 = new MountBagItemCmd();
				mountBagItemCmd3.m_unique_id = roleInfo.m_body;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd3);
				num2++;
			}
			else if (roleInfo.m_body == 0L && _enterBackpackBeforeRoleInfo.m_body != 0L)
			{
				Debug.Log("UnEquiped Body = " + roleInfo.m_body);
				MountBagItemCmd mountBagItemCmd4 = new MountBagItemCmd();
				mountBagItemCmd4.m_unique_id = roleInfo.m_body;
				mountBagItemCmd4.m_mount_part = 8;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd4);
				num2++;
			}
			if (roleInfo.m_leg != 0L && roleInfo.m_leg != ulong.MaxValue)
			{
				MountBagItemCmd mountBagItemCmd5 = new MountBagItemCmd();
				mountBagItemCmd5.m_unique_id = roleInfo.m_leg;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd5);
				num2++;
			}
			else if (roleInfo.m_leg == 0L && _enterBackpackBeforeRoleInfo.m_leg != 0L)
			{
				Debug.Log("UnEquiped Leg = " + roleInfo.m_leg);
				MountBagItemCmd mountBagItemCmd6 = new MountBagItemCmd();
				mountBagItemCmd6.m_unique_id = roleInfo.m_leg;
				mountBagItemCmd6.m_mount_part = 9;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd6);
				num2++;
			}
			if ((roleInfo.m_head_top != 0L && roleInfo.m_head_top != ulong.MaxValue) || (roleInfo.m_head_top == 0L && _enterBackpackBeforeRoleInfo.m_head_top != 0L))
			{
				MountBagItemCmd mountBagItemCmd7 = new MountBagItemCmd();
				mountBagItemCmd7.m_unique_id = roleInfo.m_head_top;
				mountBagItemCmd7.m_mount_part = 0;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd7);
				num2++;
			}
			if ((roleInfo.m_head_front != 0L && roleInfo.m_head_front != ulong.MaxValue) || (roleInfo.m_head_front == 0L && _enterBackpackBeforeRoleInfo.m_head_front != 0L))
			{
				MountBagItemCmd mountBagItemCmd8 = new MountBagItemCmd();
				mountBagItemCmd8.m_unique_id = roleInfo.m_head_front;
				mountBagItemCmd8.m_mount_part = 1;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd8);
				num2++;
			}
			if ((roleInfo.m_head_back != 0L && roleInfo.m_head_back != ulong.MaxValue) || (roleInfo.m_head_back == 0L && _enterBackpackBeforeRoleInfo.m_head_back != 0L))
			{
				MountBagItemCmd mountBagItemCmd9 = new MountBagItemCmd();
				mountBagItemCmd9.m_unique_id = roleInfo.m_head_back;
				mountBagItemCmd9.m_mount_part = 2;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd9);
				num2++;
			}
			if ((roleInfo.m_head_left != 0L && roleInfo.m_head_left != ulong.MaxValue) || (roleInfo.m_head_left == 0L && _enterBackpackBeforeRoleInfo.m_head_left != 0L))
			{
				MountBagItemCmd mountBagItemCmd10 = new MountBagItemCmd();
				mountBagItemCmd10.m_unique_id = roleInfo.m_head_left;
				mountBagItemCmd10.m_mount_part = 3;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd10);
				num2++;
			}
			if ((roleInfo.m_head_right != 0L && roleInfo.m_head_right != ulong.MaxValue) || (roleInfo.m_head_right == 0L && _enterBackpackBeforeRoleInfo.m_head_right != 0L))
			{
				MountBagItemCmd mountBagItemCmd11 = new MountBagItemCmd();
				mountBagItemCmd11.m_unique_id = roleInfo.m_head_right;
				mountBagItemCmd11.m_mount_part = 4;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd11);
				num2++;
			}
			if ((roleInfo.m_chest_front != 0L && roleInfo.m_chest_front != ulong.MaxValue) || (roleInfo.m_chest_front == 0L && _enterBackpackBeforeRoleInfo.m_chest_front != 0L))
			{
				MountBagItemCmd mountBagItemCmd12 = new MountBagItemCmd();
				mountBagItemCmd12.m_unique_id = roleInfo.m_chest_front;
				mountBagItemCmd12.m_mount_part = 5;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd12);
				num2++;
			}
			if ((roleInfo.m_chest_back != 0L && roleInfo.m_chest_back != ulong.MaxValue) || (roleInfo.m_chest_back == 0L && _enterBackpackBeforeRoleInfo.m_chest_back != 0L))
			{
				MountBagItemCmd mountBagItemCmd13 = new MountBagItemCmd();
				mountBagItemCmd13.m_unique_id = roleInfo.m_chest_back;
				mountBagItemCmd13.m_mount_part = 6;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, mountBagItemCmd13);
				num2++;
			}
			UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(num2);
		}
		return num;
	}

	public void InitItemTypeBtnType()
	{
		_curItemTypeBtnType = EItemType.None;
		_preItemTypeBtnType = EItemType.None;
	}

	private bool ItemTypeButtonSelChanged(TUITelegram msg)
	{
		CurItemTypeBtnType = (EItemType)(int)msg._pExtraInfo;
		if (_preItemTypeBtnType != CurItemTypeBtnType)
		{
			RefreshBackpackContainer(true);
		}
		_preItemTypeBtnType = CurItemTypeBtnType;
		GameObject gameObject = (GameObject)msg._pExtraInfo2;
		gameObject.GetComponent<UICheckbox>().isChecked = true;
		return true;
	}

	private bool CaptionTypeSwitched(TUITelegram msg)
	{
		switch ((UIBackpack_ButtonCaptionType.ECaptionType)(int)msg._pExtraInfo)
		{
		case UIBackpack_ButtonCaptionType.ECaptionType.Backpack_Unsel:
			_pageBackpack.SetActive(true);
			_pageBackpack_Btns.SetActive(true);
			_pageSell.SetActive(false);
			_pageSell_Btns.SetActive(false);
			_backpackBtns.SetActive(true);
			_sellListBtns.SetActive(false);
			RefreshBackpackContainer(true);
			_curCaptionType = ECaptionType.Backpack;
			break;
		case UIBackpack_ButtonCaptionType.ECaptionType.Sell_Unsel:
			_curCaptionType = ECaptionType.SellList;
			_pageBackpack.SetActive(false);
			_pageBackpack_Btns.SetActive(false);
			_pageSell.SetActive(true);
			_pageSell_Btns.SetActive(true);
			_backpackBtns.SetActive(false);
			_sellListBtns.SetActive(true);
			RefreshSellContainer();
			if (COMA_Pref.Instance.NG2_FirstEnterSellItem)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstUseSellBtn, this, null);
			}
			break;
		}
		return true;
	}

	private bool RoleDataChanged(TUITelegram msg)
	{
		if (_pageBackpack.activeSelf)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)msg._pExtraInfo;
			if (notifyRoleDataCmd != null)
			{
				bool reCreated = true;
				if (_uiBackpackContainer.CurSelBox != null && _uiBackpackContainer.CurSelBox.BoxData != null)
				{
					reCreated = false;
				}
				RefreshBackpackContainer(reCreated);
			}
		}
		return true;
	}

	private bool RoleDataBagDataChanged(TUITelegram msg)
	{
		return RoleDataChanged(msg);
	}

	private bool RoleDataRoleInfoChanged(TUITelegram msg)
	{
		return RoleDataChanged(msg);
	}

	private bool RealGotoSquareScene(object obj)
	{
		Debug.Log("RealGotoSquareScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private void RealGotoSquare()
	{
		Debug.Log("RealGotoSquare");
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, RealGotoSquareScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
	}

	private bool RealGotoMarketScene(object obj)
	{
		Debug.Log("RealGotoMarketScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Market", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private void RealGotoMarket()
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, RealGotoMarketScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
	}

	private bool RealGotoSellItemScene(object obj)
	{
		Debug.Log("RealGotoSellItemScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.SellItems", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private void RealGotoSellItem()
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, RealGotoSellItemScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
	}

	private void RealDelItem()
	{
		if (_uiBackpackContainer.CurSelBox != null && _uiBackpackContainer.CurSelBox.BoxData != null)
		{
			UIBackpack_BoxData uIBackpack_BoxData = (UIBackpack_BoxData)_uiBackpackContainer.CurSelBox.BoxData;
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			DelBagItemCmd delBagItemCmd = new DelBagItemCmd();
			delBagItemCmd.m_unique_id = uIBackpack_BoxData.ItemId;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delBagItemCmd);
			_uiBackpackContainer.ClearCurSelBox();
		}
	}

	private bool GotoMarketClick(TUITelegram msg)
	{
		int num = SynEquipedDataToSrv();
		if (num > 0)
		{
			_curAfterSyncOperType = EAfterSyncOperType.Goto_Market;
		}
		else
		{
			RealGotoMarket();
		}
		return true;
	}

	private bool CloseBlockBox(TUITelegram msg)
	{
		switch (_curAfterSyncOperType)
		{
		case EAfterSyncOperType.Goto_Market:
			CopySrvRoleInfoDataToCache();
			RealGotoMarket();
			break;
		case EAfterSyncOperType.Goto_SellItem:
			CopySrvRoleInfoDataToCache();
			RealGotoSellItem();
			break;
		case EAfterSyncOperType.DelItem:
			CopySrvRoleInfoDataToCache();
			RealDelItem();
			break;
		case EAfterSyncOperType.Goto_Square:
			CopySrvRoleInfoDataToCache();
			RealGotoSquare();
			break;
		}
		_curAfterSyncOperType = EAfterSyncOperType.None;
		return true;
	}

	private bool GotoSellItems(TUITelegram msg)
	{
		int num = SynEquipedDataToSrv();
		if (num > 0)
		{
			_curAfterSyncOperType = EAfterSyncOperType.Goto_SellItem;
		}
		else
		{
			RealGotoSellItem();
		}
		return true;
	}

	private bool ButtonGotoSquare(TUITelegram msg)
	{
		Debug.Log("ButtonGotoSquare----");
		int num = SynEquipedDataToSrv();
		if (num > 0)
		{
			_curAfterSyncOperType = EAfterSyncOperType.Goto_Square;
		}
		else
		{
			RealGotoSquare();
		}
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_UpdateSelfAvatarOnSquare, null, null);
		return true;
	}

	private bool DelItem(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("beibaojiemian_desc5"));
		uIMessage_CommonBoxData.Mark = "PM_ConfirmDiscardItem";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "PM_ConfirmDiscardItem":
			ProcessDelItem();
			break;
		case "DelAvatarList":
			RealDelAvatarList();
			break;
		}
		return true;
	}

	private bool UnlockBagCellSuccess(TUITelegram msg)
	{
		byte b = (byte)msg._pExtraInfo;
		if (_uiBackpackContainer.LstBoxs[b - 1].BoxData.DataType != 0)
		{
			Debug.LogError("---------UnlockBagCellSuccess-----------------Error!");
		}
		_uiBackpackContainer.LstBoxs[b - 1].BoxData = null;
		return true;
	}

	private bool DelAvatarList(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("jiaoyijiemian_desc21"));
		uIMessage_CommonBoxData.Mark = "DelAvatarList";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	private void RealDelAvatarList()
	{
		if (_uiSellContainer.CurSelBox != null && _uiSellContainer.CurSelBox.BoxData != null)
		{
			UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = (UIBackpack_SellInfoBoxData)_uiSellContainer.CurSelBox.BoxData;
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			DelAvatarCmd delAvatarCmd = new DelAvatarCmd();
			delAvatarCmd.m_id = (uint)uIBackpack_SellInfoBoxData.ItemId;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delAvatarCmd);
		}
	}

	private bool Reshelf(TUITelegram msg)
	{
		if (_uiSellContainer.CurSelBox != null && _uiSellContainer.CurSelBox.BoxData != null)
		{
			UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = (UIBackpack_SellInfoBoxData)_uiSellContainer.CurSelBox.BoxData;
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			ResellAvatarCmd resellAvatarCmd = new ResellAvatarCmd();
			resellAvatarCmd.m_id = (uint)uIBackpack_SellInfoBoxData.ItemId;
			resellAvatarCmd.m_num = (byte)COMA_DataConfig.Instance._sysConfig.Shop.item_num;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, resellAvatarCmd);
		}
		return true;
	}

	private bool RefreshAvatarList(TUITelegram msg)
	{
		RefreshSellContainer();
		return true;
	}

	private bool AdAvatar(TUITelegram msg)
	{
		if (_uiSellContainer.CurSelBox != null && _uiSellContainer.CurSelBox.BoxData != null)
		{
			UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = (UIBackpack_SellInfoBoxData)_uiSellContainer.CurSelBox.BoxData;
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			AdAvatarCmd adAvatarCmd = new AdAvatarCmd();
			adAvatarCmd.m_id = (uint)uIBackpack_SellInfoBoxData.ItemId;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, adAvatarCmd);
		}
		return true;
	}

	private void ProcessDelItem()
	{
		int num = SynEquipedDataToSrv();
		if (num > 0)
		{
			_curAfterSyncOperType = EAfterSyncOperType.DelItem;
		}
		else
		{
			RealDelItem();
		}
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
		if (UIDataBufferCenter.Instance._bSellListChanged && _curCaptionType == ECaptionType.SellList)
		{
			UIDataBufferCenter.Instance._bSellListChanged = false;
			RefreshSellContainer();
		}
	}

	private void RefreshBackpackContainer(bool reCreated)
	{
		Debug.Log("RefreshBackpackContainer + " + reCreated);
		Debug.Log("CurItemTypeBtnType=" + CurItemTypeBtnType);
		if (reCreated)
		{
			_uiBackpackContainer.EnableDesginBtn(false);
			_uiBackpackContainer.EnableDelBackpackBtn(false);
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		if (notifyRoleDataCmd != null)
		{
			UIGolbalStaticFun.ParseBackpackDataToUI(notifyRoleDataCmd.m_bag_data, _uiBackpackContainer, NGUI_COMUI.UI_Container.EBoxSelType.Single, false, CurItemTypeBtnType, reCreated);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Charc);
	}

	private void RefreshSellContainer()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_SellListSelChanged, this, null);
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIDataBufferCenter.Instance.FetchPlayerSellList(UIGolbalStaticFun.GetSelfTID(), delegate(List<ShopItem> shopItems)
		{
			UIGolbalStaticFun.CloseBlockOnlyMessageBox();
			if (shopItems != null)
			{
				int count = shopItems.Count;
				Debug.Log("Sell Lst Cout=" + count);
				_uiSellContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
				_uiSellContainer.InitBoxs(count, true);
				for (int i = 0; i < count; i++)
				{
					UIBackpack_SellInfoBoxData data = new UIBackpack_SellInfoBoxData
					{
						ADTime = shopItems[i].m_ad_time,
						ItemId = shopItems[i].m_id,
						SoldedNum = COMA_DataConfig.Instance._sysConfig.Shop.item_num - shopItems[i].m_remain_num,
						FavourNum = (int)shopItems[i].m_praise,
						BalanceNum = (int)shopItems[i].m_price
					};
					UIGolbalStaticFun.GetAvatarSuitTex(new CSuitMD5(shopItems[i].m_id, shopItems[i].m_unit), data);
					_uiSellContainer.SetBoxData(i, data);
				}
			}
			_labelTips.enabled = ((shopItems != null && shopItems.Count <= 0) ? true : false);
		});
	}
}
