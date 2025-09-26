using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIBackpack_ButtonGoMarket : MonoBehaviour
{
	private void OnClick()
	{
		if (COMA_Sys.Instance.bLockMarket)
		{
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("shangdianjiemian_desc12"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
		}
		else
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_EnterMarket);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_GotoMarketClick, null, null);
		}
	}
}
