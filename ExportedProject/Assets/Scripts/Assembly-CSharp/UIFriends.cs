using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriends : UIMessageHandler
{
	private static UIFriends _instance;

	public COMA_PlayerCharacter characterCom;

	[SerializeField]
	private TUIScrollList_Avatar _scrollListModes;

	[SerializeField]
	private TUIScrollList_Avatar _scrollListPending;

	private UIFriends_OneFriend _curSelFriend;

	[SerializeField]
	private UIFriends_FriendMgr _friendMgr;

	[SerializeField]
	private TUILabel _selfIdLabel;

	[SerializeField]
	private UIFriends_FriendRequestMgr _frMgr;

	[SerializeField]
	private TUILabel _pendingNumLabel;

	[SerializeField]
	private TUILabel _addFriendsAwardsLabel;

	[SerializeField]
	private TUILabel _addFriendsAwardsNumLabel;

	[SerializeField]
	private TUIMeshSprite _addFriendsAwardsIcon;

	private Texture2D textureDefault;

	private UI_WaitingBox _waitingBox1;

	private int takePhotoCount;

	[SerializeField]
	private GameObject _inputName;

	[SerializeField]
	private UIInputName_InputBox nameInputBox;

	private int[] friendsRewards = new int[30]
	{
		-25, 500, -5, 550, -10, 600, -15, 650, -20, 700,
		-25, 750, -30, 800, -30, 850, -30, 900, -30, 950,
		-30, 1000, -30, 1100, -30, 1200, -30, 1300, 2000, -50
	};

	private List<int> showRewardList = new List<int>();

	private bool showRewardLock;

	public static UIFriends Instance
	{
		get
		{
			return _instance;
		}
	}

	public string strSelfId
	{
		set
		{
			_selfIdLabel.Text = value;
		}
	}

	private int PendingNum
	{
		get
		{
			return int.Parse(_pendingNumLabel.Text);
		}
		set
		{
			_pendingNumLabel.Text = value.ToString();
			if (value <= 0)
			{
				_pendingNumLabel.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				_pendingNumLabel.transform.parent.gameObject.SetActive(true);
			}
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	public void InitAddFriendsAwards(int nLackNum, Texture2D tex, int awardNum)
	{
		if (nLackNum < 0)
		{
			_addFriendsAwardsLabel.gameObject.SetActive(false);
			return;
		}
		string text = TUITool.StringFormat(TUITextManager.Instance().GetString("haoyoujiemian_desc9"), nLackNum);
		_addFriendsAwardsLabel.Text = text;
		_addFriendsAwardsNumLabel.Text = Mathf.Abs(awardNum).ToString();
		_addFriendsAwardsIcon.UseCustomize = false;
		if (awardNum < 0)
		{
			_addFriendsAwardsIcon.texture = "title_gem";
		}
		else
		{
			_addFriendsAwardsIcon.texture = "title_gold";
		}
	}

	public void ProcessSelFriend(UIFriends_OneFriend cur)
	{
		if (_curSelFriend != null)
		{
			_curSelFriend.NotityGetFocus(false);
		}
		_curSelFriend = cur;
		_curSelFriend.NotityGetFocus(true);
	}

	public void ProcessAddFriend()
	{
		if (COMA_Server_Friends.Instance.lst_friends.Count >= COMA_Server_Friends.Instance.friendsCountMax)
		{
			TUI_MsgBox.Instance.MessageBox(130);
			return;
		}
		COMA_CommonOperation.Instance.defaultInput = string.Empty;
		_inputName.SetActive(true);
	}

	public void ProcessRefreshFriend(UIFriends_OneFriend one)
	{
		Debug.Log("Refresh Friend:" + one.FriendData.FriendName);
		Debug.Log(one.FriendData.GID);
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/Misc/WaitingBox")) as GameObject;
		gameObject.transform.parent = one.transform;
		_waitingBox1 = gameObject.GetComponent<UI_WaitingBox>();
		_waitingBox1.StartWaiting(one.GetComponent<TUIControlImpl>());
		Object.Destroy(gameObject, 10f);
		int num = -1;
		for (int i = 0; i < COMA_Server_Friends.Instance.lst_friends.Count; i++)
		{
			if (COMA_Server_Friends.Instance.lst_friends[i].GID == one.FriendData.GID)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			Debug.LogError(num);
		}
		COMA_Server_Friends.Instance.GetFriendArchive(COMA_Server_Friends.Instance.lst_friends[num].GID, COMA_Server_Friends.Instance.lst_friends[num].url_archive, "friends_" + num);
		COMA_Server_Friends.Instance.lst_friends[num].textures[0] = null;
		COMA_Server_Friends.Instance.lst_friends[num].textures[1] = null;
		COMA_Server_Friends.Instance.lst_friends[num].textures[2] = null;
		COMA_Server_Friends.Instance.GetFriendTexture(num);
	}

	public void OnRefreshFriend(int i)
	{
		if (_waitingBox1 != null && _waitingBox1.gameObject != null)
		{
			Object.Destroy(_waitingBox1.gameObject);
		}
		StartCoroutine(OnRefreshFriend_TakePhoto(i));
	}

	private IEnumerator OnRefreshFriend_TakePhoto(int i)
	{
		takePhotoCount++;
		yield return new WaitForSeconds((float)takePhotoCount * 0.5f);
		for (int j = 0; j < COMA_Server_Friends.Instance.lst_friends[i].accounterments.Length; j++)
		{
			characterCom.CreateAccouterment(COMA_Server_Friends.Instance.lst_friends[i].accounterments[j]);
		}
		characterCom.transform.FindChild("head").renderer.material.mainTexture = COMA_Server_Friends.Instance.lst_friends[i].textures[0];
		characterCom.transform.FindChild("body").renderer.material.mainTexture = COMA_Server_Friends.Instance.lst_friends[i].textures[1];
		characterCom.transform.FindChild("breeches").renderer.material.mainTexture = COMA_Server_Friends.Instance.lst_friends[i].textures[2];
		_friendMgr.SetFriendTexture(i, IconShot.Instance.GetIconPic(characterCom.gameObject, false, 192, 192));
		characterCom.RemoveAllAccounterment();
	}

	public void ProcessAcceptFriend(UIFriends_FriendRequestData data)
	{
		Debug.Log("ProcessAcceptFriend:" + data.nickname);
		RefreshPendingList(data.GID);
		if (COMA_Server_Friends.Instance.lst_friends.Count >= COMA_Server_Friends.Instance.friendsCountMax)
		{
			TUI_MsgBox.Instance.MessageBox(130);
			return;
		}
		_friendMgr.AddFriend(data.GID, data.nickname, textureDefault);
		COMA_Server_Friends.Instance.lst_friends.Add(data);
		COMA_Server_Friends.Instance.lst_requires.Remove(data);
		COMA_Server_Friends.Instance.AcceptFriend(COMA_Server_ID.Instance.GID, data.GID);
		CheckFriendsReward();
	}

	public void ProcessRejectFriend(UIFriends_FriendRequestData data)
	{
		Debug.Log("ProcessRejectFriend:" + data.nickname);
		RefreshPendingList(data.GID);
		COMA_Server_Friends.Instance.lst_requires.Remove(data);
		COMA_Server_Friends.Instance.DeleteRequire(COMA_Server_ID.Instance.GID, data.GID);
	}

	private void RefreshPendingList(string gid)
	{
		int num = -1;
		for (int i = 0; i < COMA_Server_Friends.Instance.lst_requires.Count; i++)
		{
			if (COMA_Server_Friends.Instance.lst_requires[i].GID == gid)
			{
				num = i;
				break;
			}
		}
		COMA_Server_Friends.Instance.lst_requires.RemoveAt(num);
		_scrollListPending.Remove(num, true);
		PendingNum = COMA_Server_Friends.Instance.lst_requires.Count;
	}

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		_frMgr.gameObject.SetActive(false);
		_inputName.SetActive(false);
	}

	private void Start()
	{
		showRewardList.Clear();
		takePhotoCount = 0;
		strSelfId = "ID:" + COMA_CommonOperation.Instance.GIDForShow(COMA_Server_ID.Instance.GID);
		PendingNum = 0;
		COMA_Server_Friends.Instance.lst_friends.Clear();
		COMA_Server_Friends.Instance.FriendList(COMA_Server_ID.Instance.GID);
		RequireListInterval();
		if (COMA_CommonOperation.Instance.bfriendRequireInverval)
		{
			SceneTimerInstance.Instance.Add(10f, RequireListInterval);
		}
		characterCom.transform.FindChild("head").renderer.material.mainTexture = Resources.Load("FBX/Player/Character/Texture/T_head") as Texture2D;
		characterCom.transform.FindChild("body").renderer.material.mainTexture = Resources.Load("FBX/Player/Character/Texture/T_body") as Texture2D;
		characterCom.transform.FindChild("breeches").renderer.material.mainTexture = Resources.Load("FBX/Player/Character/Texture/T_leg") as Texture2D;
		characterCom.animation["Idle00"].time = 0.2f;
		characterCom.animation.Sample();
		textureDefault = IconShot.Instance.GetIconPic(characterCom.gameObject, false, 192, 192);
	}

	public bool RequireListInterval()
	{
		COMA_Server_Friends.Instance.RequireList(COMA_Server_ID.Instance.GID);
		return true;
	}

	public void OnListRequire()
	{
		PendingNum = COMA_Server_Friends.Instance.lst_requires.Count;
	}

	public void OnListFriend()
	{
		int num = Mathf.Min(COMA_Server_Friends.Instance.lst_friends.Count, COMA_Server_Friends.Instance.friendsCountMax);
		for (int i = 0; i < num; i++)
		{
			_friendMgr.AddFriend(COMA_Server_Friends.Instance.lst_friends[i].GID, string.Empty, textureDefault);
		}
		CheckFriendsReward();
	}

	public void OnListAdd(int i)
	{
		_friendMgr.AddFriend(COMA_Server_Friends.Instance.lst_friends[i].GID, string.Empty, textureDefault);
		CheckFriendsReward();
	}

	public void AddNameFriend(int i, string nickname)
	{
		_friendMgr.SetFriendName(i, nickname);
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			if (_aniControl != null)
			{
				_aniControl.PlayExitAni("UI.MainMenu");
			}
			else
			{
				Application.LoadLevel("UI.MainMenu");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_MailBox(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (PendingNum > 0)
			{
				Debug.Log("Button_MailBox-CommandClick");
				_frMgr.gameObject.SetActive(true);
				_frMgr.InitPendingList();
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_CloseMailBox(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_frMgr.gameObject.SetActive(false);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void CheckDelFriend(string param)
	{
		if (_friendMgr.DelFriend(_curSelFriend) == 0)
		{
			Debug.Log("-----Del Friend success!!");
			COMA_Server_Friends.Instance.DeleteFriend(COMA_Server_ID.Instance.GID, _curSelFriend.FriendData.GID);
		}
	}

	public void HandleEventButton_Del(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Del-CommandClick");
			if (_curSelFriend != null)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(131);
				uI_MsgBox.AddProceYesHandler(CheckDelFriend);
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButtonInputBox_Name(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			nameInputBox.ShowKeyboard();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_CloseAddFriend(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_inputName.SetActive(false);
			break;
		}
	}

	public void HandleEventButton_OkFriend(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (!(nameInputBox.InputText == string.Empty))
			{
				Debug.Log("Add : " + nameInputBox.InputText);
				_inputName.SetActive(false);
				string text = COMA_CommonOperation.Instance.GIDFormat(nameInputBox.InputText);
				Debug.Log(text);
				COMA_Server_Friends.Instance.AddFriend(COMA_Server_ID.Instance.GID, text);
				TUI_MsgBox.Instance.MessageBox(132);
			}
			break;
		}
	}

	public void OnAddFriend(bool bTimeOut, string param)
	{
		if (bTimeOut)
		{
			Debug.Log("添加好友请求超时");
			return;
		}
		switch (param)
		{
		case "NotFoundUser":
			Debug.Log("没有对应玩家");
			TUI_MsgBox.Instance.MessageBox(127);
			break;
		case "IsFriend":
			Debug.Log("已经是你的好友了");
			TUI_MsgBox.Instance.MessageBox(128);
			break;
		case "CanNotAddSelf":
			Debug.Log("不许添加自己");
			TUI_MsgBox.Instance.MessageBox(129);
			break;
		default:
			Debug.Log("好友请求发送成功");
			break;
		}
	}

	private void CheckFriendsReward()
	{
		if (COMA_Pref.Instance.maxFriends >= friendsRewards.Length)
		{
			Debug.Log("没什么号奖励的了，隐藏奖励提示");
			InitAddFriendsAwards(-1, null, 0);
			return;
		}
		int num = Mathf.Min(COMA_Server_Friends.Instance.lst_friends.Count, friendsRewards.Length);
		if (COMA_Pref.Instance.maxFriends < num)
		{
			for (int i = COMA_Pref.Instance.maxFriends; i < num; i++)
			{
				if (friendsRewards[i] >= 0)
				{
					COMA_Pref.Instance.AddGold(friendsRewards[i]);
				}
				else
				{
					COMA_Pref.Instance.AddCrystal(-friendsRewards[i]);
				}
				showRewardList.Add(friendsRewards[i]);
			}
			COMA_Pref.Instance.maxFriends = num;
			COMA_Pref.Instance.Save(true);
		}
		int num2 = COMA_Pref.Instance.maxFriends - num + 1;
		Debug.Log("还需要" + num2 + "个好友，就能获得" + friendsRewards[COMA_Pref.Instance.maxFriends]);
		InitAddFriendsAwards(num2, null, friendsRewards[COMA_Pref.Instance.maxFriends]);
	}

	private new void Update()
	{
		if (showRewardList.Count > 0 && !showRewardLock)
		{
			if (showRewardList[0] >= 0)
			{
				TUI_MsgBox.Instance.TipBox(0, showRewardList[0], string.Empty, null);
			}
			else
			{
				TUI_MsgBox.Instance.TipBox(1, -showRewardList[0], string.Empty, null);
			}
			showRewardLock = true;
		}
	}

	public void ShowRewardRemove()
	{
		if (showRewardList.Count > 0)
		{
			showRewardList.RemoveAt(0);
			showRewardLock = false;
		}
	}
}
