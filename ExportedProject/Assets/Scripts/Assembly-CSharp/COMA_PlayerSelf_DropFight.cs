using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_DropFight : COMA_PlayerSelf
{
	public CameraMode cmrMode;

	public COMA_DropFight_Camera cmrCom;

	private bool bGoldDrop;

	private int getRocketTimes;

	protected new void Start()
	{
		base.Start();
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/restart/restart"), base.transform.position - Vector3.up * 0.7f, base.transform.rotation) as GameObject;
		Object.DestroyObject(obj, 3f);
		speedRun = 4f;
		speedJump = 4f;
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			speedRun *= 1.1f;
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			speedJump *= 1.4f;
		}
		else
		{
			bGoldDrop = true;
		}
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		}
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(base.SendTransform);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		}
		base.OnDisable();
	}

	private void OnLockSth(TNetEventData tEvent)
	{
		RoomLockResCmd.Result result = (RoomLockResCmd.Result)(int)tEvent.data["result"];
		Debug.Log("Value Lock : " + result);
		if (result == RoomLockResCmd.Result.ok)
		{
			string text = (string)tEvent.data["key"];
			if (text == COMA_CommonOperation.Instance.ValueLock_CrazyRocket_Rocket)
			{
				ItemLoot();
			}
		}
	}

	private new void Update()
	{
		UpdateShadow();
		if (COMA_Sys.Instance.bCoverUpdate)
		{
			return;
		}
		if (cmrMode == CameraMode.FPS)
		{
			moveInput = Vector2.zero;
			moveCur = Vector3.zero;
			if (cCtl.isGrounded && movePsv.y <= 0f)
			{
				BaseState();
			}
			else
			{
				movePsv += Physics.gravity * Time.deltaTime;
			}
			Vector3 position = base.transform.position;
			if (Mathf.Abs(position.y) < 0.01f)
			{
				position.y = 0f;
			}
			cmrCom.PosTrs.position = Vector3.Lerp(cmrCom.PosTrs.position, position, 0.1f);
			cmrCom.RotTrs.rotation = Quaternion.Lerp(cmrCom.RotTrs.rotation, characterCom.boneTrs_Waist.rotation, 0.1f);
		}
		else if (cmrMode == CameraMode.GOD)
		{
			Vector3 spdV = new Vector3(moveInput.x, 0f, moveInput.y);
			if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
			{
				moveCur = spdV.normalized * speedRun;
			}
			else
			{
				moveInput = Vector2.zero;
				moveCur = Vector3.zero;
			}
			if (cCtl.isGrounded && movePsv.y <= 0f)
			{
				if (moveInput == Vector2.zero)
				{
					characterCom.PlayMyAnimation("Idle", gunCom.name);
				}
				else
				{
					characterCom.PlayMyAnimation("Run", gunCom.name, "Front");
					TurnAround(spdV);
				}
			}
			else
			{
				movePsv += Physics.gravity * Time.deltaTime;
			}
			cmrCom.PosTrs.position = Vector3.Lerp(cmrCom.PosTrs.position, cmrCom.posGodMode, 0.1f);
			cmrCom.RotTrs.rotation = Quaternion.Lerp(cmrCom.RotTrs.rotation, cmrCom.rotGodMode, 0.1f);
		}
		if (base.IsVenom)
		{
			moveCur *= 0.4f;
		}
		if (buff_flashHit)
		{
			moveCur *= 0.5f;
		}
		if (buff_speedUp)
		{
			moveCur *= 1.5f;
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
	}

	protected override void RotatePlayer(float _x, float _y)
	{
		if (cmrMode != CameraMode.GOD)
		{
			base.RotatePlayer(_x, _y);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!base.IsDead)
		{
			string text = LayerMask.LayerToName(hit.gameObject.layer);
			if (text == "Death")
			{
				ReceiveHurt(COMA_Sys.Instance.apToOneShot, Vector3.zero);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.name.Contains("_"))
		{
			return;
		}
		string[] array = other.name.Split('_');
		int num = int.Parse(array[0]);
		int num2 = int.Parse(array[1]);
		switch (num2)
		{
		case 0:
		{
			COMA_DropFight_SceneController.Instance.DeleteItem(num, num2);
			COMA_CD_DeleteItem cOMA_CD_DeleteItem2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMDELETE) as COMA_CD_DeleteItem;
			cOMA_CD_DeleteItem2.blockIndex = (byte)num;
			cOMA_CD_DeleteItem2.itemIndex = (byte)num2;
			COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteItem2);
			COMA_Network.Instance.LockValue(COMA_CommonOperation.Instance.ValueLock_CrazyRocket_Rocket);
			break;
		}
		case 1:
		{
			goldGet++;
			if (bGoldDrop)
			{
				Random.seed = (int)Time.time * 12121;
				if (Random.Range(0f, 1f) < 0.1f)
				{
					goldGet++;
				}
			}
			COMA_DropFight_SceneController.Instance.DeleteItem(num, num2);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Gold, base.transform, 2f, false);
			COMA_CD_DeleteItem cOMA_CD_DeleteItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMDELETE) as COMA_CD_DeleteItem;
			cOMA_CD_DeleteItem.blockIndex = (byte)num;
			cOMA_CD_DeleteItem.itemIndex = (byte)num2;
			COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteItem);
			break;
		}
		}
	}

	private void ItemLoot()
	{
		IsInvincible = true;
		base.transform.position = COMA_DropFight_SceneController.Instance.firePlatformTrs.position + Vector3.up;
		cmrMode = CameraMode.FPS;
		COMA_DropFight_SceneController.Instance.ChangeMode_Fire();
		PutOnGun("W001_Rocket");
		base.transform.eulerAngles = new Vector3(0f, 100f, 0f);
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.BeGod;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
		getRocketTimes++;
		if (getRocketTimes >= 6)
		{
			COMA_Achievement.Instance.Rocketer++;
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Weapon, base.transform);
		SceneTimerInstance.Instance.Add(3f, UnlockItem_Rocket);
	}

	public bool UnlockItem_Rocket()
	{
		COMA_Network.Instance.UnlockValue(COMA_CommonOperation.Instance.ValueLock_CrazyRocket_Rocket);
		return false;
	}

	public void DropDown()
	{
		if (!(base.transform.parent == null))
		{
			IsInvincible = false;
			if (cmrMode == CameraMode.FPS)
			{
				cmrMode = CameraMode.GOD;
				base.transform.position = COMA_DropFight_SceneController.Instance.GetAvailablePosition();
				localEuler = LOCALEULER;
				characterCom.boneTrs_Waist.localEulerAngles = LOCALEULER;
				COMA_DropFight_SceneController.Instance.ChangeMode_Jump();
				PutDownGun();
			}
		}
	}

	public override bool OnRelive()
	{
		IsInvincible = true;
		base.transform.position = COMA_DropFight_SceneController.Instance.standPlatformTrs[sitIndex].position + Vector3.up;
		cmrMode = CameraMode.FPS;
		characterCom.PlayMyAnimation("Die", gunCom.name, "0");
		base.hp = HP;
		movePsv = Vector3.zero;
		COMA_DropFight_SceneController.Instance.ChangeMode_Fire();
		PutOnGun("W006_Rocket");
		base.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		return false;
	}
}
