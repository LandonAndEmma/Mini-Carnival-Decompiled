using System;
using LitJson;
using MC_UIToolKit;
using NGUI_COMUI;
using UnityEngine;

public class UIMails_MailBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private GameObject _mailReadObj;

	[SerializeField]
	private GameObject _mailUnreadObj;

	[SerializeField]
	private GameObject _mailGiftObj;

	[SerializeField]
	private UILabel _themeLabel;

	[SerializeField]
	private UILabel _timeLabel;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIMailsMailBox" + i;
		}
		else
		{
			base.gameObject.name = "UIMailsMailBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIMails_MailBoxData uIMails_MailBoxData = base.BoxData as UIMails_MailBoxData;
		if (uIMails_MailBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_mailReadObj.SetActive((uIMails_MailBoxData.MailInfo.m_status != 0) ? true : false);
		_mailUnreadObj.SetActive(uIMails_MailBoxData.MailInfo.m_status == 0);
		DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(uIMails_MailBoxData.MailInfo.m_time);
		_timeLabel.text = dateTime.Year + "." + dateTime.Month + "." + dateTime.Day;
		if (uIMails_MailBoxData.MailInfo.m_type == 1)
		{
			_themeLabel.text = uIMails_MailBoxData.MailInfo.m_title;
			_mailGiftObj.SetActive(false);
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 2)
		{
			string attach = uIMails_MailBoxData.MailInfo.m_attach;
			Debug.Log("Attach:" + attach + "   ID=" + uIMails_MailBoxData.MailInfo.m_id);
			if (attach == string.Empty)
			{
				_themeLabel.text = uIMails_MailBoxData.MailInfo.m_title;
				_mailGiftObj.SetActive(false);
				return;
			}
			JsonData jsonData = JsonMapper.ToObject<JsonData>(attach);
			int num = int.Parse(jsonData["type"].ToString());
			if (num == 1)
			{
				string sender_name = jsonData["nick"].ToString();
				uIMails_MailBoxData.MailInfo.m_sender_name = sender_name;
			}
			_themeLabel.text = UIGolbalStaticFun.GetMailThemeByType((byte)num, uIMails_MailBoxData);
			if (attach.Contains("award") && uIMails_MailBoxData.MailInfo.m_status != 2)
			{
				_mailGiftObj.SetActive(true);
			}
			else
			{
				_mailGiftObj.SetActive(false);
			}
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 3)
		{
			Debug.Log(uIMails_MailBoxData.MailInfo.m_sender_name);
			string text = TUITool.StringFormat(Localization.instance.Get("youxiang_title10"), uIMails_MailBoxData.MailInfo.m_sender_name);
			_themeLabel.text = text;
			_mailGiftObj.SetActive((uIMails_MailBoxData.MailInfo.m_status != 2) ? true : false);
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 4)
		{
			string text2 = TUITool.StringFormat(Localization.instance.Get("youxiang_title12"), uIMails_MailBoxData.MailInfo.m_sender_name);
			_themeLabel.text = text2;
			_mailGiftObj.SetActive((uIMails_MailBoxData.MailInfo.m_status != 2) ? true : false);
		}
	}
}
