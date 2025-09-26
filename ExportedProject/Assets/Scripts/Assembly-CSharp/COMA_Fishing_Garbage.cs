using UnityEngine;

public class COMA_Fishing_Garbage : COMA_Fishing_FishableObj
{
	private void Awake()
	{
		SetEntityType(102);
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
