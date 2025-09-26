using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_Hunger : COMA_PlayerSelf
{
	private float hurtInterval = 62f;

	private int avoidMine;

	private float heightRoRaise = 5.1f;

	private string curLockingItemName = string.Empty;

	protected new void Start()
	{
		base.Start();
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		speedRun = 4.5f;
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			UIInGame_PropsBox.Instance.GetProps(1);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			UIInGame_PropsBox.Instance.GetProps(4);
		}
		else
		{
			UIInGame_PropsBox.Instance.GetProps(0);
		}
		cCtl.enabled = false;
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
		string param = (string)tEvent.data["key"];
		if (result == RoomLockResCmd.Result.ok)
		{
			ItemLoot(param);
		}
	}

	private new void Update()
	{
		UpdateShadow();
		if (heightRoRaise > 0f)
		{
			heightRoRaise -= Time.deltaTime;
			base.transform.position += new Vector3(0f, Time.deltaTime, 0f);
			if (heightRoRaise <= 0f)
			{
				cCtl.enabled = true;
			}
		}
		else
		{
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
			if (cCtl.isGrounded && movePsv.y <= 0f)
			{
				BaseState();
			}
			else
			{
				movePsv += Physics.gravity * Time.deltaTime;
			}
			if (base.IsVenom)
			{
				moveCur *= 0.4f;
			}
			if (buff_speedUp)
			{
				moveCur *= 2f;
			}
			if (IsGrounded)
			{
				moveCur = Vector3.zero;
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
			if (!COMA_Sys.Instance.bCoverUIInput)
			{
				UpdateHurting();
			}
			RotateWaist();
			UpdateFire();
		}
	}

	private void UpdateHurting()
	{
		if (hurtInterval > 2f)
		{
			hurtInterval -= Time.deltaTime;
			if (hurtInterval <= 2f)
			{
				Debug.Log("--------------------" + hurtInterval);
				COMA_Hunger_SceneController.Instance.famineCom.OpenInGameTips();
				COMA_Hunger_SceneController.Instance.StartRedScreen();
			}
		}
		else if (hurtInterval > 0f)
		{
			hurtInterval -= Time.deltaTime;
			if (hurtInterval <= 0f)
			{
				hurtInterval = 2f;
				ReceiveHurt(0, 0, 1f, Vector3.zero, false);
				UIInGameHungers_PlayersMgr.Instance._testData[sitIndex].HP = base.hp / HP;
				OnHPChange();
			}
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
		if (other.name.Contains("|"))
		{
			curLockingItemName = COMA_CommonOperation.Instance.ValueLock_Hunger_Item + other.name;
			COMA_Network.Instance.LockValue(curLockingItemName);
		}
		else if (other.name == "PFB_Prop_Mine")
		{
			if (avoidMine > 0)
			{
				avoidMine--;
				return;
			}
			Vector3 push = base.transform.position - other.transform.position + Vector3.up;
			ReceiveHurt(100f, push);
			COMA_Hunger_SceneController.Instance.DelMine(other.transform.position);
			COMA_CD_DelMine cOMA_CD_DelMine = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.DELMINE) as COMA_CD_DelMine;
			cOMA_CD_DelMine.pos = other.transform.position;
			COMA_CommandHandler.Instance.Send(cOMA_CD_DelMine);
		}
	}

	private void ItemLoot(string param)
	{
		string[] array = param.Split('|');
		int num = int.Parse(array[1]);
		switch (array[0])
		{
		default:
			return;
		case "PFB_Item_Hunger0":
			UIInGame_PropsBox.Instance.GetProps(0);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Ham, base.transform);
			break;
		case "PFB_Item_Hunger1":
			UIInGame_PropsBox.Instance.GetProps(1);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Cloak, base.transform);
			break;
		case "PFB_Item_Hunger2":
			UIInGame_PropsBox.Instance.GetProps(2);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_shandian, base.transform);
			break;
		case "PFB_Item_Hunger3":
			UIInGame_PropsBox.Instance.GetProps(3, 2);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Mine, base.transform);
			break;
		case "PFB_Item_Hunger4":
			UIInGame_PropsBox.Instance.GetProps(4);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Shoe, base.transform);
			break;
		case "PFB_Item_Hunger5":
			PutOnGun("W001_Hunger");
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Weapon, base.transform);
			break;
		case "PFB_Item_Hunger6":
			PutOnGun("W002_Hunger");
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Weapon, base.transform);
			break;
		case "PFB_Item_Hunger7":
			PutOnGun("W003_Hunger");
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Weapon, base.transform);
			break;
		}
		COMA_Network.Instance.UnlockValue(COMA_CommonOperation.Instance.ValueLock_Hunger_Item + param);
		Debug.Log("Real Get Item:" + num);
		COMA_Hunger_SceneController.Instance.DeleteItem(num);
		COMA_CD_DeleteItem cOMA_CD_DeleteItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMDELETE) as COMA_CD_DeleteItem;
		cOMA_CD_DeleteItem.blockIndex = (byte)num;
		COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteItem);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
	}

	public override int UseItem(int itemID)
	{
		if (base.IsDead)
		{
			return 1;
		}
		Debug.Log("Use Item : " + itemID);
		switch (itemID)
		{
		case 0:
		{
			base.hp += 50f;
			UIInGameHungers_PlayersMgr.Instance._testData[sitIndex].HP = base.hp / HP;
			GameObject gameObject3 = Object.Instantiate(Resources.Load("Particle/effect/HP/hp")) as GameObject;
			gameObject3.transform.position = base.transform.position;
			Object.DestroyObject(gameObject3, 1f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Ham, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff4 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff4.buffState = COMA_Buff.Buff.Heal;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff4);
			break;
		}
		case 1:
		{
			HideStart(0.3f);
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Stealth/Stealth")) as GameObject;
			gameObject2.transform.position = base.transform.position;
			Object.DestroyObject(gameObject2, 1f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Cloak, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff3 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff3.buffState = COMA_Buff.Buff.Hide;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff3);
			break;
		}
		case 2:
		{
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff2 = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff2.buffState = COMA_Buff.Buff.GetFlash;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff2);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_shandian, base.transform);
			break;
		}
		case 3:
		{
			avoidMine = 1;
			COMA_Hunger_SceneController.Instance.PutMine(base.transform.position);
			COMA_CD_PutMine cOMA_CD_PutMine = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PUTMINE) as COMA_CD_PutMine;
			cOMA_CD_PutMine.pos = base.transform.position;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PutMine);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Mine, base.transform);
			break;
		}
		case 4:
		{
			buff_speedUp = true;
			SceneTimerInstance.Instance.Remove(BuffRemove_SpeedUp);
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_speedUp, BuffRemove_SpeedUp);
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_SpeedUp"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_speedUp);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Shoe_JiaSu, base.transform);
			COMA_CD_PlayerBuff cOMA_CD_PlayerBuff = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_BUFF) as COMA_CD_PlayerBuff;
			cOMA_CD_PlayerBuff.buffState = COMA_Buff.Buff.SpeedUp;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerBuff);
			break;
		}
		}
		return 0;
	}

	public override bool OnRelive()
	{
		if (!COMA_Scene.Instance.runingGameOver)
		{
			COMA_Scene.Instance.runingGameOver = true;
			COMA_Scene.Instance.GameFinishBySurvive();
		}
		return false;
	}
}
