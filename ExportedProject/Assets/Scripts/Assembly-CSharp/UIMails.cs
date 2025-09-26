using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.Mail.C2S;
using UIGlobal;
using UnityEngine;

public class UIMails : UIEntity
{
	public enum ECaptionType
	{
		None = -1,
		All = 0,
		Notice = 1,
		System = 2,
		Friend = 3
	}

	private const string c_HasItemToGet = "HasItemToGet";

	private const string c_DelNoticeItem = "DelNoticeItem";

	private const string c_DelOtherMailItem = "DelOtherMailItem";

	private List<uint> _lstDelOther = new List<uint>();

	[SerializeField]
	private UILabel _newMailNumLabel;

	[SerializeField]
	private UILabel _totalMailNumLabel;

	[SerializeField]
	private UIMails_MailContainer _uiMailContainer;

	[SerializeField]
	private ECaptionType _curCaptionBtnType;

	private ECaptionType _preCaptionType = ECaptionType.None;

	private bool _checkMailFull;

	public ECaptionType CurCaptionBtnType
	{
		get
		{
			return _curCaptionBtnType;
		}
		set
		{
			_curCaptionBtnType = value;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMails_CaptionTypeChanged, this, CaptionTypeChanged);
		RegisterMessage(EUIMessageID.UIMails_CloseMails, this, CloseMails);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteMailInfoArrived, this, RemoteMailInfoArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this, MailDataChanged);
		RegisterMessage(EUIMessageID.UI_DelAllReadedMails, this, DelAllReadedMails);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, UICOMBoxYesClick);
		RegisterMessage(EUIMessageID.UICOMBox_NoClick, this, UICOMBoxNoClick);
		_checkMailFull = true;
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMails_CaptionTypeChanged, this);
		UnregisterMessage(EUIMessageID.UIMails_CloseMails, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteMailInfoArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this);
		UnregisterMessage(EUIMessageID.UI_DelAllReadedMails, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UICOMBox_NoClick, this);
	}

	private bool CaptionTypeChanged(TUITelegram msg)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_CloseMail, null, null);
		_curCaptionBtnType = (ECaptionType)(int)msg._pExtraInfo;
		RefreshMailContainer();
		_preCaptionType = CurCaptionBtnType;
		return true;
	}

	private bool MailDataChanged(TUITelegram msg)
	{
		RefreshMailContainer();
		return true;
	}

	private bool HasItemToGet(Email MailInfo)
	{
		string attach = MailInfo.m_attach;
		if (MailInfo.m_type >= 3)
		{
			if (MailInfo.m_status == 2)
			{
				return false;
			}
			return true;
		}
		if (MailInfo.m_status == 2 || attach == string.Empty || !attach.Contains("award"))
		{
			return false;
		}
		Debug.Log("HasItemToGet:" + attach);
		return true;
	}

	private bool UICOMBoxYesClick(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = (UIMessage_CommonBoxData)msg._pExtraInfo;
		if (uIMessage_CommonBoxData != null && uIMessage_CommonBoxData.Mark == "HasItemToGet")
		{
			if (CurCaptionBtnType == ECaptionType.All)
			{
				List<uint> list = new List<uint>();
				int count = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
				for (int i = 0; i < count; i++)
				{
					Email email = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[i];
					list.Add(email.m_id);
				}
				if (list.Count > 0)
				{
					UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(list.Count);
					for (int j = 0; j < list.Count; j++)
					{
						DelMailCmd delMailCmd = new DelMailCmd();
						delMailCmd.m_mail_id = list[j];
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delMailCmd);
					}
				}
			}
			else if (CurCaptionBtnType == ECaptionType.System)
			{
				List<uint> list2 = new List<uint>();
				int count2 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
				for (int k = 0; k < count2; k++)
				{
					Email email2 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[k];
					if (email2.m_type == 2)
					{
						list2.Add(email2.m_id);
					}
				}
				if (list2.Count > 0)
				{
					UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(list2.Count);
					for (int l = 0; l < list2.Count; l++)
					{
						DelMailCmd delMailCmd2 = new DelMailCmd();
						delMailCmd2.m_mail_id = list2[l];
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delMailCmd2);
					}
				}
			}
		}
		else if (uIMessage_CommonBoxData != null && uIMessage_CommonBoxData.Mark == "DelNoticeItem")
		{
			List<uint> list3 = new List<uint>();
			int num = 0;
			int count3 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
			for (int m = 0; m < count3; m++)
			{
				Email email3 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[m];
				if (email3.m_type == 1)
				{
					num++;
					list3.Add(email3.m_id);
				}
			}
			if (num > 0)
			{
				UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(num);
				for (int n = 0; n < num; n++)
				{
					DelMailCmd delMailCmd3 = new DelMailCmd();
					delMailCmd3.m_mail_id = list3[n];
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delMailCmd3);
				}
			}
		}
		else if (uIMessage_CommonBoxData != null && uIMessage_CommonBoxData.Mark == "DelOtherMailItem" && _lstDelOther.Count > 0)
		{
			UIAutoDelBlockOnlyMessageBoxMgr.Instance.PopAutoDelBlockOnlyMessageBox(_lstDelOther.Count);
			for (int num2 = 0; num2 < _lstDelOther.Count; num2++)
			{
				DelMailCmd delMailCmd4 = new DelMailCmd();
				delMailCmd4.m_mail_id = _lstDelOther[num2];
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delMailCmd4);
			}
			_lstDelOther.Clear();
		}
		return true;
	}

	private bool UICOMBoxNoClick(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = (UIMessage_CommonBoxData)msg._pExtraInfo;
		return true;
	}

	private bool DelAllReadedMails(TUITelegram msg)
	{
		List<uint> list = new List<uint>();
		bool flag = true;
		if (UIDataBufferCenter.Instance.MailBufferInfo != null)
		{
			if (CurCaptionBtnType == ECaptionType.All)
			{
				int count = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
				for (int i = 0; i < count; i++)
				{
					Email email = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[i];
					if (HasItemToGet(email) && !UIGolbalStaticFun.IsFriendRequestMail(email))
					{
						flag = false;
						break;
					}
					list.Add(email.m_id);
				}
			}
			else if (CurCaptionBtnType == ECaptionType.Notice)
			{
				List<uint> list2 = new List<uint>();
				int num = 0;
				int count2 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
				for (int j = 0; j < count2; j++)
				{
					Email email2 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[j];
					if (email2.m_type == 1)
					{
						num++;
						list2.Add(email2.m_id);
					}
				}
				if (num > 0)
				{
					UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("youxiang_desc6"));
					uIMessage_CommonBoxData.Mark = "DelNoticeItem";
					UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
				}
			}
			else if (CurCaptionBtnType == ECaptionType.System)
			{
				int count3 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
				for (int k = 0; k < count3; k++)
				{
					Email email3 = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[k];
					if (email3.m_type == 2 && HasItemToGet(email3) && !UIGolbalStaticFun.IsFriendRequestMail(email3))
					{
						flag = false;
						break;
					}
					if (email3.m_type == 2)
					{
						list.Add(email3.m_id);
					}
				}
			}
			if (!flag)
			{
				UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(0, Localization.instance.Get("youxiang_desc5"));
				uIMessage_CommonBoxData2.Mark = "HasItemToGet";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
			}
			else if (list.Count > 0)
			{
				_lstDelOther = list;
				UIMessage_CommonBoxData uIMessage_CommonBoxData3 = new UIMessage_CommonBoxData(0, Localization.instance.Get("youxiang_desc6"));
				uIMessage_CommonBoxData3.Mark = "DelOtherMailItem";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData3);
			}
		}
		return true;
	}

	private void RefreshMailContainer()
	{
		if (UIDataBufferCenter.Instance.MailBufferInfo == null)
		{
			return;
		}
		if (CurCaptionBtnType == ECaptionType.All)
		{
			byte b = 0;
			int count = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
			_uiMailContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
			_uiMailContainer.InitBoxs(count, true);
			for (int i = 0; i < count; i++)
			{
				UIMails_MailBoxData uIMails_MailBoxData = new UIMails_MailBoxData();
				uIMails_MailBoxData.MailInfo = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[i];
				_uiMailContainer.SetBoxData(i, uIMails_MailBoxData);
				if (uIMails_MailBoxData.MailInfo.m_status == 0)
				{
					b++;
				}
			}
			_newMailNumLabel.transform.parent.gameObject.SetActive(true);
		}
		else if (CurCaptionBtnType == ECaptionType.Notice)
		{
			byte newNum = 0;
			int num = ExtractClassifyMail(Email.Type.kNotice, out newNum);
			_newMailNumLabel.transform.parent.gameObject.SetActive(false);
		}
		else if (CurCaptionBtnType == ECaptionType.System)
		{
			byte newNum2 = 0;
			int num2 = ExtractClassifyMail(Email.Type.kSystem, out newNum2);
			_newMailNumLabel.transform.parent.gameObject.SetActive(false);
		}
		_newMailNumLabel.text = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count.ToString();
		_totalMailNumLabel.text = COMA_DataConfig.Instance._sysConfig.Mail.max_size.ToString();
	}

	private int ExtractClassifyMail(Email.Type type, out byte newNum)
	{
		int count = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count;
		_uiMailContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_uiMailContainer.InitBoxs(0, true);
		int result = 0;
		byte b = 0;
		for (int i = 0; i < count; i++)
		{
			if (UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[i].m_type >= (byte)type)
			{
				UIMails_MailBoxData uIMails_MailBoxData = new UIMails_MailBoxData();
				uIMails_MailBoxData.MailInfo = UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list[i];
				_uiMailContainer.AddBox();
				_uiMailContainer.SetBoxData(result++, uIMails_MailBoxData);
				if (uIMails_MailBoxData.MailInfo.m_status == 0)
				{
					b++;
				}
			}
		}
		newNum = b;
		return result;
	}

	private bool RemoteMailInfoArrived(TUITelegram msg)
	{
		RefreshMailContainer();
		return true;
	}

	private bool CloseMails(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GoSquareScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GoSquareScene(object obj)
	{
		Debug.Log("GoSquareScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	protected override void Tick()
	{
		if (_checkMailFull)
		{
			if (UIDataBufferCenter.Instance.MailBufferInfo.m_num_of_mail > UIDataBufferCenter.Instance.MailBufferInfo.m_mail_list.Count)
			{
				UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc1"));
			}
			_checkMailFull = false;
		}
	}
}
