using UnityEngine;

public class COMA_PlayerSelf_Run : COMA_PlayerSelf
{
	[SerializeField]
	private COMA_Run_SmokeEffect runSmokeEffect;

	public COMA_Run_Camera cmrCom;

	private bool onGround;

	private float itemAccelSpeed = 15f;

	private bool bAccelerating;

	private float boardAccelSpeed = 12f;

	private float boardAccelTime = 1.4f;

	private bool bBouncing;

	private float bounceAccelSpeed = 18f;

	private float bounceUpSpeed;

	private bool buff_goldAdsorb;

	private float adsorbDistance = 3f;

	private float adsorbLastTime = 5f;

	private bool buff_reduceSpeed;

	private float reduceSpeedLastTime = 5f;

	private Vector3 tarUp = Vector3.up;

	private float curSpeed = 4f;

	private float curAccel;

	private bool bFinished;

	private bool bGoldDrop;

	private bool bCrazy;

	private int avoidMine;

	private int avoidGlue;

	private bool bRideRocket;

	private bool bDropBack;

	private int nMineId;

	private float fCurResistAlpha = 0.2f;

	private float fPreGravity = -9.8f;

	private float fRocketFly;

	public bool IsFinishedGame
	{
		get
		{
			return bFinished;
		}
	}

