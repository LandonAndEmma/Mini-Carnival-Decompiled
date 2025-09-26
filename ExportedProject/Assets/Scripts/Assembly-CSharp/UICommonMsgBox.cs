using UnityEngine;

public class UICommonMsgBox : UI_MsgBox
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangeText(string strText)
	{
		if (singleLineLabel != null)
		{
			singleLineLabel.Text = strText;
		}
	}

	public void HandleEventButton_No(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_No-CommandClick");
			CallNoHandler();
			DestroyBox();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Yes(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Yes-CommandClick");
			CallYesHandler(param);
			DestroyBox();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
