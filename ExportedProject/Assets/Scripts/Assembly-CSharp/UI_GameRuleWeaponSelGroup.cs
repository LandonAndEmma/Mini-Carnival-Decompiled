using UnityEngine;

public class UI_GameRuleWeaponSelGroup : UIMessageHandler
{
	public enum ETurnState
	{
		Front = 0,
		Turning = 1,
		Behind = 2
	}

	[SerializeField]
	private UI_GameRuleWeaponSelBtn[] _weaponBtns;

	[SerializeField]
	private GameObject[] _objBtnReturns;

	[SerializeField]
	private GameObject[] _objBtnIs;

	[SerializeField]
	private UIGameRulesMsgBox _ruleMsgBox;

	private int _curSelBtn = -1;

	[SerializeField]
	private TUILabel _propSelDesExtra;

	private ETurnState[] _BtnTurnStates;

	public int CurSelBtn
	{
		get
		{
			return _curSelBtn;
		}
		set
		{
			int curSelBtn = _curSelBtn;
			_curSelBtn = value;
			if (curSelBtn != _curSelBtn)
			{
				RefreshUI();
			}
			Debug.Log("_ruleMsgBox.IsValidityByIndex(_curSelBtn)=" + _ruleMsgBox.IsValidityByIndex(_curSelBtn) + "  _curSelBtn=" + _curSelBtn);
			if (_ruleMsgBox.IsValidityByIndex(_curSelBtn))
			{
				_propSelDesExtra.gameObject.SetActive(false);
			}
			else
			{
				_propSelDesExtra.gameObject.SetActive(true);
			}
		}
	}

	public UI_GameRuleWeaponSelBtn GetBtn(int nIndex)
	{
		return _weaponBtns[nIndex];
	}

	public void SetPropSelDesExtraActive(bool b)
	{
		_propSelDesExtra.gameObject.SetActive(b);
	}

	public void SetWeaponInfo(RuleBoxWeaponInfo info)
	{
		int num = _weaponBtns.Length;
		for (int i = 0; i < num; i++)
		{
			_weaponBtns[i].Money = info.GetMoney(i);
			_weaponBtns[i].Pic = info.GetTexStr(i);
			_weaponBtns[i].PropDesID = info.GetPropDes(i);
		}
	}

	public void ChangeTurnState(int n, ETurnState state)
	{
		_BtnTurnStates[n] = state;
		switch (state)
		{
		case ETurnState.Front:
			_objBtnReturns[n].SetActive(false);
			_objBtnIs[n].SetActive(true);
			break;
		case ETurnState.Behind:
			_objBtnReturns[n].SetActive(true);
			_objBtnIs[n].SetActive(false);
			break;
		}
	}

