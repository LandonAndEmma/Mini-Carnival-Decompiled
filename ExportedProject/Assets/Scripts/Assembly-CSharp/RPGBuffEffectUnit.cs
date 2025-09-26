using System;
using UnityEngine;

[Serializable]
public class RPGBuffEffectUnit
{
	[SerializeField]
	private int _buffId;

	[SerializeField]
	private string _effectPath_cast;

	[SerializeField]
	private string _effectPath_receive;

	[SerializeField]
	private string _effectPath;

	[SerializeField]
	private float _effectDurTime;

	public int BuffId
	{
		get
		{
			return _buffId;
		}
		set
		{
			_buffId = value;
		}
	}

	public string EffectPath_cast
	{
		get
		{
			return _effectPath_cast;
		}
		set
		{
			_effectPath_cast = value;
		}
	}

	public string EffectPath_receive
	{
		get
		{
			return _effectPath_receive;
		}
		set
		{
			_effectPath_receive = value;
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
