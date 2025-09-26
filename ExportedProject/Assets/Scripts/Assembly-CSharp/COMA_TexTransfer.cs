using System.Collections.Generic;
using MessageID;
using MiscToolKits;
using UnityEngine;

public class COMA_TexTransfer : UIEntity
{
	public class TexNode
	{
		public enum TexState
		{
			Local = 0,
			Uploading = 1,
			Finish = 2
		}

		public ulong upCode;

		public string textureName = string.Empty;

		public string tid = string.Empty;

		public string md5 = string.Empty;

		public TexState state;
	}

	private enum State
	{
		Idle = 0,
		DownloadSave = 1,
		UploadSave = 2,
		UploadTex = 3,
		Finish = 4
	}

	private static COMA_TexTransfer _instance;

	private string fileName_save = COMA_FileNameManager.Instance.GetFileName("SaveTransfer");

	private string fileName_friend = COMA_FileNameManager.Instance.GetFileName("FriendTransfer");

	private string fileName_tex = COMA_FileNameManager.Instance.GetFileName("TexTransfer");

	private char sep = '|';

	private string _save = string.Empty;

	private List<string> _friend = new List<string>();

	private List<TexNode> _tex = new List<TexNode>();

	private List<string> lstTexMD5Uploading = new List<string>();

	private State st;

	public static COMA_TexTransfer Instance
	{
		get
		{
			return _instance;
		}
	}

	private string Friend_ListToString(List<string> lst)
	{
		string text = string.Empty;
		if (lst.Count > 0)
		{
			text = lst[0];
			for (int i = 1; i < lst.Count; i++)
			{
				text = text + sep + lst[i];
			}
		}
		return text;
	}

	private List<string> Friend_StringToList(string cnt)
	{
		List<string> list = new List<string>();
		if (cnt != string.Empty)
		{
			string[] array = cnt.Split(sep);
			string[] array2 = array;
			foreach (string item in array2)
			{
				list.Add(item);
			}
		}
		return list;
	}

	private string Tex_ListToString(List<TexNode> lst)
	{
		string text = string.Empty;
		if (lst.Count > 0)
		{
			text = lst[0].upCode.ToString() + "," + lst[0].textureName + "," + lst[0].tid + "," + lst[0].md5 + "," + (int)lst[0].state;
			for (int i = 1; i < lst.Count; i++)
			{
				string text2 = text;
				text = text2 + sep + lst[i].upCode.ToString() + "," + lst[i].textureName + "," + lst[i].tid + "," + lst[i].md5 + "," + (int)lst[0].state;
			}
		}
		Debug.LogWarning(text);
		return text;
	}

