using System;
using UnityEngine;

[Serializable]
public class RPGBeAttackEffectUnit
{
	[SerializeField]
	private int _careerId;

	[SerializeField]
	private string _effectPath;

	[SerializeField]
	private float _effectDurTime;

	public int CareerId
	{
		get
		{
			return _careerId;
		}
		set
		{
			_careerId = value;
		}
	}

	public string EffectPath
	{
		get
		{
			return _effectPath;
		}
		set
		{
			_effectPath = value;
		}
	}

	public float EffectDurTime
	{
		get
		{
			return _effectDurTime;
		}
		set
		{
			_effectDurTime = value;
		}
	}
}
