using UnityEngine;

public class COMA_Fishing_PlayerState_Idle : TState<COMA_Fishing_PlayerController>
{
	private float fEnterTime;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.SetCanMove(true);
		cOMA_PlayerSelf_Fishing.EnableFishPole(false);
		fEnterTime = Time.time;
		if (t.bNeedOffBoat)
		{
			t.DestoryFishingLine();
			TMessageDispatcher.Instance.DispatchMsg(-1, t.GetOwner().GetInstanceID(), 1000000002, TTelegram.SEND_MSG_IMMEDIATELY, null);
			t.bNeedOffBoat = false;
		}
		Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@Enter   Idle!!!!!!!!!!!!!!!!!!!!!!!!!!");
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		t.HandleRefreshCharacPos();
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.SetCanMove(false);
		cOMA_PlayerSelf_Fishing.EnableFishPole(true);
	}

	public override bool OnMessage(COMA_Fishing_PlayerController t, TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 0:
		{
			result = true;
			Debug.Log("COMA_Fishing_PlayerState_Idle<OnMessage> Receive msg:" + msg._nMsgId);
			int reason = 0;
			if (t.IsFishingPoleValid(out reason))
			{
				t.ChangeState(COMA_Fishing_PlayerController.EState.CastPole);
				break;
			}
			t.DestoryFishPole();
			Debug.Log("===========? INVALID FISHING POLE!!!");
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(135);
			uI_MsgBox.AddProceYesHandler(ProceBuyGoldPole);
			break;
		}
		case 1013:
			TMessageDispatcher.Instance.DispatchMsg(-1, t.GetOwner().GetInstanceID(), 1000000002, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		}
		return result;
	}

	protected void ProceBuyGoldPole(string param)
	{
		if (COMA_Pref.Instance.GetCrystal() >= 25)
		{
			COMA_Pref.Instance.AddCrystal(-25);
			int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 3, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else
		{
			TUI_MsgBox.Instance.MessageBox(106);
		}
	}
}
