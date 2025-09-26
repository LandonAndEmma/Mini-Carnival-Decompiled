using UnityEngine;

public class RPGTactic_7 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[7];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(30);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(30);
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
				if (_team.MemberLst[i].CurHp > 0f)
				{
					GameObject gameObject = _team.MemberLst[i].gameObject;
					Debug.Log("Generate Buff:" + base.name);
					RPGBuff_King rPGBuff_King = gameObject.AddComponent<RPGBuff_King>();
					rPGBuff_King.OffsetAttr.Attrs[3] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[4] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[5] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[6] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[8] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[10] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.OffsetAttr.Attrs[7] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_King.SecondType = 119;
					rPGBuff_King.IsOverlap = base.TacticUnit.CanOverlap;
					if (_team.MemberLst[i].AddBuff(rPGBuff_King, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
					{
						_buffIdLst.Add(rPGBuff_King.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5006, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
