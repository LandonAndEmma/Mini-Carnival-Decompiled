using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class UI_BoxData : ISerializable
{
	protected bool _bCanSell;

	protected UI_BoxSlot ower;

	protected int _nSortNum;

	public bool CanSell
	{
		get
		{
			return _bCanSell;
		}
		set
		{
			_bCanSell = value;
		}
	}

	public UI_BoxSlot Ower
	{
		get
		{
			return ower;
		}
		set
		{
			ower = value;
		}
	}

	public int SortNum
	{
		get
		{
			return _nSortNum;
		}
		set
		{
			_nSortNum = value;
		}
	}

	private UI_BoxData(SerializationInfo info, StreamingContext ctxt)
	{
	}

	public UI_BoxData()
	{
	}

	public UI_BoxData(UI_BoxData data)
	{
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
	}

	public virtual int SerializeBoxData(TextAsset xml)
	{
		return 0;
	}

	public virtual int SerializeBoxData(string str)
	{
		return 0;
	}

	protected virtual int DataChanged()
	{
		if (Ower != null)
		{
			Ower.NotifyDataUpdate();
		}
		return 0;
	}
}
