using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGGemCompoundTableUnit
{
	public enum EAttrType
	{
		_boutLimit_ATT = 0,
		_boutLimit_DEF = 1,
		_ATT = 2,
		_DEF = 3,
		_DodgeRate = 4,
		_CriticalRate = 5,
		_ATIndex = 6,
		_HP = 7,
		_CriticalMultiplier = 8,
		_ITDValue = 9,
		_ResistLimitBuffRate = 10,
		_SuckHP = 11,
		_LimitRHP = 12
	}

	[SerializeField]
	public int _combinationKey;

	[SerializeField]
	public string _apDes;

	[SerializeField]
	public EAttrType _attrType;

	[SerializeField]
	public List<int> _apList = new List<int>();
}
