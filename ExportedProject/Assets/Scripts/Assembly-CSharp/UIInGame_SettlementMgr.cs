using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIInGame_SettlementMgr : UIEntity
{
	public int lvUp = -1;

	public UIInGame_LevelUpUI levelUpCom;

	[SerializeField]
	private UIInGame_SettlementInfo[] _inGameSettlementInfo;

	private List<UISettlementInfo> _lstSettlementData = new List<UISettlementInfo>();

	[SerializeField]
	private UI_3DModeToTUIMgr _3dModeToTUIMgr;

	private bool _bEnableUINumericalAni;

	[SerializeField]
	private float _fUINumericalAniDurTime = 4f;

	public UISettlementInfo[] _testData;

	private bool _bEableTest;

	private float _fStartTime;

	private bool _bGetResult;

	private bool _bBackToSquare;

	[SerializeField]
	private GameObject[] _rank5678;

	[SerializeField]
	private GameObject[] _rank1234;

	protected override void Load()
	{
		Debug.Log("====================================Register NotifyGameDropResult");
		RegisterMessage(EUIMessageID.UIRPG_NotifyGameDropResult, this, NotifyGameDropResult);
	}

	protected override void UnLoad()
	{
		Debug.Log("====================================Unregister NotifyGameDropResult");
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGameDropResult, this);
	}

	public float GetUINumAniDurTime()
	{
		return _fUINumericalAniDurTime;
	}

	public bool IsEnableUINumericalAni()
	{
		return _bEnableUINumericalAni;
	}

	public void EnableUINumericalAni(bool bEnable)
	{
		_bEnableUINumericalAni = bEnable;
		if (bEnable)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Settlement_ScoreRolling, base.transform, GetUINumAniDurTime());
		}
	}

	private void TestInit()
	{
		int num = 0;
		UISettlementInfo[] testData = _testData;
		foreach (UISettlementInfo uISettlementInfo in testData)
		{
			UI_3DModeToTUI uI_3DModeToTUI = _3dModeToTUIMgr.Get3DModeToTUI(num);
			if (uI_3DModeToTUI != null)
			{
				uISettlementInfo.Tex2D = uI_3DModeToTUI.rt;
			}
			num++;
		}
		Init(_testData);
	}

	private void Awake()
	{
		UIInGame_SettlementInfo[] inGameSettlementInfo = _inGameSettlementInfo;
		foreach (UIInGame_SettlementInfo uIInGame_SettlementInfo in inGameSettlementInfo)
		{
			uIInGame_SettlementInfo.SetMgr(this);
		}
		_bEnableUINumericalAni = false;
	}

	private void Start()
	{
		if (_bEableTest)
		{
			TestInit();
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (!IsEnableUINumericalAni())
		{
			return;
		}
		bool flag = true;
		UIInGame_SettlementInfo[] inGameSettlementInfo = _inGameSettlementInfo;
		foreach (UIInGame_SettlementInfo uIInGame_SettlementInfo in inGameSettlementInfo)
		{
			if (!uIInGame_SettlementInfo.IsNumicalAniEnd())
			{
				flag = false;
			}
		}
		if (flag)
		{
			EnableUINumericalAni(false);
			if (lvUp >= 0)
			{
				Debug.Log("-----------2s JumpToMainMenu");
				_bGetResult = true;
				SceneTimerInstance.Instance.Add(2f, JumpToMainMenu);
				lvUp = -1;
				return;
			}
			_fStartTime = Time.time;
			string text = COMA_CommonOperation.Instance.SceneNameToRankID();
			if (!(text != string.Empty))
			{
				goto IL_0165;
			}
			switch (text)
			{
			case "ComAvatar001":
			case "ComAvatar002":
			case "ComAvatar003":
			case "ComAvatar004":
			case "ComAvatar005":
			case "ComAvatar008":
			case "ComAvatar009":
				break;
			default:
				goto IL_0165;
			}
			RequestGameDropCmd requestGameDropCmd = new RequestGameDropCmd();
			requestGameDropCmd.m_game_name = text;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, requestGameDropCmd);
			Debug.Log("###############################################Send MSG to SRV!############## " + text);
			goto IL_016c;
		}
		return;
		IL_016c:
		Debug.Log("-----------5s JumpToMainMenu");
		SceneTimerInstance.Instance.Add(5f, JumpToMainMenu);
		if (UI_3DModeToTUIMgr.Instance.ptlFlowerObj != null)
		{
			UI_3DModeToTUIMgr.Instance.ptlFlowerObj.particleSystem.Play(true);
		}
		return;
		IL_0165:
		_bGetResult = true;
		goto IL_016c;
	}

	private bool NotifyGameDropResult(TUITelegram msg)
	{
		Debug.Log("----------------NotifyGameDropResult");
		RequestGameDropResultCmd requestGameDropResultCmd = msg._pExtraInfo as RequestGameDropResultCmd;
		if (requestGameDropResultCmd.m_result == 1)
		{
			UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Gem, 1, requestGameDropResultCmd.m_gem_id);
			UIGolbalStaticFun.PopGetItemBox(data);
		}
		_bGetResult = true;
		JumpToMainMenu1(1);
		return true;
	}

	public bool JumpToMainMenu()
	{
		return JumpToMainMenu1(0);
	}

	public bool JumpToMainMenu1(int src)
	{
		Debug.Log("-----------JumpToMainMenu1 = " + src + " _bBackToSquare=" + _bBackToSquare + " _bGetResult=" + _bGetResult);
		if (src == 0)
		{
			if (!_bBackToSquare && _bGetResult)
			{
				_bBackToSquare = true;
				COMA_NetworkConnect.Instance.BackFromScene();
			}
		}
		else
		{
			float num = ((!(Time.time - _fStartTime >= 5f)) ? (5f - (Time.time - _fStartTime)) : 0.01f);
			Debug.Log("-----------JumpToMainMenu1 = " + num);
			SceneTimerInstance.Instance.Add(num, JumpToMainMenu);
		}
		return false;
	}

	public void Init(UISettlementInfo[] infos)
	{
		Init(infos, true);
	}

	public void Init(UISettlementInfo[] infos, bool b1234)
	{
		foreach (UISettlementInfo uISettlementInfo in infos)
		{
			uISettlementInfo.MGR = this;
		}
		_lstSettlementData.Clear();
		_lstSettlementData.AddRange(infos);
		DataChanged();
		if (b1234)
		{
			GameObject[] rank = _rank5678;
			foreach (GameObject gameObject in rank)
			{
				gameObject.SetActive(false);
			}
			GameObject[] rank2 = _rank1234;
			foreach (GameObject gameObject2 in rank2)
			{
				gameObject2.SetActive(true);
			}
		}
		else
		{
			GameObject[] rank3 = _rank5678;
			foreach (GameObject gameObject3 in rank3)
			{
				gameObject3.SetActive(true);
			}
			GameObject[] rank4 = _rank1234;
			foreach (GameObject gameObject4 in rank4)
			{
				gameObject4.SetActive(false);
			}
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 0)
		{
			COMA_Sys.Instance.nTeachingId = 1;
			COMA_Sys.Instance.bFirstGame = false;
			Debug.Log("-----------reset bfirstgame=true");
		}
	}

	public void DataChanged()
	{
		ConcentData();
	}

	private void ConcentData()
	{
		int num = 0;
		foreach (UISettlementInfo lstSettlementDatum in _lstSettlementData)
		{
			_inGameSettlementInfo[num]._settlementInfo = lstSettlementDatum;
			_inGameSettlementInfo[num].RefreshUI();
			num++;
		}
	}
}
