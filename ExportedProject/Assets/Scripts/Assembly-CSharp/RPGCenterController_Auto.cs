using System.Collections.Generic;
using RPGMODE;
using UnityEngine;

public class RPGCenterController_Auto : TBaseEntityCenterController
{
	public enum EState
	{
		None = 0,
		BuffGet = 1,
		Idle = 2,
		Begin_Bout = 3,
		Select_Aims = 4,
		Select_Aims_Manual = 5,
		Attack = 6,
		End_Bout = 7,
		Death = 8,
		MaxCount = 9
	}

	private StateMachine<RPGCenterController_Auto> stateMachine;

	private TStateFactory<RPGCenterController_Auto>[] stateFactory;

	private TState<RPGCenterController_Auto>[] objState;

	public GameObject _signSelfObj;

	[SerializeField]
	private bool _autoFight = true;

	private List<RPGEntity> _mainAim = new List<RPGEntity>();

	[SerializeField]
	public int _attackCount = 1;

	[SerializeField]
	public int _curAttackCount;

	[SerializeField]
	public float _muliAttackDecrease;

	[SerializeField]
	public int _extraATTCount;

	[SerializeField]
	public int _curExtraATTCount;

	private float v1;

	private float fCloseSpeed;

	private Vector3 _closedir = Vector3.zero;

	private Vector3 _oriPos = Vector3.zero;

	public float gravity = 60f;

	public float vJump = 9f;

	private float fTime_MinEndBout;

	private bool bDodgeIgnoreChance = true;

	public bool AutoFight
	{
		get
		{
			return _autoFight;
		}
		set
		{
			_autoFight = value;
		}
	}

	public List<RPGEntity> MainAim
	{
		get
		{
			return _mainAim;
		}
		set
		{
			_mainAim = value;
		}
	}

	public float Time_MinEndBout
	{
		get
		{
			return fTime_MinEndBout;
		}
		set
		{
			fTime_MinEndBout = value;
		}
	}

	public RPGCenterController_Auto(TBaseEntity own)
		: base(own)
	{
	}

	protected override int InitController()
	{
		stateMachine = new StateMachine<RPGCenterController_Auto>(this);
		stateFactory = new TStateFactory<RPGCenterController_Auto>[9];
		stateFactory[0] = new RPGRoleStateCreator_None();
		stateFactory[1] = new RPGRoleStateCreator_BuffGet();
		stateFactory[2] = new RPGRoleStateCreator_Idle();
		stateFactory[3] = new RPGRoleStateCreator_Begin_Bout();
		stateFactory[4] = new RPGRoleStateCreator_Select_Aims();
		stateFactory[5] = new RPGRoleStateCreator_Select_Aims_Manual();
		stateFactory[6] = new RPGRoleStateCreator_Attack();
		stateFactory[7] = new RPGRoleStateCreator_End_Bout();
		stateFactory[8] = new RPGRoleStateCreator_Death();
		objState = new TState<RPGCenterController_Auto>[9];
		for (int i = 0; i < 9; i++)
		{
			objState[i] = stateFactory[i].CreateState();
		}
		stateMachine.Init(objState[2], new RPGRoleStateCreator_Global().CreateState());
		return 0;
	}

	public void ChangeState(EState curtate)
	{
		if (curtate != EState.MaxCount)
		{
			stateMachine.ChangeState(objState[(int)curtate]);
		}
	}

	public StateMachine<RPGCenterController_Auto> GetStateMachine()
	{
		return stateMachine;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		base.HandleMessage(msg);
		return stateMachine.HandleMessage(msg);
	}

	public override void Tick()
	{
		stateMachine.Update();
	}

	public bool IsValidAim()
	{
		int num = 0;
		for (int i = 0; i < MainAim.Count; i++)
		{
			if (MainAim[i].CurHp <= 0f)
			{
				num++;
			}
		}
		return (num != MainAim.Count) ? true : false;
	}

