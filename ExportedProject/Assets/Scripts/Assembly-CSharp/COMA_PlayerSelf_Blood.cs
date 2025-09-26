using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_Blood : COMA_PlayerSelf
{
	private string shortWeaponName = string.Empty;

	private string longWeaponName = string.Empty;

	private bool bPickLongWeapon;

	private int clipBullets;

	private int clips;

	private Dictionary<string, int> CLIPNUMBER_Blood = new Dictionary<string, int>();

	private Dictionary<int, int> lst_score2 = new Dictionary<int, int>();

	private float reliveTime = 5f;

	public void AddDeadScore(int id, int n)
	{
		if (!lst_score2.ContainsKey(id))
		{
			lst_score2.Add(id, 0);
		}
		Dictionary<int, int> dictionary2;
		Dictionary<int, int> dictionary = (dictionary2 = lst_score2);
		int key2;
		int key = (key2 = id);
		key2 = dictionary2[key2];
		dictionary[key] = key2 + n;
	}

	public int GetDeadScore(int id)
	{
		if (!lst_score2.ContainsKey(id))
		{
			return 0;
		}
		return lst_score2[id];
	}

	private bool IsShortWeapon(string weaponName)
	{
		switch (weaponName)
		{
		case "W025_Blood":
		case "W026_Blood":
		case "W027_Blood":
			return true;
		default:
			return false;
		}
	}

	private new void Awake()
	{
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
		}
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
		COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(base.SendTransform);
		}
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
		COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		base.OnDisable();
	}

	private void OnLockSth(TNetEventData tEvent)
	{
		if ((int)tEvent.data["result"] == 0)
		{
			string text = (string)tEvent.data["key"];
			if (text == COMA_CommonOperation.Instance.ValueLock_Blood_Cannon)
			{
				ItemLoot();
			}
		}
	}

	private new void Start()
	{
		CLIPNUMBER = 3;
		CLIPNUMBER_Blood.Add("W020", 3);
		CLIPNUMBER_Blood.Add("W021", 3);
		CLIPNUMBER_Blood.Add("W022", 5);
		CLIPNUMBER_Blood.Add("W008", 2);
		base.Start();
		UIIngame_BloodUI.Instance._info.HP = 1f;
		UIIngame_BloodUI.Instance._info.Name = nickname;
		UIIngame_BloodUI.Instance._info.NumKill = 0;
		UIIngame_BloodUI.Instance._info.NumDie = 0;
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 0)
		{
			HP += HP * 0.05f;
			base.hp = HP;
		}
		Transform parent = bornPointTrs.parent;
		GameObject gameObject = Object.Instantiate(COMA_Blood_SceneController.Instance.teamSign[0], parent.position, parent.rotation) as GameObject;
		gameObject.transform.parent = parent;
		Transform transform = null;
		if (parent.name == "BornPoint1")
		{
			transform = parent.parent.FindChild("BornPoint2");
		}
		else if (parent.name == "BornPoint2")
		{
			transform = parent.parent.FindChild("BornPoint1");
		}
		GameObject gameObject2 = Object.Instantiate(COMA_Blood_SceneController.Instance.teamSign[1], transform.position, transform.rotation) as GameObject;
		gameObject2.transform.parent = transform;
		if (COMA_Network.Instance.IsRoomMaster(id))
		{
			SceneTimerInstance.Instance.Add(30f, COMA_Blood_SceneController.Instance.CreateCannon);
		}
	}

	private new void Update()
	{
		UpdateShadow();
		if (COMA_Sys.Instance.bCoverUpdate)
		{
			return;
		}
		Vector3 vector = base.transform.forward * moveInput.y + base.transform.right * moveInput.x;
		if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
		{
			float num = speedRun + gunCom.config.movespeedadd;
			moveCur = vector.normalized * num;
		}
		else
		{
			moveInput = Vector2.zero;
			moveCur = Vector3.zero;
		}
		if (IsGrounded)
		{
			moveCur = Vector3.zero;
		}
		if (cCtl.isGrounded && movePsv.y <= 0f)
		{
			BaseState();
		}
		else
		{
			movePsv += Physics.gravity * Time.deltaTime;
		}
		CollisionFlags collisionFlags = cCtl.Move((moveCur + movePsv) * Time.deltaTime);
		if (collisionFlags == CollisionFlags.Above && movePsv.y > 0f)
		{
			movePsv.y = 0f;
		}
		else
		{
			switch (collisionFlags)
			{
			case CollisionFlags.Sides:
				movePsv = new Vector3(0f, movePsv.y, 0f);
				break;
			case CollisionFlags.Below:
				movePsv = Vector3.zero;
				break;
			}
		}
		RotateWaist();
		UpdateFire();
		if (!IsShortWeapon(gunCom.name))
		{
			clipBullets = clipCount;
			clips = clipNumber;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!base.IsDead)
		{
			string text = LayerMask.LayerToName(hit.gameObject.layer);
			if (text == "Death")
			{
				ReceiveHurt(0, 0, COMA_Sys.Instance.apToOneShot, Vector3.zero);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IsGrounded)
		{
			return;
		}
		Debug.Log("---------------------------------OnTriggerEnter : " + other.name);
		string text = "PFB_Item_Blood_";
		if (other.name == "PFB_Item_Blood_W008")
		{
			COMA_Network.Instance.LockValue(COMA_CommonOperation.Instance.ValueLock_Blood_Cannon);
		}
		else if (other.name.StartsWith(text))
		{
			string text2 = other.name.Substring(text.Length);
			string weaponName = text2 + "_Blood";
			if (IsShortWeapon(weaponName))
			{
				clipNumber = CLIPNUMBER;
			}
			else
			{
				clipNumber = CLIPNUMBER_Blood[text2];
			}
			bPickLongWeapon = true;
			PutOnGun(weaponName);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Get_Weapon, base.transform);
		}
		else if (other.name.StartsWith("PFB_Jumper"))
		{
			float num = 20f;
			float num2 = num * 2f / 10f;
			float num3 = Random.Range(35f, 86f) / num2;
			float num4 = Random.Range(-25f, 6f) / num2;
			if (other.name.EndsWith("1"))
			{
				movePsv = new Vector3(0f - num3, 20f, 0f - num4);
			}
			else if (other.name.EndsWith("2"))
			{
				movePsv = new Vector3(num3, 20f, num4);
			}
			else
			{
				Debug.LogError("No more Jumper!!");
			}
			characterCom.PlayMyAnimation("Jump", gunCom.name);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Spring, base.transform);
		}
	}

	private void ItemLoot()
	{
		string text = "W008";
		string weaponName = text + "_Blood";
		if (IsShortWeapon(weaponName))
		{
			clipNumber = CLIPNUMBER;
		}
		else
		{
			clipNumber = CLIPNUMBER_Blood[text];
		}
		bPickLongWeapon = true;
		PutOnGun(weaponName);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Get_Weapon, base.transform);
		COMA_Blood_SceneController.Instance.DeleteCannon();
		SceneTimerInstance.Instance.Add(5f, DelayToUnlock);
	}

	public bool DelayToUnlock()
	{
		COMA_Network.Instance.UnlockValue(COMA_CommonOperation.Instance.ValueLock_Blood_Cannon);
		return false;
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
			if (!Screen.lockCursor)
			{
				if (tPCInputEvent.code == KeyCode.E)
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
				else if (tPCInputEvent.code == KeyCode.Q && tPCInputEvent.type == EventType.KeyUp)
				{
					UI_SetFire2(0);
				}
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Space)
			{
				UI_Jump();
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha1 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W020_Blood");
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha2 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W021_Blood");
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha3 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W022_Blood");
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha4 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W025_Blood");
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha5 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W026_Blood");
			}
			if (tPCInputEvent.type == EventType.KeyDown && tPCInputEvent.code == KeyCode.Alpha6 && COMA_Sys.Instance.IsSuperAccount)
			{
				PutOnGun("W027_Blood");
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

	protected new void PutDownGun()
	{
		PutOnGun("W000");
	}

	protected new void PutOnGun(string weaponName)
	{
		if (gunCom != null)
		{
			Object.DestroyObject(gunCom.gameObject);
		}
		string text = weaponName.Substring(0, 4);
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Magazine/Weapon/PFB/" + text)) as GameObject;
		gameObject.name = weaponName;
		gameObject.transform.parent = characterCom.handTrs;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gunCom = gameObject.GetComponent<COMA_Gun>();
		if (gunCom.name == "W000")
		{
			COMA_Aim.Instance.SetVisible(false);
			if (COMA_Scene.Instance.magazineCom != null)
			{
				COMA_Scene.Instance.magazineCom.gameObject.SetActive(false);
			}
		}
		else if (IsShortWeapon(gunCom.name))
		{
			gunCom.InitData();
			COMA_Aim.Instance.SetVisible(false);
			COMA_Aim.Instance.maxSize = gunCom.config.aimradiusmax;
			frequencyTime = 0f;
			reloadTime = 0f;
			preshootTime = gunCom.config.preshoottime;
			clipCount = 3;
			if (COMA_Scene.Instance.magazineCom != null)
			{
				COMA_Scene.Instance.magazineCom.gameObject.SetActive(false);
			}
			shortWeaponName = gunCom.name;
			characterCom.PlayMyAnimation("TakeOut_ShortWeapon", gunCom.name);
		}
		else
		{
			gunCom.InitData();
			COMA_Aim.Instance.SetVisible(true);
			COMA_Aim.Instance.maxSize = gunCom.config.aimradiusmax;
			frequencyTime = 0f;
			reloadTime = 0f;
			preshootTime = gunCom.config.preshoottime;
			clipCount = gunCom.config.clipcount;
			if (!bPickLongWeapon)
			{
				clipNumber = clips;
			}
			if (COMA_Scene.Instance.magazineCom != null)
			{
				COMA_Scene.Instance.magazineCom.gameObject.SetActive(true);
				Debug.Log(clipCount + " " + clipNumber);
				UIInGame_BoxMagazineMgr.UIBoxMagazine[] array = new UIInGame_BoxMagazineMgr.UIBoxMagazine[clipNumber + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new UIInGame_BoxMagazineMgr.UIBoxMagazine(clipCount, clipCount);
				}
				COMA_Scene.Instance.magazineCom.InitBoxMagazine(array, 0, COMA_Scene.Instance.bInfinityMode);
			}
			if (!bPickLongWeapon)
			{
				clipCount = clipBullets;
				if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
				{
					COMA_Scene.Instance.magazineCom.RefreshCurBulletNum(clipCount, false);
				}
			}
			bPickLongWeapon = false;
			longWeaponName = gunCom.name;
			characterCom.PlayMyAnimation("TakeOut_LongWeapon", gunCom.name);
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
		Debug.Log(gunCom.name);
		COMA_Blood_SceneController.Instance.weaponSelectCom.SetWeapon(gunCom.name);
	}

	public override void UI_SetFire(int type, float w, float l)
	{
		if (!base.IsDead && !IsGrounded)
		{
			base.UI_SetFire(type, w, l);
		}
	}

	public override void UI_SetFire2(object param)
	{
		if (!base.IsDead && !IsGrounded)
		{
			if (gunCom.name == shortWeaponName && longWeaponName != string.Empty)
			{
				PutOnGun(longWeaponName);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Weapon_Change, base.transform);
			}
			else if (gunCom.name == longWeaponName && shortWeaponName != string.Empty)
			{
				PutOnGun(shortWeaponName);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Weapon_Change, base.transform);
			}
		}
	}

	public override bool ReceiveHurt(int fromID, int bulletID, float bulletAP, Vector3 push)
	{
		Debug.Log("---------------------------------------------------1 ReceiveHurt :" + fromID + " " + bulletID + " " + bulletAP + " " + push);
		if (IsInvincible)
		{
			return false;
		}
		Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(fromID.ToString());
		if (transform != null)
		{
			COMA_PlayerSync_Blood component = transform.GetComponent<COMA_PlayerSync_Blood>();
			if (component.team == team)
			{
				return false;
			}
		}
		bool flag = ReceiveHurt_Blood(fromID, bulletID, bulletAP, push);
		if (flag)
		{
			characterCom.PlayMyAnimation("Die", gunCom.name);
			SceneTimerInstance.Instance.Add(reliveTime, OnRelive);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Player_Death, base.transform);
			if (fromID != 0)
			{
				Transform transform2 = COMA_Scene.Instance.playerNodeTrs.FindChild(fromID.ToString());
				if (transform2 != null)
				{
					COMA_Creation component2 = transform2.GetComponent<COMA_Creation>();
					COMA_Blood_SceneController.Instance.ShowKilledInfo(component2.nickname, component2.rt3DObj.transform.parent.camera.targetTexture);
					COMA_Blood_SceneController.Instance.KillInfo(component2.nickname, nickname);
				}
			}
			else
			{
				COMA_Blood_SceneController.Instance.ShowKilledInfo(nickname, rt3DObj.transform.parent.camera.targetTexture);
				COMA_Blood_SceneController.Instance.KillInfo(nickname, nickname);
			}
			AddDeadScore(id, 1);
			UIIngame_BloodUI.Instance._info.NumDie = GetDeadScore(id);
			COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
			cOMA_CD_PlayerScoreGet.playerId = id;
			cOMA_CD_PlayerScoreGet.curScore = -1;
			cOMA_CD_PlayerScoreGet.addScore = 1;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet);
			if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
			{
				float num = 5f;
				float bulletAP2 = 50f;
				float num2 = 6f;
				for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
				{
					Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
					COMA_Player component3 = child.GetComponent<COMA_Player>();
					if (component3.team != team)
					{
						Vector3 vector = child.position - base.transform.position;
						if (vector.sqrMagnitude < num * num)
						{
							Vector3 push2 = (vector + Vector3.up).normalized * num2;
							OnHitOther(id, component3.id, "B008", bulletAP2, push2);
						}
					}
				}
				GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Cannon_Smitten/Cannon_Smitten"), base.transform.position, Quaternion.identity) as GameObject;
				Object.DestroyObject(gameObject, 3f);
				EffectAudioBehaviour component4 = gameObject.GetComponent<EffectAudioBehaviour>();
				if (component4 != null)
				{
					component4.enabled = false;
				}
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Player_Explode, base.transform);
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.SelfBlast;
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
			}
		}
		else
		{
			RedShine();
		}
		return flag;
	}

	private bool ReceiveHurt_Blood(int fromID, int bulletID, float bulletAP, Vector3 push)
	{
		if (base.IsDead)
		{
			return false;
		}
		if (bulletID == 0 || bulletID == 255 || bulletID == 254 || bulletID == 254)
		{
			GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Cold_Steel/Axe"), base.transform.position, Quaternion.identity) as GameObject;
			Object.DestroyObject(obj, 1f);
		}
		switch (bulletID)
		{
		case 254:
		{
			IsGrounded = true;
			SceneTimerInstance.Instance.Remove(CoverStun);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_blood_stun, CoverStun);
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/stun/stun_01")) as GameObject;
			gameObject.transform.position = base.transform.position + Vector3.up * 1.6f;
			gameObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
			gameObject.transform.parent = base.transform;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_blood_stun);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.Blood_Stun;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Mace_Impact, base.transform);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Mace_Stun, base.transform);
			break;
		}
		case 253:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Axe_Impact, base.transform);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Axe_TurnAround, base.transform);
			break;
		}
		if (bulletID == 9 || bulletID == 19)
		{
			Debug.Log(bulletID);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Impact_Player, base.transform);
		}
		base.hp -= bulletAP;
		OnHPChange();
		Debug.Log("HP : " + base.hp + "/" + HP);
		movePsv = push;
		if (movePsv.sqrMagnitude > 0.1f)
		{
			characterCom.PlayMyAnimation("Jump", gunCom.name);
		}
		return base.IsDead;
	}

	public bool CoverStun()
	{
		IsGrounded = false;
		return false;
	}

	protected override void ReceiveScoreGet(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = commandDatas as COMA_CD_PlayerScoreGet;
		if (cOMA_CD_PlayerScoreGet.curScore < 0)
		{
			Debug.Log("Receive score2 :" + cOMA_CD_PlayerScoreGet.curScore);
			AddDeadScore(cOMA_CD_PlayerScoreGet.playerId, cOMA_CD_PlayerScoreGet.addScore);
			return;
		}
		Debug.Log("Receive score :" + cOMA_CD_PlayerScoreGet.curScore);
		base.ReceiveScoreGet(commandDatas);
		if (cOMA_CD_PlayerScoreGet.playerId == id)
		{
			COMA_Achievement.Instance.BloodKill50++;
			COMA_Achievement.Instance.BloodKill100++;
		}
	}

	public override bool OnRelive()
	{
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/restart/restart")) as GameObject;
		gameObject.transform.position = base.transform.position - Vector3.up * 0.2f;
		Object.DestroyObject(gameObject, 3f);
		base.OnRelive();
		PutDownGun();
		longWeaponName = string.Empty;
		shortWeaponName = string.Empty;
		InvincibleStart(3f);
		return false;
	}

	public override void CharacterCall_Fire2()
	{
		if (gunCom == null || !IsShortWeapon(gunCom.name))
		{
			return;
		}
		clipCount++;
		float num = gunCom.config.bulletrange * gunCom.config.precision;
		Vector3 vector = new Vector3(Random.Range(0f - num, num), Random.Range(0f - num, num), gunCom.config.bulletrange);
		Vector3 vector2 = characterCom.viewTrs.position + characterCom.viewTrs.rotation * vector;
		Vector3 vector3 = base.transform.position + Vector3.up * bodyHeight * base.transform.localScale.x * 0.5f;
		Vector3 direction = vector2 - vector3;
		Ray ray = new Ray(vector3, direction);
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");
		RaycastHit hitInfo;
		if (Physics.SphereCast(ray, 0.5f, out hitInfo, direction.magnitude, layerMask))
		{
			COMA_Creation component = hitInfo.collider.GetComponent<COMA_Creation>();
			Vector3 push = ((component.transform.position - base.transform.position).normalized + Vector3.up) * gunCom.config.push;
			string bulletName = string.Empty;
			if (gunCom.name.StartsWith("W025"))
			{
				bulletName = "B255";
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_LightSabre_Impact, component.transform);
			}
			else if (gunCom.name.StartsWith("W026"))
			{
				bulletName = "B254";
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Mace_Impact, component.transform);
			}
			else if (gunCom.name.StartsWith("W027"))
			{
				bulletName = "B253";
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Axe_Impact, component.transform);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Axe_TurnAround, component.transform);
			}
			else
			{
				Debug.LogError("no short weapon name");
			}
			OnHitOther(id, component.id, bulletName, gunCom.config.ap, push);
			GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Cold_Steel/Axe"), hitInfo.collider.transform.position, Quaternion.identity) as GameObject;
			Object.DestroyObject(obj, 1f);
			if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
			{
				base.hp += gunCom.config.ap;
				OnHPChange();
				Debug.Log("HP Recover : " + base.hp + "/" + HP);
				GameObject obj2 = Object.Instantiate(Resources.Load("Particle/effect/Vampire/Vampire_01_01"), hitInfo.transform.position + Vector3.up * bodyHeight * 0.5f, Quaternion.identity) as GameObject;
				Object.DestroyObject(obj2, 2f);
				GameObject obj3 = Object.Instantiate(Resources.Load("Particle/effect/Vampire/Vampire_02_pfb"), base.transform.position + Vector3.up * bodyHeight * 0.5f, Quaternion.identity) as GameObject;
				Object.DestroyObject(obj3, 2f);
			}
			COMA_PlayerSync_Blood cOMA_PlayerSync_Blood = component as COMA_PlayerSync_Blood;
			if (cOMA_PlayerSync_Blood != null)
			{
				cOMA_PlayerSync_Blood.RedShine();
			}
		}
	}

	public override void CharacterCall_Fire()
	{
		if (!(gunCom == null) && !IsShortWeapon(gunCom.name))
		{
			base.CharacterCall_Fire();
		}
	}

	public void RedShine()
	{
		HideAdd(1f, 0f, 0f, 1f);
		SceneTimerInstance.Instance.Remove(RecoverRedShine);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_blood_shotRed, RecoverRedShine);
	}

	public bool RecoverRedShine()
	{
		HideAdd(1f, 1f, 1f, 1f);
		return false;
	}
}
