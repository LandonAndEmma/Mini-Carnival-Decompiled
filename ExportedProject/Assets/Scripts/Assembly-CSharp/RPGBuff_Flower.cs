using UnityEngine;

public class RPGBuff_Flower : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 13;
	}

	public override RPGRoleAttr GetOffsetResult(RPGRoleAttr baseAttr)
	{
		RPGRoleAttr rPGRoleAttr = new RPGRoleAttr();
		rPGRoleAttr.Attrs[0] = 0f;
		rPGRoleAttr.Attrs[1] = 0f;
		rPGRoleAttr.Attrs[2] = 0f;
		for (int i = 3; i < 24; i++)
		{
			rPGRoleAttr.Attrs[i] = baseAttr.Attrs[i] * (base.OffsetAttr.Attrs[i] / 100f);
		}
		rPGRoleAttr.Attrs[6] = base.OffsetAttr.Attrs[6];
		return rPGRoleAttr;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5009 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}

	protected override void InitBufferEffect()
	{
		if (!RPGGlobalData.Instance.BuffEffectPool._dict.ContainsKey(base.ConfId))
		{
			return;
		}
		RPGBuffEffectUnit rPGBuffEffectUnit = RPGGlobalData.Instance.BuffEffectPool._dict[base.ConfId];
		Object obj = Resources.Load(rPGBuffEffectUnit.EffectPath);
		if (obj != null)
		{
			GameObject gameObject = Object.Instantiate(obj) as GameObject;
			gameObject.transform.parent = base.RPGEntityOwner.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			AddSpecBuffEffectObj(gameObject);
		}
		Object obj2 = Resources.Load(rPGBuffEffectUnit.EffectPath_cast);
		if (!(obj2 != null) || base.RPGEntityOwner.CareerUnit.CareerId != 35)
		{
			return;
		}
		for (int i = 0; i < base.RPGEntityOwner.TeamOwner.MemberLst.Count; i++)
		{
			if (base.RPGEntityOwner.TeamOwner.MemberLst[i] != base.RPGEntityOwner)
			{
				GameObject gameObject2 = Object.Instantiate(obj2, base.RPGEntityOwner.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject2.transform.parent = base.RPGEntityOwner.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
				RPGBuffEffectFlyToTeam rPGBuffEffectFlyToTeam = gameObject2.AddComponent<RPGBuffEffectFlyToTeam>();
				rPGBuffEffectFlyToTeam.StartFly(base.RPGEntityOwner.TeamOwner.MemberLst[i].transform, 1f, 0f, rPGBuffEffectUnit.EffectPath_receive);
			}
		}
	}
}
