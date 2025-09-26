using System.Collections.Generic;
using UnityEngine;

public class UIRankingList : UIMessageHandler
{
	[SerializeField]
	private TUIControl[] _allGameModeBtn;

	private List<TUIControl> _gameModeBtnList;

	[SerializeField]
	private TUIScrollList_Avatar _scrollListGameMode;

	private TUIControl _curSelGameMode;

	private int _curSelModeIndex = -1;

	private TUIControl _preSelGameMode;

	[SerializeField]
	private UIRankingList_RankingArear[] _allRaningBtns;

	private int _preSelRaningBtnIndex = -1;

	private int _curSelRaningBtnIndex;

	[SerializeField]
	private GameObject _friendRankCaption;

	[SerializeField]
	private GameObject _worldRankCaption;

	[SerializeField]
	private TUILabel _remainingDaysLabel;

	[SerializeField]
	private TUILabel _remainingHoursLabel;

	private static UIRankingList _instance;

	[SerializeField]
	private UIRankingList_RankingContainer _rankingContainer;

	public TUIControl CurSelGameMode
	{
		get
		{
			return _curSelGameMode;
		}
		set
		{
			_preSelGameMode = _curSelGameMode;
			_curSelGameMode = value;
			if (_preSelGameMode != null)
			{
				UIRankingList_GameMode component = _preSelGameMode.GetComponent<UIRankingList_GameMode>();
				if (component != null)
				{
					component.NotifyGetFocus(false);
				}
			}
			if (_curSelGameMode != null)
			{
				UIRankingList_GameMode component2 = _curSelGameMode.GetComponent<UIRankingList_GameMode>();
				if (component2 != null)
				{
					component2.NotifyGetFocus(true);
				}
				CurSelModeIndex = component2.ModeIndex;
			}
		}
	}

	public int CurSelModeIndex
	{
		get
		{
			return _curSelModeIndex;
		}
		set
		{
			if (_curSelModeIndex != value)
			{
				NotifySelModeChanged(value);
			}
			_curSelModeIndex = value;
		}
	}

	public int CurSelRaningBtnIndex
	{
		get
		{
			return _curSelRaningBtnIndex;
		}
		set
		{
			_preSelRaningBtnIndex = _curSelRaningBtnIndex;
			if (_curSelRaningBtnIndex != value && value != -1)
			{
				NorifySelRaningModeChanged(value);
			}
			_curSelRaningBtnIndex = value;
			if (_preSelRaningBtnIndex != -1)
			{
				_allRaningBtns[_preSelRaningBtnIndex].NotifyGetFocus(false);
			}
			if (_curSelRaningBtnIndex != -1)
			{
				_allRaningBtns[_curSelRaningBtnIndex].NotifyGetFocus(true);
			}
		}
	}

	public int RemainingDays
	{
		set
		{
			_remainingDaysLabel.Text = value.ToString();
		}
	}

	public int RemainingHours
	{
		set
		{
			_remainingHoursLabel.Text = value.ToString();
		}
	}

	public static UIRankingList Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		_gameModeBtnList = new List<TUIControl>();
		_preSelGameMode = null;
		if (COMA_Version.Instance != null)
		{
			int num = COMA_Version.Instance.tickets.Length;
			int[] array = new int[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (COMA_Version.Instance.tickets[i] > 0 && COMA_Version.Instance.orders[i] != 5 && COMA_Version.Instance.orders[i] != 6 && COMA_Version.Instance.orders[i] != 7 && COMA_Version.Instance.orders[i] != 8 && COMA_Version.Instance.orders[i] != 9)
				{
					array[num2] = COMA_Version.Instance.orders[i];
					num2++;
				}
			}
			for (int j = 0; j < num2; j++)
			{
				_allGameModeBtn[array[j]].gameObject.SetActive(true);
				_gameModeBtnList.Add(_allGameModeBtn[array[j]]);
			}
			CurSelGameMode = _allGameModeBtn[array[0]];
		}
		else
		{
			_gameModeBtnList.Add(_allGameModeBtn[0]);
			_gameModeBtnList.Add(_allGameModeBtn[1]);
			CurSelGameMode = _allGameModeBtn[0];
		}
		_scrollListGameMode.Clear(true);
		for (int k = 0; k < _gameModeBtnList.Count; k++)
		{
			TUIControl component = _gameModeBtnList[k].GetComponent<TUIControl>();
			if (component == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			_scrollListGameMode.Add(component);
		}
		_scrollListGameMode.ScrollListTo(0f);
		_scrollListGameMode.GetComponent<TUIClipBinder>().SetClipRect();
		CurSelRaningBtnIndex = 0;
		Debug.Log(" ------------------------> " + COMA_Server_Rank.Instance.leftDays + " " + COMA_Server_Rank.Instance.leftHours);
		RemainingDays = COMA_Server_Rank.Instance.leftDays;
		RemainingHours = COMA_Server_Rank.Instance.leftHours;
		COMA_Server_Rank.Instance.GetRankings();
	}

