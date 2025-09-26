using UnityEngine;

public class DebugCmd_DirectGetFishItem : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("directgetfishitem", this) == 0)
		{
			Debug.Log("------Register <directgetfishitem> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1000000000, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}
}
