using UnityEngine;

public class COMA_Fishing_PlayerState_FetchingPole : TState<COMA_Fishing_PlayerController>
{
	private float fCancelPoleTime;

	private float fEnterTime;

	private GameObject _objFetchFloat;

	private float fTickStart;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		nCurFrame = 0;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		fTickStart = Time.time;
		fEnterTime = Time.time;
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Floats/Floats_01_pfb")) as GameObject;
		gameObject.transform.position = cOMA_PlayerSelf_Fishing.CurFishFloat.transform.position;
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
		Object.DestroyObject(gameObject, 1f);
		Debug.Log("==============>Floats_01_pfb:" + gameObject.transform.position);
		int num = ((t._fetchingNextState == COMA_Fishing_PlayerController.EState.SuccessPole) ? 1 : 0);
		if (cOMA_PlayerSelf_Fishing.CurFishItem != null)
		{
			if (cOMA_PlayerSelf_Fishing.CurFishItem.GetEntityType() == 101)
			{
				num = 1;
			}
			else if (cOMA_PlayerSelf_Fishing.CurFishItem.GetEntityType() == 100)
			{
				num = 2;
			}
			else if (cOMA_PlayerSelf_Fishing.CurFishItem.GetEntityType() == 102)
			{
				num = 3;
			}
		}
		string text = "Fishing_cancelpole";
		string animation = "pole_fetch";
		if (t.nFetchParam == 1)
		{
			text = "Fishing_cancelpole02";
			animation = "pole_fetch02";
		}
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation(text, string.Empty, new Vector3(num, 0f, 0f));
		fCancelPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation[text].length;
		cOMA_PlayerSelf_Fishing.CurFishPole._aniCmp.Play(animation);
		_objFetchFloat = null;
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		nCurFrame++;
		if (nCurFrame == 1)
		{
			COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
			if (t._fetchingNextState == COMA_Fishing_PlayerController.EState.SuccessPole)
			{
				COMA_Fishing_FishableObj curFishItem = cOMA_PlayerSelf_Fishing.CurFishItem;
				curFishItem.gameObject.transform.parent = cOMA_PlayerSelf_Fishing.CurFishPole.GetFishingPoleItemPos();
				curFishItem.gameObject.transform.localPosition = Vector3.zero;
				curFishItem.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			}
			else if (t._fetchingNextState == COMA_Fishing_PlayerController.EState.CancelPole)
			{
				_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FetchFloat")) as GameObject;
				_objFetchFloat.transform.parent = cOMA_PlayerSelf_Fishing.CurFishPole.GetFishingPoleItemPos();
				_objFetchFloat.transform.localPosition = Vector3.zero;
				_objFetchFloat.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
		}
		if (Time.time - fEnterTime >= fCancelPoleTime)
		{
			t.ChangeState(t._fetchingNextState);
		}
	}

	public override void Exit(COMA_Fishing_PlayerController t)
	{
		if (t._fetchingNextState == COMA_Fishing_PlayerController.EState.CancelPole && _objFetchFloat != null)
		{
			Object.DestroyObject(_objFetchFloat);
		}
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
