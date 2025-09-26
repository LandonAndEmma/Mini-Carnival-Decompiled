public class UILoginFacebookMessageBoxData : UIMessage_CommonBoxData
{
	public UILoginFacebookMessageBoxData()
	{
		_type = UIMessageBoxMgr.EMessageBoxType.FaceBook;
		_channel = (int)_type;
	}
}
