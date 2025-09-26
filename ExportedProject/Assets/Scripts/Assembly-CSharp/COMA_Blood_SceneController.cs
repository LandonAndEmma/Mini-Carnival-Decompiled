using System.Collections.Generic;
using UnityEngine;

public class COMA_Blood_SceneController : TBaseEntity
{
	private static COMA_Blood_SceneController _instance;

	public UI_WeaponSelect weaponSelectCom;

	public GameObject UI_WinObj;

	public GameObject UI_LoseObj;

	public UI_KilledBy killedByCom;

	public UI_UsePropInfoMgr usePropInfoCom;

	public Texture2D killIcon;

	public static int mapCount = 4;

	public GameObject[] mapObjs;

	public GameObject[] teamSign;

	private List<Vector3> cannonsPos = new List<Vector3>();

	private float CANNONINTERVAL = 30f;

	private GameObject itemObj;

	private float updateTeamScoreInterval;

	public static COMA_Blood_SceneController Instance
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
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.CREATEEVILITEM, ReceiveCreateEvilItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.DELETEEVILITEM, ReceiveDeleteEvilItem);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.RUN_USEITEMINFO, ReceiveUseItemInfo);
	}

	private new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.CREATEEVILITEM, ReceiveCreateEvilItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.DELETEEVILITEM, ReceiveDeleteEvilItem);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.RUN_USEITEMINFO, ReceiveUseItemInfo);
	}

	private void ReceiveCreateEvilItem(COMA_CommandDatas commandDatas)
	{
		if (COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_CreateEvilItem cOMA_CD_CreateEvilItem = commandDatas as COMA_CD_CreateEvilItem;
			OnCreateCannon(cOMA_CD_CreateEvilItem.pos);
		}
	}

	private void ReceiveDeleteEvilItem(COMA_CommandDatas commandDatas)
	{
		if (COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			OnDeleteCannon();
		}
	}

	private void ReceiveUseItemInfo(COMA_CommandDatas commandDatas)
	{
		if (!(COMA_PlayerSelf.Instance == null) && COMA_PlayerSelf.Instance.id != commandDatas.dataSender.Id)
		{
			COMA_CD_UseItemInfo cOMA_CD_UseItemInfo = commandDatas as COMA_CD_UseItemInfo;
			UseItemInfoLocal(cOMA_CD_UseItemInfo.nameA, cOMA_CD_UseItemInfo.nameB);
		}
	}

	private void Start()
	{
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput, this);
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Castle);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
	}

	private void OnDestroy()
	{
		if (TPCInputMgr.Instance != null)
		{
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput, this);
		}
	}

	public void KillInfo(string a, string b)
	{
		UseItemInfoLocal(a, b);
		COMA_CD_UseItemInfo cOMA_CD_UseItemInfo = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.RUN_USEITEMINFO) as COMA_CD_UseItemInfo;
		cOMA_CD_UseItemInfo.nameA = a;
		cOMA_CD_UseItemInfo.nameB = b;
		cOMA_CD_UseItemInfo.itemID = 0;
		COMA_CommandHandler.Instance.Send(cOMA_CD_UseItemInfo);
	}

	private void UseItemInfoLocal(string a, string b)
	{
		UI_UsePropInfoData data = new UI_UsePropInfoData(a, b, killIcon);
		usePropInfoCom.AddUsePropInfo(data);
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(6);
		return false;
	}

	private new void Update()
	{
		if (COMA_Sys.Instance.roadIDs != string.Empty)
		{
			if (mapObjs == null)
			{
				Debug.LogError("No maps!!");
			}
			int num = int.Parse(COMA_Sys.Instance.roadIDs);
			if (num >= mapObjs.Length)
			{
				Debug.LogError("No more maps!!");
			}
			for (int i = 0; i < mapObjs.Length; i++)
			{
				if (i == num)
				{
					mapObjs[i].transform.position = Vector3.zero;
					Transform transform = mapObjs[i].transform.FindChild("disorder/gun");
					if (transform != null)
					{
						for (int num2 = transform.childCount - 1; num2 >= 0; num2--)
						{
							cannonsPos.Add(transform.GetChild(num2).position);
						}
					}
				}
				else
				{
					Object.DestroyObject(mapObjs[i]);
				}
			}
			COMA_Sys.Instance.roadIDs = string.Empty;
		}
		updateTeamScoreInterval += Time.deltaTime;
		if (updateTeamScoreInterval > 1f)
		{
			updateTeamScoreInterval = 0f;
			UpdateTeamScore();
		}
	}

	public void ShowKilledInfo(string strName, RenderTexture tex)
	{
		killedByCom.gameObject.SetActive(true);
		killedByCom.SetKilledInfo(strName, tex);
		SceneTimerInstance.Instance.Remove(HideKilledInfo);
		SceneTimerInstance.Instance.Add(5f, HideKilledInfo);
	}

	public bool HideKilledInfo()
	{
		killedByCom.gameObject.SetActive(false);
		return false;
	}

	public void UpdateTeamScore()
	{
		COMA_Player[] componentsInChildren = COMA_Scene.Instance.playerNodeTrs.GetComponentsInChildren<COMA_Player>();
		if (componentsInChildren == null)
		{
			return;
		}
		COMA_PlayerSelf_Blood cOMA_PlayerSelf_Blood = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Blood;
		if (cOMA_PlayerSelf_Blood == null)
		{
			return;
		}
		COMA_Scene.Instance.teamScore[0] = 0;
		COMA_Scene.Instance.teamScore[1] = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].team == Team.Team1)
			{
				COMA_Scene.Instance.teamScore[0] += componentsInChildren[i].score;
				COMA_Scene.Instance.teamScore[1] += cOMA_PlayerSelf_Blood.GetDeadScore(componentsInChildren[i].id);
			}
			else if (componentsInChildren[i].team == Team.Team2)
			{
				COMA_Scene.Instance.teamScore[1] += componentsInChildren[i].score;
				COMA_Scene.Instance.teamScore[0] += cOMA_PlayerSelf_Blood.GetDeadScore(componentsInChildren[i].id);
			}
		}
		if (cOMA_PlayerSelf_Blood.team == Team.Team1)
		{
			UIIngame_BloodUI.Instance._info.NumSelfTeamKill = COMA_Scene.Instance.teamScore[0];
			UIIngame_BloodUI.Instance._info.NumOpponentKill = COMA_Scene.Instance.teamScore[1];
		}
		else if (cOMA_PlayerSelf_Blood.team == Team.Team2)
		{
			UIIngame_BloodUI.Instance._info.NumSelfTeamKill = COMA_Scene.Instance.teamScore[1];
			UIIngame_BloodUI.Instance._info.NumOpponentKill = COMA_Scene.Instance.teamScore[0];
		}
	}

	public bool CreateCannon()
	{
		if (cannonsPos.Count <= 0)
		{
			return false;
		}
		int index = Random.Range(0, cannonsPos.Count);
		Vector3 pos = cannonsPos[index];
		OnCreateCannon(pos);
		COMA_CD_CreateEvilItem cOMA_CD_CreateEvilItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.CREATEEVILITEM) as COMA_CD_CreateEvilItem;
		cOMA_CD_CreateEvilItem.pos = pos;
		COMA_CommandHandler.Instance.Send(cOMA_CD_CreateEvilItem);
		return false;
	}

	public void DeleteCannon()
	{
		OnDeleteCannon();
		SceneTimerInstance.Instance.Add(30f, CreateCannon);
		COMA_CD_DeleteEvilItem commandDatas = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.DELETEEVILITEM) as COMA_CD_DeleteEvilItem;
		COMA_CommandHandler.Instance.Send(commandDatas);
	}

	private void OnCreateCannon(Vector3 pos)
	{
		if (!(COMA_PlayerSelf.Instance == null))
		{
			Object.DestroyObject(itemObj);
			itemObj = Object.Instantiate(Resources.Load("FBX/Item/PFB/PFB_Item_Blood_W008")) as GameObject;
			itemObj.name = itemObj.name.Replace("(Clone)", string.Empty);
			itemObj.transform.position = pos;
		}
	}

	private void OnDeleteCannon()
	{
		if (!(itemObj == null))
		{
			GameObject obj = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), itemObj.transform.position, itemObj.transform.rotation) as GameObject;
			Object.DestroyObject(obj, 2f);
			Object.DestroyObject(itemObj);
		}
	}
}
