using System;
using UnityEngine;

public class COMA_PlayerSync_Tank : COMA_PlayerSync
{
	public Transform character;

	private Vector3 _vLastPos = Vector3.zero;

	private COMA_TankModel _tankModel;

	public Transform characterView;

	private COMA_TankHp _hpComp;

	private bool _bColorSet;

	private string _strTankName;

	private float _fLastHp = 100f;

	private bool _bIgnoreObstacle;

	private new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SWITCHWEAPON, ArmedWithTank);
		base.OnEnable();
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SWITCHWEAPON, base.ReceivePutOnGun);
		COMA_PlayerSyncCharacter component = character.GetComponent<COMA_PlayerSyncCharacter>();
		component._onPlayRecieveAnim = (COMA_PlayerSyncCharacter.onPlayRecievedAnim)Delegate.Combine(component._onPlayRecieveAnim, new COMA_PlayerSyncCharacter.onPlayRecievedAnim(onPlayRecievedAnim));
		_hpComp = GetComponentInChildren<COMA_TankHp>();
		COMA_Tank_SceneController.Instance.AddPlayer(this);
	}

	private new void OnDisable()
	{
		base.OnDisable();
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SWITCHWEAPON, ArmedWithTank);
		COMA_PlayerSyncCharacter component = character.GetComponent<COMA_PlayerSyncCharacter>();
		component._onPlayRecieveAnim = (COMA_PlayerSyncCharacter.onPlayRecievedAnim)Delegate.Remove(component._onPlayRecieveAnim, new COMA_PlayerSyncCharacter.onPlayRecievedAnim(onPlayRecievedAnim));
	}

	private void setHpColor()
	{
		if (!_bColorSet && COMA_PlayerSelf.Instance != null)
		{
			Debug.Log("set team material sync:" + sitIndex + "my:" + COMA_PlayerSelf.Instance.sitIndex);
			_hpComp.setTeamMaterial(TankCommon.isAliance(sitIndex, COMA_PlayerSelf.Instance.sitIndex));
			_bColorSet = true;
		}
	}

	protected new void Update()
	{
		UpdateShadow();
		setHpColor();
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
			if (lerp > 0.99f || (targetPosition[1] - base.transform.position).magnitude > 4f)
			{
				base.transform.position = targetPosition[1];
				lerp = 0f;
				targetPosition.RemoveAt(0);
				targetRotation.RemoveAt(0);
			}
		}
		if (!(_tankModel != null))
		{
		}
	}

	private void ArmedWithTank(COMA_CommandDatas commandDatas)
	{
		Debug.Log("recieve: ArmedWithTank");
		if (commandDatas.dataSender.Id.ToString() != base.gameObject.name)
		{
			return;
		}
		COMA_CD_PlayerSwitchWeapon cOMA_CD_PlayerSwitchWeapon = commandDatas as COMA_CD_PlayerSwitchWeapon;
		if (!(cOMA_CD_PlayerSwitchWeapon.weaponSerialName != "W000"))
		{
			return;
		}
		GameObject gameObject = ArmedWithTank(cOMA_CD_PlayerSwitchWeapon.weaponSerialName);
		COMA_Gun component = gameObject.GetComponent<COMA_Gun>();
		if (component != null)
		{
			bool flag = TankCommon.isAliance(sitIndex, COMA_PlayerSelf.Instance.sitIndex);
			if (component.flashPath.Contains("Fire_Tank_Attack_pfb"))
			{
				string flashPath = component.flashPath.Replace("Fire_Tank_Attack_pfb", (!flag) ? "Fire_Tank_Attack_R_pfb" : "Fire_Tank_Attack_B_pfb");
				component.flashPath = flashPath;
			}
			else if (component.flashPath.Contains("Tank_RawAttack"))
			{
				string flashPath2 = component.flashPath.Replace("Tank_RawAttack", (!flag) ? "Tank_Energy_gun_R_Atk" : "Tank_Energy_gun_B_Atk");
				component.flashPath = flashPath2;
			}
		}
	}

	private GameObject ArmedWithTank(string strTankName)
	{
		_strTankName = strTankName;
		if (gunCom != null)
		{
			UnityEngine.Object.DestroyObject(gunCom.gameObject);
		}
		string text = strTankName.Substring(0, 4);
		Debug.Log("armed with tank:" + strTankName);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Scene/Tank/Prefab/" + strTankName)) as GameObject;
		gameObject.name = strTankName;
		gameObject.transform.parent = characterCom.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gunCom = gameObject.GetComponent<COMA_Gun>();
		gunCom.InitData();
		_tankModel = gameObject.GetComponent<COMA_TankModel>();
		_tankModel.name = id.ToString();
		if (IsHidden)
		{
			HideAdd(0.3f);
		}
		base.transform.rotation = Quaternion.identity;
		character.parent = _tankModel._characterMout;
		character.localPosition = Vector3.zero;
		character.parent.name = id.ToString();
		characterView.parent = _tankModel.transform;
		_tankModel.shotAnimCallback = CharacterCall_Fire;
		return gameObject;
	}

	private void FixedUpdate()
	{
		if (base.transform.parent == null)
		{
			base.transform.parent = COMA_Scene.Instance.playerNodeTrs;
		}
	}

	protected override void ReceiveBuff(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = commandDatas as COMA_CD_PlayerBuff;
		Debug.Log("buffdata:" + cOMA_CD_PlayerBuff.buffState);
		if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GetFlash)
		{
			doGetFlash(cOMA_CD_PlayerBuff);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GetInvincible)
		{
			doGetInvincible(cOMA_CD_PlayerBuff);
		}
		else if (cOMA_CD_PlayerBuff.buffState == COMA_Buff.Buff.GetBulletIgnore)
		{
			Debug.Log("recieve GetBulletIgnore");
			doGetBulletIgnore(cOMA_CD_PlayerBuff);
		}
		else
		{
			base.ReceiveBuff(commandDatas);
		}
	}

	private void OnDrawGizmos()
	{
		if (gunCom != null)
		{
			float num = 0f;
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num, num), gunCom.config.bulletrange);
			Vector3 to = characterCom.viewTrs.position + characterCom.viewTrs.rotation * vector;
			Ray ray = new Ray(characterCom.viewTrs.position, characterCom.viewTrs.rotation * vector);
			ray.origin += ray.direction * 3f;
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, gunCom.config.bulletrange, layerMask))
			{
				to = hitInfo.point;
			}
			Gizmos.color = Color.green;
			Gizmos.DrawLine(gunCom.bulletInitLoc.position, to);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(characterView.position, characterView.position + characterView.forward * 100f);
			Gizmos.color = Color.white;
		}
	}

	public void onPlayRecievedAnim(string strAnimName, float fSpeed)
	{
		if (_tankModel == null)
		{
			return;
		}
		if (strAnimName.Contains("Tank_Fire"))
		{
			_tankModel.playFireAnim(strAnimName);
		}
		if (!strAnimName.Contains("Tank_Death"))
		{
			return;
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Tank_Destroy, base.transform);
		if (fSpeed == 0f)
		{
			_tankModel.OnRelive();
			character.GetComponent<COMA_PlayerSyncCharacter>().PlayMyAnimation("Idle", "W011");
			character.parent = _tankModel._characterMout;
			character.localPosition = Vector3.zero;
			character.localRotation = Quaternion.Euler(90f, 0f, 0f);
			return;
		}
		int nId = 0;
		GameObject gameObject = null;
		switch (strAnimName)
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

	protected override void onHpChange(COMA_CommandDatas commandDatas)
	{
		COMA_CD_PlayerHPSet cOMA_CD_PlayerHPSet = commandDatas as COMA_CD_PlayerHPSet;
		base.hp = cOMA_CD_PlayerHPSet.hp;
		Debug.Log("onHpChange:" + base.hp);
		_hpComp.setHpRatio(base.hp / 100f);
		if (base.hp < _fLastHp)
		{
			RedShine();
		}
		_fLastHp = base.hp;
	}

	private void doGetFlash(COMA_CD_PlayerBuff buffData)
	{
		if (sitIndex == buffData.dataSender.SitIndex)
		{
			COMA_PlayerSelf_Tank cOMA_PlayerSelf_Tank = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Tank;
			if (!TankCommon.isAliance(cOMA_PlayerSelf_Tank.sitIndex, buffData.dataSender.SitIndex) && !COMA_PlayerSelf.Instance.IsInvincible)
			{
				COMA_PlayerSelf.Instance.BuffAdd_GetFlash(0f, 1.5f);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_flash_kill, cOMA_PlayerSelf_Tank.transform);
			}
		}
	}

	private void doGetInvincible(COMA_CD_PlayerBuff buffData)
	{
		if (sitIndex == buffData.dataSender.SitIndex)
		{
			COMA_PlayerSelf_Tank cOMA_PlayerSelf_Tank = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Tank;
			if (TankCommon.isAliance(cOMA_PlayerSelf_Tank.sitIndex, buffData.dataSender.SitIndex))
			{
				cOMA_PlayerSelf_Tank.StartInvincible();
			}
		}
	}

	private bool disableIgnoreObstacle()
	{
		return _bIgnoreObstacle = false;
	}

	private void doGetBulletIgnore(COMA_CD_PlayerBuff buffData)
	{
		if (sitIndex == buffData.dataSender.SitIndex)
		{
			_bIgnoreObstacle = true;
			SceneTimerInstance.Instance.Add(10f, disableIgnoreObstacle);
		}
	}

	public override void CharacterCall_Fire()
	{
		if (gunCom == null || gunCom.bulletInitLoc == null)
		{
			return;
		}
		Debug.Log("CharacterCall_Fire");
		float num = gunCom.config.bulletrange * gunCom.config.precision;
		Vector3 vector = characterCom.transform.position + Vector3.up;
		Quaternion rotation = characterCom.transform.rotation;
		for (int i = 0; i < gunCom.config.shotcount; i++)
		{
			Vector3 vector2 = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num, num), gunCom.config.bulletrange);
			Vector3 vector3 = vector + rotation * vector2;
			Ray ray = new Ray(vector, rotation * vector2);
			LayerMask layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, gunCom.config.bulletrange, layerMask))
			{
				vector3 = hitInfo.point;
			}
			Vector3 position = gunCom.bulletInitLoc.position;
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(vector3 - position);
			identity.eulerAngles = new Vector3(0f, identity.eulerAngles.y, 0f);
			UnityEngine.Object obj = null;
			obj = ((!TankCommon.isAliance(sitIndex, COMA_PlayerSelf.Instance.sitIndex)) ? gunCom.bulletPfbRed : gunCom.bulletPfbBlue);
			GameObject gameObject = UnityEngine.Object.Instantiate(obj, position, identity) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			gameObject.name = gameObject.name.Substring(0, 4);
			COMA_Bullet component = gameObject.GetComponent<COMA_Bullet>();
			component.distance = gunCom.config.bulletrange;
			component.moveSpeed = gunCom.config.bulletspeed;
			component.gravity = gunCom.config.bulletgravity;
			component.ap = gunCom.config.ap;
			component.apr = gunCom.config.apr;
			component.radius = gunCom.config.radius;
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

	private void RedShine()
	{
		HideAdd(1f, 0f, 0f, 1f);
		SceneTimerInstance.Instance.Remove(RecoverRedShine);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_blood_shotRed, RecoverRedShine);
		_tankModel.HideAdd(1f, 0f, 0f, 1f);
	}

	private bool RecoverRedShine()
	{
		HideAdd(1f, 1f, 1f, 1f);
		float num = 75f / 128f;
		_tankModel.HideAdd(num, num, num, 1f);
		return false;
	}
}
