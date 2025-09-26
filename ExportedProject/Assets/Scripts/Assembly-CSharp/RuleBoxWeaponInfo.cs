using System;
using UnityEngine;

[Serializable]
public class RuleBoxWeaponInfo
{
	[SerializeField]
	private string[] _strTex;

	[SerializeField]
	private int[] _nMoney;

	[SerializeField]
	private Texture2D[] _texs;

	[SerializeField]
	private string[] _strPropDes;

	public void SetPropDes(string[] des)
	{
		_strPropDes = des;
	}

	public string GetPropDes(int index)
	{
		return _strPropDes[index];
	}

	public void SetMoney(int[] money)
	{
		_nMoney = money;
	}

	public void SetMoney(int n, int money)
	{
		_nMoney[n] = money;
	}

	public void SetTex(string[] tex)
	{
		_strTex = tex;
	}

	public void SetTex(int n, string tex)
	{
		_strTex[n] = tex;
	}

	public void SetTex(Texture2D[] tex)
	{
		_texs = tex;
	}

	public void SetTex(int n, Texture2D tex)
	{
		_texs[n] = tex;
	}

	public void SetTex(int n, Texture tex)
	{
		_texs[n] = (Texture2D)tex;
	}

	public int GetMoney(int n)
	{
		return _nMoney[n];
	}

	public Texture2D GetTex(int n)
	{
		return _texs[n];
	}

	public string GetTexStr(int n)
	{
		return _strTex[n];
	}
}
