using System.Collections;
using System.Xml;
using UnityEngine;

public class COMA_Sys
{
	private static COMA_Sys _instance;

	public bool DESIGNER;

	public string VID = string.Empty;

	public string version = "0.0.0";

	public bool bMemFirstGame;

	public bool bFirstGame;

	public int playTimes;

	public bool bFirstEdit;

	public int nTeachingId;

	public bool bNeedInitPref;

	public string tipContent = string.Empty;

	public bool bCanGame;

	public string fishLocalInfo = string.Empty;

	public bool bLockMarket;

	public float marketRefreshTime;

	public float marketRefreshTime_ADRandom;

	public int marketRefreshInterval = 10;

	public int marketRefreshCrystal = 10;

	public float curMarketRefreshInterval = 10f;

	public bool bRateActive;

	public DevicePlugin PlayerDevice = new DevicePlugin();

	public Hashtable playerIDToSeatIndex = new Hashtable();

	public int suitIndex;

	public int bodyIndex;

	public float apToOneShot = 1000000f;

	public bool bCoverUpdate;

	public bool bCoverUIInput;

	public bool bGameCounting;

	public bool bIgnoreGameCount;

	public bool bRealStartGame;

	public string roadIDs = string.Empty;

	public int tax = 2;

	public int modelSuitSaleRate = 80;

	public float sensitivity = 1f;

	public static COMA_Sys Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Sys();
			}
			return _instance;
		}
	}

	public bool IsSuperAccount
	{
		get
		{
			return false;
		}
	}

	public bool IsOfficialEditor
	{
		get
		{
			return false;
		}
	}

	public bool IsCleaner
	{
		get
		{
			return false;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public string GetDeviceName()
	{
		string empty = string.Empty;
		return Instance.VID;
	}

	public XmlNode ParseXml(TextAsset ta)
	{
		if (ta == null)
		{
			Debug.LogError("ta is Null!!");
		}
		XmlDocument xmlDocument = new XmlDocument();
		if (DESIGNER)
		{
			string xml = COMA_FileIO.ReadTextDirectly("Data/" + ta.name + ".xml");
			xmlDocument.LoadXml(xml);
		}
		else
		{
			xmlDocument.LoadXml(ta.text);
		}
		return xmlDocument.DocumentElement;
	}

	public string NumberToString_SeparationByColon(int number)
	{
		return NumberToString_SeparationByColon(number, 3);
	}

	public string NumberToString_SeparationByColon(int number, int sections)
	{
		string text = string.Empty;
		if (sections >= 1)
		{
			text = (number % 60).ToString("d2");
			number /= 60;
		}
		if (sections >= 2)
		{
			text = (number % 60).ToString("d2") + ":" + text;
			number /= 60;
		}
		if (sections >= 3)
		{
			text = number.ToString("d2") + ":" + text;
		}
		return text;
	}
}
