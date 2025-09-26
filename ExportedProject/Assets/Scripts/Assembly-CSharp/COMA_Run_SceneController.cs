using System;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Run_SceneController : MonoBehaviour
{
	public enum ERoadDiffiLevel
	{
		Simple = 0,
		Normal = 1,
		Hard = 2
	}

	public class Run_SceneData
	{
		public string strD1SecObjs = string.Empty;

		public string strD2SecObjs = string.Empty;

		public string strD3SecObjs = string.Empty;
	}

	private static COMA_Run_SceneController _instance;

	public UIInGame_PropsBox propsCom;

	public UI_ParkourProcessMgr processCom;

	public UI_Effect_Ink inkCom;

	public UI_UsePropInfoMgr usePropInfoCom;

	public Texture2D[] iconTexs;

	private bool bHasCreateRoad;

	public GameObject strObj;

	public GameObject[] _startTerrains;

	public GameObject endObj;

	[SerializeField]
	public GameObject[] newEndObjs;

	public GameObject[] secObj;

	private Vector3 secPos = Vector3.zero;

	private List<int> wayIDList = new List<int>();

	private List<Transform> wayList = new List<Transform>();

	private int roadCount = 8;

	public static bool bRoadCreated;

	private float totalDistance = 1f;

	[NonSerialized]
	public Transform mineNodeTrs;

	private List<GameObject> ltMineObjs = new List<GameObject>();

	[SerializeField]
	private COMA_Run_RoadOneSec[] _d1SecObj;

	[SerializeField]
	private COMA_Run_RoadOneSec[] _d2SecObj;

	[SerializeField]
	private COMA_Run_RoadOneSec[] _d3SecObj;

	private COMA_Run_RoadOneSec[][] _dSecObjs;

	private int[] _roadDiffIndexs = new int[9] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };

	private int[] _nDCount = new int[3] { 3, 3, 3 };

	[SerializeField]
	private int _nCustomLandformIndex = -1;

	[SerializeField]
	private bool _bUseLanform = true;

	private Run_SceneData _sceneData;

	[SerializeField]
	private GameObject[] _skys;

	public static COMA_Run_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool IsCreatedRoad
	{
		get
		{
			return bRoadCreated;
		}
	}

	public void SetSky(int nIndex)
	{
		for (int i = 0; i < _skys.Length; i++)
		{
			if (i == nIndex)
			{
				_skys[i].SetActive(true);
			}
			else
			{
				_skys[i].SetActive(false);
			}
		}
		if (nIndex == 0)
		{
			RenderSettings.fogColor = new Color(0.22f, 0.596f, 0.92f);
			RenderSettings.fogColor = new Color(0.051f, 0.13f, 0.192f);
			RenderSettings.fog = true;
			Debug.Log("白天======================================>RenderSettings.fogColor=" + RenderSettings.fogColor);
		}
		else
		{
			RenderSettings.fogColor = new Color(0.051f, 0.13f, 0.192f);
			RenderSettings.fog = true;
			Debug.Log("晚上======================================>RenderSettings.fogColor=" + RenderSettings.fogColor);
		}
	}

	private void InitSceneData()
	{
		string[] array = _sceneData.strD1SecObjs.Split(',');
		_d1SecObj = new COMA_Run_RoadOneSec[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = "FBX/Scene/Run/Prefab/";
			text += array[i];
			GameObject gameObject = Resources.Load(text) as GameObject;
			_d1SecObj[i] = gameObject.GetComponent<COMA_Run_RoadOneSec>();
			if (_d1SecObj[i] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject.name);
			}
		}
		string[] array2 = _sceneData.strD2SecObjs.Split(',');
		_d2SecObj = new COMA_Run_RoadOneSec[array2.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			string text2 = "FBX/Scene/Run/Prefab/";
			text2 += array2[j];
			GameObject gameObject2 = Resources.Load(text2) as GameObject;
			_d2SecObj[j] = gameObject2.GetComponent<COMA_Run_RoadOneSec>();
			if (_d2SecObj[j] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject2.name);
			}
		}
		string[] array3 = _sceneData.strD3SecObjs.Split(',');
		_d3SecObj = new COMA_Run_RoadOneSec[array3.Length];
		for (int k = 0; k < array3.Length; k++)
		{
			string text3 = "FBX/Scene/Run/Prefab/";
			text3 += array3[k];
			GameObject gameObject3 = Resources.Load(text3) as GameObject;
			_d3SecObj[k] = gameObject3.GetComponent<COMA_Run_RoadOneSec>();
			if (_d3SecObj[k] == null)
			{
				Debug.LogError("------Lack COMA_Run_RoadOneSec:" + gameObject3.name);
			}
		}
	}

	private void Awake()
	{
		bRoadCreated = false;
		_sceneData = new Run_SceneData();
		TextAsset textAsset = Resources.Load("Data/Run_SceneData") as TextAsset;
		string text = textAsset.text;
		_sceneData = COMA_Tools.DeserializeObject<Run_SceneData>(text) as Run_SceneData;
		InitSceneData();
		_dSecObjs = new COMA_Run_RoadOneSec[3][];
		_dSecObjs[0] = _d1SecObj;
		_dSecObjs[1] = _d2SecObj;
		_dSecObjs[2] = _d3SecObj;
		wayList.Add(strObj.transform);
		secPos = strObj.transform.FindChild("Next").position;
	}

	public void InitPropIcons()
	{
		propsCom.Init(iconTexs);
	}

	private void OnEnable()
	{
		_instance = this;
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.RUN_CREATEROAD, ReceiveCreateRoad);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.DELMINE, ReceiveDelMine);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.RUN_USEITEMINFO, ReceiveUseItemInfo);
	}

	private void OnDisable()
	{
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.RUN_CREATEROAD, ReceiveCreateRoad);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.DELMINE, ReceiveDelMine);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.RUN_USEITEMINFO, ReceiveUseItemInfo);
	}

	private void ReceiveCreateRoad(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_CreateRoad cOMA_CD_CreateRoad = commandDatas as COMA_CD_CreateRoad;
			CreateNewRoad(cOMA_CD_CreateRoad.rIDs);
		}
	}

	private void ReceivePutMine(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_PutMine cOMA_CD_PutMine = commandDatas as COMA_CD_PutMine;
			PutMine(cOMA_CD_PutMine.pos, cOMA_CD_PutMine.strMineName);
		}
	}

	private void ReceiveDelMine(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_DelMine cOMA_CD_DelMine = commandDatas as COMA_CD_DelMine;
			DelMine(cOMA_CD_DelMine.strMineName);
		}
	}

	private void ReceiveUseItemInfo(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_UseItemInfo cOMA_CD_UseItemInfo = commandDatas as COMA_CD_UseItemInfo;
			UseItemInfoLocal(cOMA_CD_UseItemInfo.nameA, cOMA_CD_UseItemInfo.itemID, cOMA_CD_UseItemInfo.nameB);
			Debug.Log(cOMA_CD_UseItemInfo.nameA + " " + cOMA_CD_UseItemInfo.itemID + " " + cOMA_CD_UseItemInfo.nameB);
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Run);
		mineNodeTrs = new GameObject("MineNodeTrs").transform;
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
		SceneTimerInstance.Instance.Add(4f, CreateDefaultRoad);
	}

	public bool CreateDefaultRoad()
	{
		return false;
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(5);
		return false;
	}

	private string GenerateTerrainConf()
	{
		int num = _d1SecObj.Length;
		int num2 = _d2SecObj.Length;
		int num3 = _d3SecObj.Length;
		int num4 = 3;
		if (num < num4 || num2 < num4 || num3 < num4)
		{
			Debug.LogError("Invalid road conf!");
			return string.Empty;
		}
		int[] array = new int[3] { num, num2, num3 };
		int[][] array2 = new int[num4][];
		for (int i = 0; i < num4; i++)
		{
			array2[i] = new int[_nDCount[i]];
		}
		for (int j = 0; j < num4; j++)
		{
			UnityEngine.Random.seed = ((int)Time.time + j) * 113651;
			COMA_Tools.GetRandomArray_AppointSize(array[j], ref array2[j]);
		}
		int[] array3 = new int[3];
		string text = string.Empty;
		int num5 = UnityEngine.Random.Range(0, 5);
		if (num5 >= 3)
		{
			num5 = 3;
		}
		for (int k = 0; k < _roadDiffIndexs.Length; k++)
		{
			int num6 = _roadDiffIndexs[k];
			int num7 = array2[num6][array3[num6]];
			array3[num6]++;
			COMA_Run_RoadOneSec cOMA_Run_RoadOneSec = _dSecObjs[num6][num7];
			int num8 = 0;
			int num9 = 0;
			num9 = UnityEngine.Random.Range(0, cOMA_Run_RoadOneSec._NextPos.Length);
			switch (num6)
			{
			case 0:
				num8 = UnityEngine.Random.Range(0, cOMA_Run_RoadOneSec._diffLevels1.Length);
				break;
			case 1:
				num8 = UnityEngine.Random.Range(0, cOMA_Run_RoadOneSec._diffLevels2.Length);
				break;
			case 2:
				num8 = UnityEngine.Random.Range(0, cOMA_Run_RoadOneSec._diffLevels3.Length);
				break;
			}
			UnityEngine.Random.seed = ((int)Time.time + k) * 213651;
			text += num6;
			text += ",";
			text += num7;
			text += ":";
			text += num8;
			text += ".";
			text += num9;
			text += ".";
			text += num5;
			text += ";";
		}
		return text;
	}

	private void Update()
	{
		if (COMA_Sys.Instance.roadIDs != string.Empty)
		{
			Debug.Log("------------------------------ " + COMA_Sys.Instance.roadIDs);
			CreateNewRoad(COMA_Sys.Instance.roadIDs);
			COMA_Sys.Instance.roadIDs = string.Empty;
		}
		if (COMA_PlayerSelf.Instance != null && COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id) && !bRoadCreated)
		{
			string empty = string.Empty;
			empty = GenerateTerrainConf();
			CreateNewRoad(empty);
			COMA_CD_CreateRoad cOMA_CD_CreateRoad = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_CREATEROAD) as COMA_CD_CreateRoad;
			cOMA_CD_CreateRoad.rIDs = empty;
			COMA_CommandHandler.Instance.Send(cOMA_CD_CreateRoad);
		}
		ShowProgress();
		for (int i = 1; i < wayList.Count; i++)
		{
			COMA_Run_RoadOneSec component = wayList[i].GetComponent<COMA_Run_RoadOneSec>();
			if (component != null)
			{
				component.TickOptimize();
			}
		}
	}

	private void CreateNewRoad(string rIDs)
	{
		Debug.Log("bRoadCreated : " + bRoadCreated);
		if (bRoadCreated)
		{
			return;
		}
		bRoadCreated = true;
		Debug.Log("rIDs---Create front : " + rIDs);
		if (rIDs == string.Empty)
		{
			return;
		}
		Debug.Log("CreateOneSec : " + rIDs);
		int num = 0;
		string[] array = rIDs.Split(';');
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text == string.Empty)
			{
				break;
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string[] array3 = text.Split(',');
			num2 = int.Parse(array3[0]);
			string[] array4 = array3[1].Split(':');
			num3 = int.Parse(array4[0]);
			string[] array5 = array4[1].Split('.');
			num4 = int.Parse(array5[0]);
			num5 = int.Parse(array5[1]);
			int num6 = int.Parse(array5[2]);
			if (_nCustomLandformIndex != -1)
			{
				num6 = _nCustomLandformIndex;
			}
			COMA_Run_RoadOneSec cOMA_Run_RoadOneSec = _dSecObjs[num2][num3];
			string text2 = "FBX/Scene/Run/Prefab/";
			text2 += cOMA_Run_RoadOneSec.gameObject.name;
			switch (num6)
			{
			case 0:
				text2 += "_a";
				break;
			case 1:
				text2 += "_b";
				break;
			case 2:
				text2 += "_c";
				break;
			case 3:
				text2 += "_d";
				break;
			}
			num = num6;
			GameObject gameObject = null;
			gameObject = ((!_bUseLanform) ? (UnityEngine.Object.Instantiate(cOMA_Run_RoadOneSec.gameObject) as GameObject) : (UnityEngine.Object.Instantiate(Resources.Load(text2)) as GameObject));
			gameObject.transform.position = secPos;
			cOMA_Run_RoadOneSec = gameObject.GetComponent<COMA_Run_RoadOneSec>();
			cOMA_Run_RoadOneSec.InitSecRoadDiff(num2, num4);
			cOMA_Run_RoadOneSec.InitSecRoadNext(num5);
			secPos = cOMA_Run_RoadOneSec._NextPos[num5].position;
			wayList.Add(gameObject.transform);
		}
		for (int j = 0; j < _startTerrains.Length; j++)
		{
			Debug.Log("[[[[[[[landformInex=" + num);
			if (j == num)
			{
				_startTerrains[j].SetActive(true);
			}
			else
			{
				_startTerrains[j].SetActive(false);
			}
		}
		if (num == 3)
		{
			SetSky(1);
			newEndObjs[1].SetActive(true);
			newEndObjs[0].SetActive(false);
			newEndObjs[1].transform.position = secPos;
			wayList.Add(newEndObjs[1].transform);
			totalDistance = newEndObjs[1].transform.position.z - strObj.transform.position.z;
			Debug.Log("Total Distance : " + totalDistance);
		}
		else
		{
			SetSky(0);
			newEndObjs[0].SetActive(true);
			newEndObjs[1].SetActive(false);
			newEndObjs[0].transform.position = secPos;
			wayList.Add(newEndObjs[0].transform);
			totalDistance = newEndObjs[0].transform.position.z - strObj.transform.position.z;
			Debug.Log("Total Distance : " + totalDistance);
		}
	}

	private void CreateRoad(string rIDs)
	{
		if (bRoadCreated)
		{
			return;
		}
		bRoadCreated = true;
		if (!(rIDs == string.Empty))
		{
			Debug.Log("CreateOneSec : " + rIDs);
			string[] array = rIDs.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				int item = int.Parse(array[i]);
				wayIDList.Add(item);
			}
			while (wayIDList.Count > 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(secObj[wayIDList[0]]) as GameObject;
				gameObject.transform.position = secPos;
				wayList.Add(gameObject.transform);
				secPos = gameObject.transform.FindChild("Next").position;
				wayIDList.RemoveAt(0);
			}
			if (!endObj.activeSelf)
			{
				endObj.SetActive(true);
				endObj.transform.position = secPos;
				wayList.Add(endObj.transform);
			}
			totalDistance = endObj.transform.position.z - strObj.transform.position.z;
			Debug.Log("Total Distance : " + totalDistance);
		}
	}

	public int CheckSec(Vector3 pos)
	{
		int num = wayList.Count - 1;
		while (num >= 0 && !(pos.z > wayList[num].FindChild("Next").position.z))
		{
			num--;
		}
		return num;
	}

	public void PutMine(Vector3 pos)
	{
		Ray ray = new Ray(pos + Vector3.up, Vector3.down);
		int layerMask = 1 << LayerMask.NameToLayer("Collision");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, layerMask) && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) == "Collision")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Prop_Mine"), hitInfo.point, Quaternion.identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			gameObject.transform.parent = mineNodeTrs;
			ltMineObjs.Add(gameObject);
		}
	}

	public void PutMine(Vector3 pos, string mineName)
	{
		Ray ray = new Ray(pos + Vector3.up, Vector3.down);
		int layerMask = 1 << LayerMask.NameToLayer("Collision");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, layerMask) && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) == "Collision")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Prop_Mine"), hitInfo.point, Quaternion.identity) as GameObject;
			gameObject.name = mineName;
			gameObject.transform.parent = hitInfo.collider.gameObject.transform;
			ltMineObjs.Add(gameObject);
		}
	}

	public void DelMine(string mineName)
	{
		for (int i = 0; i < ltMineObjs.Count; i++)
		{
			Transform transform = ltMineObjs[i].transform;
			if (ltMineObjs[i].name == mineName)
			{
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Bazooka_Brust/Mine_Brust"), transform.position, transform.rotation) as GameObject;
				UnityEngine.Object.DestroyObject(obj, 2f);
				ltMineObjs.RemoveAt(i);
				UnityEngine.Object.DestroyObject(transform.gameObject);
				break;
			}
		}
	}

	public void DelMine(Vector3 pos)
	{
		for (int num = mineNodeTrs.childCount - 1; num >= 0; num--)
		{
			Transform child = mineNodeTrs.GetChild(num);
			if (Mathf.Abs(child.position.x - pos.x) < 0.01f && Mathf.Abs(child.position.y - pos.y) < 0.01f && Mathf.Abs(child.position.z - pos.z) < 0.01f)
			{
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Bazooka_Brust/Mine_Brust"), child.position, child.rotation) as GameObject;
				UnityEngine.Object.DestroyObject(obj, 2f);
				UnityEngine.Object.DestroyObject(child.gameObject);
				break;
			}
		}
	}

	public void PutGlue(Vector3 pos)
	{
		Ray ray = new Ray(pos + Vector3.up, Vector3.down);
		int layerMask = 1 << LayerMask.NameToLayer("Collision");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, layerMask) && LayerMask.LayerToName(hitInfo.collider.gameObject.layer) == "Collision")
		{
			Vector3 point = hitInfo.point;
			point.y -= 0.1f;
			point.z -= 0.5f;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Scene/Run/Prefab/Glue"), point, Quaternion.identity) as GameObject;
			gameObject.transform.parent = hitInfo.collider.gameObject.transform;
			UnityEngine.Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_run_glue);
		}
	}

	private void ShowProgress()
	{
		for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
		{
			Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
			if (child == null)
			{
				continue;
			}
			COMA_Creation component = child.GetComponent<COMA_Creation>();
			if (!(component == null) && processCom._infos[component.sitIndex] != null)
			{
				if (totalDistance <= 1f)
				{
					processCom._infos[component.sitIndex].PlayerProcess = 0f;
				}
				else
				{
					processCom._infos[component.sitIndex].PlayerProcess = component.transform.position.z / totalDistance;
				}
			}
		}
	}

	public bool IsFirst(Transform pTrs)
	{
		if (GetOrder(pTrs) == 0)
		{
			return true;
		}
		return false;
	}

	public void RunFinish(Transform pTrs)
	{
		COMA_Creation component = pTrs.GetComponent<COMA_Creation>();
		component.score = Mathf.FloorToInt(pTrs.position.z);
		for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
		{
			Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
			if (child != pTrs)
			{
				COMA_Creation component2 = child.GetComponent<COMA_Creation>();
				if (child.position.z > wayList[wayList.Count - 1].FindChild("Finish").position.z)
				{
					component2.score = component.score + UnityEngine.Random.Range(1, 20);
				}
				else
				{
					component2.score = Mathf.FloorToInt(child.position.z);
				}
			}
		}
		COMA_Scene.Instance.GameFinishByTime();
	}

	public int GetOrder(Transform pTrs)
	{
		int num = 0;
		for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
		{
			Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
			if (!(child == pTrs) && child.position.z > pTrs.position.z && child.position.z < wayList[wayList.Count - 1].FindChild("Finish").position.z)
			{
				num++;
			}
		}
		return num;
	}

	public void ShowInk()
	{
		inkCom.PlayEffect();
	}

	public void UseItemInfo(string a, int id, string b)
	{
		UseItemInfoLocal(a, id, b);
		COMA_CD_UseItemInfo cOMA_CD_UseItemInfo = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_USEITEMINFO) as COMA_CD_UseItemInfo;
		cOMA_CD_UseItemInfo.nameA = a;
		cOMA_CD_UseItemInfo.nameB = b;
		cOMA_CD_UseItemInfo.itemID = id;
		COMA_CommandHandler.Instance.Send(cOMA_CD_UseItemInfo);
		Debug.Log(cOMA_CD_UseItemInfo.nameA + " " + cOMA_CD_UseItemInfo.itemID + " " + cOMA_CD_UseItemInfo.nameB);
	}

	private void UseItemInfoLocal(string a, int id, string b)
	{
		UI_UsePropInfoData data = new UI_UsePropInfoData(a, b, iconTexs[id]);
		usePropInfoCom.AddUsePropInfo(data);
	}

	public void GoldAdsorb(Transform trs)
	{
		for (int num = wayList.Count - 2; num >= 0; num--)
		{
			if (trs.position.z > wayList[num].FindChild("Next").position.z)
			{
				COMA_Gold[] componentsInChildren = wayList[num + 1].GetComponentsInChildren<COMA_Gold>();
				COMA_Gold[] array = componentsInChildren;
				foreach (COMA_Gold cOMA_Gold in array)
				{
					if ((cOMA_Gold.transform.position - trs.position).sqrMagnitude < 49f)
					{
						cOMA_Gold.targetTrs = trs;
					}
				}
				break;
			}
		}
	}
}
