using MessageID;
using UIGlobal;
using UnityEngine;

public class UIRankings_ButtonGotoMails : MonoBehaviour
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
		TLoadScene extraInfo = new TLoadScene("UI.Mails", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}
}
