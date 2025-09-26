using MessageID;
using UnityEngine;

public class UIRPGCGem_GotoSquare : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIGemsComposition_GotoSquare, null, null);
	}
}
