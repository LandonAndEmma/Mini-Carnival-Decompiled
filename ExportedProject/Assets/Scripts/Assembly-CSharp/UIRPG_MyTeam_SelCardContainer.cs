using System.Collections.Generic;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_MyTeam_SelCardContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UIRPG_MyTeam_SelCardMgr _selCardMgr;

	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null)
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
		box.SetSelected();
		UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = box.BoxData as UIRPG_MyTeam_SelCardBoxData;
		Debug.Log("ProcessBoxSelected(NGUI_COMUI.UI_Box box)");
		Debug.Log("data.IsSel : " + uIRPG_MyTeam_SelCardBoxData.IsSel);
		_myTeamMgr.SelCardSelBtnObj.SetActive(true);
		if (!uIRPG_MyTeam_SelCardBoxData.IsSel)
		{
			Debug.Log(TUITool.StringFormat(Localization.instance.Get("myteam_anniu6")));
			_myTeamMgr.SelCardSelBtnLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_anniu6"));
			_selCardMgr.SelBtnStat = true;
		}
		else
		{
			Debug.Log(TUITool.StringFormat(Localization.instance.Get("myteam_anniu5")));
			_myTeamMgr.SelCardSelBtnLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_anniu5"));
			_selCardMgr.SelBtnStat = false;
		}
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		box.SetSelected();
	}

	public override void DataSort()
	{
		List<NGUI_COMUI.UI_BoxData> list = new List<NGUI_COMUI.UI_BoxData>();
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = base.LstBoxs[i].BoxData as UIRPG_MyTeam_SelCardBoxData;
			list.Add(base.LstBoxs[i].BoxData);
		}
		list.Sort();
		for (int j = 0; j < list.Count; j++)
		{
			base.LstBoxs[j].BoxData = list[j];
		}
	}
}
