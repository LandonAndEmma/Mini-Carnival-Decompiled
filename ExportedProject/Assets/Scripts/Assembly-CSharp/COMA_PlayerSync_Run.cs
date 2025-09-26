using UnityEngine;

public class COMA_PlayerSync_Run : COMA_PlayerSync
{
	protected override void ReceiveBuff(COMA_CommandDatas commandDatas)
	{
		Debug.Log(" >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ");
		COMA_PlayerSelf_Run cOMA_PlayerSelf_Run = (COMA_PlayerSelf_Run)COMA_PlayerSelf.Instance;
		if (cOMA_PlayerSelf_Run.IsFinishedGame || commandDatas.dataSender.Id.ToString() != base.gameObject.name)
		{
			return;
		}
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = commandDatas as COMA_CD_PlayerBuff;
		if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Ice)
		{
			characterCom.FreezeAnimation();
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Ice"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_ice);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Invincible)
		{
			if (base.transform.FindChild("Shield_01") != null)
			{
				Object.DestroyObject(base.transform.FindChild("Shield_01").gameObject);
			}
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Shield/Shield_01")) as GameObject;
			gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 2.3f, -0.3f);
			Object.DestroyObject(gameObject2, COMA_Buff.Instance.lastTime_invincible);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GetFlash)
		{
			if (!(COMA_PlayerSelf.Instance == null) && COMA_Run_SceneController.Instance.IsFirst(COMA_PlayerSelf.Instance.transform))
			{
				Debug.Log(nickname);
				COMA_Run_SceneController.Instance.UseItemInfo(nickname, 3, COMA_PlayerSelf.Instance.nickname);
				COMA_PlayerSelf.Instance.BuffAdd_GetFlash(0f, COMA_Buff.Instance.lastTime_run_flashHitGround);
				COMA_PlayerSelf_Run cOMA_PlayerSelf_Run2 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Run;
				if (!cOMA_PlayerSelf_Run2.IsInvincible)
				{
					cOMA_PlayerSelf_Run2.StopMove();
				}
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_flash_kill, cOMA_PlayerSelf_Run2.transform);
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Flash)
		{
			GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/flash_kill/flash_kill"), base.transform.position, base.transform.rotation) as GameObject;
			Object.DestroyObject(obj, 1.5f);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.SpeedUp)
		{
			if (base.transform.FindChild("PFB_Buff_SpeedUp") != null)
			{
				Object.DestroyObject(base.transform.FindChild("PFB_Buff_SpeedUp").gameObject);
			}
			GameObject gameObject3 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_SpeedUp"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject3.name = gameObject3.name.Replace("(Clone)", string.Empty);
			gameObject3.transform.parent = base.transform;
			Object.DestroyObject(gameObject3, COMA_Buff.Instance.lastTime_run_speedUp);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Doodle)
		{
			if (!(COMA_PlayerSelf.Instance == null))
			{
				COMA_PlayerSelf_Run cOMA_PlayerSelf_Run3 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Run;
				if (!cOMA_PlayerSelf_Run3.IsInvincible)
				{
					cOMA_PlayerSelf_Run3.BeDoodle();
				}
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GoldAdsorb)
		{
			if (base.transform.FindChild("Cohesion_Money_01") != null)
			{
				Object.DestroyObject(base.transform.FindChild("Cohesion_Money_01").gameObject);
			}
			GameObject gameObject4 = Object.Instantiate(Resources.Load("Particle/effect/Cohesion_Money/Cohesion_Money_01")) as GameObject;
			gameObject4.name = gameObject4.name.Replace("(Clone)", string.Empty);
			gameObject4.transform.parent = base.transform;
			gameObject4.transform.localPosition = new Vector3(0f, 1f, -0.3f);
			Object.DestroyObject(gameObject4, COMA_Buff.Instance.lastTime_run_goldAdsorb);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Exhaust)
		{
			GameObject gameObject5 = Object.Instantiate(Resources.Load("FBX/SceneAddition/Run/Exhaust")) as GameObject;
			gameObject5.name = gameObject5.name.Replace("(Clone)", string.Empty);
			gameObject5.transform.position = base.transform.position + Vector3.up * 0.8f + Vector3.forward * -0.5f;
			Object.DestroyObject(gameObject5, COMA_Buff.Instance.lastTime_run_exhaust);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Inverted)
		{
			if (!(COMA_PlayerSelf.Instance == null))
			{
				COMA_PlayerSelf_Run cOMA_PlayerSelf_Run4 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Run;
				if (!cOMA_PlayerSelf_Run4.IsInvincible)
				{
					cOMA_PlayerSelf_Run4.BeInverted();
				}
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.EffectInverted)
		{
			GameObject gameObject6 = Object.Instantiate(Resources.Load("Particle/effect/Curse/Curse_pfb")) as GameObject;
			gameObject6.transform.position = base.transform.position + new Vector3(0f, 1.7f, 0f);
			gameObject6.transform.parent = base.transform;
			Object.DestroyObject(gameObject6, COMA_Buff.Instance.lastTime_run_inverted);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.RideRocket)
		{
			if (!(COMA_PlayerSelf.Instance == null))
			{
				RideRocket(cOMA_CD_PlayerBuff.buffTime);
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Glue)
		{
			COMA_Run_SceneController.Instance.PutGlue(base.transform.position);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Mine)
		{
			COMA_Run_SceneController.Instance.PutMine(base.transform.position, cOMA_CD_PlayerBuff.buffName);
		}
	}

	public void RideRocket(float fT)
	{
		fT = Mathf.Clamp(fT, 1f, 10f);
		InvincibleStart(fT + 2f);
		Vector3 position = base.transform.position;
		position.x = 0f;
		base.transform.position = position;
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Rocket_Appear/Rocket_Appear1")) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("-------Rocket_Appear  Null");
		}
		gameObject.transform.position = base.transform.position + new Vector3(0f, 0.7f, 0f);
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, 1f);
		GameObject gameObject2 = Object.Instantiate(Resources.Load("FBX/Scene/Run/Prefab/Rocket")) as GameObject;
		gameObject2.transform.parent = base.transform.Find("Character/Bip01");
		gameObject2.transform.localPosition = new Vector3(-0.5f, -1f, -0.5f);
		gameObject2.transform.localRotation = Quaternion.Euler(-90f, -90f, 0f);
		_bRideRocket = true;
		Debug.Log("-------------------------------other fT=" + fT);
		Object.DestroyObject(gameObject2, fT);
		SceneTimerInstance.Instance.Remove(DownRocket);
		SceneTimerInstance.Instance.Add(fT, DownRocket);
	}

	public bool DownRocket()
	{
		_bRideRocket = false;
		return false;
	}

	public override void CharacterCall_Fire()
	{
		if (gunCom.name == "W000")
		{
			return;
		}
		Vector3 position = gunCom.bulletInitLoc.position;
		Quaternion rotation = base.transform.rotation;
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/Run/B006"), position, rotation) as GameObject;
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		COMA_BulletFollower component = gameObject.GetComponent<COMA_BulletFollower>();
		component.distance = gunCom.config.bulletrange;
		component.moveSpeed = gunCom.config.bulletspeed;
		component.gravity = gunCom.config.bulletgravity;
		component.ap = gunCom.config.ap;
		component.apr = gunCom.config.apr;
		component.radius = gunCom.config.radius;
		float num = float.PositiveInfinity;
		Transform target = null;
		for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
		{
			Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
			if (!(child.name == base.name))
			{
				float num2 = child.position.z - base.transform.position.z;
				if (!(num2 <= 0f) && num2 < num)
				{
					num = num2;
					target = child;
				}
			}
		}
		component.SetTarget(target);
		if (gunCom.config.shotcount > 0)
		{
			gunCom.PlayFlash();
		}
	}
}
