using System;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Role.C2S;
using UnityEngine;

public class UIRPG_CheckPointsVertex : MonoBehaviour
{
	public enum UIRPG_CheckPointsVertexStat
	{
		KLock = 0,
		KNpc = 1,
		KNpcThrough = 2,
		KBoss = 3,
		KBossThrough = 4,
		KPlayer = 5,
		KGold = 6
	}

	private int _vertexIndex;

	[SerializeField]
	private UISprite[] _displaySprite;

	[SerializeField]
	private UILabel _checkPointIndex;

	[SerializeField]
	private UISprite _playInfoBkSprite;

	[SerializeField]
	private UILabel _playNameLabel;

	[SerializeField]
	private UILabel _playLvLabel;

	[SerializeField]
	private GameObject _rpgUIBossPrefab;

	[HideInInspector]
	public GameObject _rpgUIBossQuote;

	[SerializeField]
	private GameObject _rpgUIVictoryPrefab;

	private GameObject _rpgUIVictoryQuote;

	[SerializeField]
	private GameObject _rpgUITreasurePrefab;

	[HideInInspector]
	public GameObject _rpgUITreasureQuote;

	[SerializeField]
	private GameObject _rpgUITreasureExplosionPrefab;

	[SerializeField]
	private GameObject _rpgUIFireworksPrefab;

	private UIRPG_CheckPointsVertexStat _vertexStat;

	private uint _roleId;

	private uint _startTime;

	private List<int> _preLst;

	private List<int> _nextLst;

	private Dictionary<Vector3, bool> _directionDict = new Dictionary<Vector3, bool>();

	private Dictionary<UIRPG_CheckPointsVertexMap, UIRPG_CheckPointsVertexMap> _mapDict = new Dictionary<UIRPG_CheckPointsVertexMap, UIRPG_CheckPointsVertexMap>();

	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	public int VertexIndex
	{
		get
		{
			return _vertexIndex;
		}
		set
		{
			_vertexIndex = value;
		}
	}

	public UISprite[] DisplaySprite
	{
		get
		{
			return _displaySprite;
		}
	}

	public GameObject RPGUITreasureExplosionPrefab
	{
		get
		{
			return _rpgUITreasureExplosionPrefab;
		}
	}

	public UIRPG_CheckPointsVertexStat VertexStat
	{
		get
		{
			return _vertexStat;
		}
		set
		{
			_vertexStat = value;
		}
	}

	public uint Role_id
	{
		get
		{
			return _roleId;
		}
		set
		{
			_roleId = value;
		}
	}

