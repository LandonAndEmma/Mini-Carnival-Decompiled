using MessageID;
using UnityEngine;

public class UIRankings_ButtonGotoSearch : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, null, UIRankings_ButtonCaptionType.ECaptionType.SearchInput);
	}
}
