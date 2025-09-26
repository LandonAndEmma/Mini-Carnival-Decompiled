using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_MyTeamMgr : UIEntity
{
	[SerializeField]
	private UIRPG_MyTeamContainer _myTeamContainer;

	[SerializeField]
	private UIRPG_MyTeamSelEquipBtnMgr _selEquipBtnMgr;

	[SerializeField]
	private GameObject _popUpSelCardObj;

	[SerializeField]
	private GameObject _popUpSelEquipObj;

	[SerializeField]
	private GameObject _selCardSelBtnObj;

	[SerializeField]
	private UILabel _selCardSelBtnLabel;

	[SerializeField]
	private GameObject _switchBtnObj;

	public Dictionary<ulong, int> _dict = new Dictionary<ulong, int>();

	private ulong _captainTag;

	private UIRPG_MyTeamDisplayData[] _cardPropertyData = new UIRPG_MyTeamDisplayData[6];

	public UIRPG_MyTeamContainer MyTeamContainer
	{
		get
		{
			return _myTeamContainer;
		}
	}

	public UIRPG_MyTeamSelEquipBtnMgr SelEquipBtnMgr
	{
		get
		{
			return _selEquipBtnMgr;
		}
	}

	public GameObject PopUpSelCardObj
	{
		get
		{
			return _popUpSelCardObj;
		}
	}

	public GameObject PopUpSelEquipObj
	{
		get
		{
			return _popUpSelEquipObj;
		}
	}

	public GameObject SelCardSelBtnObj
	{
		get
		{
			return _selCardSelBtnObj;
		}
	}

	public UILabel SelCardSelBtnLabel
	{
		get
		{
			Debug.Log("SelCardSelBtnLabel");
			return _selCardSelBtnLabel;
		}
	}

	public GameObject SwitchBtnObj
	{
		get
		{
			return _switchBtnObj;
		}
	}

	public ulong CaptainTag
	{
		get
		{
			return _captainTag;
		}
	}

	public UIRPG_MyTeamDisplayData[] CardPropertyData
	{
		get
		{
			return _cardPropertyData;
		}
	}

	private void Start()
	{
		InitContainer();
		_myTeamContainer.InitStartSelBox();
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPGTeam_SelNewCardClick, this, HandleSelNewCardClick);
		RegisterMessage(EUIMessageID.UIRPG_NotifyChangeMemberResult, this, HandleChangeMemberResult);
		RegisterMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, this, MyTeamBackToSquare);
		RegisterMessage(EUIMessageID.UI_ASAniEnterEnd, this, ASAniEnterEnd);
		RegisterMessage(EUIMessageID.UIRPG_NG_2_1_End, this, NG_2_1_End);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPGTeam_SelNewCardClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyChangeMemberResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, this);
		UnregisterMessage(EUIMessageID.UI_ASAniEnterEnd, this);
		UnregisterMessage(EUIMessageID.UIRPG_NG_2_1_End, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "FirstInTeamLess3":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, null, null);
			break;
		}
		return true;
	}

	public void InitContainer()
	{
		_myTeamContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		MemberSlot[] member_slot = UIDataBufferCenter.Instance.RPGData.m_member_slot;
		for (int i = 0; i < member_slot.Length; i++)
		{
			RefreshSingleBoxData(i, member_slot);
		}
	}

	private void RefreshSingleBoxData(int i, MemberSlot[] memberSlot)
	{
		uint rpg_level = UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		uint lvLimit_TeamPos = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos4;
		uint lvLimit_TeamPos2 = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos5;
		uint lvLimit_TeamPos3 = (uint)RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos6;
		int type = 0;
		if (memberSlot[i].m_member != 0)
		{
			type = 1;
			if (_dict.ContainsKey(memberSlot[i].m_unqiue))
			{
				_dict[memberSlot[i].m_unqiue] = i;
			}
			else
			{
				_dict.Add(memberSlot[i].m_unqiue, i);
			}
		}
		if (i == 3 && rpg_level < lvLimit_TeamPos)
		{
			type = 2;
		}
		else if (i == 4 && rpg_level < lvLimit_TeamPos2)
		{
			type = 2;
		}
		else if (i == 5 && rpg_level < lvLimit_TeamPos3)
		{
			type = 2;
		}
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = new UIRPG_MyTeamBoxData(type, i, memberSlot[i].m_member);
		uIRPG_MyTeamBoxData.SpriteName = "RPG_Small_" + uIRPG_MyTeamBoxData.CardId;
		if (i == UIDataBufferCenter.Instance.RPGData.m_leader_pos)
		{
			uIRPG_MyTeamBoxData.IsCaptain = true;
			_captainTag = memberSlot[i].m_unqiue;
		}
		else
		{
			uIRPG_MyTeamBoxData.IsCaptain = false;
		}
		_myTeamContainer.SetBoxData(i, uIRPG_MyTeamBoxData);
	}

	public bool HandleSelNewCardClick(TUITelegram msg)
	{
		UIRPG_MyTeam_SelCardSelBtnData uIRPG_MyTeam_SelCardSelBtnData = msg._pExtraInfo as UIRPG_MyTeam_SelCardSelBtnData;
		ChangeMemberCmd changeMemberCmd = new ChangeMemberCmd();
		changeMemberCmd.m_card_unique_id = uIRPG_MyTeam_SelCardSelBtnData.ItemId;
		if (uIRPG_MyTeam_SelCardSelBtnData.IsPutOn)
		{
			_popUpSelCardObj.SetActive(false);
			changeMemberCmd.m_card_id = uIRPG_MyTeam_SelCardSelBtnData.CardId;
			changeMemberCmd.m_pos = (byte)((UIRPG_MyTeamBoxData)_myTeamContainer.CurSelBox.BoxData).CurPos;
		}
		else
		{
			changeMemberCmd.m_card_id = 0u;
			changeMemberCmd.m_card_unique_id = 0uL;
			changeMemberCmd.m_pos = (byte)_dict[uIRPG_MyTeam_SelCardSelBtnData.ItemId];
			_dict.Remove(uIRPG_MyTeam_SelCardSelBtnData.ItemId);
			Debug.Log("HandleSelNewCardClick");
			_selCardSelBtnLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_anniu6"));
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, changeMemberCmd);
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		return true;
	}

	public bool HandleChangeMemberResult(TUITelegram msg)
	{
		ChangeMemberResultCmd changeMemberResultCmd = msg._pExtraInfo as ChangeMemberResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (changeMemberResultCmd.m_result == 0)
		{
			if (_dict.ContainsKey(changeMemberResultCmd.m_unmember_unique_id))
			{
				_dict.Remove(changeMemberResultCmd.m_unmember_unique_id);
			}
			RefreshSingleBoxData(changeMemberResultCmd.m_pos, UIDataBufferCenter.Instance.RPGData.m_member_slot);
			_selEquipBtnMgr.DisplayPropertyInfo();
			_switchBtnObj.SetActive(UIRPG_MyTeamContainer.IsDisplaySwitchBtn(_myTeamContainer.CurSelBox));
		}
		return true;
	}

	private bool GotoSquare(object obj)
	{
		Debug.Log("GotoSquare");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
		}
		return true;
	}

	private bool GotoMap(object obj)
	{
		Debug.Log("GotoMap");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.Map", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
		}
		return true;
	}

	public bool MyTeamBackToSquare(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		if (UIRPG_DataBufferCenter.isPreSceneMap)
		{
			UIRPG_DataBufferCenter.isPreSceneMap = false;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoMap);
		}
		else
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool ASAniEnterEnd(TUITelegram msg)
	{
		Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ End Ani__Enter !");
		return true;
	}

	private bool NG_2_1_End(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		return true;
	}
}
