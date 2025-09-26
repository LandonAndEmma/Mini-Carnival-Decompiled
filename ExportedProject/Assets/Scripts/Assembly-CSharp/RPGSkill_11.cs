using System.Collections;
using UnityEngine;

public class RPGSkill_11 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[11];
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

	private IEnumerator DelayThrowChicken(RPGEntity healEntity, float fHp)
	{
		yield return new WaitForSeconds(0.5f);
		Vector3 pos = base.SkillOwner.transform.position;
		GameObject obj = Object.Instantiate(position: new Vector3(pos.x, 1.5f, pos.z), original: Resources.Load("Particle/effect/Skill/RPG_BUFF_chicken/RPG_BUFF_Chicken"), rotation: Quaternion.Euler(Vector3.zero)) as GameObject;
		RPGChickenFly fly = obj.GetComponent<RPGChickenFly>();
		float flyTime = fly.FlyToAim(healEntity, fHp, 1.5f);
	}

	public override float Skill_BeginBout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		if (base.SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Begin_Bout)
		{
			ActiveSkill();
		}
		RPGEntity heal = null;
		float fHp = 0f;
		if (t.HealingHPToAnyTeamMem(out heal, out fHp) != -1)
		{
			base.SkillOwner.PlayAni(RPGEntity.EAniLST.Buff);
			StartCoroutine(DelayThrowChicken(heal, fHp));
			state.DealyAttack(t, 2.2f);
			return 1f;
		}
		return 0f;
	}
}
