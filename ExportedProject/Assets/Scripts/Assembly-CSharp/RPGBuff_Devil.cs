using UnityEngine;

public class RPGBuff_Devil : RPGTBuff
{
	public int _subtractHPRate;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 9;
	}

	public override float NotifyBuffBegin_Bout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		base.NotifyBuffBegin_Bout(state, t);
		int num = Mathf.FloorToInt((float)_subtractHPRate / 100f * base.RPGEntityOwner.MaxHp);
		base.RPGEntityOwner.CurHp -= num;
		SSHurtNum.Instance.HitNormalFont(num, base.RPGEntityOwner.transform);
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/RPG_Skull/RPG_Skull_Lu")) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Object.Destroy(gameObject, 1f);
		state.DealyAttack(t, 1f);
		return 1f;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5013 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}
}
