using MC_UIToolKit;
using MessageID;
using MiscToolKits;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class UIInputName : UIMessageHandler
{
	[SerializeField]
	private UIInputName_InputBox nameInputBox;

	[SerializeField]
	private TUILabel _labelTitle;

	private static UIInputName _instance;

	[SerializeField]
	private TUILabel _labelTip;

	private GameObject _waitingBox;

	public static UIInputName Instance
	{
		get
		{
			return _instance;
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	public void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
	}

	public void Start()
	{
		switch (COMA_CommonOperation.Instance.inputKind)
		{
		case InputBoardCategory.Register:
			_labelTitle.TextID = "yonghumingjiemian_desc1";
			_labelTip.TextID = "yonghumingjiemian_desc2";
			break;
		case InputBoardCategory.SearchFriend:
			_labelTitle.TextID = "haoyoujiemian_biaoti1";
			_labelTip.TextID = "haoyoujiemian_desc15";
			break;
		case InputBoardCategory.Rename:
			_labelTitle.TextID = "yonghumingjiemian_desc1";
			_labelTip.TextID = "yonghumingjiemian_desc2";
			break;
		}
	}

	public void HandleEventButtonInputBox_Name(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			nameInputBox.ShowKeyboard();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_ConfirmInput(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			nameInputBox.CloseKeyboard();
			if (nameInputBox.InputText == string.Empty)
			{
				if (COMA_CommonOperation.Instance.inputKind == InputBoardCategory.SearchFriend)
				{
					LeaveAnim(string.Empty);
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_InputNameEnd_Search, null, string.Empty);
				}
				break;
			}
			string text = nameInputBox.InputText.ToUpper();
			switch (COMA_CommonOperation.Instance.inputKind)
			{
			case InputBoardCategory.Register:
				if (MiscStaticTools.SF_IsPureNumber(text))
				{
					UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("yonghumingjiemian_desc3"));
					UIGolbalStaticFun.PopCommonMessageBox(data2);
					return;
				}
				if (text.Length > 10)
				{
					text = text.Substring(0, 10);
				}
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_InputNameEnd_Register, null, text);
				break;
			case InputBoardCategory.SearchFriend:
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_InputNameEnd_Search, null, text);
				break;
			case InputBoardCategory.Rename:
			{
				if (MiscStaticTools.SF_IsPureNumber(text))
				{
					text = "P" + text;
				}
				if (text.Length > 10)
				{
					text = text.Substring(0, 10);
				}
				NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
				notifyRoleDataCmd.m_info.m_name = text;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_SetData, null, notifyRoleDataCmd);
				ModifyNicknameCmd modifyNicknameCmd = new ModifyNicknameCmd();
				modifyNicknameCmd.m_nickName = text;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyNicknameCmd);
				if (UIOptions.Instance != null)
				{
					UIOptions.Instance.DisableBlock();
				}
				break;
			}
			default:
				Debug.LogError("inputKind is None!");
				break;
			}
			LeaveAnim(string.Empty);
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

	public void LeaveAnim(string sceneName)
	{
		_aniControl.PlayExitAni(sceneName);
	}

	private void WaitingBox()
	{
		GameObject boxPrefab = Resources.Load("UI/Misc/IAPBox") as GameObject;
		UI_MsgBox uI_MsgBox = MessageBox("Loading...", string.Empty, 0, boxPrefab, -300f);
		_waitingBox = uI_MsgBox.gameObject;
		_waitingBox.transform.parent = base.transform.FindChild("TUIControls");
	}

	public bool DestroyWaitingBox()
	{
		if (_waitingBox != null)
		{
			Object.Destroy(_waitingBox);
			_waitingBox = null;
		}
		return false;
	}
}
