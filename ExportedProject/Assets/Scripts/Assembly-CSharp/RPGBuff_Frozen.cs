using UnityEngine;

public class RPGBuff_Frozen : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 14;
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
		return true;
	}

	protected override void InitBufferEffect()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/Ice/Ice_debuff")) as GameObject;
		gameObject.transform.parent = base.RPGEntityOwner.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.AddComponent<RPGMarkAimFollow>();
		AddSpecEffectObj(gameObject);
	}
}
