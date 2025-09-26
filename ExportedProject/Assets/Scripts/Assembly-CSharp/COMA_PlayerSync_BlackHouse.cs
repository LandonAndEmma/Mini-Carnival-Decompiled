public class COMA_PlayerSync_BlackHouse : COMA_PlayerSync
{
	protected new void OnEnable()
	{
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	private new void Start()
	{
		base.Start();
	}

	private new void Update()
	{
		UpdateShadow();
		if (!(COMA_PlayerSelf.Instance == null))
		{
			base.Update();
		}
	}
}
