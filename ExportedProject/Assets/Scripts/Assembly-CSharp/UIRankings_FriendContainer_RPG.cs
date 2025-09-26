using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_FriendContainer_RPG : NGUI_COMUI.UI_Container
{
	public UIRankings_ButtonDelFriend btn_delFriend;

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
		btn_delFriend.gameObject.SetActive(false);
		showPannel.SetActive(false);
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
		UIRankings_FriendBoxData uIRankings_FriendBoxData = (UIRankings_FriendBoxData)box.BoxData;
		Debug.Log(uIRankings_FriendBoxData.Unit);
		btn_delFriend.gameObject.SetActive(true);
		btn_delFriend.tarID = uIRankings_FriendBoxData.PlayerID;
		showPannel.SetActive(true);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Other, uIRankings_FriendBoxData.watchInfo);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		btn_delFriend.gameObject.SetActive(false);
		showPannel.SetActive(false);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Hide_Other);
	}

	private void AddChatRecord(UISquare_ChatRecordBoxData data)
	{
	}
}
