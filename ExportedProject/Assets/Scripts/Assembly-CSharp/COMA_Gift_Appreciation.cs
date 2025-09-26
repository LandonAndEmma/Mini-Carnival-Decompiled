public class COMA_Gift_Appreciation : COMA_ResetInstance
{
	private static COMA_Gift_Appreciation _instance;

	private int _count;

	public static COMA_Gift_Appreciation Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Gift_Appreciation();
				COMA_InstanceManager.Instance.RegistResetInstance(_instance);
			}
			return _instance;
		}
	}

	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
		}
	}

	public override void ResetInstance()
	{
		_instance = null;
	}
}
