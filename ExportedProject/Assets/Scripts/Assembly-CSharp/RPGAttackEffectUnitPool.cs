using System;
using UnityEngine;

[Serializable]
public class RPGAttackEffectUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGAttackEffectUnit> _dict = new SerializableDictionary<int, RPGAttackEffectUnit>();
}
