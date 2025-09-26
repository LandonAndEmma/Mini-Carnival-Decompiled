using MessageID;
using UnityEngine;

public class UISquare_ButtonSelPrivateChatObject : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_SelPrivateChatObject, null, null);
	}
}
