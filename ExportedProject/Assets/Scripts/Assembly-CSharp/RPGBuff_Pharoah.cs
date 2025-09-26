public class RPGBuff_Pharoah : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 22;
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
		rPGRoleAttr.Attrs[6] = base.OffsetAttr.Attrs[6];
		rPGRoleAttr.Attrs[8] = base.OffsetAttr.Attrs[8];
		rPGRoleAttr.Attrs[10] = base.OffsetAttr.Attrs[10];
		return rPGRoleAttr;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5010 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}
}
