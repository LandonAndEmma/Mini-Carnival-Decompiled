using System.Collections.Generic;
using UnityEngine;

public class UI_RecordMgr_RPG : MonoBehaviour
{
	[SerializeField]
	private GameObject RECObj;

	private bool bRecording;

	private float flashTimer;

	private void Start()
	{
		if (!(COMA_VideoController.Instance != null) || COMA_VideoController.Instance.nVideo == 0)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
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

	private void OnDisable()
	{
		if (COMA_VideoController.Instance != null && bRecording)
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

	private void OnClick()
	{
		Debug.Log("Button_Record-CommandClick");
		if (!(COMA_VideoController.Instance == null))
		{
			if (!bRecording)
			{
				COMA_VideoController.Instance.StartRecording();
				bRecording = true;
				return;
			}
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
}
