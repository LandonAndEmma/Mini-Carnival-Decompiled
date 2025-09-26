using MessageID;
using UnityEngine;

public class UIBackpack_ButtonCloseSellItems : MonoBehaviour
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
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_CloseSellItemsScene, null, null);
	}
}
