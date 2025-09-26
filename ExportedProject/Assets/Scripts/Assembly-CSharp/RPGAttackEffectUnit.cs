using System;
using UnityEngine;

[Serializable]
public class RPGAttackEffectUnit
{
	[SerializeField]
	private int _careerId;

	[SerializeField]
	private string _launchEffectPath;

	[SerializeField]
	private float _launchEffectDurTime;

	[SerializeField]
	private string _effectPath;

	[SerializeField]
	private string _launchLoc;

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

	public string LaunchEffectPath
	{
		get
		{
			return _launchEffectPath;
		}
		set
		{
			_launchEffectPath = value;
		}
	}

	public float LaunchEffectDurTime
	{
		get
		{
			return _launchEffectDurTime;
		}
		set
		{
			_launchEffectDurTime = value;
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

	public string LaunchLoc
	{
		get
		{
			return _launchLoc;
		}
		set
		{
			_launchLoc = value;
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
