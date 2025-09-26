using UnityEngine;

public class COMA_Fishing_PlayerState_CastPole : TState<COMA_Fishing_PlayerController>
{
	private float fEnterTime;

	private bool bCalFishFloat;

	private float fCastPoleTime;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		bCalFishFloat = false;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_castpole", string.Empty);
		fCastPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_castpole"].length;
		cOMA_PlayerSelf_Fishing.CurFishPole._aniCmp.Play("pole_cast");
		Debug.Log("---------------fCastPoleTime=" + fCastPoleTime);
		if (cOMA_PlayerSelf_Fishing.IsOnBoat())
		{
			cOMA_PlayerSelf_Fishing.OnBoatPosOffsetStart();
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_Throw, t.GetOwner().transform);
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (!bCalFishFloat && Time.time - fEnterTime >= fCastPoleTime)
		{
			t.CalFishFloatPos();
			bCalFishFloat = true;
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
	}

	public override bool OnMessage(COMA_Fishing_PlayerController t, TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 4:
		{
			result = true;
			t.ChangeState(COMA_Fishing_PlayerController.EState.Fishing);
			COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = (COMA_PlayerSelf_Fishing)t.GetOwner();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_Float, cOMA_PlayerSelf_Fishing.CurFishFloat.transform);
			break;
		}
		case 5:
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
