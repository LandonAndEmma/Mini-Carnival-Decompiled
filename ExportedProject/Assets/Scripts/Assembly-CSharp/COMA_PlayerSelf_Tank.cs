using System;
using System.Collections.Generic;
using UnityEngine;

public class COMA_PlayerSelf_Tank : COMA_PlayerSelf
{
	private Dictionary<int, int> lst_score2 = new Dictionary<int, int>();

	private bool buff_reduceSpeed;

	private COMA_TankModel _tankModel;

	public Transform _cameraLook;

	private COMA_TankHp _hpComp;

	public ITAudioEvent _TankMoveSound;

	private float fElapsed;

	private Vector3 _vCurVelocity;

	public float _fCurSpeed;

	private string _strTankName;

	private bool _bArmTankSent;

	private bool _bTripleReflectionBullet;

	private bool _bSpeedUpFireFrequency;

	private bool _bIgnoreObstacle;

	public Transform character;

	public Transform characterView;

	private static float _fFrequencyDiscount = 0.5f;

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

	private void onPlayAnim(string strAnim, float fSpeed)
	{
		int nId = 0;
		if (strAnim.Contains("Tank_Fire"))
		{
			_tankModel.playFireAnim(strAnim);
		}
		else if (strAnim.Contains("Tank_Death"))
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank_Destroy, base.transform);
			GameObject gameObject = null;
			switch (strAnim)
			{
			case "Tank_Death01":
				nId = 1;
				gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_TankDead01"), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
				break;
			case "Tank_Death02":
				nId = 2;
				gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_TankDead02"), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
				break;
			case "Tank_Death03":
				nId = 3;
				gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_TankDead02"), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
				break;
			}
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
				UnityEngine.Object.DestroyObject(gameObject, 3f);
			}
			_tankModel.playDeadAnim(nId, fSpeed);
		}
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, SendTransformEx);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
		}
		COMA_PlayerCharacter component = character.GetComponent<COMA_PlayerCharacter>();
		component._onPlayAnim = (COMA_PlayerCharacter.onPlayAnim)Delegate.Combine(component._onPlayAnim, new COMA_PlayerCharacter.onPlayAnim(onPlayAnim));
		_hpComp = GetComponentInChildren<COMA_TankHp>();
		base.OnEnable();
		COMA_Tank_SceneController.Instance.AddPlayer(this);
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(SendTransformEx);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
		}
		COMA_PlayerCharacter component = character.GetComponent<COMA_PlayerCharacter>();
		component._onPlayAnim = (COMA_PlayerCharacter.onPlayAnim)Delegate.Remove(component._onPlayAnim, new COMA_PlayerCharacter.onPlayAnim(onPlayAnim));
		if (COMA_Tank_SceneController.Instance != null && COMA_Scene.Instance != null && COMA_Scene.Instance.settlementCom != null && !COMA_Scene.Instance.settlementCom.gameObject.activeSelf)
		{
			Debug.Log("self tank remove player");
			COMA_Tank_SceneController.Instance.RemovePlayer(this);
		}
		base.OnDisable();
	}

	private new void Start()
	{
		base.Start();
		if (COMA_AudioManager.Instance.bSound)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank_Move")) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			_TankMoveSound = gameObject.GetComponent<ITAudioEvent>();
		}
		else
		{
			_TankMoveSound = null;
		}
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 0)
		{
			ArmedWithTank("W011_Tank");
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			GameObject gameObject2 = ArmedWithTank("W014_Tank");
			COMA_Gun component = gameObject2.GetComponent<COMA_Gun>();
			if (component != null && component.flashPath.Contains("Fire_Tank_Attack_pfb"))
			{
				string flashPath = component.flashPath.Replace("Fire_Tank_Attack_pfb", "Fire_Tank_Attack_B_pfb");
				component.flashPath = flashPath;
			}
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			ArmedWithTank("W019_Tank");
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 3)
		{
			ArmedWithTank("W016_Tank");
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 4)
		{
			GameObject gameObject3 = ArmedWithTank("W013_Tank");
			COMA_Gun component2 = gameObject3.GetComponent<COMA_Gun>();
			if (component2 != null)
			{
				Debug.Log("gunScript.flashPath:" + component2.flashPath);
				if (component2.flashPath.Contains("Tank_RawAttack"))
				{
					Debug.Log("gunScript.flashPath:" + (component2.flashPath = component2.flashPath.Replace("Tank_RawAttack", "Tank_Energy_gun_B_Atk")));
				}
			}
		}
		else
		{
			ArmedWithTank("W011_Tank");
			Debug.LogError("select tank error:" + COMA_CommonOperation.Instance.selectedWeaponIndex);
			Debug.Break();
		}
		localEuler.y = 0f;
	}

	private new void Update()
	{
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			return;
		}
		if (!_bArmTankSent)
		{
			fElapsed += Time.deltaTime;
			if (COMA_Scene.Instance.playerNodeTrs.childCount == TankCommon.nPlayerCount || fElapsed > 3f)
			{
				sendArmedByTank(_strTankName);
			}
		}
		updateVelocity(new Vector2(moveInput.y, moveInput.x));
		UpdateFire();
		if (_hpComp != null)
		{
			_hpComp.setFireRatio(getFrequencyRatio());
		}
	}

	private void updateVelocity(Vector2 vInput)
	{
		if (IsGrounded)
		{
			moveInput = Vector2.zero;
		}
		float num = 0f;
		float num2 = 0f;
		if (buff_speedUp)
		{
			num = _tankModel._fMaxSpeed * 0.3f;
			num2 = _tankModel._fMaxForce * 0.3f;
		}
		if (buff_reduceSpeed)
		{
			num = (0f - _tankModel._fMaxSpeed) * 0.3f;
			num2 = (0f - _tankModel._fMaxForce) * 0.3f;
		}
		Vector3 vector = new Vector3(moveInput.y, 0f, 0f - moveInput.x);
		_vCurVelocity += vector * (_tankModel._fMaxForce + num2) * Time.deltaTime;
		Vector3 vector2 = -_vCurVelocity.normalized * _tankModel._fDragForce;
		_vCurVelocity += vector2 * Time.deltaTime;
		if (_vCurVelocity.magnitude > _tankModel._fMaxSpeed + num)
		{
			_vCurVelocity = _vCurVelocity.normalized * (_tankModel._fMaxSpeed + num);
		}
		if (_vCurVelocity.magnitude < 0.1f && vector.magnitude == 0f)
		{
			_vCurVelocity = Vector3.zero;
		}
		_fCurSpeed = _vCurVelocity.magnitude;
		if (cCtl.isGrounded && movePsv.y <= 0f)
		{
			BaseState();
		}
		else
		{
			movePsv += Physics.gravity * Time.deltaTime;
		}
		CollisionFlags collisionFlags = cCtl.Move((_vCurVelocity + movePsv) * Time.deltaTime);
		if (_fCurSpeed > 0.1f)
		{
			if (_TankMoveSound != null && !_TankMoveSound.isPlaying)
			{
				_TankMoveSound.Trigger();
			}
		}
		else if (_TankMoveSound != null && _TankMoveSound.isPlaying)
		{
			_TankMoveSound.Stop();
		}
	}

	private GameObject ArmedWithTank(string strTankName)
	{
		if (gunCom != null)
		{
			UnityEngine.Object.DestroyObject(gunCom.gameObject);
		}
		string text = strTankName.Substring(0, 4);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Scene/Tank/Prefab/" + strTankName)) as GameObject;
		gameObject.name = strTankName;
		gameObject.transform.parent = characterCom.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gunCom = gameObject.GetComponent<COMA_Gun>();
		_tankModel = gameObject.GetComponent<COMA_TankModel>();
		gunCom.InitData();
		COMA_Aim.Instance.SetVisible(true);
		COMA_Aim.Instance.maxSize = gunCom.config.aimradiusmax;
		frequencyTime = gunCom.config.frequency;
		reloadTime = 0f;
		preshootTime = gunCom.config.preshoottime;
		clipCount = gunCom.config.clipcount;
		_strTankName = strTankName;
		if (IsHidden)
		{
			HideAdd(0.3f);
		}
		resetTankTransform();
		_cameraLook.rotation = Quaternion.Euler(0f, bornPointTrs.rotation.eulerAngles.y, 0f);
		characterView.parent = _tankModel.transform;
		COMA_Tank_Camera cOMA_Tank_Camera = COMA_Camera.Instance as COMA_Tank_Camera;
		cOMA_Tank_Camera.setCameraRotTarget(null);
		_tankModel.shotAnimCallback = CharacterCall_Fire;
		RotatePlayer(0f, 0f);
		_tankModel._tankBase.forward = bornPointTrs.forward;
		return gameObject;
	}

	private void sendArmedByTank(string strTankName)
	{
		if (COMA_Network.Instance.IsConnected())
		{
			COMA_CD_PlayerSwitchWeapon cOMA_CD_PlayerSwitchWeapon = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SWITCHWEAPON) as COMA_CD_PlayerSwitchWeapon;
			cOMA_CD_PlayerSwitchWeapon.weaponSerialName = strTankName;
			Debug.Log("send arm tank:" + strTankName + "command:" + cOMA_CD_PlayerSwitchWeapon.weaponSerialName);
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerSwitchWeapon);
			_bArmTankSent = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
		Debug.Log("-------------------other.name:-------------------" + other.name);
		if (other.name.Contains("Tank_Item") && other.gameObject.layer == 29)
		{
			int nIndex = int.Parse(other.name.Substring(9, 1));
			doUseMagicPower(nIndex);
			destoryCollider(other);
		}
		else if (other.name == "Gold" && other.gameObject.layer == 29)
		{
			destoryCollider(other);
		}
	}

	private void destoryCollider(Collider objCollider)
	{
		Transform parent = objCollider.gameObject.transform.parent;
		if (parent != null)
		{
			COMA_Tank_ItemCreator component = parent.GetComponent<COMA_Tank_ItemCreator>();
			component.destoryItem(true);
		}
	}

	private void doUseMagicPower(int nIndex)
	{
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		int num = UnityEngine.Random.Range(0, 5);
		num = nIndex;
		Debug.Log("Use item:" + num);
		switch (num)
		{
		case 0:
			Debug.Log("use flash!");
			useItemFlash(cOMA_CD_PlayerBuff);
			break;
		case 1:
			useItemInvincible(cOMA_CD_PlayerBuff);
			StartInvincible();
			break;
		case 2:
			_bIgnoreObstacle = true;
			cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.GetBulletIgnore;
			SceneTimerInstance.Instance.Remove(disableIgnoreObstacle);
			SceneTimerInstance.Instance.Add(10f, disableIgnoreObstacle);
			break;
		case 3:
			_bTripleReflectionBullet = true;
			SceneTimerInstance.Instance.Remove(disableTripleReflection);
			SceneTimerInstance.Instance.Add(10f, disableTripleReflection);
			break;
		case 4:
			_bSpeedUpFireFrequency = true;
			SceneTimerInstance.Instance.Remove(disableSpeedUpFireFequency);
			SceneTimerInstance.Instance.Add(10f, disableSpeedUpFireFequency);
			break;
		}
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
	}

	private void useItemFlash(COMA_CD_PlayerBuff buffData)
	{
		buffData.buffState = COMA_Buff.Buff.GetFlash;
	}

	private void useItemInvincible(COMA_CD_PlayerBuff buffData)
	{
		buffData.buffState = COMA_Buff.Buff.GetInvincible;
	}

	private void sendSpeedBuffGot(bool bSpeedUp)
	{
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.SpeedUp;
		cOMA_CD_PlayerBuff.buffName = ((!bSpeedUp) ? "Down" : "Up");
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
	}

	private bool disableTripleReflection()
	{
		return _bTripleReflectionBullet = false;
	}

	private bool disableSpeedUpFireFequency()
	{
		return _bSpeedUpFireFrequency = false;
	}

	private bool disableIgnoreObstacle()
	{
		return _bIgnoreObstacle = false;
	}

	public void getFlash()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/flash_kill/flash_kill"), base.transform.position, base.transform.rotation) as GameObject;
		UnityEngine.Object.DestroyObject(gameObject, 2f);
		gameObject.transform.parent = base.transform;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_flash_kill, base.transform);
		GroundedStart(2f);
	}

	public override void OnHitOther(int fromID, int toID, string bulletName, float bulletAP, Vector3 push)
	{
		Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(toID.ToString());
		if (transform != null)
		{
			COMA_PlayerSync component = transform.GetComponent<COMA_PlayerSync>();
			if (component != null && !component.IsInvincible && !TankCommon.isAliance(sitIndex, component.sitIndex))
			{
				base.OnHitOther(fromID, toID, bulletName, bulletAP, push);
			}
		}
	}

	private void resetTankTransform()
	{
		base.transform.rotation = Quaternion.identity;
		if (bornPointTrs != null)
		{
			_tankModel._turret.localRotation = Quaternion.Euler(0f, 0f, bornPointTrs.rotation.eulerAngles.y);
			base.transform.position = bornPointTrs.position;
		}
		character.parent = _tankModel._characterMout;
		character.localPosition = Vector3.zero;
		character.localRotation = Quaternion.Euler(90f, 0f, 0f);
	}

	public override bool OnRelive()
	{
		base.OnRelive();
		COMA_Sys.Instance.bCoverUIInput = false;
		OnHPChange();
		_tankModel.OnRelive();
		characterCom.PlayMyAnimation("Idle", gunCom.name);
		resetTankTransform();
		RotatePlayer(0f, 0f);
		StartInvincible();
		return false;
	}

	public void StartInvincible()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Rebirth, base.transform);
		InvincibleStart(3f);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Tank_Invincible/Invincible_pfb"), base.transform.position + Vector3.up * 0.5f, base.transform.rotation) as GameObject;
		UnityEngine.Object.Destroy(gameObject, 3f);
		gameObject.transform.parent = base.transform;
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.TankEffect;
		cOMA_CD_PlayerBuff.buffName = "Invinceble";
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
	}

	protected override void RotatePlayer(float _x, float _y)
	{
	}

	private void OnDrawGizmos()
	{
	}

	public void RedShine()
	{
		HideAdd(1f, 0f, 0f, 1f);
		SceneTimerInstance.Instance.Remove(RecoverRedShine);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_blood_shotRed, RecoverRedShine);
		_tankModel.HideAdd(1f, 0f, 0f, 1f);
	}

	public bool RecoverRedShine()
	{
		HideAdd(1f, 1f, 1f, 1f);
		float num = 75f / 128f;
		_tankModel.HideAdd(num, num, num, 1f);
		return false;
	}

	public override bool ReceiveHurt(int fromID, int bulletID, float bulletAP, Vector3 push)
	{
		RedShine();
		float num = base.hp;
		bool result = ReceiveHurtEx(fromID, bulletID, bulletAP, Vector3.zero, false);
		Debug.Log("push:" + push);
		float num2 = base.hp;
		if (num > 0f && num2 <= 0f)
		{
			EnemyScore(fromID);
			COMA_Tank_SceneController.Instance._nDieCount++;
			Transform transform = COMA_Scene.Instance.playerNodeTrs.FindChild(fromID.ToString());
			if (transform != null)
			{
				transform.GetComponent<COMA_Creation>().score++;
			}
		}
		push.y = 0f;
		Vector3 vector = character.worldToLocalMatrix * push;
		float num3 = Vector3.Angle(vector, Vector3.forward);
		string empty = string.Empty;
		if (num3 <= 45f)
		{
			empty = "B";
			Debug.Log("anim back hit!");
		}
		else if (num3 <= 135f)
		{
			if (vector.x >= 0f)
			{
				empty = "R";
				Debug.Log("anim right hit");
			}
			else
			{
				empty = "L";
				Debug.Log("anim left hit");
			}
		}
		else
		{
			empty = "F";
			Debug.Log("anim forward hit ");
		}
		characterCom.PlayMyAnimation("TankHurt", gunCom.name, empty);
		Debug.Log(string.Concat("pushlocal:", vector, "delta:", Vector3.Angle(vector, Vector3.forward)));
		Debug.Log(string.Concat("vPushLocal:", vector, "|", Quaternion.Euler(vector).eulerAngles));
		return result;
	}

	public bool ReceiveHurtEx(int fromID, int bulletID, float bulletAP, Vector3 push, bool bNeedJump)
	{
		if (base.IsDead)
		{
			return false;
		}
		if (base.IsFrozen)
		{
			int num = 0;
		}
		if (IsInvincible)
		{
			return false;
		}
		if (bulletID == 14 && !base.IsVenom)
		{
			base.IsVenom = true;
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_venom, base.VenomRecover);
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankFire_R"), base.transform.position + Vector3.up, base.transform.rotation) as GameObject;
			Debug.Log("fireobj pos:" + gameObject.transform.position);
			gameObject.transform.parent = characterCom.transform;
			UnityEngine.Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_venom);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.TankEffect;
			cOMA_CD_PlayerBuff.buffName = "Fire";
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
		}
		switch (bulletID)
		{
		case 16:
			if (!base.IsFrozen)
			{
				icefromID = fromID;
				iceAP = bulletAP;
				icePush = push.magnitude;
				if (base.hp - bulletAP > 0f)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankIce"), base.transform.position, base.transform.rotation) as GameObject;
					gameObject3.transform.parent = base.transform;
					UnityEngine.Object.DestroyObject(gameObject3, COMA_Buff.Instance.lastTime_ice);
					SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_ice, FrozenBroken);
					COMA_CD_PlayerBuff cOMA_CD_PlayerBuff3 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
					cOMA_CD_PlayerBuff3.buffState = COMA_Buff.Buff.TankEffect;
					cOMA_CD_PlayerBuff3.buffName = "Ice";
					COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff3);
				}
			}
			break;
		case 11:
		case 19:
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_TankDmgSmoke"), base.transform.position + Vector3.up * 1.5f, base.transform.rotation) as GameObject;
			gameObject2.transform.parent = characterCom.transform;
			gameObject2.transform.localPosition = -Vector3.forward;
			UnityEngine.Object.DestroyObject(gameObject2, 3f);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff2.buffState = COMA_Buff.Buff.TankEffect;
			cOMA_CD_PlayerBuff2.buffName = "Smoke";
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff2);
			break;
		}
		}
		bool flag = base.IsFrozen;
		base.IsFrozen = false;
		base.hp -= bulletAP;
		base.IsFrozen = flag;
		OnHPChange();
		Debug.Log("HP : " + base.hp + "/" + HP);
		movePsv = push;
		if (bulletID == 16 && !base.IsDead && !base.IsFrozen)
		{
			base.IsFrozen = true;
		}
		Debug.Log("------------hp:" + base.hp);
		if (base.IsDead)
		{
			if (IsHidden)
			{
				HideTerminate();
			}
			Debug.Log("------------play dead animation");
			base.IsFrozen = false;
			characterCom.PlayMyAnimation("Die", gunCom.name);
			base.IsFrozen = flag;
			SceneTimerInstance.Instance.Add(3f, OnRelive);
			COMA_Sys.Instance.bCoverUIInput = true;
			AddDeadScore(id, 1);
			COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
			cOMA_CD_PlayerScoreGet.playerId = id;
			cOMA_CD_PlayerScoreGet.curScore = -1;
			cOMA_CD_PlayerScoreGet.addScore = 1;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet);
			if (fromID == -1)
			{
				COMA_Player[] componentsInChildren = COMA_Scene.Instance.playerNodeTrs.GetComponentsInChildren<COMA_Player>();
				COMA_Player[] array = componentsInChildren;
				foreach (COMA_Player cOMA_Player in array)
				{
					if (!TankCommon.isAliance(sitIndex, cOMA_Player.sitIndex))
					{
						COMA_CD_PlayerScoreGet cOMA_CD_PlayerScoreGet2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_SCOREGET) as COMA_CD_PlayerScoreGet;
						cOMA_CD_PlayerScoreGet2.playerId = cOMA_Player.id;
						cOMA_CD_PlayerScoreGet2.addScore = 1;
						cOMA_CD_PlayerScoreGet2.curScore = 100;
						COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerScoreGet2);
						break;
					}
				}
			}
			return true;
		}
		return false;
	}

	public override bool FrozenBroken()
	{
		base.FrozenBroken();
		IsInvincible = false;
		Debug.Log("ice broken ");
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Ice_Crack, base.transform);
		return false;
	}

	protected override void OnHPChange()
	{
		base.OnHPChange();
		_hpComp.setHpRatio(base.hp / 100f);
	}

	private void EnemyScore(int nFromId)
	{
		int nScore = 1;
		int nTeamIndex = ((TankCommon.getTeamIndex(COMA_PlayerSelf.Instance.sitIndex) == 0) ? 1 : 0);
		COMA_Tank_SceneController.Instance.addScore(nTeamIndex, nScore);
	}

	protected bool SendTransformEx()
	{
		COMA_CD_PlayerTransform cOMA_CD_PlayerTransform = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_TRANSFORM) as COMA_CD_PlayerTransform;
		cOMA_CD_PlayerTransform.position = base.transform.position;
		cOMA_CD_PlayerTransform.rotation = Quaternion.Euler(0f, _tankModel._turret.localRotation.eulerAngles.z, 0f);
		cOMA_CD_PlayerTransform.btElevation = (byte)localEuler.y;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerTransform);
		return true;
	}

	public override void UI_SetFire(int type, float w, float l)
	{
		if ((double)Mathf.Abs(w) > 0.1 || (double)Mathf.Abs(l) > 0.1)
		{
			if (w == 0f)
			{
				w = 0.001f;
			}
			float num = 0f;
			num = ((!(w < 0f)) ? (Mathf.Atan((0f - l) / w) * 57.29578f + 180f) : (Mathf.Atan((0f - l) / w) * 57.29578f));
			_tankModel._turret.eulerAngles = new Vector3(270f, num, 0f);
		}
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

	protected float getFrequencyRatio()
	{
		float num = gunCom.config.frequency;
		if (_bSpeedUpFireFrequency)
		{
			num *= _fFrequencyDiscount;
		}
		float num2 = frequencyTime / num;
		num2 = ((!(num2 < 0f)) ? num2 : 0f);
		return 1f - num2;
	}

	protected override void Fire()
	{
		if (gunCom.name == "W003")
		{
			gunCom.EnergyGunAccumulate(true);
		}
		characterCom.PlayMyAnimation("Attack", gunCom.name, "0");
		characterCom.PlayMyAnimation("Attack", gunCom.name);
		SceneTimerInstance.Instance.Remove(base.ToHoldState);
		SceneTimerInstance.Instance.Add(1.5f, base.ToHoldState);
		COMA_Aim.Instance.Magnify(gunCom.config.aimmagnify);
		frequencyTime = gunCom.config.frequency;
		if (_bSpeedUpFireFrequency)
		{
			frequencyTime *= _fFrequencyDiscount;
		}
		clipCount--;
		preshootTime = gunCom.config.preshoottime;
		if (COMA_Scene.Instance.magazineCom != null && COMA_Scene.Instance.magazineCom.gameObject.activeSelf)
		{
			COMA_Scene.Instance.magazineCom.RefreshCurBulletNum(clipCount, false);
		}
	}

	public override void CharacterCall_Fire()
	{
		if (gunCom.name == "W003")
		{
			gunCom.EnergyGunAccumulate(false);
		}
		float num = gunCom.config.bulletrange * gunCom.config.precision;
		Vector3 vector = characterCom.transform.position + Vector3.up;
		Quaternion rotation = characterCom.transform.rotation;
		for (int i = 0; i < gunCom.config.shotcount; i++)
		{
			Vector3 vector2 = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num, num), gunCom.config.bulletrange);
			Vector3 vector3 = vector + rotation * vector2;
			Ray ray = new Ray(vector, rotation * vector2);
			ray.origin += ray.direction * 3f;
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, gunCom.config.bulletrange, layerMask))
			{
				vector3 = hitInfo.point;
			}
			Vector3 position = gunCom.bulletInitLoc.position;
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(vector3 - position);
			identity.eulerAngles = new Vector3(0f, identity.eulerAngles.y, 0f);
			Ray ray2 = new Ray(character.position, character.rotation * Vector3.forward);
			Gizmos.color = Color.black;
			LayerMask layerMask2 = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2, 2f, layerMask2))
			{
				position -= rotation * Vector3.forward * 2f;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(gunCom.bulletPfbBlue, position, identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			gameObject.name = gameObject.name.Substring(0, 4);
			COMA_Bullet component = gameObject.GetComponent<COMA_Bullet>();
			component.fromPlayerCom = this;
			component.distance = gunCom.config.bulletrange;
			component.moveSpeed = gunCom.config.bulletspeed;
			component.gravity = gunCom.config.bulletgravity;
			component.ap = ((!buff_oneShot) ? gunCom.config.ap : COMA_Sys.Instance.apToOneShot);
			component.apr = ((!buff_oneShot) ? gunCom.config.apr : COMA_Sys.Instance.apToOneShot);
			component.radius = gunCom.config.radius;
			component.push = gunCom.config.push;
			if (_bTripleReflectionBullet)
			{
				if (component.name == "B013_R" || component.name == "B013_B")
				{
					component._nReflectLeft = 4;
				}
				else
				{
					component._nReflectLeft = 3;
				}
			}
			component._bIngoreObstacle = _bIgnoreObstacle;
		}
		if (gunCom.config.shotcount > 0)
		{
			gunCom.PlayFlash();
		}
		if (_strTankName == "W011_Tank" || _strTankName == "W019_Tank")
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Tank01_Fire, base.transform);
		}
		else if (_strTankName == "W014_Tank")
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Tank02_Fire, base.transform);
		}
		else if (_strTankName == "W016_Tank")
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Tank03_Fire, base.transform);
		}
		else if (_strTankName == "W013_Tank")
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_energy_gun_BulletFly, base.transform);
		}
	}
}
