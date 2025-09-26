using System.Collections.Generic;

public class COMA_TexBuyBuffer
{
	private static COMA_TexBuyBuffer _instance;

	public List<COMA_ItemInTradeHall> items = new List<COMA_ItemInTradeHall>();

	public static COMA_TexBuyBuffer Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_TexBuyBuffer();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
