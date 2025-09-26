using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_CheckPointsVertexMgr : UIEntity
{
	private Queue<int> _collectGoldQueue = new Queue<int>();

	[SerializeField]
	private float[] _gapDistance;

	[SerializeField]
	private GameObject _checkPointPrefab;

	[SerializeField]
	private GameObject _pathPrefab;

	[SerializeField]
	private Transform _pathParent;

	[SerializeField]
	private GameObject _popUpBossObj;

	[SerializeField]
	private GameObject _popUpPlayerObj;

	[SerializeField]
	private UILabel _medalLabel;

	[SerializeField]
	private UILabel _mobilityLabel;

	[SerializeField]
	private UILabel _timeLabel;

	[SerializeField]
	private GameObject _timeObj;

	[SerializeField]
	private UILabel _apLabel;

	[SerializeField]
	private GameObject _popUpAddMobilityObj;

	[SerializeField]
	private GameObject _addMobilityBtn;

	[SerializeField]
	private float _dymanicTimeGap = 0.8f;

	[SerializeField]
	private SS_ScreenMoveScale _screenMoveScale;

	[SerializeField]
	private GameObject _collectBtnObj;

	public int _goldNum;

	[SerializeField]
	private GameObject _allGoldPositionObj;

	public GameObject _goldFlowPrefab;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Camera _cameraMap;

	[SerializeField]
	private GameObject _mobilityBackGroundObj;

	[SerializeField]
	private GameObject _mobilityBackGroundObjPrefab;

	private int _curVertexIndex = -1;

	private UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat _curVertexType;

	private Dictionary<int, UIRPG_CheckPointsVertex> _vertexPassDict = new Dictionary<int, UIRPG_CheckPointsVertex>();

	private Dictionary<int, UIRPG_CheckPointsVertex> _vertexAllDict = new Dictionary<int, UIRPG_CheckPointsVertex>();

	private List<int> _vertexDisplayLst = new List<int>();

	private List<int> _vertexNextLst;

	private float _timer;

	public bool _specialDropOut;

	[SerializeField]
	private UIPopupDialogEntity _popEntity;

	public Queue<int> CollectGoldQueue
	{
		get
		{
			return _collectGoldQueue;
		}
	}

	public float[] GapDistance
	{
		get
		{
			return _gapDistance;
		}
		set
		{
			_gapDistance = value;
		}
	}

	public GameObject PopUpBossObj
	{
		get
		{
			return _popUpBossObj;
		}
	}

	public GameObject PopUpPlayerObj
	{
		get
		{
			return _popUpPlayerObj;
		}
	}

	public SS_ScreenMoveScale ScreenMoveScale
	{
		get
		{
			return _screenMoveScale;
		}
	}

	public GameObject CollectBtnObj
	{
		get
		{
			return _collectBtnObj;
		}
	}

	public GameObject AllGoldPositionObj
	{
		get
		{
			return _allGoldPositionObj;
		}
	}

	public int CurVertexIndex
	{
		get
		{
			return _curVertexIndex;
		}
		set
		{
			_curVertexIndex = value;
		}
	}

	public UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat CurVertexType
	{
		get
		{
			return _curVertexType;
		}
		set
		{
			_curVertexType = value;
		}
	}

	public Dictionary<int, UIRPG_CheckPointsVertex> VertexPassDict
	{
		get
		{
			return _vertexPassDict;
		}
	}

	public Dictionary<int, UIRPG_CheckPointsVertex> VertexAllDict
	{
		get
		{
			return _vertexAllDict;
		}
	}

	public UIPopupDialogEntity PopEntity
	{
		get
		{
			return _popEntity;
		}
	}

	protected override void Load()
	{
		UIRPG_DataBufferCenter.isPreSceneMap = true;
		if (COMA_Platform.Instance != null)
		{
			Debug.Log("COMA_Platform.Instance != null");
			COMA_Platform.Instance.DestroyPlatform();
		}
		RegisterMessage(EUIMessageID.UIRPG_NotifyMapDataResult, this, HandleNotifyDataResult);
		RegisterMessage(EUIMessageID.UIRPG_NotifyMobilityChanged, this, HandleNotifyMobilityChanged);
		RegisterMessage(EUIMessageID.UIRPG_LevelConfigReady, this, HandleLevelConfigReady);
		RegisterMessage(EUIMessageID.UIRPG_NotifyGainGoldResult, this, HandleNotifyGainGoldResult);
		RegisterMessage(EUIMessageID.UIRPG_NotifyGainAllGoldResult, this, HandleNotifyGainAllGoldResult);
		RegisterMessage(EUIMessageID.UIRPG_NotifyMapDataChanged, this, HandleNotifyMapDataChanged);
		RegisterMessage(EUIMessageID.UIRPG_NofityBuyMobilityResult, this, HandleNotifyBuyMobilityResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIRPG_Map_CheckPointAppear, this, HandleCheckPointAppear);
		RegisterMessage(EUIMessageID.UIRPGRanking_GotoSquare, this, GotoSquareClick);
		RegisterMessage(EUIMessageID.UIRPG_NotifyMedalChanged, this, HandleNotifyMedalChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMapDataResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMobilityChanged, this);
		UnregisterMessage(EUIMessageID.UIRPG_LevelConfigReady, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGainGoldResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGainAllGoldResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMapDataChanged, this);
		UnregisterMessage(EUIMessageID.UIRPG_NofityBuyMobilityResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_Map_CheckPointAppear, this);
		UnregisterMessage(EUIMessageID.UIRPGRanking_GotoSquare, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMedalChanged, this);
	}

	public void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.RPG_Precombat);
		_goldNum = 0;
		RefreshMapData();
	}

	protected override void Tick()
	{
		_timer += Time.deltaTime;
		if (_timer > 1f)
		{
			_timer = 0f;
			SetMobilityValue();
			SetTimeValue();
		}
	}

	public void RefreshMapData()
	{
		if (UIRPG_DataBufferCenter._isPreSceneBattle && !UIDataBufferCenter.Instance.CurBattleResult_Common)
		{
			GameObject gameObject = Object.Instantiate(_mobilityBackGroundObjPrefab) as GameObject;
			gameObject.transform.parent = _mobilityBackGroundObj.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			Object.Destroy(gameObject, 1.2f);
		}
		if (UIRPG_DataBufferCenter._isPreSceneBattle)
		{
			UIRPG_DataBufferCenter._isPreSceneBattle = false;
		}
		SetMobilityValue();
		SetDisplayData();
		bool flag = UIGolbalStaticFun.RequestRefreshMapData();
		Debug.Log("break " + flag);
		if (flag)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
		}
		else
		{
			InitCheckPointsVertex();
		}
	}

	public void InitCheckPointsVertex()
	{
		MapPoint[] mapPoint = UIDataBufferCenter.Instance.RPGData.m_mapPoint;
		SerializableDictionary<int, RPGMapLayoutUnit> dict = RPGGlobalData.Instance.MapLayout._dict;
		int curBattleLevelIndex = UIDataBufferCenter.Instance.CurBattleLevelIndex;
		if (UIDataBufferCenter.Instance.CurBattleResult && dict.ContainsKey(curBattleLevelIndex + 1))
		{
			Debug.Log("curIndex = " + curBattleLevelIndex);
			_vertexNextLst = dict[curBattleLevelIndex + 1]._followLevelList;
		}
		for (int i = 0; i < mapPoint.Length; i++)
		{
			InitSingleCheckPointsVertex(i);
		}
		Debug.Log("_vertexPassDict.Count = " + _vertexPassDict.Count);
		DisplayStaticPath();
		if (!UIDataBufferCenter.Instance.CurBattleResult)
		{
			return;
		}
		UIRPG_CheckPointsVertex uIRPG_CheckPointsVertex = _vertexAllDict[UIDataBufferCenter.Instance.CurBattleLevelIndex];
		uIRPG_CheckPointsVertex.GetNextMapPoints();
		_vertexPassDict.Add(_curVertexIndex, uIRPG_CheckPointsVertex);
		foreach (UIRPG_CheckPointsVertexMap key in uIRPG_CheckPointsVertex.MapDict.Keys)
		{
			StartCoroutine(DynamicDisplayPath(key, uIRPG_CheckPointsVertex.MapDict[key]));
		}
	}

	public void InitSingleCheckPointsVertex(int index)
	{
		MapPoint[] mapPoint = UIDataBufferCenter.Instance.RPGData.m_mapPoint;
		SerializableDictionary<int, RPGMapLayoutUnit> dict = RPGGlobalData.Instance.MapLayout._dict;
		int num = (int)(RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32() - mapPoint[index].m_start_time);
		int num2 = (int)(RPGGlobalData.Instance.RpgMiscUnit._produceGoldInterTime * 3600f);
		UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KLock;
		switch (mapPoint[index].m_status)
		{
		case 0:
			return;
		case 1:
			uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpc;
			break;
		case 3:
			uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer;
			break;
		case 2:
			uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpcThrough;
			if (num2 <= num)
			{
				uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KGold;
				_goldNum++;
			}
			break;
		}
		if (uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpc && index % 5 == 4)
		{
			uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBoss;
		}
		if (uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpcThrough && index % 10 == 9)
		{
			Debug.Log("3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333");
			uIRPG_CheckPointsVertexStat = UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBossThrough;
		}
		if (!_vertexAllDict.ContainsKey(index))
		{
			GameObject gameObject = Object.Instantiate(_checkPointPrefab) as GameObject;
			if (index < 9)
			{
				gameObject.name = "UIRPG_MapCheckPoint0" + (index + 1);
			}
			else
			{
				gameObject.name = "UIRPG_MapCheckPoint" + (index + 1);
			}
			gameObject.transform.parent = _pathParent;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = UIRPG_DataBufferCenter.Instance.PosVector3[index] - new Vector3(512f, 340f, 0f);
			gameObject.SetActive(false);
			UIRPG_CheckPointsVertex component = gameObject.GetComponent<UIRPG_CheckPointsVertex>();
			component.VertexIndex = index;
			component.VertexMgr = this;
			component.PreLst = dict[index + 1]._frontLevelList;
			component.NextLst = dict[index + 1]._followLevelList;
			component.GetEightDirectionPoints();
			_vertexAllDict.Add(index, component);
			if ((uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpcThrough || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBossThrough || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KGold) && (index != UIDataBufferCenter.Instance.CurBattleLevelIndex || !UIDataBufferCenter.Instance.CurBattleResult))
			{
				_vertexPassDict.Add(index, component);
			}
		}
		_vertexAllDict[index].StartTime = mapPoint[index].m_start_time;
		_vertexAllDict[index].VertexStat = uIRPG_CheckPointsVertexStat;
		_vertexAllDict[index].Role_id = mapPoint[index].m_role_id;
		if (_vertexNextLst != null && _vertexNextLst.Contains(index + 1) && !_vertexDisplayLst.Contains(index))
		{
			Debug.Log("_vertexNextLst.Contains(index) = " + index);
		}
		else
		{
			_vertexAllDict[index].gameObject.SetActive(true);
			if (uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpcThrough || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBossThrough || uIRPG_CheckPointsVertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KGold)
			{
				Debug.Log("zhangyongjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj index = " + index);
				if (index != UIDataBufferCenter.Instance.CurBattleLevelIndex || !UIDataBufferCenter.Instance.CurBattleResult)
				{
					foreach (int item in _vertexAllDict[index].NextLst)
					{
						if (!_vertexDisplayLst.Contains(item - 1))
						{
							_vertexDisplayLst.Add(item - 1);
						}
						Debug.Log(" 9999999999999999999999999999999999999999999999999999999999foreach (int key in _vertexAllDict[index].NextLst)");
					}
				}
				Debug.Log("9999999999999999999999999999999999999999999999999999999999foreach  _vertexDisplayLst.Count = " + _vertexDisplayLst.Count);
				if (_vertexDisplayLst.Count != 0)
				{
					Debug.Log("-------------------------------------------------------------------------if (_vertexDisplayLst.Count != 0)");
				}
			}
			_vertexAllDict[index].BoxDataChanged();
		}
		_collectBtnObj.SetActive(_goldNum > 0);
	}

	public void DisplayStaticPath()
	{
		foreach (int key in _vertexPassDict.Keys)
		{
			StartCoroutine(StaticDisplayPath(key));
		}
	}

	public IEnumerator StaticDisplayPath(int index)
	{
		int maxFrame = 20;
		if (index != 0 && index % maxFrame == 0)
		{
			yield return 0;
		}
		UIRPG_CheckPointsVertex vertex = _vertexPassDict[index];
		vertex.GetNextMapPoints();
		foreach (UIRPG_CheckPointsVertexMap key in vertex.MapDict.Keys)
		{
			UIRPG_CheckPointsVertexMap value = vertex.MapDict[key];
			float distance = (value.DirectionPos - key.DirectionPos).magnitude;
			int max = Mathf.CeilToInt(distance / GetGapDistance(distance));
			Vector3 vec = value.DirectionPos - key.DirectionPos;
			for (int i = 0; i <= max; i++)
			{
				Vector3 v = key.DirectionPos + i * vec / max;
				GameObject obj = Object.Instantiate(_pathPrefab) as GameObject;
				obj.transform.parent = _pathParent;
				obj.transform.localPosition = v;
				obj.transform.localScale = Vector3.one;
			}
		}
	}

	public IEnumerator DynamicDisplayPath(UIRPG_CheckPointsVertexMap key, UIRPG_CheckPointsVertexMap value)
	{
		float distance = (value.DirectionPos - key.DirectionPos).magnitude;
		int max = Mathf.CeilToInt(distance / GetGapDistance(distance));
		Vector3 vec = value.DirectionPos - key.DirectionPos;
		for (int i = 0; i <= max; i++)
		{
			Vector3 v = key.DirectionPos + i * vec / max;
			GameObject obj = Object.Instantiate(_pathPrefab) as GameObject;
			obj.transform.parent = _pathParent;
			obj.transform.localPosition = v;
			obj.transform.localScale = Vector3.one;
			yield return new WaitForSeconds(_dymanicTimeGap);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Map_CheckPointAppear, null, value.VertexIndex);
	}

	private float GetGapDistance(float distance)
	{
		int num = 0;
		num = ((!(distance < 35f)) ? ((distance < 45f) ? 1 : 2) : 0);
		return _gapDistance[num];
	}

	private void SetMobilityValue()
	{
		int num = RPGGlobalClock.Instance.GetMobilityValue();
		if (num < 0)
		{
			num = 0;
		}
		else if (num > 5)
		{
			num = 5;
		}
		_mobilityLabel.text = num.ToString();
		_addMobilityBtn.SetActive(RPGGlobalClock.Instance.GetMobilityValue() != RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max);
		_timeObj.SetActive(RPGGlobalClock.Instance.GetMobilityValue() != RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max);
	}

	private void SetTimeValue()
	{
		uint num = RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32() - UIDataBufferCenter.Instance.RPGData.m_mobility_time;
		uint num2 = (uint)(RPGGlobalData.Instance.RpgMiscUnit._energyRenewTimePerUnit * 60);
		uint num3 = num / num2;
		uint num4 = num2 - (num - num3 * num2);
		uint num5 = num4 / 60;
		uint num6 = num4 % 60;
		string text = null;
		text = ((num6 < 10) ? ("0" + num6) : num6.ToString());
		_timeLabel.text = num5 + ":" + text;
	}

	private void SetDisplayData()
	{
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		_medalLabel.text = UIDataBufferCenter.Instance.RPGData.m_medal.ToString();
		int teamTotalAp = UIRPG_DataBufferCenter.GetTeamTotalAp(UIDataBufferCenter.Instance.RPGData.m_member_slot, rpg_level, false, 0u);
		_apLabel.text = "AP " + teamTotalAp;
	}

	public bool HandleNotifyDataResult(TUITelegram msg)
	{
		ReqRefreshPlayerLevelResultCmd reqRefreshPlayerLevelResultCmd = msg._pExtraInfo as ReqRefreshPlayerLevelResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (reqRefreshPlayerLevelResultCmd.m_result == 0)
		{
			Debug.Log("UIRPG_CheckPointsVertexMgr : HandleNotifyDataResult");
		}
		InitCheckPointsVertex();
		return true;
	}

	public bool HandleNotifyMobilityChanged(TUITelegram msg)
	{
		SetMobilityValue();
		return true;
	}

	public bool HandleLevelConfigReady(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		_vertexAllDict[CurVertexIndex].OnClick();
		return true;
	}

	public bool HandleNotifyGainGoldResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		ReqGainGoldResultCmd reqGainGoldResultCmd = msg._pExtraInfo as ReqGainGoldResultCmd;
		if (reqGainGoldResultCmd.m_result == 0)
		{
			_goldNum--;
			_collectBtnObj.SetActive(_goldNum > 0);
			TweenGold();
		}
		return true;
	}

	public bool HandleNotifyGainAllGoldResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		_goldNum = 0;
		_collectBtnObj.SetActive(_goldNum > 0);
		TweenGold();
		return true;
	}

	public bool HandleNotifyBuyMobilityResult(TUITelegram msg)
	{
		BuyMobilityResultCmd buyMobilityResultCmd = msg._pExtraInfo as BuyMobilityResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (buyMobilityResultCmd.m_result == 0)
		{
			SetMobilityValue();
			string str = TUITool.StringFormat(Localization.instance.Get("energy_desc3"));
			UIGolbalStaticFun.PopupTipsBox(str);
		}
		else if (buyMobilityResultCmd.m_result == 2)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "BuyCrystral";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "BuyCrystral":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		case "IsBuyMobility":
			_popUpAddMobilityObj.SetActive(true);
			break;
		}
		return true;
	}

	private bool HandleCheckPointAppear(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		UIDataBufferCenter.Instance.CurBattleResult = false;
		if (_vertexNextLst != null && _vertexNextLst.Contains(num + 1) && _vertexAllDict.ContainsKey(num) && !_vertexDisplayLst.Contains(num))
		{
			Debug.Log("+++++++++++++++++++++++++++HandleCheckPointAppear index = " + num);
			_vertexAllDict[num].gameObject.SetActive(true);
			_vertexAllDict[num].BoxDataChanged();
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/RPG_UI_Map/RPG_UI_Flags/RPG_UI_Flags")) as GameObject;
			gameObject.transform.parent = _vertexAllDict[num].gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			Object.Destroy(gameObject, 1.2f);
			UISprite sprite = null;
			for (int i = 0; i < _vertexAllDict[num].DisplaySprite.Length; i++)
			{
				if (_vertexAllDict[num].DisplaySprite[i].gameObject.activeSelf)
				{
					sprite = _vertexAllDict[num].DisplaySprite[i];
					break;
				}
			}
			StartCoroutine(DynamicDisplaySingleSprite(sprite));
		}
		return true;
	}

	public bool HandleNotifyMapDataChanged(TUITelegram msg)
	{
		Debug.Log(" public bool HandleNotifyMapDataChanged(TUITelegram msg)");
		NotifyMapChangeCmd data = msg._pExtraInfo as NotifyMapChangeCmd;
		for (int i = 0; i < data.m_points.Count; i++)
		{
			int j = i;
			UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
			{
				Debug.Log("COMA_Server_Account.Instance.svrTime = " + time);
				COMA_Server_Account.Instance.svrTime = time;
				RPGGlobalClock.Instance.InitClock(time);
				InitSingleCheckPointsVertex(data.m_points[j].m_index);
			});
		}
		return true;
	}

	private bool GotoSquare(object obj)
	{
		Debug.Log("GotoSquare");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoSquareClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool HandleNotifyMedalChanged(TUITelegram msg)
	{
		_medalLabel.text = UIDataBufferCenter.Instance.RPGData.m_medal.ToString();
		return true;
	}

	private IEnumerator DynamicDisplaySingleSprite(UISprite sprite)
	{
		float maxY = sprite.gameObject.transform.localScale.y;
		float x = sprite.gameObject.transform.localScale.x;
		float z = sprite.gameObject.transform.localScale.z;
		float singleSize = maxY / 10f;
		for (int i = 0; i <= 10; i++)
		{
			Vector3 scale = new Vector3(x, (float)i * singleSize, z);
			sprite.gameObject.transform.localScale = scale;
			yield return 0;
		}
	}

	public void TweenGold()
	{
		while (_collectGoldQueue.Count != 0)
		{
			int num = _collectGoldQueue.Dequeue();
			Debug.Log("public void TweenGold()  index = " + num);
			Transform parent = _vertexAllDict[num].gameObject.transform;
			GameObject gameObject = Object.Instantiate(_vertexAllDict[num].RPGUITreasureExplosionPrefab) as GameObject;
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			Vector3 position = _allGoldPositionObj.transform.position;
			Debug.Log("------------------------------------------------targetPos one = " + position);
			position = _cameraMap.WorldToScreenPoint(position);
			position = _camera.ScreenToWorldPoint(position);
			Debug.Log("------------------------------------------------targetPos one = " + position);
			_vertexAllDict[num].DisplaySprite[16].gameObject.GetComponent<SpringPosition_MC>().target = position;
			_vertexAllDict[num].DisplaySprite[17].gameObject.GetComponent<SpringPosition_MC>().target = position;
			_vertexAllDict[num].DisplaySprite[16].gameObject.GetComponent<SpringPosition_MC>().enabled = true;
			_vertexAllDict[num].DisplaySprite[17].gameObject.GetComponent<SpringPosition_MC>().enabled = true;
			_vertexAllDict[num].DisplaySprite[16].gameObject.SetActive(true);
			_vertexAllDict[num].DisplaySprite[17].gameObject.SetActive(true);
			Object.Destroy(gameObject, 1.2f);
			Object.Destroy(_vertexAllDict[num]._rpgUITreasureQuote, 1f);
		}
	}
}
