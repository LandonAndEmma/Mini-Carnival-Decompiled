using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_FriendContainer : NGUI_COMUI.UI_Container
{
	public UIRankings_ButtonDelFriend btn_delFriend;

	public UIRankings_ButtonDonate btn_donate;

	public GameObject showPannel;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIRankings_FirstClickNG, this, FirstClickNG);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIRankings_FirstClickNG, this);
	}

	private bool FirstClickNG(TUITelegram msg)
	{
		UIWidget[] componentsInChildren = btn_donate.transform.GetComponentsInChildren<UIWidget>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].depth -= 600;
		}
		btn_donate.gameObject.SetActive(false);
		return true;
	}

	private void Awake()
	{
		btn_delFriend.gameObject.SetActive(false);
		btn_donate.gameObject.SetActive(false);
		showPannel.SetActive(false);
	}

	private void Start()
	{
		if (COMA_Pref.Instance.NG2_FirstEnterFriends)
		{
			btn_donate.gameObject.SetActive(true);
			UIWidget[] componentsInChildren = btn_donate.transform.GetComponentsInChildren<UIWidget>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].depth += 600;
			}
		}
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
		btn_donate.gameObject.SetActive(true);
		btn_donate.tarID = uIRankings_FriendBoxData.PlayerID;
		if (uIRankings_FriendBoxData.IsRPG)
		{
			showPannel.SetActive(false);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Hide_Other);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, null, (ulong)uIRankings_FriendBoxData.PlayerID);
		}
		else
		{
			showPannel.SetActive(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Other, uIRankings_FriendBoxData.watchInfo);
		}
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		btn_delFriend.gameObject.SetActive(false);
		if (!COMA_Pref.Instance.NG2_FirstEnterFriends)
		{
			btn_donate.gameObject.SetActive(false);
		}
		showPannel.SetActive(false);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Hide_Other);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_HidePlayerTeamInfo, this, null);
	}

	private void AddChatRecord(UISquare_ChatRecordBoxData data)
	{
	}
}
