using UnityEngine;

public class RPGTactic_1 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[1];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			int aliveFarmerCount = _team.GetAliveFarmerCount();
			Debug.Log("Team Alive Farmer Count=" + aliveFarmerCount);
			if (aliveFarmerCount > (int)_tacticUnit.ParamLst[0])
			{
				return true;
			}
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null && _team.GetAliveFarmerCount() <= (int)_tacticUnit.ParamLst[0])
		{
			return true;
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
				if (_team.MemberLst[i].CareerUnit.CareerId == 1)
				{
					GameObject gameObject = _team.MemberLst[i].gameObject;
					Debug.Log("Generate Buff:" + base.name);
					RPGBuff_FarmerPromoteATT rPGBuff_FarmerPromoteATT = gameObject.AddComponent<RPGBuff_FarmerPromoteATT>();
					rPGBuff_FarmerPromoteATT.OffsetAttr.Attrs[4] = (int)_tacticUnit.ParamLst[1];
					Debug.Log("Tactic-Farmers:Add ATT:" + rPGBuff_FarmerPromoteATT.OffsetAttr.Attrs[4]);
					rPGBuff_FarmerPromoteATT.SecondType = 101;
					rPGBuff_FarmerPromoteATT.IsOverlap = false;
					if (_team.MemberLst[i].AddBuff(rPGBuff_FarmerPromoteATT, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
					{
						_buffIdLst.Add(rPGBuff_FarmerPromoteATT.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5000, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
