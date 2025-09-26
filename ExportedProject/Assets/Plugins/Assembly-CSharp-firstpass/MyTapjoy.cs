using UnityEngine;

public class MyTapjoy : MonoBehaviour
{
	private const string methodName = "GotTapPoints";

	private static MyTapjoy sInstance;

	private string androidAppId = "d4227c6c-3f09-42b0-91c7-fc9554072abb";

	private string androidSecretKey = "ccDzn90bPWpJahoc4Ncn";

	public string iphoneAppId = "b952a592-777e-42a3-a6ef-39e9912411be";

	public string iphoneSecretKey = "WhAcbx2dBoJdUyjvxuEN";

	private int tapPoints;

	public static MyTapjoy Instance()
	{
		if (sInstance == null)
		{
			sInstance = new GameObject("_MyTapjoy").AddComponent<MyTapjoy>();
		}
		return sInstance;
	}

	public static void Show()
	{
		TapjoyPlugin.ShowOffers();
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.AttachCurrentThread();
		}
		TapjoyPlugin.EnableLogging(false);
		TapjoyPlugin.SetCallbackHandler(base.gameObject.name);
		if (Application.platform == RuntimePlatform.Android)
		{
			TapjoyPlugin.RequestTapjoyConnect(androidAppId, androidSecretKey);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TapjoyPlugin.RequestTapjoyConnect(iphoneAppId, iphoneSecretKey);
		}
		TapjoyPlugin.GetTapPoints();
	}

	private void OnEnable()
	{
		TapjoyPlugin.getTapPointsSucceeded += TapPointsLoaded;
		TapjoyPlugin.spendTapPointsSucceeded += TapPointsSpent;
		TapjoyPlugin.spendTapPointsFailed += TapPointsSpendError;
		TapjoyPlugin.awardTapPointsSucceeded += TapPointsAwarded;
		TapjoyPlugin.tapPointsEarned += CurrencyEarned;
	}

	private void OnDisable()
	{
		TapjoyPlugin.getTapPointsSucceeded -= TapPointsLoaded;
		TapjoyPlugin.spendTapPointsSucceeded -= TapPointsSpent;
		TapjoyPlugin.spendTapPointsFailed -= TapPointsSpendError;
		TapjoyPlugin.awardTapPointsSucceeded -= TapPointsAwarded;
		TapjoyPlugin.tapPointsEarned -= CurrencyEarned;
	}

	public void TapPointsLoaded(int message)
	{
		MonoBehaviour.print("TapPointsLoaded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsSpent(int message)
	{
		MonoBehaviour.print("TapPointsSpent: " + message);
		SpendSuccessful();
	}

	public void TapPointsSpendError()
	{
		MonoBehaviour.print("TapPointsSpendError");
		tapPoints = 0;
	}

	public void TapPointsAwarded()
	{
		MonoBehaviour.print("TapPointsAwarded");
		UsedAllTapPoints();
	}

	public void CurrencyEarned(int message)
	{
		MonoBehaviour.print("CurrencyEarned: " + message);
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		TapjoyPlugin.GetTapPoints();
	}

	public void TapjoyConnectSuccess(string message)
	{
		MonoBehaviour.print(message);
	}

	public void TapjoyConnectFail(string message)
	{
		MonoBehaviour.print(message);
	}

	public void TapPointsLoaded(string message)
	{
		MonoBehaviour.print("TapPointsLoaded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsLoadedError(string message)
	{
		MonoBehaviour.print("TapPointsLoadedError: " + message);
	}

	public void TapPointsSpent(string message)
	{
		MonoBehaviour.print("TapPointsSpent: " + message);
		SpendSuccessful();
	}

	public void TapPointsSpendError(string message)
	{
		MonoBehaviour.print("TapPointsSpendError: " + message);
		tapPoints = 0;
	}

	public void TapPointsAwarded(string message)
	{
		MonoBehaviour.print("TapPointsAwarded: " + message);
		UsedAllTapPoints();
	}

	public void TapPointsAwardError(string message)
	{
		MonoBehaviour.print("TapPointsAwardError: " + message);
	}

	public void CurrencyEarned(string message)
	{
		MonoBehaviour.print("CurrencyEarned: " + message);
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
		TapjoyPlugin.GetTapPoints();
	}

	public void FullScreenAdLoaded(string message)
	{
		MonoBehaviour.print("FullScreenAdLoaded: " + message);
		TapjoyPlugin.ShowFullScreenAd();
	}

	public void FullScreenAdError(string message)
	{
		MonoBehaviour.print("FullScreenAdError: " + message);
	}

	public void DailyRewardAdLoaded(string message)
	{
		MonoBehaviour.print("DailyRewardAd: " + message);
		TapjoyPlugin.ShowDailyRewardAd();
	}

	public void DailyRewardAdError(string message)
	{
		MonoBehaviour.print("DailyRewardAd: " + message);
	}

	public void DisplayAdLoaded(string message)
	{
		MonoBehaviour.print("DisplayAdLoaded: " + message);
		TapjoyPlugin.ShowDisplayAd();
	}

	public void DisplayAdError(string message)
	{
		MonoBehaviour.print("DisplayAdError: " + message);
	}

	public void VideoAdStart(string message)
	{
		MonoBehaviour.print("VideoAdStart: " + message);
	}

	public void VideoAdError(string message)
	{
		MonoBehaviour.print("VideoAdError: " + message);
	}

	public void VideoAdComplete(string message)
	{
		MonoBehaviour.print("VideoAdComplete: " + message);
	}

	private void UsedAllTapPoints()
	{
		tapPoints = TapjoyPlugin.QueryTapPoints();
		if (tapPoints > 0)
		{
			TapjoyPlugin.SpendTapPoints(tapPoints);
		}
	}

	private void SpendSuccessful()
	{
		Debug.Log("SpendSuccessful");
		if (tapPoints > 0)
		{
			SendMessage("GotTapPoints", tapPoints);
		}
	}
}
