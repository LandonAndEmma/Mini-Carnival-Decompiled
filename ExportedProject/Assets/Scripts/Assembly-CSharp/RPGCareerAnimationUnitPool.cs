using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGCareerAnimationUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, List<RPGCareerAnimationUnit>> _dict = new SerializableDictionary<int, List<RPGCareerAnimationUnit>>();
}
