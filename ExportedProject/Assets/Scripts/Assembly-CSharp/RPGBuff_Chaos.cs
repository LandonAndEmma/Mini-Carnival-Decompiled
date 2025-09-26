using UnityEngine;

public class RPGBuff_Chaos : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 8;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	protected override void InitBufferEffect()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/Chaos/Chaos_Debuff")) as GameObject;
		gameObject.transform.parent = base.RPGEntityOwner.transform;
		gameObject.transform.localPosition = Vector3.zero;
		AddSpecEffectObj(gameObject);
	}
}
