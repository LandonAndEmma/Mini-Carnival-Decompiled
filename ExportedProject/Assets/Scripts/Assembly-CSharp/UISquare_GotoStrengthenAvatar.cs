using MessageID;
using UnityEngine;

public class UISquare_GotoStrengthenAvatar : MonoBehaviour
{
	[SerializeField]
	private GameObject _owner;

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_owner.SetActive(false);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoStrengthenAvatarClick, null, null);
	}
}
