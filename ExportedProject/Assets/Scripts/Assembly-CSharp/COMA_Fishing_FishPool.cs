using UnityEngine;

public class COMA_Fishing_FishPool : TBaseEntity
{
	private static COMA_Fishing_FishPool _instance;

	public static COMA_Fishing_FishPool Instance
	{
		get
		{
			return _instance;
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	private void Awake()
	{
		_resData = Resources.Load("ResData/Fishing/FishPoolData") as COMA_Fishing_FishPoolData;
	}

	public Fish_Param GetFishParam(int customID)
	{
		if (customID < 1 || customID >= ((COMA_Fishing_FishPoolData)_resData)._fishPool.GetLength(0))
		{
			return null;
		}
		return ((COMA_Fishing_FishPoolData)_resData)._fishPool[customID];
	}
}
