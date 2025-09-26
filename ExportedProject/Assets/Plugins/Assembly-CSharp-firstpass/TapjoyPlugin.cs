using System;
using System.Collections.Generic;
using UnityEngine;

public class TapjoyPlugin : MonoBehaviour
{
	public const string MacAddressOptionOn = "0";

	public const string MacAddressOptionOffWithVersionCheck = "1";

	public const string MacAddressOptionOff = "2";

	private static Dictionary<string, TapjoyEvent> eventDictionary = new Dictionary<string, TapjoyEvent>();

	public static event Action connectCallSucceeded;

	public static event Action connectCallFailed;

	public static event Action<int> getTapPointsSucceeded;

	public static event Action getTapPointsFailed;

	public static event Action<int> spendTapPointsSucceeded;

	public static event Action spendTapPointsFailed;

	public static event Action awardTapPointsSucceeded;

	public static event Action awardTapPointsFailed;

	public static event Action<int> tapPointsEarned;

	public static event Action getFullScreenAdSucceeded;

	public static event Action getFullScreenAdFailed;

	public static event Action getDailyRewardAdSucceeded;

	public static event Action getDailyRewardAdFailed;

	public static event Action getDisplayAdSucceeded;

	public static event Action getDisplayAdFailed;

	public static event Action videoAdStarted;

	public static event Action videoAdFailed;

	public static event Action videoAdCompleted;

	public static event Action showOffersFailed;

	public static event Action<TapjoyViewType> viewOpened;

	public static event Action<TapjoyViewType> viewClosed;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		SetCallbackHandler(base.gameObject.name);
		Debug.Log("C#: UnitySendMessage directs to " + base.gameObject.name);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void TapjoyConnectSuccess(string message)
	{
		if (TapjoyPlugin.connectCallSucceeded != null)
		{
			TapjoyPlugin.connectCallSucceeded();
		}
	}

	public void TapjoyConnectFail(string message)
	{
		if (TapjoyPlugin.connectCallFailed != null)
		{
			TapjoyPlugin.connectCallFailed();
		}
	}

	public void TapPointsLoaded(string message)
	{
		if (TapjoyPlugin.getTapPointsSucceeded != null)
		{
			TapjoyPlugin.getTapPointsSucceeded(int.Parse(message));
		}
	}

	public void TapPointsLoadedError(string message)
	{
		if (TapjoyPlugin.getTapPointsFailed != null)
		{
			TapjoyPlugin.getTapPointsFailed();
		}
	}

	public void TapPointsSpent(string message)
	{
		Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++TapPointsSpent");
		if (TapjoyPlugin.spendTapPointsSucceeded != null)
		{
			TapjoyPlugin.spendTapPointsSucceeded(int.Parse(message));
		}
	}

	public void TapPointsSpendError(string message)
	{
		if (TapjoyPlugin.spendTapPointsFailed != null)
		{
			TapjoyPlugin.spendTapPointsFailed();
		}
	}

	public void TapPointsAwarded(string message)
	{
		if (TapjoyPlugin.awardTapPointsSucceeded != null)
		{
			TapjoyPlugin.awardTapPointsSucceeded();
		}
	}

	public void TapPointsAwardError(string message)
	{
		if (TapjoyPlugin.awardTapPointsFailed != null)
		{
			TapjoyPlugin.awardTapPointsFailed();
		}
	}

	public void CurrencyEarned(string message)
	{
		if (TapjoyPlugin.tapPointsEarned != null)
		{
			TapjoyPlugin.tapPointsEarned(int.Parse(message));
		}
	}

	public void FullScreenAdLoaded(string message)
	{
		if (TapjoyPlugin.getFullScreenAdSucceeded != null)
		{
			TapjoyPlugin.getFullScreenAdSucceeded();
		}
	}

	public void FullScreenAdError(string message)
	{
		if (TapjoyPlugin.getFullScreenAdFailed != null)
		{
			TapjoyPlugin.getFullScreenAdFailed();
		}
	}

	public void DailyRewardAdLoaded(string message)
	{
		if (TapjoyPlugin.getDailyRewardAdSucceeded != null)
		{
			TapjoyPlugin.getDailyRewardAdSucceeded();
		}
	}

	public void DailyRewardAdError(string message)
	{
		if (TapjoyPlugin.getDailyRewardAdFailed != null)
		{
			TapjoyPlugin.getDailyRewardAdFailed();
		}
	}

