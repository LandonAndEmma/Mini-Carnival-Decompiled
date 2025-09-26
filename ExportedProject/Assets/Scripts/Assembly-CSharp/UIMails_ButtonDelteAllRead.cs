using MessageID;
using UnityEngine;

public class UIMails_ButtonDelteAllRead : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_DelAllReadedMails, null, null);
	}
}
