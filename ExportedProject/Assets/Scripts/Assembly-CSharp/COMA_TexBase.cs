using BaseCoder;

public class COMA_TexBase
{
	private static COMA_TexBase _instance;

	public int width = 64;

	public int height = 64;

	public int texCountToSell = 100;

	public static COMA_TexBase Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_TexBase();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public string TextureBytesToString(byte[] bytes)
	{
		return new string(Base64.Encode(bytes));
	}

	public byte[] StringToTextureBytes(string content)
	{
		return Base64.Decode(content.ToCharArray());
	}
}
