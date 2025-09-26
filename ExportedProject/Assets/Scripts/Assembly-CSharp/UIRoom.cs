using UnityEngine;

public class UIRoom : UIChatAble
{
	[SerializeField]
	private GameObject _cm;

	[SerializeField]
	private GameObject _btnCM;

	[SerializeField]
	private GameObject ngui_FriendSelect;

	[SerializeField]
	private GameObject blockObj;

	private bool bHideButtons;

	[SerializeField]
	private GameObject inviteBtnObj;

	[SerializeField]
	private GameObject startBtnObj;

	[SerializeField]
	private UI_TUIAdaptRPGMode _adaptRPGMode;

	[SerializeField]
	private UISquare_ChatFriendsContainer _chatFriendsContainer;

	public UIInputChatBox GetInputChatBox()
	{
		return _chatBox;
	}

	public void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	private void Start()
	{
		if (COMA_CommonOperation.Instance.IsMode_Run(COMA_NetworkConnect.sceneName))
		{
			_btnCM.SetActive(true);
		}
		else
		{
			_btnCM.SetActive(false);
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_EnterWaitingRoom);
		if (COMA_Pref.Instance.controlMode_Run == -1 && COMA_CommonOperation.Instance.IsMode_Run(COMA_NetworkConnect.sceneName))
		{
			_cm.SetActive(true);
		}
		else
		{
			_cm.SetActive(false);
		}
		if (COMA_CommonOperation.Instance.isCreateRoom && COMA_Network.Instance.IsRoomMaster(COMA_Network.Instance.TNetInstance.Myself.Id))
		{
			inviteBtnObj.SetActive(true);
			startBtnObj.SetActive(true);
		}
		else
		{
			inviteBtnObj.SetActive(false);
			startBtnObj.SetActive(false);
		}
	}

	private new void Update()
	{
		if (!bHideButtons && COMA_CommonOperation.Instance.isCreateRoom && COMA_Network.Instance.IsRoomMaster(COMA_Network.Instance.TNetInstance.Myself.Id))
		{
			if (COMA_Network.Instance.TNetInstance.CurRoom.UserCount <= 1)
			{
				startBtnObj.SetActive(false);
			}
			else if (COMA_Network.Instance.TNetInstance.CurRoom.UserCount <= 2 && COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName))
			{
				startBtnObj.SetActive(false);
			}
			else
			{
				startBtnObj.SetActive(true);
			}
		}
	}

	public void HandleEventButton_JoyStick(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		COMA_PlayerSelf.Instance.UI_SetMoveInput(wparam, lparam);
	}

	public void HandleEventButton_RotateCmr(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (!Screen.lockCursor)
		{
			COMA_PlayerSelf.Instance.UI_SetRotatePlayer(wparam, lparam);
		}
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			bHideButtons = true;
			_adaptRPGMode.Mask();
			COMA_NetworkConnect.Instance.BackFromScene();
			COMA_Pref.Instance.Save(true);
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_invite(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			blockObj.SetActive(true);
			ngui_FriendSelect.SetActive(true);
			_chatFriendsContainer.InitFriendsContainer();
			COMA_WaitingRoom_SceneController.Instance.friendListToInvite.Clear();
			Debug.Log("HandleEventButton_invite");
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_start(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			COMA_Network.Instance.StartGame();
			inviteBtnObj.SetActive(false);
			startBtnObj.SetActive(false);
			bHideButtons = true;
			Debug.Log("HandleEventButton_start");
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_chat(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_chatBox.SetFocus();
			Debug.Log("Button_chat-CommandClick");
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

	public void HandleEventButton_controllManner(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_cm.SetActive(true);
			Debug.Log("Button_controllManner-CommandClick");
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

	public void HandleEventButton_c1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_Pref.Instance.controlMode_Run = 0;
			_cm.GetComponent<UIRoom_ControllManner>().SetSelIndex(COMA_Pref.Instance.controlMode_Run);
			COMA_Pref.Instance.Save(true);
			SceneTimerInstance.Instance.Add(0.5f, CloseCM);
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

	public void HandleEventButton_c2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_Pref.Instance.controlMode_Run = 1;
			_cm.GetComponent<UIRoom_ControllManner>().SetSelIndex(COMA_Pref.Instance.controlMode_Run);
			COMA_Pref.Instance.Save(true);
			SceneTimerInstance.Instance.Add(0.5f, CloseCM);
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

	public void HandleEventButton_c3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_Pref.Instance.controlMode_Run = 2;
			_cm.GetComponent<UIRoom_ControllManner>().SetSelIndex(COMA_Pref.Instance.controlMode_Run);
			COMA_Pref.Instance.Save(true);
			SceneTimerInstance.Instance.Add(0.5f, CloseCM);
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

	public bool CloseCM()
	{
		_cm.SetActive(false);
		return false;
	}
}
