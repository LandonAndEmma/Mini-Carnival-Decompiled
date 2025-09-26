using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_SearchFriendsContainer : NGUI_COMUI.UI_Container
{
	public GameObject showPannel;

	protected override void Load()
	{
		base.Load();
	}

	protected override void UnLoad()
	{
		base.UnLoad();
	}

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
			if (box.BoxData != null)
			{
				if (box != _curSelBox)
				{
					loseSel = _curSelBox;
					return true;
				}
				loseSel = _curSelBox;
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
		base.ProcessBoxSelected(box);
		UIRankings_SearchFriendBoxData uIRankings_SearchFriendBoxData = (UIRankings_SearchFriendBoxData)box.BoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Other, uIRankings_SearchFriendBoxData.watchInfo);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Hide_Other);
	}
}
