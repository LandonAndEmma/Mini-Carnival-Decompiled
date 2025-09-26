using System;
using UnityEngine;

[Serializable]
public class RPGSkillUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGSkillUnit> _dict = new SerializableDictionary<int, RPGSkillUnit>();
}
