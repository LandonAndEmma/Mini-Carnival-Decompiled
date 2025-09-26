using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_Maze : COMA_PlayerSelf
{
	public bool bEvil;

	private GameObject evilObj;

	private bool bPounce;

	private COMA_Maze_Font goldGetCom;

	private bool bGoldDrop;

	public bool bShowItem;

	protected new void Start()
	{
		if (COMA_Network.Instance != null)
		{
			if (COMA_Network.Instance.IsRoomMaster(id))
			{
				SceneTimerInstance.Instance.Add(5f, COMA_Maze_SceneController.Instance.CreateEvilItem);
			}
			base.Start();
			if (goldGetCom == null)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Golds/PFB_GoldGet")) as GameObject;
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = new Vector3(0f, 2.3f, 0f);
				gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				gameObject.transform.parent = characterCom.boneTrs_Waist;
				goldGetCom = gameObject.GetComponent<COMA_Maze_Font>();
			}
			base.transform.position = bornPointTrs.position;
			base.transform.rotation = bornPointTrs.rotation;
			speedRun = 4.5f;
			if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
			{
				speedRun += 1f;
			}
			else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
			{
				bShowItem = true;
			}
			else
			{
				bGoldDrop = true;
			}
			if (UIInGame_DirTagMgr.Instance != null)
			{
				UIInGame_DirTagMgr.Instance.SetPlayerSelf(COMA_Scene.Instance.cmrMoveCom.cmr.transform);
			}
		}
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_HIT, base.ReceiveHurt);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_SCOREGET, ReceiveScoreGet);
			COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.POUNCE, ReceiveBePounced);
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
			COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.POUNCE, ReceiveBePounced);
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
			if (text == COMA_CommonOperation.Instance.ValueLock_Maze_Pumpkin)
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
		Vector3 vector = base.transform.forward * moveInput.y + base.transform.right * moveInput.x;
		if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
		{
			float num = (float)base.score / 100f;
			float num2 = speedRun - num;
			if (bEvil)
			{
				num2 = num2 * 1.2f + 1f;
			}
			moveCur = vector.normalized * num2;
		}
		else
		{
			moveInput = Vector2.zero;
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
		UpdatePouncing();
	}

	private void UpdatePouncing()
	{
		if (!bPounce)
		{
			return;
		}
		Ray ray = new Ray(base.transform.position + Vector3.up * 0.2f, base.transform.forward);
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1.5f, layerMask))
		{
			Debug.Log("扑到了");
			COMA_PlayerSync_Maze component = hitInfo.collider.GetComponent<COMA_PlayerSync_Maze>();
			int num = component.score / 2;
			GetScore(num);
			COMA_CD_Pounce commandDatas = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.POUNCE) as COMA_CD_Pounce;
			COMA_CommandHandler.Instance.Send(commandDatas, component.id);
			SceneTimerInstance.Instance.Remove(EndEvil);
			SceneTimerInstance.Instance.Add(1f, EndEvil);
			RecoverPounce();
			if (num >= 50)
			{
				COMA_Achievement.Instance.Robber++;
			}
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Gold_Effect/Gold_Jet_01")) as GameObject;
			gameObject.transform.position = component.transform.position;
			Object.DestroyObject(gameObject, 2f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Gold_Jet, base.transform);
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Gold_Effect/Gold_Cohesion_01")) as GameObject;
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.up * 0.6f;
			Object.DestroyObject(gameObject2, 2.5f);
		}
	}

	public override void CharacterCall_Fire()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Swoop/Swoop")) as GameObject;
		gameObject.transform.position = base.transform.position + base.transform.forward * 0.8f;
		gameObject.transform.rotation = base.transform.rotation;
		Object.DestroyObject(gameObject, 1f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Swoop, base.transform);
	}

	public override void UI_Jump()
	{
		if (bEvil && IsOnGround())
		{
			movePsv = base.transform.forward * 6f + Vector3.up * 3f;
			characterCom.PlayMyAnimation("Getdown", gunCom.name);
			COMA_Sys.Instance.bCoverUIInput = true;
			SceneTimerInstance.Instance.Add(1.3f, RecoverUIInput);
			bPounce = true;
			SceneTimerInstance.Instance.Add(1.3f, RecoverPounce);
		}
	}

	public bool RecoverUIInput()
	{
		COMA_Sys.Instance.bCoverUIInput = false;
		return false;
	}

	public bool RecoverPounce()
	{
		bPounce = false;
		return false;
	}

	public void ReceiveBePounced(COMA_CommandDatas commandDatas)
	{
		GetScore(-base.score / 2);
		characterCom.PlayMyAnimation("Falldown", gunCom.name);
		COMA_Sys.Instance.bCoverUIInput = true;
		SceneTimerInstance.Instance.Add(2f, RecoverUIInput);
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Gold_Effect/Gold_Jet_01")) as GameObject;
		gameObject.transform.position = base.transform.position;
		Object.DestroyObject(gameObject, 2f);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Gold_Jet, base.transform);
	}

	private bool Flash()
	{
		GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Transfiguration/Transfiguration"), base.transform.position, base.transform.rotation) as GameObject;
		Object.DestroyObject(obj, 2f);
		COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
		cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.Evil;
		COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Transfiguration, base.transform, 4f);
		return false;
	}

	public bool Change()
	{
		characterCom.PlayMyAnimation("Makeup02", gunCom.name);
		SetEvil(true);
		return false;
	}

	public void SetEvil(bool evil)
	{
		SetEvil(evil, true);
	}

	public void SetEvil(bool evil, bool bCreateNextItem)
	{
		if (bEvil == evil)
		{
			return;
		}
		bEvil = evil;
		if (evil)
		{
			evilObj = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/PumpkinHead")) as GameObject;
			evilObj.transform.parent = characterCom.head_top.parent;
			evilObj.transform.localPosition = new Vector3(0.8f, 0f, 0f);
			evilObj.transform.localEulerAngles = new Vector3(0f, 270f, 180f);
			characterCom.bodyObjs[0].SetActive(false);
			characterCom.bodyObjs[1].renderer.material.mainTexture = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Tex_Body")) as Texture2D;
			characterCom.bodyObjs[2].renderer.material.mainTexture = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Tex_Leg")) as Texture2D;
			goldGetCom.gameObject.SetActive(false);
			if (UIInGame_DirTagMgr.Instance != null)
			{
				UIInGame_DirTagMgr.Instance.ShowAllTag();
			}
			COMA_Maze_SceneController.Instance.SetJumpEnable(true);
			SceneTimerInstance.Instance.Add(30f, EndEvil);
			COMA_Scene.Instance.cmrMoveCom.cmr.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerTrigger"));
		}
		else
		{
			if (evilObj != null)
			{
				Object.DestroyObject(evilObj);
			}
			characterCom.bodyObjs[0].SetActive(true);
			characterCom.bodyObjs[1].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].texture;
			characterCom.bodyObjs[2].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].texture;
			goldGetCom.gameObject.SetActive(true);
			if (UIInGame_DirTagMgr.Instance != null)
			{
				UIInGame_DirTagMgr.Instance.HideAllTag();
			}
			if (bCreateNextItem)
			{
				COMA_Maze_SceneController.Instance.CreateEvilItem();
			}
			COMA_Maze_SceneController.Instance.SetJumpEnable(false);
			COMA_Scene.Instance.cmrMoveCom.cmr.cullingMask |= 1 << LayerMask.NameToLayer("PlayerTrigger");
		}
		COMA_CD_BeEvil cOMA_CD_BeEvil = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.EVIL) as COMA_CD_BeEvil;
		cOMA_CD_BeEvil.bEvil = evil;
		COMA_CommandHandler.Instance.Send(cOMA_CD_BeEvil);
	}

	public bool EndEvil()
	{
		Flash();
		SetEvil(false);
		COMA_Network.Instance.UnlockValue(COMA_CommonOperation.Instance.ValueLock_Maze_Pumpkin);
		return false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (bEvil)
		{
			return;
		}
		if (other.tag == "Gold")
		{
			GetScore(1);
			if (bGoldDrop)
			{
				Random.seed = (int)Time.time * 12121;
				if (Random.Range(0f, 1f) < 0.1f)
				{
					GetScore(1);
				}
			}
			int num = int.Parse(other.name);
			COMA_Maze_SceneController.Instance.DeleteGold(num);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Gold, base.transform, 2f, false);
			COMA_CD_DeleteItem cOMA_CD_DeleteItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMDELETE) as COMA_CD_DeleteItem;
			cOMA_CD_DeleteItem.itemIndex = (byte)num;
			COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteItem);
		}
		else if (other.name == "PFB_Item_Evil")
		{
			COMA_Network.Instance.LockValue(COMA_CommonOperation.Instance.ValueLock_Maze_Pumpkin);
		}
	}

	private void ItemLoot()
	{
		COMA_Maze_SceneController.Instance.DeleteEvilItem();
		Flash();
		Change();
		COMA_Sys.Instance.bCoverUIInput = true;
		SceneTimerInstance.Instance.Add(4.5f, RecoverUIInput);
	}

	private void GetScore(int add)
	{
		base.score += add;
		SendScore();
		goldGetCom.SetNumber(base.score);
	}

	public override bool OnRelive()
	{
		return base.OnRelive();
	}
}
