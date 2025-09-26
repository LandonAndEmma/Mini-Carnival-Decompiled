using UnityEngine;

public class TUI_Deliver : MonoBehaviour, TUIHandler
{
	public GameObject pauseObj;

	private void Start()
	{
		TUI component = base.gameObject.GetComponent<TUI>();
		component.SetHandler(this);
	}

	private void Update()
	{
		if (Screen.lockCursor && base.transform.FindChild("TUIControls").gameObject.activeSelf)
		{
			base.transform.FindChild("TUIControls").gameObject.SetActive(false);
		}
		else if (!Screen.lockCursor && !base.transform.FindChild("TUIControls").gameObject.activeSelf)
		{
			base.transform.FindChild("TUIControls").gameObject.SetActive(true);
		}
		if (COMA_Pref.Instance.controlMode_Run == 0 && COMA_CommonOperation.Instance.IsMode_Run(Application.loadedLevelName) && !(COMA_PlayerSelf.Instance == null))
		{
			if (COMA_Sys.Instance.bCoverUIInput)
			{
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				COMA_PlayerSelf.Instance.UI_SetFire(2, 0f, 0f);
			}
			else if (Input.acceleration.x < -0.1f)
			{
				COMA_PlayerSelf.Instance.UI_SetMoveInput(-1f, 0f);
			}
			else if (Input.acceleration.x > 0.1f)
			{
				COMA_PlayerSelf.Instance.UI_SetMoveInput(1f, 0f);
			}
			else
			{
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
			}
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.invokeOnEvent)
		{
			TUITool.TUIControlEventInvoke(control, control, eventType, wparam, lparam, data);
		}
		if (COMA_PlayerSelf.Instance == null)
		{
			return;
		}
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
			COMA_PlayerSelf.Instance.UI_SetFire(2, 0f, 0f);
			return;
		}
		switch (control.name)
		{
		case "RotateCamera":
			if (!Screen.lockCursor)
			{
				COMA_PlayerSelf.Instance.UI_SetRotatePlayer(wparam * COMA_Sys.Instance.sensitivity, lparam * COMA_Sys.Instance.sensitivity);
			}
			break;
		case "Button_JoyStick":
			COMA_PlayerSelf.Instance.UI_SetMoveInput(wparam, lparam);
			break;
		case "Button_MoveLeft":
			switch (eventType)
			{
			case 1:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(-1f, 0f);
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				break;
			}
			break;
		case "Button_MoveRight":
			switch (eventType)
			{
			case 1:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(1f, 0f);
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				break;
			}
			break;
		case "Button_LRControll":
			switch (eventType)
			{
			case 1:
			case 2:
				Debug.Log(wparam);
				if (wparam < -0.238f)
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(-1f, 0f);
				}
				else if (wparam > 0.238f)
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(1f, 0f);
				}
				else
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				}
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				break;
			}
			break;
		case "Button_Run_JoyStick":
			switch (eventType)
			{
			case 1:
			case 2:
				if (wparam < -0.001f)
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(-1f, 0f);
				}
				else if (wparam > 0.001f)
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(1f, 0f);
				}
				else
				{
					COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				}
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetMoveInput(0f, 0f);
				break;
			}
			break;
		case "Button_Fire":
			switch (eventType)
			{
			case 1:
				COMA_PlayerSelf.Instance.UI_SetFire(0, 0f, 0f);
				break;
			case 4:
				COMA_PlayerSelf.Instance.UI_SetFire(0, 0f, 0f);
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetFire(2, 0f, 0f);
				break;
			}
			break;
		case "Button_Tank_Fire":
			switch (eventType)
			{
			case 1:
				COMA_PlayerSelf.Instance.UI_SetFire(0, wparam, lparam);
				break;
			case 2:
				COMA_PlayerSelf.Instance.UI_SetFire(0, wparam, lparam);
				break;
			case 4:
				COMA_PlayerSelf.Instance.UI_SetFire(0, wparam, lparam);
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetFire(2, wparam, lparam);
				break;
			}
			break;
		case "Button_Jump":
			if (eventType == 1)
			{
				COMA_PlayerSelf.Instance.UI_Jump();
			}
			break;
		case "Button_ShortAttack":
			switch (eventType)
			{
			case 1:
				COMA_PlayerSelf.Instance.UI_SetFire2(0);
				break;
			case 4:
				COMA_PlayerSelf.Instance.UI_SetFire2(0);
				break;
			case 3:
				COMA_PlayerSelf.Instance.UI_SetFire2(2);
				break;
			}
			break;
		case "Button_SwitchWeapon":
			if (eventType == 3)
			{
				COMA_PlayerSelf.Instance.UI_SetFire2(0);
			}
			break;
		case "Button_Fishing":
			if (TFishingAddressBook.Instance != null && eventType == 1)
			{
				int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
				TMessageDispatcher.Instance.DispatchMsg(-2, iDByName, 0, TTelegram.SEND_MSG_IMMEDIATELY, null);
			}
			break;
		case "Button_Pause":
			if (eventType == 2)
			{
				pauseObj.SetActive(true);
				if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
				{
					COMA_CommonOperation.Instance.GamePause(true);
				}
			}
			break;
		}
	}
}
