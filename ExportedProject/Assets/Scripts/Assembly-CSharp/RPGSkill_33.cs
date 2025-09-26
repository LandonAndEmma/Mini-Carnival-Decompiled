using UnityEngine;

public class RPGSkill_33 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[33];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}

	public override float Skill_BeginBout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		float num = base.SkillOwner.PlayAni(RPGEntity.EAniLST.Buff);
		Object obj = Resources.Load("Particle/effect/Skill/RPG_BUFF_Biochemical/RPG_BUFF_Biochemical_Fly");
		if (obj != null && _attTran != null)
		{
			Vector3 position = _attTran.position;
			GameObject gameObject = Object.Instantiate(obj, position, Quaternion.Euler(Vector3.zero)) as GameObject;
			gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			gameObject.transform.parent = _attTran;
			Object.DestroyObject(gameObject, 2f);
		}
		AddOnlyIconBuff("Particle/effect/Skill/RPG_Icon_Increase_attack/RPG_Icon_Increase_attack");
		return state.DealyAttack(t, 1.5f);
	}

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		return Mathf.FloorToInt((float)dam * (1f + (float)_skillOwner.CurBattleBoutIndex * ((float)(int)_skillUnit.ParamLst[0] / 100f)));
	}
}
