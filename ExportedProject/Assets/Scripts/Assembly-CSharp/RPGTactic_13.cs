using UnityEngine;

public class RPGTactic_13 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[13];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(37);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(37);
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
					RPGBuff_ImmuneDebuff rPGBuff_ImmuneDebuff = gameObject.AddComponent<RPGBuff_ImmuneDebuff>();
					rPGBuff_ImmuneDebuff.OffsetAttr.Attrs[16] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_ImmuneDebuff.OffsetAttr.Attrs[17] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_ImmuneDebuff.OffsetAttr.Attrs[18] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_ImmuneDebuff.OffsetAttr.Attrs[22] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_ImmuneDebuff.SecondType = 121;
					rPGBuff_ImmuneDebuff.IsOverlap = base.TacticUnit.CanOverlap;
					if (_team.MemberLst[i].AddBuff(rPGBuff_ImmuneDebuff, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
					{
						_buffIdLst.Add(rPGBuff_ImmuneDebuff.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5012, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
