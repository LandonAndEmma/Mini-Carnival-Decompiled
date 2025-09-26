using UnityEngine;

public class COMA_Fishing_Fish : COMA_Fishing_FishableObj
{
	[SerializeField]
	private int _nWeight;

	[SerializeField]
	protected float _fBigRate = 0.75f;

	public void SetWeight(int w)
	{
		_nWeight = w;
	}

	public int GetWeight()
	{
		return _nWeight;
	}

	public bool IsBigFish()
	{
		if ((float)_nWeight >= Mathf.Lerp(COMA_Fishing_FishPool.Instance.GetFishParam(GetCustomID())._nMinWeight, COMA_Fishing_FishPool.Instance.GetFishParam(GetCustomID())._nMaxWeight, _fBigRate))
		{
			return true;
		}
		return false;
	}

	private void Awake()
	{
		SetEntityType(100);
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