	private void Awake()
	{
		CurSelBtn = 0;
		_BtnTurnStates = new ETurnState[_weaponBtns.Length];
		for (int i = 0; i < _BtnTurnStates.Length; i++)
		{
			_BtnTurnStates[i] = ETurnState.Front;
		}
		GameObject[] objBtnReturns = _objBtnReturns;
		foreach (GameObject gameObject in objBtnReturns)
		{
			gameObject.SetActive(false);
		}
		GameObject[] objBtnIs = _objBtnIs;
		foreach (GameObject gameObject2 in objBtnIs)
		{
			gameObject2.SetActive(true);
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	private void RefreshUI()
	{
		int num = _weaponBtns.Length;
		for (int i = 0; i < num; i++)
		{
			bool bSel = false;
			if (i == _curSelBtn)
			{
				bSel = true;
			}
			_weaponBtns[i].WeaponSel(bSel);
		}
	}

	public void RefreshMoney(string strParam)
	{
		for (int i = 1; i < _weaponBtns.Length; i++)
		{
			_weaponBtns[i].RefreshMoney(strParam);
		}
	}

	public void HandleEventButton_weapon1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon1-CommandClick");
			CurSelBtn = 0;
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButton_weapon2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon2-CommandClick");
			if (_weaponBtns[1].IsNeedIAP())
			{
				Debug.Log("Go to IAP~");
				MsgIap(_weaponBtns[1]._tradeType);
			}
			else
			{
				CurSelBtn = 1;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButton_weapon3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon3-CommandClick");
			if (_weaponBtns[2].IsNeedIAP())
			{
				Debug.Log("Go to IAP~");
				MsgIap(_weaponBtns[2]._tradeType);
			}
			else
			{
				CurSelBtn = 2;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButton_weapon4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon3-CommandClick");
			if (_weaponBtns[3].IsNeedIAP())
			{
				Debug.Log("Go to IAP~");
				MsgIap(_weaponBtns[3]._tradeType);
			}
			else
			{
				CurSelBtn = 3;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButton_weapon5(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon3-CommandClick");
			if (_weaponBtns[4].IsNeedIAP())
			{
				Debug.Log("Go to IAP~");
				MsgIap(_weaponBtns[4]._tradeType);
			}
			else
			{
				CurSelBtn = 4;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButton_i1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_i1-CommandClick");
			if (_BtnTurnStates[0] == ETurnState.Front)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
				UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
				if (component != null)
				{
					_BtnTurnStates[0] = ETurnState.Turning;
					component.Turn();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_i2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_i2-CommandClick");
			if (_BtnTurnStates[1] == ETurnState.Front)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
				UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
				if (component != null)
				{
					_BtnTurnStates[1] = ETurnState.Turning;
					component.Turn();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_i3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_i3-CommandClick");
			if (_BtnTurnStates[2] == ETurnState.Front)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
				UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
				if (component != null)
				{
					_BtnTurnStates[2] = ETurnState.Turning;
					component.Turn();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_i4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_i4-CommandClick");
			if (_BtnTurnStates[3] == ETurnState.Front)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
				UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
				if (component != null)
				{
					_BtnTurnStates[3] = ETurnState.Turning;
					component.Turn();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_i5(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_i2-CommandClick");
			if (_BtnTurnStates[4] == ETurnState.Front)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
				UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
				if (component != null)
				{
					_BtnTurnStates[4] = ETurnState.Turning;
					component.Turn();
				}
			}
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_R1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_R1-CommandClick");
		if (_BtnTurnStates[0] == ETurnState.Behind)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
			if (component != null)
			{
				_BtnTurnStates[0] = ETurnState.Turning;
				component.Turn();
			}
		}
	}

	public void HandleEventButton_R2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_R2-CommandClick");
		if (_BtnTurnStates[1] == ETurnState.Behind)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
			if (component != null)
			{
				_BtnTurnStates[1] = ETurnState.Turning;
				component.Turn();
			}
		}
	}

	public void HandleEventButton_R3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_R3-CommandClick");
		if (_BtnTurnStates[2] == ETurnState.Behind)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
			if (component != null)
			{
				_BtnTurnStates[2] = ETurnState.Turning;
				component.Turn();
			}
		}
	}

	public void HandleEventButton_R4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_R1-CommandClick");
		if (_BtnTurnStates[3] == ETurnState.Behind)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
			if (component != null)
			{
				_BtnTurnStates[3] = ETurnState.Turning;
				component.Turn();
			}
		}
	}

	public void HandleEventButton_R5(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("HandleEventButton_R1-CommandClick");
		if (_BtnTurnStates[4] == ETurnState.Behind)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			UI_GameItemTurn component = control.gameObject.GetComponent<UI_GameItemTurn>();
			if (component != null)
			{
				_BtnTurnStates[4] = ETurnState.Turning;
				component.Turn();
			}
		}
	}

	public void NeedMoreCoin(string param)
	{
		EnterIAPUI("UI.MainMenu", null);
	}

	private void MsgIap(UI_GameRuleWeaponSelBtn.ETradeType type)
	{
		switch (type)
		{
		case UI_GameRuleWeaponSelBtn.ETradeType.Gold:
		{
			UI_MsgBox uI_MsgBox2 = TUI_MsgBox.Instance.MessageBox(108);
			uI_MsgBox2.AddProceYesHandler(NeedMoreCoin);
			break;
		}
		case UI_GameRuleWeaponSelBtn.ETradeType.Gem:
		{
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(109);
			uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
			break;
		}
		}
	}
}
