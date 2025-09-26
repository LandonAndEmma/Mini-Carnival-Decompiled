using UnityEngine;

public class DebugCmd_FloatNotEnterWater : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("floatnotenterwater", this) == 0)
		{
			Debug.Log("------Register <floatnotenterwater> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 5, TTelegram.SEND_MSG_IMMEDIATELY, Vector3.zero);
	}
}
