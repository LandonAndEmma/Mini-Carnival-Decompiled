using MessageID;
using UnityEngine;

public class UIBackpack_ButtonPlusPrice : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIBackpack_SellItemPlusPrice, null, null);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Figure);
	}
}
