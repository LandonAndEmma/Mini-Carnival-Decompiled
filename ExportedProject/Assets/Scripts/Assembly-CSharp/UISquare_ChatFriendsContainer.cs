using MessageID;
using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UISquare_ChatFriendsContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UIChatInput _uiChatInput;

	[SerializeField]
	private GameObject _objOwner;

	[SerializeField]
	private UILabel _labelNoOnlineFriends;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UI_OnlineFriendsChanged, this, OnOnlineFriendsChanged);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UI_OnlineFriendsChanged, this);
	}

	private bool OnOnlineFriendsChanged(TUITelegram msg)
	{
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxSelected(box);
		if (base.BoxSelType == EBoxSelType.Single)
		{
			UISquare_ChatFriendBoxData uISquare_ChatFriendBoxData = box.BoxData as UISquare_ChatFriendBoxData;
			string friendName = uISquare_ChatFriendBoxData.FriendName;
			uint friendID = uISquare_ChatFriendBoxData.FriendID;
			Debug.Log("Sel:" + friendName + "  ID:" + friendID);
			if (_uiChatInput != null)
			{
				_uiChatInput.text = "@" + friendName + " ";
				_uiChatInput.extraInfo = friendID;
			}
			ClearContainer();
			if (_objOwner != null)
			{
				_objOwner.SetActive(false);
			}
			else
			{
				base.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (base.BoxSelType == EBoxSelType.Multi)
		{
			UISquare_ChatFriendBoxData uISquare_ChatFriendBoxData2 = box.BoxData as UISquare_ChatFriendBoxData;
			COMA_WaitingRoom_SceneController.Instance.friendListToInvite.Add(uISquare_ChatFriendBoxData2.FriendID);
		}
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		if (base.BoxSelType != EBoxSelType.Single && base.BoxSelType == EBoxSelType.Multi && box != null && box.BoxData != null)
		{
			UISquare_ChatFriendBoxData uISquare_ChatFriendBoxData = box.BoxData as UISquare_ChatFriendBoxData;
			COMA_WaitingRoom_SceneController.Instance.friendListToInvite.Remove(uISquare_ChatFriendBoxData.FriendID);
		}
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
		if (base.BoxSelType == EBoxSelType.Multi)
		{
			if (box.BoxData != null)
			{
				if (IsExistInPreList(box))
				{
					loseSel = box;
					return false;
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

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}

	public void InitFriendsContainer()
	{
		if (_objOwner != null)
		{
			_objOwner.SetActive(true);
		}
		else
		{
			base.gameObject.transform.parent.gameObject.SetActive(true);
		}
		int count = UIDataBufferCenter.Instance.Online_friend_list.Count;
		if (_labelNoOnlineFriends != null)
		{
			_labelNoOnlineFriends.enabled = ((count <= 0) ? true : false);
		}
		InitContainer();
		InitBoxs(count, true);
		for (int i = 0; i < count; i++)
		{
			UISquare_ChatFriendBoxData data = new UISquare_ChatFriendBoxData(string.Empty, 0u);
			UIDataBufferCenter.Instance.FetchPlayerProfile(UIDataBufferCenter.Instance.Online_friend_list[i], delegate(WatchRoleInfo playerInfo)
			{
				Debug.Log("player ID : " + playerInfo.m_player_id);
				data.FriendID = playerInfo.m_player_id;
				data.FriendName = playerInfo.m_name;
				UIDataBufferCenter.Instance.FetchFacebookIconByTID(playerInfo.m_player_id, delegate(Texture2D tex)
				{
					data.FriendTexture = tex;
					data.SetDirty();
				});
				data.SetDirty();
			});
			SetBoxData(i, data);
		}
		if (count > 6)
		{
			SetMoveForce(new Vector3(0f, 1f, 0f));
		}
		else
		{
			SetMoveForce(Vector3.zero);
		}
	}
}
