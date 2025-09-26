using UnityEngine;

public class COMA_ServerManager
{
	private static COMA_ServerManager _instance;

	private bool isLocalNet;

	public string serverAddr_IAP = string.Empty;

	public string serverName_IAP = "COM_IAP";

	public string serverKey_IAP = string.Empty;

	public float serverTimeout_IAP = 120f;

	public string serverAddr_Game = string.Empty;

	public float idSvrOutTime = 20f;

	public string idSvrAddr = string.Empty;

	public string idSvrKey = "dkeoi45793de2k56";

	public float deliverSvrOutTime = 20f;

	public string deliverSvrAddr = string.Empty;

	public string deliverSvrKey = "abcd@@##980[]L>.";

	public float saverSvrOutTime = 20f;

	public string saverSvrAddr = string.Empty;

	public string saverSvrKey = "abcd@@##980[]L>.";

	public float serverAddr_Save_OutTime = 20f;

	public string serverAddr_Save = string.Empty;

	public string serverAddr_Save_Key = string.Empty;

	public float dataCollectSvrOutTime = 20f;

	public string dataCollectSvrAddr = string.Empty;

	public string dataCollectSvrKey = string.Empty;

	public static COMA_ServerManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_ServerManager();
			}
			return _instance;
		}
	}

	public string serverAddr_Config
	{
		get
		{
			if (isLocalNet)
			{
				return "http://account.trinitigame.com/game/COMA/cm.txt?q=" + Random.Range(0, 100000000);
			}
			return "http://account.trinitigame.com/game/COMA/cmo_2_1_2.txt?q=" + Random.Range(0, 100000000);
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