	public uint StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
		}
	}

	public List<int> PreLst
	{
		get
		{
			return _preLst;
		}
		set
		{
			_preLst = value;
		}
	}

	public List<int> NextLst
	{
		get
		{
			return _nextLst;
		}
		set
		{
			_nextLst = value;
		}
	}

	public Dictionary<UIRPG_CheckPointsVertexMap, UIRPG_CheckPointsVertexMap> MapDict
	{
		get
		{
			return _mapDict;
		}
	}

	public UIRPG_CheckPointsVertexMgr VertexMgr
	{
		get
		{
			return _vertexMgr;
		}
		set
		{
			_vertexMgr = value;
		}
	}

	public void GetEightDirectionPoints()
	{
		_directionDict.Clear();
		float num = _vertexMgr.GapDistance[0];
		Vector3 vector = new Vector3(num, num, base.transform.localPosition.z);
		for (int i = 0; i < 8; i++)
		{
			Vector3 vector2 = new Vector3(vector.x * Mathf.Cos((float)Math.PI * (float)i / 4f), vector.y * Mathf.Sin((float)Math.PI * (float)i / 4f), vector.z);
			Vector3 key = vector2 + base.transform.localPosition;
			_directionDict.Add(key, false);
		}
	}

	public void GetNextMapPoints()
	{
		for (int i = 0; i < _nextLst.Count; i++)
		{
			UIRPG_CheckPointsVertex uIRPG_CheckPointsVertex = _vertexMgr.VertexAllDict[_nextLst[i] - 1];
			Vector3 lhs = uIRPG_CheckPointsVertex.transform.localPosition - base.transform.localPosition;
			Vector3 vector = Vector3.zero;
			float num = float.MaxValue;
			foreach (Vector3 key in _directionDict.Keys)
			{
				if (!_directionDict[key] && num > Mathf.Abs(Vector3.Dot(lhs, key - uIRPG_CheckPointsVertex.transform.localPosition)))
				{
					num = Mathf.Abs(Vector3.Dot(lhs, key - uIRPG_CheckPointsVertex.transform.localPosition));
					vector = key;
				}
			}
			_directionDict[vector] = true;
			num = float.MaxValue;
			Vector3 vector2 = Vector3.zero;
			foreach (Vector3 key2 in uIRPG_CheckPointsVertex._directionDict.Keys)
			{
				if (!uIRPG_CheckPointsVertex._directionDict[key2] && num > Mathf.Abs(Vector3.Dot(lhs, key2 - base.transform.localPosition)))
				{
					num = Mathf.Abs(Vector3.Dot(lhs, key2 - base.transform.localPosition));
					vector2 = key2;
				}
			}
			uIRPG_CheckPointsVertex._directionDict[vector2] = true;
			_mapDict.Add(new UIRPG_CheckPointsVertexMap(_vertexIndex, vector), new UIRPG_CheckPointsVertexMap(_nextLst[i] - 1, vector2));
		}
	}

	public void BoxDataChanged()
	{
		base.gameObject.SetActive(true);
		SetActiveForSprite(false);
		_checkPointIndex.text = (_vertexIndex + 1).ToString();
		_checkPointIndex.gameObject.SetActive(true);
		_playInfoBkSprite.gameObject.SetActive(false);
		_playNameLabel.gameObject.SetActive(false);
		_playLvLabel.gameObject.SetActive(false);
		if (_vertexIndex == UIDataBufferCenter.Instance.CurBattleLevelIndex && UIDataBufferCenter.Instance.CurBattleResult_Common)
		{
			Debug.Log("_vertexMgr.CurVertexIndex = " + _vertexMgr.CurVertexIndex);
			Debug.Log("fjakjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj");
			GameObject gameObject = UnityEngine.Object.Instantiate(_rpgUIFireworksPrefab) as GameObject;
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			UnityEngine.Object.Destroy(gameObject, 5f);
			UIDataBufferCenter.Instance.CurBattleResult_Common = false;
		}
		switch (_vertexStat)
		{
		case UIRPG_CheckPointsVertexStat.KNpc:
			_displaySprite[0].gameObject.SetActive(true);
			break;
		case UIRPG_CheckPointsVertexStat.KNpcThrough:
			_displaySprite[1].gameObject.SetActive(true);
			break;
		case UIRPG_CheckPointsVertexStat.KBoss:
			if (_vertexIndex != 99)
			{
				_displaySprite[2].gameObject.SetActive(true);
			}
			else if (_vertexIndex == 99)
			{
				_displaySprite[7].gameObject.SetActive(true);
				_rpgUIBossQuote = UnityEngine.Object.Instantiate(_rpgUIBossPrefab) as GameObject;
				_rpgUIBossQuote.transform.parent = base.gameObject.transform;
				_rpgUIBossQuote.transform.localPosition = Vector3.zero;
				_rpgUIBossQuote.transform.localScale = Vector3.one;
			}
			break;
		case UIRPG_CheckPointsVertexStat.KBossThrough:
			if (_vertexIndex != 99)
			{
				_displaySprite[3].gameObject.SetActive(true);
			}
			else if (_vertexIndex == 99)
			{
				_displaySprite[6].gameObject.SetActive(true);
			}
			break;
		case UIRPG_CheckPointsVertexStat.KPlayer:
			_displaySprite[4].gameObject.SetActive(true);
			_playNameLabel.gameObject.SetActive(true);
			_playLvLabel.gameObject.SetActive(true);
			UIDataBufferCenter.Instance.FetchPlayerRPGData(_roleId, delegate(PlayerRpgDataCmd playData)
			{
				if (playData != null)
				{
					_playLvLabel.text = "lv." + playData.m_rpg_level;
					UIDataBufferCenter.Instance.FetchPlayerProfile(playData.who, delegate(WatchRoleInfo roleInfo)
					{
						if (roleInfo == null)
						{
							_playNameLabel.text = string.Empty;
						}
						else if (roleInfo.m_name.Length > 5)
						{
							_playNameLabel.text = roleInfo.m_name.Substring(0, 5) + "...";
						}
						else
						{
							_playNameLabel.text = roleInfo.m_name;
						}
					});
				}
			});
			break;
		case UIRPG_CheckPointsVertexStat.KGold:
			_displaySprite[5].gameObject.SetActive(true);
			Debug.Log("_rpgUITreasurePrefab = " + _rpgUITreasurePrefab.name);
			_rpgUITreasureQuote = UnityEngine.Object.Instantiate(_rpgUITreasurePrefab) as GameObject;
			_rpgUITreasureQuote.transform.parent = base.gameObject.transform;
			_rpgUITreasureQuote.transform.localPosition = Vector3.zero;
			_rpgUITreasureQuote.transform.localScale = Vector3.one;
			break;
		}
		if (_vertexStat == UIRPG_CheckPointsVertexStat.KBossThrough && _vertexIndex == 99)
		{
			Debug.Log("44444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444");
			_rpgUIVictoryQuote = UnityEngine.Object.Instantiate(_rpgUIVictoryPrefab) as GameObject;
			_rpgUIVictoryQuote.transform.parent = base.gameObject.transform;
			_rpgUIVictoryQuote.transform.localPosition = Vector3.zero;
			_rpgUIVictoryQuote.transform.localScale = Vector3.one;
			if (_rpgUIBossQuote != null)
			{
				UnityEngine.Object.Destroy(_rpgUIBossQuote);
			}
			UIDataBufferCenter.Instance.CurBattleResult_Common = false;
		}
	}

	private void SetActiveForSprite(bool isActive)
	{
		Debug.Log("private void SetActiveForSprite(bool isActive)");
		for (int i = 0; i < _displaySprite.Length - 2; i++)
		{
			_displaySprite[i].gameObject.SetActive(isActive);
		}
	}

	public void OnClick()
	{
		Debug.Log("UIRPG_CheckPointsVertex : OnClick");
		_vertexMgr.CurVertexIndex = _vertexIndex;
		_vertexMgr.CurVertexType = _vertexStat;
		_vertexMgr._specialDropOut = true;
		Debug.Log("MAP POINT index:  " + _vertexIndex);
		switch (_vertexStat)
		{
		case UIRPG_CheckPointsVertexStat.KNpc:
			_displaySprite[8].gameObject.SetActive(true);
			VerifyMd5();
			base.gameObject.transform.localScale = 1.2f * Vector3.one;
			break;
		case UIRPG_CheckPointsVertexStat.KNpcThrough:
			break;
		case UIRPG_CheckPointsVertexStat.KBoss:
			if (_vertexIndex != 99)
			{
				_displaySprite[10].gameObject.SetActive(true);
			}
			else if (_vertexIndex == 99)
			{
				_displaySprite[15].gameObject.SetActive(true);
			}
			VerifyMd5();
			base.gameObject.transform.localScale = 1.2f * Vector3.one;
			break;
		case UIRPG_CheckPointsVertexStat.KBossThrough:
			break;
		case UIRPG_CheckPointsVertexStat.KGold:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Coin_collect);
			_vertexMgr.PopEntity.BlockMsgBox(UIMessageBoxMgr.EMessageBoxType.GetItems);
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			ReqGainGoldCmd reqGainGoldCmd = new ReqGainGoldCmd();
			reqGainGoldCmd.m_map_point = (byte)_vertexIndex;
			_vertexMgr.CollectGoldQueue.Clear();
			_vertexMgr.CollectGoldQueue.Enqueue(_vertexIndex);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reqGainGoldCmd);
			break;
		}
		case UIRPG_CheckPointsVertexStat.KPlayer:
			_vertexMgr._specialDropOut = false;
			_displaySprite[12].gameObject.SetActive(true);
			_vertexMgr.PopUpPlayerObj.SetActive(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Map_InitEnemyInfo, null, null);
			base.gameObject.transform.localScale = 1.2f * Vector3.one;
			break;
		}
	}

	private void VerifyMd5()
	{
		Debug.Log("private void VerifyMd5()");
		string levelConfigNameByID = UIGolbalStaticFun.GetLevelConfigNameByID(_vertexIndex + 1);
		string fileContent = COMA_FileIO.LoadFile("Levels", levelConfigNameByID);
		string fileMD = COMA_FileIO.GetFileMD5(fileContent);
		if (fileMD.Equals(UIDataBufferCenter.Instance.RpgConfig_LevelValid[_vertexIndex]))
		{
			_vertexMgr.PopUpBossObj.SetActive(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Map_InitEnemyInfo, null, null);
		}
		else
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			DragConfigFileCmd dragConfigFileCmd = new DragConfigFileCmd();
			dragConfigFileCmd.fileName = levelConfigNameByID;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, dragConfigFileCmd);
		}
		Debug.Log("end private void VerifyMd5()");
	}

	public void OnDestroy()
	{
		if (_rpgUITreasureQuote != null)
		{
			UnityEngine.Object.Destroy(_rpgUITreasureQuote);
		}
		if (_rpgUIBossQuote != null)
		{
			UnityEngine.Object.Destroy(_rpgUIBossQuote);
		}
		if (_rpgUIVictoryQuote != null)
		{
			UnityEngine.Object.Destroy(_rpgUIVictoryQuote);
		}
	}

	public void ReCoverTweenGold(object obj)
	{
		Debug.Log("------------------------------------------public void ReCoverTweenGold(object obj)");
		_displaySprite[16].gameObject.SetActive(false);
		_displaySprite[17].gameObject.SetActive(false);
		_displaySprite[16].gameObject.transform.localPosition = Vector3.zero;
		_displaySprite[17].gameObject.transform.localPosition = Vector3.zero;
		GameObject gameObject = UnityEngine.Object.Instantiate(_vertexMgr._goldFlowPrefab) as GameObject;
		gameObject.transform.parent = _vertexMgr.AllGoldPositionObj.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		UnityEngine.Object.Destroy(gameObject, 1.2f);
		_vertexMgr.PopEntity.UnBlockMsgBox(UIMessageBoxMgr.EMessageBoxType.GetItems);
	}
}