	protected new void Start()
	{
		gravity = -9.8f;
		speedRun = 7f;
		speedJump = 5f;
		nMineId = 0;
		bounceUpSpeed = 3.4f * speedJump * speedRun / bounceAccelSpeed;
		curAccel = speedRun * 0.5f;
		CLIPNUMBER = 0;
		base.Start();
		base.transform.position = bornPointTrs.position;
		Debug.Log("--------COMA_PlayerSelf_Run---------   :" + base.transform.position.y);
		base.transform.rotation = bornPointTrs.rotation;
		OnRelive();
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			int num = Random.Range(0, 11);
			int useCount = 1;
			switch (num)
			{
			case 2:
				useCount = 3;
				break;
			case 10:
				useCount = 5;
				break;
			}
			UIInGame_PropsBox.Instance.GetProps(num, useCount);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			bCrazy = true;
		}
		else
		{
			bGoldDrop = true;
		}
		runSmokeEffect.SetSmokeActive(false);
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
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
		}
		base.OnDisable();
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			return true;
		}
		base.HandleMessage(msg);
		if (msg._nMsgId == 30000)
		{
			TPCInputEvent tPCInputEvent = (TPCInputEvent)msg._pExtraInfo;
			Debug.Log(string.Concat("Key: ", tPCInputEvent.code, " ", tPCInputEvent.type));
			if (tPCInputEvent.type == EventType.KeyDown)
			{
				if (tPCInputEvent.code == KeyCode.Z)
				{
					UseItem(0);
				}
				if (tPCInputEvent.code == KeyCode.X)
				{
					UseItem(1);
				}
				if (tPCInputEvent.code == KeyCode.C)
				{
					UseItem(2);
				}
				if (tPCInputEvent.code == KeyCode.V)
				{
					UseItem(3);
				}
				if (tPCInputEvent.code == KeyCode.B)
				{
					UseItem(4);
				}
				if (tPCInputEvent.code == KeyCode.N)
				{
					UseItem(5);
				}
				if (tPCInputEvent.code == KeyCode.M)
				{
					UseItem(6);
				}
				if (tPCInputEvent.code == KeyCode.K)
				{
					UseItem(7);
				}
				if (tPCInputEvent.code == KeyCode.I)
				{
					UseItem(8);
				}
				if (tPCInputEvent.code == KeyCode.P)
				{
					UseItem(9);
				}
				if (tPCInputEvent.code == KeyCode.O)
				{
					UseItem(10);
				}
			}
		}
		return true;
	}

	public void DropBackToMainMenu(string param)
	{
		COMA_NetworkConnect.Instance.BackFromScene();
	}

	private new void Update()
	{
		UpdateShadow();
		if (!COMA_Sys.Instance.bRealStartGame)
		{
			cmrCom.PosTrs.position = Vector3.Lerp(cmrCom.PosTrs.position, base.transform.position, 0.1f);
		}
		else if (base.transform.position.y < -50f)
		{
			if (!bDropBack)
			{
				bDropBack = true;
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(102);
				uI_MsgBox.AddProceYesHandler(DropBackToMainMenu);
			}
		}
		else
		{
			if (COMA_Sys.Instance.bCoverUpdate)
			{
				return;
			}
			if (bFinished)
			{
				if (curSpeed > 0f)
				{
					curSpeed -= speedRun * Time.deltaTime;
					base.transform.position += base.transform.forward * curSpeed * Time.deltaTime;
				}
				else
				{
					characterCom.PlayMyAnimation("Idle", gunCom.name);
				}
				cmrCom.PosTrs.position = Vector3.Lerp(cmrCom.PosTrs.position, base.transform.position, 0.1f);
				cmrCom.PosTrs.rotation = Quaternion.Lerp(cmrCom.PosTrs.rotation, new Quaternion(0f, 1f, 0f, 0f), 0.05f);
				return;
			}
			if (!COMA_Sys.Instance.bCoverUIInput)
			{
				moveInput.y = 1f;
				if (bCrazy)
				{
					bCrazy = false;
					UseItem(1);
					UseItem(0);
				}
			}
			else if (bBouncing || bRideRocket)
			{
				moveInput.y = 1f;
			}
			if (base.IsFrozen)
			{
				moveInput = Vector2.zero;
			}
			if (IsGrounded)
			{
				moveInput = Vector2.zero;
			}
			Vector3 vector = base.transform.forward * moveInput.y + base.transform.right * moveInput.x;
			if (bBouncing)
			{
				vector = Vector3.forward * moveInput.y;
			}
			if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
			{
				if (buff_speedUp)
				{
					if (curSpeed < itemAccelSpeed)
					{
						curSpeed = itemAccelSpeed;
					}
				}
				else if (bAccelerating)
				{
					curSpeed = boardAccelSpeed;
				}
				else if (bBouncing)
				{
					if (curSpeed < bounceAccelSpeed)
					{
						curSpeed = bounceAccelSpeed;
						Debug.Log("*****-------Run CurSpeed=" + curSpeed);
					}
				}
				else if (!bRideRocket)
				{
					if (curSpeed > speedRun)
					{
						curSpeed = Mathf.Max(curSpeed - curAccel * Time.deltaTime, speedRun);
					}
					else if (curSpeed < speedRun)
					{
						curSpeed = Mathf.Min(curSpeed + curAccel * Time.deltaTime, speedRun);
					}
				}
				moveCur = vector.normalized * curSpeed;
				if (buff_reduceSpeed)
				{
					moveCur /= 2f;
				}
			}
			else
			{
				moveInput = Vector2.zero;
				moveCur = Vector3.zero;
			}
			if (onGround)
			{
				if (moveInput == Vector2.zero)
				{
					if (!IsGrounded)
					{
						characterCom.PlayMyAnimation("Idle", gunCom.name);
					}
				}
				else if (buff_speedUp || bAccelerating)
				{
					characterCom.PlayMyAnimation("Run00_fast", string.Empty);
				}
				else if (bBouncing)
				{
					characterCom.PlayMyAnimation("Jump00_fly01", gunCom.name);
				}
				else if (moveInput.x > 0.1f && gunCom.name == "W000")
				{
					characterCom.PlayMyAnimation("Run", gunCom.name, "Front_R");
				}
				else if (moveInput.x < -0.1f && gunCom.name == "W000")
				{
					characterCom.PlayMyAnimation("Run", gunCom.name, "Front_L");
				}
				else
				{
					characterCom.PlayMyAnimation("Run", gunCom.name, "Front");
				}
				if (moveInput != Vector2.zero)
				{
					runSmokeEffect.SetSmokeActive(true);
				}
			}
			else
			{
				runSmokeEffect.SetSmokeActive(false);
				if (!bRideRocket)
				{
					characterCom.PlayMyAnimation("Jump", gunCom.name);
					movePsv += Vector3.up * gravity * Time.deltaTime;
				}
				else
				{
					movePsv += Vector3.up * gravity * Time.deltaTime;
					if (movePsv.y <= 0f)
					{
						movePsv.y = 0f;
					}
				}
			}
			if (bAccelerating)
			{
			}
			CollisionFlags collisionFlags = cCtl.Move((moveCur + movePsv) * Time.deltaTime);
			if (collisionFlags == CollisionFlags.Above && movePsv.y > 0f)
			{
				movePsv.y = 0f;
			}
			else if (collisionFlags != CollisionFlags.Sides)
			{
			}
			if (!bRideRocket)
			{
				CheckGround();
			}
			CheckFrontObject();
			if (buff_goldAdsorb)
			{
				COMA_Run_SceneController.Instance.GoldAdsorb(base.transform);
			}
			cmrCom.PosTrs.position = Vector3.Lerp(cmrCom.PosTrs.position, base.transform.position, 0.1f);
			UpdateFire();
		}
	}

	public void ColliderBounceBoard()
	{
		gravity = -10.8f;
		bBouncing = true;
		COMA_Sys.Instance.bCoverUIInput = true;
		base.transform.up = Vector3.up;
		base.transform.localEulerAngles = Vector3.zero;
		movePsv = base.transform.up * bounceUpSpeed;
		float num = Mathf.Abs(2f * bounceUpSpeed / gravity);
		Debug.Log("bBouncingTime=" + num);
		SceneTimerInstance.Instance.Add(num, BouncingEnd);
		characterCom.PlayMyAnimation("Jump00_fly01", gunCom.name);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Spring, base.transform);
		cmrCom.cmr.animation.Play("Camera_PushClose");
	}

	public void ColliderFire(int type)
	{
		InvincibleStart(3f);
		Debug.Log("Collider:Fire--------");
		StopMove();
		StopPsvMove();
		characterCom.PlayMyAnimation("Makeup01", gunCom.name);
		IsGrounded = true;
		SceneTimerInstance.Instance.Add(1f, CoverStun);
		GameObject gameObject = null;
		if (type == 0)
		{
			ColorStart(0.1f, 0.1f, 0.1f, 1f);
			gameObject = Object.Instantiate(Resources.Load("Particle/effect/Combustion/Combustion_01")) as GameObject;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Fire_burst, base.transform);
			gameObject.transform.position = base.transform.position + new Vector3(0f, 0.5f, 0f);
		}
		else
		{
			gameObject = Object.Instantiate(Resources.Load("Particle/effect/Fire/Flash_kill_01")) as GameObject;
			gameObject.transform.position = base.transform.position + new Vector3(0f, 1f, 0f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_flash_kill, base.transform);
		}
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, 0.8f);
	}

	private void CheckGround()
	{
		bool flag = false;
		onGround = false;
		Ray ray = new Ray(base.transform.position + base.transform.up * 1f, -base.transform.up);
		Ray ray2 = new Ray(base.transform.position + Vector3.up * 1f, -Vector3.up);
		int layerMask = 1 << LayerMask.NameToLayer("Death");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray2, out hitInfo, 5f, layerMask) && hitInfo.distance < 2.1f && Vector3.Dot(movePsv, base.transform.up) <= 0f)
		{
			Transform transform = hitInfo.collider.transform.root.FindChild("StartPoint");
			if (transform == null)
			{
				COMA_Run_StartPoint componentInChildren = hitInfo.collider.transform.root.GetComponentInChildren<COMA_Run_StartPoint>();
				transform = componentInChildren.transform;
			}
			Debug.Log("----StartPoint:" + transform.gameObject.name + " Child Count:" + transform.childCount);
			float num = float.PositiveInfinity;
			Vector3 position = base.transform.position;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				float num2 = Mathf.Abs(child.position.z - base.transform.position.z);
				if (num > num2)
				{
					num = num2;
					position = child.position;
				}
			}
			base.transform.position = position;
			base.transform.localEulerAngles = Vector3.zero;
			StopMove();
			StopPsvMove();
			CoverStun();
			CoverStunAnim();
			Debug.Log(base.transform.position);
			flag = true;
		}
		Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10f);
		int layerMask2 = 1 << LayerMask.NameToLayer("Collision");
		RaycastHit hitInfo2;
		if (!flag && Physics.Raycast(ray, out hitInfo2, 1.5f, layerMask2))
		{
			if (hitInfo2.distance < 1.2f && Vector3.Dot(movePsv, base.transform.up) <= 0f)
			{
				onGround = true;
				StopPsvMove();
				base.transform.position = hitInfo2.point;
			}
			tarUp = hitInfo2.normal;
			if (hitInfo2.collider.tag == "Accelerator" && base.transform.FindChild("PFB_Buff_SpeedUp") == null)
			{
				bAccelerating = true;
				SceneTimerInstance.Instance.Add(boardAccelTime, AcceleratingEnd);
				characterCom.PlayMyAnimation("Run00_fast", string.Empty);
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_SpeedUp"), base.transform.position, base.transform.rotation) as GameObject;
				gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
				gameObject.transform.parent = base.transform;
				Object.DestroyObject(gameObject, boardAccelTime);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Shoe_JiaSu, base.transform);
			}
			if (!bBouncing && hitInfo2.collider.tag == "Bounce")
			{
				ColliderBounceBoard();
			}
		}
		base.transform.up = Vector3.Lerp(base.transform.up, tarUp, 0.1f);
	}

	private void ColliderDizzy()
	{
		base.transform.position += new Vector3(0f, 0f, -1f);
		StopMove();
		StopPsvMove();
		characterCom.PlayMyAnimation("Dizzy", gunCom.name);
		SceneTimerInstance.Instance.Add(1.6f, CoverStunAnim);
		IsGrounded = true;
		SceneTimerInstance.Instance.Add(2f, CoverStun);
		InvincibleStart(4f);
		SceneTimerInstance.Instance.Add(2f, ResistStun);
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/stun/stun_01")) as GameObject;
		gameObject.transform.position = base.transform.position + new Vector3(0f, 1.7f, -0.5f);
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, 1.6f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_stun, base.transform);
	}

	private void CheckFrontObject()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 0.5f, base.transform.forward);
		int layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit hitInfo;
		if (!IsGrounded && !IsInvincible && Physics.Raycast(ray, out hitInfo, 1.5f, layerMask))
		{
			ColliderDizzy();
			COMA_Run_RoadCollider component = hitInfo.collider.gameObject.GetComponent<COMA_Run_RoadCollider>();
			if (component != null)
			{
				component.NotifyCollideHitPlayer();
			}
		}
	}

	public bool AcceleratingEnd()
	{
		bAccelerating = false;
		characterCom.PlayMyAnimation("Run00_fast", string.Empty, "0");
		return false;
	}

	public bool BouncingEnd()
	{
		gravity = -9.8f;
		bBouncing = false;
		COMA_Sys.Instance.bCoverUIInput = false;
		characterCom.PlayMyAnimation("Jump00_fly01", gunCom.name, "0");
		cmrCom.cmr.animation.Play("Camera_PushFarther");
		curSpeed = speedRun;
		return false;
	}

	public override void UI_Jump()
	{
		if (!base.IsFrozen && !IsGrounded && onGround)
		{
			movePsv = base.transform.up * speedJump;
			characterCom.PlayMyAnimation("Weightlessness", string.Empty);
		}
	}

	public bool CoverStunAnim()
	{
		characterCom.PlayMyAnimation("Idle", gunCom.name);
		return false;
	}

	public bool CoverStun()
	{
		IsGrounded = false;
		return false;
	}

	public bool ResistStun()
	{
		SceneTimerInstance.Instance.Add(0.2f, ResistStunEffect, 10, true);
		fCurResistAlpha = 0.2f;
		return false;
	}

	public bool ResistStunEffect()
	{
		HideAdd(fCurResistAlpha);
		if (fCurResistAlpha > 0.9f)
		{
			fCurResistAlpha = 0.2f;
		}
		else
		{
			fCurResistAlpha = 1f;
		}
		return false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!base.IsDead && !IsInvincible)
		{
			COMA_Run_Road_Move component = hit.collider.gameObject.GetComponent<COMA_Run_Road_Move>();
			if (component != null)
			{
				Debug.Log("Hit ------ JS-----");
				ColliderDizzy();
				component.NotifyCollideHitPlayer();
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.name.StartsWith("PFB_Prop_Mine"))
		{
			if (avoidMine > 0)
			{
				avoidMine--;
				return;
			}
			if (!IsInvincible)
			{
				StopMove();
				Vector3 push = base.transform.position - other.transform.position + Vector3.up * 5f * 2f;
				ReceiveHurt(0f, push);
			}
			COMA_Run_SceneController.Instance.DelMine(other.name);
			COMA_CD_DelMine cOMA_CD_DelMine = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.DELMINE) as COMA_CD_DelMine;
			cOMA_CD_DelMine.pos = other.transform.position;
			cOMA_CD_DelMine.strMineName = other.name;
			COMA_CommandHandler.Instance.Send(cOMA_CD_DelMine);
		}
		else if (other.name.StartsWith("Exhaust"))
		{
			if (!IsInvincible)
			{
				StopMove();
				Vector3 push2 = base.transform.position - other.transform.position + Vector3.up * 5f;
				ReceiveHurt(0f, push2);
			}
		}
		else if (other.name.StartsWith("Glue"))
		{
			if (avoidGlue > 0)
			{
				avoidGlue--;
				Debug.Log("-------------Avoid Glue one time!@");
			}
			else if (!IsInvincible)
			{
				buff_reduceSpeed = true;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Sweat, base.transform);
				GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Sweat/Sweat")) as GameObject;
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = base.transform.position + Vector3.up;
				Object.DestroyObject(gameObject, reduceSpeedLastTime);
				SceneTimerInstance.Instance.Add(reduceSpeedLastTime, RecoverSpeed);
			}
		}
		else if (other.name.StartsWith("PFB_Gold"))
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
			Object.DestroyObject(other.gameObject);
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Get_money/Get_money_01")) as GameObject;
			gameObject2.transform.position = base.transform.position + Vector3.up;
			Object.DestroyObject(gameObject2, 2f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Gold, base.transform, 2f, false);
		}
		else if (other.name == "Item")
		{
			int randomItemID = GetRandomItemID();
			if (randomItemID != -1)
			{
				int useCount = 1;
				switch (randomItemID)
				{
				case 2:
					useCount = 3;
					break;
				case 10:
					useCount = 5;
					break;
				}
				UIInGame_PropsBox.Instance.GetProps(randomItemID, useCount);
				Object.DestroyObject(other.gameObject);
				GameObject gameObject3 = Object.Instantiate(Resources.Load("Particle/effect/Get_Item/Get_Item_01")) as GameObject;
				gameObject3.transform.position = base.transform.position + Vector3.up;
				Object.DestroyObject(gameObject3, 2f);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item_01, base.transform, 2f, false);
			}
		}
		else if (other.tag == "Finish")
		{
			if (other.name == "Stop")
			{
				StopMove();
			}
			else if (!bFinished)
			{
				bFinished = true;
				COMA_Run_SceneController.Instance.RunFinish(base.transform);
			}
		}
	}

	private bool RecoverSpeed()
	{
		buff_reduceSpeed = false;
		return false;
	}

	private int GetRandomItemID()
	{
		int order = COMA_Run_SceneController.Instance.GetOrder(base.transform);
		Random.seed = (int)(base.transform.position.z * 545476f);
		float num = Random.Range(0f, 1000000f) / 10000f;
		int result = -1;
		switch (order)
		{
		case 0:
			result = ((!(num < 5f)) ? ((num < 10f) ? 1 : ((!(num < 30f)) ? ((!(num < 30f)) ? ((!(num < 30f)) ? ((!(num < 40f)) ? ((!(num < 60f)) ? ((!(num < 75f)) ? ((!(num < 85f)) ? ((!(num < 85f)) ? 10 : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2)) : 0);
			break;
		case 1:
			result = ((!(num < 10f)) ? ((num < 15f) ? 1 : ((!(num < 30f)) ? ((!(num < 35f)) ? ((!(num < 50f)) ? ((!(num < 60f)) ? ((!(num < 70f)) ? ((!(num < 80f)) ? ((!(num < 90f)) ? ((!(num < 90f)) ? 10 : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2)) : 0);
			break;
		case 2:
			result = ((!(num < 15f)) ? ((num < 25f) ? 1 : ((!(num < 35f)) ? ((!(num < 50f)) ? ((!(num < 70f)) ? ((!(num < 75f)) ? ((!(num < 75f)) ? ((!(num < 80f)) ? ((!(num < 90f)) ? ((!(num < 95f)) ? 10 : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2)) : 0);
			break;
		case 3:
			result = ((!(num < 20f)) ? ((num < 25f) ? 1 : ((!(num < 25f)) ? ((!(num < 40f)) ? ((!(num < 65f)) ? ((!(num < 70f)) ? ((!(num < 70f)) ? ((!(num < 70f)) ? ((!(num < 85f)) ? ((!(num < 100f)) ? 10 : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2)) : 0);
			break;
		default:
			Debug.LogError("No Order 5!!");
			break;
		}
		return result;
	}

	public override int UseItem(int itemID)
	{
		if (base.IsDead)
		{
			return 1;
		}
		if (base.IsFrozen)
		{
			return 1;
		}
		if (IsGrounded)
		{
			return 1;
		}
		Debug.Log("Use Item : " + itemID);
		switch (itemID)
		{
		case 0:
		{
			SceneTimerInstance.Instance.Remove(AcceleratingEnd);
			AcceleratingEnd();
			buff_speedUp = true;
			SceneTimerInstance.Instance.Remove(BuffRemove_SpeedUp);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_run_speedUp, BuffRemove_SpeedUp);
			characterCom.PlayMyAnimation("Run00_fast", string.Empty);
			if (base.transform.FindChild("PFB_Buff_SpeedUp") != null)
			{
				Object.DestroyObject(base.transform.FindChild("PFB_Buff_SpeedUp").gameObject);
			}
			GameObject gameObject3 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_SpeedUp"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject3.name = gameObject3.name.Replace("(Clone)", string.Empty);
			gameObject3.transform.parent = base.transform;
			Object.DestroyObject(gameObject3, COMA_Buff.Instance.lastTime_run_speedUp);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Shoe_JiaSu, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff7 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff7.buffState = COMA_Buff.Buff.SpeedUp;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff7);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 1:
		{
			InvincibleStart(COMA_Buff.Instance.lastTime_invincible);
			if (base.transform.FindChild("Shield_01") != null)
			{
				Object.DestroyObject(base.transform.FindChild("Shield_01").gameObject);
			}
			GameObject gameObject4 = Object.Instantiate(Resources.Load("Particle/effect/Shield/Shield_01")) as GameObject;
			gameObject4.name = gameObject4.name.Replace("(Clone)", string.Empty);
			gameObject4.transform.parent = base.transform;
			gameObject4.transform.localPosition = new Vector3(0f, 2.3f, -0.3f);
			Object.DestroyObject(gameObject4, COMA_Buff.Instance.lastTime_invincible);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Shield, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff8 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff8.buffState = COMA_Buff.Buff.Invincible;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff8);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 2:
		{
			avoidMine = 1;
			string text = "PFB_Prop_Mine" + COMA_Server_ID.Instance.GID + nMineId;
			nMineId++;
			COMA_Run_SceneController.Instance.PutMine(base.transform.position, text);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Mine_use, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff3 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff3.buffState = COMA_Buff.Buff.Mine;
			cOMA_CD_PlayerBuff3.buffName = text;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff3);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 3:
		{
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff10 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff10.buffState = COMA_Buff.Buff.GetFlash;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff10);
			break;
		}
		case 4:
			PutOnGun("W006_Run");
			UI_SetFire(0, 0f, 0f);
			break;
		case 5:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Screen_ink_use, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff9 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff9.buffState = COMA_Buff.Buff.Doodle;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff9);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 6:
		{
			buff_goldAdsorb = true;
			SceneTimerInstance.Instance.Remove(BuffRemove_GoldAdsorb);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_run_goldAdsorb, BuffRemove_GoldAdsorb);
			if (base.transform.FindChild("Cohesion_Money_01") != null)
			{
				Object.DestroyObject(base.transform.FindChild("Cohesion_Money_01").gameObject);
			}
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Cohesion_Money/Cohesion_Money_01")) as GameObject;
			gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 1f, -0.3f);
			Object.DestroyObject(gameObject2, COMA_Buff.Instance.lastTime_run_goldAdsorb);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Absorb, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff6 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff6.buffState = COMA_Buff.Buff.GoldAdsorb;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff6);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 7:
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/Run/Exhaust")) as GameObject;
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			gameObject.transform.position = base.transform.position + Vector3.up * 0.8f + Vector3.forward * -0.5f;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_run_exhaust);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Exhaust, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff5 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff5.buffState = COMA_Buff.Buff.Exhaust;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff5);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 8:
		{
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff4 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff4.buffState = COMA_Buff.Buff.Inverted;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff4);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Curse_use, base.transform);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		case 9:
			if (!bBouncing)
			{
				float buffTime = RideRocket();
				COMA_CD_PlayerBuff cOMA_CD_PlayerBuff2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
				cOMA_CD_PlayerBuff2.buffState = COMA_Buff.Buff.RideRocket;
				cOMA_CD_PlayerBuff2.buffTime = buffTime;
				COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff2);
				COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			}
			break;
		case 10:
		{
			avoidGlue = 1;
			COMA_Run_SceneController.Instance.PutGlue(base.transform.position);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.Glue;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Sweat_use, base.transform);
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, itemID, string.Empty);
			break;
		}
		}
		return 0;
	}

	public override bool BuffRemove_SpeedUp()
	{
		buff_speedUp = false;
		characterCom.PlayMyAnimation("Run00_fast", string.Empty, "0");
		return false;
	}

	public void BeDoodle()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Screen_ink, base.transform);
		COMA_Run_SceneController.Instance.ShowInk();
	}

	private void SetGameObjectLayer(GameObject obj, int nLayer)
	{
		obj.layer = nLayer;
		Transform[] componentsInChildren = obj.transform.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = nLayer;
		}
	}

	public float RideRocket()
	{
		float num = 15f;
		bRideRocket = true;
		onGround = false;
		COMA_Sys.Instance.bCoverUIInput = true;
		InvincibleStart(COMA_Buff.Instance.lastTime_run_ride_rocket + 2f);
		base.transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		tarUp = Vector3.up;
		SceneTimerInstance.Instance.Remove(AcceleratingEnd);
		AcceleratingEnd();
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Rocket_Appear/Rocket_Appear1")) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("-------Rocket_Appear  Null");
		}
		gameObject.transform.position = base.transform.position + new Vector3(0f, 0.7f, 0f);
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, 1f);
		fPreGravity = gravity;
		Vector3 localPosition = base.transform.localPosition;
		if (localPosition.y > num)
		{
			localPosition.y = num;
		}
		float num2 = Mathf.Sqrt(Mathf.Abs(2f * (num - localPosition.y) / gravity));
		movePsv.y = Mathf.Abs(gravity * num2);
		curSpeed = speedRun * 3f;
		GameObject gameObject2 = Object.Instantiate(Resources.Load("FBX/Scene/Run/Prefab/Rocket")) as GameObject;
		gameObject2.transform.parent = base.transform.Find("Character/Bip01");
		gameObject2.transform.localPosition = new Vector3(-0.5f, -1f, -0.5f);
		gameObject2.transform.localRotation = Quaternion.Euler(-90f, -90f, 0f);
		characterCom.PlayMyAnimation("Rockets03_huang", gunCom.name);
		_bRideRocket = true;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Rocket_Fly_A, base.transform);
		Vector3 vector = Vector3.zero;
		Ray ray = new Ray(base.transform.position + base.transform.up * 1f + new Vector3(0f - base.transform.position.x, num, curSpeed * COMA_Buff.Instance.lastTime_run_ride_rocket), -base.transform.up);
		int layerMask = 1 << LayerMask.NameToLayer("Death");
		bool flag = false;
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 400f, layerMask))
		{
			Debug.Log("----StartPoint:" + hitInfo.collider.transform.root.name);
			Transform transform = hitInfo.collider.transform.root.FindChild("RocketPoint");
			if (transform == null)
			{
				COMA_Run_RocketPoint componentInChildren = hitInfo.collider.transform.root.GetComponentInChildren<COMA_Run_RocketPoint>();
				if (componentInChildren != null)
				{
					transform = componentInChildren.transform;
				}
			}
			float num3 = float.PositiveInfinity;
			vector = base.transform.position;
			if (transform != null)
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					float num4 = Mathf.Abs(child.position.z - base.transform.position.z);
					if (num3 > num4 && num4 > 0f)
					{
						num3 = num4;
						vector = child.position;
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			vector = new Vector3(0f, num, base.transform.position.z) + new Vector3(0f, 0f, 50f);
			Debug.Log("--------------------Rocket!!!!! Front 50m Explode!");
		}
		if (vector.z > COMA_Run_SceneController.Instance.endObj.transform.position.z)
		{
			vector.z = COMA_Run_SceneController.Instance.endObj.transform.position.z;
			Debug.Log("--------------------------Rocket!!!!!!!!!!!!!!!!!!!Limit Rocket fly....");
		}
		Debug.Log("----Rocket explode Pos:" + vector);
		float num5 = speedRun;
		if (vector.y > num)
		{
			vector.y = num;
		}
		float num6 = Mathf.Sqrt(Mathf.Abs(2f * (num - vector.y) / gravity));
		Debug.Log("-----t1=" + num6);
		float num7 = (vector.z - curSpeed * num6 - num5 * num6 - base.transform.position.z) / curSpeed;
		num7 += num6;
		Debug.Log("-----fExporeRunSpeed=" + num5);
		Object.DestroyObject(gameObject2, num7);
		cmrCom.cmr.animation.Play("Camera_PushClose");
		SceneTimerInstance.Instance.Remove(DownRocket);
		SceneTimerInstance.Instance.Add(num7, DownRocket);
		SetGameObjectLayer(base.gameObject, LayerMask.NameToLayer("NoCollision"));
		SceneTimerInstance.Instance.Add(1f, RocketBackHead);
		fRocketFly = num7 - 1f;
		Debug.Log("-----DownRocket after : " + num7);
		return num7;
	}

	private bool RocketBackHead()
	{
		if (bRideRocket)
		{
			characterCom.PlayMyAnimation("Lookback_huang", gunCom.name);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Rocket_Fly_B, base.transform, fRocketFly);
		}
		return false;
	}

	public bool DownRocket()
	{
		curSpeed = speedRun;
		SetGameObjectLayer(base.gameObject, LayerMask.NameToLayer("Player"));
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Rocket_Brust/Rocket_Brust")) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("-------Rocket_Appear  Null");
		}
		gameObject.transform.position = base.transform.position + new Vector3(0f, 0.7f, 0f);
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, 3f);
		characterCom.PlayMyAnimation("Rockets03_huang", gunCom.name, "0");
		characterCom.PlayMyAnimation("Point_down", gunCom.name);
		gravity = fPreGravity;
		_bRideRocket = false;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Rocket_Fly_C, base.transform);
		COMA_Sys.Instance.bCoverUIInput = false;
		bRideRocket = false;
		cmrCom.cmr.animation.Play("Camera_PushFarther");
		return false;
	}

	public void BeInverted()
	{
		IsInverted = true;
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Curse/Curse_pfb")) as GameObject;
		gameObject.transform.position = base.transform.position + new Vector3(0f, 1.7f, 0f);
		gameObject.transform.parent = base.transform;
		Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_run_inverted);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Curse, base.transform);
		SceneTimerInstance.Instance.Remove(base.InvertedRecover);
		SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_run_inverted, base.InvertedRecover);
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.EffectInverted;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
	}

	public bool BuffRemove_GoldAdsorb()
	{
		buff_goldAdsorb = false;
		return false;
	}

	public override void CharacterCall_Fire()
	{
		Vector3 position = gunCom.bulletInitLoc.position;
		Quaternion rotation = base.transform.rotation;
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/Run/B006"), position, rotation) as GameObject;
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		COMA_BulletFollower component = gameObject.GetComponent<COMA_BulletFollower>();
		component.fromPlayerCom = this;
		component.distance = gunCom.config.bulletrange;
		component.moveSpeed = gunCom.config.bulletspeed;
		component.gravity = gunCom.config.bulletgravity;
		component.ap = ((!buff_oneShot) ? gunCom.config.ap : COMA_Sys.Instance.apToOneShot);
		component.apr = ((!buff_oneShot) ? gunCom.config.apr : COMA_Sys.Instance.apToOneShot);
		component.radius = gunCom.config.radius;
		component.push = gunCom.config.push;
		float num = float.PositiveInfinity;
		Transform transform = null;
		for (int i = 0; i < COMA_Scene.Instance.playerNodeTrs.childCount; i++)
		{
			Transform child = COMA_Scene.Instance.playerNodeTrs.GetChild(i);
			if (!(child.name == base.name))
			{
				float num2 = child.position.z - base.transform.position.z;
				if (!(num2 <= 0f) && num2 < num)
				{
					num = num2;
					transform = child;
				}
			}
		}
		component.SetTarget(transform);
		if (transform != null)
		{
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, 4, transform.GetComponent<COMA_Creation>().nickname);
		}
		else
		{
			COMA_Run_SceneController.Instance.UseItemInfo(nickname, 4, string.Empty);
		}
	}

	public void StopMove()
	{
		curSpeed = 0f;
	}

	public void StopPsvMove()
	{
		movePsv = Vector3.zero;
	}

	public override bool OnRelive()
	{
		return false;
	}
}
