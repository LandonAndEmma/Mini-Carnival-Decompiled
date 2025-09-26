public class RPGBuff_ImmuneDebuff : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 15;
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
		rPGRoleAttr.Attrs[16] = base.OffsetAttr.Attrs[16];
		rPGRoleAttr.Attrs[17] = base.OffsetAttr.Attrs[17];
		rPGRoleAttr.Attrs[18] = base.OffsetAttr.Attrs[18];
		rPGRoleAttr.Attrs[22] = base.OffsetAttr.Attrs[22];
		return rPGRoleAttr;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5012 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}
}
