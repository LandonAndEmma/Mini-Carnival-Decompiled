using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class COMA_PlayerSync : COMA_Player
{
	[NonSerialized]
	public string[] syncTID = new string[3]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	protected List<Vector3> targetPosition = new List<Vector3>();

	protected List<Quaternion> targetRotation = new List<Quaternion>();

	protected float lerp;

	protected float totalDelay;

	protected float curlerp = 10f;

	protected new void Start()
	{
		base.Start();
		targetPosition.Add(base.transform.position);
		targetRotation.Add(base.transform.rotation);
	}

	protected new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_TRANSFORM, ReceiveTransform);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SWITCHWEAPON, ReceivePutOnGun);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HPSET, ReceiveHPChange);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_BUFF, ReceiveBuff);
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_TRANSFORM, ReceiveTransform);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SWITCHWEAPON, ReceivePutOnGun);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_HPSET, ReceiveHPChange);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_BUFF, ReceiveBuff);
		SceneTimerInstance.Instance.Remove(FrozenBroken);
		SceneTimerInstance.Instance.Remove(FronzenBroken2);
		base.OnDisable();
	}

	protected void ReceiveTransform(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_PlayerTransform cOMA_CD_PlayerTransform = commandDatas as COMA_CD_PlayerTransform;
			targetPosition.Add(cOMA_CD_PlayerTransform.position);
			targetRotation.Add(cOMA_CD_PlayerTransform.rotation);
			localEuler.y = (int)cOMA_CD_PlayerTransform.btElevation;
		}
	}

	protected void ReceivePutOnGun(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_PlayerSwitchWeapon cOMA_CD_PlayerSwitchWeapon = commandDatas as COMA_CD_PlayerSwitchWeapon;
			PutOnGun(cOMA_CD_PlayerSwitchWeapon.weaponSerialName);
		}
	}

	protected void ReceiveHPChange(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			onHpChange(commandDatas);
		}
	}

	protected virtual void onHpChange(COMA_CommandDatas commandDatas)
	{
		if (base.IsDead)
		{
			return;
		}
		COMA_CD_PlayerHPSet cOMA_CD_PlayerHPSet = commandDatas as COMA_CD_PlayerHPSet;
		base.hp = cOMA_CD_PlayerHPSet.hp;
		if (base.IsDead)
		{
			COMA_Scene.Instance.AddToDeathList(this);
			if (IsHidden)
			{
				HideTerminate();
			}
		}
	}

	protected virtual void ReceiveBuff(COMA_CommandDatas commandDatas)
	{
		Debug.Log(" <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ");
		if (commandDatas.dataSender.Id.ToString() != base.gameObject.name)
		{
			return;
		}
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = commandDatas as COMA_CD_PlayerBuff;
		if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Ice)
		{
			characterCom.FreezeAnimation();
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Ice"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_ice);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.TankEffect)
		{
			if (cOMA_CD_PlayerBuff.buffName == "Ice")
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankIce"), base.transform.position, base.transform.rotation) as GameObject;
				gameObject2.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject2, COMA_Buff.Instance.lastTime_ice);
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FronzenBroken2);
			}
			else if (cOMA_CD_PlayerBuff.buffName == "Fire")
			{
				string empty = string.Empty;
				empty = ((!TankCommon.isAliance(sitIndex, COMA_PlayerSelf.Instance.sitIndex)) ? "FBX/Buff/PFB/PFB_Buff_TankFire_B" : "FBX/Buff/PFB/PFB_Buff_TankFire_R");
				GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load(empty), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
				gameObject3.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject3, COMA_Buff.Instance.lastTime_venom);
			}
			else if (cOMA_CD_PlayerBuff.buffName == "Smoke")
			{
				GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankDmgSmoke"), base.transform.position + Vector3.up * 1.5f, base.transform.rotation) as GameObject;
				gameObject4.transform.parent = characterCom.transform;
				gameObject4.transform.localPosition = -Vector3.forward;
				UnityEngine.Object.DestroyObject(gameObject4, 3f);
			}
			else if (cOMA_CD_PlayerBuff.buffName == "Invinceble")
			{
				GameObject gameObject5 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Invincible/Invincible_pfb"), base.transform.position + Vector3.up * 0.5f, base.transform.rotation) as GameObject;
				UnityEngine.Object.Destroy(gameObject5, 3f);
				gameObject5.transform.parent = base.transform;
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Confused)
		{
			GameObject gameObject6 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Confused"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject6.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject6, COMA_Buff.Instance.lastTime_confused);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Venom)
		{
			GameObject gameObject7 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Venom"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject7.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject7, COMA_Buff.Instance.lastTime_venom);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Invincible)
		{
			GameObject gameObject8 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Invincible"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject8.name = gameObject8.name.Replace("(Clone)", string.Empty);
			gameObject8.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject8, COMA_Buff.Instance.lastTime_invincible);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.BeGod)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Transmission_Deliver/Transmission_Deliver"), base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.DestroyObject(obj, 1.5f);
			COMA_PlayerSelf_DropFight cOMA_PlayerSelf_DropFight = COMA_PlayerSelf.Instance as COMA_PlayerSelf_DropFight;
			if (cOMA_PlayerSelf_DropFight.cmrMode == CameraMode.FPS)
			{
				cOMA_PlayerSelf_DropFight.DropDown();
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Hide)
		{
			HideStart(0f);
			GameObject gameObject9 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Stealth/Stealth")) as GameObject;
			gameObject9.transform.position = base.transform.position;
			UnityEngine.Object.DestroyObject(gameObject9, 1f);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Heal)
		{
			base.hp += 50f;
			GameObject gameObject10 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/HP/hp")) as GameObject;
			gameObject10.transform.position = base.transform.position;
			UnityEngine.Object.DestroyObject(gameObject10, 1f);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GetFlash)
		{
			if (COMA_PlayerSelf.Instance != null)
			{
				COMA_PlayerSelf.Instance.BuffAdd_GetFlash(30f, COMA_Buff.Instance.lastTime_flashHitGround);
				if (COMA_PlayerSelf.Instance.IsDead)
				{
					COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
					cOMA_CD_PlayerScoreGet.playerId = cOMA_CD_PlayerBuff.dataSender.Id;
					cOMA_CD_PlayerScoreGet.addScore = 1;
					COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet);
				}
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Flash)
		{
			GameObject gameObject11 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/flash_kill/flash_kill"), base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.DestroyObject(gameObject11, 1.5f);
			if (COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName))
			{
				gameObject11.transform.parent = base.transform;
			}
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Evil)
		{
			GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Transfiguration/Transfiguration"), base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.DestroyObject(obj2, 2f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Transfiguration, base.transform, 4f);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.SpeedUp)
		{
			GameObject gameObject12 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_SpeedUp"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject12.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject12, COMA_Buff.Instance.lastTime_speedUp);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.SelfBlast)
		{
			GameObject gameObject13 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Cannon_Smitten/Cannon_Smitten"), base.transform.position, Quaternion.identity) as GameObject;
			UnityEngine.Object.DestroyObject(gameObject13, 3f);
			EffectAudioBehaviour component = gameObject13.GetComponent<EffectAudioBehaviour>();
			if (component != null)
			{
				component.enabled = false;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Player_Explode, base.transform);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.Blood_Stun)
		{
			GameObject gameObject14 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/stun/stun_01"), base.transform.position, Quaternion.identity) as GameObject;
			gameObject14.transform.position = base.transform.position + Vector3.up * 1.6f;
			gameObject14.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
			gameObject14.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject14, COMA_Buff.Instance.lastTime_blood_stun);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Mace_Stun, base.transform);
		}
	}

	public override bool FrozenBroken()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_IceBroken")) as GameObject;
		gameObject.transform.position = base.transform.position + Vector3.up * 0.5f;
		UnityEngine.Object.DestroyObject(gameObject, 2f);
		return false;
	}

	public bool FronzenBroken2()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_IceBroken")) as GameObject;
		gameObject.transform.position = base.transform.position + Vector3.up * 0.5f;
		UnityEngine.Object.DestroyObject(gameObject, 2f);
		if (COMA_CommonOperation.Instance.IsMode_Tank(Application.loadedLevelName))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Ice_Crack, base.transform);
		}
		return false;
	}

	public void FinishDownLoadTextures(string response)
	{
		Debug.Log("FinishDownLoadTextures : " + response);
		JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
		string text = jsonData["tid"].ToString();
		string content = jsonData["Tex"].ToString();
		string s = jsonData["Kind"].ToString();
		byte[] data = COMA_TexBase.Instance.StringToTextureBytes(content);
		int num = int.Parse(s);
		Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		string key = base.gameObject.name + "_" + num;
		if (!COMA_TexLib.Instance.currentRoomPlayerTextures.ContainsKey(key))
		{
			COMA_TexLib.Instance.currentRoomPlayerTextures.Add(key, texture2D);
		}
		characterCom.bodyObjs[num].renderer.material.mainTexture = texture2D;
		if (UI_3DModeToTexture.Instance != null && num == 0)
		{
			Transform transform = UI_3DModeToTexture.Instance.sourceObjs[sitIndex]._srcObj.transform.FindChild("head");
			transform.renderer.material.mainTexture = texture2D;
			UI_3DModeToTexture.Instance.Set3DModelEnableRender(sitIndex);
		}
		if (UI_3DModeToTUIMgr.Instance != null)
		{
			switch (num)
			{
			case 0:
			{
				Transform transform4 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("head");
				transform4.renderer.material.mainTexture = texture2D;
				break;
			}
			case 1:
			{
				Transform transform3 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("body");
				transform3.renderer.material.mainTexture = texture2D;
				break;
			}
			case 2:
			{
				Transform transform2 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("breeches");
				transform2.renderer.material.mainTexture = texture2D;
				break;
			}
			}
		}
	}

	public void TryToPutAvatar()
	{
		for (int i = 0; i < 3; i++)
		{
			string text = base.gameObject.name + "_" + i;
			Texture2D value = null;
			if (!COMA_TexLib.Instance.currentRoomPlayerTextures.TryGetValue(text, out value))
			{
				continue;
			}
			int num = i;
			Debug.Log("TryToPutAvatar : " + num + " " + text);
			if (value != null)
			{
				value.filterMode = FilterMode.Point;
			}
			characterCom.bodyObjs[num].renderer.material.mainTexture = value;
			if (UI_3DModeToTexture.Instance != null && num == 0)
			{
				Transform transform = UI_3DModeToTexture.Instance.sourceObjs[sitIndex]._srcObj.transform.FindChild("head");
				transform.renderer.material.mainTexture = value;
				UI_3DModeToTexture.Instance.Set3DModelEnableRender(sitIndex);
			}
			if (UI_3DModeToTUIMgr.Instance != null)
			{
				switch (num)
				{
				case 0:
				{
					Transform transform4 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("head");
					transform4.renderer.material.mainTexture = value;
					break;
				}
				case 1:
				{
					Transform transform3 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("body");
					transform3.renderer.material.mainTexture = value;
					break;
				}
				case 2:
				{
					Transform transform2 = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].transform.FindChild("breeches");
					transform2.renderer.material.mainTexture = value;
					break;
				}
				}
			}
		}
	}

	protected void PutDownGun()
	{
		PutOnGun("W000");
	}

	protected void PutOnGun(string weaponName)
	{
		if (gunCom != null)
		{
			UnityEngine.Object.DestroyObject(gunCom.gameObject);
		}
		string text = weaponName.Substring(0, 4);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Magazine/Weapon/PFB/" + text)) as GameObject;
		gameObject.name = weaponName;
		gameObject.transform.parent = characterCom.handTrs;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gunCom = gameObject.GetComponent<COMA_Gun>();
		if (gunCom.name != "W000")
		{
			gunCom.InitData();
		}
		if (IsHidden)
		{
			HideAdd(0f);
		}
	}

	protected new void Update()
	{
		UpdateShadow();
		if (targetPosition.Count > 3)
		{
			curlerp = targetPosition.Count - 2 + 10;
		}
		else
		{
			curlerp = 10f;
		}
		if (targetPosition.Count > 1)
		{
			lerp += Time.deltaTime * curlerp;
			base.transform.position = Vector3.Lerp(targetPosition[0], targetPosition[1], lerp);
			base.transform.rotation = Quaternion.Lerp(targetRotation[0], targetRotation[1], lerp);
			if (lerp > 0.99f)
			{
				base.transform.position = targetPosition[1];
				base.transform.rotation = targetRotation[1];
				lerp = 0f;
				targetPosition.RemoveAt(0);
				targetRotation.RemoveAt(0);
			}
		}
		RotateWaist();
	}

	public override void CharacterCall_Fire()
	{
		if (gunCom == null || gunCom.bulletInitLoc == null)
		{
			return;
		}
		float num = gunCom.config.bulletrange * gunCom.config.precision;
		for (int i = 0; i < gunCom.config.shotcount; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num, num), gunCom.config.bulletrange);
			Vector3 vector2 = characterCom.viewTrs.position + characterCom.viewTrs.rotation * vector;
			Ray ray = new Ray(characterCom.viewTrs.position, vector2 - characterCom.viewTrs.position);
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, gunCom.config.bulletrange, layerMask))
			{
				vector2 = hitInfo.point;
			}
			Vector3 position = gunCom.bulletInitLoc.position;
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(vector2 - position);
			GameObject gameObject = UnityEngine.Object.Instantiate(gunCom.bulletPfb, position, identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			COMA_Bullet component = gameObject.GetComponent<COMA_Bullet>();
			component.distance = gunCom.config.bulletrange;
			component.moveSpeed = gunCom.config.bulletspeed;
			component.gravity = gunCom.config.bulletgravity;
			component.ap = gunCom.config.ap;
			component.apr = gunCom.config.apr;
			component.radius = gunCom.config.radius;
		}
		if (gunCom.config.shotcount > 0)
		{
			gunCom.PlayFlash();
		}
	}

	public void CharacterCall_Death()
	{
		base.hp = 0f;
	}
}
