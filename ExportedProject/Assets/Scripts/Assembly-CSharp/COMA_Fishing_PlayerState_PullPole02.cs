using UnityEngine;

public class COMA_Fishing_PlayerState_PullPole02 : TState<COMA_Fishing_PlayerController>
{
	private float fPullPoleTime;

	private float fEnterTime;

	private float fUnsuccessTime;

	private float fTickStart;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_pullpole02", string.Empty);
		fPullPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_pullpole02"].length;
		cOMA_PlayerSelf_Fishing.CurFishPole._aniCmp.Play("pole_pull02");
		fTickStart = Time.time;
		int instanceID = cOMA_PlayerSelf_Fishing.CurFishFloat.GetInstanceID();
		TMessageDispatcher.Instance.DispatchMsg(-1, instanceID, 12, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (Time.time - fEnterTime >= fPullPoleTime)
		{
			t._fetchingNextState = COMA_Fishing_PlayerController.EState.SuccessPole;
			t.nFetchParam = 0;
			t.ChangeState(COMA_Fishing_PlayerController.EState.Fetching);
		}
		else if (Time.time - fTickStart >= 0.05f)
		{
			t.DrawFishingLine_Straight();
			fTickStart = Time.time;
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
		t.DestoryFishingLine();
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
