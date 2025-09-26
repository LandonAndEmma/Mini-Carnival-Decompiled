using System;
using UnityEngine;

[Serializable]
public class RPGLevel_SceneUnitPool
{
	[SerializeField]
	public SerializableDictionary<int, RPGLevel_SceneUnit> _dict = new SerializableDictionary<int, RPGLevel_SceneUnit>();
}
