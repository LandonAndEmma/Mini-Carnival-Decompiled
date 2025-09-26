public class RPGBuff_Painter : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 19;
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
		rPGRoleAttr.Attrs[10] = base.OffsetAttr.Attrs[10];
		rPGRoleAttr.Attrs[11] = base.OffsetAttr.Attrs[11];
		return rPGRoleAttr;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	private new void Update()
	{
		base.Update();
	}
}
