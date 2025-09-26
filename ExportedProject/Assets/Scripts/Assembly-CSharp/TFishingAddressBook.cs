public class TFishingAddressBook : TBaseAddressBook
{
	public enum EEntityName
	{
		MainActor_bob = 0,
		UI_Fishing = 1
	}

	private static TFishingAddressBook _instance = null;

	private static readonly object _lock = new object();

	public static TFishingAddressBook Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new TFishingAddressBook();
					}
				}
			}
			return _instance;
		}
	}

	private TFishingAddressBook()
	{
	}

	public void ResetInstance()
	{
		_instance = null;
	}

	private void Start()
	{
	}
}
