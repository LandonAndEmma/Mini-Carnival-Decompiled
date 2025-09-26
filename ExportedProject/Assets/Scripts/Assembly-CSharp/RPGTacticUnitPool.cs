using System;
using UnityEngine;

[Serializable]
public class RPGTacticUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGTacticUnit> _dict = new SerializableDictionary<int, RPGTacticUnit>();
}
