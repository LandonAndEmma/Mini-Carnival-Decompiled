using UnityEngine;

public class DebugCmd_GetFishPole : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("getfishpole", this) == 0)
		{
			Debug.Log("------Register <getfishpole> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		if (args != null)
		{
			Debug.Log("DebugCmd_GetFishPole-Execute  args:" + args[0] + "  " + args.Length);
		}
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		Debug.Log("nMainID=" + iDByName);
		if (args == null || args[0] == string.Empty || args[0] == "black")
		{
			Debug.Log("Get Black FishPole");
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else if (args[0] == "silver")
		{
			Debug.Log("Get Silver FishPole");
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 2, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else if (args[0] == "gold")
		{
			Debug.Log("Get Gold FishPole");
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 3, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
	}
}
