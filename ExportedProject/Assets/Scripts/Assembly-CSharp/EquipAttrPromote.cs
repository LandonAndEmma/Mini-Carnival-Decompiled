using System;
using UnityEngine;

[Serializable]
public class EquipAttrPromote
{
	[SerializeField]
	public float _promoteValue;

	[SerializeField]
	public int _boutCount;

	public EquipAttrPromote()
	{
		_promoteValue = 0f;
		_boutCount = 10000000;
	}
}
