using UnityEngine;

public class COMA_Fishing_PlayerState_PullPole : TState<COMA_Fishing_PlayerController>
{
	private float fPullPoleTime;

	private float fEnterTime;

	private float fUnsuccessTime;

	private float fTickStart;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1001, TTelegram.SEND_MSG_IMMEDIATELY, null);
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_pullpole", string.Empty);
		fPullPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_pullpole"].length;
		cOMA_PlayerSelf_Fishing.CurFishPole._aniCmp.Play("pole_pull");
		fTickStart = Time.time;
		int instanceID = cOMA_PlayerSelf_Fishing.CurFishFloat.GetInstanceID();
		TMessageDispatcher.Instance.DispatchMsg(-1, instanceID, 12, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (Time.time - fTickStart >= 0.05f)
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
		switch (msg._nMsgId)
		{
		case 9:
		{
			result = true;
			int iDByName2 = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 1002, TTelegram.SEND_MSG_IMMEDIATELY, null);
			t._fetchingNextState = COMA_Fishing_PlayerController.EState.SuccessPole;
			t.nFetchParam = 0;
			t.ChangeState(COMA_Fishing_PlayerController.EState.Fetching);
			break;
		}
		case 10:
		{
			result = true;
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1002, TTelegram.SEND_MSG_IMMEDIATELY, null);
			t._fetchingNextState = COMA_Fishing_PlayerController.EState.CancelPole;
			t.nFetchParam = 0;
			t.ChangeState(COMA_Fishing_PlayerController.EState.Fetching);
			break;
		}
		case 11:
			result = true;
			t.ChangeState(COMA_Fishing_PlayerController.EState.Idle);
			break;
		case 1013:
			result = true;
			t.bNeedOffBoat = true;
			break;
		}
		return result;
	}
}
