using System;
using UnityEngine;

[Serializable]
public class RPGGemDropUnitPool
{
	[SerializeField]
	public SerializableDictionary<string, RPGGemDropUnit> _dict = new SerializableDictionary<string, RPGGemDropUnit>();
}
