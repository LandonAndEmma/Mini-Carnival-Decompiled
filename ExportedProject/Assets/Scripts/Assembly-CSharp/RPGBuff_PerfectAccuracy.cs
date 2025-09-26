public class RPGBuff_PerfectAccuracy : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 21;
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
		rPGRoleAttr.Attrs[7] = base.OffsetAttr.Attrs[7];
		return rPGRoleAttr;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}
}
