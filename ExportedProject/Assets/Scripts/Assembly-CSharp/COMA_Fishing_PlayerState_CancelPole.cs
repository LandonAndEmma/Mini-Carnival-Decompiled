using UnityEngine;

public class COMA_Fishing_PlayerState_CancelPole : TState<COMA_Fishing_PlayerController>
{
	private float fUnSuccessPoleTime;

	private float fEnterTime;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_unsuccess", string.Empty);
		fUnSuccessPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_unsuccess"].length;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_Draw_Empty, t.GetOwner().transform);
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (Time.time - fEnterTime >= fUnSuccessPoleTime)
		{
			t.ChangeState(COMA_Fishing_PlayerController.EState.Idle);
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
