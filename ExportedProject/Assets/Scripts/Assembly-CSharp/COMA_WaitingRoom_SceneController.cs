using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class COMA_WaitingRoom_SceneController : TBaseEntity
{
	public class Run_SceneData
	{
		public string strD1SecObjs = string.Empty;

		public string strD2SecObjs = string.Empty;

		public string strD3SecObjs = string.Empty;
	}

	private static COMA_WaitingRoom_SceneController _instance;

	private bool isCounting;

	public UIRoom_Countdown startCountDownCom;

	private int needPlayers = 4;

	public GameObject barObj;

	public GameObject[] titleObjs;

	public UIWaitingRoomPlayerNum roomInfoPlayerNum;

	private bool bBoxLock;

	private float goldDropInterval = 3f;

	private int goldDropNumber = 1;

	private Vector3 tarPos = Vector3.zero;

	public GameObject btnBack;

	public List<uint> friendListToInvite = new List<uint>();

	public static COMA_WaitingRoom_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private new void OnEnable()
	{
		base.OnEnable();
		_instance = this;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.ROOM_START, StartToGo);
		}
	}

	private new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.ROOM_START, StartToGo);
		}
	}

	private void Start()
	{
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput, this);
		COMA_Sys.Instance.bCoverUpdate = false;
		COMA_Sys.Instance.bCoverUIInput = false;
		COMA_Sys.Instance.bGameCounting = false;
		COMA_Sys.Instance.bIgnoreGameCount = false;
		needPlayers = COMA_CommonOperation.Instance.SceneNameToPlayerCount(COMA_NetworkConnect.sceneName);
	}

	private void OnDestroy()
	{
		if (TPCInputMgr.Instance != null)
		{
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput, this);
		}
	}

	private void StartToGo(TNetEventData tEvent)
	{
		Debug.Log("StartToGo");
		EnterGame();
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (msg._nMsgId == 30000)
		{
			TPCInputEvent tPCInputEvent = (TPCInputEvent)msg._pExtraInfo;
			if (tPCInputEvent.type == EventType.KeyDown)
			{
				if (tPCInputEvent.code == KeyCode.G)
				{
					if (COMA_Sys.Instance.IsSuperAccount)
					{
						if (COMA_CommonOperation.Instance.IsMode_Run(COMA_NetworkConnect.sceneName))
						{
							COMA_Sys.Instance.roadIDs = InitSceneData();
						}
						if (COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName) && !COMA_Tank_SceneController.bLevelCreated)
						{
							int num = Random.Range(1, 5);
							COMA_Sys.Instance.roadIDs = num.ToString();
							COMA_Sys.Instance.roadIDs += ",";
							for (int i = 0; i < 4; i++)
							{
								COMA_Sys.Instance.roadIDs += Random.Range(0, 5);
							}
							Debug.Log(" ------------------------------------------tank level> " + COMA_Sys.Instance.roadIDs);
							COMA_CD_CreateRoad cOMA_CD_CreateRoad = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_CREATEROAD) as COMA_CD_CreateRoad;
							cOMA_CD_CreateRoad.rIDs = COMA_Sys.Instance.roadIDs;
							COMA_CommandHandler.Instance.Send(cOMA_CD_CreateRoad);
						}
						COMA_CommonOperation.Instance.isEnterWithG = true;
						if (COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id))
						{
							COMA_Network.Instance.StartGame();
						}
						COMA_Pref.Instance.Save(true);
						Application.LoadLevel(COMA_NetworkConnect.sceneName);
					}
				}
				else if (tPCInputEvent.code == KeyCode.F && COMA_PlayerSelf.Instance != null && COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id) && !isCounting)
				{
					COMA_Network.Instance.StartGame();
				}
			}
		}
		return true;
	}

	private void EnterGame()
	{
		if (isCounting)
		{
			return;
		}
		if (btnBack != null)
		{
			btnBack.SetActive(false);
		}
		Debug.Log(roomInfoPlayerNum.CurNum + " / " + roomInfoPlayerNum.MaxNum + "    " + isCounting);
		isCounting = true;
		startCountDownCom.StartCountDown(10f);
		COMA_Pref.Instance.Save(true);
		goldDropInterval = 999999f;
		if (COMA_CommonOperation.Instance.IsMode_Run(COMA_NetworkConnect.sceneName))
		{
			if (COMA_PlayerSelf.Instance != null && COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id) && !COMA_Run_SceneController.bRoadCreated)
			{
				COMA_Sys.Instance.roadIDs = InitSceneData();
				Debug.Log(" ------------------------------------------> " + COMA_Sys.Instance.roadIDs);
				COMA_CD_CreateRoad cOMA_CD_CreateRoad = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_CREATEROAD) as COMA_CD_CreateRoad;
				cOMA_CD_CreateRoad.rIDs = COMA_Sys.Instance.roadIDs;
				COMA_CommandHandler.Instance.Send(cOMA_CD_CreateRoad);
			}
		}
		else if (COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName) && COMA_PlayerSelf.Instance != null && COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id) && !COMA_Tank_SceneController.bLevelCreated)
		{
			int num = Random.Range(1, 5);
			COMA_Sys.Instance.roadIDs = num.ToString();
			COMA_Sys.Instance.roadIDs += ",";
			for (int i = 0; i < 4; i++)
			{
				COMA_Sys.Instance.roadIDs += Random.Range(0, 5);
			}
			Debug.Log(" ------------------------------------------tank level> " + COMA_Sys.Instance.roadIDs);
			COMA_CD_CreateRoad cOMA_CD_CreateRoad2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_CREATEROAD) as COMA_CD_CreateRoad;
			cOMA_CD_CreateRoad2.rIDs = COMA_Sys.Instance.roadIDs;
			COMA_CommandHandler.Instance.Send(cOMA_CD_CreateRoad2);
		}
	}

	private new void Update()
	{
		if (bBoxLock || COMA_Network.Instance.TNetInstance == null)
		{
			return;
		}
		if (COMA_Network.Instance.TNetInstance.CurRoom == null)
		{
			roomInfoPlayerNum.MaxNum = needPlayers;
			roomInfoPlayerNum.CurNum = needPlayers;
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(117);
			uI_MsgBox.AddProceYesHandler(GoBackToMainMenu);
			bBoxLock = true;
			return;
		}
		roomInfoPlayerNum.CurNum = COMA_Network.Instance.TNetInstance.CurRoom.UserCount;
		roomInfoPlayerNum.MaxNum = COMA_Network.Instance.TNetInstance.CurRoom.MaxUsers;
		if (!COMA_CommonOperation.Instance.isCreateRoom && roomInfoPlayerNum.CurNum >= roomInfoPlayerNum.MaxNum && COMA_PlayerSelf.Instance != null && COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id) && !isCounting)
		{
			COMA_Network.Instance.StartGame();
		}
		if (COMA_Network.Instance.TNetInstance.CurRoom.IsGaming)
		{
			EnterGame();
		}
		goldDropInterval -= Time.deltaTime;
		if (goldDropInterval < 0f)
		{
			goldDropInterval = Random.Range(10, 21);
			goldDropNumber = Random.Range(1, 4);
			float num = Random.Range(-12f, 12f);
			float num2 = Random.Range(-12f, 12f);
			if (num > -6f && num < 6f && num2 > -6f && num2 < 6f)
			{
				num2 = 7f;
			}
			tarPos = new Vector3(num, 0f, num2);
			Vector3 position = new Vector3(6f, 10f, num);
			Quaternion identity = Quaternion.identity;
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/WaitingRoom/FallingGold"), position, identity) as GameObject;
			COMA_FallingGold component = gameObject.GetComponent<COMA_FallingGold>();
			component.ptl_path_meteor = "Meteor/Meteor_02";
			component.ptl_path_meteorBlast = "Meteor_Brust/Meteor_Brust_1";
			component.pos_target = tarPos;
			component.lerpTime = 1f;
			SceneTimerInstance.Instance.Add(component.lerpTime + 0.2f, AppearGold);
		}
	}

	public bool AppearGold()
	{
		for (int i = 0; i < goldDropNumber; i++)
		{
			Vector3 position = tarPos;
			position.x += Random.Range(-1f, 1f);
			position.z += Random.Range(-1f, 1f);
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Item_Gold"), position, Quaternion.identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			Object.DestroyObject(gameObject, 10f);
		}
		return false;
	}

	private string InitSceneData()
	{
		int[] array = new int[9] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
		int[] array2 = new int[3] { 3, 3, 3 };
		Run_SceneData run_SceneData = new Run_SceneData();
		TextAsset textAsset = Resources.Load("Data/Run_SceneData") as TextAsset;
		string text = textAsset.text;
		run_SceneData = COMA_Tools.DeserializeObject<Run_SceneData>(text) as Run_SceneData;
		string[] array3 = run_SceneData.strD1SecObjs.Split(',');
		COMA_Run_RoadOneSec[] array4 = new COMA_Run_RoadOneSec[array3.Length];
		for (int i = 0; i < array3.Length; i++)
		{
			string text2 = "FBX/Scene/Run/Prefab/";
			text2 += array3[i];
			GameObject gameObject = Resources.Load(text2) as GameObject;
			array4[i] = gameObject.GetComponent<COMA_Run_RoadOneSec>();
			if (array4[i] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject.name);
			}
		}
		string[] array5 = run_SceneData.strD2SecObjs.Split(',');
		COMA_Run_RoadOneSec[] array6 = new COMA_Run_RoadOneSec[array5.Length];
		for (int j = 0; j < array5.Length; j++)
		{
			string text3 = "FBX/Scene/Run/Prefab/";
			text3 += array5[j];
			GameObject gameObject2 = Resources.Load(text3) as GameObject;
			array6[j] = gameObject2.GetComponent<COMA_Run_RoadOneSec>();
			if (array6[j] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject2.name);
			}
		}
		string[] array7 = run_SceneData.strD3SecObjs.Split(',');
		COMA_Run_RoadOneSec[] array8 = new COMA_Run_RoadOneSec[array7.Length];
		for (int k = 0; k < array7.Length; k++)
		{
			string text4 = "FBX/Scene/Run/Prefab/";
			text4 += array7[k];
			GameObject gameObject3 = Resources.Load(text4) as GameObject;
			array8[k] = gameObject3.GetComponent<COMA_Run_RoadOneSec>();
			if (array8[k] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject3.name);
			}
		}
		COMA_Run_RoadOneSec[][] array9 = new COMA_Run_RoadOneSec[3][] { array4, array6, array8 };
		int num = array4.Length;
		int num2 = array6.Length;
		int num3 = array8.Length;
		int num4 = 3;
		if (num < num4 || num2 < num4 || num3 < num4)
		{
			Debug.LogError("Invalid road conf!");
			return string.Empty;
		}
		int[] array10 = new int[3] { num, num2, num3 };
		int[][] array11 = new int[num4][];
		for (int l = 0; l < num4; l++)
		{
			array11[l] = new int[array2[l]];
		}
		for (int m = 0; m < num4; m++)
		{
			Random.seed = ((int)Time.time + m) * 113651;
			COMA_Tools.GetRandomArray_AppointSize(array10[m], ref array11[m]);
		}
		int[] array12 = new int[3];
		string text5 = string.Empty;
		int num5 = Random.Range(0, 5);
		if (num5 >= 3)
		{
			num5 = 3;
		}
		for (int n = 0; n < array.Length; n++)
		{
			int num6 = array[n];
			int num7 = array11[num6][array12[num6]];
			array12[num6]++;
			COMA_Run_RoadOneSec cOMA_Run_RoadOneSec = array9[num6][num7];
			int num8 = 0;
			int num9 = 0;
			num9 = Random.Range(0, cOMA_Run_RoadOneSec._NextPos.Length);
			switch (num6)
			{
			case 0:
				num8 = Random.Range(0, cOMA_Run_RoadOneSec._diffLevels1.Length);
				break;
			case 1:
				num8 = Random.Range(0, cOMA_Run_RoadOneSec._diffLevels2.Length);
				break;
			case 2:
				num8 = Random.Range(0, cOMA_Run_RoadOneSec._diffLevels3.Length);
				break;
			}
			Random.seed = ((int)Time.time + n) * 213651;
			text5 += num6;
			text5 += ",";
			text5 += num7;
			text5 += ":";
			text5 += num8;
			text5 += ".";
			text5 += num9;
			text5 += ".";
			text5 += num5;
			text5 += ";";
		}
		return text5;
	}

	public void GoBackToMainMenu(string param)
	{
		COMA_NetworkConnect.Instance.BackFromScene();
	}
}
