public class UIMessageBlockForTUIBoxData : UIMessage_BoxData
{
	public UIMessageBlockForTUIBoxData()
	{
		_type = UIMessageBoxMgr.EMessageBoxType.BlockForTUI;
		_layout = 3;
		_channel = (int)_type;
	}
}
