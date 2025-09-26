using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Tank_SceneController : MonoBehaviour
{
	public GameObject UI_WinObj;

	public GameObject UI_LoseObj;

	public static bool bLevelCreated;

	private static COMA_Tank_SceneController _instance;

	private float fGameTime = 180f;

	private float fGameTimeLeft;

	private List<COMA_Player> _players = new List<COMA_Player>();

	public Transform obstacleRoot;

	public COMA_Tank_ItemCreator[] itemCreators;

	public int[] nTeamScore = new int[2];

	public int _nKillCount;

	public int _nDieCount;

	public GameObject dustEffect;

	public GameObject TumbleweedEffect;

	public float fMinEffectTime;

	public float fMaxEffectTime = 0.5f;

	private float fNextPlayTime = 1f;

	public static COMA_Tank_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Start()
	{
		COMA_TankModel.resetShadowHeight();
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Tank);
		fGameTimeLeft = fGameTime;
		COMA_Scene.Instance.CountToStart(7);
		COMA_Buff.Instance.lastTime_ice = 2f;
		COMA_Sys.Instance.bCoverUIInput = true;
		Debug.Log("------------------------------------levelid:" + COMA_Sys.Instance.roadIDs);
		doCreateLevel(COMA_Sys.Instance.roadIDs);
	}

	private void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ENEMY_HIT, RecieveHurt);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMCREATE, RecieveCreateItem);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.ITEMDELETE, RecieveDeleteItem);
			_instance = this;
		}
	}

	protected void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ENEMY_HIT, RecieveHurt);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMCREATE, RecieveCreateItem);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.ITEMDELETE, RecieveDeleteItem);
		}
		Debug.Log("COMA_Tank_Scenecontroller disabled!");
		_instance = null;
		bLevelCreated = false;
	}

	public void AddPlayer(COMA_Player tankPlayer)
	{
		_players.Add(tankPlayer);
	}

	public void RemovePlayer(COMA_Player tankPlayer)
	{
		Debug.Log("COMA_Scene.Instance.settlementCom.gameObject.activeSelf:" + COMA_Scene.Instance.settlementCom.gameObject.activeSelf);
		if (COMA_Scene.Instance.settlementCom.gameObject.activeSelf)
		{
			return;
		}
		_players.Remove(tankPlayer);
		if (_players.Count > TankCommon.nPlayerCount / 2)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < _players.Count; i++)
		{
			if (COMA_PlayerSelf.Instance != null && _players[i] != null && !TankCommon.isAliance(COMA_PlayerSelf.Instance.sitIndex, _players[i].sitIndex))
			{
				num++;
			}
		}
		if (num == 0)
		{
			StartCoroutine(delayEnd());
		}
	}

	private IEnumerator delayEnd()
	{
		yield return new WaitForSeconds(3f);
		COMA_Scene.Instance.GameFinishByTime_Tank();
	}

	private void RecieveHurt(COMA_CommandDatas data)
	{
		COMA_CD_EnemyHit cOMA_CD_EnemyHit = data as COMA_CD_EnemyHit;
		Debug.Log("RecieveHurt:" + cOMA_CD_EnemyHit.bEnemyID + " dmg:" + cOMA_CD_EnemyHit.attackPoint);
		Transform transform = obstacleRoot.FindChild(cOMA_CD_EnemyHit.bEnemyID.ToString());
		if (transform != null)
		{
			Debug.Log("-----------find obj:" + transform.name);
			COMA_Tank_Breakable component = transform.GetComponent<COMA_Tank_Breakable>();
			if (component != null)
			{
				component.doHurt(cOMA_CD_EnemyHit.attackPoint, true);
			}
		}
	}

	private void RecieveCreateItem(COMA_CommandDatas data)
	{
		COMA_CD_CreateItem cOMA_CD_CreateItem = data as COMA_CD_CreateItem;
		Debug.Log("------------RecieveCreateItem" + cOMA_CD_CreateItem.blockIndex + "|" + cOMA_CD_CreateItem.itemIndex);
		COMA_Tank_ItemCreator cOMA_Tank_ItemCreator = itemCreators[cOMA_CD_CreateItem.blockIndex];
		cOMA_Tank_ItemCreator.doCreateItem(cOMA_CD_CreateItem.itemIndex);
	}

	private void RecieveDeleteItem(COMA_CommandDatas data)
	{
		COMA_CD_DeleteItem cOMA_CD_DeleteItem = data as COMA_CD_DeleteItem;
		if (cOMA_CD_DeleteItem != null)
		{
			Debug.Log("------------RecieveDeleteItem" + cOMA_CD_DeleteItem.blockIndex + "|" + cOMA_CD_DeleteItem.itemIndex);
			if (cOMA_CD_DeleteItem.blockIndex < 4)
			{
				COMA_Tank_ItemCreator cOMA_Tank_ItemCreator = itemCreators[cOMA_CD_DeleteItem.blockIndex];
				cOMA_Tank_ItemCreator.destoryItem();
			}
		}
	}

	private void Update()
	{
	}

	public int getScore(int nIndex)
	{
		if (nIndex < nTeamScore.Length)
		{
			return nTeamScore[nIndex];
		}
		return -1;
	}

	public int getOurScore()
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			int teamIndex = TankCommon.getTeamIndex(COMA_PlayerSelf.Instance.sitIndex);
			return getScore(teamIndex);
		}
		return 0;
	}

	public int getOppScore()
	{
		if (COMA_PlayerSelf.Instance != null)
		{
			int nIndex = ((TankCommon.getTeamIndex(COMA_PlayerSelf.Instance.sitIndex) == 0) ? 1 : 0);
			return getScore(nIndex);
		}
		return 0;
	}

	public void addScore(int nTeamIndex, int nScore)
	{
		Debug.Log("----------------------add score team index:" + nTeamIndex);
		nTeamScore[nTeamIndex] += nScore;
	}

	protected void ReceiveScoreGet(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = commandDatas as COMA_CD_PlayerScoreGet;
		if (cOMA_CD_PlayerScoreGet.curScore < 0)
		{
			COMA_PlayerSelf_Tank cOMA_PlayerSelf_Tank = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Tank;
			cOMA_PlayerSelf_Tank.AddDeadScore(cOMA_CD_PlayerScoreGet.playerId, cOMA_CD_PlayerScoreGet.addScore);
			return;
		}
		Debug.Log("+++++++++++++++++ReceiveScoreGet" + cOMA_CD_PlayerScoreGet.addScore + "id:" + cOMA_CD_PlayerScoreGet.playerId);
		Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(cOMA_CD_PlayerScoreGet.playerId.ToString());
		COMA_Creation component = transform.GetComponent<COMA_Creation>();
		component.score++;
		int teamIndex = TankCommon.getTeamIndex(component.sitIndex);
		addScore(teamIndex, cOMA_CD_PlayerScoreGet.addScore);
		if (cOMA_CD_PlayerScoreGet.playerId == COMA_PlayerSelf.Instance.id && cOMA_CD_PlayerScoreGet.curScore != 100)
		{
			_nKillCount++;
		}
	}

	private void updateEffect()
	{
		fNextPlayTime -= Time.deltaTime;
		if (fNextPlayTime < 0f)
		{
			playEnviromentEffect();
			fNextPlayTime = Random.Range(fMinEffectTime, fMaxEffectTime);
		}
	}

	private void playEnviromentEffect()
	{
		GameObject gameObject = Object.Instantiate((Random.Range(0, 2) != 0 || 1 == 0) ? TumbleweedEffect : dustEffect) as GameObject;
		float x = Random.Range(-30f, 30f);
		float z = Random.Range(-42f, 42f);
		gameObject.transform.position = new Vector3(x, 0.575f, z);
		float y = Random.Range(0, 360);
		gameObject.transform.rotation = Quaternion.Euler(0f, y, 0f);
		gameObject.transform.parent = base.transform;
		Object.Destroy(gameObject, 2f);
	}

	private void doCreateLevel(string id)
	{
		if (bLevelCreated)
		{
			return;
		}
		bLevelCreated = true;
		Debug.Log("rIDs---Create front : " + id);
		if (!(id == string.Empty) && !(id == "0"))
		{
			string[] array = id.Split(',');
			string text = "FBX/Scene/Tank/Prefab/obstacles" + array[0];
			Debug.Log("load obstacle:" + text);
			GameObject gameObject = Object.Instantiate(Resources.Load(text)) as GameObject;
			obstacleRoot = gameObject.transform;
			for (int i = 0; i < 4; i++)
			{
				int nIndex = int.Parse(array[1].Substring(i, 1));
				StartCoroutine(itemCreators[i].delayCreateItem(nIndex));
			}
		}
	}
}
