using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGCareerUnit
{
	[SerializeField]
	public enum ERoleMainAttr
	{
		Main_STR = 0,
		Main_DEX = 1,
		Main_INT = 2,
		MAX = 3
	}

	[SerializeField]
	public enum ECareerAttackType
	{
		None = 0,
		Melee = 1,
		Remote = 2
	}

	[SerializeField]
	private int _careerId;

	[SerializeField]
	private byte _starGrade;

	[SerializeField]
	private string _careerName;

	[SerializeField]
	private ERoleMainAttr _mainAttr;

	[SerializeField]
	private ECareerAttackType _attackType;

	[SerializeField]
	private int _beAttackPriority;

	[SerializeField]
	private float[] _attrValue = new float[3];

	[SerializeField]
	private List<RPGDressUnit> _dressUnitLst = new List<RPGDressUnit>();

	[SerializeField]
	private List<int> _skillLst = new List<int>();

	public int CareerId
	{
		get
		{
			return _careerId;
		}
		set
		{
			_careerId = value;
		}
	}

	public byte StarGrade
	{
		get
		{
			return _starGrade;
		}
		set
		{
			_starGrade = value;
		}
	}

	public string CareerName
	{
		get
		{
			return _careerName;
		}
		set
		{
			_careerName = value;
		}
	}

	public ERoleMainAttr MainAttr
	{
		get
		{
			return _mainAttr;
		}
		set
		{
			_mainAttr = value;
		}
	}

	public ECareerAttackType AttackType
	{
		get
		{
			return _attackType;
		}
		set
		{
			_attackType = value;
		}
	}

	public int BeAttackPriority
	{
		get
		{
			return _beAttackPriority;
		}
		set
		{
			_beAttackPriority = value;
		}
	}

	public float[] AttrValue
	{
		get
		{
			return _attrValue;
		}
		set
		{
			_attrValue = value;
		}
	}

	public List<RPGDressUnit> DressUnitLst
	{
		get
		{
			return _dressUnitLst;
		}
		set
		{
			_dressUnitLst = value;
		}
	}

	public List<int> SkillLst
	{
		get
		{
			return _skillLst;
		}
		set
		{
			_skillLst = value;
		}
	}

	public RPGCareerUnit()
	{
	}

	public RPGCareerUnit(RPGCareerUnit unit)
	{
		AttackType = unit.AttackType;
		AttrValue = unit.AttrValue;
		CareerId = unit.CareerId;
		CareerName = unit.CareerName;
		DressUnitLst = unit.DressUnitLst;
		MainAttr = unit.MainAttr;
		SkillLst = unit.SkillLst;
		StarGrade = unit.StarGrade;
	}
}
