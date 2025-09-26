using UnityEngine;

public class RPGRole_PlayerState_Death : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	private float fSpeed = 10f;

	private float v0;

	private float fAcc = 20f;

	private Transform deathTrans;

	private Vector3 dir;

	private float fDur;

	public override void Enter(RPGCenterController_Auto t)
	{
		fEnterTime = Time.time;
		((RPGEntity)t.GetOwner()).PlayAni(RPGEntity.EAniLST.Death);
		deathTrans = ((RPGEntity)t.GetOwner()).TeamOwner.GetRandomDeathPos();
		dir = deathTrans.position - t.GetOwner().transform.position;
		dir.Normalize();
		float num = Vector3.Distance(deathTrans.position, t.GetOwner().transform.position);
		fDur = num / fSpeed;
		t.InitDeathSpeed(fSpeed);
	}

	public override void Update(RPGCenterController_Auto t)
	{
		if (Time.time - fEnterTime <= fDur)
		{
			t.CloseToDeathPos(dir, fSpeed, fAcc);
		}
		else
		{
			t.ChangeState(RPGCenterController_Auto.EState.None);
		}
	}

	public override void Exit(RPGCenterController_Auto t)
	{
		((RPGEntity)t.GetOwner()).StopAni();
		Object.Destroy(t._signSelfObj);
		((RPGEntity)t.GetOwner()).transform.position = deathTrans.position;
		t._signSelfObj = null;
		TMessageDispatcher.Instance.DispatchMsg(-1, ((RPGEntity)t.GetOwner()).TeamOwner.Refree.GetInstanceID(), 5019, TTelegram.SEND_MSG_IMMEDIATELY, t.GetOwner());
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
