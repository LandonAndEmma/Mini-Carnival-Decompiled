using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class COMA_PlayerSelf_Fishing : COMA_PlayerSelf
{
	public enum EBuffType
	{
		FloatShake = 0,
		OnBoat = 1
	}

	private List<string> _lstShieldingIDS = new List<string>();

	[SerializeField]
	private COMA_Fishing_FishPole _curFishPole;

	[SerializeField]
	private COMA_Fishing_FishFloat _curFishFloat;

	[SerializeField]
	private COMA_Fishing_FishableObj _curFishItem;

	public GameObject _objFishingLine;

	public Material _fishingMaterial;

	private bool _bCanMove = true;

	public float _fMinFloatDis = 0.5f;

	public float _fMaxFloatDis = 3f;

	private Vector3 _onBoatPos;

	private Vector3 _preG;

	private Vector3 _vG;

	private GameObject cmrParentObj;

	[SerializeField]
	private Animation _comAniCmp;

	private bool bMoveBoat;

	private string _OnBoatId = "-1";

	public static Vector3[] _staBoatStartPos = new Vector3[2]
	{
		new Vector3(22f, 0.5f, 1f),
		new Vector3(22f, 0.5f, -6.5f)
	};

	public static bool[] _bBoatState = new bool[2];

	public static Vector3 _boatLocPos = new Vector3(0f, 0f, 0f);

	public static Vector3 _playerOnBoatPos = new Vector3(-0.1827603f, 0.2074193f, -1.4f);

	public static Vector3 _playerOnBoatFishingPos = new Vector3(0f, 0.6f, 0.3f);

	public static int _nOnBoatPrice = 500;

	private float fOffBoatR;

	public COMA_Fishing_FishBoat _curUseBoat;

	private string _willBoatID = "-1";

	private bool _bCanApplyBoat = true;

	public bool _bStayNotifyOnBoat;

	public COMA_Fishing_FishPole CurFishPole
	{
		get
		{
			return _curFishPole;
		}
		set
		{
			_curFishPole = value;
		}
	}

	public COMA_Fishing_FishFloat CurFishFloat
	{
		get
		{
			return _curFishFloat;
		}
		set
		{
			_curFishFloat = value;
		}
	}

	public COMA_Fishing_FishableObj CurFishItem
	{
		get
		{
			return _curFishItem;
		}
		set
		{
			_curFishItem = value;
		}
	}

	public bool CanApplyBoat
	{
		get
		{
			return _bCanApplyBoat;
		}
		set
		{
			_bCanApplyBoat = value;
		}
	}

	public void AddShieldingId(string add)
	{
		_lstShieldingIDS.Add(add);
	}

	public void DelShieldingId(string del)
	{
		if (_lstShieldingIDS.Contains(del))
		{
			_lstShieldingIDS.Remove(del);
		}
	}

	public bool IsInShieldingList(string id)
	{
		return _lstShieldingIDS.Contains(id);
	}

	public void SetCanMove(bool b)
	{
		_bCanMove = b;
	}

	protected new void OnEnable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Add(0.1f, base.SendTransform);
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		}
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		if (COMA_Network.Instance.IsConnected())
		{
			SceneTimerInstance.Instance.Remove(base.SendTransform);
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.LOCK_STH, OnLockSth);
		}
		base.OnDisable();
	}

	private new void Awake()
	{
		_centerController = new COMA_Fishing_PlayerController(this);
		int sysType = RegisterSystemCtrl(new TFishingPlayerBuffController(this));
		_centerController.MapChildSysTag(0, sysType);
		GameObject gameObject = null;
		gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishFloat")) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Fish Float CREATE FAILURE!");
		}
		_curFishFloat = gameObject.GetComponent<COMA_Fishing_FishFloat>();
		_vG = Physics.gravity;
	}

	protected new void Start()
	{
		base.Start();
		Debug.Log("Bob:" + GetInstanceID());
		TFishingAddressBook.Instance.RegisterAddr(0, GetInstanceID());
		base.transform.position = bornPointTrs.position;
		base.transform.rotation = bornPointTrs.rotation;
		if (COMA_CommonOperation.Instance.selectedWeaponValidity)
		{
			if (COMA_CommonOperation.Instance.selectedWeaponIndex == 0)
			{
				COMA_Fishing.Instance.SetPoleTime(0);
			}
			else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
			{
				COMA_Fishing.Instance.SetPoleTime(1);
			}
		}
		if (COMA_CommonOperation.Instance.selectedWeaponPrice > 0)
		{
			if (COMA_CommonOperation.Instance.selectedWeaponValidity)
			{
				COMA_Pref.Instance.AddGold(-COMA_CommonOperation.Instance.selectedWeaponPrice);
				COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
			}
			COMA_Pref.Instance.Save(true);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponPrice < 0)
		{
			if (COMA_CommonOperation.Instance.selectedWeaponValidity)
			{
				COMA_Pref.Instance.AddCrystal(COMA_CommonOperation.Instance.selectedWeaponPrice);
				COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
			}
			COMA_Pref.Instance.Save(true);
		}
		else
		{
			COMA_Pref.Instance.Save(true);
		}
		if (COMA_CommonOperation.Instance.selectedWeaponIndex == 0)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, GetInstanceID(), 1, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 1)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, GetInstanceID(), 2, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponIndex == 2)
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, GetInstanceID(), 3, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		COMA_CommonOperation.Instance.selectedWeaponValidity = true;
		cmrParentObj = GameObject.Find("CameraNode").gameObject;
	}

	public bool IsOnBoat()
	{
		int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
		TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
		if (tFishingPlayerBuffController.IsExitBuff(1))
		{
			return true;
		}
		return false;
	}

	public void OnBoatPosOffsetStart()
	{
		_curUseBoat.SetSail(false);
		characterCom.transform.position = _curUseBoat._transPlayerFishingNode.position;
		shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
		dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), true);
	}

	public void OnBoatPosOffsetEnd()
	{
		characterCom.transform.position = _curUseBoat._transPlayerNode.position;
		shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
		dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), true);
	}

	private new void Update()
	{
		base.Update();
		UpdateShadow();
		if (!_bCanMove)
		{
			return;
		}
		int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
		TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
		if (tFishingPlayerBuffController.IsExitBuff(1))
		{
			if (moveInput.y < 0f)
			{
				moveInput.y = 0f;
			}
			if (Mathf.Abs(moveInput.x) == 1f)
			{
				moveInput.y = 0.2f;
			}
		}
		Vector3 vector = cmrParentObj.transform.forward * moveInput.y + cmrParentObj.transform.right * moveInput.x;
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
		if (tFishingPlayerBuffController.IsExitBuff(1))
		{
			movePsv.y = 0f;
			if (moveCur == Vector3.zero)
			{
				if (bMoveBoat)
				{
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_StopBoat, base.transform);
				}
				bMoveBoat = false;
				characterCom.PlayMyAnimation("Ship_Idle01", gunCom.name);
				_curUseBoat.PlayBoatAni("Idle01");
				_curUseBoat.SetSail(false);
				Transform transform = base.transform.Find("Amb_Boat_move(Clone)");
				if (transform != null)
				{
					Object.Destroy(transform.gameObject);
				}
			}
			else
			{
				if (!bMoveBoat)
				{
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_OnSail, base.transform, 0f, false);
					bMoveBoat = true;
				}
				_curUseBoat.SetSail(true);
				float x = moveInput.x;
				float num2 = moveInput.y * 1.73f;
				if (num2 > 0f - x && num2 >= x)
				{
					characterCom.PlayMyAnimation("Ship_Forward01", gunCom.name);
					_curUseBoat.PlayBoatAni("Forward01");
				}
				else if (num2 <= 0f - x && num2 > x)
				{
					characterCom.PlayMyAnimation("Ship_TurnLeft01", gunCom.name);
					_curUseBoat.PlayBoatAni("TurnLeft01");
				}
				else if (num2 >= 0f - x && num2 < x)
				{
					characterCom.PlayMyAnimation("Ship_TurnRight01", gunCom.name);
					_curUseBoat.PlayBoatAni("TurnRight01");
				}
				TurnAround(vector / 25f);
			}
		}
		else if (cCtl.isGrounded && movePsv.y <= 0f)
		{
			if (moveCur == Vector3.zero)
			{
				characterCom.PlayMyAnimation("Idle", gunCom.name);
			}
			else
			{
				characterCom.PlayMyAnimation("Run", gunCom.name, "Front");
				TurnAround(vector);
			}
		}
		else
		{
			movePsv += _vG * Time.deltaTime;
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
		if (tFishingPlayerBuffController.IsExitBuff(1) && !(moveCur == Vector3.zero))
		{
			cmrParentObj.transform.rotation = Quaternion.Lerp(cmrParentObj.transform.rotation, base.transform.rotation, 0.1f);
		}
		cmrParentObj.transform.position = Vector3.Lerp(cmrParentObj.transform.position, base.transform.position, 0.1f);
	}

	public void NotifyOtherPlayerFishingInfo(int item, int nWeight, string id)
	{
		if (item != -1)
		{
			COMA_CD_FishingInfo cOMA_CD_FishingInfo = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_INFO) as COMA_CD_FishingInfo;
			cOMA_CD_FishingInfo.itemID = item;
			cOMA_CD_FishingInfo.nFishWeight = nWeight;
			cOMA_CD_FishingInfo.strId = id;
			COMA_CommandHandler.Instance.Send(cOMA_CD_FishingInfo);
			Debug.Log("Send-------FishingInfo:" + item);
		}
	}

	public int GetOnBoatId()
	{
		return int.Parse(_OnBoatId);
	}

	public override bool HandleMessage(TTelegram msg)
	{
		base.HandleMessage(msg);
		GameObject gameObject = null;
		switch (msg._nMsgId)
		{
		case 999999999:
			Debug.Log(string.Concat("@@@@\r\n", ((COMA_Fishing_PlayerController)_centerController).GetStateMachine().CurState, "@@@\r\n"));
			break;
		case 1000000000:
			if (GetFishingItem() != null)
			{
				Debug.Log("==========Direct Get ==============>" + _curFishItem.name);
				((COMA_Fishing_PlayerController)_centerController).ChangeState(COMA_Fishing_PlayerController.EState.ShowItem);
			}
			break;
		case 1:
			gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_black")) as GameObject;
			break;
		case 2:
			gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_silver")) as GameObject;
			break;
		case 3:
			gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_gold")) as GameObject;
			break;
		case 4:
			_curFishFloat.EnableFishFloat((Vector3)msg._pExtraInfo);
			break;
		case 1000000001:
		{
			COMA_Pref.Instance.AddGold(-_nOnBoatPrice);
			COMA_Pref.Instance.Save(true);
			_bCanApplyBoat = true;
			_OnBoatId = (string)msg._pExtraInfo;
			_bBoatState[int.Parse(_OnBoatId) - 1] = true;
			COMA_Fishing_SceneController.Instance._fOnBoatTimes[int.Parse(_OnBoatId) - 1] = Time.time;
			GameObject gameObject3 = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Boat/Boat")) as GameObject;
			gameObject3.name = "Boat";
			gameObject3.transform.parent = _comAniCmp.transform;
			gameObject3.transform.localPosition = _boatLocPos;
			gameObject3.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			_onBoatPos = bornPointTrs.position;
			base.transform.position = _staBoatStartPos[int.Parse(_OnBoatId) - 1];
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
			_preG = Physics.gravity;
			_vG = Vector3.zero;
			_curUseBoat = gameObject3.GetComponent<COMA_Fishing_FishBoat>();
			fOffBoatR = cCtl.radius;
			cCtl.radius = 2f;
			cCtl.stepOffset = 0.1f;
			cmrParentObj.transform.Find("Main Camera").localPosition = new Vector3(0f, 2.65f, -5.6f);
			COMA_Fishing_SceneController.Instance.ActiveBuoys(true);
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1008, TTelegram.SEND_MSG_IMMEDIATELY, null);
			COMA_CD_FishingOnBoat cOMA_CD_FishingOnBoat = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_ONBOAT) as COMA_CD_FishingOnBoat;
			cOMA_CD_FishingOnBoat.boatId = (string)msg._pExtraInfo;
			COMA_CommandHandler.Instance.Send(cOMA_CD_FishingOnBoat);
			COMA_Fishing_SceneController.Instance.SetBoatActive(false, int.Parse(cOMA_CD_FishingOnBoat.boatId) - 1);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_StopBoat, base.transform);
			break;
		}
		case 1000000002:
		{
			COMA_Network.Instance.UnlockValue("ValueLock_Fishing_Boat" + _OnBoatId);
			Transform transform2 = base.transform.Find("AniLayer/Boat");
			if (transform2 == null)
			{
				Debug.LogWarning("Cann't Find Boat!");
				return true;
			}
			COMA_Fishing_SceneController.Instance.ActiveBuoys(false);
			Object.DestroyObject(transform2.gameObject);
			_comAniCmp.animation.Stop();
			base.transform.position = _onBoatPos;
			_vG = _preG;
			cCtl.radius = fOffBoatR;
			cCtl.stepOffset = 1f;
			characterCom.transform.localPosition = Vector3.zero;
			shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
			dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), true);
			cmrParentObj.transform.Find("Main Camera").localPosition = new Vector3(0f, 2.65f, -5.6f);
			int iDByName2 = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 1009, TTelegram.SEND_MSG_IMMEDIATELY, null);
			_bBoatState[int.Parse(_OnBoatId) - 1] = false;
			COMA_CD_FishingOffBoat cOMA_CD_FishingOffBoat = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_OFFBOAT) as COMA_CD_FishingOffBoat;
			cOMA_CD_FishingOffBoat.boatId = _OnBoatId;
			COMA_CommandHandler.Instance.Send(cOMA_CD_FishingOffBoat);
			COMA_Fishing_SceneController.Instance.SetBoatActive(true, int.Parse(cOMA_CD_FishingOffBoat.boatId) - 1);
			_OnBoatId = "-1";
			break;
		}
		case 15:
			if ((int)msg._pExtraInfo == 0)
			{
				_bCanApplyBoat = true;
			}
			else
			{
				COMA_Network.Instance.LockValue("ValueLock_Fishing_Boat" + _willBoatID);
			}
			break;
		case 16:
			TMessageDispatcher.Instance.DispatchMsg(-1, GetInstanceID(), 1000000002, TTelegram.SEND_MSG_IMMEDIATELY, _OnBoatId);
			break;
		case 17:
		{
			COMA_Fishing_PlayerController cOMA_Fishing_PlayerController = _centerController as COMA_Fishing_PlayerController;
			if (cOMA_Fishing_PlayerController.IsIdleState())
			{
				characterCom.PlayMyAnimation("Hello", gunCom.name);
			}
			Transform transform = base.transform.parent.Find(((int)msg._pExtraInfo).ToString());
			if (transform != null)
			{
				GameObject gameObject2 = Object.Instantiate(Resources.Load("Particle/effect/Interaction/Interaction_02_pfb")) as GameObject;
				gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
				gameObject2.transform.parent = transform;
				gameObject2.transform.localPosition = characterCom.transform.localPosition + new Vector3(0f, 1f, 0f);
				Object.DestroyObject(gameObject2, 5f);
			}
			break;
		}
		}
		if (gameObject != null)
		{
			if (_curFishPole != null)
			{
				Object.DestroyObject(_curFishPole.gameObject);
			}
			_curFishPole = gameObject.GetComponent<COMA_Fishing_FishPole>();
			Debug.Log("Cur Month:" + COMA_Server_Account.Instance.GetCurMonth());
			_curFishPole.SetCurMonth(COMA_Server_Account.Instance.GetCurMonth());
			int nCount = -1;
			if (!COMA_CommonOperation.Instance.selectedWeaponValidity)
			{
				nCount = 0;
			}
			_curFishPole.InitFishPole(nCount);
			EquipFishPole();
		}
		_centerController.HandleMessage(msg);
		return true;
	}

	public COMA_Fishing_FishableObj GetFishingItem()
	{
		if (_curFishPole == null)
		{
			Debug.LogWarning("------------>No FishingPole!!!!!Cann't get fishing item.");
			_curFishItem = null;
			return null;
		}
		_curFishItem = _curFishPole.GeneratorFishingObj();
		return _curFishItem;
	}

	public void EquipFishPole()
	{
		if (_curFishPole != null)
		{
			_curFishPole.transform.parent = characterCom.handTrs;
			_curFishPole.transform.localPosition = Vector3.zero;
			_curFishPole.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			_curFishPole.gameObject.SetActive(false);
			int num = 0;
			if (_curFishPole.GetEntityType() == 0)
			{
				num = 0;
			}
			else if (_curFishPole.GetEntityType() == 1)
			{
				num = 1;
			}
			else if (_curFishPole.GetEntityType() == 2)
			{
				num = 2;
			}
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1006, TTelegram.SEND_MSG_IMMEDIATELY, num);
			itemSelected = num;
			COMA_CD_FishingUpdateFishrot cOMA_CD_FishingUpdateFishrot = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_UPDATE_FISHROT) as COMA_CD_FishingUpdateFishrot;
			cOMA_CD_FishingUpdateFishrot.btType = (byte)num;
			COMA_CommandHandler.Instance.Send(cOMA_CD_FishingUpdateFishrot);
		}
	}

	public void EnableFishPole(bool bActive)
	{
		if (_curFishPole != null)
		{
			_curFishPole.gameObject.SetActive(bActive);
		}
	}

	protected override void RotatePlayer(float _x, float _y)
	{
		int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
		TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
		if (!tFishingPlayerBuffController.IsExitBuff(1) || !(moveCur != Vector3.zero))
		{
			float y = _x * 314f * 2f / (float)Screen.width;
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			cmrParentObj.transform.rotation *= quaternion;
		}
	}

	private void OnLockSth(TNetEventData tEvent)
	{
		RoomLockResCmd.Result result = (RoomLockResCmd.Result)(int)tEvent.data["result"];
		Debug.Log("Value Lock : " + result);
		if (result == RoomLockResCmd.Result.ok)
		{
			string text = (string)tEvent.data["key"];
			if (text.StartsWith("ValueLock_Fishing_Boat"))
			{
				string text2 = text.Substring(22, text.Length - 22);
				Debug.Log("key=" + text);
				Debug.Log("boatNum=" + text2);
				TMessageDispatcher.Instance.DispatchMsg(-1, GetInstanceID(), 1000000001, TTelegram.SEND_MSG_IMMEDIATELY, text2);
			}
		}
		else
		{
			_bCanApplyBoat = true;
			string text3 = (string)tEvent.data["key"];
			string text4 = text3.Substring(22, text3.Length - 22);
			COMA_Fishing_SceneController.Instance.SetBoatActive(false, int.Parse(text4) - 1);
			Debug.Log("-----------------------Boat is onsea!!!!!!!!");
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1011, TTelegram.SEND_MSG_IMMEDIATELY, text4);
		}
	}

	private void ProcessCollision_GetBoat(Collider other, bool bOnBoat)
	{
		string text = other.name.Substring(17, other.name.Length - 17);
		if (COMA_Fishing_SceneController.Instance.IsBoatActive(int.Parse(text) - 1) && _bCanApplyBoat && !bOnBoat)
		{
			_bCanApplyBoat = false;
			Debug.Log("Need Get Boat" + text);
			_willBoatID = text;
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1010, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		else if (!COMA_Fishing_SceneController.Instance.IsBoatActive(int.Parse(text) - 1) && !bOnBoat)
		{
			int iDByName2 = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 1011, TTelegram.SEND_MSG_IMMEDIATELY, text);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		bool bOnBoat = false;
		int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
		TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
		if (tFishingPlayerBuffController.IsExitBuff(1))
		{
			bOnBoat = true;
		}
		if (other.name.StartsWith("Collision_GetBoat"))
		{
			ProcessCollision_GetBoat(other, bOnBoat);
		}
		else if (other.name.StartsWith("Collision_CloseBoatWindow"))
		{
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1020, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (_bStayNotifyOnBoat)
		{
			_bStayNotifyOnBoat = false;
			bool bOnBoat = false;
			int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
			TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
			if (tFishingPlayerBuffController.IsExitBuff(1))
			{
				bOnBoat = true;
			}
			if (other.name.StartsWith("Collision_GetBoat"))
			{
				ProcessCollision_GetBoat(other, bOnBoat);
			}
		}
	}

	public void OptimizeShowBoat(string id)
	{
		COMA_Fishing_SceneController.Instance.SetBoatActive(true, int.Parse(id) - 1);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		bool flag = false;
		int childSysTypeByTag = _centerController.GetChildSysTypeByTag(0);
		TFishingPlayerBuffController tFishingPlayerBuffController = GetSystemCtrlByType(childSysTypeByTag) as TFishingPlayerBuffController;
		if (tFishingPlayerBuffController.IsExitBuff(1))
		{
			flag = true;
		}
		if (flag && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && hit.collider.gameObject.name.StartsWith("CollisionOffBoat"))
		{
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1012, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
	}
}
