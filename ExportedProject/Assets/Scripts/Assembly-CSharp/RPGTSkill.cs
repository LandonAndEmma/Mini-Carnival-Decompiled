using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGTSkill : MonoBehaviour
{
	public static readonly int MAX_DAM = 9999999;

	protected Transform _attTran;

	[SerializeField]
	protected RPGEntity _skillOwner;

	private GameObject _launchEffectObj;

	protected RPGSkillUnit _skillUnit;

	public RPGEntity SkillOwner
	{
		get
		{
			return _skillOwner;
		}
		set
		{
			_skillOwner = value;
			StartCoroutine(FindEffectTran());
		}
	}

	public RPGSkillUnit SkillUnit
	{
		get
		{
			return _skillUnit;
		}
		set
		{
			_skillUnit = value;
		}
	}

	private IEnumerator FindEffectTran()
	{
		yield return new WaitForSeconds(0.3f);
		if (!RPGGlobalData.Instance.AttackEffectPool._dict.ContainsKey(_skillOwner.CareerUnit.CareerId))
		{
			yield break;
		}
		RPGAttackEffectUnit unit = RPGGlobalData.Instance.AttackEffectPool._dict[_skillOwner.CareerUnit.CareerId];
		for (int i = 8; i >= 0; i--)
		{
			Transform t = _skillOwner.Charac.GetDecoByPart((RPGCharacter.EDecorationPart)i);
			if (t.childCount > 0)
			{
				_attTran = t.GetChild(0).Find(unit.LaunchLoc);
				if (_attTran == null)
				{
					RPGAimAtTarget aimTar = t.GetChild(0).GetComponent<RPGAimAtTarget>();
					if (aimTar != null)
					{
						_attTran = aimTar._effectTriggerPos;
					}
				}
				if (_attTran != null)
				{
					break;
				}
			}
		}
		if (_attTran == null)
		{
			_attTran = _skillOwner.gameObject.transform;
		}
	}

	public virtual float ActiveSkill()
	{
		return 0f;
	}

	public virtual int UnactiveSkill()
	{
		return 0;
	}

	public virtual float Skill_Awake()
	{
		if (SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Awake)
		{
			return ActiveSkill();
		}
		return 0f;
	}

	public virtual float Skill_BeginBout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		if (SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Begin_Bout)
		{
			ActiveSkill();
		}
		return 0f;
	}

	public virtual void Skill_PreAttack()
	{
		if (SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Pre_Attack)
		{
			ActiveSkill();
		}
	}

	public virtual void Skill_AftAttack()
	{
		if (SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Aft_Attack)
		{
			ActiveSkill();
		}
	}

	public virtual void Skill_EndBout()
	{
		if (SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.End_Bout)
		{
			ActiveSkill();
		}
	}

	protected int CompareAimSel(RPGEntity entity1, RPGEntity entity2)
	{
		if (entity1.CareerUnit.BeAttackPriority == entity2.CareerUnit.BeAttackPriority)
		{
			float num = entity1.CurHp / entity1.CalcAttr.Attrs[5];
			float num2 = entity2.CurHp / entity2.CalcAttr.Attrs[5];
			if (num == num2)
			{
				return 0;
			}
			return (num - num2 > 0f) ? 1 : (-1);
		}
		return (entity1.CareerUnit.BeAttackPriority - entity2.CareerUnit.BeAttackPriority > 0) ? 1 : (-1);
	}

	public virtual int Skill_SelectAttackAim(RPGCenterController_Auto controller)
	{
		RPGEntity rPGEntity = controller.GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		RPGTSkill attackSkill = rPGEntity.GetAttackSkill();
		controller.MainAim.Clear();
		List<RPGEntity> list = new List<RPGEntity>();
		for (int i = 0; i < enemyTeam.MemberLst.Count; i++)
		{
			if (enemyTeam.MemberLst[i].CurHp > 0f)
			{
				if (enemyTeam.MemberLst[i].CareerUnit.CareerId == 6)
				{
					controller.MainAim.Add(enemyTeam.MemberLst[i]);
					break;
				}
				list.Add(enemyTeam.MemberLst[i]);
			}
		}
		List<RPGEntity> list2 = new List<RPGEntity>();
		List<RPGEntity> list3 = new List<RPGEntity>();
		for (int j = 0; j < enemyTeam.MemberLst.Count; j++)
		{
			if (enemyTeam.MemberLst[j].CurHp > 0f)
			{
				if (!enemyTeam.MemberLst[j].IsExitBuff(220))
				{
					list3.Add(enemyTeam.MemberLst[j]);
				}
				list2.Add(enemyTeam.MemberLst[j]);
			}
		}
		if (rPGEntity.IsExitBuff(221))
		{
			for (int k = 0; k < teamOwner.MemberLst.Count; k++)
			{
				if (teamOwner.MemberLst[k].CurHp > 0f && teamOwner.MemberLst[k] != rPGEntity)
				{
					list.Add(teamOwner.MemberLst[k]);
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count);
				controller.MainAim.Clear();
				controller.MainAim.Add(list[index]);
			}
		}
		else if (controller.MainAim.Count == 0 && list.Count > 0)
		{
			int num = Random.Range(0, 100);
			if (num < 20 && list2.Count > 0)
			{
				controller.MainAim.Add(list2[Random.Range(0, list2.Count)]);
			}
			else
			{
				list.Sort(CompareAimSel);
				int num2 = 0;
				if (SkillUnit.SkillId == 26 || SkillUnit.SkillId == 28 || SkillUnit.SkillId == 32)
				{
					for (int l = 0; l < list.Count && list[l].IsExitBuff(220); l++)
					{
						num2 = l + 1;
					}
				}
				if (num2 < list.Count)
				{
					controller.MainAim.Add(list[num2]);
				}
				else
				{
					controller.MainAim.Add(list[0]);
				}
			}
		}
		return controller.MainAim.Count;
	}

	public virtual int Skill_SelectAttackAim_Manual(RPGCenterController_Auto controller)
	{
		return 0;
	}

	public virtual void Skill_InitAttackCount(RPGCenterController_Auto controller)
	{
		controller._attackCount = 1;
		controller._curAttackCount = 0;
		controller._extraATTCount = 0;
		controller._curExtraATTCount = 0;
	}

	public virtual void Skill_ProcessMultiAttack(RPGRole_PlayerState_Attack state_attack, RPGCenterController_Auto t)
	{
		t.ChangeState(RPGCenterController_Auto.EState.Select_Aims);
	}

	public virtual int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		return dam;
	}

	public virtual void Skill_IsAT(ref bool at, RPGEntity attack, RPGEntity beAttack)
	{
	}

	public virtual void Skill_AttackEffect_LaunchEnd(RPGCenterController_Auto controller)
	{
		if (_launchEffectObj != null)
		{
			Object.DestroyObject(_launchEffectObj);
			_launchEffectObj = null;
		}
	}

	public virtual void Skill_AttackEffect_Launch(RPGCenterController_Auto controller)
	{
		if (!RPGGlobalData.Instance.AttackEffectPool._dict.ContainsKey(SkillOwner.CareerUnit.CareerId))
		{
			return;
		}
		Debug.Log("Pool");
		RPGAttackEffectUnit rPGAttackEffectUnit = RPGGlobalData.Instance.AttackEffectPool._dict[SkillOwner.CareerUnit.CareerId];
		if (!(rPGAttackEffectUnit.LaunchEffectPath != string.Empty))
		{
			return;
		}
		Object obj = Resources.Load(rPGAttackEffectUnit.LaunchEffectPath);
		Debug.Log(rPGAttackEffectUnit.LaunchEffectPath);
		if (!(obj != null) || !(_attTran != null))
		{
			return;
		}
		Debug.Log("obj _attTran");
		Vector3 position = _attTran.position;
		GameObject gameObject = (_launchEffectObj = Object.Instantiate(obj, position, Quaternion.Euler(Vector3.zero)) as GameObject);
		RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
		if (component != null)
		{
			if (component._launchFollowWeapon)
			{
				gameObject.transform.parent = _attTran;
			}
			if (!component._rotateByModel)
			{
				gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			}
			else
			{
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
			if (!component._isLoop)
			{
				Object.DestroyObject(gameObject, component._effectDurationTime);
			}
		}
		else
		{
			Object.DestroyObject(gameObject);
		}
	}

	public virtual void Skill_AttackEffect(RPGCenterController_Auto controller)
	{
		for (int i = 0; i < controller.MainAim.Count; i++)
		{
			if (!RPGGlobalData.Instance.AttackEffectPool._dict.ContainsKey(SkillOwner.CareerUnit.CareerId))
			{
				continue;
			}
			RPGAttackEffectUnit rPGAttackEffectUnit = RPGGlobalData.Instance.AttackEffectPool._dict[SkillOwner.CareerUnit.CareerId];
			Object obj = Resources.Load(rPGAttackEffectUnit.EffectPath);
			if (!(obj != null) || !(_attTran != null))
			{
				continue;
			}
			Vector3 position = _attTran.position;
			GameObject gameObject = Object.Instantiate(obj, position, Quaternion.Euler(Vector3.zero)) as GameObject;
			if (SkillOwner.CareerUnit.AttackType == RPGCareerUnit.ECareerAttackType.Remote)
			{
				RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
				if (component != null)
				{
					float fDur = component._effectDurationTime;
					if (component._effectFlyTime > 0.001f)
					{
						fDur = component._effectFlyTime;
					}
					Ammo_Fly ammo_Fly = gameObject.AddComponent<Ammo_Fly>();
					ammo_Fly.StartFly(controller.MainAim[i].transform, fDur, component._effectFlyOffsetTime);
				}
				else
				{
					Object.DestroyObject(gameObject);
				}
				continue;
			}
			gameObject.transform.parent = _attTran;
			RPGEffectContr component2 = gameObject.GetComponent<RPGEffectContr>();
			if (component2 != null)
			{
				if (!component2._rotateByModel)
				{
					gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				}
				else
				{
					gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				}
				Object.DestroyObject(gameObject, component2._effectDurationTime);
			}
			else
			{
				Object.DestroyObject(gameObject);
			}
		}
	}

	public virtual int Skill_BeAttackEffect(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		if (RPGGlobalData.Instance.BeAttackEffectPool._dict.ContainsKey(attack.CareerUnit.CareerId))
		{
			RPGBeAttackEffectUnit rPGBeAttackEffectUnit = RPGGlobalData.Instance.BeAttackEffectPool._dict[attack.CareerUnit.CareerId];
			Object obj = Resources.Load(rPGBeAttackEffectUnit.EffectPath);
			Debug.Log(rPGBeAttackEffectUnit.EffectPath);
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.parent = beAttack.transform;
				gameObject.transform.localPosition = Vector3.zero;
				RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
				if (component != null)
				{
					if (component._beAttackFollowWeapon)
					{
						gameObject.AddComponent<RPGMarkAimFollow>();
					}
					if (!component._rotateByModel)
					{
						gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
					}
					else
					{
						gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
					}
					Object.DestroyObject(gameObject, component._effectDurationTime);
				}
				else
				{
					Object.DestroyObject(gameObject);
				}
			}
		}
		return dam;
	}

	public virtual int Skill_DAM_Cri(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		return dam;
	}

	protected void PlayShareDAMEffect(RPGEntity knight)
	{
		Object obj = Resources.Load("Particle/effect/Skill/RPG_knight/RPG_knight");
		if (obj != null)
		{
			GameObject gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
			gameObject.transform.parent = knight.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			Object.DestroyObject(gameObject, 1f);
		}
	}

	public virtual int Skill_ProcessShareDAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		if (rPGSkillUnit.CanDAMShare && beAttack.IsExitBuff(114))
		{
			RPGTeam teamOwner = beAttack.TeamOwner;
			List<RPGEntity> list = new List<RPGEntity>();
			for (int i = 0; i < teamOwner.MemberLst.Count; i++)
			{
				if (teamOwner.MemberLst[i].IsExistSkill(20) && teamOwner.MemberLst[i].CurHp > 0f)
				{
					list.Add(teamOwner.MemberLst[i]);
				}
			}
			if (!beAttack.IsExistSkill(20))
			{
				int num = dam / (list.Count + 1);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].CurHp -= num;
					SSHurtNum.Instance.HitNormalFont(num, list[j].transform);
					PlayShareDAMEffect(list[j]);
				}
				return num;
			}
			if (list.Count > 1)
			{
				int num2 = dam / list.Count;
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] != beAttack)
					{
						list[k].CurHp -= num2;
						SSHurtNum.Instance.HitNormalFont(num2, list[k].transform);
						PlayShareDAMEffect(list[k]);
					}
				}
				return num2;
			}
		}
		return dam;
	}

	public virtual void Skill_Feedback(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		beAttack.TriggerBuffer_BeAttack(attack, dam);
	}

	public virtual void Skill_DodgeFeedBack(RPGEntity attack, RPGEntity beAttack)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
	}

	public void AddOnlyIconBuff(string path)
	{
		GameObject gameObject = _skillOwner.gameObject;
		RPGTBuff rPGTBuff = gameObject.AddComponent("RPGBuff_IconOnly") as RPGTBuff;
		rPGTBuff.SecondType = 999;
		rPGTBuff.IsOverlap = false;
		((RPGBuff_IconOnly)rPGTBuff).EffectPath_Icon = path;
		float fLen = 0f;
		if (_skillOwner.AddBuff(rPGTBuff, true, ref fLen, _skillOwner) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + _skillOwner.gameObject.name);
		}
	}

	public void AddOnlyIconBuff_Limit(string path)
	{
		GameObject gameObject = _skillOwner.gameObject;
		RPGTBuff rPGTBuff = gameObject.AddComponent("RPGBuff_IconOnly_LimitTime") as RPGTBuff;
		rPGTBuff.SecondType = 1000;
		rPGTBuff.IsOverlap = true;
		((RPGBuff_IconOnly_LimitTime)rPGTBuff).EffectPath_Icon = path;
		float fLen = 0f;
		if (_skillOwner.AddBuff(rPGTBuff, true, ref fLen, _skillOwner) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + _skillOwner.gameObject.name);
		}
	}

	public virtual void Load()
	{
	}

	public virtual void Unload()
	{
	}

	protected void OnEnable()
	{
		Load();
	}

	protected void OnDisable()
	{
		Unload();
	}
}
