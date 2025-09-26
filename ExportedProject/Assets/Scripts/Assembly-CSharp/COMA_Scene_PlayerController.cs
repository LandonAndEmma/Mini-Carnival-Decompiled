using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Hall.C2S;
using Protocol.Hall.S2C;
using UnityEngine;

public class COMA_Scene_PlayerController : UIEntity
{
	private static COMA_Scene_PlayerController _instance;

	[SerializeField]
	private GameObject tarObj;

	[SerializeField]
	private Transform cmrTrs;

	[SerializeField]
	private UIJoyController joyCom;

	[SerializeField]
	private UIMoveController moveCom;

	private COMA_Square_Player playerCom;

	private Vector3 lastPos = Vector3.zero;

	private int[] locInfos = new int[200]
	{
		4, 0, -9, 0, -7, 0, -6, 0, -13, 0,
		-3, 0, 8, 0, 1, 0, -3, 0, -9, 0,
		-14, 0, 2, 0, 1, 0, 11, 0, -12, 0,
		6, 0, -3, 0, 8, 0, 12, 0, 6, 0,
		0, 0, -10, 0, -12, 0, -4, 0, -8, 0,
		8, 0, 1, 0, 10, 0, -3, 0, 13, 0,
		-6, 0, 10, 0, 8, 0, 7, 0, 12, 0,
		0, 0, -8, 0, 4, 0, 9, 0, -6, 0,
		12, 0, 2, 0, 12, 0, -6, 0, -11, 0,
		8, 0, 10, 0, -2, 0, -7, 0, -2, 0,
		-9, 0, -9, 0, 6, 0, -12, 0, 8, 0,
		-3, 0, 7, 0, 5, 0, 5, 0, 8, 0,
		0, 0, 10, 180, -11, 0, 1, 180, -6, 0,
		7, 180, 5, 0, 12, 180, 10, 0, 10, 180,
		-8, 0, 1, 180, 10, 0, -9, 180, -5, 0,
		-9, 180, 14, 0, 2, 180, 1, 0, -9, 180,
		-9, 0, -1, 90, -10, 0, 4, 90, -14, 0,
		4, 90, -12, 0, -1, 90, -12, 0, 3, 90,
		6, 0, -5, 270, 8, 0, -1, 270, 9, 0,
		5, 270, 14, 0, -3, 270, 14, 0, 5, 270
	};

	private bool _ignoreMoveMsg;

	private List<uint> m_who_ignoreMoveList = new List<uint>();

	private float interval;

