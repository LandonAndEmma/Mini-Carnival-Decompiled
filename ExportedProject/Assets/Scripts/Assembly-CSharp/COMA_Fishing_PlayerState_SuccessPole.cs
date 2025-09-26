using UnityEngine;

public class COMA_Fishing_PlayerState_SuccessPole : TState<COMA_Fishing_PlayerController>
{
	private float fSuccessPoleTime;

	private float fEnterTime;

	private bool bSwitchItem;

	public override void Enter(COMA_Fishing_PlayerController t)
	{
		fEnterTime = Time.time;
		bSwitchItem = false;
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = t.GetOwner() as COMA_PlayerSelf_Fishing;
		cOMA_PlayerSelf_Fishing.characterCom.PlayMyAnimation("Fishing_success", string.Empty);
		fSuccessPoleTime = cOMA_PlayerSelf_Fishing.characterCom.animation["Fishing_success"].length;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_Inhand, t.GetOwner().transform);
		cOMA_PlayerSelf_Fishing.CurFishItem.gameObject.transform.parent = cOMA_PlayerSelf_Fishing.characterCom.fishing_item;
		cOMA_PlayerSelf_Fishing.CurFishItem.gameObject.transform.localPosition = Vector3.zero;
		cOMA_PlayerSelf_Fishing.CurFishItem.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
		if (cOMA_PlayerSelf_Fishing.CurFishItem != null && cOMA_PlayerSelf_Fishing.CurFishItem.GetEntityType() == 100)
		{
			Debug.Log("Add Score:" + ((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetWeight());
			COMA_Pref.Instance.AddRankScoreOfCurrentScene(((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetWeight());
			COMA_Achievement.Instance.Fish1++;
			COMA_Achievement.Instance.Fish2++;
			if (COMA_FishCatalog.Instance.lst[((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetCustomID() - 1].num == 0)
			{
				COMA_Achievement.Instance.Fish4++;
			}
			if (COMA_FishCatalog.Instance.lst[((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetCustomID() - 1].num == 0)
			{
				COMA_Achievement.Instance.Fish5++;
			}
			if (cOMA_PlayerSelf_Fishing.CurFishPole != null)
			{
				Fish_Param fishParam = COMA_Fishing_FishPool.Instance.GetFishParam(((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetCustomID());
				if (COMA_Achievement.Instance.Fish3 == 0 && fishParam._nMaxWeight <= 750)
				{
					bool flag = true;
					int fishCount = COMA_FishCatalog.Instance.GetFishCount();
					for (int i = 0; i < fishCount; i++)
					{
						if (i != ((COMA_Fishing_Fish)cOMA_PlayerSelf_Fishing.CurFishItem).GetCustomID() - 1 && COMA_Fishing_FishPool.Instance.GetFishParam(i + 1)._nMaxWeight <= 750 && COMA_FishCatalog.Instance.lst[i].num <= 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						COMA_Achievement.Instance.Fish3++;
					}
				}
			}
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Player_joy_jump, t.GetOwner().transform);
	}

	public override void Update(COMA_Fishing_PlayerController t)
	{
		if (Time.time - fEnterTime >= fSuccessPoleTime)
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
