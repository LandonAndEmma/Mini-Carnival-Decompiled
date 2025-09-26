using MessageID;
using UnityEngine;

public class UIMarket_ButtonCaptionClose : MonoBehaviour
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
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIMarket_CaptionClose, null, null);
	}
}
