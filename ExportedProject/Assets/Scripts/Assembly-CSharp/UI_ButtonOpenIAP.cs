using MessageID;
using UnityEngine;

public class UI_ButtonOpenIAP : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
	}
}
