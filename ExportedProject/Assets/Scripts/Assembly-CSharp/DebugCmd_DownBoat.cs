using UnityEngine;

public class DebugCmd_DownBoat : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance)
		{
			string text = "downboat";
			if (TInput_DebugCommand.Instance.RegisterDebugCommand(text, this) == 0)
			{
				Debug.Log("------Register <" + text + "> Succeed!");
			}
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		Debug.Log("nMainID=" + iDByName);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1000000002, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}
}
