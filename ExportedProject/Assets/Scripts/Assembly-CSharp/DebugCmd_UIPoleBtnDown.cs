using UnityEngine;

public class DebugCmd_UIPoleBtnDown : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("uipolebtndown", this) == 0)
		{
			Debug.Log("------Register <uipolebtndown> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		TMessageDispatcher.Instance.DispatchMsg(-2, iDByName, 0, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}
}
