using MessageID;
using UnityEngine;

public class UIMarket_ButtonPraiseAvatar : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Applaud);
		base.gameObject.SetActive(false);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_BtnPraiseAvatarClick, null, null);
	}
}
