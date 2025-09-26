using UnityEngine;

public class RPGRole_PlayerState_Begin_Bout : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	private float fDelaySelAim;

	private bool bDelaySelAim;

	private Quaternion rot_ori;

	private float fDelayDur = 0.5f;

	public float DealyAttack(RPGCenterController_Auto t, RPGEntity healEntity)
	{
		fDelaySelAim = Time.time;
		bDelaySelAim = true;
		rot_ori = t.GetOwner().transform.rotation;
		t.GetOwner().transform.LookAt(healEntity.transform.position);
		return fDelaySelAim;
	}

	public float DealyAttack(RPGCenterController_Auto t, float fDealy)
	{
		fDelaySelAim = Time.time;
		bDelaySelAim = true;
		rot_ori = t.GetOwner().transform.rotation;
		fDelayDur = fDealy;
		return fDealy;
	}

	public override void Enter(RPGCenterController_Auto t)
	{
		if (t._signSelfObj == null)
		{
			RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
			if (rPGEntity.CareerUnit.CareerId == 40)
			{
				t._signSelfObj = Object.Instantiate(Resources.Load("FBX/Player/Character/RPG/RPG_Sign_Attack_Tank")) as GameObject;
			}
			else
			{
				t._signSelfObj = Object.Instantiate(Resources.Load("FBX/Player/Character/RPG/RPG_Sign_Attack")) as GameObject;
			}
		}
		t._signSelfObj.transform.parent = t.GetOwner().transform;
		t._signSelfObj.transform.localPosition = Vector3.zero;
		bDelaySelAim = false;
		fDelaySelAim = 0f;
		fEnterTime = Time.time;
		float num = t.NotifyBuffsBeginBount(this);
		t.InitSkillAttackCount();
		int num2 = ((RPGEntity)t.GetOwner()).IsCanBeginBout();
		if (num2 > 0)
		{
			bool flag = false;
			RPGEntity rPGEntity2 = (RPGEntity)t.GetOwner();
			for (int i = 0; i < rPGEntity2.GetSkillCount(); i++)
			{
				if (rPGEntity2.GetSkillByIndex(i).Skill_BeginBout(this, t) == 0f)
				{
					flag = true;
				}
			}
			if (flag && num == 0f)
			{
				t.ChangeState(RPGCenterController_Auto.EState.Select_Aims);
			}
		}
		else
		{
			if (num2 == 0)
			{
				RPGEntity rPGEntity3 = (RPGEntity)t.GetOwner();
				rPGEntity3.TriggerBuffer_Limit();
			}
			t.ChangeState(RPGCenterController_Auto.EState.End_Bout);
		}
	}

	public override void Update(RPGCenterController_Auto t)
	{
		if (bDelaySelAim && Time.time - fDelaySelAim >= fDelayDur)
		{
			t.GetOwner().transform.rotation = rot_ori;
			t.ChangeState(RPGCenterController_Auto.EState.Select_Aims);
			bDelaySelAim = false;
		}
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
