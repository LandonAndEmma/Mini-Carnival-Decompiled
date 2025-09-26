public class UIACPopupBoxData : UIMessage_BoxData
{
	private int _acId;

	public int ACId
	{
		get
		{
			return _acId;
		}
		set
		{
			_acId = value;
		}
	}

	public UIACPopupBoxData(int id)
	{
		_type = UIMessageBoxMgr.EMessageBoxType.Achievement;
		_channel = (int)_type;
		_acId = id;
	}
}
