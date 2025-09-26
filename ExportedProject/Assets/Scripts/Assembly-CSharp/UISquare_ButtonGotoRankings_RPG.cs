using MessageID;
using UnityEngine;

public class UISquare_ButtonGotoRankings_RPG : MonoBehaviour
{
	[SerializeField]
	private bool _gotoFriend;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIDataBufferCenter.Instance.EnableToFriendTab = _gotoFriend;
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoRankings_RPG, null, null);
	}
}
