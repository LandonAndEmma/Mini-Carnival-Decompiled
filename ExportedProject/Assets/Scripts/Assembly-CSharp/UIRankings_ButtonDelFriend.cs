using MessageID;
using UnityEngine;

public class UIRankings_ButtonDelFriend : MonoBehaviour
{
	public uint tarID;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FriendDel, null, this);
	}
}
