using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMA_DropFight_SceneController : MonoBehaviour
{
	private static COMA_DropFight_SceneController _instance;

	public GameObject jumpObj;

	public GameObject fireObj;

	public GameObject magazineObj;

	public GameObject goldGetPtlPrefab;

	public Transform blockNodeTrs;

	private int blockX = 6;

	private int blockZ = 6;

	private int blockSize = 2;

	private int blockCount;

	private List<int> blockIndexList = new List<int>();

	private Hashtable blockTrsTable = new Hashtable();

	public Transform firePlatformTrs;

	public Transform[] standPlatformTrs;

	private Transform itemNodeTrs;

	private float itemRefreshDuration = 5f;

	private float ITEMREFRESHDURATION = 15f;

	private float goldRefreshDuration;

	private float GOLDREFRESHDURATION = 8f;

	public GameObject deathTipObj;

	public static COMA_DropFight_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		blockCount = blockX * blockZ;
		for (int i = 0; i < blockNodeTrs.childCount; i++)
		{
			Transform child = blockNodeTrs.GetChild(i);
			child.name = "Block" + i.ToString("d2");
			blockIndexList.Add(i);
			blockTrsTable.Add(i, child.position);
		}
	}

	private void OnEnable()
	{
		_instance = this;
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMCREATE, ReceiveCreateItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.BLOCKDELETE, ReceiveBlockBroken);
	}

	private void OnDisable()
	{
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMCREATE, ReceiveCreateItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.BLOCKDELETE, ReceiveBlockBroken);
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
		if (COMA_PlayerSelf.Instance == null || COMA_PlayerSelf.Instance.id == commandDatas.dataSender.Id)
		{
			return;
		}
		COMA_CD_DeleteItem cOMA_CD_DeleteItem = commandDatas as COMA_CD_DeleteItem;
		DeleteItem(cOMA_CD_DeleteItem.blockIndex, cOMA_CD_DeleteItem.itemIndex);
		if (cOMA_CD_DeleteItem.itemIndex != 1)
		{
			return;
		}
		Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(cOMA_CD_DeleteItem.dataSender.Id.ToString());
		if (transform != null)
		{
			COMA_Creation component = transform.GetComponent<COMA_Creation>();
			if (component != null)
			{
				component.goldGet++;
			}
		}
	}

	private void ReceiveBlockBroken(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_DeleteBlock cOMA_CD_DeleteBlock = commandDatas as COMA_CD_DeleteBlock;
			DeleteBlock(cOMA_CD_DeleteBlock.id);
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Rocket);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
		ChangeMode_Jump();
		itemNodeTrs = new GameObject().transform;
		itemNodeTrs.name = "itemNodeTrs";
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(0);
		return false;
	}

	public void ChangeMode_Jump()
	{
		jumpObj.SetActive(true);
		fireObj.SetActive(false);
		magazineObj.SetActive(false);
	}

	public void ChangeMode_Fire()
	{
		jumpObj.SetActive(false);
		fireObj.SetActive(true);
		magazineObj.SetActive(true);
	}

	public bool GameFinish()
	{
		COMA_Scene.Instance.GameFinishBySurvive();
		return false;
	}

	private void Update()
	{
		if (COMA_PlayerSelf.Instance == null)
		{
			return;
		}
		if (!COMA_Scene.Instance.runingGameOver && COMA_Scene.Instance.IsGameOver())
		{
			COMA_Scene.Instance.runingGameOver = true;
			SceneTimerInstance.Instance.Add(2f, GameFinish);
		}
		if (COMA_PlayerSelf.Instance != null && !COMA_Network.Instance.IsRoomMaster(COMA_PlayerSelf.Instance.id))
		{
			return;
		}
		if (blockIndexList.Count > 0)
		{
			itemRefreshDuration += Time.deltaTime;
			if (itemRefreshDuration > ITEMREFRESHDURATION)
			{
				itemRefreshDuration = 0f;
				int blockIDWithoutItems = GetBlockIDWithoutItems();
				if (blockIDWithoutItems >= 0)
				{
					CreateItem(blockIDWithoutItems, 0);
					COMA_CD_CreateItem cOMA_CD_CreateItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMCREATE) as COMA_CD_CreateItem;
					cOMA_CD_CreateItem.blockIndex = (byte)blockIDWithoutItems;
					cOMA_CD_CreateItem.itemIndex = 0;
					COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem);
				}
			}
		}
		int num = blockX * blockZ / 2;
		if (blockIndexList.Count <= num)
		{
			return;
		}
		goldRefreshDuration += Time.deltaTime;
		if (!(goldRefreshDuration > GOLDREFRESHDURATION))
		{
			return;
		}
		goldRefreshDuration = 0f;
		int num2 = Random.Range(2, 6);
		if (num2 > num)
		{
			num2 = num;
		}
		while (num2-- > 0)
		{
			int blockIDWithoutItems2 = GetBlockIDWithoutItems();
			if (blockIDWithoutItems2 >= 0)
			{
				CreateItem(blockIDWithoutItems2, 1);
				COMA_CD_CreateItem cOMA_CD_CreateItem2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMCREATE) as COMA_CD_CreateItem;
				cOMA_CD_CreateItem2.blockIndex = (byte)blockIDWithoutItems2;
				cOMA_CD_CreateItem2.itemIndex = 1;
				COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem2);
			}
		}
		COMA_PlayerSelf_DropFight cOMA_PlayerSelf_DropFight = COMA_PlayerSelf.Instance as COMA_PlayerSelf_DropFight;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Gold_appear, cOMA_PlayerSelf_DropFight.cmrCom.transform);
	}

	private int GetBlockID()
	{
		if (blockIndexList.Count <= 0)
		{
			return -1;
		}
		int index = Random.Range(0, blockIndexList.Count);
		return blockIndexList[index];
	}

	private int GetBlockIDWithoutItems()
	{
		List<int> list = new List<int>(blockIndexList);
		int childCount = itemNodeTrs.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = itemNodeTrs.GetChild(i);
			string[] array = child.name.Split('_');
			int item = int.Parse(array[0]);
			list.Remove(item);
		}
		if (list.Count <= 0)
		{
			return -1;
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	private Vector3 GetBlockPosFromID(int id)
	{
		Vector3 vector = (Vector3)blockTrsTable[id];
		return vector + new Vector3(0f, 1f, 0f);
	}

	public Vector3 GetAvailablePosition()
	{
		int blockID = GetBlockID();
		if (blockID >= 0)
		{
			return GetBlockPosFromID(blockID);
		}
		return Vector3.zero;
	}

	public void SendBlockBroken(GameObject blockObj)
	{
		int num = int.Parse(blockObj.name.Substring(5));
		DeleteBlock(num);
		COMA_CD_DeleteBlock cOMA_CD_DeleteBlock = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.BLOCKDELETE) as COMA_CD_DeleteBlock;
		cOMA_CD_DeleteBlock.id = (byte)num;
		COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteBlock);
	}

	private void DeleteBlock(int id)
	{
		blockIndexList.Remove(id);
		GameObject obj = blockNodeTrs.FindChild("Block" + id.ToString("d2")).gameObject;
		Object.DestroyObject(obj);
	}

	public int GetLeftBlockCount()
	{
		return blockIndexList.Count;
	}

	private void CreateItem(int blockID, int itemID)
	{
		Vector3 position = GetBlockPosFromID(blockID) + Vector3.up * 0.02f;
		string empty = string.Empty;
		empty = ((itemID != 0) ? "FBX/Item/PFB/PFB_Item_Gold" : "FBX/Item/PFB/PFB_Item_W001");
		GameObject gameObject = Object.Instantiate(Resources.Load(empty), position, Quaternion.identity) as GameObject;
		gameObject.name = blockID + "_" + itemID;
		gameObject.transform.parent = itemNodeTrs;
		if (itemID == 0)
		{
			Object.DestroyObject(gameObject, ITEMREFRESHDURATION - 5f);
		}
		else
		{
			Object.DestroyObject(gameObject, GOLDREFRESHDURATION - 1f);
		}
	}

	public void DeleteItem(int blockID, int itemID)
	{
		string text = blockID + "_" + itemID;
		Transform transform = itemNodeTrs.FindChild(text);
		if (transform != null)
		{
			Object.DestroyObject(transform.gameObject);
			switch (itemID)
			{
			case 0:
			{
				GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Transmission_Deliver/Transmission_Deliver"), transform.position, transform.rotation) as GameObject;
				Object.DestroyObject(obj, 1.5f);
				break;
			}
			case 1:
			{
				GameObject gameObject = Object.Instantiate(goldGetPtlPrefab) as GameObject;
				gameObject.transform.position = transform.position + Vector3.up * 0.5f;
				Object.DestroyObject(gameObject, 2f);
				break;
			}
			}
		}
	}

	public void ShowDeathTip()
	{
		deathTipObj.SetActive(true);
		SceneTimerInstance.Instance.Add(2f, HideDeathTip);
	}

	public bool HideDeathTip()
	{
		deathTipObj.SetActive(false);
		return false;
	}
}
