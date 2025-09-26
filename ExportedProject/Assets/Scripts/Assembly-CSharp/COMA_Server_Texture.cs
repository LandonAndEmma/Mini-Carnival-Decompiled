using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_Server_Texture : MonoBehaviour
{
	protected const string serverName_TexturePackage = "svr_packageTexture";

	protected const string serverName_TextureSell = "svr_sellTexture";

	protected const string serverName_TextureOfficial = "svr_TriTexture";

	protected const string serverName_TextureSuit = "svr_SuitTexture";

	private static COMA_Server_Texture _instance;

	private bool bInitServer;

	protected string actionName_Tex_Set = "Callofminiavatar/SetTexturePackage";

	protected string actionName_Tex_Get = "Callofminiavatar/GetTexturePackage";

	protected string actionName_TexSell_Set = "Callofminiavatar/SetSell";

	protected string actionName_TexSell_Delete = "Callofminiavatar/DelSell";

	protected string actionName_TexSell_LeftTime = "Callofminiavatar/GetSellBytid";

	protected string actionName_TexSell_GetTexs = "Callofminiavatar/GetRandSell";

	protected string actionName_TexSell_GetTexInfo = "Callofminiavatar/GetSellAllBytid";

	protected string actionName_TexSell_Buy = "Callofminiavatar/UpdateSell";

	protected string actionName_TexOfficial_GetTexInfo = "Callofminiavatar/GetAvatarBytid";

	protected string actionName_TexOfficial_Update = "Callofminiavatar/UpdateAvatar";

	protected string actionName_TexSuit_Set = "Callofminiavatar/SetSuit";

	protected string actionName_TexSuit_Delete = "Callofminiavatar/DelSuit";

	protected string actionName_TexSuit_LeftTime = "Callofminiavatar/GetSuitBytid";

	protected string actionName_TexSuit_GetTexs = "Callofminiavatar/GetRandSuit";

	protected string actionName_TexSuit_GetTexInfo = "Callofminiavatar/GetSuitAllBytid";

	protected string actionName_TexSuit_Buy = "Callofminiavatar/UpdateSuit";

	public static COMA_Server_Texture Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_TEXTUREINIT, CreatePlayerTexture);
	}

	private void OnDisable()
	{
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_TEXTUREINIT, CreatePlayerTexture);
	}

	private void Start()
	{
	}

	public void InitServer(string addr, float timeout, string key)
	{
		bInitServer = true;
		HttpClient.Instance().AddServer("svr_packageTexture", addr, timeout, key);
		HttpClient.Instance().AddServer("svr_sellTexture", addr, timeout, key);
		HttpClient.Instance().AddServer("svr_TriTexture", addr, timeout, key);
		HttpClient.Instance().AddServer("svr_SuitTexture", addr, timeout, key);
	}

	public void TexturePackage_Set(string id, string content)
	{
		if (bInitServer)
		{
			Debug.Log("Set Texture : " + id);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("uuid", id);
			hashtable.Add("Content", content);
			string data = JsonMapper.ToJson(hashtable);
			HttpClient.Instance().SendRequest("svr_packageTexture", actionName_Tex_Set, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
		}
	}

	public void TexturePackage_Get(string id, int i)
	{
		if (bInitServer)
		{
			Debug.Log("Get Texture:" + id);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("uuid", id);
			string data = JsonMapper.ToJson(hashtable);
			HttpClient.Instance().SendRequest("svr_packageTexture", actionName_Tex_Get, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", i);
		}
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
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_Set, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", com);
	}

	public void TextureSell_GetTexListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_GetTexs, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", param);
	}

	public void TextureSell_GetTextureFromServer(string tid, string param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_GetTexInfo, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", param);
	}

	public void TextureSell_DeleteToServer(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_Delete, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
	}

	public void TextureSell_LeftTimeFromServer(string tid, int slotID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_LeftTime, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", slotID);
	}

	public void TextureSell_Buy(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_sellTexture", actionName_TexSell_Buy, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
	}

	public void TextureOfficial_GetTextureFromServer(int param)
	{
		Hashtable obj = new Hashtable();
		string data = JsonMapper.ToJson(obj);
		HttpClient.Instance().SendRequest("svr_TriTexture", actionName_TexOfficial_GetTexInfo, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", param);
	}

	public void TextureOfficial_UpdateTextureToServer(string tex, string tex1, string tex2, int gold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Tex", tex);
		hashtable.Add("Tex1", tex1);
		hashtable.Add("Tex2", tex2);
		hashtable.Add("Gold", gold);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_TriTexture", actionName_TexOfficial_Update, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
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
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_Set, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", com);
	}

	public void TextureSuit_GetTexListFromServer(int count, int param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("num", count);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_GetTexs, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", param);
	}

	public void TextureSuit_GetTextureFromServer(string tid, string param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_GetTexInfo, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", param);
	}

	public void TextureSuit_DeleteToServer(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_Delete, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
	}

	public void TextureSuit_LeftTimeFromServer(string tid, int slotID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_LeftTime, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", slotID);
	}

	public void TextureSuit_Buy(string tid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("tid", tid);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest("svr_SuitTexture", actionName_TexSuit_Buy, data, base.gameObject.name, "COMA_Server_Texture", "ReceiveFunction", string.Empty);
	}

	private void CreatePlayerTexture(COMA_CommandDatas commandDatas)
	{
		Debug.Log("CreatePlayerTexture!!");
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

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (server == "svr_packageTexture" && action == actionName_Tex_Get)
			{
				int i = (int)param;
				string id = COMA_Pref.Instance.playerID + "_" + i;
				COMA_Pref.Instance.DownLoadAPackageTexture(id, i);
			}
			return;
		}
		switch (server)
		{
		case "svr_packageTexture":
			if (action == actionName_Tex_Set)
			{
				Debug.Log("Set Texture Success!!");
			}
			else
			{
				if (!(action == actionName_Tex_Get))
				{
					break;
				}
				Debug.Log(response);
				int index = (int)param;
				if (response == string.Empty)
				{
					Debug.Log("response is null~~");
					string empty2 = string.Empty;
					empty2 = ((COMA_Pref.Instance.package.pack[index].serialName == "Head01") ? "6ba2377776d6c137ee29551baff81bb5" : ((COMA_Pref.Instance.package.pack[index].serialName == "Body01") ? "54245d0a0b0c5c8305976247da71f59f" : ((!(COMA_Pref.Instance.package.pack[index].serialName == "Leg01")) ? "bfbe092a8f68fabfe83afbc5e961487d" : "9a53aef61db65e1ed1298fca0cc15a3d")));
					UIDataBufferCenter.Instance.FetchFileByMD5(empty2, delegate(byte[] buffer)
					{
						Texture2D texture2D4 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						texture2D4.LoadImage(buffer);
						texture2D4.Apply(false);
						texture2D4.filterMode = FilterMode.Point;
						COMA_Pref.Instance.package.pack[index].texture = texture2D4;
						COMA_Pref.Instance.package.pack[index].SavePNG();
					});
				}
				else
				{
					JsonData jsonData12 = JsonMapper.ToObject<JsonData>(response);
					string content9 = jsonData12["Content"].ToString();
					Texture2D texture2D3 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					texture2D3.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content9));
					texture2D3.Apply(false);
					texture2D3.filterMode = FilterMode.Point;
					COMA_Pref.Instance.package.pack[index].texture = texture2D3;
					COMA_Pref.Instance.package.pack[index].SavePNG();
					Debug.Log("response finish~~");
				}
			}
			break;
		case "svr_sellTexture":
			Debug.Log("散装 action : " + action + "  response : " + response);
			if (action == actionName_TexSell_Set)
			{
				JsonData jsonData6 = JsonMapper.ToObject<JsonData>(response);
				if (param == null)
				{
					Debug.LogError("Market not Exist any more!!");
				}
				string tid3 = jsonData6["tid"].ToString();
				UIMarket uIMarket2 = (UIMarket)param;
				uIMarket2.SellTexSuccess(tid3);
			}
			else if (action == actionName_TexSell_LeftTime)
			{
				JsonData jsonData7 = JsonMapper.ToObject<JsonData>(response);
				int slotID2 = (int)param;
				int gold4 = int.Parse(jsonData7["Gold"].ToString());
				int num6 = int.Parse(jsonData7["Number"].ToString());
				float num7 = float.Parse(jsonData7["Time"].ToString());
				if (num7 < 0f)
				{
					num7 = 0f;
				}
				if (UITrade.Instance != null)
				{
					UITrade.Instance.UpdateSlotToSellShell(slotID2, gold4, num6, num7);
				}
			}
			else if (action == actionName_TexSell_GetTexInfo)
			{
				string text2 = (string)param;
				if (response == "{\"code\":1}")
				{
					if (text2.StartsWith("OnSale_"))
					{
						string[] array6 = text2.Split('_');
						Debug.Log(" 已删除挂单物品 ----------------> " + array6[3]);
						COMA_TexOnSale.Instance.DeleteWithtid(array6[3]);
						COMA_Pref.Instance.Save(true);
					}
					break;
				}
				JsonData jsonData8 = JsonMapper.ToObject<JsonData>(response);
				string content4 = jsonData8["Tex"].ToString();
				Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content4));
				texture2D.Apply(false);
				texture2D.filterMode = FilterMode.Point;
				int kind = int.Parse(jsonData8["Kind"].ToString());
				int gold5 = int.Parse(jsonData8["Gold"].ToString());
				int num8 = int.Parse(jsonData8["Number"].ToString());
				float num9 = float.Parse(jsonData8["Time"].ToString());
				if (num9 < 0f)
				{
					num9 = 0f;
				}
				if (text2.StartsWith("OnSale_"))
				{
					if (UIMarket.Instance != null)
					{
						string[] array7 = text2.Split('_');
						UIMarket.Instance.RefreshSellingInfo(array7[3], texture2D, kind, gold5, num8, num9, int.Parse(array7[1]), int.Parse(array7[2]));
					}
				}
				else if (COMA_Scene_Trade.Instance != null)
				{
					string[] array8 = text2.Split('_');
					COMA_Scene_Trade.Instance.SetPart(array8[1], texture2D, kind, gold5, num8, num9, false, int.Parse(array8[0]));
				}
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
						break;
					}
					JsonData jsonData9 = JsonMapper.ToObject<JsonData>(response);
					JsonData jsonData10 = jsonData9["datas"];
					Debug.Log("Get " + jsonData10.Count + " Textures.");
					for (int k = 0; k < jsonData10.Count; k++)
					{
						string tid4 = jsonData10[k]["tid"].ToString();
						string content5 = jsonData10[k]["Tex"].ToString();
						int kind2 = int.Parse(jsonData10[k]["Kind"].ToString());
						int gold6 = int.Parse(jsonData10[k]["Gold"].ToString());
						float num10 = float.Parse(jsonData10[k]["Time"].ToString());
						if (num10 < 0f)
						{
							num10 = 0f;
						}
						Texture2D texture2D2 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						texture2D2.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content5));
						texture2D2.Apply(false);
						texture2D2.filterMode = FilterMode.Point;
						COMA_Scene_Trade.Instance.SetPart(tid4, texture2D2, kind2, gold6, 1, num10, false, (int)param);
					}
					COMA_Scene_Trade.Instance.texCountToLoad_Part = 0;
				}
			}
			else if (!(action == actionName_TexSell_Buy) && !(action == actionName_TexSell_Delete))
			{
			}
			break;
		case "svr_TriTexture":
			Debug.Log("官方 action : " + action + "  response : " + response);
			if (action == actionName_TexOfficial_GetTexInfo)
			{
				if (COMA_Scene_Trade.Instance != null)
				{
					JsonData jsonData11 = JsonMapper.ToObject<JsonData>(response);
					string content6 = jsonData11["Tex"].ToString();
					string content7 = jsonData11["Tex1"].ToString();
					string content8 = jsonData11["Tex2"].ToString();
					int gold7 = int.Parse(jsonData11["Gold"].ToString());
					Texture2D[] array9 = new Texture2D[3]
					{
						new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false),
						null,
						null
					};
					array9[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content6));
					array9[0].Apply(false);
					array9[0].filterMode = FilterMode.Point;
					array9[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					array9[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content7));
					array9[1].Apply(false);
					array9[1].filterMode = FilterMode.Point;
					array9[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					array9[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content8));
					array9[2].Apply(false);
					array9[2].filterMode = FilterMode.Point;
					COMA_Scene_Trade.Instance.SetOfficial(array9[0], 0, gold7, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.SetOfficial(array9[1], 1, gold7, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.SetOfficial(array9[2], 2, gold7, 2, 86400f, true, (int)param);
					COMA_Scene_Trade.Instance.texCountToLoad_Official = 0;
				}
			}
			else if (action == actionName_TexOfficial_Update && response == "{\"code\":0}")
			{
				Debug.Log("Update Successfull!!!!");
			}
			break;
		case "svr_SuitTexture":
			Debug.Log("套装 action : " + action + "  response : " + response);
			if (action == actionName_TexSuit_Set)
			{
				JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
				if (param == null)
				{
					Debug.LogError("Market not Exist any more!!");
				}
				string tid = jsonData["tid"].ToString();
				UIMarket uIMarket = (UIMarket)param;
				uIMarket.SellSuitSuccess(tid);
			}
			else if (action == actionName_TexSuit_LeftTime)
			{
				JsonData jsonData2 = JsonMapper.ToObject<JsonData>(response);
				int slotID = (int)param;
				int gold = int.Parse(jsonData2["Gold"].ToString());
				int num = int.Parse(jsonData2["Number"].ToString());
				float num2 = float.Parse(jsonData2["Time"].ToString());
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				if (UITrade.Instance != null)
				{
					UITrade.Instance.UpdateSlotToSellShell(slotID, gold, num, num2);
				}
			}
			else if (action == actionName_TexSuit_GetTexInfo)
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
					break;
				}
				JsonData jsonData3 = JsonMapper.ToObject<JsonData>(response);
				Texture2D[] array2 = new Texture2D[3];
				string empty = string.Empty;
				empty = jsonData3["Tex"].ToString();
				array2[0] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array2[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array2[0].Apply(false);
				array2[0].filterMode = FilterMode.Point;
				empty = jsonData3["Tex1"].ToString();
				array2[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array2[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array2[1].Apply(false);
				array2[1].filterMode = FilterMode.Point;
				empty = jsonData3["Tex2"].ToString();
				array2[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				array2[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(empty));
				array2[2].Apply(false);
				array2[2].filterMode = FilterMode.Point;
				int gold2 = int.Parse(jsonData3["Gold"].ToString());
				int num3 = int.Parse(jsonData3["Number"].ToString());
				float num4 = float.Parse(jsonData3["Time"].ToString());
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				if (text.StartsWith("OnSale_"))
				{
					if (UIMarket.Instance != null)
					{
						string[] array3 = text.Split('_');
						UIMarket.Instance.RefreshSellingInfo(array3[3], array2, gold2, num3, num4, int.Parse(array3[1]), int.Parse(array3[2]));
					}
				}
				else if (COMA_Scene_Trade.Instance != null)
				{
					string[] array4 = text.Split('_');
					COMA_Scene_Trade.Instance.SetSuit(array4[1], array2, gold2, num3, num4, false, int.Parse(array4[0]));
				}
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
						break;
					}
					JsonData jsonData4 = JsonMapper.ToObject<JsonData>(response);
					JsonData jsonData5 = jsonData4["datas"];
					Debug.Log("Get " + jsonData5.Count + " Textures.");
					for (int j = 0; j < jsonData5.Count; j++)
					{
						string tid2 = jsonData5[j]["tid"].ToString();
						string content = jsonData5[j]["Tex"].ToString();
						string content2 = jsonData5[j]["Tex1"].ToString();
						string content3 = jsonData5[j]["Tex2"].ToString();
						int gold3 = int.Parse(jsonData5[j]["Gold"].ToString());
						float num5 = float.Parse(jsonData5[j]["Time"].ToString());
						if (num5 < 0f)
						{
							num5 = 0f;
						}
						Texture2D[] array5 = new Texture2D[3]
						{
							new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false),
							null,
							null
						};
						array5[0].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content));
						array5[0].Apply(false);
						array5[0].filterMode = FilterMode.Point;
						array5[1] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						array5[1].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content2));
						array5[1].Apply(false);
						array5[1].filterMode = FilterMode.Point;
						array5[2] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						array5[2].LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content3));
						array5[2].Apply(false);
						array5[2].filterMode = FilterMode.Point;
						COMA_Scene_Trade.Instance.SetSuit(tid2, array5, gold3, 1, num5, false, (int)param);
					}
					COMA_Scene_Trade.Instance.texCountToLoad_Suit = 0;
				}
			}
			else if (!(action == actionName_TexSuit_Buy) && !(action == actionName_TexSuit_Delete))
			{
			}
			break;
		default:
			Debug.LogError(server);
			break;
		}
	}
}
