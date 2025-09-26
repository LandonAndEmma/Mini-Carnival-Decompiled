using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using TNetSdk;
using UIGlobal;
using UnityEngine;

public class COMA_NetworkConnect : MonoBehaviour
{
	private List<TNetRoom> roomList;

	private bool bJoining;

	private bool bGetRoomList;

	private string waitingInfo = "Connecting...";

	public static string sceneName = string.Empty;

	public static int sceneId;

	public static string gameModeName = string.Empty;

	private bool bEnteringFriendsRoom;

	private static COMA_NetworkConnect _instance;

	public bool bNetworkConnectLogin;

	public static COMA_NetworkConnect Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("COMA_NetworkConnect");
				_instance = gameObject.AddComponent<COMA_NetworkConnect>();
				Object.DontDestroyOnLoad(gameObject);
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		sceneName = string.Empty;
		sceneId = 0;
		gameModeName = string.Empty;
		_instance = null;
	}

	public void TryToEnterRoom(string scene)
	{
		if (!bNetworkConnectLogin)
		{
			Debug.LogError("have not login!!");
		}
		bEnteringFriendsRoom = false;
		COMA_CommonOperation.Instance.isCreateRoom = false;
		int num = int.Parse(scene);
		sceneName = COMA_CommonOperation.Instance.SceneIDToName(num);
		sceneId = num;
		gameModeName = TUITextManager.Instance().GetString(UI_GlobalData.Instance._strModeID[num]);
		TryToEnterRoom();
	}

	private void TryToEnterRoom()
	{
		Debug.Log("sceneName : " + sceneName);
		if (COMA_CommonOperation.Instance.IsMode_Castle(sceneName) || COMA_CommonOperation.Instance.IsMode_Flappy(sceneName))
		{
			if (COMA_Platform.Instance != null)
			{
				COMA_Platform.Instance.DestroyPlatform();
			}
			Application.LoadLevel(sceneName);
			COMA_Login.Instance.AddGameModeCount(sceneId, false);
			COMA_CommonOperation.Instance.EnterMode(sceneName);
			return;
		}
		if (COMA_CommonOperation.Instance.IsMode_BlackHouse(sceneName))
		{
			int groupID = COMA_CommonOperation.Instance.GetGroupID(sceneName);
			if (COMA_Network.Instance.TNetInstance != null)
			{
				COMA_Network.Instance.TNetInstance.Send(new GetRoomListRequest(groupID, 0, 10, RoomDragListCmd.ListType.all));
			}
			bJoining = true;
			return;
		}
		if (COMA_CommonOperation.Instance.IsMode_Fishing(sceneName))
		{
			COMA_Login.Instance.AddGameModeCount(sceneId, false);
			COMA_CommonOperation.Instance.EnterMode(sceneName);
		}
		int groupID2 = COMA_CommonOperation.Instance.GetGroupID(sceneName);
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Send(new GetRoomListRequest(groupID2, 0, 10, RoomDragListCmd.ListType.not_full_not_game));
		}
		bJoining = true;
	}

	public void CancelToEnterRoom()
	{
		bJoining = false;
		bGetRoomList = true;
	}

	public void TryToCreateRoom(string scene)
	{
		int num = int.Parse(scene);
		sceneName = COMA_CommonOperation.Instance.SceneIDToName(num);
		sceneId = num;
		gameModeName = TUITextManager.Instance().GetString(UI_GlobalData.Instance._strModeID[num]);
		if (COMA_CommonOperation.Instance.IsMode_Castle(sceneName))
		{
			if (COMA_Platform.Instance != null)
			{
				COMA_Platform.Instance.DestroyPlatform();
			}
			Application.LoadLevel(sceneName);
			COMA_Login.Instance.AddGameModeCount(sceneId, true);
			return;
		}
		COMA_CommonOperation.Instance.isCreateRoom = true;
		if (!bNetworkConnectLogin)
		{
			Debug.LogError("have not login!!");
		}
		Debug.Log("CreateRoom : " + sceneName);
		if (COMA_CommonOperation.Instance.IsMode_Blood(sceneName))
		{
			COMA_Sys.Instance.roadIDs = Random.Range(0, COMA_Blood_SceneController.mapCount).ToString();
		}
		else
		{
			COMA_Sys.Instance.roadIDs = string.Empty;
		}
		Debug.Log("roadIDs : " + COMA_Sys.Instance.roadIDs);
		int groupID = COMA_CommonOperation.Instance.GetGroupID(sceneName);
		if (COMA_Network.Instance.TNetInstance != null)
		{
			string room_name = UIDataBufferCenter.Instance.playerInfo.m_player_id.ToString();
			string roadIDs = COMA_Sys.Instance.roadIDs;
			COMA_Network.Instance.TNetInstance.Send(new CreateRoomRequest(room_name, "triniti", groupID, COMA_CommonOperation.Instance.SceneNameToPlayerCount(sceneName), RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, roadIDs));
		}
	}

	public void TryToEnterFriendsRoom(int id, int roomID)
	{
		string text = COMA_CommonOperation.Instance.SceneIDToName(id);
		bEnteringFriendsRoom = true;
		COMA_CommonOperation.Instance.isCreateRoom = true;
		Debug.Log("Enter room:" + roomID + " Scene:" + text);
		sceneName = text;
		sceneId = COMA_CommonOperation.Instance.SceneNameToID(sceneName);
		COMA_Network.Instance.TNetInstance.Send(new JoinRoomRequest(roomID, "triniti"));
	}

	public void StartConnect(string IP, int PORT)
	{
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.CONNECTION, OnConnect);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.LOGIN, OnLogin);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.GET_ROOM_LIST, OnGetRoomList);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.ROOM_CREATION, OnRoomCreation);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.ROOM_JOIN, OnJoinRoom);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.DISCONNECT, OnDisconnect);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnConnectionLost);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.CONNECTION_KILLED, OnConnectionLost);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectionLost);
			COMA_Network.Instance.TNetInstance.Connect(IP, PORT);
		}
	}

	public void EndConnect()
	{
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Close();
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.CONNECTION, OnConnect);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.LOGIN, OnLogin);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.GET_ROOM_LIST, OnGetRoomList);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.ROOM_CREATION, OnRoomCreation);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.ROOM_JOIN, OnJoinRoom);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.DISCONNECT, OnDisconnect);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnConnectionLost);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.CONNECTION_KILLED, OnConnectionLost);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectionLost);
			COMA_Network.Instance.Disconnect();
		}
	}

	private void OnDestroy()
	{
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Send(new LeaveRoomRequest());
			EndConnect();
		}
	}

	private void Update()
	{
		COMA_Network.Instance.TNetInstance.Update(Time.deltaTime);
		if (!bJoining || !bGetRoomList)
		{
			return;
		}
		string room_name = sceneName.Substring(11);
		bool flag = false;
		if (COMA_CommonOperation.Instance.IsMode_BlackHouse(sceneName))
		{
			if (roomList != null && roomList.Count > 0)
			{
				int num = 0;
				List<TNetRoom> list = new List<TNetRoom>();
				for (int i = 0; i < roomList.Count; i++)
				{
					num += roomList[i].UserCount;
					if (roomList[i].UserCount < roomList[i].MaxUsers)
					{
						list.Add(roomList[i]);
					}
				}
				Debug.LogWarning(sceneName + " Room Count : " + roomList.Count + "  Player Count : " + num);
				float num2 = (float)num / ((float)roomList.Count * 6f);
				Random.seed = num * 54321;
				float num3 = Random.Range(0f, 1f);
				if (!(num3 < num2) && list.Count > 0)
				{
					int index = Random.Range(0, list.Count);
					if (COMA_Network.Instance.TNetInstance != null)
					{
						COMA_Network.Instance.TNetInstance.Send(new JoinRoomRequest(list[index].Id, "0"));
						flag = true;
					}
				}
			}
			if (!flag)
			{
				int groupID = COMA_CommonOperation.Instance.GetGroupID(sceneName);
				if (COMA_Network.Instance.TNetInstance != null)
				{
					string comment = COMA_Sys.Instance.roadIDs.ToString();
					COMA_Network.Instance.TNetInstance.Send(new CreateRoomRequest(room_name, string.Empty, groupID, COMA_CommonOperation.Instance.SceneNameToPlayerCount(sceneName), RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, comment));
				}
			}
		}
		else if (COMA_CommonOperation.Instance.IsMode_Blood(sceneName))
		{
			if (roomList != null && roomList.Count > 0)
			{
				for (int j = 0; j < roomList.Count; j++)
				{
					if (!roomList[j].IsPasswordProtected)
					{
						if (COMA_Network.Instance.TNetInstance != null)
						{
							COMA_Network.Instance.TNetInstance.Send(new JoinRoomRequest(roomList[j].Id, string.Empty));
							flag = true;
						}
						break;
					}
				}
			}
			if (!flag)
			{
				COMA_Sys.Instance.roadIDs = Random.Range(0, COMA_Blood_SceneController.mapCount).ToString();
				Debug.Log("Network : Create Room : " + COMA_Sys.Instance.roadIDs);
				int groupID2 = COMA_CommonOperation.Instance.GetGroupID(sceneName);
				if (COMA_Network.Instance.TNetInstance != null)
				{
					string comment2 = COMA_Sys.Instance.roadIDs.ToString();
					COMA_Network.Instance.TNetInstance.Send(new CreateRoomRequest(room_name, string.Empty, groupID2, COMA_CommonOperation.Instance.SceneNameToPlayerCount(sceneName), RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, comment2));
				}
			}
		}
		else
		{
			COMA_Sys.Instance.roadIDs = string.Empty;
			int i2 = COMA_CommonOperation.Instance.SceneNameToID(sceneName);
			int rankScoreOfSceneID = COMA_Pref.Instance.GetRankScoreOfSceneID(i2);
			if (roomList != null && roomList.Count > 0)
			{
				Debug.LogWarning(sceneName + " Room Count : " + roomList.Count);
				for (int k = 0; k < roomList.Count; k++)
				{
					if (!roomList[k].IsPasswordProtected)
					{
						if (COMA_Network.Instance.TNetInstance != null)
						{
							COMA_Network.Instance.TNetInstance.Send(new JoinRoomRequest(roomList[k].Id, string.Empty));
							flag = true;
						}
						break;
					}
				}
			}
			if (!flag)
			{
				Debug.Log("Network : Create Room : " + rankScoreOfSceneID);
				int groupID3 = COMA_CommonOperation.Instance.GetGroupID(sceneName);
				if (COMA_Network.Instance.TNetInstance != null)
				{
					string comment3 = COMA_Sys.Instance.roadIDs.ToString();
					COMA_Network.Instance.TNetInstance.Send(new CreateRoomRequest(room_name, string.Empty, groupID3, COMA_CommonOperation.Instance.SceneNameToPlayerCount(sceneName), RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, comment3));
				}
			}
		}
		bGetRoomList = false;
	}

	public void BackFromScene()
	{
		Debug.Log("Back");
		COMA_Sys.Instance.bCoverUpdate = false;
		if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
		{
			COMA_CommonOperation.Instance.GamePause(false);
		}
		COMA_CommonOperation.Instance.isEnterWithG = false;
		COMA_CommonOperation.Instance.ExitMode(sceneName);
		sceneName = string.Empty;
		gameModeName = string.Empty;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Send(new LeaveRoomRequest());
		}
		LoadToMainMenu(string.Empty);
		COMA_CommonOperation.Instance.selectedWeaponIndex = 0;
		COMA_CommonOperation.Instance.selectedWeaponPrice = 0;
		COMA_TexLib.Instance.currentRoomPlayerTextures.Clear();
		bJoining = false;
	}

	private void OnConnect(TNetEventData tEvent)
	{
		Debug.Log("OnConnect");
		COMA_Network.Instance.TNetInstance.Send(new LoginRequest(COMA_Pref.Instance.nickname, string.Empty, "COMA"));
	}

	private void OnApplicationQuit()
	{
		Debug.Log("OnApplicationQuit");
	}

	private void OnDisconnect(TNetEventData tEvent)
	{
		COMA_Login.Instance.ReconnectGameServer();
		Debug.Log("OnDisconnect");
		COMA_Network.Instance.Disconnect();
		LoadToMainMenu(string.Empty);
	}

	public void TestConnectionLost()
	{
		OnConnectionLost(null);
	}

	private void OnConnectionLost(TNetEventData tEvent)
	{
		Debug.Log("OnConnectionLost");
		COMA_Network.Instance.Disconnect();
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		COMA_Login.Instance.ReconnectGameServer();
		if (COMA_CommonOperation.Instance.IsMode_Network(Application.loadedLevelName))
		{
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(102);
			uI_MsgBox.AddProceYesHandler(LoadToMainMenu);
		}
	}

	public void LoadToMainMenu(string param)
	{
		if (COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}

	private void OnLogin(TNetEventData tEvent)
	{
		Debug.Log(string.Concat("OnLogin result:", tEvent.data["result"], " OnLogin id:", tEvent.data["userId"], " name:", tEvent.data["nickName"]));
		RoomJoinResCmd.Result result = (RoomJoinResCmd.Result)(int)tEvent.data["result"];
		Debug.Log(result);
		bNetworkConnectLogin = true;
	}

	private void OnGetRoomList(TNetEventData tEvent)
	{
		if (!bGetRoomList)
		{
			Debug.Log(string.Concat("GetRoomList cur:", tEvent.data["curPage"], " pageSum:", tEvent.data["pageSum"]));
			roomList = (List<TNetRoom>)tEvent.data["roomList"];
			bGetRoomList = true;
		}
	}

	private void OnRoomCreation(TNetEventData tEvent)
	{
		Debug.Log(string.Concat("OnRoomCreation result:", tEvent.data["result"], " roomId:", tEvent.data["roomId"]));
	}

	private void OnJoinRoom(TNetEventData tEvent)
	{
		Debug.Log(string.Concat("OnJoinRoom result:", tEvent.data["result"], " sitIndex:", tEvent.data["sitIndex"]));
		RoomJoinResCmd.Result result = (RoomJoinResCmd.Result)(int)tEvent.data["result"];
		Debug.Log(result);
		switch (result)
		{
		case RoomJoinResCmd.Result.ok:
			if (sceneName == "COMA_Scene_Fishing")
			{
				if (COMA_Platform.Instance != null)
				{
					COMA_Platform.Instance.DestroyPlatform();
				}
				COMA_Loading.LoadLevel(sceneName);
			}
			else if (sceneName != null && sceneName != string.Empty)
			{
				if (COMA_CommonOperation.Instance.IsMode_Blood(sceneName))
				{
					COMA_Sys.Instance.roadIDs = COMA_Network.Instance.TNetInstance.CurRoom.Commnet;
				}
				if (COMA_Platform.Instance != null)
				{
					COMA_Platform.Instance.DestroyPlatform();
				}
				COMA_Loading.LoadLevel("COMA_Scene_WaitingRoom");
			}
			break;
		case RoomJoinResCmd.Result.full:
		case RoomJoinResCmd.Result.no_exist:
		case RoomJoinResCmd.Result.gaming:
			if (bEnteringFriendsRoom)
			{
				Debug.LogWarning("Enter room Fail!!");
				switch (result)
				{
				case RoomJoinResCmd.Result.full:
				case RoomJoinResCmd.Result.gaming:
				{
					UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("newroom_desc4"));
					UIGolbalStaticFun.PopCommonMessageBox(data2);
					break;
				}
				case RoomJoinResCmd.Result.no_exist:
				{
					UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("newroom_desc5"));
					UIGolbalStaticFun.PopCommonMessageBox(data);
					break;
				}
				}
			}
			else
			{
				TryToEnterRoom();
			}
			break;
		}
	}
}
