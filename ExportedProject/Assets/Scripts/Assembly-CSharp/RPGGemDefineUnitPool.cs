using System;
using UnityEngine;

[Serializable]
public class RPGGemDefineUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGGemDefineUnit> _dict = new SerializableDictionary<int, RPGGemDefineUnit>();
}
