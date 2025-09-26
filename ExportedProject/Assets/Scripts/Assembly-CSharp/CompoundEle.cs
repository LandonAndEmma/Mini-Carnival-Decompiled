using System;
using UnityEngine;

[Serializable]
public class CompoundEle
{
	[SerializeField]
	public enum ECurrency
	{
		Gold = 0,
		Crystal = 1
	}

	[SerializeField]
	public int _grade;

	[SerializeField]
	public ECurrency _currency;

	[SerializeField]
	public int _fee;
}
