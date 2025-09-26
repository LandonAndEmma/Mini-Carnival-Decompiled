using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_MyTeamSelEquipBtnMgr : UIEntity
{
	public enum UIRPG_MyTeamGemCompostion
	{
		RRR = 111,
		YYY = 222,
		BBB = 333,
		PPP = 444,
		RRY = 112,
		RRB = 113
	}

	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	[SerializeField]
	private UILabel _careerNameLabel;

	[SerializeField]
	private UISprite[] _careerGradeSprite;

	[SerializeField]
	private UILabel _carerrSkillDescLabel;

	[SerializeField]
	private UILabel _totalLabel;

	[SerializeField]
	private UILabel[] _displayLabels;

	private int _curPos;

	[SerializeField]
	private List<UIRPG_MyTeamSelEquipBtnBox> _btnList = new List<UIRPG_MyTeamSelEquipBtnBox>();

	public Dictionary<ushort, byte> _dict = new Dictionary<ushort, byte>();

	public int CurPos
	{
		get
		{
			return _curPos;
		}
		set
		{
			_curPos = value;
		}
	}

	public List<UIRPG_MyTeamSelEquipBtnBox> BtnList
	{
		get
		{
			return _btnList;
		}
	}

	public void Awake()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPGTeam_SelNewAvatarClick, this, HandleSelNewAvatarClick);
		RegisterMessage(EUIMessageID.UIRPG_NotifyMountEquipResult, this, HandleMountEquipResult);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPGTeam_SelNewAvatarClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMountEquipResult, this);
	}

	public void InitContainer()
	{
		for (int i = 0; i < _btnList.Count; i++)
		{
			RefreshSingleBtnData(_btnList[i], _btnList[i].CurPart);
		}
	}

	public void RefreshSingleBtnData(UIRPG_MyTeamSelEquipBtnBox btn, BagItem.Part part)
	{
		if (_myTeamMgr.MyTeamContainer.CurSelBox == null)
		{
			return;
		}
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = _myTeamMgr.MyTeamContainer.CurSelBox.BoxData as UIRPG_MyTeamBoxData;
		Debug.Log("-----------------:" + uIRPG_MyTeamBoxData.CurPos);
		UIRPG_MyTeamSelEquipBtnBoxData uIRPG_MyTeamSelEquipBtnBoxData = btn.BoxData as UIRPG_MyTeamSelEquipBtnBoxData;
		if (uIRPG_MyTeamSelEquipBtnBoxData == null)
		{
			btn.BoxData = new UIRPG_MyTeamSelEquipBtnBoxData();
		}
		uIRPG_MyTeamSelEquipBtnBoxData = btn.BoxData as UIRPG_MyTeamSelEquipBtnBoxData;
		Debug.Log("-----------------:RefreshSingleBtnData");
		NotifyRPGDataCmd rPGData = UIDataBufferCenter.Instance.RPGData;
		MemberSlot[] member_slot = UIDataBufferCenter.Instance.RPGData.m_member_slot;
		switch (part)
		{
		case BagItem.Part.head:
			uIRPG_MyTeamSelEquipBtnBoxData.EquipData = ((!rPGData.m_equip_bag.ContainsKey(member_slot[uIRPG_MyTeamBoxData.CurPos].m_head)) ? null : rPGData.m_equip_bag[member_slot[uIRPG_MyTeamBoxData.CurPos].m_head]);
			break;
		case BagItem.Part.body:
			uIRPG_MyTeamSelEquipBtnBoxData.EquipData = ((!rPGData.m_equip_bag.ContainsKey(member_slot[uIRPG_MyTeamBoxData.CurPos].m_body)) ? null : rPGData.m_equip_bag[member_slot[uIRPG_MyTeamBoxData.CurPos].m_body]);
			break;
		case BagItem.Part.leg:
			uIRPG_MyTeamSelEquipBtnBoxData.EquipData = ((!rPGData.m_equip_bag.ContainsKey(member_slot[uIRPG_MyTeamBoxData.CurPos].m_leg)) ? null : rPGData.m_equip_bag[member_slot[uIRPG_MyTeamBoxData.CurPos].m_leg]);
			break;
		}
		if (uIRPG_MyTeamSelEquipBtnBoxData.EquipData != null)
		{
			UIGolbalStaticFun.GetAvatarPartTex((EAvatarPart)uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_part, uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_md5, uIRPG_MyTeamSelEquipBtnBoxData);
			if (uIRPG_MyTeamSelEquipBtnBoxData.Tex == null)
			{
				Debug.Log("btn.BtnData.Tex == null");
			}
		}
		else
		{
			uIRPG_MyTeamSelEquipBtnBoxData.SetDirty();
		}
	}

	public bool HandleSelNewAvatarClick(TUITelegram msg)
	{
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = _myTeamMgr.MyTeamContainer.CurSelBox.BoxData as UIRPG_MyTeamBoxData;
		UIRPG_MyTeam_SelEquipTakeOrPutData uIRPG_MyTeam_SelEquipTakeOrPutData = msg._pExtraInfo as UIRPG_MyTeam_SelEquipTakeOrPutData;
		MountMemberEquipCmd mountMemberEquipCmd = new MountMemberEquipCmd();
		mountMemberEquipCmd.m_member_pos = (byte)uIRPG_MyTeamBoxData.CurPos;
		Debug.Log("_____________________________________Curos " + uIRPG_MyTeamBoxData.CurPos);
		Debug.Log("_____________________________________m_part " + mountMemberEquipCmd.m_part);
		Debug.Log("____________________________________m_id " + uIRPG_MyTeam_SelEquipTakeOrPutData.EquipData.m_id);
		mountMemberEquipCmd.m_part = uIRPG_MyTeam_SelEquipTakeOrPutData.EquipData.m_part;
		if (uIRPG_MyTeam_SelEquipTakeOrPutData.IsPutOn)
		{
			_myTeamMgr.PopUpSelEquipObj.SetActive(false);
			mountMemberEquipCmd.m_equip = uIRPG_MyTeam_SelEquipTakeOrPutData.EquipData.m_id;
		}
		else
		{
			mountMemberEquipCmd.m_equip = 0uL;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, mountMemberEquipCmd);
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		return true;
	}

	public bool HandleMountEquipResult(TUITelegram msg)
	{
		MountMemberEquipResultCmd mountMemberEquipResultCmd = msg._pExtraInfo as MountMemberEquipResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (mountMemberEquipResultCmd.m_result == 0)
		{
			Debug.Log("HandleMountEquipResultOk");
			RefreshSingleBtnData(_btnList[_curPos], (BagItem.Part)mountMemberEquipResultCmd.m_part);
			DisplayPropertyInfo();
		}
		return true;
	}

	public void RefreshPropertyValue(MountMemberEquipResultCmd cc_cmd)
	{
		bool flag = false;
		MemberSlot[] member_slot = UIDataBufferCenter.Instance.RPGData.m_member_slot;
		if (member_slot[cc_cmd.m_member_pos].m_member == 0)
		{
			return;
		}
		if (cc_cmd.m_mount_equip != 0L && UIDataBufferCenter.Instance.RPGData.m_equip_bag.ContainsKey(cc_cmd.m_mount_equip))
		{
			Equip equip = UIDataBufferCenter.Instance.RPGData.m_equip_bag[cc_cmd.m_mount_equip];
			if (_dict.ContainsKey(equip.m_type))
			{
				int num = _dict[equip.m_type];
				_myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[num] += UIRPG_DataBufferCenter.GetGemCompoundValue(equip.m_type, equip.m_level);
				flag = true;
			}
		}
		else if (cc_cmd.m_unmount_equip != 0L && UIDataBufferCenter.Instance.RPGData.m_equip_bag.ContainsKey(cc_cmd.m_unmount_equip))
		{
			Equip equip2 = UIDataBufferCenter.Instance.RPGData.m_equip_bag[cc_cmd.m_unmount_equip];
			if (_dict.ContainsKey(equip2.m_type))
			{
				int num2 = _dict[equip2.m_type];
				_myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[num2] -= UIRPG_DataBufferCenter.GetGemCompoundValue(equip2.m_type, equip2.m_level);
				flag = true;
			}
		}
		if (flag)
		{
			int member = (int)member_slot[cc_cmd.m_member_pos].m_member;
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[member];
			int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
			float num3 = rPGCareerUnit.AttrValue[0] + rPGCareerUnit.AttrValue[1] + rPGCareerUnit.AttrValue[2];
			float num4 = _myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[4];
			float num5 = _myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[3];
			float num6 = _myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[5];
			_myTeamMgr.CardPropertyData[cc_cmd.m_member_pos].BaseAttrs[10] = num3 * (float)rpg_level + num4 + num5 / num6;
		}
	}

	public void DisplayCardInfo()
	{
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = _myTeamMgr.MyTeamContainer.CurSelBox.BoxData as UIRPG_MyTeamBoxData;
		int curPos = uIRPG_MyTeamBoxData.CurPos;
		if (uIRPG_MyTeamBoxData.CardId != 0)
		{
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)uIRPG_MyTeamBoxData.CardId];
			string text = TUITool.StringFormat(Localization.instance.Get(rPGCareerUnit.CareerName));
			_careerNameLabel.text = text;
			for (int i = 0; i < _careerGradeSprite.Length; i++)
			{
				_careerGradeSprite[i].gameObject.SetActive((i < rPGCareerUnit.StarGrade) ? true : false);
			}
			for (int j = 0; j < _displayLabels.Length; j++)
			{
				_displayLabels[j].text = Mathf.Ceil(_myTeamMgr.CardPropertyData[curPos].BaseAttrs[j + 3]) + ((j > 2 && j < 7) ? "%" : string.Empty);
			}
		}
		else
		{
			_careerNameLabel.text = string.Empty;
			for (int k = 0; k < _careerGradeSprite.Length; k++)
			{
				_careerGradeSprite[k].gameObject.SetActive(false);
			}
			for (int l = 0; l < _displayLabels.Length; l++)
			{
				_displayLabels[l].text = string.Empty;
			}
			_carerrSkillDescLabel.text = string.Empty;
		}
		float num = 0f;
		for (int m = 0; m < _myTeamMgr.MyTeamContainer.LstBoxs.Count; m++)
		{
			num += (float)((_myTeamMgr.MyTeamContainer.LstBoxs[m].BoxData.DataType == 1) ? Mathf.CeilToInt(_myTeamMgr.CardPropertyData[m].BaseAttrs[10]) : 0);
		}
		_totalLabel.text = ((num == 0f) ? string.Empty : num.ToString());
	}

	public void InitDict()
	{
		if (_dict.Count <= 0)
		{
			Debug.Log("==================================================================InitDict");
			_dict.Add(111, 9);
			_dict.Add(222, 5);
			_dict.Add(333, 7);
			_dict.Add(444, 6);
			_dict.Add(112, 8);
			_dict.Add(113, 3);
		}
	}

	public void DisplayCareerSkillDesc(uint cardId)
	{
		_carerrSkillDescLabel.text = UIRPG_DataBufferCenter.GetCardCareerDesByCardId((int)cardId);
	}

	public void DisplayPropertyInfo()
	{
		UIRPG_MyTeamBox uIRPG_MyTeamBox = _myTeamMgr.MyTeamContainer.CurSelBox as UIRPG_MyTeamBox;
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = uIRPG_MyTeamBox.BoxData as UIRPG_MyTeamBoxData;
		if (uIRPG_MyTeamBoxData.DataType == 1)
		{
			_carerrSkillDescLabel.text = UIRPG_DataBufferCenter.GetCardCareerDesByCardId((int)uIRPG_MyTeamBoxData.CardId);
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)uIRPG_MyTeamBoxData.CardId];
			string text = TUITool.StringFormat(Localization.instance.Get(rPGCareerUnit.CareerName));
			_careerNameLabel.text = text;
			for (int i = 0; i < _careerGradeSprite.Length; i++)
			{
				_careerGradeSprite[i].gameObject.SetActive((i < rPGCareerUnit.StarGrade) ? true : false);
			}
			int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
			int[] singleMemberAttr = UIRPG_DataBufferCenter.GetSingleMemberAttr(UIDataBufferCenter.Instance.RPGData.m_member_slot[uIRPG_MyTeamBox._index], rpg_level, false, 0u);
			_displayLabels[0].text = singleMemberAttr[3].ToString();
			_displayLabels[1].text = singleMemberAttr[4].ToString();
			_displayLabels[2].text = singleMemberAttr[5].ToString();
			_displayLabels[3].text = singleMemberAttr[6] + "%";
			_displayLabels[4].text = singleMemberAttr[7] + "%";
			_displayLabels[5].text = singleMemberAttr[8].ToString();
			_displayLabels[6].text = singleMemberAttr[9] + "%";
			_displayLabels[7].text = singleMemberAttr[10].ToString();
		}
		else if (uIRPG_MyTeamBoxData.DataType == 0)
		{
			_careerNameLabel.text = string.Empty;
			for (int j = 0; j < _careerGradeSprite.Length; j++)
			{
				_careerGradeSprite[j].gameObject.SetActive(false);
			}
			for (int k = 0; k < _displayLabels.Length; k++)
			{
				_displayLabels[k].text = string.Empty;
			}
			_carerrSkillDescLabel.text = string.Empty;
		}
		int teamTotalAp = UIRPG_DataBufferCenter.GetTeamTotalAp(UIDataBufferCenter.Instance.RPGData.m_member_slot, (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level, false, 0u);
		_totalLabel.text = ((teamTotalAp == 0) ? string.Empty : teamTotalAp.ToString());
	}
}
