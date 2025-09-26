using UnityEngine;

public class RPGRole_PlayerState_None : TState<RPGCenterController_Auto>
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
		if (nMsgId == 5015)
		{
			result = true;
			TMessageDispatcher.Instance.DispatchMsg(-1, ((RPGEntity)t.GetOwner()).TeamOwner.Refree.GetInstanceID(), 5019, TTelegram.SEND_MSG_IMMEDIATELY, t.GetOwner());
		}
		return result;
	}
}
