using UnityEngine;

public class UIMessage_JoinGameBox : UIMessage_Box
{
	[SerializeField]
	private UILabel _labelDes;

	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UIJoinGameMessageBoxData uIJoinGameMessageBoxData = base.BoxData as UIJoinGameMessageBoxData;
		if (uIJoinGameMessageBoxData != null)
		{
			string text = TUITextManager.Instance().GetString(UI_GlobalData.Instance._strModeID[uIJoinGameMessageBoxData.SceneID]);
			string text2 = TUITool.StringFormat(Localization.instance.Get("newroom_desc2"), "[99FF00]" + uIJoinGameMessageBoxData.HostName + "[-]", "[99FF00]" + text + "[-]");
			_labelDes.text = text2;
		}
	}

	private void Start()
	{
		Object.Destroy(base.gameObject, 30f);
	}
}
