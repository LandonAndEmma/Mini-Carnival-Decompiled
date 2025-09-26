using System;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class COMA_Scene : MonoBehaviour
{
	private static COMA_Scene _instance;

	[NonSerialized]
	public bool runingGameOver;

	private bool bCalScore;

	public GameObject playerPrefab;

	public GameObject playerSyncPrefab;

	public Transform playerNodeTrs;

	public COMA_Camera cmrMoveCom;

	public int teamsNum = 1;

	public UI_GameStartMgr startCom;

	public bool bInfinityMode;

	public UIInGame_BoxMagazineMgr magazineCom;

	public UIInGame_SettlementMgr settlementCom;

	public int gameTime = 180;

	private GameObject playerSelfObj;

	public Transform[] bornPoint;

	private List<COMA_Creation> deathList = new List<COMA_Creation>();

	private bool alreadyStart;

	public UIIngame_WaitingOtherPlayers waittingforReadyCom;

	private bool bLeaveRoom;

	public int[] teamScore = new int[2];

	public static COMA_Scene Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_CREATE, CreatePlayer);
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnLeaveRoomNotify);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.ROOM_GO, ReadyToGo);
		}
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.GAME_START, ForceToStart);
	}

	private void OnDisable()
	{
		OpenClikPlugin.Hide();
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_CREATE, CreatePlayer);
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.USER_EXIT_ROOM, OnLeaveRoomNotify);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.ROOM_GO, ReadyToGo);
		}
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.GAME_START, ForceToStart);
	}

	private void OnLeaveRoomNotify(TNetEventData tEvent)
	{
		bLeaveRoom = true;
		TNetUser tNetUser = (TNetUser)tEvent.data["user"];
		Debug.Log("OnLeaveRoomNotify userId : " + tNetUser.Id);
		Transform transform = playerNodeTrs.FindChild(tNetUser.Id.ToString());
		if (transform != null && COMA_CommonOperation.Instance.IsMode_Maze(Application.loadedLevelName))
		{
			COMA_PlayerSync_Maze component = transform.GetComponent<COMA_PlayerSync_Maze>();
			if (component != null && component.bEvil)
			{
				COMA_PlayerSync_Maze[] componentsInChildren = playerNodeTrs.GetComponentsInChildren<COMA_PlayerSync_Maze>();
				int i;
				for (i = 0; i < componentsInChildren.Length && (componentsInChildren[i] == component || componentsInChildren[i].id >= COMA_PlayerSelf.Instance.id); i++)
				{
				}
				if (i >= componentsInChildren.Length)
				{
					COMA_Maze_SceneController.Instance.CreateEvilItem();
				}
			}
		}
		if (transform != null && !COMA_CommonOperation.Instance.IsMode_Run(Application.loadedLevelName))
		{
			TBaseEntity component2 = transform.GetComponent<TBaseEntity>();
			if (component2 != null)
			{
				component2.DumRes();
			}
			UnityEngine.Object.DestroyObject(transform.gameObject);
			if (COMA_CommonOperation.Instance.IsMode_Tank(Application.loadedLevelName))
			{
				COMA_PlayerSync_Tank component3 = transform.GetComponent<COMA_PlayerSync_Tank>();
				COMA_Tank_SceneController.Instance.RemovePlayer(component3);
			}
		}
	}

	private void Start()
	{
		bLeaveRoom = false;
		if (!COMA_CommonOperation.Instance.IsWaittingRoom(Application.loadedLevelName) && COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
		if (COMA_Network.Instance.TNetInstance != null && !COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
		{
			COMA_CD_PlayerCreate cOMA_CD_PlayerCreate = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_CREATE) as COMA_CD_PlayerCreate;
			cOMA_CD_PlayerCreate.GID = COMA_Server_ID.Instance.GID;
			Debug.Log("===========================================>>>>>commandCreatePlayer.GID=" + cOMA_CD_PlayerCreate.GID);
			int sitIndex = COMA_Network.Instance.TNetInstance.Myself.SitIndex;
			cOMA_CD_PlayerCreate.position = bornPoint[sitIndex].position;
			cOMA_CD_PlayerCreate.rotation = bornPoint[sitIndex].rotation;
			cOMA_CD_PlayerCreate.nickname = UIDataBufferCenter.Instance.playerInfo.m_name;
			cOMA_CD_PlayerCreate.lv = (int)UIDataBufferCenter.Instance.playerInfo.m_level;
			cOMA_CD_PlayerCreate.exp = COMA_Pref.Instance.exp;
			cOMA_CD_PlayerCreate.rankscore = COMA_Pref.Instance.GetRankScoreOfCurrentScene();
			cOMA_CD_PlayerCreate.texUUID[0] = COMA_Pref.Instance.TID[0];
			cOMA_CD_PlayerCreate.texUUID[1] = COMA_Pref.Instance.TID[1];
			cOMA_CD_PlayerCreate.texUUID[2] = COMA_Pref.Instance.TID[2];
			cOMA_CD_PlayerCreate.accSerial[0] = ((COMA_Pref.Instance.AInPack[0] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[0]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[1] = ((COMA_Pref.Instance.AInPack[1] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[1]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[2] = ((COMA_Pref.Instance.AInPack[2] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[2]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[3] = ((COMA_Pref.Instance.AInPack[3] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[3]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[4] = ((COMA_Pref.Instance.AInPack[4] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[4]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[5] = ((COMA_Pref.Instance.AInPack[5] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[5]].serialName : " ");
			cOMA_CD_PlayerCreate.accSerial[6] = ((COMA_Pref.Instance.AInPack[6] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[6]].serialName : " ");
			cOMA_CD_PlayerCreate.itemSelected = COMA_CommonOperation.Instance.selectedWeaponIndex;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerCreate);
			if (Application.loadedLevelName == "COMA_Scene_WaitingRoom" || Application.loadedLevelName == "COMA_Scene_Fishing")
			{
				COMA_CD_PlayerTextureInit cOMA_CD_PlayerTextureInit = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_TEXTUREINIT) as COMA_CD_PlayerTextureInit;
				cOMA_CD_PlayerTextureInit.texUUID[0] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].serialName;
				cOMA_CD_PlayerTextureInit.texUUID[1] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].serialName;
				cOMA_CD_PlayerTextureInit.texUUID[2] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].serialName;
				Debug.Log(COMA_Pref.Instance.TInPack[0]);
				Debug.Log(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].serialName);
				cOMA_CD_PlayerTextureInit.texStr[0] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].texture.EncodeToPNG());
				cOMA_CD_PlayerTextureInit.texStr[1] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].texture.EncodeToPNG());
				cOMA_CD_PlayerTextureInit.texStr[2] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].texture.EncodeToPNG());
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerTextureInit);
			}
		}
		COMA_CommonOperation.Instance.otherRankScores.Clear();
		Resources.UnloadUnusedAssets();
	}

	public void CountToStart(int mode)
	{
		startCom.ShowGameModel(mode);
		if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
		{
			AllReadyAndCountDown();
		}
		else if (Application.loadedLevelName != "COMA_Scene_WaitingRoom")
		{
			COMA_Network.Instance.ReadyGame();
			if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName) || COMA_CommonOperation.Instance.IsMode_Maze(Application.loadedLevelName) || COMA_CommonOperation.Instance.IsMode_Flag(Application.loadedLevelName))
			{
				AllReadyAndCountDown();
			}
			else
			{
				waittingforReadyCom.StartCountDown(30f);
			}
		}
	}

	private void ReadyToGo(TNetEventData tEvent)
	{
		waittingforReadyCom.ForceEndCountDown();
		AllReadyAndCountDown();
		SceneTimerInstance.Instance.Remove(AllReadyAndCountDown);
	}

	public bool AllReadyAndCountDown()
	{
		if (alreadyStart)
		{
			return false;
		}
		alreadyStart = true;
		COMA_Sys.Instance.bGameCounting = true;
		startCom.EnterGameModel();
		if (COMA_Sys.Instance.bIgnoreGameCount)
		{
			startCom.ForceToStartGameForSync();
		}
		return false;
	}

	public void ReadyToPlay()
	{
		COMA_CD_GameStart commandDatas = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.GAME_START) as COMA_CD_GameStart;
		COMA_CommandHandler.Instance.Send(commandDatas);
	}

	private void ForceToStart(COMA_CommandDatas commandDatas)
	{
		COMA_Sys.Instance.bIgnoreGameCount = true;
		if (COMA_Sys.Instance.bGameCounting)
		{
			startCom.ForceToStartGameForSync();
		}
	}

	private void Update()
	{
	}

	private void CreatePlayer(COMA_CommandDatas commandDatas)
	{
		Debug.Log("[==CreatePlayer==] : " + commandDatas.dataSender.Id);
		COMA_Sys.Instance.playerIDToSeatIndex[commandDatas.dataSender.Id] = commandDatas.dataSender.SitIndex;
		COMA_CD_PlayerCreate cOMA_CD_PlayerCreate = commandDatas as COMA_CD_PlayerCreate;
		if (cOMA_CD_PlayerCreate.dataSender.IsItMe)
		{
			string text = cOMA_CD_PlayerCreate.dataSender.Id.ToString();
			Debug.Log("----------CreatePlayer self-----------  :" + text);
			if (playerNodeTrs.FindChild(text) == null)
			{
				playerSelfObj = UnityEngine.Object.Instantiate(playerPrefab, cOMA_CD_PlayerCreate.position, cOMA_CD_PlayerCreate.rotation) as GameObject;
				playerSelfObj.name = text;
				playerSelfObj.transform.parent = playerNodeTrs;
				if (cmrMoveCom != null)
				{
					cmrMoveCom.CameraInit(playerSelfObj.transform);
				}
				Debug.Log("================================================>gid=" + cOMA_CD_PlayerCreate.GID);
				COMA_PlayerSelf.Instance.gid = cOMA_CD_PlayerCreate.GID;
				COMA_PlayerSelf.Instance.id = int.Parse(text);
				COMA_PlayerSelf.Instance.sitIndex = cOMA_CD_PlayerCreate.dataSender.SitIndex;
				COMA_PlayerSelf.Instance.bornPointTrs = bornPoint[cOMA_CD_PlayerCreate.dataSender.SitIndex];
				COMA_PlayerSelf.Instance.nickname = cOMA_CD_PlayerCreate.nickname;
				COMA_PlayerSelf.Instance.lv = cOMA_CD_PlayerCreate.lv;
				COMA_PlayerSelf.Instance.exp = cOMA_CD_PlayerCreate.exp;
				COMA_PlayerSelf.Instance.rankscore = cOMA_CD_PlayerCreate.rankscore;
				COMA_PlayerSelf.Instance.itemSelected = cOMA_CD_PlayerCreate.itemSelected;
				COMA_PlayerSelf.Instance.team = (Team)(1 + COMA_PlayerSelf.Instance.sitIndex % teamsNum);
				COMA_CommonOperation.Instance.otherRankScores.Add(cOMA_CD_PlayerCreate.rankscore);
				if (UI_3DModeToTUIMgr.Instance != null)
				{
					COMA_PlayerSelf.Instance.rt3DObj = UI_3DModeToTUIMgr.Instance.sourceObjs[cOMA_CD_PlayerCreate.dataSender.SitIndex];
				}
			}
		}
		else
		{
			string text2 = cOMA_CD_PlayerCreate.dataSender.Id.ToString();
			Debug.Log("----------CreatePlayer other-----------  :" + text2);
			if (playerNodeTrs.FindChild(text2) == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(playerSyncPrefab, cOMA_CD_PlayerCreate.position, cOMA_CD_PlayerCreate.rotation) as GameObject;
				gameObject.name = text2;
				gameObject.transform.parent = playerNodeTrs;
				COMA_PlayerSync component = gameObject.GetComponent<COMA_PlayerSync>();
				component.gid = cOMA_CD_PlayerCreate.GID;
				component.id = int.Parse(text2);
				component.sitIndex = cOMA_CD_PlayerCreate.dataSender.SitIndex;
				component.nickname = cOMA_CD_PlayerCreate.nickname;
				component.lv = cOMA_CD_PlayerCreate.lv;
				component.exp = cOMA_CD_PlayerCreate.exp;
				component.rankscore = cOMA_CD_PlayerCreate.rankscore;
				component.itemSelected = cOMA_CD_PlayerCreate.itemSelected;
				component.team = (Team)(1 + component.sitIndex % teamsNum);
				COMA_CommonOperation.Instance.otherRankScores.Add(cOMA_CD_PlayerCreate.rankscore);
				if (UI_3DModeToTUIMgr.Instance != null)
				{
					component.rt3DObj = UI_3DModeToTUIMgr.Instance.sourceObjs[cOMA_CD_PlayerCreate.dataSender.SitIndex];
				}
				component.syncTID[0] = cOMA_CD_PlayerCreate.texUUID[0];
				component.syncTID[1] = cOMA_CD_PlayerCreate.texUUID[1];
				component.syncTID[2] = cOMA_CD_PlayerCreate.texUUID[2];
				component.TryToPutAvatar();
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[0]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[1]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[2]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[3]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[4]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[5]);
				component.characterCom.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[6]);
				if (UI_3DModeToTUIMgr.Instance != null)
				{
					COMA_PlayerCharacter component2 = UI_3DModeToTUIMgr.Instance.sourceObjs[component.sitIndex].GetComponent<COMA_PlayerCharacter>();
					for (int i = 0; i < cOMA_CD_PlayerCreate.accSerial.Length; i++)
					{
						component2.CreateAccouterment(cOMA_CD_PlayerCreate.accSerial[i], "Player");
					}
				}
				if (cOMA_CD_PlayerCreate.bNeedRecreate == 1)
				{
					COMA_CD_PlayerCreate cOMA_CD_PlayerCreate2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_CREATE) as COMA_CD_PlayerCreate;
					cOMA_CD_PlayerCreate2.GID = COMA_Server_ID.Instance.GID;
					cOMA_CD_PlayerCreate2.bNeedRecreate = 0;
					int sitIndex = COMA_Network.Instance.TNetInstance.Myself.SitIndex;
					cOMA_CD_PlayerCreate2.position = ((!(playerSelfObj == null)) ? playerSelfObj.transform.position : bornPoint[sitIndex].position);
					cOMA_CD_PlayerCreate2.rotation = ((!(playerSelfObj == null)) ? playerSelfObj.transform.rotation : bornPoint[sitIndex].rotation);
					cOMA_CD_PlayerCreate2.nickname = COMA_Pref.Instance.nickname;
					cOMA_CD_PlayerCreate2.lv = COMA_Pref.Instance.lv;
					cOMA_CD_PlayerCreate2.exp = COMA_Pref.Instance.exp;
					cOMA_CD_PlayerCreate2.rankscore = COMA_Pref.Instance.GetRankScoreOfCurrentScene();
					cOMA_CD_PlayerCreate2.texUUID[0] = COMA_Pref.Instance.TID[0];
					cOMA_CD_PlayerCreate2.texUUID[1] = COMA_Pref.Instance.TID[1];
					cOMA_CD_PlayerCreate2.texUUID[2] = COMA_Pref.Instance.TID[2];
					cOMA_CD_PlayerCreate2.accSerial[0] = ((COMA_Pref.Instance.AInPack[0] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[0]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[1] = ((COMA_Pref.Instance.AInPack[1] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[1]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[2] = ((COMA_Pref.Instance.AInPack[2] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[2]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[3] = ((COMA_Pref.Instance.AInPack[3] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[3]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[4] = ((COMA_Pref.Instance.AInPack[4] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[4]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[5] = ((COMA_Pref.Instance.AInPack[5] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[5]].serialName : " ");
					cOMA_CD_PlayerCreate2.accSerial[6] = ((COMA_Pref.Instance.AInPack[6] >= 0) ? COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[6]].serialName : " ");
					cOMA_CD_PlayerCreate2.itemSelected = COMA_CommonOperation.Instance.selectedWeaponIndex;
					COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerCreate2, commandDatas.dataSender.Id);
					if (Application.loadedLevelName == "COMA_Scene_WaitingRoom" || Application.loadedLevelName == "COMA_Scene_Fishing")
					{
						COMA_CD_PlayerTextureInit cOMA_CD_PlayerTextureInit = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_TEXTUREINIT) as COMA_CD_PlayerTextureInit;
						cOMA_CD_PlayerTextureInit.texUUID[0] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].serialName;
						cOMA_CD_PlayerTextureInit.texUUID[1] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].serialName;
						cOMA_CD_PlayerTextureInit.texUUID[2] = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].serialName;
						cOMA_CD_PlayerTextureInit.texStr[0] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].texture.EncodeToPNG());
						cOMA_CD_PlayerTextureInit.texStr[1] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].texture.EncodeToPNG());
						cOMA_CD_PlayerTextureInit.texStr[2] = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].texture.EncodeToPNG());
						Debug.Log(cOMA_CD_PlayerTextureInit.texStr[0]);
						COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerTextureInit, commandDatas.dataSender.Id);
					}
					if (Application.loadedLevelName == "COMA_Scene_Fishing")
					{
						COMA_CD_FishingState cOMA_CD_FishingState = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_STATE) as COMA_CD_FishingState;
						if (COMA_PlayerSelf.Instance == null)
						{
							cOMA_CD_FishingState.nState = 0;
						}
						else
						{
							int childSysTypeByTag = COMA_PlayerSelf.Instance.GetCenterController().GetChildSysTypeByTag(0);
							TFishingPlayerBuffController tFishingPlayerBuffController = COMA_PlayerSelf.Instance.GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
							if (tFishingPlayerBuffController.IsExitBuff(1))
							{
								cOMA_CD_FishingState.nState = 1;
								cOMA_CD_FishingState.nFishBoatID = ((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).GetOnBoatId();
								cOMA_CD_FishingState.fOnBoatDurTime = Time.time - COMA_Fishing_SceneController.Instance._fOnBoatTimes[cOMA_CD_FishingState.nFishBoatID - 1];
							}
							else
							{
								cOMA_CD_FishingState.nState = 0;
							}
						}
						COMA_CommandHandler.Instance.Send(cOMA_CD_FishingState, commandDatas.dataSender.Id);
					}
				}
			}
		}
		if (UIInGame_PlayersUIMgr.Instance != null)
		{
			UIInGame_PlayersUIMgr.Instance._testData[commandDatas.dataSender.SitIndex].Name = cOMA_CD_PlayerCreate.nickname;
			UIInGame_PlayersUIMgr.Instance._testData[commandDatas.dataSender.SitIndex].Num = 0;
			UIInGame_PlayersUIMgr.Instance._testData[commandDatas.dataSender.SitIndex].HP = 1f;
		}
		if (UIInGameHungers_PlayersMgr.Instance != null)
		{
			UIInGameHungers_PlayersMgr.Instance._testData[commandDatas.dataSender.SitIndex].Name = cOMA_CD_PlayerCreate.nickname;
			UIInGameHungers_PlayersMgr.Instance._testData[commandDatas.dataSender.SitIndex].HP = 1f;
			if (cOMA_CD_PlayerCreate.dataSender.IsItMe)
			{
				UIInGameHungers_PlayersMgr.Instance._testData[commandDatas.dataSender.SitIndex].IsSelf = true;
			}
			else
			{
				UIInGameHungers_PlayersMgr.Instance._testData[commandDatas.dataSender.SitIndex].IsSelf = false;
			}
		}
		if (UIInGame_LabyrinthUIMgr.Instance != null)
		{
			UIInGame_LabyrinthUIMgr.Instance._testData[commandDatas.dataSender.SitIndex].HP = 1f;
			UIInGame_LabyrinthUIMgr.Instance._testData[commandDatas.dataSender.SitIndex].GoldNum = 0;
		}
	}

	public void AddToDeathList(COMA_Creation com)
	{
		deathList.Add(com);
		com.transform.parent = null;
	}

	public void AddToDeathList(COMA_Creation com, int index)
	{
		deathList.Insert(index, com);
		com.transform.parent = null;
	}

	public bool IsGameOver()
	{
		if (playerNodeTrs.childCount <= 1 && (deathList.Count >= 1 || bLeaveRoom))
		{
			return true;
		}
		return false;
	}

	public void GameFinishBySurvive()
	{
		COMA_Sys.Instance.bCoverUIInput = true;
		bool flag = true;
		if (COMA_CommonOperation.Instance.IsMode_Hunger(Application.loadedLevelName))
		{
			if (!COMA_PlayerSelf.Instance.IsDead)
			{
				deathList.Add(COMA_PlayerSelf.Instance);
			}
			else if (playerNodeTrs.childCount >= 4)
			{
				flag = false;
				while (deathList.Count < 4 && playerNodeTrs.childCount > 0)
				{
					int index = UnityEngine.Random.Range(0, playerNodeTrs.childCount);
					Transform child = playerNodeTrs.GetChild(index);
					child.parent = null;
					COMA_Creation component = child.GetComponent<COMA_Creation>();
					deathList.Add(component);
				}
			}
			else
			{
				flag = true;
				for (int num = deathList.Count - 1; num >= 0; num--)
				{
					if (deathList[num].id == COMA_PlayerSelf.Instance.id)
					{
						deathList.RemoveAt(num);
					}
				}
				deathList.Add(COMA_PlayerSelf.Instance);
				Debug.Log("-------------------------------><><><><><><>--------------------------------");
				for (int num2 = playerNodeTrs.childCount - 1; num2 >= 0; num2--)
				{
					Transform child2 = playerNodeTrs.GetChild(num2);
					child2.parent = null;
					COMA_Creation component2 = child2.GetComponent<COMA_Creation>();
					deathList.Add(component2);
				}
				while (deathList.Count > 4)
				{
					deathList.RemoveAt(0);
				}
			}
		}
		else if (playerNodeTrs.childCount > 0)
		{
			Transform child3 = playerNodeTrs.GetChild(0);
			COMA_Creation component3 = child3.GetComponent<COMA_Creation>();
			deathList.Add(component3);
		}
		deathList.Reverse();
		while (deathList.Count > 4)
		{
			int index2 = deathList.Count - 1;
			deathList.RemoveAt(index2);
		}
		if (flag && COMA_CommonOperation.Instance.IsMode_Hunger(COMA_NetworkConnect.sceneName) && COMA_Network.Instance.IsMyself(deathList[0].id))
		{
			if (deathList[0].score <= 0)
			{
				COMA_Achievement.Instance.Lucky++;
			}
			else if (deathList[0].score >= 7)
			{
				COMA_Achievement.Instance.Baddy++;
			}
		}
		if (flag && COMA_CommonOperation.Instance.IsMode_Rocket(COMA_NetworkConnect.sceneName) && COMA_Network.Instance.IsMyself(deathList[0].id) && COMA_DropFight_SceneController.Instance.GetLeftBlockCount() <= 10)
		{
			COMA_Achievement.Instance.Escaper++;
		}
		if (flag)
		{
			UI_3DModeToTUIMgr.Instance.ptlFlowerObj.transform.position = deathList[0].rt3DObj.transform.position;
		}
		UISettlementInfo[] array = new UISettlementInfo[deathList.Count];
		for (int i = 0; i < deathList.Count; i++)
		{
			int rank = ((!flag) ? (i + 4) : i);
			COMA_Creation cOMA_Creation = deathList[i];
			int lv = cOMA_Creation.lv;
			int score = cOMA_Creation.score;
			cOMA_Creation.expGet = COMA_CommonOperation.Instance.CalExp(COMA_NetworkConnect.sceneName, rank, score, lv);
			cOMA_Creation.goldGet = COMA_CommonOperation.Instance.CalGold(COMA_NetworkConnect.sceneName, rank, score, cOMA_Creation.goldGet, lv);
			cOMA_Creation.crystalGet = 0;
			float num3 = (float)cOMA_Creation.exp / (float)COMA_Pref.Instance.expLv[lv - 1];
			float num4 = 0f;
			if (lv < COMA_Pref.Instance.lvMax)
			{
				if (cOMA_Creation.exp + cOMA_Creation.expGet < COMA_Pref.Instance.expLv[lv - 1])
				{
					num4 = (float)cOMA_Creation.expGet / (float)COMA_Pref.Instance.expLv[lv - 1];
				}
				else
				{
					num4 = 1f - num3;
					float num5 = cOMA_Creation.expGet - (COMA_Pref.Instance.expLv[lv - 1] - cOMA_Creation.exp);
					int num6 = lv + 1;
					while (num5 > (float)COMA_Pref.Instance.expLv[num6 - 1] && num6 < COMA_Pref.Instance.lvMax)
					{
						num5 -= (float)COMA_Pref.Instance.expLv[num6 - 1];
						num6++;
						num4 += 1f;
					}
					if (num6 < COMA_Pref.Instance.lvMax)
					{
						num4 += num5 / (float)COMA_Pref.Instance.expLv[num6 - 1];
					}
				}
			}
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && num3 + num4 > 1f)
			{
				settlementCom.lvUp = lv + 1;
			}
			int num7 = COMA_CommonOperation.Instance.CalScore(COMA_NetworkConnect.sceneName, rank, COMA_CommonOperation.Instance.GetRankScoreAverage(), cOMA_Creation.rankscore);
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && !bCalScore)
			{
				bCalScore = true;
				COMA_Pref.Instance.AddRankScoreOfCurrentScene(num7);
			}
			array[i] = new UISettlementInfo(cOMA_Creation.nickname, num7, cOMA_Creation.expGet, cOMA_Creation.goldGet, cOMA_Creation.crystalGet, lv, num3, num4);
			array[i].Tex2D = cOMA_Creation.rt3DObj.transform.parent.camera.targetTexture;
		}
		settlementCom.gameObject.SetActive(true);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_ScoreCheck);
		settlementCom.Init(array, flag);
		if (flag)
		{
			deathList[0].rt3DObj.animation.Play("Win");
		}
		if (deathList.Count >= 2)
		{
			deathList[deathList.Count - 1].rt3DObj.animation.Play("Lose");
			deathList[deathList.Count - 1].rt3DObj.animation.PlayQueued("Lose_idle");
		}
		LocalSettlement();
	}

	public void GameFinishByTime()
	{
		if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName))
		{
			GameFinishByTime_Blood();
		}
		else if (COMA_CommonOperation.Instance.IsMode_Tank(Application.loadedLevelName))
		{
			GameFinishByTime_Tank();
		}
		else
		{
			GameFinishByTime_Run_Maze_Glag();
		}
	}

	public void GameFinishByTime_Run_Maze_Glag()
	{
		COMA_Sys.Instance.bCoverUIInput = true;
		if (COMA_Flag_SceneController.Instance != null)
		{
			COMA_Flag_SceneController.Instance.flagTrs.gameObject.SetActive(false);
		}
		COMA_Player[] componentsInChildren = playerNodeTrs.GetComponentsInChildren<COMA_Player>();
		int num = componentsInChildren.Length;
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num - 1; i++)
		{
			for (int j = i + 1; j < num; j++)
			{
				if (componentsInChildren[j].score > componentsInChildren[i].score)
				{
					COMA_Player cOMA_Player = componentsInChildren[i];
					componentsInChildren[i] = componentsInChildren[j];
					componentsInChildren[j] = cOMA_Player;
				}
			}
		}
		if (COMA_CommonOperation.Instance.IsMode_Flag(COMA_NetworkConnect.sceneName) && COMA_Network.Instance.IsMyself(componentsInChildren[0].id))
		{
			COMA_Achievement.Instance.Flagger++;
		}
		if (COMA_CommonOperation.Instance.IsMode_Maze(COMA_NetworkConnect.sceneName) && COMA_Network.Instance.IsMyself(componentsInChildren[0].id) && componentsInChildren[0].score >= 100)
		{
			COMA_Achievement.Instance.Digger++;
		}
		UI_3DModeToTUIMgr.Instance.ptlFlowerObj.transform.position = componentsInChildren[0].rt3DObj.transform.position;
		UISettlementInfo[] array = new UISettlementInfo[num];
		for (int k = 0; k < num; k++)
		{
			COMA_Creation cOMA_Creation = componentsInChildren[k];
			int lv = cOMA_Creation.lv;
			int score = cOMA_Creation.score;
			cOMA_Creation.expGet = COMA_CommonOperation.Instance.CalExp(COMA_NetworkConnect.sceneName, k, score, lv);
			cOMA_Creation.goldGet = COMA_CommonOperation.Instance.CalGold(COMA_NetworkConnect.sceneName, k, score, cOMA_Creation.goldGet, lv);
			cOMA_Creation.crystalGet = 0;
			float num2 = (float)cOMA_Creation.exp / (float)COMA_Pref.Instance.expLv[lv - 1];
			float num3 = 0f;
			if (lv < COMA_Pref.Instance.lvMax)
			{
				if (cOMA_Creation.exp + cOMA_Creation.expGet < COMA_Pref.Instance.expLv[lv - 1])
				{
					num3 = (float)cOMA_Creation.expGet / (float)COMA_Pref.Instance.expLv[lv - 1];
				}
				else
				{
					num3 = 1f - num2;
					float num4 = cOMA_Creation.expGet - (COMA_Pref.Instance.expLv[lv - 1] - cOMA_Creation.exp);
					int num5 = lv + 1;
					while (num4 > (float)COMA_Pref.Instance.expLv[num5 - 1] && num5 < COMA_Pref.Instance.lvMax)
					{
						num4 -= (float)COMA_Pref.Instance.expLv[num5 - 1];
						num5++;
						num3 += 1f;
					}
					if (num5 < COMA_Pref.Instance.lvMax)
					{
						num3 += num4 / (float)COMA_Pref.Instance.expLv[num5 - 1];
					}
				}
			}
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && num2 + num3 > 1f)
			{
				settlementCom.lvUp = lv + 1;
			}
			Debug.Log(cOMA_Creation.nickname + " sc:" + score + " exp:" + cOMA_Creation.expGet + " gold:" + cOMA_Creation.goldGet + " ct:" + cOMA_Creation.crystalGet + " lv:" + lv + " " + num2 + " " + num3);
			int num6 = COMA_CommonOperation.Instance.CalScore(COMA_NetworkConnect.sceneName, k, COMA_CommonOperation.Instance.GetRankScoreAverage(), cOMA_Creation.rankscore);
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && !bCalScore)
			{
				bCalScore = true;
				COMA_Pref.Instance.AddRankScoreOfCurrentScene(num6);
			}
			array[k] = new UISettlementInfo(cOMA_Creation.nickname, num6, cOMA_Creation.expGet, cOMA_Creation.goldGet, cOMA_Creation.crystalGet, lv, num2, num3);
			array[k].Tex2D = cOMA_Creation.rt3DObj.transform.parent.camera.targetTexture;
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Show Finish : " + k);
		}
		settlementCom.gameObject.SetActive(true);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_ScoreCheck);
		settlementCom.Init(array);
		componentsInChildren[0].rt3DObj.animation.Play("Win");
		if (num >= 2)
		{
			componentsInChildren[num - 1].rt3DObj.animation.Play("Lose");
			componentsInChildren[num - 1].rt3DObj.animation.PlayQueued("Lose_idle");
		}
		LocalSettlement();
	}

	public void GameFinishByTime_Blood()
	{
		COMA_Blood_SceneController.Instance.UpdateTeamScore();
		COMA_Sys.Instance.bCoverUIInput = true;
		COMA_Player[] componentsInChildren = playerNodeTrs.GetComponentsInChildren<COMA_Player>();
		if (componentsInChildren.Length <= 0)
		{
			return;
		}
		COMA_PlayerSelf_Blood cOMA_PlayerSelf_Blood = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Blood;
		Team team = Team.None;
		team = ((teamScore[0] > teamScore[1]) ? Team.Team1 : ((teamScore[0] >= teamScore[1]) ? cOMA_PlayerSelf_Blood.team : Team.Team2));
		Debug.Log(team);
		bool flag = false;
		if (team == cOMA_PlayerSelf_Blood.team)
		{
			flag = true;
		}
		if (flag)
		{
			COMA_Achievement.Instance.BloodWin++;
		}
		List<COMA_Player> list = new List<COMA_Player>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].team == cOMA_PlayerSelf_Blood.team)
			{
				list.Add(componentsInChildren[i]);
			}
		}
		componentsInChildren = list.ToArray();
		UISettlementInfo[] array = new UISettlementInfo[componentsInChildren.Length];
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			COMA_Creation cOMA_Creation = componentsInChildren[j];
			int lv = cOMA_Creation.lv;
			int score = cOMA_Creation.score;
			cOMA_Creation.expGet = ((!flag) ? 400 : 600) + lv * 5 + score * 20;
			cOMA_Creation.goldGet = ((!flag) ? 100 : 200) + lv * 5;
			cOMA_Creation.crystalGet = 0;
			float num = (float)cOMA_Creation.exp / (float)COMA_Pref.Instance.expLv[lv - 1];
			float num2 = 0f;
			if (lv < COMA_Pref.Instance.lvMax)
			{
				if (cOMA_Creation.exp + cOMA_Creation.expGet < COMA_Pref.Instance.expLv[lv - 1])
				{
					num2 = (float)cOMA_Creation.expGet / (float)COMA_Pref.Instance.expLv[lv - 1];
				}
				else
				{
					num2 = 1f - num;
					float num3 = cOMA_Creation.expGet - (COMA_Pref.Instance.expLv[lv - 1] - cOMA_Creation.exp);
					int num4 = lv + 1;
					while (num3 > (float)COMA_Pref.Instance.expLv[num4 - 1] && num4 < COMA_Pref.Instance.lvMax)
					{
						num3 -= (float)COMA_Pref.Instance.expLv[num4 - 1];
						num4++;
						num2 += 1f;
					}
					if (num4 < COMA_Pref.Instance.lvMax)
					{
						num2 += num3 / (float)COMA_Pref.Instance.expLv[num4 - 1];
					}
				}
			}
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && num + num2 > 1f)
			{
				settlementCom.lvUp = lv + 1;
			}
			Debug.Log(cOMA_Creation.nickname + " sc:" + score + " exp:" + cOMA_Creation.expGet + " gold:" + cOMA_Creation.goldGet + " ct:" + cOMA_Creation.crystalGet + " lv:" + lv + " " + num + " " + num2);
			array[j] = new UISettlementInfo(cOMA_Creation.nickname, cOMA_Creation.score, cOMA_Creation.expGet, cOMA_PlayerSelf_Blood.GetDeadScore(cOMA_Creation.id), cOMA_Creation.goldGet, lv, num, num2);
			array[j].Tex2D = cOMA_Creation.rt3DObj.transform.parent.camera.targetTexture;
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Show Finish : " + j);
		}
		settlementCom.gameObject.SetActive(true);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_ScoreCheck);
		settlementCom.Init(array);
		if (flag)
		{
			if (componentsInChildren.Length > 0)
			{
				componentsInChildren[0].rt3DObj.animation.Play("Win");
			}
			if (componentsInChildren.Length > 1)
			{
				componentsInChildren[1].rt3DObj.animation.Play("Win");
			}
			if (componentsInChildren.Length > 2)
			{
				componentsInChildren[2].rt3DObj.animation.Play("Win");
			}
			if (componentsInChildren.Length > 3)
			{
				componentsInChildren[3].rt3DObj.animation.Play("Win");
			}
			COMA_Blood_SceneController.Instance.UI_WinObj.SetActive(true);
			COMA_Blood_SceneController.Instance.UI_LoseObj.SetActive(false);
		}
		else
		{
			COMA_Blood_SceneController.Instance.UI_WinObj.SetActive(false);
			COMA_Blood_SceneController.Instance.UI_LoseObj.SetActive(true);
		}
		COMA_Pref.Instance.exp += COMA_PlayerSelf.Instance.expGet;
		COMA_PlayerSelf.Instance.expGet = 0;
		COMA_Pref.Instance.AddGold(COMA_PlayerSelf.Instance.goldGet);
		COMA_PlayerSelf.Instance.goldGet = 0;
		COMA_Pref.Instance.AddCrystal(COMA_PlayerSelf.Instance.crystalGet);
		COMA_PlayerSelf.Instance.crystalGet = 0;
		COMA_Pref.Instance.Save(true);
	}

	public void GameFinishByTime_Tank()
	{
		COMA_PlayerSelf_Tank cOMA_PlayerSelf_Tank = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Tank;
		if (cOMA_PlayerSelf_Tank != null && cOMA_PlayerSelf_Tank._TankMoveSound != null)
		{
			cOMA_PlayerSelf_Tank._TankMoveSound.Stop();
		}
		COMA_Sys.Instance.bCoverUIInput = true;
		COMA_Player[] componentsInChildren = playerNodeTrs.GetComponentsInChildren<COMA_Player>();
		if (componentsInChildren.Length <= 0)
		{
			return;
		}
		int num = componentsInChildren.Length;
		COMA_Player[] array = new COMA_Player[2];
		COMA_Player[] array2 = new COMA_Player[2];
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (TankCommon.isAliance(COMA_PlayerSelf.Instance.sitIndex, componentsInChildren[i].sitIndex))
			{
				array[num2++] = componentsInChildren[i];
			}
			else
			{
				array2[num3++] = componentsInChildren[i];
			}
		}
		componentsInChildren = new COMA_Player[4]
		{
			array[0],
			array[1],
			array2[0],
			array2[1]
		};
		bool flag = false;
		flag = COMA_Tank_SceneController.Instance.getOurScore() >= COMA_Tank_SceneController.Instance.getOppScore();
		COMA_AudioManager.Instance.MusicPlay((!flag) ? AudioCategory.BGM_Tank_Lose : AudioCategory.BGM_Tank_Lose);
		COMA_PlayerSelf_Tank cOMA_PlayerSelf_Tank2 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Tank;
		UISettlementInfo[] array3 = new UISettlementInfo[componentsInChildren.Length];
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			COMA_Creation cOMA_Creation = componentsInChildren[j];
			if (cOMA_Creation == null)
			{
				array3[j] = new UISettlementInfo(string.Empty, 0, 0, 0, 0, 0, 0f, 0f);
				array3[j].Tex2D = null;
				continue;
			}
			int lv = cOMA_Creation.lv;
			int score = cOMA_Creation.score;
			cOMA_Creation.expGet = ((!flag) ? 400 : 600) + lv * 5 + score * 20;
			cOMA_Creation.goldGet = ((!flag) ? 100 : 200) + lv * 5;
			cOMA_Creation.crystalGet = 0;
			float num4 = (float)cOMA_Creation.exp / (float)COMA_Pref.Instance.expLv[lv - 1];
			float num5 = 0f;
			if (lv < COMA_Pref.Instance.lvMax)
			{
				if (cOMA_Creation.exp + cOMA_Creation.expGet < COMA_Pref.Instance.expLv[lv - 1])
				{
					num5 = (float)cOMA_Creation.expGet / (float)COMA_Pref.Instance.expLv[lv - 1];
				}
				else
				{
					num5 = 1f - num4;
					float num6 = cOMA_Creation.expGet - (COMA_Pref.Instance.expLv[lv - 1] - cOMA_Creation.exp);
					int num7 = lv + 1;
					while (num6 > (float)COMA_Pref.Instance.expLv[num7 - 1] && num7 < COMA_Pref.Instance.lvMax)
					{
						num6 -= (float)COMA_Pref.Instance.expLv[num7 - 1];
						num7++;
						num5 += 1f;
					}
					if (num7 < COMA_Pref.Instance.lvMax)
					{
						num5 += num6 / (float)COMA_Pref.Instance.expLv[num7 - 1];
					}
				}
			}
			if (cOMA_Creation.id == COMA_PlayerSelf.Instance.id && num4 + num5 > 1f)
			{
				settlementCom.lvUp = lv + 1;
			}
			Debug.Log(cOMA_Creation.nickname + " sc:" + score + " exp:" + cOMA_Creation.expGet + " gold:" + cOMA_Creation.goldGet + " ct:" + cOMA_Creation.crystalGet + " lv:" + lv + " " + num4 + " " + num5);
			array3[j] = new UISettlementInfo(cOMA_Creation.nickname, cOMA_Creation.score, cOMA_Creation.expGet, cOMA_PlayerSelf_Tank2.GetDeadScore(cOMA_Creation.id), cOMA_Creation.goldGet, lv, num4, num5);
			array3[j].Tex2D = cOMA_Creation.rt3DObj.transform.parent.camera.targetTexture;
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Show Finish : " + j);
		}
		settlementCom.gameObject.SetActive(true);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_ScoreCheck);
		settlementCom.Init(array3);
		if (componentsInChildren[0] != null)
		{
			componentsInChildren[0].rt3DObj.animation.Play((!flag) ? "Lose" : "Win");
		}
		if (componentsInChildren[1] != null)
		{
			componentsInChildren[1].rt3DObj.animation.Play((!flag) ? "Lose" : "Win");
		}
		if (componentsInChildren[2] != null)
		{
			componentsInChildren[2].rt3DObj.animation.Play((!flag) ? "Win" : "Lose");
		}
		if (componentsInChildren[3] != null)
		{
			componentsInChildren[3].rt3DObj.animation.Play((!flag) ? "Win" : "Lose");
		}
		if (flag)
		{
			COMA_Tank_SceneController.Instance.UI_WinObj.SetActive(true);
			COMA_Tank_SceneController.Instance.UI_LoseObj.SetActive(false);
		}
		else
		{
			COMA_Tank_SceneController.Instance.UI_WinObj.SetActive(false);
			COMA_Tank_SceneController.Instance.UI_LoseObj.SetActive(true);
		}
		COMA_Pref.Instance.exp += COMA_PlayerSelf.Instance.expGet;
		COMA_PlayerSelf.Instance.expGet = 0;
		COMA_Pref.Instance.AddGold(COMA_PlayerSelf.Instance.goldGet);
		COMA_PlayerSelf.Instance.goldGet = 0;
		COMA_Pref.Instance.AddCrystal(COMA_PlayerSelf.Instance.crystalGet);
		COMA_PlayerSelf.Instance.crystalGet = 0;
		COMA_Pref.Instance.Save(true);
	}

	public bool GameFinishByLocal()
	{
		COMA_Sys.Instance.bCoverUIInput = true;
		UISettlementInfo[] array = new UISettlementInfo[1];
		COMA_Creation instance = COMA_PlayerSelf.Instance;
		int lv = COMA_Pref.Instance.lv;
		int score = instance.score;
		instance.expGet = COMA_CommonOperation.Instance.CalExp(COMA_NetworkConnect.sceneName, 0, score, 0);
		instance.goldGet = COMA_CommonOperation.Instance.CalGold(COMA_NetworkConnect.sceneName, 0, score, instance.goldGet, 0);
		instance.crystalGet = 0;
		float num = (float)instance.exp / (float)COMA_Pref.Instance.expLv[lv - 1];
		float num2 = (float)instance.expGet / (float)COMA_Pref.Instance.expLv[lv - 1];
		if (instance.id == COMA_PlayerSelf.Instance.id && num + num2 > 1f)
		{
			settlementCom.lvUp = lv + 1;
		}
		array[0] = new UISettlementInfo(instance.nickname, score, instance.expGet, instance.goldGet, instance.crystalGet, lv, num, num2);
		array[0].Tex2D = instance.rt3DObj.transform.parent.camera.targetTexture;
		settlementCom.gameObject.SetActive(true);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_ScoreCheck);
		settlementCom.Init(array);
		LocalSettlement();
		return false;
	}

	private void LocalSettlement()
	{
		COMA_PlayerSelf.Instance.score = 0;
		COMA_Pref.Instance.exp += COMA_PlayerSelf.Instance.expGet;
		COMA_PlayerSelf.Instance.expGet = 0;
		COMA_Pref.Instance.AddGold(COMA_PlayerSelf.Instance.goldGet);
		COMA_PlayerSelf.Instance.goldGet = 0;
		COMA_Pref.Instance.AddCrystal(COMA_PlayerSelf.Instance.crystalGet);
		COMA_PlayerSelf.Instance.crystalGet = 0;
		COMA_Pref.Instance.Save(true);
	}
}
