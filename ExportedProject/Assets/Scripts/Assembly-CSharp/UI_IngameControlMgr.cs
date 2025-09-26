using UnityEngine;

public class UI_IngameControlMgr : UIMessageHandler
{
	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_Pause(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Pause-CommandClick");
			break;
		case 1:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			UI_ButtonEffect component2 = control.gameObject.GetComponent<UI_ButtonEffect>();
			if (component2 != null)
			{
				component2.SetAlpha(1f);
			}
			break;
		}
		case 2:
		{
			BtnCloseLight(control);
			BtnRestoreScale(control);
			UI_ButtonEffect component = control.gameObject.GetComponent<UI_ButtonEffect>();
			if (component != null)
			{
				component.SetAlpha(0.5f);
			}
			break;
		}
		}
	}

	public void HandleEventButton_Jump(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Jump-CommandClick");
			break;
		case 1:
		{
			BtnOpenLight(control);
			BtnScale(control);
			UI_ButtonEffect component2 = control.gameObject.GetComponent<UI_ButtonEffect>();
			if (component2 != null)
			{
				component2.SetAlpha(1f);
			}
			break;
		}
		case 2:
		{
			BtnCloseLight(control);
			BtnRestoreScale(control);
			UI_ButtonEffect component = control.gameObject.GetComponent<UI_ButtonEffect>();
			if (component != null)
			{
				component.SetAlpha(0.5f);
			}
			break;
		}
		}
	}

	public void HandleEventButton_Share(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Share-CommandClick");
			COMA_NetworkConnect.Instance.BackFromScene();
			COMA_CommonOperation.Instance.bNeedFBFeed = true;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnScale(control);
			break;
		case 2:
			BtnRestoreScale(control);
			break;
		}
	}
}
