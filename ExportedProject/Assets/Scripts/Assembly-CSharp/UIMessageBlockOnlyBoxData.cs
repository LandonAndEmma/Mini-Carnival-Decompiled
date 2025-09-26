public class UIMessageBlockOnlyBoxData : UIMessage_BoxData
{
	public UIMessageBlockOnlyBoxData()
	{
		_type = UIMessageBoxMgr.EMessageBoxType.OnlyBlock;
		_layout = 3;
		_channel = (int)_type;
	}
}
