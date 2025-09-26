using Protocol;
using UnityEngine;

public class COMA_Fishing_PlayerState_ShowItem : TState<COMA_Fishing_PlayerController>
{
	public override void Enter(COMA_Fishing_PlayerController t)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
		Debug.Log("=======>Notify UISYSTEM UI_GAIN_ITEM");
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1000, TTelegram.SEND_MSG_IMMEDIATELY, ((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishItem);
		Debug.Log("=======>Notify End!!!!!! UISYSTEM UI_GAIN_ITEM");
		if (((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishItem != null)
		{
			int num = -1;
			int num2 = 0;
			switch ((COMA_Fishing_FishableObj.EFishableType)((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishItem.GetEntityType())
			{
			case COMA_Fishing_FishableObj.EFishableType.Fish:
			{
				num = ((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishItem.GetCustomID();
				num2 = ((COMA_Fishing_Fish)((COMA_PlayerSelf_Fishing)t.GetOwner()).CurFishItem).GetWeight();
				int num3 = 0;
				if (UIDataBufferCenter.Instance.GetRank_World("ComAvatar007") != null)
				{
					int num4 = UIDataBufferCenter.Instance.GetRank_World("ComAvatar007").m_list.Count - 1;
					if (num4 >= 0)
					{
						RankItem rankItem = UIDataBufferCenter.Instance.GetRank_World("ComAvatar007").m_list[num4];
						num3 = (int)rankItem.m_score;
					}
				}
				if (num2 > num3)
				{
					((COMA_PlayerSelf_Fishing)t.GetOwner()).NotifyOtherPlayerFishingInfo(num, num2, COMA_CommonOperation.Instance.GIDFormat(COMA_Server_ID.Instance.GID));
					COtherPlayerInfo cOtherPlayerInfo = new COtherPlayerInfo(num, COMA_PlayerSelf.Instance.nickname);
					cOtherPlayerInfo._weight = num2;
					cOtherPlayerInfo._playerId = COMA_CommonOperation.Instance.GIDFormat(COMA_Server_ID.Instance.GID);
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1004, TTelegram.SEND_MSG_IMMEDIATELY, cOtherPlayerInfo);
				}
				break;
			}
			case COMA_Fishing_FishableObj.EFishableType.Chest:
				num = 10001;
				break;
			}
		}
		nCurFrame = 0;
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		nCurFrame++;
		if (nCurFrame >= 2)
		{
			Debug.Log("   =======>Test   From ShowItem to Idle!!!!!");
			t.ChangeState(COMA_Fishing_PlayerController.EState.Idle);
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
		t.ClearCurFishItem();
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
