using UnityEngine;

public class COMA_Fishing_FishPole_Black : COMA_Fishing_FishPole
{
	private new void Awake()
	{
		base.Awake();
		_resData = Resources.Load("ResData/Fishing/BlackFishPoleData") as COMA_Fishing_FishPoleData;
		SetEntityType(0);
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
