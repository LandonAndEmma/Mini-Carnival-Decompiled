using UnityEngine;

public class RPGSkill_17 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[17];
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

	public override void Skill_InitAttackCount(RPGCenterController_Auto controller)
	{
		RPGEntity rPGEntity = controller.GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		RPGTSkill attackSkill = rPGEntity.GetAttackSkill();
		int min = (int)attackSkill.SkillUnit.ParamLst[0];
		int num = (int)attackSkill.SkillUnit.ParamLst[1];
		controller._attackCount = Random.Range(min, num + 1);
		Debug.LogWarning("AttackCount=" + controller._attackCount);
		controller._curAttackCount = 0;
	}

	public override void Skill_ProcessMultiAttack(RPGRole_PlayerState_Attack state_attack, RPGCenterController_Auto t)
	{
		state_attack.InitEnter(t, false);
	}
}
