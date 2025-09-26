using UnityEngine;

public class RPGRole_PlayerState_Select_Aims_Manual : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	public override void Enter(RPGCenterController_Auto t)
	{
		fEnterTime = Time.time;
		if (!t.IsCanSelectATTAim())
		{
			t.ChangeState(RPGCenterController_Auto.EState.End_Bout);
		}
	}

	public override void Update(RPGCenterController_Auto t)
	{
	}

	public override void Exit(RPGCenterController_Auto t)
	{
	}

	public override bool OnMessage(RPGCenterController_Auto t, TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 5016:
		{
			result = true;
			RPGEntity rPGEntity = msg._pExtraInfo as RPGEntity;
			if (t.EnemyHasAlivePupil())
			{
				t.SelectATTAim();
			}
			else
			{
				t.MainAim.Clear();
				t.MainAim.Add(rPGEntity);
				RPGEntity rPGEntity2 = t.GetOwner() as RPGEntity;
				RPGTeam teamOwner = rPGEntity2.TeamOwner;
				RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
				RPGTSkill attackSkill = rPGEntity2.GetAttackSkill();
				attackSkill.Skill_SelectAttackAim_Manual(t);
			}
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/RPG_Double_sword/RPG_Double_sword")) as GameObject;
			gameObject.transform.parent = rPGEntity.transform;
			gameObject.transform.localPosition = Vector3.zero;
			Object.Destroy(gameObject, 2f);
			t.MarkAim();
			t.ChangeState(RPGCenterController_Auto.EState.Attack);
			break;
		}
		case 5023:
			result = true;
			t.ChangeState(RPGCenterController_Auto.EState.Select_Aims);
			break;
		}
		return result;
	}
}
