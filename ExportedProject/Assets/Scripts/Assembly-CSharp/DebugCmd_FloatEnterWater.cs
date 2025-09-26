using UnityEngine;

public class DebugCmd_FloatEnterWater : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("floatenterwater", this) == 0)
		{
			Debug.Log("------Register <floatenterwater> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 4, TTelegram.SEND_MSG_IMMEDIATELY, Vector3.zero);
	}
}
