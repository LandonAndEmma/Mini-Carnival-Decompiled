using UnityEngine;

public class RPGTactic_12 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[12];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(20);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(20);
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
				RPGBuff_Knight rPGBuff_Knight = gameObject.AddComponent<RPGBuff_Knight>();
				rPGBuff_Knight.SecondType = 114;
				rPGBuff_Knight.IsOverlap = base.TacticUnit.CanOverlap;
				if (_team.MemberLst[i].AddBuff(rPGBuff_Knight, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
				{
					_buffIdLst.Add(rPGBuff_Knight.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5011, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
