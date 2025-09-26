using MessageID;
using UnityEngine;

public class UIRPGAvatarStrengthen_GotoSquare : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIAvatarEnhance_GotoSquare, null, null);
	}
}
