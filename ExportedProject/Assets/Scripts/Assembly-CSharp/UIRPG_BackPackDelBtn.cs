using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_BackPackDelBtn : UIEntity
{
	[SerializeField]
	private UIRPG_BackPack_Avatar_Mgr _avatarMgr;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIRPG_NotifyAvatarDelResult, this, HandleNotifyAvatarDelResult);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyAvatarDelResult, this);
	}

	public void OnClick()
	{
		Debug.Log("----------------------------------------------------------------------UIRPG_BackPackDelBtn");
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = _avatarMgr.AvatarMgrContainer.CurSelBox.BoxData as UIRPG_BackPack_Avatar_BoxData;
		if (!uIRPG_BackPack_Avatar_BoxData.IsHasEquip)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("zhuangbeibeibao_desc4"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "DelEquipment";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		else
		{
			string des2 = "is Equip!, Can't Del";
			UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(1, des2);
			uIMessage_CommonBoxData2.Mark = "Can't Del";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
		}
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "DelEquipment":
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = _avatarMgr.AvatarMgrContainer.CurSelBox.BoxData as UIRPG_BackPack_Avatar_BoxData;
			DeleteEquipCmd deleteEquipCmd = new DeleteEquipCmd();
			deleteEquipCmd.m_equip_id = uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, deleteEquipCmd);
			break;
		}
		}
		return true;
	}

	private bool HandleNotifyAvatarDelResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		NotifyAvatarDelCmd notifyAvatarDelCmd = msg._pExtraInfo as NotifyAvatarDelCmd;
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
		Debug.Log("------------------------------------------------------fjakjf boxId = " + num);
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = new UIRPG_BackPack_Avatar_BoxData();
		uIRPG_BackPack_Avatar_BoxData.DataType = 0;
		_avatarMgr.AvatarMgrContainer.SetBoxData(num, uIRPG_BackPack_Avatar_BoxData);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_BackPack_PartDelOrDecompose, null, null);
		return true;
	}
}
