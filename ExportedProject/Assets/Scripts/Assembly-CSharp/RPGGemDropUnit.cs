using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGGemDropUnit
{
	[SerializeField]
	public string _gameId;

	[SerializeField]
	public List<RPGGemDefineUnit.EGemColor> _dropList = new List<RPGGemDefineUnit.EGemColor>();
}
