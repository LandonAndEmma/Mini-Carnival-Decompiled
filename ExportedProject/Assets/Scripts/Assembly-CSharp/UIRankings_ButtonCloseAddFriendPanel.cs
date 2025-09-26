using MessageID;
using UnityEngine;

public class UIRankings_ButtonCloseAddFriendPanel : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CloseAddFriendPanel, null, null);
	}
}
