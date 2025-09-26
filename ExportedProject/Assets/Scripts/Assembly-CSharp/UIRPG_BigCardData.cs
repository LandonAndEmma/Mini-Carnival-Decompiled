using NGUI_COMUI;

public class UIRPG_BigCardData : NGUI_COMUI.UI_BoxData
{
	private int _id;

	private bool _bShowInfo = true;

	public int CardId
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public bool ShowInfoBtn
	{
		get
		{
			return _bShowInfo;
		}
		set
		{
			_bShowInfo = value;
		}
	}

	public UIRPG_BigCardData()
	{
		_bShowInfo = false;
	}

	public UIRPG_BigCardData(int id)
	{
		_bShowInfo = false;
		_id = id;
	}
}
