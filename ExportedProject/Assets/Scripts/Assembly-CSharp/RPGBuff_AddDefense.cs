using UnityEngine;

public class RPGBuff_AddDefense : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 2;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5001)
		{
			if (base.RPGEntityOwner != null)
			{
				base.RPGEntityOwner.RemoveBuff(GetInstanceID());
			}
			Debug.Log("Buff:ALL DEF ADD -------------Removed:" + base.Id);
		}
		return true;
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
		rPGRoleAttr.Attrs[5] = base.OffsetAttr.Attrs[5];
		return rPGRoleAttr;
	}
}
