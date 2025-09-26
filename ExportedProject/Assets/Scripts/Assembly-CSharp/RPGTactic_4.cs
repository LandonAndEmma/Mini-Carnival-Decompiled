using RPGMODE;
using UnityEngine;

public class RPGTactic_4 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[4];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveCareer(ERPGCareer.TigerWarrior) && _team.HasAliveCareer(ERPGCareer.PandaWarrior);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveCareer(ERPGCareer.TigerWarrior) && _team.HasAliveCareer(ERPGCareer.PandaWarrior);
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
				if (_team.MemberLst[i].CareerUnit.CareerId == 23 && _team.MemberLst[i].CurHp > 0f)
				{
					GameObject gameObject = _team.MemberLst[i].gameObject;
					Debug.Log("Generate Buff:" + base.name);
					RPGBuff_Lion rPGBuff_Lion = gameObject.AddComponent<RPGBuff_Lion>();
					rPGBuff_Lion.OffsetAttr.Attrs[4] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Lion.SecondType = 116;
					rPGBuff_Lion.IsOverlap = base.TacticUnit.CanOverlap;
					if (_team.MemberLst[i].AddBuff(rPGBuff_Lion, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
					{
						_buffIdLst.Add(rPGBuff_Lion.GetInstanceID());
					}
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5003, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