	private new void Update()
	{
	}

	private void NotifySelModeChanged(int nCurSel)
	{
		InitContainer(nCurSel, CurSelRaningBtnIndex);
	}

	private void NorifySelRaningModeChanged(int nCurSel)
	{
		InitContainer(CurSelModeIndex, nCurSel);
	}

	public void InitContainer_djzhu(int nCurGameMode, int nCurRaningMode)
	{
		if (CurSelModeIndex == nCurGameMode && CurSelRaningBtnIndex == nCurRaningMode)
		{
			InitContainer(nCurGameMode, nCurRaningMode);
		}
	}

	private void InitContainer(int nCurGameMode, int nCurRaningMode)
	{
		Debug.Log("------InitContainer:nCurGameMode=" + nCurGameMode + "  nCurRaningMode=" + nCurRaningMode);
		UIRankingList_RankingData[] datas = null;
		UIRankingList_RankingData._nTestRaningId = 1;
		switch (nCurRaningMode)
		{
		case 0:
			_friendRankCaption.SetActive(false);
			_worldRankCaption.SetActive(true);
			datas = new UIRankingList_WorldRankingData[51];
			if (COMA_Server_Rank.Instance.lst_rankWorlds[nCurGameMode] != null)
			{
				for (int j = 0; j < COMA_Server_Rank.Instance.lst_rankWorlds[nCurGameMode].Count; j++)
				{
					datas[j] = COMA_Server_Rank.Instance.lst_rankWorlds[nCurGameMode][j];
				}
			}
			datas[datas.Length - 1] = COMA_Server_Rank.Instance.selfRankWorlds[nCurGameMode];
			break;
		case 1:
			_friendRankCaption.SetActive(true);
			_worldRankCaption.SetActive(false);
			datas = new UIRankingList_FriendRankingData[51];
			if (COMA_Server_Rank.Instance.lst_rankFriends[nCurGameMode] != null)
			{
				for (int i = 0; i < COMA_Server_Rank.Instance.lst_rankFriends[nCurGameMode].Count; i++)
				{
					datas[i] = COMA_Server_Rank.Instance.lst_rankFriends[nCurGameMode][i];
				}
			}
			break;
		}
		_rankingContainer.InitRaningContainer(nCurGameMode, nCurRaningMode, ref datas);
	}

	public void HandleEventButton_Back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
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

	private int GameModeBtnSelChanged(TUIControl control)
	{
		UIRankingList_GameMode component = control.GetComponent<UIRankingList_GameMode>();
		CurSelGameMode = _allGameModeBtn[component.ModeIndex];
		return component.ModeIndex;
	}

	private void AutoScroll(TUIControl control)
	{
		float y = control.transform.localPosition.y;
		TUIButtonClick tUIButtonClick = (TUIButtonClick)control;
		float num = y + tUIButtonClick.size.y / 2f;
		float f = 0f - (_scrollListGameMode.size.y / 2f - tUIButtonClick.size.y);
		float value = Mathf.Abs(num - _scrollListGameMode.size.y / 2f) / (Mathf.Abs(f) + _scrollListGameMode.size.y / 2f);
		value = Mathf.Clamp01(value);
		Debug.Log("------------------------AutoScroll to : " + value);
		_scrollListGameMode.ScrollListTo(value);
	}

	public void HandleEventButton_Run(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			GameModeBtnSelChanged(control);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Fishing(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Rocket(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			GameModeBtnSelChanged(control);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Castle(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			GameModeBtnSelChanged(control);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Treasure(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			GameModeBtnSelChanged(control);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_FriendList(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			CurSelRaningBtnIndex = 1;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_WorldList(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			CurSelRaningBtnIndex = 0;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}
}
