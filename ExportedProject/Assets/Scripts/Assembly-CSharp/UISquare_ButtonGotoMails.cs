using MessageID;
using UnityEngine;

public class UISquare_ButtonGotoMails : MonoBehaviour
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
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoMails, null, null);
	}
}
