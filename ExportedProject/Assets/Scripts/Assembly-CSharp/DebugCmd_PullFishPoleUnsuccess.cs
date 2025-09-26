using UnityEngine;

public class DebugCmd_PullFishPoleUnsuccess : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("pullfishpoleunsuccess", this) == 0)
		{
			Debug.Log("------Register <pullfishpoleunsuccess> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		if (args == null || args[0] == string.Empty || args[0] == "bob")
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 10, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
	}
}
