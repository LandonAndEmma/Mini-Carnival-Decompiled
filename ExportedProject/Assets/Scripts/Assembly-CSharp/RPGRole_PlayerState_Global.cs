using UnityEngine;

public class RPGRole_PlayerState_Global : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	public override void Enter(RPGCenterController_Auto t)
	{
		fEnterTime = Time.time;
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
