using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_WorldContainer : NGUI_COMUI.UI_Container
{
	public UIRankings_ButtonAddFriend btn_addFriend;

	public GameObject showPannel;

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
			UIRankings_WorldBoxData uIRankings_WorldBoxData = (UIRankings_WorldBoxData)_curSelBox.BoxData;
			Debug.Log("Try to add friend : " + uIRankings_WorldBoxData.PlayerName + "  ID : " + uIRankings_WorldBoxData.PlayerID);
			uint playerID = uIRankings_WorldBoxData.PlayerID;
			UIDataBufferCenter.Instance.TryToAddFriend(playerID);
		}
		return true;
	}

	private void Awake()
	{
		btn_addFriend.gameObject.SetActive(false);
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
		UIRankings_WorldBoxData uIRankings_WorldBoxData = (UIRankings_WorldBoxData)box.BoxData;
		Debug.Log(uIRankings_WorldBoxData.PlayerID);
		if (UIGolbalStaticFun.GetSelfTID() != uIRankings_WorldBoxData.PlayerID && !UIDataBufferCenter.Instance.IsMyFriend(uIRankings_WorldBoxData.PlayerID))
		{
			btn_addFriend.gameObject.SetActive(true);
			btn_addFriend.tarID = uIRankings_WorldBoxData.PlayerID;
		}
		showPannel.SetActive(true);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Other, uIRankings_WorldBoxData.watchInfo);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		btn_addFriend.gameObject.SetActive(false);
		showPannel.SetActive(false);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Hide_Other);
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}

	private void AddChatRecord(UISquare_ChatRecordBoxData data)
	{
	}
}
