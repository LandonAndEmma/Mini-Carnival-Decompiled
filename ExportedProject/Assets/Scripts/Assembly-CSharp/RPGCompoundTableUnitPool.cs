using System;
using UnityEngine;

[Serializable]
public class RPGCompoundTableUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGGemCompoundTableUnit> _dict = new SerializableDictionary<int, RPGGemCompoundTableUnit>();
}
