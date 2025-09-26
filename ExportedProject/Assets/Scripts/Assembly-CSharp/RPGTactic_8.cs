using UnityEngine;

public class RPGTactic_8 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[8];
		base.PriorityLevel = 1000;
		base.Spec = true;
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(46);
		}
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

	public override void ConditionChanged_Enemy()
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
			bool flag = _team.HasAliveSkill(46);
			return !flag;
		}
		return false;
	}

	private float Rock(RPGTeam team)
	{
		float fLen = 0f;
		for (int i = 0; i < team.MemberLst.Count; i++)
		{
			if (!(team.MemberLst[i].CurHp > 0f))
			{
				continue;
			}
			GameObject gameObject = team.MemberLst[i].gameObject;
			bool flag = false;
			int num = Random.Range(0, 100);
			flag = ((num > (int)team.MemberLst[i].CalcAttr.Attrs[18]) ? true : false);
			Debug.Log("-----------------Chaos=" + flag + "  nR=" + num);
			if (flag)
			{
				Debug.Log("Generate Buff:" + base.name);
				RPGBuff_Rock rPGBuff_Rock = gameObject.AddComponent<RPGBuff_Rock>();
				rPGBuff_Rock.SecondType = 221;
				rPGBuff_Rock.IsOverlap = base.TacticUnit.CanOverlap;
				Debug.Log(i);
				Debug.Log(team.MemberLst.Count);
				if (team.MemberLst[i].AddBuff(rPGBuff_Rock, true, ref fLen, (!IsExistTacticOwner(team.MemberLst[i])) ? null : team.MemberLst[i]) != -1f)
				{
					_buffIdLst.Add(rPGBuff_Rock.GetInstanceID());
				}
			}
		}
		return fLen;
	}

	public override float ActiveTactic()
	{
		Debug.Log("ActiveTactic:" + base.name);
		if (_team != null)
		{
			RPGTeam enemyTeam = _team.Refree.GetEnemyTeam(_team);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_BUFF_Rock_Fly);
			Rock(enemyTeam);
			return Rock(_team);
		}
		return 0f;
	}

	public override int UnactiveTactic()
	{
		for (int i = 0; i < _buffIdLst.Count; i++)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5007, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
