using System.Collections;
using System.Collections.Generic;
using Protocol;
using RPGMODE;
using UnityEngine;

public class RPGTeam : TBaseEntity
{
	[SerializeField]
	public enum ETeamShowBuffState
	{
		None = 0,
		Skill_Awake = 1,
		InitNormalTactic = 2,
		InitSpecTactic = 3
	}

	[SerializeField]
	private RPGRefree _refree;

	[SerializeField]
	private Transform _areaAttackTran;

	[SerializeField]
	private bool _bArearAttackHas;

	[SerializeField]
	private List<Transform> _deathPosLst = new List<Transform>();

	[SerializeField]
	private bool _autoFight = true;

	[SerializeField]
	private bool _self;

	[SerializeField]
	private List<RPGEntity> _memberLst = new List<RPGEntity>();

	[SerializeField]
	private List<RPGTTactic> _tacticLst = new List<RPGTTactic>();

	[SerializeField]
	private int _teamDeadNum;

	private RPGEntity _diedEntity;

	[SerializeField]
	private List<RPGTTactic> _tacticNormalLst = new List<RPGTTactic>();

	[SerializeField]
	private List<RPGTTactic> _tacticSpecLst = new List<RPGTTactic>();

	[SerializeField]
	private List<RPGShowGetBuff> _showGetBuffLst = new List<RPGShowGetBuff>();

	[SerializeField]
	private bool _bTestAutoFight;

	[SerializeField]
	private ETeamShowBuffState _curTeamShowBuffState;

	[SerializeField]
	private float _curSkillAwakeEnterTime;

	[SerializeField]
	private float _curSkillAwakeDurTime;

	[SerializeField]
	private int _curSkillAwakeMemIndex;

	[SerializeField]
	private int _curSkillAwakeMemSkillIndex;

	[SerializeField]
	private int _curSkillAwakeMemSkillCout;

	[SerializeField]
	private float _curNormalTacticInitEnterTime;

	[SerializeField]
	private float _curNormalTacticInitDurTime;

	[SerializeField]
	private int _curNormalTacticIndex;

	[SerializeField]
	private float _curSpecTacticInitEnterTime;

	[SerializeField]
	private float _curSpecTacticInitDurTime;

	[SerializeField]
	private int _curSpecTacticIndex;

	public RPGRefree Refree
	{
		get
		{
			return _refree;
		}
	}

	public Transform AreaAttackTran
	{
		get
		{
			return _areaAttackTran;
		}
	}

	public bool ArearAttackHas
	{
		get
		{
			return _bArearAttackHas;
		}
		set
		{
			_bArearAttackHas = value;
		}
	}

	public bool AutoFight
	{
		get
		{
			return _autoFight;
		}
		set
		{
			_autoFight = value;
			for (int i = 0; i < MemberLst.Count; i++)
			{
				RPGEntity rPGEntity = MemberLst[i];
				RPGCenterController_Auto rPGCenterController_Auto = rPGEntity.GetCenterController() as RPGCenterController_Auto;
				rPGCenterController_Auto.AutoFight = _autoFight;
				if (rPGCenterController_Auto.AutoFight)
				{
					TMessageDispatcher.Instance.DispatchMsg(-1, rPGEntity.GetInstanceID(), 5023, TTelegram.SEND_MSG_IMMEDIATELY, null);
				}
			}
		}
	}

	public List<RPGEntity> MemberLst
	{
		get
		{
			return _memberLst;
		}
	}

	public List<RPGTTactic> TacticLst
	{
		get
		{
			return _tacticLst;
		}
	}

	public Transform GetRandomDeathPos()
	{
		return _deathPosLst[Random.Range(0, _deathPosLst.Count)];
	}

