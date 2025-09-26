using System;
using UnityEngine;

[Serializable]
public class RPGGemShopUnit
{
	[SerializeField]
	public int _itemId;

	[SerializeField]
	public int _gemId;

	[SerializeField]
	public int _totalPrice;

	[SerializeField]
	public int _currencyType;

	[SerializeField]
	public int _purchaseNum;
}