	public bool IsCanSelectATTAim()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		for (int i = 0; i < enemyTeam.MemberLst.Count; i++)
		{
			if (enemyTeam.MemberLst[i].CurHp > 0f)
			{
				return true;
			}
		}
		return false;
	}

	public int SelectATTAim()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		RPGTSkill attackSkill = rPGEntity.GetAttackSkill();
		attackSkill.Skill_SelectAttackAim(this);
		return _mainAim.Count;
	}

	public bool EnemyHasAlivePupil()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		return enemyTeam.HasAliveCareer(ERPGCareer.Pupil);
	}

	public void ClearAimTeamArearMark()
	{
		for (int i = 0; i < MainAim.Count; i++)
		{
			MainAim[i].TeamOwner.ArearAttackHas = false;
		}
	}

	public void MarkAim()
	{
		MarkAim(false);
	}

	public void MarkAim(bool manual)
	{
		for (int i = 0; i < MainAim.Count; i++)
		{
			GameObject gameObject = null;
			gameObject = ((MainAim[i].CareerUnit.CareerId != 40) ? (Object.Instantiate(Resources.Load("Common/Ring/PFB_Ring_Red_Aim")) as GameObject) : (Object.Instantiate(Resources.Load("Common/Ring/PFB_Ring_Red_Aim_Tank")) as GameObject));
			gameObject.transform.parent = MainAim[i].transform;
			gameObject.transform.localPosition = Vector3.zero;
			Object.Destroy(gameObject, 1f);
		}
	}

	public int ClosetoAim()
	{
		RPGEntity rPGEntity = _mainAim[0];
		RPGEntity rPGEntity2 = GetOwner() as RPGEntity;
		_closedir.y -= gravity * Time.deltaTime;
		rPGEntity2.GetComponent<CharacterController>().Move(_closedir * Time.deltaTime);
		return 0;
	}

	public void InitDeathSpeed(float f)
	{
		v1 = f;
	}

	public int CloseToDeathPos(Vector3 dir, float v, float acc)
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		rPGEntity.transform.position += v * dir * Time.deltaTime;
		Vector3 eulerAngles = rPGEntity.transform.localRotation.eulerAngles;
		return 0;
	}

	public float JudgeIsNeedApproachAim()
	{
		fCloseSpeed = 0f;
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		_oriPos = rPGEntity.transform.position;
		if (rPGEntity.CareerUnit.AttackType == RPGCareerUnit.ECareerAttackType.Melee)
		{
			float num = 0.3f;
			RPGEntity rPGEntity2 = _mainAim[0];
			Vector3 position = rPGEntity2.MeleeAttackTran.position;
			if (rPGEntity2.TeamOwner == rPGEntity.TeamOwner)
			{
				position = rPGEntity2.MeleeAttackTranSelf.position;
			}
			float num2 = Vector3.Distance(position, rPGEntity.transform.position);
			num = vJump * 2f / gravity;
			fCloseSpeed = num2 / num;
			_closedir = position - rPGEntity.transform.position;
			_closedir.Normalize();
			_closedir *= fCloseSpeed;
			_closedir.y = vJump;
			return num;
		}
		if (rPGEntity.CareerUnit.AttackType == RPGCareerUnit.ECareerAttackType.Remote)
		{
			return 0f;
		}
		return -1f;
	}

	public float JudgeIsNeedBack()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		if (rPGEntity.CareerUnit.AttackType == RPGCareerUnit.ECareerAttackType.Melee)
		{
			float num = 0.3f;
			RPGEntity rPGEntity2 = _mainAim[0];
			float num2 = Vector3.Distance(rPGEntity.transform.position, _oriPos);
			num = vJump * 2f / gravity;
			fCloseSpeed = num2 / num;
			_closedir = _oriPos - rPGEntity.transform.position;
			_closedir.Normalize();
			_closedir *= fCloseSpeed;
			_closedir.y = vJump;
			return num;
		}
		if (rPGEntity.CareerUnit.AttackType == RPGCareerUnit.ECareerAttackType.Remote)
		{
			return 0f;
		}
		return -1f;
	}

	public virtual void RealAttack(float delay)
	{
		Debug.LogWarning("Real Attack!  " + MainAim.Count);
		for (int i = 0; i < MainAim.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(GetOwner().GetInstanceID(), _mainAim[i].GetInstanceID(), 5017, TTelegram.SEND_MSG_IMMEDIATELY, GetOwner(), delay);
		}
	}

	public virtual void RealAttack(float delay, float fDelay)
	{
		for (int i = 0; i < MainAim.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(GetOwner().GetInstanceID(), _mainAim[i].GetInstanceID(), 5017, fDelay, GetOwner(), delay);
		}
	}

	public void RealAttackEnd()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		if (rPGEntity.CareerUnit.CareerId != 2)
		{
			rPGEntity.transform.position = _oriPos;
			rPGEntity.PlayAni(RPGEntity.EAniLST.Idle);
		}
	}

	public float NotifyBuffsBeginBount(RPGRole_PlayerState_Begin_Bout state)
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		return rPGEntity.NotifyBuffBeginBount(state);
	}

	public void NotifyBuffsEndBount()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		rPGEntity.NotifyBuffEndBount();
	}

	public bool HasDodgeIgnoreChance()
	{
		return bDodgeIgnoreChance;
	}

	public void DisableDodgeIgnoreChance()
	{
		bDodgeIgnoreChance = false;
	}

	public void GainDodgeIgnoreChance()
	{
		bDodgeIgnoreChance = true;
	}

	public void InitSkillAttackCount()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		GainDodgeIgnoreChance();
		rPGEntity.GetAttackSkill().Skill_InitAttackCount(this);
	}

	public int HealingHPToAnyTeamMem(out RPGEntity heal, out float fHp)
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		List<RPGEntity> list = new List<RPGEntity>();
		for (int i = 0; i < teamOwner.MemberLst.Count; i++)
		{
			if (teamOwner.MemberLst[i].CurHp > 0f)
			{
				list.Add(teamOwner.MemberLst[i]);
			}
		}
		if (list.Count > 0)
		{
			int num = Random.Range(0, list.Count);
			RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[rPGEntity.CareerUnit.SkillLst[0]];
			float num2 = (float)(int)rPGSkillUnit.ParamLst[1] / 100f * list[num].MaxHp;
			Debug.Log("Heal HP:" + Mathf.FloorToInt(num2));
			heal = list[num];
			fHp = num2;
			return num;
		}
		heal = null;
		fHp = 0f;
		return -1;
	}

	public void HealingHPToAllTeamMem()
	{
		RPGEntity rPGEntity = GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		List<RPGEntity> list = new List<RPGEntity>();
		for (int i = 0; i < teamOwner.MemberLst.Count; i++)
		{
			if (teamOwner.MemberLst[i].CurHp > 0f)
			{
				list.Add(teamOwner.MemberLst[i]);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		for (int j = 0; j < list.Count; j++)
		{
			RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[rPGEntity.CareerUnit.SkillLst[0]];
			float num = (float)(int)rPGSkillUnit.ParamLst[0] / 100f * list[j].MaxHp;
			Debug.Log("Heal HP:" + Mathf.FloorToInt(num));
			list[j].CurHp += num;
			SSHurtNum.Instance.HealingFont(num, list[j].transform);
			Object obj = Resources.Load("Particle/effect/Skill/RPB_BUFF_Holylight/RPB_BUFF_Holylight");
			if (obj != null)
			{
				Vector3 position = list[j].transform.position;
				GameObject gameObject = Object.Instantiate(obj, position, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				gameObject.transform.parent = list[j].transform;
				Object.DestroyObject(gameObject, 1f);
			}
		}
	}
}
