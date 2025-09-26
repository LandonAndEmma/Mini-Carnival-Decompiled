using System;
using NGUI_COMUI;
using Protocol;

public class UIRPG_BackPack_Avatar_BoxData : NGUI_COMUI.UI_BoxData, IComparable
{
	public enum EDataType
	{
		None = 0,
		FirstLocked = 1,
		Locked = 2,
		Data = 3
	}

	private bool _isHasEquip;

	private Equip _equipAvatar;

	public bool IsHasEquip
	{
		get
		{
			return _isHasEquip;
		}
		set
		{
			_isHasEquip = value;
		}
	}

	public Equip EquipAvatar
	{
		get
		{
			return _equipAvatar;
		}
		set
		{
			_equipAvatar = value;
		}
	}

	public UIRPG_BackPack_Avatar_BoxData()
	{
	}

	public UIRPG_BackPack_Avatar_BoxData(ulong id, string name)
	{
		_itemId = id;
		_unit = name;
	}

	public int CompareTo(object obj)
	{
		int num = 0;
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = obj as UIRPG_BackPack_Avatar_BoxData;
		if (_equipAvatar.m_level == uIRPG_BackPack_Avatar_BoxData._equipAvatar.m_level)
		{
			return 0;
		}
		if (_equipAvatar.m_level < uIRPG_BackPack_Avatar_BoxData._equipAvatar.m_level)
		{
			return -1;
		}
		return 1;
	}
}
