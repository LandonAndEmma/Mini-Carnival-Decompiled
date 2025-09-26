using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_MyTeamContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null && box.BoxData.DataType != 2)
			{
				if (box != _curSelBox)
				{
					loseSel = _curSelBox;
					_curSelBox = box;
					return true;
				}
				loseSel = null;
				return false;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		_myTeamMgr.SwitchBtnObj.SetActive(IsDisplaySwitchBtn(box));
		Debug.Log("UING_RPG_BtnDown------------------------------------------0");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RPG_BtnDown, null, 1);
		box.SetSelected();
		if (box.BoxData.DataType == 0)
		{
			_myTeamMgr.SelCardSelBtnObj.SetActive(false);
			_myTeamMgr.PopUpSelCardObj.SetActive(true);
			uint num = 0u;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, this, num, ((UIRPG_MyTeamBox)box)._index);
		}
		else
		{
			uint num2 = 0u;
			num2 = UIDataBufferCenter.Instance.RPGData.m_member_slot[((UIRPG_MyTeamBox)box)._index].m_member;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, this, num2, ((UIRPG_MyTeamBox)box)._index);
		}
		_myTeamMgr.SelEquipBtnMgr.InitContainer();
		_myTeamMgr.SelEquipBtnMgr.DisplayPropertyInfo();
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RPG_BtnDown, null, 1);
		if (box.BoxData.DataType == 0)
		{
			box.SetSelected();
			_myTeamMgr.SelCardSelBtnObj.SetActive(false);
			_myTeamMgr.PopUpSelCardObj.SetActive(true);
			uint num = 0u;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, this, num, ((UIRPG_MyTeamBox)box)._index);
		}
	}

	protected void ProcessFirstBoxSeleced(NGUI_COMUI.UI_Box box)
	{
		box.SetSelected();
		_myTeamMgr.SwitchBtnObj.SetActive(IsDisplaySwitchBtn(box));
		_myTeamMgr.SelEquipBtnMgr.InitContainer();
		_myTeamMgr.SelEquipBtnMgr.DisplayPropertyInfo();
		uint num = 0u;
		num = UIDataBufferCenter.Instance.RPGData.m_member_slot[((UIRPG_MyTeamBox)box)._index].m_member;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, this, num, 0);
		Debug.Log("-----------------------------------000-------------UIMyTeam_PreviewChanged" + num);
	}

	public void InitStartSelBox()
	{
		int index = 0;
		for (int i = 0; i < UIDataBufferCenter.Instance.RPGData.m_member_slot.Length; i++)
		{
			if (UIDataBufferCenter.Instance.RPGData.m_member_slot[i].m_member != 0)
			{
				index = i;
				break;
			}
		}
		_curSelBox = base.LstBoxs[index];
		_preSelBox = base.LstBoxs[index];
		ProcessFirstBoxSeleced(_curSelBox);
	}

	public static bool IsDisplaySwitchBtn(NGUI_COMUI.UI_Box box)
	{
		return box.BoxData.DataType == 1;
	}
}
