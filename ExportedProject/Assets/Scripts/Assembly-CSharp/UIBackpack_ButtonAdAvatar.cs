using MessageID;
using UnityEngine;

public class UIBackpack_ButtonAdAvatar : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		Debug.Log("UIBackpack_ButtonAdAvatar");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_AdAvatar, null, null);
	}
}
