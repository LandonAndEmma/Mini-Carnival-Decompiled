using System.Collections;
using UnityEngine;

public class COMA_FileNameManager
{
	private static COMA_FileNameManager _instance;

	private Hashtable nameTable = new Hashtable();

	public static COMA_FileNameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_FileNameManager();
				_instance.nameTable.Add("Pref", "0186518486");
				_instance.nameTable.Add("Pack", "0256823914");
				_instance.nameTable.Add("Tex", "0398456123");
				_instance.nameTable.Add("PaintColor", "0448956123");
				_instance.nameTable.Add("AudioControl", "0561235489");
				_instance.nameTable.Add("GCTID", "0894156131");
				_instance.nameTable.Add("Board", "0348679642");
				_instance.nameTable.Add("FishLocal", "0256526574");
				_instance.nameTable.Add("PCVID", "PC_VID");
				_instance.nameTable.Add("LocalTID", "0883214453");
				_instance.nameTable.Add("TransferState", "Transfer/0123234583");
				_instance.nameTable.Add("SaveTransfer", "Transfer/0465897544");
				_instance.nameTable.Add("FriendTransfer", "Transfer/0635467864");
				_instance.nameTable.Add("TexTransfer", "Transfer/0564987423");
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public string GetFileName(string key)
	{
		string empty = string.Empty;
		if (key.StartsWith("Head"))
		{
			return "1" + Random.Range(0, 10000000).ToString("d7") + key.Substring(4);
		}
		if (key.StartsWith("Body"))
		{
			return "2" + Random.Range(0, 10000000).ToString("d7") + key.Substring(4);
		}
		if (key.StartsWith("Leg"))
		{
			return "3" + Random.Range(0, 10000000).ToString("d7") + key.Substring(3);
		}
		return (string)nameTable[key];
	}
}
