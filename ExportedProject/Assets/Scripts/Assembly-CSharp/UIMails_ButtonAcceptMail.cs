using MessageID;
using UnityEngine;

public class UIMails_ButtonAcceptMail : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMail, null, null);
	}
}
