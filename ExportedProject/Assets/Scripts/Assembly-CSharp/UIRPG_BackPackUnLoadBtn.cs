using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Role.S2C;
using UnityEngine;

public class UIRPG_BackPackUnLoadBtn : UIEntity
{
	[SerializeField]
	private UIRPG_BackPack_Avatar_Mgr _avatarMgr;

	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private GameObject _ngArrow;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIRPG_DecomposeEquipResult, this, HandleDecomposeEquipResult);
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		RegisterMessage(EUIMessageID.UING_RPG_FirstClickAvatar, this, FirstClickAvatar);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_DecomposeEquipResult, this);
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
		UnregisterMessage(EUIMessageID.UING_RPG_FirstClickAvatar, this);
	}

	public void OnClick()
	{
		Debug.Log("----------------------------------------------------------------------UIRPG_BackPackUnLoadBtn");
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = _avatarMgr.AvatarMgrContainer.CurSelBox.BoxData as UIRPG_BackPack_Avatar_BoxData;
		if (!uIRPG_BackPack_Avatar_BoxData.IsHasEquip)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("zhuangbeibeibao_desc3"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "UnLoadEquip";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		else
		{
			string des2 = "is Equip!, Can't UnLoad";
			UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(1, des2);
			uIMessage_CommonBoxData2.Mark = "Can't UnLoad";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
		}
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "UnLoadEquip":
		{
			if (RPGGlobalData.Instance.RpgMiscUnit._cancelReinforcePrice_FirstTime > UIDataBufferCenter.Instance.playerInfo.m_gold)
			{
				string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(0, des);
				uIMessage_CommonBoxData2.Mark = "LackOfMoney";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
				return true;
			}
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			List<BagItem> bag_list = notifyRoleDataCmd.m_bag_data.m_bag_list;
			if (bag_list.Count == notifyRoleDataCmd.m_bag_data.m_bag_capacity)
			{
				string des2 = TUITool.StringFormat(Localization.instance.Get("rpgmap_desc3"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData3 = new UIMessage_CommonBoxData(1, des2);
				uIMessage_CommonBoxData3.Mark = "LackOfSpace";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData3);
				return true;
			}
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = _avatarMgr.AvatarMgrContainer.CurSelBox.BoxData as UIRPG_BackPack_Avatar_BoxData;
			DecomposeEquipCmd decomposeEquipCmd = new DecomposeEquipCmd();
			decomposeEquipCmd.m_equip_id = uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, decomposeEquipCmd);
			break;
		}
		case "LackOfMoney":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}

	private bool HandleDecomposeEquipResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		DecomposeEquipResultCmd decomposeEquipResultCmd = msg._pExtraInfo as DecomposeEquipResultCmd;
		if (decomposeEquipResultCmd.m_result == 0)
		{
			_avatarMgr.DelBtnObj.SetActive(false);
			_avatarMgr.DesObj.SetActive(false);
			_avatarMgr.UnLoadBtnObj.SetActive(false);
			int num = -1;
			for (int i = 0; i < _avatarMgr.AvatarMgrContainer.LstBoxs.Count; i++)
			{
				if (_avatarMgr.AvatarMgrContainer.LstBoxs[i] == _avatarMgr.AvatarMgrContainer.CurSelBox)
				{
					num = i;
					break;
				}
			}
			_avatarMgr.AvatarMgrContainer.CurSelBox.SetLoseSelected();
			Debug.Log("++++++++++++++++++++++++++++++++++fjakjf boxId = " + num);
			UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = new UIRPG_BackPack_Avatar_BoxData();
			uIRPG_BackPack_Avatar_BoxData.DataType = 0;
			_avatarMgr.AvatarMgrContainer.SetBoxData(num, uIRPG_BackPack_Avatar_BoxData);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_BackPack_PartDelOrDecompose, null, null);
			string str = TUITool.StringFormat(Localization.instance.Get("zhuangbeibeibao_desc9"));
			UIGolbalStaticFun.PopupTipsBox(str);
		}
		return true;
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 == 1)
		{
			_sprite_light.enabled = false;
			_ngArrow.SetActive(false);
		}
		return true;
	}

	private bool FirstClickAvatar(TUITelegram msg)
	{
		_sprite_light.enabled = true;
		_ngArrow.SetActive(true);
		return true;
	}
}
