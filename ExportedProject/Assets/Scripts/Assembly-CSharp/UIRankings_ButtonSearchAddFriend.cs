using MessageID;
using UnityEngine;

public class UIRankings_ButtonSearchAddFriend : MonoBehaviour
{
	public UIRankings_SearchFriendBox boxCom;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		uint playerID = boxCom.PlayerID;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_SearchFriendAdd, null, playerID, boxCom);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
	}
}
