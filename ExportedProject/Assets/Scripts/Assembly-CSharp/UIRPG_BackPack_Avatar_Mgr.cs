using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_BackPack_Avatar_Mgr : UIEntity
{
	[SerializeField]
	private UIRPG_BackPack_Avatar_Container _avatarMgrContainer;

	[SerializeField]
	private UILabel _desLabel;

	[SerializeField]
	private GameObject _desObj;

	[SerializeField]
	private GameObject _delBtnObj;

	[SerializeField]
	private GameObject _unLoadBtnObj;

	[SerializeField]
	private UILabel _unLoadLabel;

	public UIRPG_BackPack_Avatar_Container AvatarMgrContainer
	{
		get
		{
			return _avatarMgrContainer;
		}
	}

	public UILabel DesLabel
	{
		get
		{
			return _desLabel;
		}
	}

	public GameObject DesObj
	{
		get
		{
			return _desObj;
		}
	}

	public GameObject DelBtnObj
	{
		get
		{
			return _delBtnObj;
		}
	}

	public GameObject UnLoadBtnObj
	{
		get
		{
			return _unLoadBtnObj;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		_unLoadLabel.text = RPGGlobalData.Instance.RpgMiscUnit._cancelReinforcePrice_FirstTime.ToString();
	}

	protected override void Tick()
	{
	}

	protected override void Load()
	{
		InitContainer();
		RegisterMessage(EUIMessageID.UIRPGAvatar_UnlockBtnClick, this, UnLockBtnClick);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIRPG_AvatarCapacityChange, this, AvatarCapacityChanged);
		RegisterMessage(EUIMessageID.UIRPG_AvatarCapacityChangeError, this, HandleAvatarCapacityChangeError);
		RegisterMessage(EUIMessageID.UIRPGAvatar_AvatarInfoBtnClick, this, InfoBtnClick);
		RegisterMessage(EUIMessageID.UIRPG_Ani_AvatarBagBackToSquare, this, BackToSquare);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPGAvatar_UnlockBtnClick, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_AvatarCapacityChange, this);
		UnregisterMessage(EUIMessageID.UIRPG_AvatarCapacityChangeError, this);
		UnregisterMessage(EUIMessageID.UIRPGAvatar_AvatarInfoBtnClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_Ani_AvatarBagBackToSquare, this);
	}

	private IEnumerator MultiFrameAddContainerBox()
	{
		NotifyRPGDataCmd rpgData = UIDataBufferCenter.Instance.RPGData;
		Dictionary<ulong, Equip> avatarEquitBag = rpgData.m_equip_bag;
		int i = 0;
		int maxFrameBox = 20;
		int curi = i;
		foreach (ulong key in avatarEquitBag.Keys)
		{
			Equip avatarUnit = UIDataBufferCenter.Instance.RPGData.m_equip_bag[key];
			UIRPG_BackPack_Avatar_BoxData data = new UIRPG_BackPack_Avatar_BoxData(key, avatarUnit.m_md5)
			{
				DataType = 3,
				EquipAvatar = avatarUnit,
				IsHasEquip = IsEquip(key)
			};
			_avatarMgrContainer.SetBoxData(_avatarMgrContainer.AddBox(i), data);
			Debug.LogWarning((EAvatarPart)avatarUnit.m_part);
			UIGolbalStaticFun.GetAvatarPartTex((EAvatarPart)avatarUnit.m_part, avatarUnit.m_md5, data);
			i++;
			if (i - curi >= maxFrameBox)
			{
				curi = i;
				yield return 0;
			}
		}
		yield return 0;
		curi = i;
		while (i < RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag)
		{
			UIRPG_BackPack_Avatar_BoxData data2 = new UIRPG_BackPack_Avatar_BoxData
			{
				DataType = ((i >= UIDataBufferCenter.Instance.RPGData.m_equip_capacity) ? 1 : 0)
			};
			if (data2.DataType == 1 && i - 1 >= 0 && i - 1 < _avatarMgrContainer.LstBoxs.Count && (_avatarMgrContainer.LstBoxs[i - 1].BoxData.DataType == 1 || _avatarMgrContainer.LstBoxs[i - 1].BoxData.DataType == 2))
			{
				data2.DataType = 2;
			}
			_avatarMgrContainer.SetBoxData(_avatarMgrContainer.AddBox(i), data2);
			i++;
			if (i - curi >= maxFrameBox)
			{
				curi = i;
				yield return 0;
			}
		}
	}

	private void InitContainer()
	{
		_avatarMgrContainer.ClearContainer();
		_avatarMgrContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		StartCoroutine(MultiFrameAddContainerBox());
	}

	private bool IsEquip(ulong key)
	{
		bool result = false;
		MemberSlot[] member_slot = UIDataBufferCenter.Instance.RPGData.m_member_slot;
		for (int i = 0; i < member_slot.Length; i++)
		{
			if (member_slot[i].m_head == key || member_slot[i].m_body == key || member_slot[i].m_leg == key)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool UnLockBtnClick(TUITelegram msg)
	{
		string des = TUITool.StringFormat(Localization.instance.Get("beibaojiemian_desc7"), 1, RPGGlobalData.Instance.RpgMiscUnit._unitRPGAvatarBagPrice);
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
		uIMessage_CommonBoxData.Mark = "UnlockBagCell";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "UnlockBagCell":
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			BuyRpgBagCapacityCmd buyRpgBagCapacityCmd = new BuyRpgBagCapacityCmd();
			buyRpgBagCapacityCmd.m_bag_type = 1;
			buyRpgBagCapacityCmd.m_buy_num = 1;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, buyRpgBagCapacityCmd);
			COMA_HTTP_DataCollect.Instance.SendUnlockRPGBackpackByGemCount("1");
			break;
		}
		case "LackOfMoney":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}

	private bool AvatarCapacityChanged(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		uint num = (uint)msg._pExtraInfo;
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = _avatarMgrContainer.LstBoxs[(int)(num - 1)].BoxData as UIRPG_BackPack_Avatar_BoxData;
		uIRPG_BackPack_Avatar_BoxData.DataType = 0;
		uIRPG_BackPack_Avatar_BoxData.SetDirty();
		if (num < _avatarMgrContainer.LstBoxs.Count && _avatarMgrContainer.LstBoxs[(int)num].BoxData.DataType == 2)
		{
			_avatarMgrContainer.LstBoxs[(int)num].BoxData.DataType = 1;
			_avatarMgrContainer.LstBoxs[(int)num].BoxData.SetDirty();
		}
		return true;
	}

	public bool HandleAvatarCapacityChangeError(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
		uIMessage_CommonBoxData.Mark = "LackOfMoney";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	public bool InfoBtnClick(TUITelegram msg)
	{
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

	public bool BackToSquare(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}
}
