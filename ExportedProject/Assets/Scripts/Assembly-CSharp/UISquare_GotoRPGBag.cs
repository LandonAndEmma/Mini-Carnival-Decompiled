using MessageID;
using UnityEngine;

public class UISquare_GotoRPGBag : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoRpgBagClick, null, null);
	}
}
