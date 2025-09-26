using MessageID;
using Protocol.Role.C2S;
using UnityEngine;

public class UISwitchButton : MonoBehaviour
{
	[SerializeField]
	private GameObject picOn;

	[SerializeField]
	private GameObject picOff;

	public UI_ButtonLight btnOn;

	public UI_ButtonLight btnOff;

	private bool bSwitchOn;

	public bool SwitchOn
	{
		get
		{
			return bSwitchOn;
		}
	}

	private void Start()
	{
		if (base.name == "Music_Switch")
		{
			Init(COMA_AudioManager.Instance.bMusic);
		}
		else if (base.name == "Sound_Switch")
		{
			Init(COMA_AudioManager.Instance.bSound);
		}
		else if (base.name == "Accept_Switch")
		{
			UIDataBufferCenter.Instance.GetAcceptFriendRequest();
			Init(UIDataBufferCenter.Instance._bAcceptFriendRequest);
		}
	}

	private void Update()
	{
	}

	public void Init(bool bOn)
	{
		bSwitchOn = bOn;
		if (bOn)
		{
			picOn.SetActive(true);
			picOff.SetActive(false);
		}
		else
		{
			picOn.SetActive(false);
			picOff.SetActive(true);
		}
	}

	public void HandleEventButton_On(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_On-CommandClick + " + base.name + " On");
			if (base.name == "Music_Switch")
			{
				COMA_AudioManager.Instance.bMusic = true;
			}
			else if (base.name == "Sound_Switch")
			{
				COMA_AudioManager.Instance.bSound = true;
			}
			else if (base.name == "Accept_Switch")
			{
				SetForbidFriendRequestCmd setForbidFriendRequestCmd = new SetForbidFriendRequestCmd();
				setForbidFriendRequestCmd.m_val = 0;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, setForbidFriendRequestCmd);
				UIDataBufferCenter.Instance._bAcceptFriendRequest = true;
			}
			Init(true);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			if (!SwitchOn && btnOn != null)
			{
				btnOn.LightOn();
			}
			break;
		case 2:
			if (!SwitchOn && btnOn != null)
			{
				btnOn.LightOff();
			}
			break;
		}
	}

	public void HandleEventButton_Off(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Off-CommandClick + " + base.name + " Off");
			if (base.name == "Music_Switch")
			{
				COMA_AudioManager.Instance.bMusic = false;
			}
			else if (base.name == "Sound_Switch")
			{
				COMA_AudioManager.Instance.bSound = false;
			}
			else if (base.name == "Accept_Switch")
			{
				SetForbidFriendRequestCmd setForbidFriendRequestCmd = new SetForbidFriendRequestCmd();
				setForbidFriendRequestCmd.m_val = 1;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, setForbidFriendRequestCmd);
				UIDataBufferCenter.Instance._bAcceptFriendRequest = false;
			}
			Init(false);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			if (SwitchOn && btnOff != null)
			{
				btnOff.LightOn();
			}
			break;
		case 2:
			if (SwitchOn && btnOff != null)
			{
				btnOff.LightOff();
			}
			break;
		}
	}
}
