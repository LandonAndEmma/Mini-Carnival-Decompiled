using NGUI_COMUI;
using Protocol;

public class UIRPG_MyTeamSelEquipBtnBoxData : NGUI_COMUI.UI_BoxData
{
	private Equip _equipData;

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
