using UnityEngine;

public class UIFriends_FriendRequest : UIMessageHandler
{
	private UIFriends_FriendRequestData _data;

	[SerializeField]
	private TUILabel _frNameLabel;

	public UIFriends_FriendRequestData FRData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			DataChanged();
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void DataChanged()
	{
		if (FRData != null)
		{
			_frNameLabel.Text = FRData.nickname;
		}
	}

	public void HandleEventButton_Accept(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIFriends component = base.transform.root.GetComponent<UIFriends>();
			if (component != null)
			{
				UIFriends_FriendRequestData fRData = control.transform.parent.GetComponent<UIFriends_FriendRequest>().FRData;
				component.ProcessAcceptFriend(fRData);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Reject(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIFriends component = base.transform.root.GetComponent<UIFriends>();
			if (component != null)
			{
				UIFriends_FriendRequestData fRData = control.transform.parent.GetComponent<UIFriends_FriendRequest>().FRData;
				component.ProcessRejectFriend(fRData);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
