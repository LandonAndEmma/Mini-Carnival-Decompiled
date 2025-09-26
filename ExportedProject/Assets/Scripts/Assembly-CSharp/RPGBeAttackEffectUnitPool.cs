using System;
using UnityEngine;

[Serializable]
public class RPGBeAttackEffectUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGBeAttackEffectUnit> _dict = new SerializableDictionary<int, RPGBeAttackEffectUnit>();
}
