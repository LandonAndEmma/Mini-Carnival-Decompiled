using UnityEngine;

public class MiscPlugin
{
	public static string GetMacAddr()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static bool IsIAPCrack()
	{
		return false;
	}

	public static bool IsJailbreak()
	{
		return false;
	}

	public static string GetAppVersion()
	{
		return "2.1.2";
	}
}
