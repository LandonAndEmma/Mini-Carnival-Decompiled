using System.Collections;
using LitJson;
using UnityEngine;

public class TexServPressManager : COMA_HTTP_TextureManager
{
	private static int timeInterval;

	private static int COUNT;

	private static int count;

	private static int error;

	private static bool bDiliverSvrInit;

	private static bool bTexSvrInit;

	private string playerid = string.Empty;

	private string nickname = string.Empty;

	private void Awake()
	{
		COUNT++;
	}

	private IEnumerator Start()
	{
		for (int i = 0; i < timeInterval; i++)
		{
			yield return new WaitForEndOfFrame();
		}
		timeInterval++;
		WWW www = new WWW(COMA_ServerManager.Instance.serverAddr_Config);
		yield return www;
		string totalContent = www.text.Replace("\r\n", string.Empty);
		string[] content = totalContent.Split('|');
		if (!bDiliverSvrInit)
		{
			bDiliverSvrInit = true;
			COMA_ServerManager.Instance.deliverSvrAddr = content[7];
			InitDeliverServer();
		}
		playerid = "TestID" + Random.Range(0, 100000000).ToString("d8");
		nickname = "TestName" + Random.Range(0, 100000000).ToString("d8");
		DeliverServerUpdate(playerid, nickname);
	}

	public new void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			error++;
			Debug.Log(count + "/" + error + "(" + result + ")/" + COUNT + "  " + action);
		}
		else if (server == serverName_Deliver)
		{
			if (!(action == deliverName_File_Get))
			{
				return;
			}
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (response.Contains("\"existName\":true"))
			{
				Debug.LogError("Name Exist!!");
				return;
			}
			if (!bTexSvrInit)
			{
				bTexSvrInit = true;
				COMA_ServerManager.Instance.saverSvrAddr = jsonData["dataServer"].ToString();
				COMA_ServerManager.Instance.serverAddr_Save = jsonData["fileServer"].ToString();
				Debug.Log(COMA_ServerManager.Instance.saverSvrAddr + " " + COMA_ServerManager.Instance.serverAddr_Save);
				InitTextureServer();
			}
			int num = int.Parse(jsonData["state"].ToString());
			if (num == 1)
			{
				TUI_MsgBox.Instance.MessageBox(103);
			}
			else if ((bool)param)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("uuid", playerid);
				hashtable.Add("TID0", string.Empty);
				hashtable.Add("TID1", string.Empty);
				hashtable.Add("TID2", string.Empty);
				hashtable.Add("Name", nickname);
				hashtable.Add("Lv", 1);
				hashtable.Add("Exp", 0);
				hashtable.Add("Gold", 99);
				hashtable.Add("Crystal", 99);
				string data = JsonMapper.ToJson(hashtable);
				HttpClient.Instance().SendRequest(serverName_File, actionName_File_Set, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
			}
			else
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2.Add("uuid", playerid);
				string data2 = JsonMapper.ToJson(hashtable2);
				HttpClient.Instance().SendRequest(serverName_File, actionName_File_Get, data2, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
			}
		}
		else if (server == serverName_File)
		{
			TextureOfficial_GetTextureFromServer(0);
			TextureSuit_GetTexListFromServer(7, 0);
			TextureSell_GetTexListFromServer(12, 0);
		}
		else if (server == serverName_TextureSell)
		{
			count++;
			Debug.Log(count + "/" + error + "/" + COUNT);
		}
		else if (!(server == serverName_TextureOfficial) && !(server == serverName_TextureSuit))
		{
		}
	}
}
