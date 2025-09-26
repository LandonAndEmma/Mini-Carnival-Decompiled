using NGUI_COMUI;

public class UIBackpack_BoxData : NGUI_COMUI.UI_BoxData
{
	public enum EDataType
	{
		Locked = 0,
		None = 1,
		Avatar_Head = 2,
		Avatar_Body = 3,
		Avatar_Leg = 4,
		Decoration = 5
	}

	public enum EDataState
	{
		Unknow = 0,
		NoEditNoSell = 1,
		CanEditNoSell = 2,
		CanEditCanSell = 3
	}

	private EDataState _dataState;

	public EDataState DataState
	{
		get
		{
			return _dataState;
		}
		set
		{
			_dataState = value;
		}
	}

	public UIBackpack_BoxData()
	{
		base.DataType = 1;
		_dataState = EDataState.Unknow;
		_itemId = 0uL;
		_unit = string.Empty;
	}
}
