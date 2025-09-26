using UnityEngine;

public class DebugCmd_ClearFishingTimeLimit : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("clearfishingtimelimit", this) == 0)
		{
			Debug.Log("------Register <clearfishingtimelimit> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		COMA_Server_Account.Instance.ClearSrvtime();
		COMA_Fishing.Instance.SetPoleTime(0);
		COMA_Fishing.Instance.SetPoleTime(1);
		COMA_Server_Account.Instance.RecoverSrvtime();
	}
}
