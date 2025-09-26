using System.Collections.Generic;
using UnityEngine;

public class UI_RecordMgr : UIMessageHandler
{
	[SerializeField]
	private GameObject RECObj;

	private bool bRecording;

	private float flashTimer;

	private void Start()
	{
		if (COMA_VideoController.Instance.nVideo == 0)
		{
			base.gameObject.SetActive(false);
		}
	}

	private new void Update()
	{
		if (bRecording)
		{
			flashTimer += Time.deltaTime;
			if (flashTimer > 0.5f)
			{
				flashTimer = 0f;
				RECObj.SetActive(!RECObj.activeSelf);
			}
		}
		else if (RECObj.activeSelf)
		{
			RECObj.SetActive(false);
		}
	}

	private new void OnDisable()
	{
		if (bRecording)
		{
			COMA_VideoController.Instance.StopRecording();
			bRecording = false;
			COMA_VideoController.Instance.bRecorded = true;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("ID", COMA_CommonOperation.Instance.GIDForShow(COMA_Server_ID.Instance.GID));
			dictionary.Add("Name", COMA_Pref.Instance.nickname);
			dictionary.Add("Level", COMA_Pref.Instance.lv);
			COMA_VideoController.Instance.SetMetadata(dictionary);
		}
	}

	public void HandleEventButton_Record(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Record-CommandClick");
			if (!bRecording)
			{
				COMA_VideoController.Instance.StartRecording();
				bRecording = true;
				break;
			}
			COMA_VideoController.Instance.StopRecording();
			bRecording = false;
			COMA_VideoController.Instance.bRecorded = true;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("ID", COMA_CommonOperation.Instance.GIDForShow(COMA_Server_ID.Instance.GID));
			dictionary.Add("Name", COMA_Pref.Instance.nickname);
			dictionary.Add("Level", COMA_Pref.Instance.lv);
			COMA_VideoController.Instance.SetMetadata(dictionary);
			break;
		}
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
}
