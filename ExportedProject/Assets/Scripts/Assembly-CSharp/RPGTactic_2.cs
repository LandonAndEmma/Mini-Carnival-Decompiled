using RPGMODE;
using UnityEngine;

public class RPGTactic_2 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[2];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveCareer(ERPGCareer.ShieldWarrior);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveCareer(ERPGCareer.ShieldWarrior);
			return !flag;
		}
		return false;
	}

	public override float ActiveTactic()
	{
		Debug.Log("ActiveTactic:" + base.name);
		if (_team != null)
		{
			float fLen = 0f;
			for (int i = 0; i < _team.MemberLst.Count; i++)
			{
				GameObject gameObject = _team.MemberLst[i].gameObject;
				Debug.Log("Generate Buff:" + base.name);
				RPGBuff_AddDefense rPGBuff_AddDefense = gameObject.AddComponent<RPGBuff_AddDefense>();
				rPGBuff_AddDefense.OffsetAttr.Attrs[5] = (int)_tacticUnit.ParamLst[0];
				Debug.Log("Tactic-Farmers:Add DEF:" + (int)_tacticUnit.ParamLst[0]);
				rPGBuff_AddDefense.SecondType = 107;
				rPGBuff_AddDefense.IsOverlap = base.TacticUnit.CanOverlap;
				if (_team.MemberLst[i].AddBuff(rPGBuff_AddDefense, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
				{
					_buffIdLst.Add(rPGBuff_AddDefense.GetInstanceID());
				}
			}
			return fLen;
		}
		return 0f;
	}

	public override int UnactiveTactic()
	{
		for (int i = 0; i < _buffIdLst.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5001, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
