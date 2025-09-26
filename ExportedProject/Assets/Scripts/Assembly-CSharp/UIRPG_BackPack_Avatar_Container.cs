using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_BackPack_Avatar_Container : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UIRPG_BackPack_Avatar_Mgr _avatarMgr;

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null && box.BoxData.DataType > 2)
			{
				if (box != _curSelBox)
				{
					loseSel = _curSelBox;
					return true;
				}
				loseSel = null;
				return true;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		box.SetSelected();
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = box.BoxData as UIRPG_BackPack_Avatar_BoxData;
		if (!uIRPG_BackPack_Avatar_BoxData.IsHasEquip)
		{
			_avatarMgr.DelBtnObj.SetActive(true);
			_avatarMgr.UnLoadBtnObj.SetActive(true);
		}
		else
		{
			_avatarMgr.DelBtnObj.SetActive(false);
			_avatarMgr.UnLoadBtnObj.SetActive(false);
		}
		_avatarMgr.DesObj.SetActive(true);
		_avatarMgr.DesLabel.text = UIRPG_DataBufferCenter.GetDesByGemTypeAndLevel(uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_type, uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_level);
		if (COMA_Pref.Instance.NG2_1_FirstRPGBag_Click)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RPG_FirstClickAvatar, null, null);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Backpack_PartPreview, null, uIRPG_BackPack_Avatar_BoxData);
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		if (box.BoxData.DataType == 2 || box.BoxData.DataType == 1)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGAvatar_UnlockBtnClick, null, null);
		}
	}

	public override void DataSort()
	{
		int num = 0;
		List<NGUI_COMUI.UI_BoxData> list = new List<NGUI_COMUI.UI_BoxData>();
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = base.LstBoxs[i].BoxData as UIRPG_BackPack_Avatar_BoxData;
			if (uIRPG_BackPack_Avatar_BoxData != null && uIRPG_BackPack_Avatar_BoxData.DataType == 3)
			{
				list.Add(base.LstBoxs[i].BoxData);
				num = i;
			}
		}
		list.Sort();
		int num2 = 0;
		for (int num3 = list.Count - 1; num3 >= 0; num3--)
		{
			base.LstBoxs[num2++].BoxData = list[num3];
		}
		if (num >= list.Count)
		{
			for (int j = list.Count; j <= num; j++)
			{
				UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData2 = new UIRPG_BackPack_Avatar_BoxData();
				uIRPG_BackPack_Avatar_BoxData2.DataType = 0;
				base.LstBoxs[j].BoxData = uIRPG_BackPack_Avatar_BoxData2;
			}
		}
	}
}
