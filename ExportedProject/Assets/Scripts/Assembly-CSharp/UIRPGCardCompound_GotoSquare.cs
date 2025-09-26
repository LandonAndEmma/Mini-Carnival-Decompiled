using MC_UIToolKit;
using MessageID;
using UIGlobal;
using UnityEngine;

public class UIRPGCardCompound_GotoSquare : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 5)
		{
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("NoviceProcess_62a"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
		}
		else
		{
			TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		}
	}
}
