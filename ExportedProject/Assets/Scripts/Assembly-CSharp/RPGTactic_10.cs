using RPGMODE;
using UnityEngine;

public class RPGTactic_10 : RPGTTactic
{
	private new void Awake()
	{
		base.Awake();
		base.TacticUnit = RPGGlobalData.Instance.TacticUnitPool._dict[10];
	}

	public override bool MatchAvtiveCondition()
	{
		if (_team != null)
		{
			return _team.HasAliveCareer(ERPGCareer.FlowerFaerie);
		}
		return false;
	}

	public override bool MatchUnavtiveCondition()
	{
		if (_team != null)
		{
			bool flag = _team.HasAliveCareer(ERPGCareer.FlowerFaerie);
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
					RPGBuff_Flower rPGBuff_Flower = gameObject.AddComponent<RPGBuff_Flower>();
					rPGBuff_Flower.OffsetAttr.Attrs[6] = (int)_tacticUnit.ParamLst[0];
					rPGBuff_Flower.SecondType = 120;
					rPGBuff_Flower.IsOverlap = base.TacticUnit.CanOverlap;
					if (_team.MemberLst[i].AddBuff(rPGBuff_Flower, true, ref fLen, (!IsExistTacticOwner(_team.MemberLst[i])) ? null : _team.MemberLst[i]) != -1f)
					{
						_buffIdLst.Add(rPGBuff_Flower.GetInstanceID());
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
			TMessageDispatcher.Instance.DispatchMsg(-1, _buffIdLst[i], 5009, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		_buffIdLst.Clear();
		return 0;
	}
}
