using System;
using UnityEngine;

[Serializable]
public class RPGGemDefineUnit
{
	public enum EGemColor
	{
		Red = 1,
		Yellow = 2,
		Blue = 3,
		Purple = 4
	}

	[SerializeField]
	private int _gemId;

	[SerializeField]
	private string _gemName;

	public int GemId
	{
		get
		{
			return _gemId;
		}
		set
		{
			_gemId = value;
		}
	}

	public string GemName
	{
		get
		{
			return _gemName;
		}
		set
		{
			_gemName = value;
		}
	}

	public static EGemColor GetGemColorByID(int id)
	{
		return (EGemColor)(id / 100);
	}

	public static int GetGemGradeByID(int id)
	{
		return id % 100;
	}
}
