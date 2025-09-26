using MessageID;
using UnityEngine;

public class UIMails_ButtonCloseMails : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_CloseMails, null, null);
	}
}
