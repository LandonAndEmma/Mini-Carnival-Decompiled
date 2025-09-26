using MessageID;
using UIGlobal;
using UnityEngine;

public class UICredits : UIMessageHandler
{
	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_right(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_Credits.Instance.Right();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_left(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_Credits.Instance.Left();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Move(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			COMA_Credits.Instance.MoveBegin(wparam, lparam);
			break;
		case 2:
			COMA_Credits.Instance.Moveing(wparam, lparam);
			break;
		case 3:
			COMA_Credits.Instance.MoveEnd(wparam, lparam);
			break;
		}
	}
}
