using UnityEngine;

public class RPGRole_PlayerState_Idle : TState<RPGCenterController_Auto>
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
		float num = 0f;
		if (msg._pExtraInfo2 != null)
		{
			num = (float)msg._pExtraInfo2;
		}
		switch (msg._nMsgId)
		{
		case 5015:
			result = true;
			((RPGEntity)t.GetOwner()).CurChildBoutCount = 0;
			((RPGEntity)t.GetOwner()).CurBattleBoutIndex++;
			t.ChangeState(RPGCenterController_Auto.EState.Begin_Bout);
			break;
		case 5017:
		{
			result = true;
			RPGEntity rPGEntity = t.GetOwner() as RPGEntity;
			RPGEntity rPGEntity2 = msg._pExtraInfo as RPGEntity;
			rPGEntity.SetEnemy(rPGEntity2);
			if (num == 2f)
			{
				rPGEntity2.GetAttackSkill().Skill_BeAttackEffect(rPGEntity2, rPGEntity, 0);
				return true;
			}
			RPGRoleAttr calcAttr = rPGEntity.CalcAttr;
			RPGRoleAttr calcAttr2 = rPGEntity2.CalcAttr;
			bool flag = false;
			float value = calcAttr.Attrs[6];
			value = Mathf.Clamp(value, 0f, 70f);
			float f = calcAttr2.Attrs[7] - value;
			if (Mathf.FloorToInt(f) > 99)
			{
				flag = true;
			}
			else
			{
				int num2 = Random.Range(0, 100);
				flag = ((Mathf.FloorToInt(f) > num2) ? true : false);
			}
			rPGEntity2.GetAttackSkill().Skill_IsAT(ref flag, rPGEntity2, rPGEntity);
			int num3 = 0;
			if (flag)
			{
				int num4 = Random.Range(0, 100);
				float value2 = calcAttr2.Attrs[8];
				value2 = Mathf.Clamp(value2, 0f, 90f);
				float num5 = calcAttr.Attrs[5];
				num5 *= 1f - calcAttr2.Attrs[23];
				float value3 = num5 / 100f;
				value3 = Mathf.Clamp(value3, 0f, 0.7f);
				float f2 = calcAttr2.Attrs[4] * (1f - value3);
				int dam = Mathf.FloorToInt(f2);
				dam = rPGEntity2.GetAttackSkill().Skill_DAM(rPGEntity2, rPGEntity, dam);
				if (num == 0f)
				{
					rPGEntity2.GetAttackSkill().Skill_BeAttackEffect(rPGEntity2, rPGEntity, dam);
				}
				float num6 = Random.Range(-0.05f, 0.05f);
				if (value2 > (float)num4)
				{
					int num7 = Mathf.FloorToInt((float)dam * calcAttr2.Attrs[9]);
					num3 = Mathf.FloorToInt((float)num7 * (1f + calcAttr.Attrs[10] / 100f));
					num3 = rPGEntity2.GetAttackSkill().Skill_DAM_Cri(rPGEntity2, rPGEntity, num3);
					num3 = rPGEntity2.GetAttackSkill().Skill_ProcessShareDAM(rPGEntity2, rPGEntity, num3);
					num3 = Mathf.FloorToInt((float)num3 * (1f + num6));
					if (num3 <= 0)
					{
						num3 = 1;
					}
					int num8 = num3;
					if ((double)num8 >= (double)RPGTSkill.MAX_DAM * 0.94)
					{
						num8 = Mathf.CeilToInt(rPGEntity2.CurHp);
					}
					SSHurtNum.Instance.HitCriticalFont(num8, rPGEntity.transform);
					Debug.Log("OffsetCriticalDama:" + calcAttr.Attrs[10]);
					Debug.Log("---------------Critical : " + num3);
				}
				else
				{
					num3 = Mathf.FloorToInt((float)dam * (1f + calcAttr.Attrs[11] / 100f));
					num3 = rPGEntity2.GetAttackSkill().Skill_ProcessShareDAM(rPGEntity2, rPGEntity, num3);
					num3 = Mathf.FloorToInt((float)num3 * (1f + num6));
					if (num3 <= 0)
					{
						num3 = 1;
					}
					int num9 = num3;
					if ((double)num9 >= (double)RPGTSkill.MAX_DAM * 0.94)
					{
						num9 = Mathf.CeilToInt(rPGEntity2.CurHp);
					}
					SSHurtNum.Instance.HitNormalFont(num9, rPGEntity.transform);
				}
				float num10 = rPGEntity.PlayAni(RPGEntity.EAniLST.Beattack);
				TMessageDispatcher.Instance.DispatchMsg(rPGEntity.GetInstanceID(), rPGEntity2.GetInstanceID(), 5018, TTelegram.SEND_MSG_IMMEDIATELY, num10);
				rPGEntity.CurHp -= num3;
			}
			else
			{
				SSHurtNum.Instance.HitMiss(rPGEntity.transform);
				rPGEntity2.GetAttackSkill().Skill_DodgeFeedBack(rPGEntity2, rPGEntity);
			}
			rPGEntity2.GetAttackSkill().Skill_Feedback(rPGEntity2, rPGEntity, num3);
			rPGEntity2.TriggerBuffer_Attack(rPGEntity2, num3);
			break;
		}
		}
		return result;
	}
}
