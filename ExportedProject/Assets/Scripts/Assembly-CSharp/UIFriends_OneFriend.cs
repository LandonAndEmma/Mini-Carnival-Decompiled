using UnityEngine;

public class UIFriends_OneFriend : UIMessageHandler
{
	private UIFriends_OneFriendData _data;

	[SerializeField]
	private GameObject _addBtn;

	[SerializeField]
	private GameObject _selFriendBtn;

	[SerializeField]
	private GameObject _selFrame;

	[SerializeField]
	private TUILabel _friendName;

	[SerializeField]
	private TUIMeshSprite _friendIcon;

	[SerializeField]
	private GameObject _refFriendInfoBtn;

	public UIFriends_OneFriendData FriendData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			_data.DataControl = this;
			DataChanged();
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void NotityGetFocus(bool bFocus)
	{
		_selFrame.SetActive(bFocus);
	}

	public void DataChanged()
	{
		if (FriendData == null)
		{
			return;
		}
		if (FriendData.IsAddBtn)
		{
			_addBtn.SetActive(true);
			_selFriendBtn.SetActive(false);
			_friendName.gameObject.SetActive(false);
			_friendIcon.gameObject.SetActive(false);
			_refFriendInfoBtn.SetActive(false);
			return;
		}
		_addBtn.SetActive(false);
		_selFriendBtn.SetActive(true);
		_friendName.gameObject.SetActive(true);
		_friendIcon.gameObject.SetActive(true);
		_refFriendInfoBtn.SetActive(true);
		_friendName.Text = FriendData.FriendName;
		if (FriendData.FriendTex2D != null)
		{
			_friendIcon.UseCustomize = true;
			_friendIcon.CustomizeTexture = FriendData.FriendTex2D;
			_friendIcon.CustomizeRect = new Rect(0f, 0f, FriendData.FriendTex2D.width, FriendData.FriendTex2D.height);
		}
	}

	public void HandleEventButton_refresh(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIFriends component = base.transform.root.GetComponent<UIFriends>();
			if (component != null)
			{
				Debug.Log("HandleEventButton_refresh-CommandClick");
				component.ProcessRefreshFriend(control.transform.parent.parent.GetComponent<UIFriends_OneFriend>());
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton_AddFriend(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButton_refresh-CommandClick");
			UIFriends component = base.transform.root.GetComponent<UIFriends>();
			if (component != null)
			{
				component.ProcessSelFriend(control.transform.parent.GetComponent<UIFriends_OneFriend>());
				component.ProcessAddFriend();
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton_SelFriend(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButton_refresh-CommandClick");
			UIFriends component = base.transform.root.GetComponent<UIFriends>();
			if (component != null)
			{
				component.ProcessSelFriend(control.transform.parent.GetComponent<UIFriends_OneFriend>());
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}
}
