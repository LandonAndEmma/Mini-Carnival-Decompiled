using MessageID;
using UnityEngine;

public class UIRankings_ButtonSearchRefresh : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_SearchFriendRefresh, null, null);
	}
}
