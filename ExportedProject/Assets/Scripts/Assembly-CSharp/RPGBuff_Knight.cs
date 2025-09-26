using UnityEngine;

public class RPGBuff_Knight : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 17;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5011)
		{
			if (base.RPGEntityOwner != null)
			{
				base.RPGEntityOwner.RemoveBuff(GetInstanceID());
			}
			Debug.Log("Buff:ALL CRI RATE ADD -------------Removed:" + base.Id);
		}
		return true;
	}
}
