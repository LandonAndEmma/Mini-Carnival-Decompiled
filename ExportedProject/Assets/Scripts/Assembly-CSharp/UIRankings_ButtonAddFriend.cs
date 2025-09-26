using MessageID;
using UnityEngine;

public class UIRankings_ButtonAddFriend : MonoBehaviour
{
	public uint tarID;

	public UILabel _nameLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FriendAdd, null, tarID);
		base.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		if (_nameLabel != null)
		{
			_nameLabel.enabled = true;
		}
	}

	private void OnDisable()
	{
		if (_nameLabel != null)
		{
			_nameLabel.enabled = false;
		}
	}
}
