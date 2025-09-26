using UnityEngine;

public class COMA_PlayerSelf_Castle : COMA_PlayerSelf
{
	private GameObject doorNodeObj;

	public bool buff_doubleScore;

	private float timeIntervalToHeal = 1f;

	protected new void Start()
	{
		exp = COMA_Pref.Instance.exp;
		rt3DObj = UI_3DModeToTUIMgr.Instance.sourceObjs[0];
		CLIPNUMBER = 5;
		base.Start();
		OnRelive();
		doorNodeObj = GameObject.Find("SceneAddition/Door");
		if (doorNodeObj == null)
		{
			Debug.LogError("doorNodeObj is Null!!");
		}
		string weaponName = "W002_Castle";
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			weaponName = "W001_Castle";
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			weaponName = "W008_Castle";
		}
		PutOnGun(weaponName);
		nickname = COMA_Pref.Instance.nickname;
		UIIngame_DefendUI.Instance._info.HP = 1f;
		UIIngame_DefendUI.Instance._info.Name = nickname;
		UIIngame_DefendUI.Instance._info.Num = base.score;
	}

	private new void Update()
	{
		UpdateShadow();
		if (COMA_Sys.Instance.bCoverUpdate)
		{
			return;
		}
		if (!base.IsDead)
		{
			if (timeToExitAttack >= 0f)
			{
				timeToExitAttack -= Time.deltaTime;
			}
			else
			{
				timeIntervalToHeal -= Time.deltaTime;
				if (timeIntervalToHeal < 0f)
				{
					timeIntervalToHeal = 1f;
					base.hp += HP * 0.2f;
				}
			}
		}
		else if (!COMA_Scene.Instance.runingGameOver)
		{
			COMA_Scene.Instance.runingGameOver = true;
			COMA_Scene.Instance.GameFinishByLocal();
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
		if (base.score >= 2000)
		{
			COMA_Achievement.Instance.Fighter++;
		}
	}

	public bool BuffRemove_DoubleScore()
	{
		buff_doubleScore = false;
		return false;
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("---------------------------------OnTriggerEnter : " + other.name);
		switch (other.name)
		{
		case "Trigger_OpenDoor":
		{
			COMA_Door component2 = other.transform.parent.GetComponent<COMA_Door>();
			component2.OpenDoor();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Gate_up, component2.transform, 4f);
			break;
		}
		case "Trigger_RecoverDoor":
		{
			COMA_Door component = other.transform.parent.GetComponent<COMA_Door>();
			if (component.GetHPRate() < 1f)
			{
				UIInGame_ProgressBar.Instance.StartProgressBar();
			}
			break;
		}
		case "PFB_Item_DoubleScore":
		{
			Object.DestroyObject(other.gameObject);
			GameObject obj5 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
			Object.DestroyObject(obj5, 2f);
			buff_doubleScore = true;
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_doubleScore, BuffRemove_DoubleScore);
			GameObject gameObject2 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_DoubleScore"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject2.transform.parent = base.transform;
			Object.DestroyObject(gameObject2, COMA_Buff.Instance.lastTime_doubleScore);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
			break;
		}
		case "PFB_Item_OneShot":
		{
			Object.DestroyObject(other.gameObject);
			GameObject obj2 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
			Object.DestroyObject(obj2, 2f);
			buff_oneShot = true;
			SceneTimerInstance.Instance.Add(COMA_Buff.Instance.lastTime_oneShot, base.BuffRemove_OneShot);
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_OneShot"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			Object.DestroyObject(gameObject, COMA_Buff.Instance.lastTime_oneShot);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
			break;
		}
		case "PFB_Item_RepairDoor":
		{
			Object.DestroyObject(other.gameObject);
			GameObject obj4 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
			Object.DestroyObject(obj4, 2f);
			COMA_Door[] componentsInChildren = doorNodeObj.GetComponentsInChildren<COMA_Door>();
			COMA_Door[] array2 = componentsInChildren;
			foreach (COMA_Door cOMA_Door in array2)
			{
				cOMA_Door.BeHeal(400f);
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
			break;
		}
		case "PFB_Item_KillAll":
		{
			Object.DestroyObject(other.gameObject);
			GameObject obj6 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
			Object.DestroyObject(obj6, 2f);
			COMA_Enemy_Zombie[] componentsInChildren2 = COMA_Scene.Instance.playerNodeTrs.GetComponentsInChildren<COMA_Enemy_Zombie>();
			COMA_Enemy_Zombie[] array3 = componentsInChildren2;
			foreach (COMA_Enemy_Zombie cOMA_Enemy_Zombie in array3)
			{
				cOMA_Enemy_Zombie.OnHurt(null, string.Empty, COMA_Sys.Instance.apToOneShot, Vector3.zero);
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
			break;
		}
		case "PFB_Item_FullFill":
		{
			Object.DestroyObject(other.gameObject);
			GameObject obj3 = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
			Object.DestroyObject(obj3, 2f);
			clipCount = gunCom.config.clipcount;
			clipNumber = CLIPNUMBER;
			if (COMA_Scene.Instance.magazineCom != null)
			{
				Debug.Log(clipCount + " " + clipNumber);
				UIInGame_BoxMagazineMgr.UIBoxMagazine[] array = new UIInGame_BoxMagazineMgr.UIBoxMagazine[clipNumber + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new UIInGame_BoxMagazineMgr.UIBoxMagazine(clipCount, clipCount);
				}
				COMA_Scene.Instance.magazineCom.InitBoxMagazine(array, 0, COMA_Scene.Instance.bInfinityMode);
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Max_ammo, base.transform);
			break;
		}
		default:
		{
			string text = "PFB_Item_";
			if (other.name.StartsWith(text))
			{
				Object.DestroyObject(other.gameObject);
				GameObject obj = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), other.transform.position, other.transform.rotation) as GameObject;
				Object.DestroyObject(obj, 2f);
				clipNumber = CLIPNUMBER;
				string text2 = other.name.Substring(text.Length) + "_Castle";
				Debug.Log(text2);
				PutOnGun(text2);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Weapon, base.transform);
			}
			break;
		}
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_item, base.transform);
	}

	private void OnTriggerStay(Collider other)
	{
		switch (other.name)
		{
		case "Trigger_RecoverDoor":
		{
			COMA_Door component = other.transform.parent.GetComponent<COMA_Door>();
			component.BeHeal(60f * Time.deltaTime);
			if (component.GetHPRate() < 1f)
			{
				UIInGame_ProgressBar.Instance.ProgressBar(component.GetHPRate());
			}
			else
			{
				UIInGame_ProgressBar.Instance.EndProgressBar();
			}
			break;
		}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		switch (other.name)
		{
		case "Trigger_OpenDoor":
		{
			COMA_Door component = other.transform.parent.GetComponent<COMA_Door>();
			component.CloseDoor();
			break;
		}
		case "Trigger_RecoverDoor":
			UIInGame_ProgressBar.Instance.EndProgressBar();
			break;
		}
	}

	public override bool OnRelive()
	{
		return false;
	}
}
