public class UIMessage_CommonBoxData : UIMessage_BoxData
{
	private string _des;

	private string _yesAndNoLabelYes;

	private string _yesAndNoLabelNo;

	private string _onlyYesLabelYes;

	public string Description
	{
		get
		{
			return _des;
		}
		set
		{
			_des = value;
		}
	}

	public string YesAndNoLabelYes
	{
		get
		{
			return _yesAndNoLabelYes;
		}
		set
		{
			_yesAndNoLabelYes = value;
		}
	}

	public string YesAndNoLabelNo
	{
		get
		{
			return _yesAndNoLabelNo;
		}
		set
		{
			_yesAndNoLabelNo = value;
		}
	}

	public string OnlyYesLabelYes
	{
		get
		{
			return _onlyYesLabelYes;
		}
		set
		{
			_onlyYesLabelYes = value;
		}
	}

	public UIMessage_CommonBoxData()
	{
		_type = UIMessageBoxMgr.EMessageBoxType.CommonBox;
		_layout = 0;
		_channel = (int)_type;
		_yesAndNoLabelYes = string.Empty;
		_yesAndNoLabelNo = string.Empty;
		_onlyYesLabelYes = string.Empty;
	}

	public UIMessage_CommonBoxData(string des)
	{
		_des = des;
		_type = UIMessageBoxMgr.EMessageBoxType.CommonBox;
		_layout = 0;
		_channel = (int)_type;
		_yesAndNoLabelYes = string.Empty;
		_yesAndNoLabelNo = string.Empty;
		_onlyYesLabelYes = string.Empty;
	}

	public UIMessage_CommonBoxData(int layout, string des)
	{
		_des = des;
		_type = UIMessageBoxMgr.EMessageBoxType.CommonBox;
		_layout = layout;
		_channel = (int)_type;
		_yesAndNoLabelYes = string.Empty;
		_yesAndNoLabelNo = string.Empty;
		_onlyYesLabelYes = string.Empty;
	}

	public UIMessage_CommonBoxData(int layout, string des, string yesLabel, string noLabel)
	{
		_des = des;
		_type = UIMessageBoxMgr.EMessageBoxType.CommonBox;
		_layout = layout;
		_channel = (int)_type;
		_yesAndNoLabelYes = yesLabel;
		_yesAndNoLabelNo = noLabel;
		_onlyYesLabelYes = string.Empty;
	}

	public UIMessage_CommonBoxData(int layout, string des, string yesLabel)
	{
		_des = des;
		_type = UIMessageBoxMgr.EMessageBoxType.CommonBox;
		_layout = layout;
		_channel = (int)_type;
		_yesAndNoLabelYes = string.Empty;
		_yesAndNoLabelNo = string.Empty;
		_onlyYesLabelYes = yesLabel;
	}
}
