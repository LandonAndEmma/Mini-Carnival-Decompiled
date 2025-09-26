using UnityEngine;

public class RPGSkill_4 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[4];
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

	public override void Skill_DodgeFeedBack(RPGEntity attack, RPGEntity beAttack)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		RPGCenterController_Auto rPGCenterController_Auto = attack.GetCenterController() as RPGCenterController_Auto;
		if (rPGCenterController_Auto.HasDodgeIgnoreChance())
		{
			rPGCenterController_Auto._attackCount++;
			rPGCenterController_Auto.DisableDodgeIgnoreChance();
			Debug.Log("-----------------------------------Dodge chance+1");
		}
	}

	public override void Skill_ProcessMultiAttack(RPGRole_PlayerState_Attack state_attack, RPGCenterController_Auto t)
	{
		state_attack.InitEnter(t, false);
	}
}
