using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class COMA_Server_Friends : MonoBehaviour
{
	private static COMA_Server_Friends _instance;

	protected string serverName_Friends = "svr_friends";

	protected string actionName_AddFriend = "comavataraccount/friend/AddFriend";

	protected string actionName_AcceptFriend = "comavataraccount/friend/AcceptFriend";

	protected string actionName_ListFriends = "comavataraccount/friend/ListFriends";

	protected string actionName_ListRequires = "comavataraccount/friend/ListRequires";

	protected string actionName_RemoveFriends = "comavataraccount/friend/RemoveFriend";

	protected string actionName_RemoveRequires = "comavataraccount/friend/RemoveRequire";

	private bool bInitFriendTexServer;

	protected string serverName_FriendTexture = "svr_friendTex";

	protected string actionName_Archive_Get = "Callofminiavatar/GetUserBase";

	protected string actionName_Tex_Get = "Callofminiavatar/GetTexturePackage";

	[NonSerialized]
	public int requireCount;

	public List<UIFriends_FriendRequestData> lst_requires = new List<UIFriends_FriendRequestData>();

	public List<UIFriends_FriendRequestData> lst_friends = new List<UIFriends_FriendRequestData>();

	[NonSerialized]
	public int friendsCountMax = 30;

	public static COMA_Server_Friends Instance
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
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitServer(string addr, float timeout, string key)
	{
		HttpClient.Instance().AddServer(serverName_Friends, addr, timeout, key);
	}

	public void AddFriend(string GID, string friendGID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("fid", friendGID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_AddFriend, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", string.Empty);
		SceneTimerInstance.Instance.Add(6f, GettingFriendList);
	}

	public bool GettingFriendList()
	{
		Debug.Log("Send Friend List!!");
		FriendList(COMA_Server_ID.Instance.GID);
		return true;
	}

	public void AcceptFriend(string GID, string friendGID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("fid", friendGID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_AcceptFriend, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", string.Empty);
	}

	public void FriendList(string GID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_ListFriends, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", string.Empty);
	}

	public void RequireList(string GID)
	{
		RequireList(GID, string.Empty);
	}

	public void RequireList(string GID, string param)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_ListRequires, data, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", param);
	}

	public void DeleteFriend(string GID, string friendGID)
	{
		for (int i = 0; i < lst_friends.Count; i++)
		{
			if (lst_friends[i].GID == friendGID)
			{
				lst_friends.RemoveAt(i);
			}
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("fid", friendGID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_RemoveFriends, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", string.Empty);
	}

	public void DeleteRequire(string GID, string friendGID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("fid", friendGID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Friends, actionName_RemoveRequires, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", string.Empty);
	}

	public void GetFriendArchive(string GID, string addr, string param)
	{
		HttpClient.Instance().AddServer(GID, addr, 10f, "abcd@@##980[]L>.");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(GID, actionName_Archive_Get, data, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", param);
	}

	public void GetFriendTexture(int i)
	{
		GetFriendTexture(lst_friends[i].url_texture, lst_friends[i].GID + "_" + lst_friends[i].TInPack[0], "friends_0_" + i);
		GetFriendTexture(lst_friends[i].url_texture, lst_friends[i].GID + "_" + lst_friends[i].TInPack[1], "friends_1_" + i);
		GetFriendTexture(lst_friends[i].url_texture, lst_friends[i].GID + "_" + lst_friends[i].TInPack[2], "friends_2_" + i);
	}

	public void GetFriendTexture(string addr, string id, string param)
	{
		Debug.LogWarning(addr);
		if (!bInitFriendTexServer)
		{
			HttpClient.Instance().AddServer(serverName_FriendTexture, addr, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
			bInitFriendTexServer = true;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", id);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_FriendTexture, actionName_Tex_Get, text, base.gameObject.name, "COMA_Server_Friends", "ReceiveFunction", param);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (!(action == actionName_AddFriend))
			{
			}
		}
		else if (server == serverName_Friends)
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (action == actionName_AddFriend)
			{
				if (response.Contains("NotFoundUser"))
				{
					if (UIFriends.Instance != null)
					{
						UIFriends.Instance.OnAddFriend(false, "NotFoundUser");
					}
				}
				else if (response.Contains("IsFriend"))
				{
					if (UIFriends.Instance != null)
					{
						UIFriends.Instance.OnAddFriend(false, "IsFriend");
					}
				}
				else if (response.Contains("CanNotAddSelf") && UIFriends.Instance != null)
				{
					UIFriends.Instance.OnAddFriend(false, "CanNotAddSelf");
				}
			}
			else
			{
				if (action == actionName_AcceptFriend)
				{
					return;
				}
				if (action == actionName_ListFriends)
				{
					IList list = jsonData["datas"];
					string text = (string)param;
					if (!(text == string.Empty))
					{
						return;
					}
					List<UIFriends_FriendRequestData> list2 = new List<UIFriends_FriendRequestData>();
					for (int i = 0; i < list.Count; i++)
					{
						JsonData jsonData2 = (JsonData)list[i];
						string gID = jsonData2["id"].ToString();
						string url_archive = jsonData2["server"].ToString();
						UIFriends_FriendRequestData uIFriends_FriendRequestData = new UIFriends_FriendRequestData();
						uIFriends_FriendRequestData.GID = gID;
						uIFriends_FriendRequestData.url_archive = url_archive;
						list2.Add(uIFriends_FriendRequestData);
					}
					if (lst_friends.Count > 0)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							int k;
							for (k = 0; k < lst_friends.Count && !(list2[j].GID == lst_friends[k].GID); k++)
							{
							}
							if (k >= lst_friends.Count)
							{
								Debug.Log("-----" + list2[j].GID);
								lst_friends.Add(list2[j]);
								if (UIFriends.Instance != null)
								{
									int num = lst_friends.Count - 1;
									UIFriends.Instance.OnListAdd(num);
									Debug.Log("-----" + lst_friends[num].GID + " " + lst_friends[num].url_archive);
									GetFriendArchive(lst_friends[num].GID, lst_friends[num].url_archive, "friends_" + num);
								}
							}
						}
						return;
					}
					for (int l = 0; l < list2.Count; l++)
					{
						lst_friends.Add(list2[l]);
					}
					if (UIFriends.Instance != null)
					{
						UIFriends.Instance.OnListFriend();
					}
					for (int m = 0; m < list.Count; m++)
					{
						string fileName = "Friends/" + lst_friends[m].GID;
						string text2 = COMA_FileIO.LoadFile(fileName);
						if (text2 != string.Empty)
						{
							Debug.Log(text2);
							ReceiveFunction(0, 0, lst_friends[m].GID, actionName_Archive_Get, text2, "friends_" + m);
						}
						else
						{
							GetFriendArchive(lst_friends[m].GID, lst_friends[m].url_archive, "friends_" + m);
						}
						string[] array = new string[3]
						{
							"Friends/" + Instance.lst_friends[m].GID + "_0",
							"Friends/" + Instance.lst_friends[m].GID + "_1",
							"Friends/" + Instance.lst_friends[m].GID + "_2"
						};
						for (int n = 0; n < array.Length; n++)
						{
							byte[] array2 = COMA_FileIO.ReadPngData(array[n]);
							if (array2 != null)
							{
								lst_friends[m].textures[n] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
								lst_friends[m].textures[n].LoadImage(array2);
								lst_friends[m].textures[n].filterMode = FilterMode.Point;
							}
						}
						if (lst_friends[m].textures[0] != null || lst_friends[m].textures[1] != null || lst_friends[m].textures[2] != null)
						{
							Debug.Log("----------------------Load from local : " + Time.time);
							if (UIFriends.Instance != null)
							{
								UIFriends.Instance.OnRefreshFriend(m);
							}
						}
					}
					COMA_TexTransfer.Instance.SaveFriends(lst_friends);
				}
				else if (action == actionName_ListRequires)
				{
					IList list3 = jsonData["datas"];
					string text3 = (string)param;
					if (text3 == string.Empty)
					{
						lst_requires.Clear();
						for (int num2 = 0; num2 < list3.Count; num2++)
						{
							JsonData jsonData3 = (JsonData)list3[num2];
							string gID2 = jsonData3["id"].ToString();
							string url_archive2 = jsonData3["server"].ToString();
							UIFriends_FriendRequestData uIFriends_FriendRequestData2 = new UIFriends_FriendRequestData();
							uIFriends_FriendRequestData2.GID = gID2;
							uIFriends_FriendRequestData2.url_archive = url_archive2;
							lst_requires.Add(uIFriends_FriendRequestData2);
							GetFriendArchive(uIFriends_FriendRequestData2.GID, uIFriends_FriendRequestData2.url_archive, "requires_" + num2);
						}
					}
					else if (text3 == "count")
					{
						requireCount = list3.Count;
					}
				}
				else if (!(action == actionName_RemoveFriends) && !(action == actionName_RemoveRequires))
				{
				}
			}
		}
		else if (action == actionName_Archive_Get)
		{
			if (response == "{\"code\":1}")
			{
				Debug.LogError("No Archive!!");
				return;
			}
			JsonData jsonData4 = JsonMapper.ToObject<JsonData>(response);
			string nickname = jsonData4["Name"].ToString();
			string text4 = jsonData4["Package"].ToString();
			string text5 = jsonData4["Data"].ToString();
			string[] array3 = text5.Split('^');
			int num3 = 1;
			int[] array4 = new int[3]
			{
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++])
			};
			int[] array5 = new int[7]
			{
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++]),
				int.Parse(array3[num3++])
			};
			string[] array6 = new string[7];
			text4 = text4.Substring(text4.IndexOf(';') + 1);
			string[] array7 = text4.Split(';');
			for (int num4 = 0; num4 < array5.Length; num4++)
			{
				if (array5[num4] < 0)
				{
					array6[num4] = string.Empty;
					continue;
				}
				array6[num4] = array7[array5[num4]];
				if (array6[num4] != string.Empty)
				{
					string[] array8 = array6[num4].Split('^');
					array6[num4] = array8[0];
				}
			}
			string[] array9 = ((string)param).Split('_');
			if (array9[0] == "friends")
			{
				int num5 = int.Parse(array9[1]);
				lst_friends[num5].nickname = nickname;
				lst_friends[num5].url_texture = COMA_ServerManager.Instance.serverAddr_Save;
				lst_friends[num5].TInPack[0] = array4[0];
				lst_friends[num5].TInPack[1] = array4[1];
				lst_friends[num5].TInPack[2] = array4[2];
				lst_friends[num5].accounterments[0] = array6[0];
				lst_friends[num5].accounterments[1] = array6[1];
				lst_friends[num5].accounterments[2] = array6[2];
				lst_friends[num5].accounterments[3] = array6[3];
				lst_friends[num5].accounterments[4] = array6[4];
				lst_friends[num5].accounterments[5] = array6[5];
				lst_friends[num5].accounterments[6] = array6[6];
				if (UIFriends.Instance != null)
				{
					UIFriends.Instance.AddNameFriend(num5, nickname);
				}
				string fileName2 = "Friends/" + lst_friends[num5].GID;
				Debug.Log(response);
				COMA_FileIO.SaveFile(fileName2, response);
			}
			else if (array9[0] == "requires")
			{
				int index = int.Parse(array9[1]);
				lst_requires[index].nickname = nickname;
				lst_requires[index].url_texture = COMA_ServerManager.Instance.serverAddr_Save;
				lst_requires[index].TInPack[0] = array4[0];
				lst_requires[index].TInPack[1] = array4[1];
				lst_requires[index].TInPack[2] = array4[2];
				lst_requires[index].accounterments[0] = array6[0];
				lst_requires[index].accounterments[1] = array6[1];
				lst_requires[index].accounterments[2] = array6[2];
				lst_requires[index].accounterments[3] = array6[3];
				lst_requires[index].accounterments[4] = array6[4];
				lst_requires[index].accounterments[5] = array6[5];
				lst_requires[index].accounterments[6] = array6[6];
				if (UIFriends.Instance != null)
				{
					UIFriends.Instance.OnListRequire();
				}
			}
			HttpClient.Instance().RemoveServer(server);
		}
		else
		{
			if (!(server == serverName_FriendTexture) || !(action == actionName_Tex_Get))
			{
				return;
			}
			Debug.Log(response);
			JsonData jsonData5 = JsonMapper.ToObject<JsonData>(response);
			string content = jsonData5["Content"].ToString();
			Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
			texture2D.LoadImage(COMA_TexBase.Instance.StringToTextureBytes(content));
			texture2D.Apply(false);
			texture2D.filterMode = FilterMode.Point;
			string[] array10 = ((string)param).Split('_');
			if (array10[0] == "friends")
			{
				int num6 = int.Parse(array10[1]);
				int num7 = int.Parse(array10[2]);
				lst_friends[num7].textures[num6] = texture2D;
				if (lst_friends[num7].textures[0] != null && lst_friends[num7].textures[1] != null && lst_friends[num7].textures[2] != null)
				{
					if (UIFriends.Instance != null)
					{
						UIFriends.Instance.OnRefreshFriend(num7);
					}
					string[] array11 = new string[3]
					{
						"Friends/" + Instance.lst_friends[num7].GID + "_0",
						"Friends/" + Instance.lst_friends[num7].GID + "_1",
						"Friends/" + Instance.lst_friends[num7].GID + "_2"
					};
					COMA_FileIO.WritePngData(array11[0], lst_friends[num7].textures[0].EncodeToPNG());
					COMA_FileIO.WritePngData(array11[1], lst_friends[num7].textures[1].EncodeToPNG());
					COMA_FileIO.WritePngData(array11[2], lst_friends[num7].textures[2].EncodeToPNG());
				}
			}
			else if (array10[0] == "requires")
			{
				int num8 = int.Parse(array10[1]);
				int index2 = int.Parse(array10[2]);
				lst_requires[index2].textures[num8] = texture2D;
			}
		}
	}
}
