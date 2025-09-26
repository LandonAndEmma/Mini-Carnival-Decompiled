using System;
using UnityEngine;

[Serializable]
public class RPGBuffEffectUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGBuffEffectUnit> _dict = new SerializableDictionary<int, RPGBuffEffectUnit>();
}
