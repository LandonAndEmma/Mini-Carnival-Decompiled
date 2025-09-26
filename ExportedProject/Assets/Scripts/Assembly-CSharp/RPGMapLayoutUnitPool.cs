using System;
using UnityEngine;

[Serializable]
public class RPGMapLayoutUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGMapLayoutUnit> _dict = new SerializableDictionary<int, RPGMapLayoutUnit>();
}
