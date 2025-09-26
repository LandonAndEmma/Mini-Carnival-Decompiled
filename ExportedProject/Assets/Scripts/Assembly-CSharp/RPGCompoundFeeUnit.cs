using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGCompoundFeeUnit
{
	[SerializeField]
	public List<CompoundEle> _cardToCardList = new List<CompoundEle>();

	[SerializeField]
	public List<CompoundEle> _gemToGemList = new List<CompoundEle>();

	[SerializeField]
	public List<CompoundEle> _gemToAvatarList = new List<CompoundEle>();
}
