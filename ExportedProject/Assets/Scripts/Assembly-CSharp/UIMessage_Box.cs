using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIMessage_Box : NGUI_COMUI.UI_Box
{
	protected void OnDestroy()
	{
		Debug.Log("Destroy MessageBox!");
		UIMessage_BoxData uIMessage_BoxData = base.BoxData as UIMessage_BoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_MessageBoxDestory, null, uIMessage_BoxData.Channel);
	}
}
