using Protocol;

public class UIRPG_MyTeam_SelEquipTakeOrPutData
{
	private bool _isPutOn;

	private Equip _equipData;

	public bool IsPutOn
	{
		get
		{
			return _isPutOn;
		}
		set
		{
			_isPutOn = value;
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

	public UIRPG_MyTeam_SelEquipTakeOrPutData(Equip equip)
	{
		_equipData = equip;
	}
}
