using UnityEngine;

public class RPGRole_PlayerState_Attack : TState<RPGCenterController_Auto>
{
	private float fEnterTime;

	private float fCloseTime;

	private bool attacking;

	private float fAttackAniLen;

	private float fAttackTime;

	private bool isMoved;

	private bool isBacking;

	private float fBackDurTime;

	private float fBackStartTime;

	private float fOffsetClose;

	private float fExtraATTStartTime = -100f;

	private float fExtraATTDurTime = 1f;

	private float fDelayEndingTime;

	private bool bDelayEnding;

	private bool _bIsOffset;

	public void InitEnter(RPGCenterController_Auto t, bool IsNeedApproach)
	{
		fOffsetClose = 0f;
		fBackStartTime = 0f;
		fBackDurTime = 0f;
		isMoved = false;
		isBacking = false;
		t._curAttackCount++;
		attacking = false;
		fEnterTime = Time.time;
		if (IsNeedApproach)
		{
			fCloseTime = t.JudgeIsNeedApproachAim();
			isMoved = ((fCloseTime > 0f) ? true : false);
		}
		else
		{
			if (!t.IsValidAim())
			{
				EndAttack(t);
			}
			fCloseTime = 0f;
		}
		RPGAimAtTarget componentInChildren = t.GetOwner().transform.GetComponentInChildren<RPGAimAtTarget>();
		if (componentInChildren != null && t.MainAim[0] != null)
		{
			fOffsetClose = componentInChildren.AimToTarget(t.MainAim[0].transform, 2f);
		}
	}

	private void EndAttack(RPGCenterController_Auto t)
	{
		attacking = true;
		isBacking = true;
		fBackStartTime = Time.time;
		fBackDurTime = t.JudgeIsNeedBack();
		RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
		if (rPGEntity.CareerUnit.CareerId == 2)
		{
			rPGEntity.transform.position = new Vector3(-1000f, 1000f, 1000f);
			t.ChangeState(RPGCenterController_Auto.EState.End_Bout);
		}
	}

	public override void Enter(RPGCenterController_Auto t)
	{
		InitEnter(t, true);
		fExtraATTStartTime = -100f;
		bDelayEnding = false;
	}

	public override void Update(RPGCenterController_Auto t)
	{
		if (attacking)
		{
			if (isBacking)
			{
				if (Time.time - fBackStartTime >= fBackDurTime)
				{
					t.ChangeState(RPGCenterController_Auto.EState.End_Bout);
				}
				else
				{
					t.ClosetoAim();
				}
			}
			else if (bDelayEnding)
			{
				if (Time.time - fDelayEndingTime >= fExtraATTDurTime)
				{
					RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
					rPGEntity.StopAni();
					EndAttack(t);
				}
			}
			else if (Time.time - fAttackTime >= fAttackAniLen)
			{
				if (t._curAttackCount >= t._attackCount)
				{
					if (t._extraATTCount > 0)
					{
						if (t._curExtraATTCount < t._extraATTCount && Time.time - fExtraATTStartTime >= fExtraATTDurTime)
						{
							fExtraATTStartTime = Time.time;
							t._curExtraATTCount++;
							Debug.Log("------------------------ATT ENDt._curExtraATTCount=" + t._curExtraATTCount + "  t._extraATTCount=" + t._extraATTCount);
							if (t.SelectATTAim() > 0)
							{
								t.MarkAim();
								t.RealAttack(2f);
								t.RealAttack(1f, 0.43f);
							}
							else
							{
								Debug.Log("------EndAttack");
								fDelayEndingTime = Time.time;
								bDelayEnding = true;
							}
						}
						else if (t._curExtraATTCount >= t._extraATTCount)
						{
							Debug.Log("------EndAttack0");
							fDelayEndingTime = Time.time;
							bDelayEnding = true;
						}
					}
					else
					{
						Debug.Log("------EndAttack1");
						if (_bIsOffset)
						{
							RPGEntity rPGEntity2 = t.GetOwner() as RPGEntity;
							rPGEntity2.StopAni();
							_bIsOffset = false;
						}
						EndAttack(t);
					}
				}
				else
				{
					Debug.Log("------------------------MultiAtt");
					RPGTSkill attackSkill = ((RPGEntity)t.GetOwner()).GetAttackSkill();
					attackSkill.Skill_ProcessMultiAttack(this, t);
				}
			}
			else
			{
				fExtraATTStartTime = Time.time;
			}
		}
		else if (fCloseTime > 0f)
		{
			if (Time.time - fEnterTime >= fCloseTime)
			{
				Action_AttackAni(t);
			}
			else
			{
				t.ClosetoAim();
			}
		}
		else if (Time.time - fEnterTime >= fOffsetClose)
		{
			Action_AttackAni(t);
		}
	}

	public override void Exit(RPGCenterController_Auto t)
	{
		RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
		rPGEntity.GetAttackSkill().Skill_AttackEffect_LaunchEnd(t);
		t.RealAttackEnd();
	}

	public override bool OnMessage(RPGCenterController_Auto t, TTelegram msg)
	{
		bool result = false;
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5018)
		{
			t.Time_MinEndBout = Time.time + (float)msg._pExtraInfo;
			result = true;
		}
		return result;
	}

	private void Action_AttackAni(RPGCenterController_Auto t)
	{
		Debug.LogWarning("---------------------Action_AttackAni");
		RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
		fAttackAniLen = rPGEntity.PlayAni(RPGEntity.EAniLST.Attack);
		AnimationState animationState = rPGEntity.Charac.animation[rPGEntity.LstAni[1].name];
		fAttackAniLen *= 1f / animationState.speed;
		_bIsOffset = rPGEntity.AttackTimeOffset(ref fAttackAniLen);
		attacking = true;
		fAttackTime = Time.time;
	}
}