	public int GetAliveFarmerCount()
	{
		int num = 0;
		for (int i = 0; i < _memberLst.Count; i++)
		{
			if (_memberLst[i].CareerUnit.CareerId == 1 && _memberLst[i].CurHp > 0f)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAliveMemCout()
	{
		int num = 0;
		for (int i = 0; i < _memberLst.Count; i++)
		{
			if (_memberLst[i].CurHp > 0f)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAliveMemExceptSelfCout(RPGEntity self)
	{
		int num = 0;
		for (int i = 0; i < _memberLst.Count; i++)
		{
			if (_memberLst[i].CurHp > 0f && _memberLst[i] != self)
			{
				num++;
			}
		}
		return num;
	}

	public bool HasAliveCareer(ERPGCareer career)
	{
		for (int i = 0; i < _memberLst.Count; i++)
		{
			if (_memberLst[i].CareerUnit.CareerId == (int)career && _memberLst[i].CurHp > 0f)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasAliveSkill(int skillId)
	{
		bool flag = false;
		for (int i = 0; i < _memberLst.Count; i++)
		{
			if (_memberLst[i].CurHp > 0f)
			{
				flag = _memberLst[i].IsExistSkill(skillId);
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	private bool IsExistTactic(int id)
	{
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (_tacticLst[i].TacticUnit.TacticId == id)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsExistTactic(int id, out RPGTTactic ta)
	{
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (_tacticLst[i].TacticUnit.TacticId == id)
			{
				ta = _tacticLst[i];
				return true;
			}
		}
		ta = null;
		return false;
	}

	public float EquipTactic(int id, RPGEntity owner)
	{
		RPGTTactic rPGTTactic = RPGGlobalFun.EquipTacticById(id, this);
		int tacticId = rPGTTactic.TacticUnit.TacticId;
		rPGTTactic.TacticOwner.Add(owner);
		if (!rPGTTactic.TacticUnit.CanOverlap)
		{
			RPGTTactic ta = null;
			if (IsExistTactic(tacticId, out ta))
			{
				ta.TacticOwner.Add(owner);
				Object.Destroy(rPGTTactic);
				return -1f;
			}
		}
		Debug.Log("Equip Tactic=" + id);
		_tacticLst.Add(rPGTTactic);
		return 0f;
	}

	public void NotifyMembleDied(RPGEntity entity, float fDelay)
	{
		StartCoroutine(MembleDied(entity, fDelay));
	}

	public RPGEntity GetCurDiedEntity()
	{
		return _diedEntity;
	}

	private IEnumerator MembleDied(RPGEntity entity, float fDelay)
	{
		_diedEntity = entity;
		yield return new WaitForSeconds(fDelay);
		RPGCenterController_Auto contr = entity.GetCenterController() as RPGCenterController_Auto;
		contr.ChangeState(RPGCenterController_Auto.EState.Death);
		_teamDeadNum++;
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (!_tacticLst[i].Spec)
			{
				_tacticLst[i].ConditionChanged();
			}
		}
		for (int j = 0; j < _tacticLst.Count; j++)
		{
			if (_tacticLst[j].Spec)
			{
				_tacticLst[j].ConditionChanged();
			}
		}
		Refree.NotifyTeamMemDeath(_diedEntity);
		if (_teamDeadNum >= MemberLst.Count)
		{
			Refree.NotifyTeamAllDeath();
		}
		_refree.BattleRoleListChanged();
	}

	public void NotifyMembleRevive(RPGEntity entity)
	{
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (!_tacticLst[i].Spec)
			{
				_tacticLst[i].ConditionChanged();
			}
		}
		for (int j = 0; j < _tacticLst.Count; j++)
		{
			if (_tacticLst[j].Spec)
			{
				_tacticLst[j].ConditionChanged();
			}
		}
	}

	public void InitBattleMem(string configPath, bool pc)
	{
		Debug_TeamParam debug_TeamParam = new Debug_TeamParam();
		Debug_TeamParam debug_TeamParam2 = new Debug_TeamParam();
		Debug.Log("Before Serialize!");
		string text = null;
		text = ((!RPGRefree.Instance.IsPCLoadFromCurScene()) ? COMA_FileIO.LoadFile("Levels", configPath) : COMA_FileIO.ReadTextDirectly(configPath));
		debug_TeamParam = COMA_Tools.DeserializeObject<Debug_TeamParam>(text) as Debug_TeamParam;
		TextAsset textAsset = Resources.Load("Data/Team_enemy", typeof(TextAsset)) as TextAsset;
		debug_TeamParam2 = COMA_Tools.DeserializeObject<Debug_TeamParam>(textAsset.text) as Debug_TeamParam;
		Debug.Log("After Serialize!");
		for (int i = 0; i < debug_TeamParam._careerLst.Count; i++)
		{
			if (RPGGlobalData.Instance.CareerUnitPool._dict.ContainsKey(debug_TeamParam._careerLst[i]))
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Player/Character/RPG/Player_RPG")) as GameObject;
				RPGEntity component = gameObject.GetComponent<RPGEntity>();
				component.InitBlood((!_self) ? BloodBar.BarColor.Red : BloodBar.BarColor.Blue);
				RPGCenterController_Auto rPGCenterController_Auto = component.GetCenterController() as RPGCenterController_Auto;
				if (rPGCenterController_Auto == null)
				{
					Debug.Log("=================================================");
				}
				rPGCenterController_Auto.AutoFight = AutoFight;
				component.RoleLv = debug_TeamParam.lv;
				component.TeamOwner = this;
				RPGCareerUnit rPGCareerUnit = (component.CareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[debug_TeamParam._careerLst[i]]);
				gameObject.name = string.Concat("Player", (ERPGCareer)component.CareerUnit.CareerId, i);
				component.InitAttr();
				component.InitAnis();
				component.InitProfile();
				component.InitEquipment(debug_TeamParam._equipmentLst[i]);
				gameObject.transform.parent = base.transform;
				if (pc)
				{
					gameObject.transform.position = debug_TeamParam._transParamLst[i]._vPos;
					gameObject.transform.rotation = Quaternion.Euler(debug_TeamParam._transParamLst[i]._vRot);
				}
				else
				{
					gameObject.transform.position = debug_TeamParam2._transParamLst[i]._vPos;
					gameObject.transform.rotation = Quaternion.Euler(debug_TeamParam2._transParamLst[i]._vRot);
				}
				if (rPGCareerUnit.CareerId >= 500)
				{
					gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
				}
				else if (rPGCareerUnit.CareerId == 40)
				{
					gameObject.transform.position += new Vector3(0f, 0.76f, 0f);
				}
				List<int> skillLst = rPGCareerUnit.SkillLst;
				for (int j = 0; j < skillLst.Count; j++)
				{
					component.EquipSkill(skillLst[j]);
				}
				_memberLst.Add(component);
			}
			else
			{
				Debug.Log("Career=" + debug_TeamParam._careerLst[i] + "  can not find!");
			}
		}
		Debug.Log("Team Ready! Tactic Count=" + _tacticLst.Count);
		_refree.TeamInitEndNotify();
	}

	private void AddEquipUnit(RPGGemCompoundTableUnit.EAttrType type, int value, ref RPGEquipmentUnit unit)
	{
		switch (type)
		{
		case RPGGemCompoundTableUnit.EAttrType._boutLimit_ATT:
			if (unit._boutLimit_ATT == null)
			{
				unit._boutLimit_ATT = new EquipAttrPromote();
			}
			unit._boutLimit_ATT._boutCount = 2;
			unit._boutLimit_ATT._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._boutLimit_DEF:
			if (unit._boutLimit_DEF == null)
			{
				unit._boutLimit_DEF = new EquipAttrPromote();
			}
			unit._boutLimit_DEF._boutCount = 3;
			unit._boutLimit_DEF._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._ATT:
			if (unit._ATT == null)
			{
				unit._ATT = new EquipAttrPromote();
			}
			unit._ATT._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._DEF:
			if (unit._DEF == null)
			{
				unit._DEF = new EquipAttrPromote();
			}
			unit._DEF._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._DodgeRate:
			if (unit._DodgeRate == null)
			{
				unit._DodgeRate = new EquipAttrPromote();
			}
			unit._DodgeRate._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._CriticalRate:
			if (unit._CriticalRate == null)
			{
				unit._CriticalRate = new EquipAttrPromote();
			}
			unit._CriticalRate._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._ATIndex:
			if (unit._ATIndex == null)
			{
				unit._ATIndex = new EquipAttrPromote();
			}
			unit._ATIndex._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._HP:
			if (unit._HP == null)
			{
				unit._HP = new EquipAttrPromote();
			}
			unit._HP._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._CriticalMultiplier:
			if (unit._CriticalMultiplier == null)
			{
				unit._CriticalMultiplier = new EquipAttrPromote();
			}
			unit._CriticalMultiplier._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._ITDValue:
			if (unit._ITDValue == null)
			{
				unit._ITDValue = new EquipAttrPromote();
			}
			unit._ITDValue._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._ResistLimitBuffRate:
			if (unit._FrozenResistRate == null)
			{
				unit._FrozenResistRate = new EquipAttrPromote();
			}
			unit._FrozenResistRate._promoteValue += value;
			if (unit._StunResistRate == null)
			{
				unit._StunResistRate = new EquipAttrPromote();
			}
			unit._StunResistRate._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._SuckHP:
			if (unit._SuckHP == null)
			{
				unit._SuckHP = new EquipAttrPromote();
			}
			unit._SuckHP._promoteValue += value;
			break;
		case RPGGemCompoundTableUnit.EAttrType._LimitRHP:
			if (unit._LimitRHP == null)
			{
				unit._LimitRHP = new EquipAttrPromote();
			}
			unit._LimitRHP._promoteValue += value;
			break;
		}
	}

	private void InitRPGEquipmentUnit(Equip head, Equip body, Equip leg, ref RPGEquipmentUnit unit)
	{
		if (head != null && RPGGlobalData.Instance.CompoundTableUnitPool._dict.ContainsKey(head.m_type))
		{
			RPGGemCompoundTableUnit rPGGemCompoundTableUnit = RPGGlobalData.Instance.CompoundTableUnitPool._dict[head.m_type];
			AddEquipUnit(rPGGemCompoundTableUnit._attrType, rPGGemCompoundTableUnit._apList[head.m_level - 1], ref unit);
		}
		if (body != null && RPGGlobalData.Instance.CompoundTableUnitPool._dict.ContainsKey(body.m_type))
		{
			RPGGemCompoundTableUnit rPGGemCompoundTableUnit2 = RPGGlobalData.Instance.CompoundTableUnitPool._dict[body.m_type];
			AddEquipUnit(rPGGemCompoundTableUnit2._attrType, rPGGemCompoundTableUnit2._apList[body.m_level - 1], ref unit);
		}
		if (leg != null && RPGGlobalData.Instance.CompoundTableUnitPool._dict.ContainsKey(leg.m_type))
		{
			RPGGemCompoundTableUnit rPGGemCompoundTableUnit3 = RPGGlobalData.Instance.CompoundTableUnitPool._dict[leg.m_type];
			AddEquipUnit(rPGGemCompoundTableUnit3._attrType, rPGGemCompoundTableUnit3._apList[leg.m_level - 1], ref unit);
		}
	}

	public int InitBattleMem(bool self, int lv, MemberSlot[] memSlot, Dictionary<ulong, Equip> equipBag)
	{
		string text = "Data/Team_self";
		text = ((!self) ? "Data/Team_enemy" : "Data/Team_self");
		int num = 0;
		TextAsset textAsset = Resources.Load(text, typeof(TextAsset)) as TextAsset;
		Debug_TeamParam debug_TeamParam = new Debug_TeamParam();
		debug_TeamParam = COMA_Tools.DeserializeObject<Debug_TeamParam>(textAsset.text) as Debug_TeamParam;
		for (int i = 0; i < memSlot.Length; i++)
		{
			int member = (int)memSlot[i].m_member;
			if (RPGGlobalData.Instance.CareerUnitPool._dict.ContainsKey(member))
			{
				num++;
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Player/Character/RPG/Player_RPG")) as GameObject;
				RPGEntity component = gameObject.GetComponent<RPGEntity>();
				component.InitBlood((!_self) ? BloodBar.BarColor.Red : BloodBar.BarColor.Blue);
				RPGCenterController_Auto rPGCenterController_Auto = component.GetCenterController() as RPGCenterController_Auto;
				if (rPGCenterController_Auto == null)
				{
					Debug.Log("=================================================");
				}
				rPGCenterController_Auto.AutoFight = AutoFight;
				component.RoleLv = lv;
				component.TeamOwner = this;
				RPGCareerUnit rPGCareerUnit = (component.CareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[member]);
				gameObject.name = string.Concat("Player", (ERPGCareer)component.CareerUnit.CareerId, i);
				component.InitAttr();
				component.InitAnis();
				Equip equip = ((!equipBag.ContainsKey(memSlot[i].m_head)) ? null : equipBag[memSlot[i].m_head]);
				Equip equip2 = ((!equipBag.ContainsKey(memSlot[i].m_body)) ? null : equipBag[memSlot[i].m_body]);
				Equip equip3 = ((!equipBag.ContainsKey(memSlot[i].m_leg)) ? null : equipBag[memSlot[i].m_leg]);
				string head = ((equip == null) ? string.Empty : equip.m_md5);
				string body = ((equip2 == null) ? string.Empty : equip2.m_md5);
				string leg = ((equip3 == null) ? string.Empty : equip3.m_md5);
				component.InitProfile(head, body, leg);
				RPGEquipmentUnit unit = new RPGEquipmentUnit();
				InitRPGEquipmentUnit(equip, equip2, equip3, ref unit);
				component.InitEquipment(unit);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = debug_TeamParam._transParamLst[i]._vPos;
				gameObject.transform.rotation = Quaternion.Euler(debug_TeamParam._transParamLst[i]._vRot);
				if (rPGCareerUnit.CareerId >= 500)
				{
					gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
				}
				else if (rPGCareerUnit.CareerId == 40)
				{
					gameObject.transform.position += new Vector3(0f, 0.76f, 0f);
				}
				List<int> skillLst = rPGCareerUnit.SkillLst;
				for (int j = 0; j < skillLst.Count; j++)
				{
					component.EquipSkill(skillLst[j]);
				}
				_memberLst.Add(component);
			}
		}
		Debug.Log("Team Ready! Tactic Count=" + _tacticLst.Count);
		_refree.TeamInitEndNotify();
		return num;
	}

	private void Start()
	{
		if (_areaAttackTran == null)
		{
			_areaAttackTran = base.transform.FindChild("AreaAttackPos");
		}
	}

	public float InitNormalTactic()
	{
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (!_tacticLst[i].Spec)
			{
				_tacticLst[i].InitTactic();
			}
		}
		return 0f;
	}

	private int GetNormalTacticCount()
	{
		_tacticNormalLst.Clear();
		int num = 0;
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (!_tacticLst[i].Spec)
			{
				_tacticNormalLst.Add(_tacticLst[i]);
				num++;
			}
		}
		return num;
	}

	public float InitSpecTactic()
	{
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (_tacticLst[i].Spec)
			{
				_tacticLst[i].InitTactic();
			}
		}
		return 0f;
	}

	private int GetSpecTacticCount()
	{
		_tacticSpecLst.Clear();
		int num = 0;
		for (int i = 0; i < _tacticLst.Count; i++)
		{
			if (_tacticLst[i].Spec)
			{
				_tacticSpecLst.Add(_tacticLst[i]);
				num++;
			}
		}
		return num;
	}

	private new void Update()
	{
		switch (_curTeamShowBuffState)
		{
		case ETeamShowBuffState.None:
			break;
		case ETeamShowBuffState.Skill_Awake:
			if (!(Time.time - _curSkillAwakeEnterTime >= _curSkillAwakeDurTime))
			{
				break;
			}
			_curSkillAwakeMemSkillIndex++;
			if (_curSkillAwakeMemSkillIndex >= _curSkillAwakeMemSkillCout)
			{
				_curSkillAwakeMemIndex++;
				if (_curSkillAwakeMemIndex >= MemberLst.Count)
				{
					_curTeamShowBuffState = ETeamShowBuffState.None;
					Refree.TeamSkillAwaked(this);
					break;
				}
				_curSkillAwakeEnterTime = Time.time;
				_curSkillAwakeMemSkillIndex = 0;
				_curSkillAwakeMemSkillCout = MemberLst[_curSkillAwakeMemIndex].GetSkillCount();
				_curSkillAwakeDurTime = MemberLst[_curSkillAwakeMemIndex].GetSkillByIndex(_curSkillAwakeMemSkillIndex).ActiveSkill();
			}
			else
			{
				_curSkillAwakeEnterTime = Time.time;
				_curSkillAwakeDurTime = MemberLst[_curSkillAwakeMemIndex].GetSkillByIndex(_curSkillAwakeMemSkillIndex).ActiveSkill();
			}
			break;
		case ETeamShowBuffState.InitNormalTactic:
			if (Time.time - _curNormalTacticInitEnterTime >= _curNormalTacticInitDurTime)
			{
				_curNormalTacticIndex++;
				ProcessNotifyTeamInitNormalTactic();
			}
			break;
		case ETeamShowBuffState.InitSpecTactic:
			if (Time.time - _curSpecTacticInitEnterTime >= _curSpecTacticInitDurTime)
			{
				_curSpecTacticIndex++;
				ProcessNotifyTeamInitSpecTactic();
			}
			break;
		}
	}

	private void ProcessNotifyTeamInitNormalTactic()
	{
		if (_curNormalTacticIndex >= GetNormalTacticCount())
		{
			_curTeamShowBuffState = ETeamShowBuffState.None;
			Refree.TeamNormalTacticInited(this);
		}
		else
		{
			_curNormalTacticInitEnterTime = Time.time;
			_curNormalTacticInitDurTime = _tacticNormalLst[_curNormalTacticIndex].InitTactic();
			_curTeamShowBuffState = ETeamShowBuffState.InitNormalTactic;
		}
	}

	private void ProcessNotifyTeamInitSpecTactic()
	{
		if (_curSpecTacticIndex >= GetSpecTacticCount())
		{
			_curTeamShowBuffState = ETeamShowBuffState.None;
			Refree.TeamSpecTacticInited(this);
		}
		else
		{
			_curSpecTacticInitEnterTime = Time.time;
			_curSpecTacticInitDurTime = _tacticSpecLst[_curSpecTacticIndex].InitTactic();
			_curTeamShowBuffState = ETeamShowBuffState.InitSpecTactic;
		}
	}

	public override bool HandleMessage(TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 5020:
			result = true;
			_curSkillAwakeEnterTime = Time.time;
			_curSkillAwakeMemIndex = 0;
			_curSkillAwakeMemSkillIndex = 0;
			_curSkillAwakeMemSkillCout = MemberLst[_curSkillAwakeMemIndex].GetSkillCount();
			_curSkillAwakeDurTime = MemberLst[_curSkillAwakeMemIndex].GetSkillByIndex(_curSkillAwakeMemSkillIndex).ActiveSkill();
			_curTeamShowBuffState = ETeamShowBuffState.Skill_Awake;
			break;
		case 5021:
			result = true;
			_curNormalTacticIndex = 0;
			ProcessNotifyTeamInitNormalTactic();
			break;
		case 5022:
			result = true;
			_curSpecTacticIndex = 0;
			ProcessNotifyTeamInitSpecTactic();
			break;
		}
		return result;
	}
}
