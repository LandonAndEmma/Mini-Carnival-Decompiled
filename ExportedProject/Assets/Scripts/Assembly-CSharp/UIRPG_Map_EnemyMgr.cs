using System;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_Map_EnemyMgr : UIEntity
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	[SerializeField]
	private UISprite _playerIconSprite;

	[SerializeField]
	private UITexture _playerIconTexture;

	[SerializeField]
	private UILabel _enemyNameLabel;

	[SerializeField]
	private UILabel _enemyTeamLabel;

	[SerializeField]
	private UILabel _enemyLvLabel;

	[SerializeField]
	private UILabel _enemyMedalLabel;

	[SerializeField]
	private UILabel _enemyApLabel;

	[SerializeField]
	private UILabel _myApLabel;

	[SerializeField]
	private UILabel _rewardMedalLabel;

	[SerializeField]
	private UILabel _rewardGlodLabel;

	[SerializeField]
	private UILabel _rewardJewelryLabel;

	[SerializeField]
	private GameObject _rewardJewelryObj;

	[SerializeField]
	private UILabel _rewardExpLabel;

	[SerializeField]
	private UIRPG_Map_EnemyContainer _container;

	[SerializeField]
	private GameObject _changeBtn;

	[SerializeField]
	private GameObject _challengeBtn;

	[SerializeField]
	private GameObject _challenge2Btn;

	[SerializeField]
	private UILabel _npcLevelLabel;

	[SerializeField]
	private UIDraggablePanel _draggablePanel;

	protected override void Load()
	{
		_vertexMgr.ScreenMoveScale.OnDisable();
		RegisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerDataError, this, HandleNotifyOtherPlayerDataError);
		RegisterMessage(EUIMessageID.UIRPG_Map_InitEnemyInfo, this, HandleInitEnemyInfo);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerDataError, this);
		UnregisterMessage(EUIMessageID.UIRPG_Map_InitEnemyInfo, this);
	}

	protected override void Tick()
	{
	}

	public void InitContentData()
	{
		RecoverDefaultData();
		Debug.Log("=========================" + _vertexMgr.CurVertexType);
		switch (_vertexMgr.CurVertexType)
		{
		case UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpc:
			_playerIconSprite.spriteName = "map_icon1";
			_playerIconSprite.MakePixelPerfect();
			_enemyTeamLabel.text = TUITool.StringFormat(Localization.instance.Get("rpgmap_biaoti2"), "NPC");
			InitNpcData();
			break;
		case UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBoss:
			_playerIconSprite.spriteName = "map_icon3";
			_playerIconSprite.MakePixelPerfect();
			_enemyTeamLabel.text = TUITool.StringFormat(Localization.instance.Get("rpgmap_biaoti2"), "Boss");
			InitNpcData();
			break;
		case UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer:
		{
			_challengeBtn.SetActive(false);
			_challenge2Btn.SetActive(false);
			MapPoint[] mapPoint = UIDataBufferCenter.Instance.RPGData.m_mapPoint;
			int num = (int)(RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32() - mapPoint[_vertexMgr.CurVertexIndex].m_start_time);
			int num2 = RPGGlobalData.Instance.RpgMiscUnit._occupyParam6 * 3600;
			Debug.Log("point index: " + _vertexMgr.CurVertexIndex);
			Debug.Log("point : " + mapPoint[_vertexMgr.CurVertexIndex].m_start_time);
			Debug.Log("srv : " + RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32());
			Debug.Log("time : " + num);
			Debug.Log("timeGap : " + num2);
			bool dis = ((num >= num2) ? true : false);
			_changeBtn.SetActive(dis);
			UIDataBufferCenter.Instance.FetchPlayerRPGData(_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].Role_id, delegate(PlayerRpgDataCmd playData)
			{
				if (playData != null)
				{
					InitKPlayer(playData);
					_challengeBtn.SetActive(dis);
					_challenge2Btn.SetActive(!dis);
				}
			});
			break;
		}
		case UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpcThrough:
		case UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBossThrough:
			break;
		}
	}

	private void InitNpcData()
	{
		Debug.Log("private void InitNpcData()");
		_playerIconSprite.gameObject.SetActive(true);
		_playerIconTexture.gameObject.SetActive(false);
		string levelConfigNameByID = UIGolbalStaticFun.GetLevelConfigNameByID(_vertexMgr.CurVertexIndex + 1);
		string pXmlizedString = COMA_FileIO.LoadFile("Levels", levelConfigNameByID);
		Debug_TeamParam debug_TeamParam = new Debug_TeamParam();
		try
		{
			debug_TeamParam = COMA_Tools.DeserializeObject<Debug_TeamParam>(pXmlizedString) as Debug_TeamParam;
		}
		catch (Exception)
		{
			Debug.Log("==========================Config Error  " + levelConfigNameByID);
			_challengeBtn.SetActive(false);
			return;
		}
		_npcLevelLabel.text = "LV " + debug_TeamParam.lv;
		_npcLevelLabel.gameObject.SetActive(true);
		int lv = debug_TeamParam.lv;
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		_enemyApLabel.text = UIRPG_DataBufferCenter.GetNpcTeamTotalAp(debug_TeamParam).ToString();
		for (int i = 0; i < debug_TeamParam._careerLst.Count; i++)
		{
		}
		_myApLabel.text = UIRPG_DataBufferCenter.GetTeamTotalAp(UIDataBufferCenter.Instance.RPGData.m_member_slot, rpg_level, false, 0u).ToString();
		if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBoss)
		{
			_rewardMedalLabel.text = 3.ToString();
		}
		else if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpc)
		{
			int levelExceedPCT = RPGGlobalData.Instance.RpgMiscUnit._levelExceedPCT;
			float num = (float)levelExceedPCT * 0.01f;
			if (rpg_level + Mathf.CeilToInt((float)rpg_level * num) < lv)
			{
				_rewardMedalLabel.text = RPGGlobalData.Instance.RpgMiscUnit._levelDefeatPlayer_ExceedPCT.ToString();
			}
			else
			{
				_rewardMedalLabel.text = RPGGlobalData.Instance.RpgMiscUnit._levelDefeatPlayer.ToString();
			}
		}
		List<LevelIncome> lstLevelIncome = RPGGlobalData.Instance.LstLevelIncome;
		for (int j = 0; j < lstLevelIncome.Count; j++)
		{
			if (rpg_level <= lstLevelIncome[j]._lv)
			{
				_rewardGlodLabel.text = lstLevelIncome[j]._gold.ToString();
				_rewardExpLabel.text = lstLevelIncome[j]._exp.ToString();
				break;
			}
		}
		_rewardJewelryObj.SetActive(_vertexMgr._specialDropOut);
		InitBoxData(debug_TeamParam._careerLst);
	}

	private void InitKPlayer(PlayerRpgDataCmd playData)
	{
		int rpg_level = (int)playData.m_rpg_level;
		int rpg_level2 = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		_playerIconSprite.gameObject.SetActive(false);
		_playerIconTexture.gameObject.SetActive(true);
		UIDataBufferCenter.Instance.FetchFacebookIconByTID(playData.who, delegate(Texture2D tex)
		{
			_playerIconTexture.mainTexture = tex;
		});
		UIDataBufferCenter.Instance.FetchPlayerProfile(playData.who, delegate(WatchRoleInfo roleInfo)
		{
			if (roleInfo == null)
			{
				_enemyNameLabel.text = string.Empty;
				_enemyTeamLabel.text = string.Empty;
			}
			else
			{
				_enemyNameLabel.text = roleInfo.m_name;
				_enemyTeamLabel.text = TUITool.StringFormat(Localization.instance.Get("rpgmap_biaoti2"), roleInfo.m_name);
			}
		});
		_enemyLvLabel.text = playData.m_rpg_level.ToString();
		_enemyMedalLabel.text = playData.m_medal.ToString();
		_enemyApLabel.text = UIRPG_DataBufferCenter.GetTeamTotalAp(playData.m_member_slot, (int)playData.m_rpg_level, true, _vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].Role_id).ToString();
		_myApLabel.text = UIRPG_DataBufferCenter.GetTeamTotalAp(UIDataBufferCenter.Instance.RPGData.m_member_slot, rpg_level2, false, 0u).ToString();
		int levelExceedPCT = RPGGlobalData.Instance.RpgMiscUnit._levelExceedPCT;
		float num = (float)levelExceedPCT * 0.01f;
		if (rpg_level2 + Mathf.CeilToInt((float)rpg_level2 * num) < rpg_level)
		{
			_rewardMedalLabel.text = RPGGlobalData.Instance.RpgMiscUnit._levelDefeatPlayer_ExceedPCT.ToString();
		}
		else
		{
			_rewardMedalLabel.text = RPGGlobalData.Instance.RpgMiscUnit._levelDefeatPlayer.ToString();
		}
		List<LevelIncome> lstLevelIncome = RPGGlobalData.Instance.LstLevelIncome;
		for (int num2 = 0; num2 < lstLevelIncome.Count; num2++)
		{
			if (rpg_level2 <= lstLevelIncome[num2]._lv)
			{
				_rewardGlodLabel.text = lstLevelIncome[num2]._gold.ToString();
				_rewardExpLabel.text = lstLevelIncome[num2]._exp.ToString();
				break;
			}
		}
		List<int> list = new List<int>();
		for (int num3 = 0; num3 < playData.m_member_slot.Length; num3++)
		{
			list.Add((int)playData.m_member_slot[num3].m_member);
		}
		InitBoxData(list);
	}

	private void InitBoxData(List<int> lst)
	{
		_container.ClearContainer();
		_container.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		int num = 0;
		for (int i = 0; i < lst.Count; i++)
		{
			if (lst[i] != 0)
			{
				num++;
				UIRPG_Map_EnemyBoxData uIRPG_Map_EnemyBoxData = new UIRPG_Map_EnemyBoxData();
				uIRPG_Map_EnemyBoxData.CardId = lst[i];
				uIRPG_Map_EnemyBoxData.CardGrade = RPGGlobalData.Instance.CareerUnitPool._dict[uIRPG_Map_EnemyBoxData.CardId].StarGrade;
				_container.SetBoxData(_container.AddBox(i), uIRPG_Map_EnemyBoxData);
			}
		}
		if (num < 5)
		{
			_draggablePanel.scale = Vector3.zero;
		}
		else
		{
			_draggablePanel.scale = new Vector3(1f, 0f, 0f);
		}
	}

	private void RecoverDefaultData()
	{
		_container.ClearContainer();
		if (_enemyNameLabel != null)
		{
			_enemyNameLabel.text = string.Empty;
		}
		_enemyTeamLabel.text = string.Empty;
		if (_enemyLvLabel != null)
		{
			_enemyLvLabel.text = string.Empty;
		}
		if (_enemyMedalLabel != null)
		{
			_enemyMedalLabel.text = string.Empty;
		}
		_enemyApLabel.text = string.Empty;
		_myApLabel.text = string.Empty;
		_rewardMedalLabel.text = string.Empty;
		_rewardGlodLabel.text = string.Empty;
		_rewardJewelryLabel.text = string.Empty;
		_rewardExpLabel.text = string.Empty;
	}

	public bool HandleNotifyOtherPlayerDataError(TUITelegram msg)
	{
		Debug.Log("UIRPG_Map_EnemyMgr : HandleNotifyOtherPlayerDataError");
		return true;
	}

	public bool HandleInitEnemyInfo(TUITelegram msg)
	{
		InitContentData();
		return true;
	}
}
