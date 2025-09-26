using UnityEngine;

public class COMA_Maze_SceneController : MonoBehaviour
{
	private static COMA_Maze_SceneController _instance;

	public GameObject jumpObj;

	public GameObject goldGetPtlPrefab;

	public Transform goldNodeTrs;

	public Transform[] itemPointTrs;

	private GameObject itemObj;

	private int markID = -1;

	public static COMA_Maze_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < goldNodeTrs.childCount; i++)
		{
			goldNodeTrs.GetChild(i).name = i.ToString("d3");
		}
	}

	private void OnEnable()
	{
		_instance = this;
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.CREATEEVILITEM, ReceiveCreateEvilItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.DELETEEVILITEM, ReceiveDeleteEvilItem);
	}

	private void OnDisable()
	{
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMDELETE, ReceiveDeleteItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.CREATEEVILITEM, ReceiveCreateEvilItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.DELETEEVILITEM, ReceiveDeleteEvilItem);
	}

	private void ReceiveDeleteItem(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_DeleteItem cOMA_CD_DeleteItem = commandDatas as COMA_CD_DeleteItem;
			DeleteGold(cOMA_CD_DeleteItem.itemIndex);
		}
	}

	private void ReceiveCreateEvilItem(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_CreateEvilItem cOMA_CD_CreateEvilItem = commandDatas as COMA_CD_CreateEvilItem;
			OnCreateEvilItem(cOMA_CD_CreateEvilItem.pos);
		}
	}

	private void ReceiveDeleteEvilItem(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			OnDeleteEvilItem();
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Treasure);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
		SetJumpEnable(false);
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(4);
		return false;
	}

	private void Update()
	{
		if (!COMA_Scene.Instance.runingGameOver && COMA_Scene.Instance.IsGameOver())
		{
			COMA_Scene.Instance.runingGameOver = true;
			COMA_Scene.Instance.GameFinishBySurvive();
		}
	}

	public void SetJumpEnable(bool bEnable)
	{
		jumpObj.SetActive(bEnable);
	}

	public void DeleteGold(int goldID)
	{
		Transform transform = goldNodeTrs.FindChild(goldID.ToString("d3"));
		if (transform != null)
		{
			GameObject gameObject = Object.Instantiate(goldGetPtlPrefab) as GameObject;
			gameObject.transform.position = transform.position + Vector3.up;
			Object.DestroyObject(gameObject, 2f);
			Object.DestroyObject(transform.gameObject);
		}
	}

	public bool CreateEvilItem()
	{
		int num = Random.Range(0, itemPointTrs.Length);
		Vector3 position = itemPointTrs[num].position;
		OnCreateEvilItem(position);
		COMA_CD_CreateEvilItem cOMA_CD_CreateEvilItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.CREATEEVILITEM) as COMA_CD_CreateEvilItem;
		cOMA_CD_CreateEvilItem.pos = position;
		COMA_CommandHandler.Instance.Send(cOMA_CD_CreateEvilItem);
		return false;
	}

	public void DeleteEvilItem()
	{
		OnDeleteEvilItem();
		COMA_CD_DeleteEvilItem commandDatas = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.DELETEEVILITEM) as COMA_CD_DeleteEvilItem;
		COMA_CommandHandler.Instance.Send(commandDatas);
	}

	private void OnCreateEvilItem(Vector3 pos)
	{
		if (COMA_PlayerSelf.Instance == null)
		{
			return;
		}
		Object.DestroyObject(itemObj);
		if (UIInGame_DirTagMgr.Instance != null && markID >= 0)
		{
			UIInGame_DirTagMgr.Instance.DelTagInfo(markID);
			markID = -1;
		}
		itemObj = Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Item_Evil")) as GameObject;
		itemObj.name = itemObj.name.Replace("(Clone)", string.Empty);
		itemObj.transform.position = pos;
		if (UIInGame_DirTagMgr.Instance != null)
		{
			COMA_PlayerSelf_Maze cOMA_PlayerSelf_Maze = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Maze;
			if (cOMA_PlayerSelf_Maze.bShowItem)
			{
				markID = UIInGame_DirTagMgr.Instance.AddTagInfo(itemObj.transform);
				UIInGame_DirTagMgr.Instance.ShowTagInfo(markID);
			}
		}
	}

	private void OnDeleteEvilItem()
	{
		if (!(itemObj == null))
		{
			GameObject obj = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), itemObj.transform.position, itemObj.transform.rotation) as GameObject;
			Object.DestroyObject(obj, 2f);
			if (UIInGame_DirTagMgr.Instance != null && markID >= 0)
			{
				UIInGame_DirTagMgr.Instance.DelTagInfo(markID);
				markID = -1;
			}
			Object.DestroyObject(itemObj);
			itemObj = null;
		}
	}
}
