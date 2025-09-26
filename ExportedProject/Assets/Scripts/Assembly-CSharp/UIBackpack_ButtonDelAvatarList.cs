using MessageID;
using UnityEngine;

public class UIBackpack_ButtonDelAvatarList : MonoBehaviour
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
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_DelAvatarList, null, null);
	}
}
