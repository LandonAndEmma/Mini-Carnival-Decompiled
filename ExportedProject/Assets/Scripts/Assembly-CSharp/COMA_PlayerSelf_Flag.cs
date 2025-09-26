using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_Flag : COMA_PlayerSelf
{
	private bool isGrounded;

	private Transform curMovePlatformTrs;

	private Vector3 posMovePlatform = Vector3.zero;

	private Vector3 moveTransfer = Vector3.zero;

	private string curWeapon = string.Empty;

	private bool bTransform;

	private bool bFlagSelfLock;

	protected new void Start()
	{
		speedJump = 6f;
		base.Start();
		curWeapon = "W007_Flag";
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			curWeapon = "W005_Flag";
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			curWeapon = "W008_Flag";
		}
		OnRelive();
		SceneTimerInstance.Instance.Add(1f, DelayPutOnWeapon);
	}

	public bool DelayPutOnWeapon()
	{
		PutOnGun(curWeapon);
		return false;
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
			if (text == COMA_CommonOperation.Instance.ValueLock_Flag_Flag)
			{
				ItemLoot();
			}
		}
		else
		{
			bFlagSelfLock = false;
		}
	}

	private void LateUpdate()
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
		if ((cCtl.isGrounded && movePsv.y <= 0f) || curMovePlatformTrs != null)
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
		moveTransfer = Vector3.zero;
		UpdateCheckTarget();
		if (collisionFlags == CollisionFlags.Below)
		{
			bTransform = false;
		}
		UpdateCheckTransfer();
		RotateWaist();
		UpdateFire();
	}

	private void UpdateCheckTarget()
	{
		if (!COMA_Scene.Instance.runingGameOver && !(movePsv.y > 0f) && !bFlagSelfLock)
		{
			Ray ray = new Ray(base.transform.position + Vector3.up * 0.2f, Vector3.down);
			int layerMask = 1 << LayerMask.NameToLayer("Target");
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 0.3f, layerMask))
			{
				bFlagSelfLock = true;
				OnGetFlag();
			}
		}
	}

	private bool UpdateCheckTransfer()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 1.8f, Vector3.down);
		Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2f);
		int layerMask = 1 << LayerMask.NameToLayer("Transfer");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 2f, layerMask))
		{
			Time.fixedDeltaTime = 0.015f;
			if (curMovePlatformTrs != null && curMovePlatformTrs == hitInfo.collider.transform)
			{
				cCtl.Move(curMovePlatformTrs.position - posMovePlatform);
			}
			else
			{
				curMovePlatformTrs = hitInfo.collider.transform;
			}
			posMovePlatform = curMovePlatformTrs.position;
			return true;
		}
		Time.fixedDeltaTime = 0.1f;
		curMovePlatformTrs = null;
		return false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!base.IsDead)
		{
			string text = LayerMask.LayerToName(hit.gameObject.layer);
			if (text == "Death")
			{
				OnRelive();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("---------------------------------OnTriggerEnter : " + other.name);
		if (other.name.StartsWith("Jump"))
		{
			float num = 20f;
			float num2 = num * 2f / 10f;
			float num3 = Random.Range(-20f, 20f);
			float num4 = Random.Range(-20f, 20f);
			Vector3 position = other.transform.position;
			float x = (num3 - position.x) / num2;
			float z = (num4 - position.z) / num2;
			movePsv = new Vector3(x, 20f, z);
			characterCom.PlayMyAnimation("Jump", gunCom.name);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Spring, base.transform);
		}
	}

	private void OnGetFlag()
	{
		if (bTransform)
		{
			COMA_Achievement.Instance.Flyer++;
		}
		COMA_Network.Instance.LockValue(COMA_CommonOperation.Instance.ValueLock_Flag_Flag);
	}

	private void ItemLoot()
	{
		UI_GameCountDownMgr.Instance.PauseCountDown();
		base.transform.eulerAngles = new Vector3(0f, 45f, 0f);
		base.transform.position = COMA_Flag_SceneController.Instance.flagTrs.position;
		InvincibleStart(COMA_TimeAtlas.Instance.flagGetting);
		COMA_Sys.Instance.bCoverUpdate = true;
		COMA_Sys.Instance.bCoverUIInput = true;
		COMA_Camera.Instance.LockCameraToFlag(true);
		PutDownGun();
		COMA_Flag_SceneController.Instance.flagTrs.parent = characterCom.handTrs;
		COMA_Flag_SceneController.Instance.flagTrs.localPosition = Vector3.zero;
		COMA_Flag_SceneController.Instance.flagTrs.localEulerAngles = Vector3.zero;
		characterCom.PlayMyAnimation("Show", string.Empty);
		base.score++;
		SendScore();
		Debug.Log("-> " + base.score);
		SceneTimerInstance.Instance.Add(COMA_TimeAtlas.Instance.flagGetting, OnRelive);
		SceneTimerInstance.Instance.Add(COMA_TimeAtlas.Instance.flagGetting, OnGetFlagEnd);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Banner, base.transform);
		SceneTimerInstance.Instance.Add(COMA_TimeAtlas.Instance.flagGetting + 0.5f, UnlockItem_Flag);
	}

	public bool UnlockItem_Flag()
	{
		bFlagSelfLock = false;
		COMA_Network.Instance.UnlockValue(COMA_CommonOperation.Instance.ValueLock_Flag_Flag);
		return false;
	}

	protected override void OnOtherGetFlag(COMA_Player com)
	{
		if (!COMA_Scene.Instance.settlementCom.gameObject.activeSelf && !(COMA_Flag_SceneController.Instance.flagTrs.parent != COMA_Flag_SceneController.Instance.flagNodeTrs))
		{
			UI_GameCountDownMgr.Instance.PauseCountDown();
			COMA_Sys.Instance.bCoverUpdate = true;
			COMA_Sys.Instance.bCoverUIInput = true;
			COMA_Camera.Instance.LockCameraToFlag(true);
			com.transform.position = COMA_Flag_SceneController.Instance.flagTrs.position;
			COMA_Flag_SceneController.Instance.flagTrs.parent = com.characterCom.handTrs;
			COMA_Flag_SceneController.Instance.flagTrs.localPosition = Vector3.zero;
			COMA_Flag_SceneController.Instance.flagTrs.localEulerAngles = Vector3.zero;
			SceneTimerInstance.Instance.Add(COMA_TimeAtlas.Instance.flagGetting, OnGetFlagEnd);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Banner, base.transform);
		}
	}

	public bool OnGetFlagEnd()
	{
		UI_GameCountDownMgr.Instance.ResumeCountDown();
		COMA_Sys.Instance.bCoverUpdate = false;
		COMA_Sys.Instance.bCoverUIInput = false;
		COMA_Camera.Instance.LockCameraToFlag(false);
		COMA_Flag_SceneController.Instance.flagTrs.parent = COMA_Flag_SceneController.Instance.flagNodeTrs;
		COMA_Flag_SceneController.Instance.flagTrs.localPosition = Vector3.zero;
		COMA_Flag_SceneController.Instance.flagTrs.localEulerAngles = Vector3.zero;
		COMA_Flag_SceneController.Instance.FlagDisappear();
		return false;
	}

	public override bool OnRelive()
	{
		characterCom.PlayMyAnimation("Show", string.Empty, "0");
		PutOnGun(curWeapon);
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		Vector3 position = base.transform.position - Vector3.up * 0.2f;
		GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/restart/restart"), position, base.transform.rotation) as GameObject;
		Object.DestroyObject(obj, 3f);
		return base.OnRelive();
	}

	public override void Transmission()
	{
		base.transform.position = new Vector3(Random.Range(-25f, 25f), 30f, Random.Range(-25f, 25f));
		bTransform = true;
		base.Transmission();
	}
}
