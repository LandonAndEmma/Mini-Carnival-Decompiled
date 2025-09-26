using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Binary;
using Protocol.Role.S2C;
using Protocol.Shop.S2C;
using UnityEngine;

public class UILobby_ShopProtocolProcessor : UILobbyMessageHandler
{
	[SerializeField]
	private Renderer debug_testObj;

	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(2, this);
			OnMessage(1, OnUploadDataResult);
			OnMessage(3, OnDownloadDataResult);
			OnMessage(26, OnPlayerShopListResult);
			OnMessage(5, OnSellItemsResult);
			OnMessage(6, OnSellListAddNewItem);
			OnMessage(30, OnMarketShopListResult);
			OnMessage(20, OnMarketBuyAvatarResult);
			OnMessage(32, OnMarketBuySystemShopResult);
			OnMessage(22, OnPraiseAvatarResult);
			OnMessage(12, OnCollectAvatarResult);
			OnMessage(14, OnUncollectAvatarResult);
			OnMessage(16, OnFollowPlayerResult);
			OnMessage(18, OnUnfollowPlayerResult);
			OnMessage(28, OnCollectListResult);
			OnMessage(10, OnDelAvatarResult);
			OnMessage(8, OnResellAvatarResult);
			OnMessage(24, OnADAvatarResult);
		}
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(2, this);
		}
	}

	private bool OnUploadDataResult(UnPacker unpacker)
	{
		SetFileDataResultCmd setFileDataResultCmd = new SetFileDataResultCmd();
		if (!setFileDataResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (setFileDataResultCmd.m_result == 0)
		{
			Debug.Log("upload png MD5=" + setFileDataResultCmd.m_md5);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_UploadFileArrived, this, setFileDataResultCmd);
		}
		else if (setFileDataResultCmd.m_result == 1)
		{
			Debug.LogError("png is too big!=" + setFileDataResultCmd.m_md5);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_UploadFileArrived, this, null);
		}
		else
		{
			Debug.Log(setFileDataResultCmd.m_result);
			Debug.LogError("png write error!=" + setFileDataResultCmd.m_md5);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_UploadFileArrived, this, null);
		}
		return true;
	}

	private bool OnDownloadDataResult(UnPacker unpacker)
	{
		GetFileDataResultCmd getFileDataResultCmd = new GetFileDataResultCmd();
		if (!getFileDataResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		byte[] data = null;
		if (getFileDataResultCmd.m_result == 0)
		{
			data = getFileDataResultCmd.m_data;
			UITexCacherMgr.Instance.InsertTexToCache(getFileDataResultCmd.m_md5, data);
		}
		else
		{
			Debug.Log("------png is miss!-----------  :" + getFileDataResultCmd.m_result);
			Debug.Log("png is miss!");
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemoteFileDataArrived, this, new RemoteFileData(getFileDataResultCmd.m_md5, data));
		return true;
	}

	private bool OnPlayerShopListResult(UnPacker unpacker)
	{
		GetRoleShopListResultCmd getRoleShopListResultCmd = new GetRoleShopListResultCmd();
		if (!getRoleShopListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemotePlayerSellListArrived, this, new RemotePlayerSellListData(getRoleShopListResultCmd.m_who, getRoleShopListResultCmd.m_list));
		return true;
	}

	private bool OnSellItemsResult(UnPacker unpacker)
	{
		SellAvatarResultCmd sellAvatarResultCmd = new SellAvatarResultCmd();
		if (!sellAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (sellAvatarResultCmd.m_result == 0)
		{
			Debug.Log("Sell Items:OK!");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_SellItemsSuccess, this, null);
			string des = TUITool.StringFormat(Localization.instance.Get("jiaoyijiemian_desc23"), COMA_DataConfig.Instance._sysConfig.Shop.item_num, COMA_Sys.Instance.tax);
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
			uIMessage_CommonBoxData.Mark = "SellItemSuccess";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		else if (sellAvatarResultCmd.m_result == 1)
		{
			string des2 = TUITool.StringFormat(Localization.instance.Get("jiaoyijiemian_desc22"), COMA_DataConfig.Instance._sysConfig.Shop.max_size);
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, des2);
			UIGolbalStaticFun.PopCommonMessageBox(data);
			Debug.LogWarning("Sell Items : out max count!");
		}
		else if (sellAvatarResultCmd.m_result == 2)
		{
			UIGolbalStaticFun.PopMsgBox_LackMoney();
			Debug.LogWarning("Sell Items : Not enought crystal!");
		}
		else
		{
			Debug.LogError(" Error!!! Sell Items!");
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		return true;
	}

	private bool OnSellListAddNewItem(UnPacker unpacker)
	{
		NotifyRoleShopAddCmd notifyRoleShopAddCmd = new NotifyRoleShopAddCmd();
		if (!notifyRoleShopAddCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		ShopItem item = notifyRoleShopAddCmd.m_item;
		Debug.Log("Shop List:New Item Add!");
		return true;
	}

	private bool OnMarketShopListResult(UnPacker unpacker)
	{
		GetShopListResultCmd getShopListResultCmd = new GetShopListResultCmd();
		if (!getShopListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnMarketShopListResult :Type = " + getShopListResultCmd.m_type + "List Count=" + getShopListResultCmd.m_list.Count);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemoteMarketShopListArrived, this, new RemoteMarketShopListData(getShopListResultCmd.m_type, getShopListResultCmd.m_list));
		return true;
	}

	private bool OnCollectListResult(UnPacker unpacker)
	{
		GetRoleCollectListResultCmd getRoleCollectListResultCmd = new GetRoleCollectListResultCmd();
		if (!getRoleCollectListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnCollectListResult :List Count=" + getRoleCollectListResultCmd.m_list.Count);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_CollectMarketShopListArrived, this, getRoleCollectListResultCmd.m_list);
		return true;
	}

	private bool OnDelAvatarResult(UnPacker unpacker)
	{
		DelAvatarResultCmd delAvatarResultCmd = new DelAvatarResultCmd();
		if (!delAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, null);
		return true;
	}

	private bool OnResellAvatarResult(UnPacker unpacker)
	{
		ResellAvatarResultCmd resellAvatarResultCmd = new ResellAvatarResultCmd();
		if (!resellAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnResellAvatarResult:" + (ResellAvatarResultCmd.Code)resellAvatarResultCmd.m_result);
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (resellAvatarResultCmd.m_result == 0)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("jiaoyijiemian_desc23"), COMA_DataConfig.Instance._sysConfig.Shop.item_num, COMA_Sys.Instance.tax);
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, des);
			UIGolbalStaticFun.PopCommonMessageBox(data);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, null);
		}
		else if (resellAvatarResultCmd.m_result == 1)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, null);
		}
		else if (resellAvatarResultCmd.m_result == 2)
		{
			UIGolbalStaticFun.PopMsgBox_LackMoney();
		}
		else if (resellAvatarResultCmd.m_result != 3)
		{
		}
		return true;
	}

	private bool OnADAvatarResult(UnPacker unpacker)
	{
		AdAvatarResultCmd adAvatarResultCmd = new AdAvatarResultCmd();
		if (!adAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnADAvatarResult:" + (AdAvatarResultCmd.Code)adAvatarResultCmd.m_result);
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (adAvatarResultCmd.m_result == 0)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, null);
		}
		else if (adAvatarResultCmd.m_result == 1)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_RefreshAvatarList, this, null);
		}
		else if (adAvatarResultCmd.m_result == 2)
		{
			UIGolbalStaticFun.PopMsgBox_LackMoney();
		}
		else if (adAvatarResultCmd.m_result != 3)
		{
		}
		return true;
	}

	private bool OnMarketBuyAvatarResult(UnPacker unpacker)
	{
		BuyAvatarResultCmd buyAvatarResultCmd = new BuyAvatarResultCmd();
		if (!buyAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("Buy ID=" + buyAvatarResultCmd.m_id + " Result=" + (BuyAvatarResultCmd.Code)buyAvatarResultCmd.m_result);
		if (buyAvatarResultCmd.m_result == 0)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PurchaseShopItemsSuccess, this, buyAvatarResultCmd.m_id, 0);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PurchaseShopItemsFailure, this, (BuyAvatarResultCmd.Code)buyAvatarResultCmd.m_result, 0);
		}
		return true;
	}

	private bool OnMarketBuySystemShopResult(UnPacker unpacker)
	{
		BuySysShopResultCmd buySysShopResultCmd = new BuySysShopResultCmd();
		if (!buySysShopResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("Buy ID=" + buySysShopResultCmd.m_unit + " Result=" + (BuySysShopResultCmd.Code)buySysShopResultCmd.m_result);
		if (buySysShopResultCmd.m_result == 0)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role) as NotifyRoleDataCmd;
			if (buySysShopResultCmd.m_unit == "Head01")
			{
				notifyRoleDataCmd.m_bag_data.m_first_buy_head = 0;
			}
			else if (buySysShopResultCmd.m_unit == "Body01")
			{
				notifyRoleDataCmd.m_bag_data.m_first_buy_body = 0;
			}
			else if (buySysShopResultCmd.m_unit == "Leg01")
			{
				notifyRoleDataCmd.m_bag_data.m_first_buy_leg = 0;
			}
			else if (buySysShopResultCmd.m_unit == "HBL01")
			{
				notifyRoleDataCmd.m_bag_data.m_first_buy_head = 0;
				notifyRoleDataCmd.m_bag_data.m_first_buy_body = 0;
				notifyRoleDataCmd.m_bag_data.m_first_buy_leg = 0;
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PurchaseShopItemsSuccess, this, buySysShopResultCmd.m_unit, 1);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PurchaseShopItemsFailure, this, (BuySysShopResultCmd.Code)buySysShopResultCmd.m_result, 1);
		}
		return true;
	}

	private bool OnPraiseAvatarResult(UnPacker unpacker)
	{
		PraiseAvatarResultCmd praiseAvatarResultCmd = new PraiseAvatarResultCmd();
		if (!praiseAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PraiseAvatarResult, this, praiseAvatarResultCmd.m_id);
		return true;
	}

	private bool OnCollectAvatarResult(UnPacker unpacker)
	{
		CollectAvatarResultCmd collectAvatarResultCmd = new CollectAvatarResultCmd();
		if (!collectAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnCollectAvatarResult=" + (CollectAvatarResultCmd.Code)collectAvatarResultCmd.m_result);
		if (collectAvatarResultCmd.m_result == 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("shangdianjiemian_desc23"));
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			notifyRoleDataCmd.m_collect_list.Add(collectAvatarResultCmd.m_id);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.CollectLst);
		}
		else if (collectAvatarResultCmd.m_result == 1)
		{
			string str = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc20"), COMA_DataConfig.Instance._sysConfig.Collect.max_size);
			UIGolbalStaticFun.PopupTipsBox(str);
		}
		return true;
	}

	private bool OnUncollectAvatarResult(UnPacker unpacker)
	{
		UncollectAvatarResultCmd uncollectAvatarResultCmd = new UncollectAvatarResultCmd();
		if (!uncollectAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnUncollectAvatarResult=" + (UncollectAvatarResultCmd.Code)uncollectAvatarResultCmd.m_result);
		if (uncollectAvatarResultCmd.m_result == 0)
		{
			if (uncollectAvatarResultCmd.m_param == 1)
			{
				UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("shangdianjiemian_desc24"));
			}
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			notifyRoleDataCmd.m_collect_list.Remove(uncollectAvatarResultCmd.m_id);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.CollectLst);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_UncollectAvatarSuccess, this, null);
		}
		return true;
	}

	private bool OnFollowPlayerResult(UnPacker unpacker)
	{
		FollowRoleShopResultCmd followRoleShopResultCmd = new FollowRoleShopResultCmd();
		if (!followRoleShopResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnFollowPlayerResult=" + (FollowRoleShopResultCmd.Code)followRoleShopResultCmd.m_result);
		if (followRoleShopResultCmd.m_result == 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("shangdianjiemian_desc22"));
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			notifyRoleDataCmd.m_follow_list.Add(followRoleShopResultCmd.m_follow_id);
			UIDataBufferCenter.Instance.FollowSuccess(followRoleShopResultCmd.m_follow_id);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.FollowLst);
		}
		else if (followRoleShopResultCmd.m_result == 1)
		{
			string str = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc19"), COMA_DataConfig.Instance._sysConfig.Follow.max_size);
			UIGolbalStaticFun.PopupTipsBox(str);
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		return true;
	}

	private bool OnUnfollowPlayerResult(UnPacker unpacker)
	{
		UnfollowRoleShopResultCmd unfollowRoleShopResultCmd = new UnfollowRoleShopResultCmd();
		if (!unfollowRoleShopResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnUnfollowPlayerResult=" + (UnfollowRoleShopResultCmd.Code)unfollowRoleShopResultCmd.m_result);
		if (unfollowRoleShopResultCmd.m_result == 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("shangdianjiemian_desc25"));
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			notifyRoleDataCmd.m_follow_list.Remove(unfollowRoleShopResultCmd.m_follow_id);
			UIDataBufferCenter.Instance.UnFollowSuccess(unfollowRoleShopResultCmd.m_follow_id);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_UnfollowSuccess, this, null);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.FollowLst);
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
