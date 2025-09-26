using MessageID;
using UnityEngine;

public class UIRPG_ButtonCardLibraryClick : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICardMgr_GotoCardLibraryClick, null, null);
	}
}
