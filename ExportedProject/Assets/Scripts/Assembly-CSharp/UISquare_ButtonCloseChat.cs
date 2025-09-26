using MessageID;
using UnityEngine;

public class UISquare_ButtonCloseChat : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_CloseChat, null, null);
	}
}
