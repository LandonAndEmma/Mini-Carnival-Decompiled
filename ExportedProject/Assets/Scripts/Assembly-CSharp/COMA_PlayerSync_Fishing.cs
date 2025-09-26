using UnityEngine;

public class COMA_PlayerSync_Fishing : COMA_PlayerSync
{
	[SerializeField]
	private COMA_Fishing_FishPole _curFishPole;

	[SerializeField]
	private COMA_Fishing_FishFloat _curFishFloat;

	private GameObject _objFetchFloat;

	private string _strBoatId = "-1";

	[SerializeField]
	private Animation _comAniCmp;

	public GameObject _objFishingLine;

	public Material _fishingMaterial;

	public PraiseBtn _praiseBtn;

	private COMA_Fishing_FishBoat _curUseBoat;

	private bool _bDrawFishLine;

	private float _fCanPraiseDisSquar = 9f;

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

	protected new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_TRANSFORM, base.ReceiveTransform);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.CHAT, ReceiveChatting);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_UPDATE_FISHROT, UpdateFishrot);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_ONBOAT, FishingOnBoat);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_OFFBOAT, FishingOffBoat);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_INFO, ReceiveFishingInfo);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_STATE, ReceiveFishingState);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.FISHING_PRAISE, ReceivePraise);
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_TRANSFORM, base.ReceiveTransform);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.CHAT, ReceiveChatting);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_UPDATE_FISHROT, UpdateFishrot);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_ONBOAT, FishingOnBoat);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_OFFBOAT, FishingOffBoat);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_INFO, ReceiveFishingInfo);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_STATE, ReceiveFishingState);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.FISHING_PRAISE, ReceivePraise);
		base.OnDisable();
	}

	protected void ReceivePraise(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_FishingPraise cOMA_CD_FishingPraise = commandDatas as COMA_CD_FishingPraise;
			if (cOMA_CD_FishingPraise.nId == COMA_PlayerSelf.Instance.id)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_BePraise, COMA_PlayerSelf.Instance.transform);
				COMA_Gift_Appreciation.Instance.Count++;
				COMA_Achievement.Instance.Grata++;
				COMA_Achievement.Instance.VeryGrata++;
				Debug.Log("ID:" + commandDatas.dataSender.Id + " praise you!");
				int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
				CPopupInfo extraInfo = new CPopupInfo(1, cOMA_CD_FishingPraise.sendName);
				TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1017, TTelegram.SEND_MSG_IMMEDIATELY, extraInfo);
			}
			Debug.Log("------------Receive Praise:" + cOMA_CD_FishingPraise.nId);
			Transform transform = base.transform.parent.Find(cOMA_CD_FishingPraise.nId.ToString());
			if (transform != null)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Interaction/Interaction_02_pfb")) as GameObject;
				gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
				gameObject.transform.parent = transform;
				gameObject.transform.localPosition = characterCom.transform.localPosition + new Vector3(0f, 1f, 0f);
				Object.DestroyObject(gameObject, 5f);
			}
		}
	}

	public void OnBoatPosOffsetStart()
	{
		characterCom.transform.position = _curUseBoat._transPlayerFishingNode.position;
		shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
		dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
		Vector3 localPosition = _praiseBtn.transform.localPosition;
		localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
		_praiseBtn.transform.localPosition = localPosition;
	}

	public void OnBoatPosOffsetEnd()
	{
		characterCom.transform.position = _curUseBoat._transPlayerNode.position;
		shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
		dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
		Vector3 localPosition = _praiseBtn.transform.localPosition;
		localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
		_praiseBtn.transform.localPosition = localPosition;
	}

	public void OffBoatPosOffset()
	{
		characterCom.transform.localPosition = Vector3.zero;
		shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
		dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
		Vector3 localPosition = _praiseBtn.transform.localPosition;
		localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
		_praiseBtn.transform.localPosition = localPosition;
	}

	protected void FishingOnBoat(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			string s = (_strBoatId = ((COMA_CD_FishingOnBoat)commandDatas).boatId);
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Boat/Boat")) as GameObject;
			gameObject.name = "Boat";
			gameObject.transform.parent = _comAniCmp.transform;
			gameObject.transform.localPosition = COMA_PlayerSelf_Fishing._boatLocPos;
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			_curUseBoat = gameObject.GetComponent<COMA_Fishing_FishBoat>();
			characterCom.transform.position = _curUseBoat._transPlayerNode.position;
			shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
			dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
			Vector3 localPosition = _praiseBtn.transform.localPosition;
			localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
			_praiseBtn.transform.localPosition = localPosition;
			COMA_PlayerSelf_Fishing._bBoatState[int.Parse(_strBoatId) - 1] = true;
			COMA_Fishing_SceneController.Instance.SetBoatActive(false, int.Parse(s) - 1);
			COMA_Fishing_SceneController.Instance._fOnBoatTimes[int.Parse(s) - 1] = Time.time;
		}
	}

	protected void FishingOffBoat(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			string boatId = ((COMA_CD_FishingOffBoat)commandDatas).boatId;
			COMA_PlayerSelf_Fishing._bBoatState[int.Parse(boatId) - 1] = false;
			Transform transform = base.transform.Find("AniLayer/Boat");
			Object.DestroyObject(transform.gameObject);
			characterCom.transform.localPosition = Vector3.zero;
			shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
			dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
			Vector3 localPosition = _praiseBtn.transform.localPosition;
			localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
			_praiseBtn.transform.localPosition = localPosition;
			COMA_Fishing_SceneController.Instance.SetBoatActive(true, int.Parse(boatId) - 1);
			_strBoatId = "-1";
		}
	}

	protected void ReceiveChatting(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_Chatting cOMA_CD_Chatting = commandDatas as COMA_CD_Chatting;
			Debug.Log(base.name + "----" + cOMA_CD_Chatting.chatting);
			Chat(cOMA_CD_Chatting.chatting);
			ChatHistoryInfo chatHistoryInfo = new ChatHistoryInfo();
			chatHistoryInfo._strName = cOMA_CD_Chatting.sendName;
			chatHistoryInfo._strWords = cOMA_CD_Chatting.chatting;
			chatHistoryInfo._strID = base.gameObject.name;
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1003, TTelegram.SEND_MSG_IMMEDIATELY, chatHistoryInfo);
		}
	}

	protected void UpdateFishrot(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_FishingUpdateFishrot cOMA_CD_FishingUpdateFishrot = commandDatas as COMA_CD_FishingUpdateFishrot;
			itemSelected = cOMA_CD_FishingUpdateFishrot.btType;
		}
	}

	protected void ReceiveFishingInfo(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_FishingInfo cOMA_CD_FishingInfo = commandDatas as COMA_CD_FishingInfo;
			COtherPlayerInfo cOtherPlayerInfo = new COtherPlayerInfo(cOMA_CD_FishingInfo.itemID, nickname);
			cOtherPlayerInfo._weight = cOMA_CD_FishingInfo.nFishWeight;
			cOtherPlayerInfo._playerId = cOMA_CD_FishingInfo.strId;
			Debug.Log(base.name + "----" + cOMA_CD_FishingInfo.itemID);
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1004, TTelegram.SEND_MSG_IMMEDIATELY, cOtherPlayerInfo);
		}
	}

	protected void ReceiveFishingState(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_FishingState cOMA_CD_FishingState = commandDatas as COMA_CD_FishingState;
			if (cOMA_CD_FishingState.nState == 1)
			{
				int nIndex = cOMA_CD_FishingState.nFishBoatID - 1;
				_strBoatId = cOMA_CD_FishingState.nFishBoatID.ToString();
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Boat/Boat")) as GameObject;
				gameObject.name = "Boat";
				gameObject.transform.parent = _comAniCmp.transform;
				gameObject.transform.localPosition = COMA_PlayerSelf_Fishing._boatLocPos;
				gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				_curUseBoat = gameObject.GetComponent<COMA_Fishing_FishBoat>();
				characterCom.transform.position = _curUseBoat._transPlayerNode.position;
				shadowTrs.localPosition = characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
				dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), characterCom.transform.localPosition);
				Vector3 localPosition = _praiseBtn.transform.localPosition;
				localPosition = new Vector3(_praiseBtn.transform.localPosition.x, dialogBoxCom.transform.localPosition.y, dialogBoxCom.transform.localPosition.z);
				_praiseBtn.transform.localPosition = localPosition;
				COMA_PlayerSelf_Fishing._bBoatState[int.Parse(_strBoatId) - 1] = true;
				COMA_Fishing_SceneController.Instance._fOnBoatTimes[int.Parse(_strBoatId) - 1] = Time.time - cOMA_CD_FishingState.fOnBoatDurTime;
				COMA_Fishing_SceneController.Instance.SetBoatActive(false, nIndex);
			}
		}
	}

	private new void Awake()
	{
		GameObject gameObject = null;
		gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishFloat")) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Fish Float CREATE FAILURE!");
		}
		_curFishFloat = gameObject.GetComponent<COMA_Fishing_FishFloat>();
		_curFishFloat.ChangeStateTo(COMA_Fishing_FishFloat.EState.SYN);
	}

	private new void Start()
	{
		base.Start();
	}

	public void DestoryFishingLine()
	{
		if (_objFishingLine != null)
		{
			Object.DestroyObject(_objFishingLine);
		}
	}

	public void DrawFishingLine()
	{
		DestoryFishingLine();
		_objFishingLine = new GameObject();
		LineRenderer lineRenderer = _objFishingLine.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(0.01f, 0.01f);
		Vector3 fishingPoleEndPos = CurFishPole.GetFishingPoleEndPos();
		Vector3 position = CurFishFloat.gameObject.transform.position;
		lineRenderer.SetVertexCount(12);
		int num = 0;
		lineRenderer.SetPosition(num, fishingPoleEndPos);
		num++;
		Vector3 v = (fishingPoleEndPos + position) * 0.5f;
		v.y = Mathf.Min(fishingPoleEndPos.y, position.y);
		Bezier3 bezier = new Bezier3(fishingPoleEndPos, v, position);
		for (float num2 = 0f; num2 < 1f; num2 += 0.1f)
		{
			Vector3 pointAtTime = bezier.GetPointAtTime(num2);
			lineRenderer.SetPosition(num, pointAtTime);
			num++;
		}
		lineRenderer.SetPosition(num, position);
		num++;
		lineRenderer.material = _fishingMaterial;
	}

	public void DrawFishingLine_Straight()
	{
	}

	private void GetFishPole()
	{
		GameObject gameObject = null;
		if (CurFishPole == null)
		{
			if (itemSelected == 0)
			{
				gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_black")) as GameObject;
			}
			else if (itemSelected == 1)
			{
				gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_silver")) as GameObject;
			}
			else if (itemSelected == 2)
			{
				gameObject = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FishPole_gold")) as GameObject;
			}
			if (gameObject != null)
			{
				_curFishPole = gameObject.GetComponent<COMA_Fishing_FishPole>();
				if (_curFishPole != null)
				{
					_curFishPole.transform.parent = characterCom.handTrs;
					_curFishPole.transform.localPosition = Vector3.zero;
					_curFishPole.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
				}
			}
			Debug.Log("=================================>itemSelected=" + itemSelected);
		}
		EnableFishPole(true);
		if (_strBoatId != "-1")
		{
			OnBoatPosOffsetStart();
		}
	}

	private void EnableFishPole(bool bActive)
	{
		if (_curFishPole != null)
		{
			_curFishPole.gameObject.SetActive(bActive);
		}
	}

	public override void NotifyPlayerExtraProcess(COMA_CommandDatas commandDatas, string aniName)
	{
		_bDrawFishLine = false;
		COMA_CD_PlayerAnimation cOMA_CD_PlayerAnimation = commandDatas as COMA_CD_PlayerAnimation;
		switch (aniName)
		{
		case "Fishing_cancelpole":
		{
			GetFishPole();
			Vector3 extra = cOMA_CD_PlayerAnimation.extra;
			if (CurFishPole != null)
			{
				CurFishPole._aniCmp.Play("pole_fetch");
			}
			_objFetchFloat = null;
			if (extra.x > 0f)
			{
				if (extra.x == 1f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Chest")) as GameObject;
				}
				else if (extra.x == 2f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Fish")) as GameObject;
				}
				else if (extra.x == 3f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Fish")) as GameObject;
				}
				_objFetchFloat.transform.parent = CurFishPole.GetFishingPoleItemPos();
				_objFetchFloat.transform.localPosition = Vector3.zero;
				_objFetchFloat.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			}
			else if (extra.x == 0f)
			{
				_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FetchFloat")) as GameObject;
				_objFetchFloat.transform.parent = CurFishPole.GetFishingPoleItemPos();
				_objFetchFloat.transform.localPosition = Vector3.zero;
				_objFetchFloat.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			DestoryFishingLine();
			return;
		}
		case "Fishing_cancelpole02":
		{
			GetFishPole();
			Vector3 extra2 = cOMA_CD_PlayerAnimation.extra;
			if (CurFishPole != null)
			{
				CurFishPole._aniCmp.Play("pole_fetch02");
			}
			_objFetchFloat = null;
			if (extra2.x > 0f)
			{
				if (extra2.x == 1f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Chest")) as GameObject;
				}
				else if (extra2.x == 2f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Fish")) as GameObject;
				}
				else if (extra2.x == 3f)
				{
					_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/Fish")) as GameObject;
				}
				_objFetchFloat.transform.parent = CurFishPole.GetFishingPoleItemPos();
				_objFetchFloat.transform.localPosition = Vector3.zero;
				_objFetchFloat.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
			}
			else if (extra2.x == 0f)
			{
				_objFetchFloat = Object.Instantiate(Resources.Load("FBX/Scene/Fishing/FetchFloat")) as GameObject;
				_objFetchFloat.transform.parent = CurFishPole.GetFishingPoleItemPos();
				_objFetchFloat.transform.localPosition = Vector3.zero;
				_objFetchFloat.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			DestoryFishingLine();
			return;
		}
		case "Fishing_castpole":
			GetFishPole();
			if (CurFishPole != null)
			{
				CurFishPole._aniCmp.Play("pole_cast");
			}
			return;
		case "Fishing_idle":
		{
			GetFishPole();
			Vector3 extra3 = cOMA_CD_PlayerAnimation.extra;
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = extra3;
			}
			DrawFishingLine();
			_bDrawFishLine = true;
			return;
		}
		case "Fishing_pullpole":
			GetFishPole();
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			if (CurFishPole != null)
			{
				CurFishPole._aniCmp.Play("pole_pull");
			}
			DestoryFishingLine();
			return;
		case "Fishing_pullpole02":
			GetFishPole();
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			if (CurFishPole != null)
			{
				CurFishPole._aniCmp.Play("pole_pull02");
			}
			DestoryFishingLine();
			return;
		case "Fishing_success":
			GetFishPole();
			if (_objFetchFloat != null)
			{
				Object.DestroyObject(_objFetchFloat);
			}
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			DestoryFishingLine();
			return;
		case "Fishing_unsuccess":
			GetFishPole();
			if (_objFetchFloat != null)
			{
				Object.DestroyObject(_objFetchFloat);
			}
			if (CurFishFloat != null)
			{
				CurFishFloat.gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);
			}
			DestoryFishingLine();
			return;
		}
		EnableFishPole(false);
		DestoryFishingLine();
		if (_strBoatId != "-1")
		{
			OnBoatPosOffsetEnd();
			switch (aniName)
			{
			case "Ship_Forward01":
				_curUseBoat.PlayBoatAni("Forward01");
				break;
			case "Ship_TurnLeft01":
				_curUseBoat.PlayBoatAni("TurnLeft01");
				break;
			case "Ship_TurnRight01":
				_curUseBoat.PlayBoatAni("TurnRight01");
				break;
			case "Ship_Idle01":
				_curUseBoat.PlayBoatAni("Idle01");
				_comAniCmp.animation.Play();
				break;
			}
		}
		else
		{
			_comAniCmp.animation.Stop();
			OffBoatPosOffset();
		}
	}

	public override void DumRes()
	{
		DestoryFishingLine();
		if (CurFishFloat != null)
		{
			Object.DestroyObject(CurFishFloat.gameObject);
		}
		if ("-1" != _strBoatId)
		{
			COMA_PlayerSelf_Fishing._bBoatState[int.Parse(_strBoatId) - 1] = false;
			COMA_Fishing_SceneController.Instance.SetBoatActive(true, int.Parse(_strBoatId) - 1);
			_strBoatId = "-1";
		}
	}

	private new void Update()
	{
		base.Update();
		if (characterCom.animation.IsPlaying("Fishing_idle") && _bDrawFishLine)
		{
			DrawFishingLine();
		}
		if (COMA_PlayerSelf.Instance != null && COMA_Fishing_SceneController.Instance.IsCanUsePraise())
		{
			Vector3 position = COMA_PlayerSelf.Instance.transform.position;
			Vector3 position2 = base.transform.position;
			position.y = 0f;
			position2.y = 0f;
			float sqrMagnitude = (position - position2).sqrMagnitude;
			if (sqrMagnitude <= _fCanPraiseDisSquar)
			{
				float headTopTextLength = dialogBoxCom.GetHeadTopTextLength();
				Vector3 localPosition = _praiseBtn._iconTrans.localPosition;
				localPosition.x = dialogBoxCom.transform.localPosition.x + 0.5f + headTopTextLength / 2f;
				_praiseBtn._iconTrans.localPosition = localPosition;
				_praiseBtn.gameObject.SetActive(true);
			}
			else
			{
				_praiseBtn.gameObject.SetActive(false);
			}
		}
		else
		{
			_praiseBtn.gameObject.SetActive(false);
		}
	}
}
