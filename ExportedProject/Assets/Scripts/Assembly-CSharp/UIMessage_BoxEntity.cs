using MessageID;
using UnityEngine;

public class UIMessage_BoxEntity : UIEntity
{
	private UIMessage_BoxData _msgBoxData;

	public UIMessage_BoxData MsgBoxData
	{
		get
		{
			return _msgBoxData;
		}
		set
		{
			_msgBoxData = value;
		}
	}

	protected new void OnDestroy()
	{
		Debug.Log("Destroy MessageBox!");
		UIMessage_BoxData msgBoxData = MsgBoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_MessageBoxDestory, null, msgBoxData.Channel);
	}
}
