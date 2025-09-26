using UnityEngine;

public class DebugCmd_ChangeSeason : TBaseCommand
{
	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("changeseason", this) == 0)
		{
			Debug.Log("------Register <changeseason> Succeed!");
		}
	}

	public override void Execute(string[] args)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing != null && cOMA_PlayerSelf_Fishing.CurFishPole != null)
		{
			if (args == null || args[0] == string.Empty || args[0] == "1")
			{
				cOMA_PlayerSelf_Fishing.CurFishPole.SetCurMonth(1);
			}
			else if (args[0] == "2")
			{
				cOMA_PlayerSelf_Fishing.CurFishPole.SetCurMonth(4);
			}
			else if (args[0] == "3")
			{
				cOMA_PlayerSelf_Fishing.CurFishPole.SetCurMonth(7);
			}
			else if (args[0] == "4")
			{
				cOMA_PlayerSelf_Fishing.CurFishPole.SetCurMonth(10);
			}
		}
	}
}
