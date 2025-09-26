using NGUI_COMUI;

public class UIMessage_BoxData : NGUI_COMUI.UI_BoxData
{
	private int _id = -1;

	protected static int _nextValidId;

	private string _strMark;

	protected UIMessageBoxMgr.EMessageBoxType _type;

	protected int _layout;

	protected int _channel;

	public int MessageBoxID
	{
		get
		{
			return _id;
		}
	}

	public string Mark
	{
		get
		{
			return _strMark;
		}
		set
		{
			_strMark = value;
		}
	}

	public UIMessageBoxMgr.EMessageBoxType Type
	{
		get
		{
			return _type;
		}
	}

	public int Layout
	{
		get
		{
			return _layout;
		}
		set
		{
			_layout = value;
		}
	}

	public int Channel
	{
		get
		{
			return _channel;
		}
	}

	protected UIMessage_BoxData()
	{
		_id = _nextValidId;
		_nextValidId++;
	}
}
