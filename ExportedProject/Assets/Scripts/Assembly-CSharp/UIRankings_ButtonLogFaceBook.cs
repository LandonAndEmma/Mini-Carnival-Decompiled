using MessageID;
using UnityEngine;

public class UIRankings_ButtonLogFaceBook : MonoBehaviour
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
		UIFacebookFeedback.Instance.tipMark = UIFacebookFeedback.ConnectKind.Login;
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRankings_LogFacebook, null, null);
		base.gameObject.SetActive(false);
	}
}