	public void DisplayAdLoaded(string message)
	{
		if (TapjoyPlugin.getDisplayAdSucceeded != null)
		{
			TapjoyPlugin.getDisplayAdSucceeded();
		}
	}

	public void DisplayAdError(string message)
	{
		if (TapjoyPlugin.getDisplayAdFailed != null)
		{
			TapjoyPlugin.getDisplayAdFailed();
		}
	}

	public void VideoAdStart(string message)
	{
		if (TapjoyPlugin.videoAdStarted != null)
		{
			TapjoyPlugin.videoAdStarted();
		}
	}

	public void VideoAdError(string message)
	{
		if (TapjoyPlugin.videoAdFailed != null)
		{
			TapjoyPlugin.videoAdFailed();
		}
	}

	public void VideoAdComplete(string message)
	{
		if (TapjoyPlugin.videoAdCompleted != null)
		{
			TapjoyPlugin.videoAdCompleted();
		}
	}

	public void ShowOffersError(string message)
	{
		if (TapjoyPlugin.showOffersFailed != null)
		{
			TapjoyPlugin.showOffersFailed();
		}
	}

	public void ViewOpened(string message)
	{
		if (TapjoyPlugin.viewOpened != null)
		{
			int obj = int.Parse(message);
			TapjoyPlugin.viewOpened((TapjoyViewType)obj);
		}
	}

	public void ViewClosed(string message)
	{
		if (TapjoyPlugin.viewClosed != null)
		{
			int obj = int.Parse(message);
			TapjoyPlugin.viewClosed((TapjoyViewType)obj);
		}
	}

