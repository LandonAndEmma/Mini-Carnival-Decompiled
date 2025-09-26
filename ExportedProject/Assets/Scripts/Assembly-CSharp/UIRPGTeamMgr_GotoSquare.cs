using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIRPGTeamMgr_GotoSquare : MonoBehaviour
{
	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (UIDataBufferCenter.Instance.IsExistTeamMem())
		{
			if (COMA_Pref.Instance.NG2_1_FirstInTeam && UIDataBufferCenter.Instance.GetTeamMem() < 3)
			{
				UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("NoviceProcess_64b"));
				uIMessage_CommonBoxData.Mark = "FirstInTeamLess3";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			}
			else
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, null, null);
			}
		}
		else
		{
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("NoviceProcess_64a"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
		}
	}
}
