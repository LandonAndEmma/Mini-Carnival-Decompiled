using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGTacticUnit
{
	[SerializeField]
	private int _tacticId;

	[SerializeField]
	private string _tacticDes;

	[SerializeField]
	private bool _bOverlap;

	[SerializeField]
	private List<object> _paramLst = new List<object>();

	public int TacticId
	{
		get
		{
			return _tacticId;
		}
		set
		{
			_tacticId = value;
		}
	}

	public string TacticDes
	{
		get
		{
			return _tacticDes;
		}
		set
		{
			_tacticDes = value;
		}
	}

	public bool CanOverlap
	{
		get
		{
			return _bOverlap;
		}
		set
		{
			_bOverlap = value;
		}
	}

	public List<object> ParamLst
	{
		get
		{
			return _paramLst;
		}
		set
		{
			_paramLst = value;
		}
	}

	public RPGTacticUnit()
	{
		_tacticId = 1;
		_tacticDes = "test";
		_bOverlap = false;
	}
}