	private List<TexNode> Tex_StringToList(string cnt)
	{
		List<TexNode> list = new List<TexNode>();
		if (cnt != string.Empty)
		{
			string[] array = cnt.Split(sep);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(',');
				TexNode texNode = new TexNode();
				texNode.upCode = ulong.Parse(array3[0]);
				texNode.textureName = array3[1];
				texNode.tid = array3[2];
				texNode.md5 = array3[3];
				texNode.state = (TexNode.TexState)(0 + int.Parse(array3[4]));
				list.Add(texNode);
			}
		}
		return list;
	}

	public TexNode GetTexNodeByTextureName(string textureName)
	{
		foreach (TexNode item in _tex)
		{
			if (item.textureName == textureName)
			{
				return item;
			}
		}
		return null;
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogError("2 or nore Instance!!");
		}
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void ClearTempFiles()
	{
		COMA_FileIO.DeleteFile(fileName_save);
		COMA_FileIO.DeleteFile(fileName_friend);
		COMA_FileIO.DeleteFile(fileName_tex);
	}

	private void Start()
	{
		Debug.Log("Check Transfer!!");
		COMA_Login.Instance.ChangeConnectingTip(COMA_Login.Instance.tip_downloadContent);
		SceneTimerInstance.Instance.Add(60f, COMA_Login.Instance.Tip_NeedDownLoadSupport);
		_save = COMA_FileIO.LoadFile(fileName_save);
		_friend = Friend_StringToList(COMA_FileIO.LoadFile(fileName_friend));
		string text = COMA_FileIO.LoadFile(fileName_tex);
		_tex = Tex_StringToList(text);
		if (_save == string.Empty || text == string.Empty)
		{
			COMA_ServerManager.Instance.deliverSvrAddr = "http://192.225.224.102:9199/gameapi/comavataraccount.do";
			COMA_Server_Account.Instance.InitServer(OnGetServerAccount, COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
			COMA_Server_Account.Instance.DeliverGame(COMA_Server_ID.Instance.GID, GameCenter.Instance.gameCenterID, COMA_Server_ID.Instance.NID, string.Empty);
			COMA_Server_Friends.Instance.InitServer(COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
			COMA_Server_Friends.Instance.FriendList(COMA_Server_ID.Instance.GID);
		}
		else
		{
			COMA_ServerManager.Instance.serverAddr_Save = "http://192.225.224.2:888/gameapi/comavatartrade_no.do";
			COMA_Server_Texture.Instance.InitServer(COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
			Debug.Log("处理贴图");
			COMA_Pref.Instance.contentForServerSave = _save;
			SceneTimerInstance.Instance.Add(1f, CheckTexDownloadFinish);
		}
	}

	public void OnGetServerAccount(bool bTimeOut, bool bNoPref)
	{
		Debug.Log("OnGetServerAccount() : " + bTimeOut + " " + bNoPref);
		if (bTimeOut)
		{
			COMA_Server_Account.Instance.DeliverGame(COMA_Server_ID.Instance.GID, GameCenter.Instance.gameCenterID, COMA_Server_ID.Instance.NID, string.Empty);
			return;
		}
		COMA_Server_Archive.Instance.InitServer(OnGetServerArchive_Get, COMA_ServerManager.Instance.saverSvrAddr, COMA_ServerManager.Instance.saverSvrOutTime, COMA_ServerManager.Instance.saverSvrKey);
		COMA_Server_Texture.Instance.InitServer(COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
		if (bNoPref)
		{
			Debug.Log("-- no server save!!");
			CreateNewArchive();
		}
		else
		{
			Debug.Log("!!拉取存档!!");
			COMA_Server_Archive.Instance.PlayerPref_Get(COMA_Server_ID.Instance.GID);
		}
	}

	public void OnGetServerArchive_Get(bool bTimeOut, string content)
	{
		Debug.Log("OnGetServerArchive_Get() : " + bTimeOut + " " + content);
		if (bTimeOut)
		{
			COMA_Server_Archive.Instance.PlayerPref_Get(COMA_Server_ID.Instance.GID);
			return;
		}
		if (content == string.Empty)
		{
			Debug.Log("-- server save is null!!");
			CreateNewArchive();
			return;
		}
		Debug.Log("写入存档");
		_save = content;
		COMA_FileIO.SaveFile(fileName_save, _save);
		Debug.Log("处理贴图");
		COMA_Pref.Instance.contentForServerSave = content;
		if (MiscStaticTools.SF_IsPureNumber(COMA_Pref.Instance.nickname))
		{
			COMA_Pref.Instance.nickname = "P" + COMA_Pref.Instance.nickname;
			if (COMA_Pref.Instance.nickname.Length > 10)
			{
				COMA_Pref.Instance.nickname = COMA_Pref.Instance.nickname.Substring(0, 10);
			}
			COMA_FileIO.SaveFile(fileName_save, COMA_Pref.Instance.contentForServerSave);
		}
		SceneTimerInstance.Instance.Add(1f, CheckTexDownloadFinish);
		Debug.Log("写入待传贴图");
		ulong num = 1uL;
		COMA_PackageItem[] pack = COMA_Pref.Instance.package.pack;
		foreach (COMA_PackageItem cOMA_PackageItem in pack)
		{
			if (cOMA_PackageItem != null && cOMA_PackageItem.textureName != string.Empty)
			{
				Debug.Log(cOMA_PackageItem.serialName + " " + cOMA_PackageItem.tid + " " + cOMA_PackageItem.textureName + " " + num);
				TexNode texNode = new TexNode();
				texNode.upCode = num;
				texNode.textureName = cOMA_PackageItem.textureName;
				texNode.tid = cOMA_PackageItem.tid;
				texNode.md5 = string.Empty;
				texNode.state = TexNode.TexState.Local;
				_tex.Add(texNode);
				num++;
			}
		}
		COMA_FileIO.SaveFile(fileName_tex, Tex_ListToString(_tex));
	}

	public bool CheckTexDownloadFinish()
	{
		COMA_PackageItem[] pack = COMA_Pref.Instance.package.pack;
		foreach (COMA_PackageItem cOMA_PackageItem in pack)
		{
			if (cOMA_PackageItem == null || !(cOMA_PackageItem.textureName != string.Empty) || !(cOMA_PackageItem.texture != null))
			{
				continue;
			}
			TexNode node = GetTexNodeByTextureName(cOMA_PackageItem.textureName);
			if (node == null || node.state != TexNode.TexState.Local)
			{
				continue;
			}
			string fileMD = COMA_FileIO.GetFileMD5(cOMA_PackageItem.texture.EncodeToPNG());
			if (lstTexMD5Uploading.Contains(fileMD))
			{
				node.state = TexNode.TexState.Finish;
				continue;
			}
			switch (fileMD)
			{
			case "bfbe092a8f68fabfe83afbc5e961487d":
				node.md5 = "bfbe092a8f68fabfe83afbc5e961487d";
				node.state = TexNode.TexState.Finish;
				continue;
			case "6ba2377776d6c137ee29551baff81bb5":
				node.md5 = "6ba2377776d6c137ee29551baff81bb5";
				node.state = TexNode.TexState.Finish;
				continue;
			case "54245d0a0b0c5c8305976247da71f59f":
				node.md5 = "54245d0a0b0c5c8305976247da71f59f";
				node.state = TexNode.TexState.Finish;
				continue;
			case "9a53aef61db65e1ed1298fca0cc15a3d":
				node.md5 = "9a53aef61db65e1ed1298fca0cc15a3d";
				node.state = TexNode.TexState.Finish;
				continue;
			}
			Debug.LogWarning("Upload Texture - tid:" + cOMA_PackageItem.tid + " name:" + cOMA_PackageItem.textureName + " upCode:" + node.upCode);
			UIDataBufferCenter.Instance.UploadFile(node.upCode, cOMA_PackageItem.texture.EncodeToPNG(), delegate(string md5)
			{
				Debug.LogWarning("code:" + node.upCode + "    md5:" + md5);
				node.md5 = md5;
				node.state = TexNode.TexState.Finish;
				COMA_FileIO.SaveFile(fileName_tex, Tex_ListToString(_tex));
			});
			node.state = TexNode.TexState.Uploading;
			lstTexMD5Uploading.Add(fileMD);
			Debug.Log(node.upCode + ":" + node.md5);
		}
		COMA_FileIO.SaveFile(fileName_tex, Tex_ListToString(_tex));
		foreach (TexNode item in _tex)
		{
			if (item.state != TexNode.TexState.Finish)
			{
				return true;
			}
			Debug.Log("Tex Success : " + item.upCode + " " + item.textureName + " " + item.tid + " " + item.md5);
		}
		Debug.LogWarning("完成!!");
		UploadArchive();
		return false;
	}

	public void SaveFriends(List<UIFriends_FriendRequestData> lst)
	{
		for (int i = 0; i < lst.Count; i++)
		{
			_friend.Add(lst[i].GID);
		}
		Debug.LogWarning("save friend data!!");
		COMA_FileIO.SaveFile(fileName_friend, Friend_ListToString(_friend));
	}

	public List<string> GetFriends()
	{
		return _friend;
	}

	private void CreateNewArchive()
	{
		SceneTimerInstance.Instance.Remove(COMA_Login.Instance.Tip_NeedDownLoadSupport);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ArchiveCreate, null, null);
	}

	private void UploadArchive()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ArchiveUpload, null, null);
	}
}
