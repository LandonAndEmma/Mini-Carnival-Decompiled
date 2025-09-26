using System;
using UnityEngine;

public class RankInfo : IComparable
{
	public string _strName;

	public float _fNum;

	public Texture2D _tex;

	public string _strId;

	public int CompareTo(object obj)
	{
		RankInfo rankInfo = obj as RankInfo;
		if (rankInfo._fNum == _fNum)
		{
			return 0;
		}
		return (!(_fNum > rankInfo._fNum)) ? 1 : (-1);
	}
}
