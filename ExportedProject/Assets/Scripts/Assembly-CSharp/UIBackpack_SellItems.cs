using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class UIBackpack_SellItems : UIDataBufferDependency
{
	[SerializeField]
	private UIBackpack_SellItemsContainer _uiSellItemsContainer;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleDataChanged, this, RoleDataChanged);
		RegisterMessage(EUIMessageID.UIBackpack_SellItemsSuccess, this, SellItemsSuccess);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this, RoleDataChanged);
		RegisterMessage(EUIMessageID.UI_CloseSellItemsScene, this, CloseSellItems);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, COMBoxYesClick);
		base.Load();
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleDataChanged, this);
		UnregisterMessage(EUIMessageID.UIBackpack_SellItemsSuccess, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this);
		UnregisterMessage(EUIMessageID.UI_CloseSellItemsScene, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		base.UnLoad();
	}

	private bool RoleDataChanged(TUITelegram msg)
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)msg._pExtraInfo;
		if (notifyRoleDataCmd != null)
		{
			RefreshSellContainer();
		}
		return true;
	}

	private bool SellItemsSuccess(TUITelegram msg)
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		List<BagItem> bag_list = bag_data.m_bag_list;
		for (int i = 0; i < bag_list.Count; i++)
		{
			if (_uiSellItemsContainer.IsInSellList(bag_list[i].m_unique_id))
			{
				bag_list[i].m_state = 0;
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.BagData);
		return true;
	}

	private bool CloseSellItemsScene(object obj)
	{
		Debug.Log("CloseSellItemsScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		UIDataBufferCenter.Instance._bSellListChanged = true;
		TLoadScene extraInfo = new TLoadScene("UI.Backpack", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private bool CloseSellItems(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, CloseSellItemsScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool COMBoxYesClick(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "SellItemSuccess":
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_CloseSellItemsScene, null, null);
			break;
		}
		return true;
	}

	private new void Awake()
	{
	}

	private void Start()
	{
	}

	protected override void Tick()
	{
		if (_NeedGetNewData)
		{
			RefreshSellContainer();
			_NeedGetNewData = false;
		}
	}

	private void RefreshSellContainer()
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		if (notifyRoleDataCmd != null)
		{
			UIGolbalStaticFun.ParseBackpackDataToUI(notifyRoleDataCmd.m_bag_data, _uiSellItemsContainer, NGUI_COMUI.UI_Container.EBoxSelType.Multi, true, UIBackpack.EItemType.All, true);
		}
	}
}
