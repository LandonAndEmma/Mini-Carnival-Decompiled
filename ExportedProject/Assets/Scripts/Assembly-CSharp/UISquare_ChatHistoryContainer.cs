using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UISquare_ChatHistoryContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private List<GameObject> _lstBtns;

	public UIRankings_ButtonAddFriend btn_addFriend;

	public GameObject showPannel;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this, ChatHistoryChanged);
		RegisterMessage(EUIMessageID.UISquare_RefreshChatHistoryControl, this, RefreshChatHistoryControl);
		RegisterMessage(EUIMessageID.UISquare_RealRefreshChatHistory, this, OnRealRefreshChatHistory);
		RegisterMessage(EUIMessageID.UIRankings_FriendAdd, this, OnAddFriendOnWorldList);
		for (int i = 0; i < _lstBtns.Count; i++)
		{
			_lstBtns[i].SetActive(false);
		}
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this);
		UnregisterMessage(EUIMessageID.UISquare_RefreshChatHistoryControl, this);
		UnregisterMessage(EUIMessageID.UISquare_RealRefreshChatHistory, this);
		UnregisterMessage(EUIMessageID.UIRankings_FriendAdd, this);
		btn_addFriend.gameObject.SetActive(false);
		showPannel.SetActive(false);
		for (int i = 0; i < _lstBtns.Count; i++)
		{
			_lstBtns[i].SetActive(true);
		}
	}

	private bool ChatHistoryChanged(TUITelegram msg)
	{
		List<UISquare_ChatRecordBoxData> list = (List<UISquare_ChatRecordBoxData>)msg._pExtraInfo;
		if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.All)
		{
			int count = list.Count;
			InitContainer(EBoxSelType.Single);
			InitBoxs(count, false);
			for (int i = 0; i < count; i++)
			{
				UISquare_ChatRecordBoxData data = list[i];
				SetBoxData(i, data);
				if (data.OtherPeopleIcon == null)
				{
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(data.OtherPeopleID, delegate(Texture2D tex)
					{
						data.OtherPeopleIcon = tex;
						data.SetDirty();
					});
				}
			}
		}
		else if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.PM)
		{
			ClearContainer();
			InitContainer(EBoxSelType.Single);
			for (int num = 0; num < list.Count; num++)
			{
				UISquare_ChatRecordBoxData data2 = list[num];
				if (data2.Channel != Protocol.Channel.person)
				{
					continue;
				}
				SetBoxData(AddBox(), data2);
				if (data2.OtherPeopleIcon == null)
				{
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(data2.OtherPeopleID, delegate(Texture2D tex)
					{
						data2.OtherPeopleIcon = tex;
						data2.SetDirty();
					});
				}
			}
		}
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_RefreshChatHistoryControl, null, null);
		return true;
	}

	private bool OnRealRefreshChatHistory(TUITelegram msg)
	{
		List<UISquare_ChatRecordBoxData> list = (List<UISquare_ChatRecordBoxData>)msg._pExtraInfo;
		if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.All)
		{
			int count = list.Count;
			InitContainer(EBoxSelType.Single);
			InitBoxs(count, false);
			for (int i = 0; i < count; i++)
			{
				UISquare_ChatRecordBoxData data = list[i];
				SetBoxData(i, data);
				if (data.OtherPeopleIcon == null)
				{
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(data.OtherPeopleID, delegate(Texture2D tex)
					{
						data.OtherPeopleIcon = tex;
						data.SetDirty();
					});
				}
			}
		}
		else if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.PM)
		{
			ClearContainer();
			InitContainer(EBoxSelType.Single);
			for (int num = 0; num < list.Count; num++)
			{
				UISquare_ChatRecordBoxData data2 = list[num];
				if (data2.Channel != Protocol.Channel.person)
				{
					continue;
				}
				SetBoxData(AddBox(), data2);
				if (data2.OtherPeopleIcon == null)
				{
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(data2.OtherPeopleID, delegate(Texture2D tex)
					{
						data2.OtherPeopleIcon = tex;
						data2.SetDirty();
					});
				}
			}
		}
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_RefreshChatHistoryControl, null, null);
		return true;
	}

	private bool RefreshChatHistoryControl(TUITelegram msg)
	{
		if (GetComponent<UIDraggablePanel>() != null)
		{
			GetComponent<UIDraggablePanel>().SetDragAmount(0f, 1f, false);
		}
		UIPanel component = GetComponent<UIPanel>();
		if (component != null)
		{
			Vector4 clipRange = component.clipRange;
			clipRange.x = 0f;
			component.clipRange = clipRange;
		}
		return true;
	}

	private bool OnAddFriendOnWorldList(TUITelegram msg)
	{
		if (_curSelBox != null && _curSelBox.BoxData != null)
		{
			UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = (UISquare_ChatRecordBoxData)_curSelBox.BoxData;
			Debug.Log("Try to add friend : " + uISquare_ChatRecordBoxData.OtherPeopleName + "  ID : " + uISquare_ChatRecordBoxData.OtherPeopleID);
			uint otherPeopleID = uISquare_ChatRecordBoxData.OtherPeopleID;
			UIDataBufferCenter.Instance.TryToAddFriend(otherPeopleID);
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
				loseSel = null;
				return false;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		if (box != null && box.BoxData != null && ((UISquare_ChatRecordBoxData)box.BoxData).SelfRecord)
		{
			return;
		}
		base.ProcessBoxSelected(box);
		if (box != null && box.BoxData != null)
		{
			UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = (UISquare_ChatRecordBoxData)box.BoxData;
			btn_addFriend._nameLabel.text = uISquare_ChatRecordBoxData.OtherPeopleName;
			if (UIGolbalStaticFun.GetSelfTID() != uISquare_ChatRecordBoxData.OtherPeopleID && !UIDataBufferCenter.Instance.IsMyFriend(uISquare_ChatRecordBoxData.OtherPeopleID))
			{
				btn_addFriend.gameObject.SetActive(true);
				btn_addFriend.tarID = uISquare_ChatRecordBoxData.OtherPeopleID;
			}
			else
			{
				btn_addFriend.gameObject.SetActive(false);
			}
			btn_addFriend._nameLabel.enabled = true;
			UIDataBufferCenter.Instance.FetchPlayerProfile(uISquare_ChatRecordBoxData.OtherPeopleID, delegate(WatchRoleInfo playerInfo)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, this, UI2DCharcMgr.EOperType.Show_Other, playerInfo);
			});
			showPannel.SetActive(true);
		}
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		if (!(box != null) || box.BoxData == null || !((UISquare_ChatRecordBoxData)box.BoxData).SelfRecord)
		{
			base.ProcessBoxLoseSelected(box);
		}
	}

	private void AddChatRecord(UISquare_ChatRecordBoxData data)
	{
	}
}
