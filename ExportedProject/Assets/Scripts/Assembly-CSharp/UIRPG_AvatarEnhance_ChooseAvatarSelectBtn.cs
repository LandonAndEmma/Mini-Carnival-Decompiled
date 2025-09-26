using MessageID;
using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseAvatarSelectBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_AvatarEnhance_SelectAvatarContainer _avatarContainer;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		base.gameObject.SetActive(false);
		if (_avatarContainer.CurSelBox != null)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGAvatar_SelAvatarClick, null, _avatarContainer.CurSelBox.BoxData);
		}
	}
}