	public static void SetCallbackHandler(string handlerName)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetCallbackHandler(handlerName);
		}
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey);
		}
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, string> flags)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey, flags);
		}
	}

	public static void EnableLogging(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableLogging(enable);
		}
	}

	public static void ActionComplete(string actionID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ActionComplete(actionID);
		}
	}

	public static void SetUserID(string userID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetUserID(userID);
		}
	}

	public static void ShowOffers()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowOffers();
		}
	}

	public static void GetTapPoints()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetTapPoints();
		}
	}

	public static void SpendTapPoints(int points)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SpendTapPoints(points);
		}
	}

	public static void AwardTapPoints(int points)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.AwardTapPoints(points);
		}
	}

	public static int QueryTapPoints()
	{
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			return 0;
		}
		return TapjoyPluginAndroid.QueryTapPoints();
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDefaultEarnedCurrencyAlert();
		}
	}

	public static void GetDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAd();
		}
	}

	public static void ShowDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDisplayAd();
		}
	}

	public static void HideDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.HideDisplayAd();
		}
	}

	[Obsolete("SetDisplayAdContentSize is deprecated. Please use SetDisplayAdSize.")]
	public static void SetDisplayAdContentSize(int size)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			SetDisplayAdSize((TapjoyDisplayAdSize)size);
		}
	}

	public static void SetDisplayAdSize(TapjoyDisplayAdSize size)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			string displayAdSize = "320x50";
			if (size == TapjoyDisplayAdSize.SIZE_640X100)
			{
				displayAdSize = "640x100";
			}
			if (size == TapjoyDisplayAdSize.SIZE_768X90)
			{
				displayAdSize = "768x90";
			}
			TapjoyPluginAndroid.SetDisplayAdSize(displayAdSize);
		}
	}

	[Obsolete("RefreshDisplayAd(bool enable) is deprecated. Please use EnableDisplayAdAutoRefresh.")]
	public static void RefreshDisplayAd(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableDisplayAdAutoRefresh(enable);
		}
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableDisplayAdAutoRefresh(enable);
		}
	}

	[Obsolete("RefreshDisplayAd() is deprecated. Please use GetDisplayAd.")]
	public static void RefreshDisplayAd()
	{
		TapjoyPluginAndroid.GetDisplayAd();
	}

	public static void MoveDisplayAd(int x, int y)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.MoveDisplayAd(x, y);
		}
	}

	[Obsolete("SetUserDefinedColorWithIntValue is deprecated. There is no navigation bar for iOS in v9.x.x+.")]
	public static void SetUserDefinedColorWithIntValue(int color)
	{
	}

	public static void SetTransitionEffect(int transition)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetTransitionEffect(transition);
		}
	}

	[Obsolete("GetFeaturedApp is deprecated, please use GetFullScreenAd instead.")]
	public static void GetFeaturedApp()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAd();
		}
	}

	[Obsolete("SetFeaturedAppDisplayCount is deprecated.")]
	public static void SetFeaturedAppDisplayCount(int displayCount)
	{
	}

	[Obsolete("ShowFeaturedAppFullScreenAd is deprecated, please use ShowFullScreenAd instead.")]
	public static void ShowFeaturedAppFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowFullScreenAd();
		}
	}

	public static void GetFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAd();
		}
	}

	public static void ShowFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowFullScreenAd();
		}
	}

	[Obsolete("GetReEngagementAd is deprecated.")]
	public static void GetReEngagementAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAd();
		}
	}

	[Obsolete("ShowReEngagementAd is deprecated.")]
	public static void ShowReEngagementAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDailyRewardAd();
		}
	}

	[Obsolete("GetDailyRewardAd is deprecated.")]
	public static void GetDailyRewardAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAd();
		}
	}

	[Obsolete("ShowDailyRewardAd is deprecated.")]
	public static void ShowDailyRewardAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDailyRewardAd();
		}
	}

	[Obsolete("InitVideoAd is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void InitVideoAd()
	{
	}

	[Obsolete("SetVideoCacheCount is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void SetVideoCacheCount(int cacheCount)
	{
	}

	[Obsolete("EnableVideoCache is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void EnableVideoCache(bool enable)
	{
	}

	public static void SendShutDownEvent()
	{
		TapjoyPluginAndroid.SendShutDownEvent();
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SendIAPEvent(name, price, quantity, currencyCode);
		}
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowOffersWithCurrencyID(currencyID, selector);
		}
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAdWithCurrencyID(currencyID);
		}
	}

	[Obsolete("RefreshDisplayAdWithCurrencyID is deprecated, please use GetDisplayAdWithCurrencyID instead.")]
	public static void RefreshDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAdWithCurrencyID(currencyID);
		}
	}

	[Obsolete("GetFeaturedAppWithCurrencyID is deprecated, please use GetFullScreenAdWithCurrencyID instead.")]
	public static void GetFeaturedAppWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAdWithCurrencyID(currencyID);
		}
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAdWithCurrencyID(currencyID);
		}
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetCurrencyMultiplier(multiplier);
		}
	}

	[Obsolete("GetReEngagementAdWithCurrencyID is deprecated, please use GetDailyRewardAdWithCurrencyID instead.")]
	public static void GetReEngagementAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAdWithCurrencyID(currencyID);
		}
	}

	public static void GetDailyRewardAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAdWithCurrencyID(currencyID);
		}
	}

	public static void ResetEventDict()
	{
		eventDictionary = new Dictionary<string, TapjoyEvent>();
	}

	public void SendEventCompleteWithContent(string guid)
	{
		if (eventDictionary != null && eventDictionary.ContainsKey(guid))
		{
			eventDictionary[guid].TriggerSendEventSucceeded(true);
		}
	}

	public void SendEventComplete(string guid)
	{
		if (eventDictionary != null && eventDictionary.ContainsKey(guid))
		{
			eventDictionary[guid].TriggerSendEventSucceeded(false);
		}
	}

	public void SendEventFail(string guid)
	{
		if (eventDictionary != null && eventDictionary.ContainsKey(guid))
		{
			eventDictionary[guid].TriggerSendEventFailed(null);
		}
	}

	public void ContentWillAppear(string guid)
	{
	}

	public void ContentDidAppear(string guid)
	{
	}

	public void ContentWillDisappear(string guid)
	{
	}

	public void ContentDidDisappear(string guid)
	{
	}

	public void DidRequestAction(string guid)
	{
	}

	public static string CreateEvent(TapjoyEvent eventRef, string eventName, string eventParameter)
	{
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			return null;
		}
		string text = Guid.NewGuid().ToString();
		while (eventDictionary.ContainsKey(text))
		{
			text = Guid.NewGuid().ToString();
		}
		eventDictionary.Add(text, eventRef);
		TapjoyPluginAndroid.CreateEvent(text, eventName, eventParameter);
		return text;
	}

	public static void SendEvent(string guid)
	{
		TapjoyPluginAndroid.SendEvent(guid);
	}

	public static void ShowEvent(string guid)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowEvent(guid);
		}
	}
}
