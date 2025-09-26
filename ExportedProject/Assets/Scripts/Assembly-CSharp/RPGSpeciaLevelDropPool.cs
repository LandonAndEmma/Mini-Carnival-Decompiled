using System;
using UnityEngine;

[Serializable]
public class RPGSpeciaLevelDropPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGSpeciaLevelDrop> _dict = new SerializableDictionary<int, RPGSpeciaLevelDrop>();
}
