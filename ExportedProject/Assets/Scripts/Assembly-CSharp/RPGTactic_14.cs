using UnityEngine;

public class RPGTactic_14 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[14];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveSkill(38);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveSkill(38);
			return !flag;
		}
		return false;
	}

	public override float ActiveTactic()
	{
		Debug.Log("ActiveTactic:" + base.name);
		if (_team != null)
		{
			RPGTeam enemyTeam = _team.Refree.GetEnemyTeam(_team);
			for (int i = 0; i < enemyTeam.MemberLst.Count; i++)
			{
				if (enemyTeam.MemberLst[i].CurHp > 0f)
				{
					GameObject gameObject = enemyTeam.MemberLst[i].gameObject;
					Debug.Log("Generate Buff:" + base.name);
					RPGBuff_Devil rPGBuff_Devil = gameObject.AddComponent<RPGBuff_Devil>();
					rPGBuff_Devil._subtractHPRate = (int)base.TacticUnit.ParamLst[0];
					rPGBuff_Devil.SecondType = 122;
					rPGBuff_Devil.IsOverlap = base.TacticUnit.CanOverlap;
					if (enemyTeam.MemberLst[i].AddBuff(rPGBuff_Devil) != -1f)
					{
						_buffIdLst.Add(rPGBuff_Devil.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5013, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
