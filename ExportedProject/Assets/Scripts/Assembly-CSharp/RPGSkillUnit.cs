using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGSkillUnit
{
	[SerializeField]
	public enum ESkillDirectivity
	{
		NONE = 0,
		AOE = 1,
		SA = 2,
		SPEC = 3
	}

	[SerializeField]
	public enum ESkillType
	{
		Active = 0,
		Passive = 1
	}

	[SerializeField]
	public enum ESkillTriggerCond
	{
		Awake = 0,
		Begin_Bout = 1,
		Pre_Attack = 2,
		Aft_Attack = 3,
		End_Bout = 4
	}

	[SerializeField]
	public enum ESkillEffectType
	{
		Generate_Tactic = 0,
		Generate_Buff = 1,
		Special_Attack = 2
	}

	private static int _nextAvailableId;

	[SerializeField]
	private int _skillId;

	[SerializeField]
	private string _skillDes;

	[SerializeField]
	private ESkillDirectivity _skillDirec;

	[SerializeField]
	private ESkillType _skillType;

	[SerializeField]
	private ESkillTriggerCond _skillTrigCond;

	[SerializeField]
	private ESkillEffectType _skillEffectType;

	[SerializeField]
	private bool _canDAMShare;

	[SerializeField]
	private List<object> _paramLst = new List<object>();

	public int SkillId
	{
		get
		{
			return _skillId;
		}
		set
		{
			_skillId = value;
		}
	}

	public string SkillDes
	{
		get
		{
			return _skillDes;
		}
		set
		{
			_skillDes = value;
		}
	}

	public ESkillDirectivity SkillDirec
	{
		get
		{
			return _skillDirec;
		}
		set
		{
			_skillDirec = value;
		}
	}

	public ESkillType SkillType
	{
		get
		{
			return _skillType;
		}
		set
		{
			_skillType = value;
		}
	}

	public ESkillTriggerCond SkillTrigCond
	{
		get
		{
			return _skillTrigCond;
		}
		set
		{
			_skillTrigCond = value;
		}
	}

	public ESkillEffectType SkillEffectType
	{
		get
		{
			return _skillEffectType;
		}
		set
		{
			_skillEffectType = value;
		}
	}

	public bool CanDAMShare
	{
		get
		{
			return _canDAMShare;
		}
		set
		{
			_canDAMShare = value;
		}
	}

	public List<object> ParamLst
	{
		get
		{
			return _paramLst;
		}
		set
		{
			_paramLst = value;
		}
	}
}
