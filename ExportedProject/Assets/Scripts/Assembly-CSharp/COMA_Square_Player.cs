using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using UnityEngine;

public class COMA_Square_Player : UIEntity
{
	protected enum ESitState
	{
		None = 0,
		EnterMove = 1,
		Moving = 2,
		EnterSit = 3,
		Sitting = 4,
		EnterSitIdle = 5,
		SitIdling = 6,
		EneterLeaveSit = 7,
		LeaveSitting = 8
	}

	[SerializeField]
	private CharacterController cCtl;

	[SerializeField]
	public COMA_PlayerCharacter characterCom;

	[SerializeField]
	private Transform shadowTrs;

	[SerializeField]
	private COMA_DialogBox dialogBoxCom;

	private Transform cmrNodeTrs;

	private HallPlayerState state;

	private Vector3 tarPos = Vector3.zero;

	private Vector3 tarRot = Vector3.zero;

	private string _nickname = string.Empty;

	private string[] _md5s;

	private string[] _serials;

	private float fEnterSitTime;

	private float fSitTimeDur;

	private float fLeaveSitTime;

	private float fLeaveTimeDur;

	private Vector3 downDir;

	private Vector3 downPos;

	private Vector3 moveCur = Vector3.zero;

	private Vector3 moveto = Vector3.zero;

	private bool bMovingTo;

	private float fStartMovingTime;

	private float fMovingDur;

	private Vector3 curDir = Vector3.zero;

	private GameObject _objMoveSign;

	private float fCoolingHit;

	private ESitState curSitState;

	private bool bSitMode;

	private Transform curSitTrans;

	private Vector3 preSitPos;

	private Quaternion preSitRot;

	private float _preChangeSquare;

	private float _preOpenGames;

