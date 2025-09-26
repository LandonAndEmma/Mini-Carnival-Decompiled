public class COMA_TimeAtlas
{
	private static COMA_TimeAtlas _instance;

	public float flagGetting = 3.5f;

	public float flagRefresh = 15f;

	public static COMA_TimeAtlas Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_TimeAtlas();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
