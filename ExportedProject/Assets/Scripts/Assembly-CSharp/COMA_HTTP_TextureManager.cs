using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class COMA_HTTP_TextureManager : MonoBehaviour
{
	private static COMA_HTTP_TextureManager _instance;

	private Dictionary<int, OnHttpResponse> responseMap = new Dictionary<int, OnHttpResponse>();

	protected string serverName_Deliver = "svr_deliver";

	protected string serverName_File = "svr_pref";

	protected string serverName_TextureBase = "svr_baseTexture";

	protected string serverName_TextureSell = "svr_sellTexture";

	protected string serverName_TextureOfficial = "svr_TriTexture";

	protected string serverName_TextureSuit = "svr_SuitTexture";

	protected string deliverName_File_Get = "comavataraccount/GetAccount";

	protected string deliverName_File_SetName = "comavataraccount/SetName";

	protected string actionName_File_Set = "Callofminiavatar/SetBase";

	protected string actionName_File_Get = "Callofminiavatar/GetBase";

	protected string actionName_File_Update = "Callofminiavatar/UpdateBase";

	protected string actionName_Tex_Set = "Callofminiavatar/SetTexture";

	protected string actionName_Tex_Get = "Callofminiavatar/GetTexture";

	protected string actionName_Tex_Update = "Callofminiavatar/UpdateTexture";

	protected string actionName_Tex_Delete = "Callofminiavatar/DelTexture";

	protected string actionName_TexSell_Set = "Callofminiavatar/SetSell";

	protected string actionName_TexSell_Delete = "Callofminiavatar/DelSell";

	protected string actionName_TexSell_LeftTime = "Callofminiavatar/GetSellBytid";

	protected string actionName_TexSell_GetTIDs = "Callofminiavatar/GetRandSellId";

	protected string actionName_TexSell_GetTexs = "Callofminiavatar/GetRandSell";

	protected string actionName_TexSell_GetTexInfo = "Callofminiavatar/GetSellAllBytid";

	protected string actionName_TexSell_Buy = "Callofminiavatar/UpdateSell";

	protected string actionName_TexOfficial_GetTexInfo = "Callofminiavatar/GetAvatarBytid";

	protected string actionName_TexOfficial_Update = "Callofminiavatar/UpdateAvatar";

	protected string actionName_TexSuit_Set = "Callofminiavatar/SetSuit";

	protected string actionName_TexSuit_Delete = "Callofminiavatar/DelSuit";

	protected string actionName_TexSuit_LeftTime = "Callofminiavatar/GetSuitBytid";

	protected string actionName_TexSuit_GetTIDs = "Callofminiavatar/GetRandSuitId";

	protected string actionName_TexSuit_GetTexs = "Callofminiavatar/GetRandSuit";

	protected string actionName_TexSuit_GetTexInfo = "Callofminiavatar/GetSuitAllBytid";

	protected string actionName_TexSuit_Buy = "Callofminiavatar/UpdateSuit";

	public static COMA_HTTP_TextureManager Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Object.DestroyObject(base.gameObject);
			return;
		}
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_TEXTUREINIT, CreatePlayerTexture);
	}

	private void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_TEXTUREINIT, CreatePlayerTexture);
	}

	private void CreatePlayerTexture(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerTextureInit cOMA_CD_PlayerTextureInit = commandDatas as COMA_CD_PlayerTextureInit;
		if (cOMA_CD_PlayerTextureInit.dataSender.IsItMe)
		{
			return;
		}
		string text = cOMA_CD_PlayerTextureInit.dataSender.Id.ToString();
		GameObject gameObject = GameObject.Find("AllPlayers");
		Transform transform = null;
		if (gameObject != null)
		{
			transform = gameObject.transform.FindChild(text);
		}
		if (transform == null)
		{
			Debug.Log("Save Textures To List!!!!!!!!!!!!!!!");
			for (int i = 0; i < 3; i++)
			{
				byte[] data = COMA_TexBase.Instance.StringToTextureBytes(cOMA_CD_PlayerTextureInit.texStr[i]);
				Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(data);
				texture2D.filterMode = FilterMode.Point;
				string key = text + "_" + i;
				COMA_TexLib.Instance.currentRoomPlayerTextures.Add(key, texture2D);
			}
			return;
		}
		Debug.Log("Load Textures To Player!!!!!!!!!!!!!!!");
		COMA_PlayerSync component = transform.GetComponent<COMA_PlayerSync>();
		for (int j = 0; j < 3; j++)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("tid", cOMA_CD_PlayerTextureInit.texUUID[j]);
			hashtable.Add("Tex", cOMA_CD_PlayerTextureInit.texStr[j]);
			hashtable.Add("Kind", j.ToString());
			string response = JsonMapper.ToJson(hashtable);
			component.FinishDownLoadTextures(response);
		}
	}

	public void InitDeliverServer()
	{
		HttpClient.Instance().AddServer(serverName_Deliver, COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
	}

	protected void InitTextureServer()
	{
		HttpClient.Instance().AddServer(serverName_File, COMA_ServerManager.Instance.saverSvrAddr, COMA_ServerManager.Instance.saverSvrOutTime, COMA_ServerManager.Instance.saverSvrKey);
		HttpClient.Instance().AddServer(serverName_TextureBase, COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
		HttpClient.Instance().AddServer(serverName_TextureSell, COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
		HttpClient.Instance().AddServer(serverName_TextureOfficial, COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
		HttpClient.Instance().AddServer(serverName_TextureSuit, COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
	}

	private void Start()
	{
	}

	public void DeliverServerUpdate(string uuid, string uname)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", uuid);
		hashtable.Add("uname", uname);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_Deliver, deliverName_File_Get, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void DeliverServerChangeName(string pid, string pName)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", pid);
		hashtable.Add("uname", pName);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_Deliver, deliverName_File_SetName, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", pName);
	}

	public void PlayerPref_Init()
	{
		string contentForServerSave = COMA_Pref.Instance.contentForServerSave;
		HttpClient.Instance().SendRequest(serverName_File, actionName_File_Set, contentForServerSave, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void PlayerPref_Update(string pid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", pid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_File, actionName_File_Get, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void PlayerPref_Upload()
	{
		HttpClient.Instance().SendRequest(serverName_File, actionName_File_Update, COMA_Pref.Instance.contentForServerSave, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void Texture_Init(int i, string content)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Tex" + i, content);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureBase, actionName_Tex_Set, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void Texture_Init(string content1, string content2, string content3)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Tex0", content1);
		hashtable.Add("Tex1", content2);
		hashtable.Add("Tex2", content3);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureBase, actionName_Tex_Set, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void Texture_UpdateToServer(int i, string content)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", COMA_Pref.Instance.TID[i]);
		hashtable.Add("Tex", content);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureBase, actionName_Tex_Update, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	private void TextureBase_DeleteToServer(int i)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", COMA_Pref.Instance.TID[i]);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureBase, actionName_Tex_Delete, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void TexturePackage_Set(string id, string content)
	{
	}

	public void TexturePackage_Update(string id, string content)
	{
	}

	public void TexturePackage_Get(string id)
	{
	}

	public void TextureSell_InitToServer(string tex, int kind, int gold, int num, Component com)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("Tex", tex);
		hashtable.Add("Kind", kind);
		hashtable.Add("Gold", gold);
		hashtable.Add("Number", num);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_Set, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", com);
	}

	public void TextureSell_GetTIDListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_GetTIDs, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSell_GetTexListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_GetTexs, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSell_GetTextureFromServer(string tid, string param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_GetTexInfo, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSell_DeleteToServer(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_Delete, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void TextureSell_LeftTimeFromServer(string tid, int slotID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_LeftTime, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", slotID);
	}

	public void TextureSell_Buy(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSell, actionName_TexSell_Buy, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void TextureOfficial_GetTextureFromServer(int param)
	{
		Hashtable obj = new Hashtable();
		string data = JsonMapper.ToJson(obj);
		HttpClient.Instance().SendRequest(serverName_TextureOfficial, actionName_TexOfficial_GetTexInfo, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureOfficial_UpdateTextureToServer(string tex, string tex1, string tex2, int gold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Tex", tex);
		hashtable.Add("Tex1", tex1);
		hashtable.Add("Tex2", tex2);
		hashtable.Add("Gold", gold);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureOfficial, actionName_TexOfficial_Update, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void TextureSuit_InitToServer(string tex, string tex1, string tex2, int kind, int gold, int num, Component com)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("Tex", tex);
		hashtable.Add("Tex1", tex1);
		hashtable.Add("Tex2", tex2);
		hashtable.Add("Kind", kind);
		hashtable.Add("Gold", gold);
		hashtable.Add("Number", num);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_Set, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", com);
	}

	public void TextureSuit_GetTIDListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_GetTIDs, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSuit_GetTexListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_GetTexs, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSuit_GetTextureFromServer(string tid, string param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_GetTexInfo, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", param);
	}

	public void TextureSuit_DeleteToServer(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_Delete, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void TextureSuit_LeftTimeFromServer(string tid, int slotID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_LeftTime, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", slotID);
	}

	public void TextureSuit_Buy(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_TextureSuit, actionName_TexSuit_Buy, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	protected void Update()
	{
		HttpClient.Instance().HandleResponse();
	}

	private int Send(string server, string action, string data)
	{
		return HttpClient.Instance().SendRequest(server, action, data, base.gameObject.name, "COMA_HTTP_TextureManager", "ReceiveFunction", string.Empty);
	}

	public void Cancel(int taskID)
	{
		HttpClient.Instance().CancelTask(taskID);
		if (responseMap.ContainsKey(taskID))
		{
			responseMap.Remove(taskID);
		}
	}

	public void NeedReinputName(string param)
	{
		if (UIInputName.Instance != null)
		{
			UIInputName.Instance.DestroyWaitingBox();
		}
	}

	public void Quit(string param)
	{
		Application.Quit();
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result);
		}
		else if (server == serverName_Deliver)
		{
			if (action == deliverName_File_Get)
			{
				SceneTimerInstance.Instance.Remove(COMA_Version.Instance.TimeOut_InputName);
				SceneTimerInstance.Instance.Remove(COMA_Version.Instance.TimeOut_Update);
				Debug.LogWarning("Require Deliver file success!!");
				Debug.Log(response);
				JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
				if (response.Contains("\"existName\":true"))
				{
					UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(122);
					uI_MsgBox.AddProceYesHandler(NeedReinputName);
					return;
				}
				COMA_Sys.Instance.bNeedInitPref = false;
				if (UIInputName.Instance != null)
				{
					UIInputName.Instance.LeaveAnim(string.Empty);
				}
				COMA_ServerManager.Instance.saverSvrAddr = jsonData["dataServer"].ToString();
				COMA_ServerManager.Instance.serverAddr_Save = jsonData["fileServer"].ToString();
				InitTextureServer();
				int num = int.Parse(jsonData["state"].ToString());
				if (num == 1)
				{
					UI_MsgBox uI_MsgBox2 = TUI_MsgBox.Instance.MessageBox(103);
					uI_MsgBox2.AddProceYesHandler(Quit);
				}
				else if (bool.Parse(jsonData["isNew"].ToString()))
				{
					PlayerPref_Init();
					SceneTimerInstance.Instance.Add(10f, COMA_Version.Instance.TimeOut_PrefInit);
				}
				else
				{
					PlayerPref_Update(COMA_Pref.Instance.playerID);
					SceneTimerInstance.Instance.Add(10f, COMA_Version.Instance.TimeOut_PrefUpdate);
				}
			}
			else
			{
				if (!(action == deliverName_File_SetName))
				{
					return;
				}
				SceneTimerInstance.Instance.Remove(COMA_Version.Instance.TimeOut_InputName);
				Debug.LogWarning("Change Name success!!");
				Debug.Log(response);
				JsonData jsonData2 = JsonMapper.ToObject<JsonData>(response);
				if (bool.Parse(jsonData2["existName"].ToString()))
				{
					UI_MsgBox uI_MsgBox3 = TUI_MsgBox.Instance.MessageBox(122);
					uI_MsgBox3.AddProceYesHandler(NeedReinputName);
					return;
				}
				string nickname = (string)param;
				COMA_Pref.Instance.nickname = nickname;
				if (UIOptions.Instance != null)
				{
					UIOptions.Instance.DisableBlock();
				}
				COMA_Pref.Instance.Save(true);
				if (UIInputName.Instance != null)
				{
					UIInputName.Instance.LeaveAnim(string.Empty);
					UIInputName.Instance.DestroyWaitingBox();
				}
			}
		}
		else if (server == serverName_File)
		{
			if (action == actionName_File_Set)
			{
				SceneTimerInstance.Instance.Remove(COMA_Version.Instance.TimeOut_PrefInit);
				Debug.LogWarning("Require Player file success!!");
				Debug.Log(response);
				JsonData jsonData3 = JsonMapper.ToObject<JsonData>(response);
				COMA_Pref.Instance.playerID = jsonData3["uuid"].ToString();
				COMA_Pref.Instance.SaveLocal();
				COMA_Pref.Instance.UpPlayerTexturesInit();
				COMA_Version.Instance.LoadSaveFinish();
			}
			else if (action == actionName_File_Get)
			{
				SceneTimerInstance.Instance.Remove(COMA_Version.Instance.TimeOut_PrefUpdate);
				if (!(response == "{\"code\":1}"))
				{
					Debug.LogWarning("Load Player file success");
					Debug.Log(response);
					COMA_Pref.Instance.contentForServerSave = response;
				}
				COMA_Pref.Instance.SaveLocal();
				COMA_Version.Instance.LoadSaveFinish();
				if (UIInputName.Instance != null)
				{
					UIInputName.Instance.LeaveAnim(string.Empty);
					UIInputName.Instance.DestroyWaitingBox();
				}
			}
		}
		else if (server == serverName_TextureBase)
		{
			if (action == actionName_Tex_Set)
			{
				Debug.Log(response);
				JsonData jsonData4 = JsonMapper.ToObject<JsonData>(response);
				COMA_Pref.Instance.TID[0] = jsonData4["tid0"].ToString();
				COMA_Pref.Instance.TID[1] = jsonData4["tid1"].ToString();
				COMA_Pref.Instance.TID[2] = jsonData4["tid2"].ToString();
			}
			else if (action == actionName_Tex_Get)
			{
				if (response == "{\"code\":1}")
				{
					Debug.LogWarning("******** current texture is not Exist on server!!");
					return;
				}
				responseMap[taskId](response);
				responseMap.Remove(taskId);
			}
		}
		else if (server == serverName_TextureSell)
		{
			Debug.Log("散装 action : " + action + "  response : " + response);
			if (action == actionName_TexSell_Set)
			{
				JsonData jsonData5 = JsonMapper.ToObject<JsonData>(response);
				if (param == null)
				{
					Debug.LogError("Market not Exist any more!!");
				}
				string tid = jsonData5["tid"].ToString();
				UIMarket uIMarket = (UIMarket)param;
				uIMarket.SellTexSuccess(tid);
			}
			else if (action == actionName_TexSell_LeftTime)
			{
				JsonData jsonData6 = JsonMapper.ToObject<JsonData>(response);
				int slotID = (int)param;
				int gold = int.Parse(jsonData6["Gold"].ToString());
				int num2 = int.Parse(jsonData6["Number"].ToString());
				float num3 = float.Parse(jsonData6["Time"].ToString());
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				if (UITrade.Instance != null)
				{
					UITrade.Instance.UpdateSlotToSellShell(slotID, gold, num2, num3);
				}
			}
			else if (action == actionName_TexSell_GetTexInfo)
			{
				string text = (string)param;
				if (response == "{\"code\":1}")
				{
					if (text.StartsWith("OnSale_"))
					{
						string[] array = text.Split('_');
						Debug.Log(" 已删除挂单物品 ----------------> " + array[3]);
						COMA_TexOnSale.Instance.DeleteWithtid(array[3]);
						COMA_Pref.Instance.Save(true);
					}
					return;
				}
				JsonData jsonData7 = JsonMapper.ToObject<JsonData>(response);
				string content = jsonData7["Tex"].ToString();
				Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content));
				texture2D.Apply(false);
				texture2D.filterMode = FilterMode.Point;
				int kind = int.Parse(jsonData7["Kind"].ToString());
				int gold2 = int.Parse(jsonData7["Gold"].ToString());
				int num4 = int.Parse(jsonData7["Number"].ToString());
				float num5 = float.Parse(jsonData7["Time"].ToString());
				if (num5 < 0f)
				{
					num5 = 0f;
				}
				if (text.StartsWith("OnSale_"))
				{
					if (UIMarket.Instance != null)
					{
						string[] array2 = text.Split('_');
						UIMarket.Instance.RefreshSellingInfo(array2[3], texture2D, kind, gold2, num4, num5, int.Parse(array2[1]), int.Parse(array2[2]));
					}
				}
				else if (COMA_Scene_Trade.Instance != null)
				{
					string[] array3 = text.Split('_');
					COMA_Scene_Trade.Instance.SetPart(array3[1], texture2D, kind, gold2, num4, num5, false, int.Parse(array3[0]));
				}
			}
			else if (action == actionName_TexSell_GetTIDs)
			{
				if (response == "{\"code\":1}")
				{
					Debug.LogWarning("No part texture on server!!");
					return;
				}
				string text2 = response.Substring(response.IndexOf("[{"));
				text2 = text2.Substring(1, text2.IndexOf("}]"));
				string[] array4 = text2.Split(',');
				for (int i = 0; i < array4.Length; i++)
				{
					JsonData jsonData8 = JsonMapper.ToObject<JsonData>(array4[i]);
					string text3 = jsonData8["tid"].ToString();
					TextureSell_GetTextureFromServer(text3, (int)param + "_" + text3);
				}
				COMA_Scene_Trade.Instance.texCountToLoad_Part = array4.Length;
			}
			else if (action == actionName_TexSell_GetTexs)
			{
				if (response == "{\"code\":1}")
				{
					Debug.LogWarning("No part texture on server!!");
				}
				else
				{
					if (!(COMA_Scene_Trade.Instance != null))
					{
						return;
					}
					JsonData jsonData9 = JsonMapper.ToObject<JsonData>(response);
					JsonData jsonData10 = jsonData9["datas"];
					Debug.Log("Get " + jsonData10.Count + " Textures.");
					for (int j = 0; j < jsonData10.Count; j++)
					{
						string tid2 = jsonData10[j]["tid"].ToString();
						string content2 = jsonData10[j]["Tex"].ToString();
						int kind2 = int.Parse(jsonData10[j]["Kind"].ToString());
						int gold3 = int.Parse(jsonData10[j]["Gold"].ToString());
						float num6 = float.Parse(jsonData10[j]["Time"].ToString());
						if (num6 < 0f)
						{
							num6 = 0f;
						}
						Texture2D texture2D2 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						texture2D2.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content2));
						texture2D2.Apply(false);
						texture2D2.filterMode = FilterMode.Point;
						COMA_Scene_Trade.Instance.SetPart(tid2, texture2D2, kind2, gold3, 1, num6, false, (int)param);
					}
					COMA_Scene_Trade.Instance.texCountToLoad_Part = 0;
				}
			}
			else if (!(action == actionName_TexSell_Buy) && !(action == actionName_TexSell_Delete))
			{
			}
		}
		else if (server == serverName_TextureOfficial)
		{
			Debug.Log("官方 action : " + action + "  response : " + response);
			if (action == actionName_TexOfficial_GetTexInfo)
			{
				if (COMA_Scene_Trade.Instance != null)
				{
					JsonData jsonData11 = JsonMapper.ToObject<JsonData>(response);
					string content3 = jsonData11["Tex"].ToString();
					string content4 = jsonData11["Tex1"].ToString();
					string content5 = jsonData11["Tex2"].ToString();
					int gold4 = int.Parse(jsonData11["Gold"].ToString());
					Texture2D[] array5 = new Texture2D[3]
					{
						new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false),
						null,
						null
					};
					array5[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content3));
					array5[0].Apply(false);
					array5[0].filterMode = FilterMode.Point;
					array5[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					array5[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content4));
					array5[1].Apply(false);
					array5[1].filterMode = FilterMode.Point;
					array5[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					array5[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content5));
					array5[2].Apply(false);
					array5[2].filterMode = FilterMode.Point;
					COMA_Scene_Trade.Instance.SetOfficial(array5[0], 0, gold4, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.SetOfficial(array5[1], 1, gold4, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.SetOfficial(array5[2], 2, gold4, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.texCountToLoad_Official = 0;
				}
			}
			else if (action == actionName_TexOfficial_Update && response == "{\"code\":0}")
			{
				Debug.Log("Update Successfull!!!!");
			}
		}
		else
		{
			if (!(server == serverName_TextureSuit))
			{
				return;
			}
			Debug.Log("套装 action : " + action + "  response : " + response);
			if (action == actionName_TexSuit_Set)
			{
				JsonData jsonData12 = JsonMapper.ToObject<JsonData>(response);
				if (param == null)
				{
					Debug.LogError("Market not Exist any more!!");
				}
				string tid3 = jsonData12["tid"].ToString();
				UIMarket uIMarket2 = (UIMarket)param;
				uIMarket2.SellSuitSuccess(tid3);
			}
			else if (action == actionName_TexSuit_LeftTime)
			{
				JsonData jsonData13 = JsonMapper.ToObject<JsonData>(response);
				int slotID2 = (int)param;
				int gold5 = (int)jsonData13["Gold"];
				int num7 = (int)jsonData13["Number"];
				float num8 = (float)(double)jsonData13["Time"];
				if (num8 < 0f)
				{
					num8 = 0f;
				}
				if (UITrade.Instance != null)
				{
					UITrade.Instance.UpdateSlotToSellShell(slotID2, gold5, num7, num8);
				}
			}
			else if (action == actionName_TexSuit_GetTexInfo)
			{
				string text4 = (string)param;
				if (response == "{\"code\":1}")
				{
					if (text4.StartsWith("OnSale_"))
					{
						string[] array6 = text4.Split('_');
						Debug.Log(" 已删除挂单物品 ----------------> " + array6[3]);
						COMA_TexOnSale.Instance.DeleteWithtid(array6[3]);
						COMA_Pref.Instance.Save(true);
					}
					return;
				}
				JsonData jsonData14 = JsonMapper.ToObject<JsonData>(response);
				Texture2D[] array7 = new Texture2D[3];
				string empty = string.Empty;
				empty = (string)jsonData14["Tex"];
				array7[0] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array7[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array7[0].Apply(false);
				array7[0].filterMode = FilterMode.Point;
				empty = (string)jsonData14["Tex1"];
				array7[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array7[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array7[1].Apply(false);
				array7[1].filterMode = FilterMode.Point;
				empty = (string)jsonData14["Tex2"];
				array7[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array7[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array7[2].Apply(false);
				array7[2].filterMode = FilterMode.Point;
				int gold6 = (int)jsonData14["Gold"];
				int num9 = (int)jsonData14["Number"];
				float num10 = (float)(double)jsonData14["Time"];
				if (num10 < 0f)
				{
					num10 = 0f;
				}
				if (text4.StartsWith("OnSale_"))
				{
					if (UIMarket.Instance != null)
					{
						string[] array8 = text4.Split('_');
						UIMarket.Instance.RefreshSellingInfo(array8[3], array7, gold6, num9, num10, int.Parse(array8[1]), int.Parse(array8[2]));
					}
				}
				else if (COMA_Scene_Trade.Instance != null)
				{
					string[] array9 = text4.Split('_');
					COMA_Scene_Trade.Instance.SetSuit(array9[1], array7, gold6, num9, num10, false, int.Parse(array9[0]));
				}
			}
			else if (action == actionName_TexSuit_GetTIDs)
			{
				if (response == "{\"code\":1}")
				{
					Debug.LogWarning("No part texture on server!!");
					return;
				}
				string text5 = response.Substring(response.IndexOf("[{"));
				text5 = text5.Substring(1, text5.IndexOf("}]"));
				string[] array10 = text5.Split(',');
				for (int k = 0; k < array10.Length; k++)
				{
					JsonData jsonData15 = JsonMapper.ToObject<JsonData>(array10[k]);
					string text6 = (string)jsonData15["tid"];
					TextureSuit_GetTextureFromServer(text6, (int)param + "_" + text6);
				}
				COMA_Scene_Trade.Instance.texCountToLoad_Suit = array10.Length;
			}
			else if (action == actionName_TexSuit_GetTexs)
			{
				if (response == "{\"code\":1}")
				{
					Debug.LogWarning("No suit texture on server!!");
				}
				else
				{
					if (!(COMA_Scene_Trade.Instance != null))
					{
						return;
					}
					JsonData jsonData16 = JsonMapper.ToObject<JsonData>(response);
					JsonData jsonData17 = jsonData16["datas"];
					Debug.Log("Get " + jsonData17.Count + " Textures.");
					for (int l = 0; l < jsonData17.Count; l++)
					{
						string tid4 = (string)jsonData17[l]["tid"];
						string content6 = (string)jsonData17[l]["Tex"];
						string content7 = (string)jsonData17[l]["Tex1"];
						string content8 = (string)jsonData17[l]["Tex2"];
						int gold7 = (int)jsonData17[l]["Gold"];
						float num11 = (float)(double)jsonData17[l]["Time"];
						if (num11 < 0f)
						{
							num11 = 0f;
						}
						Texture2D[] array11 = new Texture2D[3]
						{
							new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false),
							null,
							null
						};
						array11[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content6));
						array11[0].Apply(false);
						array11[0].filterMode = FilterMode.Point;
						array11[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						array11[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content7));
						array11[1].Apply(false);
						array11[1].filterMode = FilterMode.Point;
						array11[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						array11[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content8));
						array11[2].Apply(false);
						array11[2].filterMode = FilterMode.Point;
						COMA_Scene_Trade.Instance.SetSuit(tid4, array11, gold7, 1, num11, false, (int)param);
					}
					COMA_Scene_Trade.Instance.texCountToLoad_Suit = 0;
				}
			}
			else if (!(action == actionName_TexSuit_Buy) && !(action == actionName_TexSuit_Delete))
			{
			}
		}
	}
}
