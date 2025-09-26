public class COMA_Fishing_PlayerState_Fishing : TState<COMA_Fishing_PlayerController>
{
	private bool bDrawFishingLine;

	private bool bProcessUIDown;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_idle", string.Empty, cOMA_PlayerSelf_Fishing.CurFishFloat.transform.position);
		t.DrawFishingLine();
		bDrawFishingLine = false;
		bProcessUIDown = false;
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (bDrawFishingLine)
		{
			t.DrawFishingLine_Straight();
		}
		if (t.bNeedOffBoat)
		{
			t.ChangeState(COMA_Fishing_PlayerController.EState.Idle);
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
		bDrawFishingLine = false;
	}

	public override bool OnMessage(COMA_Fishing_PlayerController t, TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 0:
		{
			if (bProcessUIDown)
			{
				break;
			}
			bProcessUIDown = true;
			result = true;
			int instanceID = ((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishFloat.GetInstanceID();
			TMessageDispatcher.Instance.DispatchMsg(-1, instanceID, 7, TTelegram.SEND_MSG_IMMEDIATELY, null);
			if (t.IsInShakeState())
			{
				if (t.IsNeedEnterPullState())
				{
					t.ChangeState(COMA_Fishing_PlayerController.EState.PullPole);
				}
				else
				{
					t.ChangeState(COMA_Fishing_PlayerController.EState.PullPole02);
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_WOW, t.GetOwner().transform);
				}
				TMessageDispatcher.Instance.DispatchMsg(-1, t.GetOwner().GetInstanceID(), 6, TTelegram.SEND_MSG_IMMEDIATELY, null);
			}
			else
			{
				t.DestoryFishingLine();
				t._fetchingNextState = COMA_Fishing_PlayerController.EState.CancelPole;
				t.nFetchParam = 0;
				t.ChangeState(COMA_Fishing_PlayerController.EState.Fetching);
			}
			t.CancelBuff_Shake();
			break;
		}
		case 13:
			result = true;
			t.DestoryFishingLine();
			t.ChangeState(COMA_Fishing_PlayerController.EState.Fetching);
			break;
		case 1013:
			result = true;
			t.DestoryFishingLine();
			t.ChangeState(COMA_Fishing_PlayerController.EState.Idle);
			TMessageDispatcher.Instance.DispatchMsg(-1, t.GetOwner().GetInstanceID(), 1000000002, TTelegram.SEND_MSG_IMMEDIATELY, null);
			break;
		}
		return result;
	}
}
