using UnityEngine;

public class TFishingPlayerBuffController : TBaseEntityBuffController
{
	public TFishingPlayerBuffController(TBaseEntity own)
		: base(own)
	{
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		if ((msg._nMsgId > 10000 && msg._nMsgId < 10200) || msg._nMsgId > 999999999)
		{
			return true;
		}
		return false;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (!base.HandleMessage(msg))
		{
			return false;
		}
		switch (msg._nMsgId)
		{
		case 10001:
		{
			Debug.Log("TFishingPlayerBuffController--->GetBuff:" + ((TBuff)msg._pExtraInfo).GetType());
			AddBuff((TBuff)msg._pExtraInfo);
			COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = (COMA_PlayerSelf_Fishing)GetOwner();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fish_Bite, cOMA_PlayerSelf_Fishing.CurFishFloat.transform);
			break;
		}
		case 10002:
			Debug.Log("TFishingPlayerBuffController--->RemoveBuff:" + ((TBuff)msg._pExtraInfo).GetType());
			RemoveBuff(((TBuff)msg._pExtraInfo).GetID());
			break;
		case 1000000001:
		{
			TBuff tBuff = new TBuff();
			tBuff.SetBuffType(1);
			AddBuff(tBuff);
			Debug.Log("Set buff: OnBoat!");
			break;
		}
		case 1000000002:
			RemoveBuffByType(1);
			Debug.Log("Remove buff: OnBoat!");
			break;
		}
		return true;
	}

	public override void Tick()
	{
	}

	public override void FixedTick()
	{
	}
}
