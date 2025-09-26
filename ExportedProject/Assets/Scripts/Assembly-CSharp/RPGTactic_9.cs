using UnityEngine;

public class RPGTactic_9 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[9];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(24);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(24);
			return !flag;
		}
		return false;
	}

	public override float ActiveTactic()
	{
		Debug.Log("ActiveTactic:" + base.name);
		if (_team != null)
		{
			for (int i = 0; i < _team.MemberLst.Count; i++)
			{
				GameObject gameObject = _team.MemberLst[i].gameObject;
				Debug.Log("Generate Buff:" + base.name);
				RPGBuff_AddCriRate rPGBuff_AddCriRate = gameObject.AddComponent<RPGBuff_AddCriRate>();
				rPGBuff_AddCriRate.OffsetAttr.Attrs[8] = (int)_tacticUnit.ParamLst[0];
				Debug.Log("Tactic-Detective:Add Critical Rate:" + (int)_tacticUnit.ParamLst[0]);
				rPGBuff_AddCriRate.SecondType = 113;
				rPGBuff_AddCriRate.IsOverlap = base.TacticUnit.CanOverlap;
				if (_team.MemberLst[i].AddBuff(rPGBuff_AddCriRate) != -1f)
				{
					_buffIdLst.Add(rPGBuff_AddCriRate.GetInstanceID());
				}
			}
		}
		return 0f;
	}

	public override int UnactiveTactic()
	{
		for (int i = 0; i < _buffIdLst.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5008, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
