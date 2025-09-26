using MessageID;
using UnityEngine;

public class UISquare_ButtonCloseMobileChat : MonoBehaviour
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
		UIDataBufferCenter.Instance._bManualCloseMobileChat = true;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_CloseMobileChat, null, null);
	}
}
