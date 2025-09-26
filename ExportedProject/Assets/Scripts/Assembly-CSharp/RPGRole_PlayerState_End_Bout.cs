using UnityEngine;

public class RPGRole_PlayerState_End_Bout : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	public override void Enter(RPGCenterController_Auto t)
	{
		fEnterTime = Time.time;
		t.ClearAimTeamArearMark();
		t.NotifyBuffsEndBount();
		((RPGEntity)t.GetOwner()).CurChildBoutCount++;
	}

	public override void Update(RPGCenterController_Auto t)
	{
		if (Time.time < t.Time_MinEndBout)
		{
			return;
		}
		t.Time_MinEndBout = 0f;
		if (Time.time - fEnterTime > 0.1f)
		{
			if ((float)((RPGEntity)t.GetOwner()).CurChildBoutCount >= ((RPGEntity)t.GetOwner()).CalcAttr.Attrs[12])
			{
				t.ChangeState(RPGCenterController_Auto.EState.Idle);
			}
			else
			{
				t.ChangeState(RPGCenterController_Auto.EState.Begin_Bout);
			}
		}
	}

	public override void Exit(RPGCenterController_Auto t)
	{
		if ((float)((RPGEntity)t.GetOwner()).CurChildBoutCount >= ((RPGEntity)t.GetOwner()).CalcAttr.Attrs[12])
		{
			RPGAimAtTarget componentInChildren = t.GetOwner().transform.GetComponentInChildren<RPGAimAtTarget>();
			if (componentInChildren != null)
			{
				componentInChildren.EndTarget(2f);
			}
			Object.Destroy(t._signSelfObj);
			t._signSelfObj = null;
			TMessageDispatcher.Instance.DispatchMsg(-1, ((RPGEntity)t.GetOwner()).TeamOwner.Refree.GetInstanceID(), 5019, TTelegram.SEND_MSG_IMMEDIATELY, t.GetOwner());
		}
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