	private float _preOpenMap;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this, OnChatHistoryChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this);
	}

	private new void OnDestroy()
	{
	}

	private void Start()
	{
		dialogBoxCom.playerNickName = _nickname;
		WearCloth();
	}

	public void WearCloth()
	{
		for (int i = 0; i < characterCom.bodyObjs.Length; i++)
		{
			if (_md5s[i] == string.Empty)
			{
				characterCom.bodyObjs[i].renderer.material.mainTexture = UIGolbalStaticFun.CreateDefaultTexture(i);
				continue;
			}
			GameObject tarObj = characterCom.bodyObjs[i];
			UIDataBufferCenter.Instance.FetchTexture2DByMD5(_md5s[i], delegate(Texture2D tex)
			{
				if ((bool)tarObj)
				{
					tarObj.renderer.material.mainTexture = tex;
				}
			});
		}
		characterCom.RemoveAllAccounterment();
		for (int num = 0; num < _serials.Length; num++)
		{
			if (_serials[num] != string.Empty)
			{
				characterCom.CreateAccouterment(_serials[num]);
			}
		}
	}

	private bool OnChatHistoryChanged(TUITelegram msg)
	{
		if (msg._pExtraInfo2 == null)
		{
			List<UISquare_ChatRecordBoxData> list = (List<UISquare_ChatRecordBoxData>)msg._pExtraInfo;
			if (list.Count <= 0)
			{
				return true;
			}
			UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = list[list.Count - 1];
			if (uISquare_ChatRecordBoxData.OtherPeopleID.ToString() == base.name && uISquare_ChatRecordBoxData.Channel == Protocol.Channel.hall && !UIDataBufferCenter.Instance.IsPlayerIDInBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID))
			{
				dialogBoxCom.Chatting(uISquare_ChatRecordBoxData.OtherPeopleChatContent);
			}
		}
		return true;
	}

	public void SetNickname(string nickname)
	{
		_nickname = nickname;
	}

	public void SetAvatar(string[] md5s, string[] serials)
	{
		_md5s = md5s;
		_serials = serials;
	}

	public void UpdateAvatar(RoleInfo info)
	{
		_md5s[0] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head);
		_md5s[1] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_body);
		_md5s[2] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_leg);
		_serials[0] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head_top);
		_serials[1] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head_front);
		_serials[2] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head_back);
		_serials[3] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head_left);
		_serials[4] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_head_right);
		_serials[5] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_chest_front);
		_serials[6] = UIGolbalStaticFun.GetBagItemUINTByID(info.m_chest_back);
		WearCloth();
	}

	public void SetState(HallPlayerState st)
	{
		state = st;
	}

	public void SetTarPos(Vector3 v)
	{
		tarPos = v;
	}

	public void SetTrs(Position pos)
	{
		tarPos = new Vector3(pos.m_x, pos.m_y, pos.m_z);
		tarRot = new Vector3(0f, pos.m_d, 0f);
	}

	public void SetCamera(Transform cmrTrs)
	{
		cmrNodeTrs = cmrTrs;
	}

	protected override void Tick()
	{
		base.Tick();
		if (cmrNodeTrs != null)
		{
			if (bSitMode)
			{
				switch (curSitState)
				{
				case ESitState.EnterSit:
					DestoryMoveToSign();
					COMA_Scene_PlayerController.Instance.Move(base.transform);
					base.transform.position = curSitTrans.position;
					base.transform.rotation = curSitTrans.rotation;
					characterCom.PlayMyAnimation("Sit_Down", "W000");
					curSitState = ESitState.Sitting;
					fEnterSitTime = Time.time;
					fSitTimeDur = characterCom.animation["Sit_Down"].length;
					COMA_Scene_PlayerController.Instance.EnterSit(base.transform);
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Click_seating, base.transform);
					break;
				case ESitState.Sitting:
					if (Time.time - fEnterSitTime >= fSitTimeDur)
					{
						characterCom.PlayMyAnimation("Stand_Idle", "W000");
						curSitState = ESitState.SitIdling;
						COMA_Scene_PlayerController.Instance.EnterSitting();
					}
					break;
				case ESitState.EneterLeaveSit:
					characterCom.PlayMyAnimation("Stand_Up", "W000");
					curSitState = ESitState.LeaveSitting;
					fLeaveSitTime = Time.time;
					fLeaveTimeDur = characterCom.animation["Stand_Up"].length;
					downDir = curDir;
					downPos = downDir.normalized;
					downPos *= 0.5f;
					downPos += curSitTrans.position;
					COMA_Scene_PlayerController.Instance.EneterLeaveSit(base.transform);
					break;
				case ESitState.LeaveSitting:
					if (Time.time - fLeaveSitTime >= fLeaveTimeDur)
					{
						Vector3 position = base.transform.position;
						position.y = 0f;
						base.transform.position = position;
						bSitMode = false;
						fStartMovingTime = Time.time;
						Debug.Log("Re Pos:" + base.transform.position);
						fCoolingHit = Time.time + 0.8f;
					}
					else if (Time.time - fLeaveSitTime > 0.87f)
					{
						base.transform.position = Vector3.Lerp(base.transform.position, preSitPos, fLeaveSitTime);
						TurnAround(curDir);
					}
					break;
				}
			}
			else if (bMovingTo)
			{
				characterCom.PlayMyAnimation("Run", "W000", "Front");
				float num = Vector3.Distance(base.transform.position, moveto);
				if (num > 0.1f)
				{
					moveCur = (moveto - base.transform.position).normalized * 3.5f;
					TurnAround(curDir);
					cCtl.Move(moveCur * Time.deltaTime);
				}
				else
				{
					DestoryMoveToSign();
					moveCur = Vector3.zero;
					characterCom.PlayMyAnimation("Idle", "W000");
					bMovingTo = false;
				}
			}
			cmrNodeTrs.position = base.transform.position;
		}
		else if (Vector3.Distance(base.transform.localPosition, tarPos) > 0.1f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, tarPos, 0.1f);
			base.transform.localEulerAngles = Vector3.Lerp(base.transform.localEulerAngles, tarRot, 0.1f);
			characterCom.PlayMyAnimation("Run", "W000", "Front");
		}
		else
		{
			characterCom.PlayMyAnimation("Idle", "W000");
		}
	}

	public void PlayerMove(float x, float y)
	{
	}

	private void MarkMoveToSign(Vector3 pos)
	{
		DestoryMoveToSign();
		_objMoveSign = Object.Instantiate(Resources.Load("Particle/effect/RPG/Scenario/RPG_Arrow/RPG_Arrow")) as GameObject;
		_objMoveSign.transform.position = pos;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Click_move, base.transform);
	}

	private void DestoryMoveToSign()
	{
		if (_objMoveSign != null)
		{
			Object.Destroy(_objMoveSign);
			_objMoveSign = null;
		}
	}

	public void PlayerMoveTo(float x, float y, Vector3 tar)
	{
		if (bSitMode)
		{
			if (curSitState < ESitState.SitIdling)
			{
				return;
			}
			if (curSitState == ESitState.SitIdling)
			{
				curSitState = ESitState.EneterLeaveSit;
			}
		}
		tar.y = base.transform.position.y;
		Debug.Log("PlayerMoveTo：" + moveto);
		float num = Vector3.Distance(tar, base.transform.position);
		if (num > 0.1f)
		{
			moveto = tar;
			curDir = tar - base.transform.position;
			moveCur = (tar - base.transform.position).normalized * 3.5f;
			fStartMovingTime = Time.time;
			fMovingDur = num / 3.5f;
			bMovingTo = true;
			MarkMoveToSign(moveto);
			if (!bSitMode || curSitState <= ESitState.SitIdling)
			{
				characterCom.PlayMyAnimation("Run", "W000", "Front");
				TurnAround(tar - base.transform.position);
			}
		}
		else
		{
			moveCur = Vector3.zero;
			bMovingTo = false;
			if (!bSitMode || curSitState <= ESitState.SitIdling)
			{
				characterCom.PlayMyAnimation("Idle", "W000");
			}
		}
	}

	private void TurnAround(Vector3 spdV3)
	{
		float num = Vector3.Angle(base.transform.forward, spdV3);
		if (num > 150f)
		{
			if (Vector3.Cross(spdV3, base.transform.forward).y > 0f)
			{
				base.transform.rotation *= new Quaternion(0f, 0.7f, 0f, 0.7f);
			}
			else
			{
				base.transform.rotation *= new Quaternion(0f, -0.7f, 0f, 0.7f);
			}
		}
		else
		{
			base.transform.forward = Vector3.Lerp(base.transform.forward, spdV3, 0.3f);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		GameObject gameObject = hit.collider.gameObject;
		Transform transform = gameObject.transform.Find("Sit");
		if (transform != null && !bSitMode && Time.time >= fCoolingHit)
		{
			Debug.Log("Hit Sit");
			bSitMode = true;
			curSitTrans = transform;
			preSitPos = base.transform.position;
			preSitRot = base.transform.rotation;
			curSitState = ESitState.EnterSit;
			Debug.Log("Pre Pos:" + preSitPos);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (base.gameObject.name != UIGolbalStaticFun.GetSelfTID().ToString())
		{
			return;
		}
		if (other.name == "ChangeSquare" && Time.time - _preChangeSquare >= 3f)
		{
			Debug.Log("ChangeSquare");
			_preChangeSquare = Time.time;
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("guangchang_desc1"));
			uIMessage_CommonBoxData.Mark = "ChangeSquare";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Refresh);
		}
		else if (other.name == "door_games" && Time.time - _preOpenGames >= 5f)
		{
			Debug.Log("door_games");
			_preOpenGames = Time.time;
			if (COMA_Pref.Instance.NG2_1_FirstEnterSmallGame)
			{
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("NoviceProcess_90"));
				UIGolbalStaticFun.PopCommonMessageBox(data);
				COMA_Pref.Instance.NG2_1_FirstEnterSmallGame = false;
				COMA_Pref.Instance.Save(true);
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenGameContent, null, null);
		}
		else if (other.name == "door_map" && Time.time - _preOpenMap >= 5f)
		{
			Debug.Log("door_map");
			_preOpenMap = Time.time;
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_GotoMap, null, null);
		}
	}
}
