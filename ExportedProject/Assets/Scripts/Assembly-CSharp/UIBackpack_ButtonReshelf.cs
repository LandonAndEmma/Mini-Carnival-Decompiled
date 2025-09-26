using MessageID;
using UnityEngine;

public class UIBackpack_ButtonReshelf : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		Debug.Log("UIBackpack_ButtonReshelf");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_Reshelf, null, null);
	}
}
