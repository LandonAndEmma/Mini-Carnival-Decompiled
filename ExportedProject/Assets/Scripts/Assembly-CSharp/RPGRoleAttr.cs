using System;
using UnityEngine;

[Serializable]
public class RPGRoleAttr
{
	public enum ERoleAttr
	{
		STR = 0,
		DEX = 1,
		INT = 2,
		HP = 3,
		ATT = 4,
		DEF = 5,
		DodgeRate = 6,
		ATIndex = 7,
		CriticalRate = 8,
		CriticalMultiplier = 9,
		OffsetCriticalDama = 10,
		OffsetNormalDama = 11,
		ATTCountPerBout = 12,
		ATTIndex = 13,
		BAIndex = 14,
		ITDRate = 15,
		FrozenResistRate = 16,
		StunResistRate = 17,
		ChaosResistRate = 18,
		FrozenRate = 19,
		StunRate = 20,
		ChaosRate = 21,
		ResistDebuff = 22,
		ITDValue = 23,
		Max = 24
	}

	[SerializeField]
	private float[] _Attrs = new float[24];

	public float[] Attrs
	{
		get
		{
			return _Attrs;
		}
		set
		{
			_Attrs = value;
		}
	}

	public RPGRoleAttr()
	{
		ResetZero();
	}

	public RPGRoleAttr(RPGRoleAttr attr)
	{
		for (int i = 0; i < 24; i++)
		{
			_Attrs[i] = attr.Attrs[i];
		}
	}

	public void ResetZero()
	{
		for (int i = 0; i < 24; i++)
		{
			_Attrs[i] = 0f;
		}
	}

	public static RPGRoleAttr operator +(RPGRoleAttr a, RPGRoleAttr b)
	{
		RPGRoleAttr rPGRoleAttr = new RPGRoleAttr();
		for (int i = 0; i < 24; i++)
		{
			rPGRoleAttr.Attrs[i] = a.Attrs[i] + b.Attrs[i];
		}
		return rPGRoleAttr;
	}

	public static RPGRoleAttr operator -(RPGRoleAttr a, RPGRoleAttr b)
	{
		RPGRoleAttr rPGRoleAttr = new RPGRoleAttr();
		for (int i = 0; i < 24; i++)
		{
			rPGRoleAttr.Attrs[i] = a.Attrs[i] - b.Attrs[i];
		}
		return rPGRoleAttr;
	}
}
