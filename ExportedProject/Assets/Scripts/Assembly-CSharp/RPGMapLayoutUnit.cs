using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGMapLayoutUnit
{
	[SerializeField]
	public int _levelId;

	[SerializeField]
	public List<int> _frontLevelList = new List<int>();

	[SerializeField]
	public List<int> _followLevelList = new List<int>();
}
