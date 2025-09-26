using UnityEngine;

public class COMA_Fishing_FishPole_Silver : COMA_Fishing_FishPole
{
	private new void Awake()
	{
		base.Awake();
		_resData = Resources.Load("ResData/Fishing/SilverFishPoleData") as COMA_Fishing_FishPoleData;
		SetEntityType(1);
	}

	private new void Start()
	{
		base.Start();
	}

	private new void Update()
	{
		base.Update();
	}

	public override COMA_Fishing_FishableObj GeneratorFishingObj()
	{
		return base.GeneratorFishingObj();
	}
}
