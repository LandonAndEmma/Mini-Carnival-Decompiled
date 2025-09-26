using MessageID;
using UnityEngine;

public class UIRPGBag_GotoSquare : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Ani_AvatarBagBackToSquare, null, null);
	}
}
