using System;
using UnityEngine;

[Serializable]
public class RPGCareerUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGCareerUnit> _dict = new SerializableDictionary<int, RPGCareerUnit>();
}
