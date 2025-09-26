using MessageID;
using UnityEngine;

public class UIRankings_ButtonReloginFaceBook : MonoBehaviour
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
		UIFacebookFeedback.Instance.tipMark = UIFacebookFeedback.ConnectKind.Refresh;
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRankings_LogFacebook, null, null);
		base.gameObject.SetActive(false);
	}
}
