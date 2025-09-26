public class UIMessageIAPBlockBoxData : UIMessage_BoxData
{
	public UIMessageIAPBlockBoxData()
	{
		_type = UIMessageBoxMgr.EMessageBoxType.IAPBlock;
		_layout = 3;
		_channel = (int)_type;
	}
}
