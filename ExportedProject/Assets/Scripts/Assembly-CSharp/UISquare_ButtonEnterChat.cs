using MessageID;
using UnityEngine;

public class UISquare_ButtonEnterChat : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_EnterChat, null, null);
	}
}
