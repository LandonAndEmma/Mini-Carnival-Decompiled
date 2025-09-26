using UnityEngine;

public class RPGBuff_FarmerPromoteATT : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 11;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5000)
		{
			if (base.RPGEntityOwner != null)
			{
				base.RPGEntityOwner.RemoveBuff(GetInstanceID());
			}
			Debug.Log("Buff:Farmer Promote ATT -------------Removed:" + base.Id);
		}
		return true;
	}
}
