using NGUI_COMUI;
using Protocol;

public class UIRPG_MyTeam_SelEquipBoxData : NGUI_COMUI.UI_BoxData
{
	private bool _isEquip;

	private bool _isEquipBySelf;

	private Equip _equipData;

	public bool IsEquip
	{
		get
		{
			return _isEquip;
		}
		set
		{
			_isEquip = value;
		}
	}

	public bool IsEquipBySelf
	{
		get
		{
			return _isEquipBySelf;
		}
		set
		{
			_isEquipBySelf = value;
		}
	}

	public Equip EquipData
	{
		get
		{
			return _equipData;
		}
		set
		{
			_equipData = value;
		}
	}
}
