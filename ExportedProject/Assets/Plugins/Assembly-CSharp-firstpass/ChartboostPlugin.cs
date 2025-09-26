public class ChartboostPlugin
{
	private static int m_show_number;

	public static void ResetShowNumber()
	{
		m_show_number = 0;
	}

	public static void StartSession(string appId, string appSignature)
	{
	}

	public static void ShowInterstitial()
	{
		if (m_show_number <= 0)
		{
			m_show_number++;
		}
	}

	public static void CacheInterstitial()
	{
	}
}
