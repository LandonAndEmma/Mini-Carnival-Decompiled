public class COMA_Fishing_PlayerState_Global : TState<COMA_Fishing_PlayerController>
{
	public override void Enter(COMA_Fishing_PlayerController t)
	{
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
	}

	public override bool OnMessage(COMA_Fishing_PlayerController t, TTelegram msg)
	{
		bool result = false;
		int nMsgId = msg._nMsgId;
		if (nMsgId == 6)
		{
			result = true;
			t.ReduceFishingPoleUseCount();
		}
		return result;
	}
}