	public static COMA_Scene_PlayerController Instance
	{
		get
		{
			return _instance;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_Hall_RoleEnter, this, OnRoleEnter);
		RegisterMessage(EUIMessageID.UI_Hall_RoleLeave, this, OnRoleLeave);
		RegisterMessage(EUIMessageID.UI_Hall_RoleMove, this, OnRoleMove);
		RegisterMessage(EUIMessageID.UI_Hall_RoleSit, this, OnRoleSit);
		RegisterMessage(EUIMessageID.UI_Hall_ReturnAllIDs, this, OnReturnHallIDs);
		RegisterMessage(EUIMessageID.UI_Hall_ChangeSquare, this, OnChangeSquare);
		RegisterMessage(EUIMessageID.UI_Hall_UpdateSelfAvatarOnSquare, this, UpdateSelfAvatar);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this, RemoteWatchInfoArrived);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_Hall_RoleEnter, this);
		UnregisterMessage(EUIMessageID.UI_Hall_RoleLeave, this);
		UnregisterMessage(EUIMessageID.UI_Hall_RoleMove, this);
		UnregisterMessage(EUIMessageID.UI_Hall_RoleSit, this);
		UnregisterMessage(EUIMessageID.UI_Hall_ReturnAllIDs, this);
		UnregisterMessage(EUIMessageID.UI_Hall_ChangeSquare, this);
		UnregisterMessage(EUIMessageID.UI_Hall_UpdateSelfAvatarOnSquare, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this);
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_RequestAllIDs, this, null);
		CreateSelf();
		joyCom._dele = OnJoyStick;
		moveCom._joyStick = OnJoyStickTo;
		moveCom._dele = OnMoveStick;
		EnterHallCmd extraInfo = new EnterHallCmd();
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
	}

	private new void OnDestroy()
	{
		_instance = null;
		LeaveHallCmd extraInfo = new LeaveHallCmd();
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
	}

	public void OnJoyStick(float x, float y)
	{
		if (playerCom != null)
		{
			playerCom.PlayerMove(x, y);
		}
	}

	public void OnJoyStickTo(float x, float y, Vector3 tar)
	{
		if (playerCom != null)
		{
			playerCom.PlayerMoveTo(x, y, tar);
		}
	}

	public void OnMoveStick(float x, float y)
	{
		float num = x * COMA_Sys.Instance.sensitivity;
		float y2 = num * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y2, 0f);
		cmrTrs.rotation *= quaternion;
		if (Application.loadedLevelName == "UI.Square")
		{
			float num2 = y * COMA_Sys.Instance.sensitivity;
			float z = num2 * 314f * 2f / (float)Screen.height;
			Quaternion quaternion2 = Quaternion.Euler(0f, 0f, z);
			cmrTrs.GetChild(0).rotation *= quaternion2;
			Quaternion rotation = cmrTrs.GetChild(0).rotation;
			if (rotation.eulerAngles.z > 290f)
			{
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 290f);
			}
			else if (rotation.eulerAngles.z < 230f)
			{
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 230f);
			}
			cmrTrs.GetChild(0).rotation = rotation;
		}
	}

	private bool OnChangeSquare(TUITelegram msg)
	{
		for (int num = base.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = base.transform.GetChild(num);
			if (child.name != playerCom.name)
			{
				Object.DestroyObject(child.gameObject);
			}
		}
		return true;
	}

	private bool OnReturnHallIDs(TUITelegram msg)
	{
		Dictionary<uint, NotifyEnterHallCmd> dictionary = (Dictionary<uint, NotifyEnterHallCmd>)msg._pExtraInfo;
		foreach (KeyValuePair<uint, NotifyEnterHallCmd> item in dictionary)
		{
			CreateOther(item.Value.m_info, item.Value.m_pos);
		}
		return true;
	}

	private void CreateSelf()
	{
		uint selfTID = UIGolbalStaticFun.GetSelfTID();
		int num = Random.Range(0, locInfos.Length / 4);
		Position position = new Position();
		position.m_x = locInfos[num * 4];
		position.m_y = 0;
		position.m_z = locInfos[num * 4 + 2];
		position.m_d = locInfos[num * 4 + 3];
		Debug.Log(num + "  " + position.m_x);
		GameObject gameObject = Object.Instantiate(tarObj) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(position.m_x, position.m_y, position.m_z);
		gameObject.transform.localEulerAngles = new Vector3(0f, position.m_d, 0f);
		gameObject.name = selfTID.ToString();
		RoleInfo playerInfo = UIDataBufferCenter.Instance.playerInfo;
		COMA_Square_Player component = gameObject.GetComponent<COMA_Square_Player>();
		component.SetNickname(playerInfo.m_name);
		component.SetTrs(position);
		lastPos = new Vector3(position.m_x, position.m_y, position.m_z);
		component.SetCamera(cmrTrs);
		playerCom = component;
		string[] md5s = new string[3]
		{
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_body),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_leg)
		};
		string[] serials = new string[7]
		{
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head_top),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head_front),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head_back),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head_left),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_head_right),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_chest_front),
			UIGolbalStaticFun.GetBagItemUINTByID(playerInfo.m_chest_back)
		};
		playerCom.SetAvatar(md5s, serials);
		Move(position);
	}

	private bool UpdateSelfAvatar(TUITelegram msg)
	{
		RoleInfo playerInfo = UIDataBufferCenter.Instance.playerInfo;
		playerCom.UpdateAvatar(playerInfo);
		return true;
	}

	private void CreateOther(WatchRoleInfo m_info, Position m_pos)
	{
		uint player_id = m_info.m_player_id;
		GameObject gameObject = Object.Instantiate(tarObj) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(m_pos.m_x, m_pos.m_y, m_pos.m_z);
		gameObject.transform.localEulerAngles = new Vector3(0f, m_pos.m_d, 0f);
		gameObject.name = player_id.ToString();
		COMA_Square_Player component = gameObject.GetComponent<COMA_Square_Player>();
		component.SetNickname(m_info.m_name);
		component.SetTrs(m_pos);
		component.SetAvatar(new string[3] { m_info.m_head, m_info.m_body, m_info.m_leg }, new string[7] { m_info.m_head_top, m_info.m_head_front, m_info.m_head_back, m_info.m_head_left, m_info.m_head_right, m_info.m_chest_front, m_info.m_chest_back });
	}

	private bool OnRoleEnter(TUITelegram msg)
	{
		WatchRoleInfo info = msg._pExtraInfo as WatchRoleInfo;
		Position pos = msg._pExtraInfo2 as Position;
		CreateOther(info, pos);
		return true;
	}

	private bool OnRoleLeave(TUITelegram msg)
	{
		uint num = (uint)msg._pExtraInfo;
		Transform transform = base.transform.FindChild(num.ToString());
		if (transform != null)
		{
			Object.DestroyObject(transform.gameObject);
		}
		return true;
	}

	private bool OnRoleMove(TUITelegram msg)
	{
		uint item = (uint)msg._pExtraInfo;
		Position trs = msg._pExtraInfo2 as Position;
		if (m_who_ignoreMoveList.Contains(item))
		{
			return true;
		}
		Transform transform = base.transform.FindChild(item.ToString());
		if (transform != null)
		{
			COMA_Square_Player component = transform.GetComponent<COMA_Square_Player>();
			component.SetTrs(trs);
		}
		return true;
	}

	private bool OnRoleSit(TUITelegram msg)
	{
		NotifyRelayDataCmd notifyRelayDataCmd = msg._pExtraInfo as NotifyRelayDataCmd;
		uint who = notifyRelayDataCmd.m_who;
		Transform transform = base.transform.FindChild(who.ToString());
		if (transform != null)
		{
			COMA_Square_Player component = transform.GetComponent<COMA_Square_Player>();
			string[] array = notifyRelayDataCmd.m_data.Split(',');
			if (array[0] == "0")
			{
				_ignoreMoveMsg = true;
				if (!m_who_ignoreMoveList.Contains(who))
				{
					m_who_ignoreMoveList.Add(who);
				}
				Vector3 vector = new Vector3(float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
				Quaternion rotation = Quaternion.Euler(float.Parse(array[4]), float.Parse(array[5]), float.Parse(array[6]));
				transform.position = vector;
				transform.rotation = rotation;
				component.SetTarPos(vector);
				component.characterCom.PlayMyAnimation("Sit_Down", "W000");
			}
			else if (array[0] == "1")
			{
				_ignoreMoveMsg = true;
				if (!m_who_ignoreMoveList.Contains(who))
				{
					m_who_ignoreMoveList.Add(who);
				}
				component.characterCom.PlayMyAnimation("Stand_Idle", "W000");
			}
			else if (array[0] == "2")
			{
				_ignoreMoveMsg = false;
				if (m_who_ignoreMoveList.Contains(who))
				{
					m_who_ignoreMoveList.Remove(who);
				}
				component.characterCom.PlayMyAnimation("Stand_Up", "W000");
			}
		}
		return true;
	}

	protected override void Tick()
	{
		base.Tick();
		interval += Time.deltaTime;
		if (interval > 2f)
		{
			interval = 0f;
			if (playerCom != null && (playerCom.transform.position - lastPos).sqrMagnitude > 0.1f)
			{
				Move(playerCom.transform);
				lastPos = playerCom.transform.position;
			}
		}
	}

	public void EnterSit(Transform trs)
	{
		RelayDataCmd relayDataCmd = new RelayDataCmd();
		string text = (relayDataCmd.m_data = "0," + trs.position.x + "," + trs.position.y + "," + trs.position.z + "," + trs.rotation.eulerAngles.x + "," + trs.rotation.eulerAngles.y + "," + trs.rotation.eulerAngles.z + "0");
		relayDataCmd.m_data_size = (ushort)relayDataCmd.m_data.Length;
		Debug.LogWarning("sit data=" + text + "   Size=" + relayDataCmd.m_data_size);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, relayDataCmd);
	}

	public void EnterSitting()
	{
		RelayDataCmd relayDataCmd = new RelayDataCmd();
		string data = "1,00";
		relayDataCmd.m_data = data;
		relayDataCmd.m_data_size = (ushort)relayDataCmd.m_data.Length;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, relayDataCmd);
	}

	public void EneterLeaveSit(Transform trs)
	{
		RelayDataCmd relayDataCmd = new RelayDataCmd();
		string data = "2," + trs.position.x + "," + trs.position.y + "," + trs.position.z + "," + trs.rotation.eulerAngles.x + "," + trs.rotation.eulerAngles.y + "," + trs.rotation.eulerAngles.z + "0";
		relayDataCmd.m_data = data;
		relayDataCmd.m_data_size = (ushort)relayDataCmd.m_data.Length;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, relayDataCmd);
	}

	public void Move(Transform trs)
	{
		Position position = new Position();
		position.m_x = (int)trs.position.x;
		position.m_y = (int)trs.position.y;
		position.m_z = (int)trs.position.z;
		position.m_d = (int)trs.localEulerAngles.y;
		Move(position);
	}

	private void Move(Position pos)
	{
		MoveRoleCmd moveRoleCmd = new MoveRoleCmd();
		moveRoleCmd.m_pos = pos;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, moveRoleCmd);
	}

	private bool RemoteWatchInfoArrived(TUITelegram msg)
	{
		Debug.LogWarning("RemoteWatchInfoArrived");
		WatchRoleInfo watchRoleInfo = msg._pExtraInfo as WatchRoleInfo;
		if (watchRoleInfo != null)
		{
			Debug.LogWarning(watchRoleInfo.m_player_id);
			string text = UIGolbalStaticFun.GetSelfTID().ToString();
			int i = 0;
			for (int childCount = base.transform.childCount; i < childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (!(child.name == text) && child.name == watchRoleInfo.m_player_id.ToString())
				{
					Debug.LogWarning(watchRoleInfo.m_name);
					COMA_Square_Player component = child.GetComponent<COMA_Square_Player>();
					component.SetNickname(watchRoleInfo.m_name);
					component.SetAvatar(new string[3] { watchRoleInfo.m_head, watchRoleInfo.m_body, watchRoleInfo.m_leg }, new string[7] { watchRoleInfo.m_head_top, watchRoleInfo.m_head_front, watchRoleInfo.m_head_back, watchRoleInfo.m_head_left, watchRoleInfo.m_head_right, watchRoleInfo.m_chest_front, watchRoleInfo.m_chest_back });
					component.WearCloth();
				}
			}
		}
		return true;
	}
}
