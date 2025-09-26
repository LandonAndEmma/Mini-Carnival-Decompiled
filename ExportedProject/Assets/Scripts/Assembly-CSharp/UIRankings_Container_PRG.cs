using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_Container_PRG : NGUI_COMUI.UI_Container
{
	public UIRankings_ButtonAddFriend btn_addFriend;

	private void Awake()
	{
		btn_addFriend.gameObject.SetActive(false);
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
		UIRankings_RPGBoxData uIRankings_RPGBoxData = (UIRankings_RPGBoxData)box.BoxData;
		Debug.Log(uIRankings_RPGBoxData.PlayerID);
		if (UIGolbalStaticFun.GetSelfTID() != uIRankings_RPGBoxData.PlayerID && !UIDataBufferCenter.Instance.IsMyFriend(uIRankings_RPGBoxData.PlayerID))
		{
			btn_addFriend.gameObject.SetActive(true);
			btn_addFriend.tarID = uIRankings_RPGBoxData.PlayerID;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, null, uIRankings_RPGBoxData.ItemId);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		btn_addFriend.gameObject.SetActive(false);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_HidePlayerTeamInfo, this, null);
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}

	private void AddChatRecord(UISquare_ChatRecordBoxData data)
	{
	}

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIRankings_FriendAdd, this, OnAddFriendOnWorldList);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIRankings_FriendAdd, this);
	}

	private bool OnAddFriendOnWorldList(TUITelegram msg)
	{
		if (_curSelBox != null && _curSelBox.BoxData != null)
		{
			UIRankings_RPGBoxData uIRankings_RPGBoxData = (UIRankings_RPGBoxData)_curSelBox.BoxData;
			Debug.Log("Try to add friend : " + uIRankings_RPGBoxData.PlayerName + "  ID : " + uIRankings_RPGBoxData.PlayerID);
			uint playerID = uIRankings_RPGBoxData.PlayerID;
			UIDataBufferCenter.Instance.TryToAddFriend(playerID);
		}
		return true;
	}
}
