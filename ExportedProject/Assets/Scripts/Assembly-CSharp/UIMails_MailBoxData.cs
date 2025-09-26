using NGUI_COMUI;
using Protocol;

public class UIMails_MailBoxData : NGUI_COMUI.UI_BoxData
{
	private Email _mailInfo;

	public Email MailInfo
	{
		get
		{
			return _mailInfo;
		}
		set
		{
			_mailInfo = value;
		}
	}
}
