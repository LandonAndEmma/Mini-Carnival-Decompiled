using UnityEngine;

public class COMA_PlayerSelf_BlackHouse : COMA_PlayerSelf
{
	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
		}
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(base.SendTransform);
		}
		base.OnDisable();
	}

	private new void Start()
	{
		base.Start();
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
				COMA_BlackHouse_SceneController.Instance.AddRoomComment();
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
}
