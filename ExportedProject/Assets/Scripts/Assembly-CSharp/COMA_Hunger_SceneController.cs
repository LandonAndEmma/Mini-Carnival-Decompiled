using System;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Hunger_SceneController : MonoBehaviour
{
	private static COMA_Hunger_SceneController _instance;

	public GameObject platformNodeObj;

	public UIInGame_Tips famineCom;

	public UIInGame_Tips itemCom;

	public UI_HungerRedMgr redScreenCom;

	public Transform itemPointTrs;

	public Transform itemPoint0Trs;

	public Transform itemPoint1Trs;

	public Transform itemPoint2Trs;

	public Transform itemPoint3Trs;

	public Transform itemNodeTrs;

	public GameObject[] itemObjs;

	private List<Vector3> ltBornPoints = new List<Vector3>();

	private List<int> ltPosIndex = new List<int>();

	private float itemRefreshDuration = 5f;

	private float ITEMREFRESHDURATION = 15f;

	[NonSerialized]
	public Transform mineNodeTrs;

	private List<GameObject> ltMineObjs = new List<GameObject>();

	public GameObject cmrDeathView;

	public UI_KilledBy killedByCom;

	public static COMA_Hunger_SceneController Instance
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
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMCREATE, ReceiveCreateItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PUTMINE, ReceivePutMine);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.DELMINE, ReceiveDelMine);
	}

	private void OnDisable()
	{
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMCREATE, ReceiveCreateItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PUTMINE, ReceivePutMine);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.DELMINE, ReceiveDelMine);
	}

	private void ReceiveCreateItem(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_CreateItem cOMA_CD_CreateItem = commandDatas as COMA_CD_CreateItem;
			CreateItem(cOMA_CD_CreateItem.blockIndex, cOMA_CD_CreateItem.itemIndex);
		}
	}

	private void ReceiveDeleteItem(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_DeleteItem cOMA_CD_DeleteItem = commandDatas as COMA_CD_DeleteItem;
			DeleteItem(cOMA_CD_DeleteItem.blockIndex);
		}
	}

	private void ReceivePutMine(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_PutMine cOMA_CD_PutMine = commandDatas as COMA_CD_PutMine;
			PutMine(cOMA_CD_PutMine.pos);
		}
	}

	private void ReceiveDelMine(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_DelMine cOMA_CD_DelMine = commandDatas as COMA_CD_DelMine;
			DelMine(cOMA_CD_DelMine.pos);
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Hunger);
		float length = platformNodeObj.animation.clip.length;
		platformNodeObj.animation.Play();
		COMA_Scene.Instance.playerNodeTrs.animation.Play();
		SceneTimerInstance.Instance.Add(length, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
		ltBornPoints.Add(itemPoint0Trs.position);
		ltBornPoints.Add(itemPoint1Trs.position);
		ltBornPoints.Add(itemPoint2Trs.position);
		ltBornPoints.Add(itemPoint3Trs.position);
		ltPosIndex.Add(0);
		ltPosIndex.Add(1);
		ltPosIndex.Add(2);
		ltPosIndex.Add(3);
		UnityEngine.Object.DestroyObject(itemPoint0Trs.gameObject);
		UnityEngine.Object.DestroyObject(itemPoint1Trs.gameObject);
		UnityEngine.Object.DestroyObject(itemPoint2Trs.gameObject);
		UnityEngine.Object.DestroyObject(itemPoint3Trs.gameObject);
		for (int i = 0; i < itemPointTrs.childCount; i++)
		{
			Vector3 position = itemPointTrs.GetChild(i).position;
			ltBornPoints.Add(position);
			ltPosIndex.Add(i + 4);
		}
		UnityEngine.Object.DestroyObject(itemPointTrs.gameObject);
		mineNodeTrs = new GameObject("MineNodeTrs").transform;
		ltPosIndex.RemoveAt(0);
		CreateItemDefault(0, 5);
		ltPosIndex.RemoveAt(0);
		CreateItemDefault(1, 1);
		ltPosIndex.RemoveAt(0);
		CreateItemDefault(2, 4);
		ltPosIndex.RemoveAt(0);
		CreateItemDefault(3, 4);
		cmrDeathView.SetActive(false);
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(2);
		return false;
	}

	private void Update()
	{
		if (!COMA_Scene.Instance.runingGameOver && COMA_Scene.Instance.IsGameOver())
		{
			COMA_Scene.Instance.runingGameOver = true;
			COMA_Scene.Instance.GameFinishBySurvive();
		}
		if (COMA_PlayerSelf.Instance != null && !COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id))
		{
			return;
		}
		itemRefreshDuration += Time.deltaTime;
		if (!(itemRefreshDuration > ITEMREFRESHDURATION))
		{
			return;
		}
		itemRefreshDuration = 0f;
		int num = 4;
		if (ltPosIndex.Count < ltBornPoints.Count / 2)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(0, ltPosIndex.Count);
			int num2 = ltPosIndex[index];
			ltPosIndex.RemoveAt(index);
			int num3 = UnityEngine.Random.Range(0, 9);
			if (num3 == 6)
			{
				num3 = 5;
			}
			if (num3 == 8)
			{
				num3 = 7;
			}
			CreateItem(num2, num3);
			COMA_CD_CreateItem cOMA_CD_CreateItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMCREATE) as COMA_CD_CreateItem;
			cOMA_CD_CreateItem.blockIndex = (byte)num2;
			cOMA_CD_CreateItem.itemIndex = (byte)num3;
			COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem);
		}
	}

	private void CreateItemDefault(int posID, int itemID)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(itemObjs[itemID], ltBornPoints[posID], Quaternion.identity) as GameObject;
		string newValue = "|" + posID;
		gameObject.name = gameObject.name.Replace("(Clone)", newValue);
		gameObject.transform.parent = itemNodeTrs;
	}

	private void CreateItem(int posID, int itemID)
	{
		itemCom.OpenInGameTips();
		GameObject gameObject = UnityEngine.Object.Instantiate(itemObjs[itemID], ltBornPoints[posID], Quaternion.identity) as GameObject;
		string newValue = "|" + posID;
		gameObject.name = gameObject.name.Replace("(Clone)", newValue);
		gameObject.transform.parent = itemNodeTrs;
	}

	public void DeleteItem(int posID)
	{
		if (posID >= 4)
		{
			ltPosIndex.Add(posID);
		}
		string value = "|" + posID;
		for (int i = 0; i < itemNodeTrs.childCount; i++)
		{
			Transform child = itemNodeTrs.GetChild(i);
			if (child.name.EndsWith(value))
			{
				UnityEngine.Object.DestroyObject(child.gameObject);
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), child.position, child.rotation) as GameObject;
				UnityEngine.Object.DestroyObject(obj, 2f);
				break;
			}
		}
	}

	public void PutMine(Vector3 pos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Prop_Mine"), pos, Quaternion.identity) as GameObject;
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		gameObject.transform.parent = mineNodeTrs;
		ltMineObjs.Add(gameObject);
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

	public void StartRedScreen()
	{
		redScreenCom.gameObject.SetActive(true);
		SceneTimerInstance.Instance.Add(1f, EndRedScreen);
	}

	public bool EndRedScreen()
	{
		redScreenCom.gameObject.SetActive(false);
		return false;
	}

	public void ShowKilledInfo(string strName, RenderTexture tex)
	{
		killedByCom.gameObject.SetActive(true);
		killedByCom.SetKilledInfo(strName, tex);
		SceneTimerInstance.Instance.Add(2f, HideKilledInfo);
	}

	public bool HideKilledInfo()
	{
		killedByCom.gameObject.SetActive(false);
		return false;
	}
}
