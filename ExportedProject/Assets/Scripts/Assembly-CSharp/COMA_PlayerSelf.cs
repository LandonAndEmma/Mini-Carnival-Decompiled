using System;
using UnityEngine;

public class COMA_PlayerSelf : COMA_Player
{
	private static COMA_PlayerSelf _instance;

	[NonSerialized]
	public Transform bornPointTrs;

	protected Vector2 moveInput = Vector2.zero;

	protected float targetHeight;

	protected float gravity = 20f;

	protected Vector3 speedPassive = Vector3.zero;

	protected float distance = 2f;

	protected Vector3 lastMousePosition = Vector3.zero;

	[NonSerialized]
	public float timeToExitAttack = 3f;

	protected bool bIsFiring;

	protected Vector3 relivePosition = Vector3.zero;

	protected float frequencyTime;

	protected float reloadTime;

	protected float preshootTime;

	protected int clipCount = 1;

	protected int CLIPNUMBER;

	protected int clipNumber;

	protected bool buff_oneShot;

	protected bool buff_speedUp;

	protected bool buff_flashHit;

	public static COMA_PlayerSelf Instance
	{
		get
		{
			return _instance;
		}
	}

	protected new void Start()
	{
		base.Start();
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput_WASD, this);
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput, this);
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput_MouseMove, this);
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput_MouseButton, this);
		clipNumber = CLIPNUMBER;
		PutDownGun();
		characterCom.PlayMyAnimation("Idle", gunCom.name);
		try
		{
			for (int i = 0; i < characterCom.bodyObjs.Length; i++)
			{
				characterCom.bodyObjs[i].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[i]].texture;
			}
			for (int j = 0; j < COMA_Pref.Instance.AInPack.Length; j++)
			{
				if (COMA_Pref.Instance.AInPack[j] >= 0)
				{
					characterCom.CreateAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[j]].serialName);
				}
			}
		}
		catch (Exception)
		{
		}
		if (UI_3DModeToTexture.Instance != null)
		{
			Transform transform = UI_3DModeToTexture.Instance.sourceObjs[sitIndex]._srcObj.transform.FindChild("head");
			transform.renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].texture;
			UI_3DModeToTexture.Instance.Set3DModelEnableRender(sitIndex);
		}
		if (!(UI_3DModeToTUIMgr.Instance != null))
		{
			return;
		}
		COMA_PlayerCharacter component = UI_3DModeToTUIMgr.Instance.sourceObjs[sitIndex].GetComponent<COMA_PlayerCharacter>();
		component.bodyObjs[0].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].texture;
		component.bodyObjs[1].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].texture;
		component.bodyObjs[2].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].texture;
		for (int k = 0; k < COMA_Pref.Instance.AInPack.Length; k++)
		{
			if (COMA_Pref.Instance.AInPack[k] >= 0)
			{
				component.CreateAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[k]].serialName, "Player");
			}
		}
	}

	private void OnDestroy()
	{
		if (TPCInputMgr.Instance != null)
		{
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput_WASD, this);
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput, this);
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput_MouseMove, this);
			TPCInputMgr.Instance.UnregisterPCInput(COMA_MsgSec.PCInput_MouseButton, this);
		}
	}

	protected new void OnEnable()
	{
		_instance = this;
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		_instance = null;
		SceneTimerInstance.Instance.Remove(ToHoldState);
		base.OnDisable();
	}

	protected bool SendTransform()
	{
		COMA_CD_PlayerTransform cOMA_CD_PlayerTransform = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_TRANSFORM) as COMA_CD_PlayerTransform;
		cOMA_CD_PlayerTransform.position = base.transform.position;
		cOMA_CD_PlayerTransform.rotation = base.transform.rotation;
		cOMA_CD_PlayerTransform.btElevation = (byte)localEuler.y;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerTransform);
		return true;
	}

	protected bool SendGameTime()
	{
		return true;
	}

	public override void OnHitOther(int fromID, int toID, string bulletName, float bulletAP, Vector3 push)
	{
		COMA_CD_PlayerHit cOMA_CD_PlayerHit = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_HIT) as COMA_CD_PlayerHit;
		// Make the conditional result an int, then cast to byte to avoid mixing byte and int in the ternary.
		cOMA_CD_PlayerHit.btBulletKind = (byte)(!string.IsNullOrEmpty(bulletName) ? int.Parse(bulletName.Substring(1)) : 0);
		cOMA_CD_PlayerHit.nFromID = fromID;
		cOMA_CD_PlayerHit.attackPoint = bulletAP;
		cOMA_CD_PlayerHit.blastPush = push;
		if (toID <= 0 || (fromID == toID && toID == id))
		{
			ReceiveHurt(cOMA_CD_PlayerHit.attackPoint, cOMA_CD_PlayerHit.blastPush);
		}
		else if (toID == id)
		{
			ReceiveHurt(cOMA_CD_PlayerHit);
		}
		else
		{
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerHit, toID);
		}
	}

	protected void ReceiveHurt(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerHit cOMA_CD_PlayerHit = commandDatas as COMA_CD_PlayerHit;
		if (ReceiveHurt(cOMA_CD_PlayerHit.nFromID, cOMA_CD_PlayerHit.btBulletKind, cOMA_CD_PlayerHit.attackPoint, cOMA_CD_PlayerHit.blastPush))
		{
			COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
			cOMA_CD_PlayerScoreGet.playerId = cOMA_CD_PlayerHit.nFromID;
			cOMA_CD_PlayerScoreGet.addScore = 1;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet);
		}
	}

	public bool ReceiveHurt(float bulletAP, Vector3 push)
	{
		return ReceiveHurt(0, 0, bulletAP, push);
	}

	public override bool ReceiveHurt(int fromID, int bulletID, float bulletAP, Vector3 push)
	{
		return ReceiveHurt(fromID, bulletID, bulletAP, push, true);
	}

	public bool ReceiveHurt(int fromID, int bulletID, float bulletAP, Vector3 push, bool bNeedJump)
	{
		if (base.IsDead)
		{
			return false;
		}
		if (base.IsFrozen)
		{
			return false;
		}
		if (IsInvincible)
		{
			return false;
		}
		switch (bulletID)
		{
		case 4:
			if (!base.IsVenom)
			{
				base.IsVenom = true;
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_venom, base.VenomRecover);
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Venom"), base.transform.position, base.transform.rotation) as GameObject;
				gameObject2.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject2, COMA_Buff.Instance.lastTime_venom);
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff2.buffState = COMA_Buff.Buff.Venom;
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff2);
			}
			break;
		case 5:
			if (!base.IsConfused)
			{
				base.IsConfused = true;
				Debug.Log(COMA_Buff.Instance.lastTime_confused);
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_confused, base.ConfusedRecover);
				GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Confused"), base.transform.position, base.transform.rotation) as GameObject;
				gameObject4.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject4, COMA_Buff.Instance.lastTime_confused);
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff4 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff4.buffState = COMA_Buff.Buff.Confused;
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff4);
			}
			break;
		case 6:
		{
			characterCom.FreezeAnimation();
			base.IsFrozen = true;
			icefromID = fromID;
			iceAP = bulletAP;
			icePush = push.magnitude;
			GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_Ice"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject3.transform.parent = base.transform;
			UnityEngine.Object.DestroyObject(gameObject3, COMA_Buff.Instance.lastTime_ice);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff3 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff3.buffState = COMA_Buff.Buff.Ice;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff3);
			return false;
		}
		case 7:
			Transmission();
			break;
		case 14:
			if (!base.IsVenom)
			{
				base.IsVenom = true;
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_venom, base.VenomRecover);
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankFire"), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
				Debug.Log("fireobj pos:" + gameObject.transform.position);
				gameObject.transform.parent = characterCom.transform;
				UnityEngine.Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_venom);
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.TankEffect;
				cOMA_CD_PlayerBuff.buffName = "Fire";
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
			}
			break;
		}
		switch (bulletID)
		{
		case 16:
			icefromID = fromID;
			iceAP = bulletAP;
			icePush = push.magnitude;
			if (base.hp - bulletAP > 0f)
			{
				GameObject gameObject6 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankIce"), base.transform.position, base.transform.rotation) as GameObject;
				gameObject6.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject6, COMA_Buff.Instance.lastTime_ice);
				SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff6 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff6.buffState = COMA_Buff.Buff.TankEffect;
				cOMA_CD_PlayerBuff6.buffName = "Ice";
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff6);
			}
			break;
		case 11:
		case 19:
		{
			GameObject gameObject5 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankDmgSmoke"), base.transform.position + Vector3.up * 1.5f, base.transform.rotation) as GameObject;
			gameObject5.transform.parent = characterCom.transform;
			gameObject5.transform.localPosition = -Vector3.forward;
			UnityEngine.Object.DestroyObject(gameObject5, 3f);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff5 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff5.buffState = COMA_Buff.Buff.TankEffect;
			cOMA_CD_PlayerBuff5.buffName = "Smoke";
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff5);
			break;
		}
		}
		base.hp -= bulletAP;
		OnHPChange();
		Debug.Log("HP : " + base.hp + "/" + HP);
		movePsv = push;
		if (bNeedJump)
		{
			characterCom.PlayMyAnimation("Jump", gunCom.name);
		}
		if (bulletID == 16 && !base.IsDead)
		{
			base.IsFrozen = true;
		}
		if (base.IsDead)
		{
			if (IsHidden)
			{
				HideTerminate();
			}
			characterCom.PlayMyAnimation("Die", gunCom.name);
			SceneTimerInstance.Instance.Add(3f, OnRelive);
			COMA_Scene.Instance.AddToDeathList(this);
			if (COMA_CommonOperation.Instance.IsMode_Hunger(Application.loadedLevelName) && fromID != 0)
			{
				Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(fromID.ToString());
				if (transform != null)
				{
					COMA_Creation component = transform.GetComponent<COMA_Creation>();
					COMA_Hunger_SceneController.Instance.ShowKilledInfo(component.nickname, component.rt3DObj.transform.parent.camera.targetTexture);
				}
			}
			if (COMA_CommonOperation.Instance.IsMode_Rocket(Application.loadedLevelName))
			{
				COMA_DropFight_SceneController.Instance.ShowDeathTip();
			}
			return true;
		}
		if (COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName))
		{
			GameObject gameObject7 = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Blood/Electric saw_blood_01")) as GameObject;
			gameObject7.transform.position = base.transform.position + Vector3.up * 0.8f;
			gameObject7.transform.eulerAngles = new Vector3(270f, 0f, 0f);
			UnityEngine.Object.DestroyObject(gameObject7, 1.5f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Blood, base.transform);
			COMA_Castle_SceneController.Instance.StartRedScreen();
		}
		return false;
	}

	public void SendScore()
	{
		COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
		cOMA_CD_PlayerScoreGet.playerId = id;
		cOMA_CD_PlayerScoreGet.curScore = base.score;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet);
	}

	protected virtual void ReceiveScoreGet(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = commandDatas as COMA_CD_PlayerScoreGet;
		string text = cOMA_CD_PlayerScoreGet.playerId.ToString();
		Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(text);
		if (transform == null)
		{
			GameObject gameObject = GameObject.Find(text);
			if (gameObject != null)
			{
				transform = gameObject.transform;
			}
		}
		if (transform != null)
		{
			COMA_Player component = transform.GetComponent<COMA_Player>();
			if (cOMA_CD_PlayerScoreGet.addScore != 0)
			{
				component.score += cOMA_CD_PlayerScoreGet.addScore;
			}
			else
			{
				component.score = cOMA_CD_PlayerScoreGet.curScore;
			}
			OnOtherGetFlag(component);
		}
	}

	protected virtual void OnHPChange()
	{
		COMA_CD_PlayerHPSet cOMA_CD_PlayerHPSet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_HPSET) as COMA_CD_PlayerHPSet;
		cOMA_CD_PlayerHPSet.hp = base.hp;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerHPSet);
	}

	protected void BaseState()
	{
		if (moveInput == Vector2.zero)
		{
			characterCom.PlayMyAnimation("Idle", gunCom.name);
			return;
		}
		float x = moveInput.x;
		float num = moveInput.y * 1.73f;
		if (num > 0f - x && num >= x)
		{
			characterCom.PlayMyAnimation("Run", gunCom.name, "Front");
		}
		else if (num < 0f - x && num <= x)
		{
			characterCom.PlayMyAnimation("Run", gunCom.name, "Back");
		}
		else if (num <= 0f - x && num > x)
		{
			characterCom.PlayMyAnimation("Run", gunCom.name, "Left");
		}
		else if (num >= 0f - x && num < x)
		{
			characterCom.PlayMyAnimation("Run", gunCom.name, "Right");
		}
	}

	protected void TurnAround(Vector3 spdV3)
	{
		float num = Vector3.Angle(base.transform.forward, spdV3);
		if (num > 150f)
		{
			if (Vector3.Cross(spdV3, base.transform.forward).y > 0f)
			{
				base.transform.rotation *= new Quaternion(0f, 0.7f, 0f, 0.7f);
			}
			else
			{
				base.transform.rotation *= new Quaternion(0f, -0.7f, 0f, 0.7f);
			}
		}
		else
		{
			base.transform.forward = Vector3.Lerp(base.transform.forward, spdV3, 0.3f);
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
			COMA_Aim.Instance.SetVisible(true);
			COMA_Aim.Instance.maxSize = gunCom.config.aimradiusmax;
			frequencyTime = gunCom.config.frequency;
			reloadTime = 0f;
			preshootTime = gunCom.config.preshoottime;
			clipCount = gunCom.config.clipcount;
			if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
			{
				Debug.Log(clipCount + " " + clipNumber);
				UIInGame_BoxMagazineMgr.UIBoxMagazine[] array = new UIInGame_BoxMagazineMgr.UIBoxMagazine[clipNumber + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new UIInGame_BoxMagazineMgr.UIBoxMagazine(clipCount, clipCount);
				}
				COMA_Scene.Instance.magazineCom.InitBoxMagazine(array, 0, COMA_Scene.Instance.bInfinityMode);
			}
		}
		else
		{
			COMA_Aim.Instance.SetVisible(false);
			if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
			{
				UIInGame_BoxMagazineMgr.UIBoxMagazine[] array2 = new UIInGame_BoxMagazineMgr.UIBoxMagazine[1];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = new UIInGame_BoxMagazineMgr.UIBoxMagazine(0, 0);
				}
				COMA_Scene.Instance.magazineCom.InitBoxMagazine(array2, 0, COMA_Scene.Instance.bInfinityMode);
			}
		}
		if (COMA_Network.Instance.IsConnected())
		{
			COMA_CD_PlayerSwitchWeapon cOMA_CD_PlayerSwitchWeapon = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SWITCHWEAPON) as COMA_CD_PlayerSwitchWeapon;
			cOMA_CD_PlayerSwitchWeapon.weaponSerialName = weaponName;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerSwitchWeapon);
		}
		if (IsHidden)
		{
			HideAdd(0.3f);
		}
	}

	public virtual int UseItem(int itemID)
	{
		return 0;
	}

	public bool ToHoldState()
	{
		characterCom.PlayMyAnimation("Attack", gunCom.name, "Hold");
		return false;
	}

	public bool BuffRemove_OneShot()
	{
		buff_oneShot = false;
		return false;
	}

	public virtual bool BuffRemove_SpeedUp()
	{
		buff_speedUp = false;
		if (characterCom.animation["Run00_fast"] != null)
		{
			characterCom.PlayMyAnimation("Run00_fast", gunCom.name, "0");
		}
		return false;
	}

	public void BuffAdd_FlashHit()
	{
		ReceiveHurt(0f, Vector3.zero);
		base.transform.localScale = Vector3.one * 0.5f;
		buff_flashHit = true;
		SceneTimerInstance.Instance.Add(5f, BuffRemove_FlashHit);
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/flash_kill/flash_kill"), base.transform.position, base.transform.rotation) as GameObject;
		UnityEngine.Object.DestroyObject(obj, 1.5f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_flash_kill, base.transform);
	}

	public bool BuffRemove_FlashHit()
	{
		base.transform.localScale = Vector3.one;
		buff_flashHit = false;
		return false;
	}

	public void BuffAdd_GetFlash(float ap, float holdTime)
	{
		if (!IsInvincible)
		{
			GroundedStart(holdTime);
			ReceiveHurt(ap, Vector3.zero);
			characterCom.PlayMyAnimation("Makeup01", gunCom.name);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/flash_kill/flash_kill"), base.transform.position, base.transform.rotation) as GameObject;
		if (COMA_CommonOperation.Instance.IsMode_Tank(COMA_NetworkConnect.sceneName))
		{
			gameObject.transform.parent = base.transform;
		}
		UnityEngine.Object.DestroyObject(gameObject, 1.5f);
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.Flash;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
	}

	protected virtual void RotatePlayer(float _x, float _y)
	{
		float y = _x * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		base.transform.rotation *= quaternion;
		float num = _y * 157f * 2f / (float)Screen.height;
		localEuler.y += num;
		if (localEuler.y > 150f)
		{
			localEuler.y = 150f;
		}
		if (localEuler.y < 10f)
		{
			localEuler.y = 10f;
		}
	}

	protected bool IsOnGround()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up, Vector3.down);
		int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Transfer"));
		RaycastHit hitInfo;
		return Physics.Raycast(ray, out hitInfo, 1.1f, layerMask);
	}

	protected void UpdateFire()
	{
		if (reloadTime > 0f)
		{
			reloadTime -= Time.deltaTime;
			if (reloadTime <= 0f)
			{
				if (!COMA_Scene.Instance.bInfinityMode)
				{
					clipNumber--;
				}
				clipCount = gunCom.config.clipcount;
				frequencyTime = 0f;
				if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
				{
					COMA_Scene.Instance.magazineCom.ChangeBoxMagazine(0);
				}
			}
		}
		else if (frequencyTime > 0f)
		{
			frequencyTime -= Time.deltaTime;
		}
		else if (clipCount <= 0)
		{
			if (clipNumber > 0 || COMA_Scene.Instance.bInfinityMode)
			{
				reloadTime = gunCom.config.reloadtime;
				characterCom.PlayMyAnimation("Reload", gunCom.name);
			}
			else if (!COMA_CommonOperation.Instance.IsMode_Castle(Application.loadedLevelName) && !COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName) && gunCom.transform.childCount > 0)
			{
				PutDownGun();
				characterCom.PlayMyAnimation("Idle", gunCom.name);
			}
		}
		else if (bIsFiring)
		{
			bIsFiring = false;
			if (preshootTime > 0f)
			{
				preshootTime -= Time.deltaTime;
			}
			else
			{
				Fire();
			}
		}
		else
		{
			preshootTime = gunCom.config.preshoottime;
		}
	}

	protected virtual void Fire()
	{
		if (gunCom.name == "W003")
		{
			gunCom.EnergyGunAccumulate(false);
		}
		characterCom.PlayMyAnimation("Attack", gunCom.name, "0");
		characterCom.PlayMyAnimation("Attack", gunCom.name);
		SceneTimerInstance.Instance.Remove(ToHoldState);
		SceneTimerInstance.Instance.Add(1.5f, ToHoldState);
		COMA_Aim.Instance.Magnify(gunCom.config.aimmagnify);
		frequencyTime = gunCom.config.frequency;
		clipCount--;
		preshootTime = gunCom.config.preshoottime;
		if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
		{
			COMA_Scene.Instance.magazineCom.RefreshCurBulletNum(clipCount, false);
		}
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			return true;
		}
		if (msg._nMsgId == 30001)
		{
			Vector2 normalized = ((Vector2)msg._pExtraInfo).normalized;
			UI_SetMoveInput(normalized.x, normalized.y);
		}
		else if (msg._nMsgId == 30000)
		{
			TPCInputEvent tPCInputEvent = (TPCInputEvent)msg._pExtraInfo;
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.L)
			{
				Screen.lockCursor = !Screen.lockCursor;
				lastMousePosition = Input.mousePosition;
			}
			if (!Screen.lockCursor && tPCInputEvent.code == KeyCode.E)
			{
				if (tPCInputEvent.type == EventType.KeyDown)
				{
					UI_SetFire(0, 0f, 0f);
				}
				else if (tPCInputEvent.type == EventType.KeyUp)
				{
					UI_SetFire(2, 0f, 0f);
				}
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Space)
			{
				UI_Jump();
			}
			for (int i = 1; i <= 8; i++)
			{
				if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == (KeyCode)(48 + i) && COMA_Sys.Instance.IsSuperAccount)
				{
					PutOnGun("W00" + i);
				}
			}
		}
		else if (msg._nMsgId == 30002)
		{
			if (Screen.lockCursor)
			{
				Vector2 vector = (Vector2)msg._pExtraInfo * 8f;
				UI_SetRotatePlayer(vector.x, vector.y);
			}
		}
		else if (msg._nMsgId == 30003)
		{
			TPCInputEvent tPCInputEvent2 = (TPCInputEvent)msg._pExtraInfo;
			if (Screen.lockCursor && tPCInputEvent2.code == KeyCode.Mouse0)
			{
				if (tPCInputEvent2.type == EventType.KeyDown)
				{
					UI_SetFire(0, 0f, 0f);
				}
				else if (tPCInputEvent2.type == EventType.KeyUp)
				{
					UI_SetFire(2, 0f, 0f);
				}
			}
		}
		return true;
	}

	public void UI_SetMoveInput(float w, float l)
	{
		if (IsInverted)
		{
			w *= -1f;
		}
		if (base.IsDead)
		{
			w = 0f;
			l = 0f;
		}
		if (base.IsFrozen)
		{
			w = 0f;
			l = 0f;
		}
		moveInput = new Vector2(w, l);
	}

	public void UI_SetRotatePlayer(float w, float l)
	{
		if (!base.IsFrozen && !base.IsDead)
		{
			RotatePlayer(w, l);
		}
	}

	public virtual void UI_SetFire(int type, float w, float l)
	{
		if (base.IsDead)
		{
			bIsFiring = false;
		}
		else if (!base.IsFrozen)
		{
			switch (type)
			{
			case 0:
				bIsFiring = true;
				break;
			case 2:
				bIsFiring = false;
				break;
			case 1:
				break;
			}
		}
	}

	public virtual void UI_SetFire2(object param)
	{
	}

	public virtual void UI_Jump()
	{
		if (!base.IsDead && !base.IsFrozen && IsOnGround())
		{
			movePsv = Vector3.up * speedJump;
			characterCom.PlayMyAnimation("Jump", gunCom.name);
		}
	}

	public override void CharacterCall_Fire()
	{
		if (gunCom.name == "W003")
		{
			gunCom.EnergyGunAccumulate(false);
		}
		float num = gunCom.config.bulletrange * gunCom.config.precision;
		for (int i = 0; i < gunCom.config.shotcount; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num, num), gunCom.config.bulletrange);
			Vector3 vector2 = characterCom.viewTrs.position + characterCom.viewTrs.rotation * vector;
			Ray ray = new Ray(characterCom.viewTrs.position, characterCom.viewTrs.rotation * vector);
			ray.origin += ray.direction * 3f;
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, gunCom.config.bulletrange, layerMask))
			{
				vector2 = hitInfo.point;
			}
			Vector3 position = gunCom.bulletInitLoc.position;
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(vector2 - position);
			Debug.LogWarning("  create bullet --" + gunCom.bulletPfb.name);
			GameObject gameObject = UnityEngine.Object.Instantiate(gunCom.bulletPfb, position, identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			COMA_Bullet component = gameObject.GetComponent<COMA_Bullet>();
			component.fromPlayerCom = this;
			component.distance = gunCom.config.bulletrange;
			component.moveSpeed = gunCom.config.bulletspeed;
			component.gravity = gunCom.config.bulletgravity;
			component.ap = ((!buff_oneShot) ? gunCom.config.ap : COMA_Sys.Instance.apToOneShot);
			component.apr = ((!buff_oneShot) ? gunCom.config.apr : COMA_Sys.Instance.apToOneShot);
			component.radius = gunCom.config.radius;
			component.push = gunCom.config.push;
		}
		if (gunCom.config.shotcount > 0)
		{
			gunCom.PlayFlash();
		}
	}

	public virtual bool OnRelive()
	{
		characterCom.PlayMyAnimation("Die", gunCom.name, "0");
		base.hp = HP;
		OnHPChange();
		movePsv = Vector3.zero;
		return false;
	}

	protected virtual void OnOtherGetFlag(COMA_Player com)
	{
	}
}
