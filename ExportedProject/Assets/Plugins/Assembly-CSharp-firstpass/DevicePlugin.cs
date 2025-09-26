using UnityEngine;

public class DevicePlugin
{
	public static AndroidJavaClass androidplatform;

	public static string openclik_url = "http://192.225.224.137/openclik/openclikAction.do";

	private static string openclik_appid = "3B4A7AFC-FB0A-483A-81E7-5968D62D1A35";

	public static void InitAndroidPlatform()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			androidplatform = new AndroidJavaClass("com.trinitigame.androidplatform.AndroidPlatformActivity");
		}
	}

	public string GetDeviceModel()
	{
		return string.Empty;
	}

	public string GetDeviceModelDetail()
	{
		return androidplatform.CallStatic<string>("GetAndroidVersion", new object[0]);
	}

	public static void AndroidQuit()
	{
		androidplatform.CallStatic("AndroidQuit");
	}

	public static void OpenclikShow(string url, string appid)
	{
		androidplatform.CallStatic("ShowAd", url, appid);
	}

	public static void HideAD()
	{
		androidplatform.CallStatic("HideTrinitiAd_t");
	}

	public string GetUUID()
	{
		return MiscPlugin.GetMacAddr() + "_UUID";
	}

	public string GetCountryCode()
	{
		return androidplatform.CallStatic<string>("GetCountry", new object[0]);
	}

	public string GetLanguageCode()
	{
		return Application.systemLanguage.ToString();
	}

	public string GetSysVersion()
	{
		return androidplatform.CallStatic<string>("GetAndroidVersion", new object[0]);
	}

	public string GetAppVersion()
	{
		return string.Empty;
	}

	public string GetAppBundleId()
	{
		return string.Empty;
	}
}
