using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class RPGRefree : TBaseEntity
{
	public enum EBattleMainState
	{
		Init = 0,
		ShowBuff = 1,
		Begin_BigBout = 2,
		BigBouting = 3,
		End_BigBout = 4,
		Settlement = 5,
		Delay_Idle = 6
	}

	public enum EShowBuffState
	{
		EnterTeam1SkillAwake = 0,
		Team1SkillAwaking = 1,
		EnterTeam1NormalTactic = 2,
		Team1NormalTacticing = 3,
		EnterTeam2SkillAwake = 4,
		Team2SkillAwaking = 5,
		EnterTeam2NormalTactic = 6,
		Team2NormalTacticing = 7,
		EnterTeam1SpecTactic = 8,
		Team1SpecTacticing = 9,
		EnterTeam2SpecTactic = 10,
		Team2SpecTacticing = 11,
		ShowBuffEnd = 12
	}

	public static RPGRefree Instance;

	public static readonly int MAX_TeamNum = 2;

	[SerializeField]
	private RPGTeam[] _teams = new RPGTeam[MAX_TeamNum];

	[SerializeField]
	private int _initEndCount;

	[SerializeField]
	private List<RPGEntity> _battleRoleList = new List<RPGEntity>();

	[SerializeField]
	private EBattleMainState _curMainState;

	[SerializeField]
	private UIRPG_SettlementMgr _settlementMgr;

	[SerializeField]
	private int _curAttackIndex;

	[SerializeField]
	private bool _notifyCurAttackRole;

	[SerializeField]
	private EShowBuffState _curShowBuffState;

	[SerializeField]
	private int _curBigCountIndex;

	private float _fBigBoutEndTime;

	[SerializeField]
	private bool _doubleTime;

	[SerializeField]
	private bool _normalTime;

	private bool CanNext = true;

	public List<RPGEntity> BattleRoleList
	{
		get
		{
			return _battleRoleList;
		}
	}

	public EBattleMainState CurMainState
	{
		get
		{
			return _curMainState;
		}
		set
		{
			_curMainState = value;
		}
	}

	public UIRPG_SettlementMgr SettlementMgr
	{
		get
		{
			return _settlementMgr;
		}
	}

	public int CurAttackIndex
	{
		get
		{
			return _curAttackIndex;
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		Instance = this;
		if (COMA_HTTP_DataCollect.Instance != null)
		{
			COMA_HTTP_DataCollect.Instance.SendRPGBattleCount("1");
		}
	}

	protected new void OnDisable()
	{
		base.OnDisable();
		Instance = null;
	}

	public RPGTeam GetEnemyTeam(RPGTeam self)
	{
		for (int i = 0; i < MAX_TeamNum; i++)
		{
			if (self != _teams[i])
			{
				return _teams[i];
			}
		}
		return null;
	}

	public int GetTeamIndex(RPGTeam team)
	{
		for (int i = 0; i < MAX_TeamNum; i++)
		{
			if (team == _teams[i])
			{
				return i;
			}
		}
		return -1;
	}

	public RPGEntity GetCurBoutingEntity()
	{
		if (_curAttackIndex < 0 || _curAttackIndex >= _battleRoleList.Count)
		{
			return null;
		}
		return _battleRoleList[_curAttackIndex];
	}

	public void RefreshBattleRoleList()
	{
		_battleRoleList.Clear();
		for (int i = 0; i < MAX_TeamNum; i++)
		{
			for (int j = 0; j < _teams[i].MemberLst.Count; j++)
			{
				if (_teams[i].MemberLst[j].CurHp > 0f)
				{
					_battleRoleList.Add(_teams[i].MemberLst[j]);
				}
			}
		}
		_battleRoleList.Sort(CompareATTIndex);
		UIRPGIngameMgr.Instance.RefreshATTRanking(_battleRoleList);
	}

	public void BattleRoleListChanged()
	{
		for (int i = 0; i < _battleRoleList.Count; i++)
		{
			if (_battleRoleList[i].CurHp <= 0f)
			{
				_battleRoleList[i].CalcAttr.Attrs[13] = -100f;
			}
		}
		List<RPGEntity> list = new List<RPGEntity>();
		for (int j = CurAttackIndex + 1; j < _battleRoleList.Count; j++)
		{
			if (j < _battleRoleList.Count)
			{
				list.Add(_battleRoleList[j]);
			}
		}
		if (list.Count > 1)
		{
			list.Sort(CompareATTIndex);
			int num = 0;
			for (int k = CurAttackIndex + 1; k < _battleRoleList.Count; k++)
			{
				if (k < _battleRoleList.Count)
				{
					_battleRoleList[k] = list[num];
					num++;
				}
			}
		}
		for (int l = CurAttackIndex + 1; l < _battleRoleList.Count; l++)
		{
			if (l < _battleRoleList.Count && _battleRoleList[l].CurHp <= 0f)
			{
				_battleRoleList.RemoveAt(l);
				l--;
			}
		}
		UIRPGIngameMgr.Instance.RefreshATTRankingFromCur();
	}

	private void Win()
	{
		_settlementMgr.Init(true);
		if (!IsPCLoadFromCurScene())
		{
			COMA_AudioManager.Instance.MusicPlay(AudioCategory.RPG_Win);
			UIDataBufferCenter.Instance.CurBattleResult_Common = true;
			if (UIDataBufferCenter.Instance.RPGData.m_mapPoint[UIDataBufferCenter.Instance.CurBattleLevelIndex].m_status == 1)
			{
				UIDataBufferCenter.Instance.CurBattleResult = true;
			}
			ReportMapBattleCmd reportMapBattleCmd = new ReportMapBattleCmd();
			reportMapBattleCmd.m_battle_result = 0;
			reportMapBattleCmd.m_map_point = (byte)UIDataBufferCenter.Instance.CurBattleLevelIndex;
			reportMapBattleCmd.m_rpg_level = (uint)UIDataBufferCenter.Instance.CurBattleLevelLV;
			UIDataBufferCenter.Instance.FetchLevelAward(reportMapBattleCmd, delegate(ReportMapBattleResultCmd award)
			{
				_settlementMgr.SetAward(award.m_medal, (int)award.m_gold, (int)award.m_exp, (int)award.m_crystal);
				_settlementMgr.SetSpecialAward((int)award.m_crystal, (int)award.m_card_id, award.m_deco_id);
			});
		}
		else
		{
			_settlementMgr.SetAward(3, 3000, 20000, 3);
		}
	}

	private void Win_Debug(int index, int player_lv)
	{
		_settlementMgr.Init(true);
		if (!IsPCLoadFromCurScene())
		{
			if (UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_status == 1)
			{
				ReportMapBattleCmd reportMapBattleCmd = new ReportMapBattleCmd();
				reportMapBattleCmd.m_battle_result = 0;
				reportMapBattleCmd.m_map_point = (byte)index;
				reportMapBattleCmd.m_rpg_level = (uint)player_lv;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reportMapBattleCmd);
			}
			if (UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_status == 3)
			{
				CanNext = false;
				UIDataBufferCenter.Instance.FetchPlayerRPGData(UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_role_id, delegate(PlayerRpgDataCmd rpgData)
				{
					CanNext = true;
					ReportMapBattleCmd extraInfo = new ReportMapBattleCmd
					{
						m_battle_result = 0,
						m_map_point = (byte)index,
						m_rpg_level = rpgData.m_rpg_level
					};
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
				});
			}
		}
		else
		{
			_settlementMgr.SetAward(3, 3000, 20000, 3);
		}
	}

	private void Failure()
	{
		if (!IsPCLoadFromCurScene())
		{
			COMA_AudioManager.Instance.MusicPlay(AudioCategory.RPG_Lose);
			ReportMapBattleCmd reportMapBattleCmd = new ReportMapBattleCmd();
			reportMapBattleCmd.m_battle_result = 1;
			reportMapBattleCmd.m_map_point = (byte)UIDataBufferCenter.Instance.CurBattleLevelIndex;
			reportMapBattleCmd.m_rpg_level = (uint)UIDataBufferCenter.Instance.CurBattleLevelLV;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reportMapBattleCmd);
		}
		_settlementMgr.Init(false);
	}

	private void ChangeMainState(EBattleMainState state)
	{
		if (CurMainState == EBattleMainState.Settlement)
		{
			return;
		}
		switch (state)
		{
		case EBattleMainState.Begin_BigBout:
			RefreshBattleRoleList();
			break;
		case EBattleMainState.ShowBuff:
			_curShowBuffState = EShowBuffState.EnterTeam1SkillAwake;
			break;
		case EBattleMainState.Settlement:
			if (_teams[0].GetAliveMemCout() > 0)
			{
				Win();
			}
			else
			{
				Failure();
			}
			break;
		}
		CurMainState = state;
	}

	private int CompareATTIndex(RPGEntity entity1, RPGEntity entity2)
	{
		if (entity1.CalcAttr.Attrs[13] == entity2.CalcAttr.Attrs[13])
		{
			return 0;
		}
		return (!(entity1.CalcAttr.Attrs[13] - entity2.CalcAttr.Attrs[13] > 0f)) ? 1 : (-1);
	}

	private void Awake()
	{
		if (IsPCLoadFromCurScene())
		{
			Debug.Log("COMA_FileIO.ReadTextDirectly(\"Data/Careers.xml\")");
			string pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/Careers.xml");
			RPGGlobalData.Instance.CareerUnitPool = COMA_Tools.DeserializeObject<RPGCareerUnitPool>(pXmlizedString) as RPGCareerUnitPool;
			Debug.Log("Career Count=" + RPGGlobalData.Instance.CareerUnitPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/Tactics.xml");
			RPGGlobalData.Instance.TacticUnitPool = COMA_Tools.DeserializeObject<RPGTacticUnitPool>(pXmlizedString) as RPGTacticUnitPool;
			Debug.Log("Tactic Count=" + RPGGlobalData.Instance.TacticUnitPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/RPGSkills.xml");
			RPGGlobalData.Instance.SkillUnitPool = COMA_Tools.DeserializeObject<RPGSkillUnitPool>(pXmlizedString) as RPGSkillUnitPool;
			Debug.Log("Skill Count=" + RPGGlobalData.Instance.SkillUnitPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/CareerAnis.xml");
			RPGGlobalData.Instance.CareerAniPool = COMA_Tools.DeserializeObject<RPGCareerAnimationUnitPool>(pXmlizedString) as RPGCareerAnimationUnitPool;
			Debug.Log("CareerAni Count=" + RPGGlobalData.Instance.CareerAniPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/AttackEffect.xml");
			RPGGlobalData.Instance.AttackEffectPool = COMA_Tools.DeserializeObject<RPGAttackEffectUnitPool>(pXmlizedString) as RPGAttackEffectUnitPool;
			Debug.Log("AttackEffect Count=" + RPGGlobalData.Instance.AttackEffectPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/BeAttackEffect.xml");
			RPGGlobalData.Instance.BeAttackEffectPool = COMA_Tools.DeserializeObject<RPGBeAttackEffectUnitPool>(pXmlizedString) as RPGBeAttackEffectUnitPool;
			Debug.Log("BeAttackEffect Count=" + RPGGlobalData.Instance.BeAttackEffectPool._dict.Count);
			pXmlizedString = COMA_FileIO.ReadTextDirectly("Data/BuffEffect.xml");
			RPGGlobalData.Instance.BuffEffectPool = COMA_Tools.DeserializeObject<RPGBuffEffectUnitPool>(pXmlizedString) as RPGBuffEffectUnitPool;
			Debug.Log("BuffEffect Count=" + RPGGlobalData.Instance.BuffEffectPool._dict.Count);
		}
	}

	public bool IsPCLoadFromCurScene()
	{
		if (UIDataBufferCenter.Instance == null)
		{
			return true;
		}
		return false;
	}

	private void Start()
	{
		if (IsPCLoadFromCurScene())
		{
			_teams[0].InitBattleMem("Data/Team_self.xml", true);
			_teams[1].InitBattleMem("Data/Team_enemy.xml", true);
			return;
		}
		if (UIDataBufferCenter.Instance.CurBattleLevelIndex != -1)
		{
			if ((UIDataBufferCenter.Instance.CurBattleLevelIndex + 1) % 5 == 0 && UIDataBufferCenter.Instance.RPGData.m_mapPoint[UIDataBufferCenter.Instance.CurBattleLevelIndex].m_status != 3)
			{
				COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Boss_Combat01);
			}
			else
			{
				COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Combat01);
			}
		}
		NotifyRPGDataCmd rPGData = UIDataBufferCenter.Instance.RPGData;
		PlayerRpgDataCmd rPGData_Enemy = UIDataBufferCenter.Instance.RPGData_Enemy;
		_teams[0].InitBattleMem(true, (int)rPGData.m_rpg_level, rPGData.m_member_slot, rPGData.m_equip_bag);
		MapPoint mapPoint = rPGData.m_mapPoint[UIDataBufferCenter.Instance.CurBattleLevelIndex];
		if (mapPoint.m_status == 0 || mapPoint.m_status == 1)
		{
			string levelConfigNameByID = UIGolbalStaticFun.GetLevelConfigNameByID(UIDataBufferCenter.Instance.CurBattleLevelIndex + 1);
			_teams[1].InitBattleMem(levelConfigNameByID, false);
		}
		else if (mapPoint.m_status == 3 && _teams[1].InitBattleMem(false, (int)rPGData_Enemy.m_rpg_level, rPGData_Enemy.m_member_slot, rPGData_Enemy.m_equip_bag) == 0)
		{
			Debug.Log("----No Enemy .");
			ChangeMainState(EBattleMainState.Settlement);
		}
	}

	protected void NotifyBoutEnd()
	{
		_curAttackIndex++;
		_notifyCurAttackRole = false;
	}

	public void NotifyTeamAllDeath()
	{
		ChangeMainState(EBattleMainState.Settlement);
	}

	public void NotifyTeamMemDeath(RPGEntity entity)
	{
		for (int i = 0; i < _teams.Length; i++)
		{
			if (entity.TeamOwner != _teams[i])
			{
				for (int j = 0; j < _teams[i].TacticLst.Count; j++)
				{
					_teams[i].TacticLst[j].ConditionChanged_Enemy();
				}
			}
		}
	}

	public void TeamSkillAwaked(RPGTeam team)
	{
		if (_curShowBuffState == EShowBuffState.Team1SkillAwaking)
		{
			_curShowBuffState = EShowBuffState.EnterTeam1NormalTactic;
		}
		else if (_curShowBuffState == EShowBuffState.Team2SkillAwaking)
		{
			_curShowBuffState = EShowBuffState.EnterTeam2NormalTactic;
		}
	}

	public void TeamNormalTacticInited(RPGTeam team)
	{
		if (_curShowBuffState == EShowBuffState.Team1NormalTacticing)
		{
			_curShowBuffState = EShowBuffState.EnterTeam2SkillAwake;
		}
		else if (_curShowBuffState == EShowBuffState.Team2NormalTacticing)
		{
			_curShowBuffState = EShowBuffState.EnterTeam1SpecTactic;
		}
	}

	public void TeamSpecTacticInited(RPGTeam team)
	{
		if (_curShowBuffState == EShowBuffState.Team1SpecTacticing)
		{
			_curShowBuffState = EShowBuffState.EnterTeam2SpecTactic;
		}
		else if (_curShowBuffState == EShowBuffState.Team2SpecTacticing)
		{
			_curShowBuffState = EShowBuffState.ShowBuffEnd;
		}
	}

	public void UpdateShowBuff()
	{
		switch (_curShowBuffState)
		{
		case EShowBuffState.EnterTeam1SkillAwake:
			_curShowBuffState = EShowBuffState.Team1SkillAwaking;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[0].GetInstanceID(), 5020, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team1SkillAwaking:
			break;
		case EShowBuffState.EnterTeam1NormalTactic:
			_curShowBuffState = EShowBuffState.Team1NormalTacticing;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[0].GetInstanceID(), 5021, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team1NormalTacticing:
			break;
		case EShowBuffState.EnterTeam2SkillAwake:
			_curShowBuffState = EShowBuffState.Team2SkillAwaking;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[1].GetInstanceID(), 5020, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team2SkillAwaking:
			break;
		case EShowBuffState.EnterTeam2NormalTactic:
			_curShowBuffState = EShowBuffState.Team2NormalTacticing;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[1].GetInstanceID(), 5021, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team2NormalTacticing:
			break;
		case EShowBuffState.EnterTeam1SpecTactic:
			_curShowBuffState = EShowBuffState.Team1SpecTacticing;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[0].GetInstanceID(), 5022, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team1SpecTacticing:
			break;
		case EShowBuffState.EnterTeam2SpecTactic:
			_curShowBuffState = EShowBuffState.Team2SpecTacticing;
			TMessageDispatcher.Instance.DispatchMsg(-1, _teams[1].GetInstanceID(), 5022, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		case EShowBuffState.Team2SpecTacticing:
			break;
		case EShowBuffState.ShowBuffEnd:
			ChangeMainState(EBattleMainState.Begin_BigBout);
			break;
		}
	}

	private new void Update()
	{
		if (_doubleTime)
		{
			Time.timeScale *= 2f;
			_doubleTime = false;
		}
		if (_normalTime)
		{
			Time.timeScale = 1f;
			_normalTime = false;
		}
		switch (CurMainState)
		{
		case EBattleMainState.ShowBuff:
			UpdateShowBuff();
			break;
		case EBattleMainState.Begin_BigBout:
			_curBigCountIndex++;
			_curAttackIndex = 0;
			ChangeMainState(EBattleMainState.BigBouting);
			UIRPGIngameMgr.Instance.NotifyRoundChanged(_curBigCountIndex);
			break;
		case EBattleMainState.BigBouting:
			if (_battleRoleList.Count <= 0)
			{
				ChangeMainState(EBattleMainState.Settlement);
			}
			else
			{
				if (_notifyCurAttackRole)
				{
					break;
				}
				_notifyCurAttackRole = true;
				if (_curAttackIndex < _battleRoleList.Count)
				{
					if (_battleRoleList[_curAttackIndex] != null)
					{
						UIRPGIngameMgr.Instance.JumpToATTPos();
						TMessageDispatcher.Instance.DispatchMsg(-1, _battleRoleList[_curAttackIndex].GetInstanceID(), 5015, 1f, null);
					}
					else
					{
						NotifyBoutEnd();
					}
				}
				else
				{
					_fBigBoutEndTime = Time.time;
					ChangeMainState(EBattleMainState.End_BigBout);
				}
			}
			break;
		case EBattleMainState.End_BigBout:
			if (Time.time - _fBigBoutEndTime >= 0.1f)
			{
				_notifyCurAttackRole = false;
				ChangeMainState(EBattleMainState.Begin_BigBout);
			}
			break;
		}
	}

	public void TeamInitEndNotify()
	{
		_initEndCount++;
		if (_initEndCount >= MAX_TeamNum)
		{
			_initEndCount = 0;
			ChangeMainState(EBattleMainState.ShowBuff);
		}
	}

	public override bool HandleMessage(TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 5019:
		{
			result = true;
			RPGEntity rPGEntity = msg._pExtraInfo as RPGEntity;
			if (_curAttackIndex < _battleRoleList.Count && rPGEntity == _battleRoleList[_curAttackIndex])
			{
				result = true;
				NotifyBoutEnd();
			}
			break;
		}
		case 5027:
			result = true;
			CurMainState = EBattleMainState.Settlement;
			Win();
			break;
		case 5028:
			result = true;
			StartCoroutine(Debug_Nofog_Rpg());
			break;
		}
		return result;
	}

	private IEnumerator Debug_Nofog_Rpg()
	{
		bool bNext = true;
		int index = 0;
		do
		{
			yield return new WaitForSeconds(0.01f);
			if (!CanNext)
			{
				continue;
			}
			yield return new WaitForSeconds(0.1f);
			CanNext = false;
			Debug.Log("------------Debug_Req_" + index);
			UIDataBufferCenter.Instance.FetchDebug_NOFOG(index, delegate
			{
				Debug.Log("------------Result_Debug_Req_" + index);
				CurMainState = EBattleMainState.Settlement;
				CanNext = true;
				Win_Debug(index, 0);
				index++;
				if (index >= 100)
				{
					bNext = false;
				}
			});
		}
		while (bNext);
	}
}
