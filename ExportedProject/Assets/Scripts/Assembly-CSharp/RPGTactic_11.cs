using UnityEngine;

public class RPGTactic_11 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[11];
	}

	public override bool MatchAvtiveCondition()
	{
		return false;
	}

	public override void ConditionChanged()
	{
		if (MatchUnavtiveCondition())
		{
			UnactiveTactic();
		}
		else
		{
			ActiveTactic();
		}
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(39);
			return !flag;
		}
		return false;
	}

	public override float ActiveTactic()
	{
		Debug.LogWarning("-------------------------------------------------------ActiveTactic:" + base.name);
		if (_team != null)
		{
			for (int i = 0; i < _team.MemberLst.Count; i++)
			{
				if (_team.MemberLst[i].CurHp > 0f && (_team.MemberLst[i].CareerUnit.CareerId == 47 || _team.MemberLst[i].CareerUnit.CareerId == 518))
				{
					GameObject gameObject = _team.MemberLst[i].gameObject;
					Debug.Log("Generate Buff:" + base.name);
					RPGBuff_Pharoah rPGBuff_Pharoah = gameObject.AddComponent<RPGBuff_Pharoah>();
					rPGBuff_Pharoah.OffsetAttr.Attrs[3] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.OffsetAttr.Attrs[4] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.OffsetAttr.Attrs[5] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.OffsetAttr.Attrs[6] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.OffsetAttr.Attrs[8] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.OffsetAttr.Attrs[10] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Pharoah.SecondType = 123;
					rPGBuff_Pharoah.IsOverlap = true;
					if (_team.MemberLst[i].AddBuff(rPGBuff_Pharoah) != -1f)
					{
						GameObject original = Resources.Load("Particle/effect/Skill/RPG_Pharaoh/RPG_Pharaoh_Lu") as GameObject;
						Vector3 position = _team.GetCurDiedEntity().transform.position;
						GameObject gameObject2 = Object.Instantiate(original, position, Quaternion.Euler(Vector3.zero)) as GameObject;
						gameObject2.transform.rotation = Quaternion.Euler(Vector3.zero);
						RPGPharaohAbsorb rPGPharaohAbsorb = gameObject2.AddComponent<RPGPharaohAbsorb>();
						rPGPharaohAbsorb.FlyToAim(_team.MemberLst[i], 0f, 0.5f);
						_buffIdLst.Add(rPGBuff_Pharoah.GetInstanceID());
					}
				}
			}
		}
		return 0f;
	}

	public override int UnactiveTactic()
	{
		for (int i = 0; i < _buffIdLst.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5010, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
