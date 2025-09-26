using UnityEngine;

public class COMA_Fishing_PlayerState_ReceivePole : TState<COMA_Fishing_PlayerController>
{
	private float fSuccessTime;

	private float fEnterTime;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_success", string.Empty);
		fSuccessTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_success"].length;
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (Time.time - fEnterTime >= fSuccessTime)
		{
			t.ChangeState(COMA_Fishing_PlayerController.EState.ShowItem);
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
	}

	public override bool OnMessage(COMA_Fishing_PlayerController t, TTelegram msg)
	{
		bool result = false;
		int nMsgId = msg._nMsgId;
		if (nMsgId == 1013)
		{
			result = true;
			t.bNeedOffBoat = true;
		}
		return result;
	}
}
