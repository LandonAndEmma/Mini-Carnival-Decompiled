using UnityEngine;

public class RPGRole_PlayerState_Select_Aims : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	public override void Enter(RPGCenterController_Auto t)
	{
		fEnterTime = Time.time;
		RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
		if (t.AutoFight || (!t.AutoFight && rPGEntity.IsExitBuff(221)) || (!t.AutoFight && rPGEntity.CareerUnit.CareerId == 16 && t._curAttackCount > 0))
		{
			if (t.SelectATTAim() > 0)
			{
				t.MarkAim();
				t.ChangeState(RPGCenterController_Auto.EState.Attack);
			}
			else
			{
				t.ChangeState(RPGCenterController_Auto.EState.End_Bout);
			}
		}
		else
		{
			t.ChangeState(RPGCenterController_Auto.EState.Select_Aims_Manual);
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
		int nMsgId = msg._nMsgId;
		if (nMsgId == 1013)
		{
			result = true;
		}
		return result;
	}
}
