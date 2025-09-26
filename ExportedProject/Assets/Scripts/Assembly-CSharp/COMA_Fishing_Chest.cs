using UnityEngine;

public class COMA_Fishing_Chest : COMA_Fishing_FishableObj
{
	[SerializeField]
	private int _nGoldNum = -1;

	[SerializeField]
	private int _nCrystal = -1;

	[SerializeField]
	private string _strDecoName = string.Empty;

	public void SetGoldNum(int num)
	{
		_nGoldNum = num;
	}

	public int GetGoldNum()
	{
		return _nGoldNum;
	}

	public void SetCrystalNum(int num)
	{
		_nCrystal = num;
	}

	public int GetCrystalNum()
	{
		return _nCrystal;
	}

	public void SetDecoName(string name)
	{
		_strDecoName = name;
	}

	public string GetDecoName()
	{
		return _strDecoName;
	}

	private void Awake()
	{
		SetEntityType(101);
		Debug.Log("--------" + GetEntityType());
	}

	private new void Start()
	{
		base.Start();
	}

	private new void Update()
	{
	}
}
