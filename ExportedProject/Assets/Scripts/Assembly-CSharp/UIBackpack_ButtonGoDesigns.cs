using MessageID;
using UIGlobal;
using UnityEngine;

public class UIBackpack_ButtonGoDesigns : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		TLoadScene extraInfo = new TLoadScene("UI.Designs", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}
}
