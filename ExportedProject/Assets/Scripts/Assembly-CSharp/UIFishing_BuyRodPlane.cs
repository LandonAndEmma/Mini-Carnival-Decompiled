using UnityEngine;

public class UIFishing_BuyRodPlane : UICOM
{
	public UI_TimeLimit[] _timelimits;

	[SerializeField]
	private UI_GameRuleWeaponSelBtn[] _weaponBtns;

	private int _nCurSel = -2;

	public int CurSelBtn
	{
		get
		{
			return _nCurSel;
		}
		set
		{
			int nCurSel = _nCurSel;
			_nCurSel = value;
			if (nCurSel != _nCurSel)
			{
				RefreshUI();
			}
		}
	}

	private new void OnEnable()
	{
		string strParam = COMA_Pref.Instance.GetGold() + "," + COMA_Pref.Instance.GetCrystal();
		_weaponBtns[1].RefreshMoney(strParam);
		_weaponBtns[2].RefreshMoney(strParam);
	}

	private void RefreshUI()
	{
		int num = _weaponBtns.Length;
		for (int i = 0; i < num; i++)
		{
			bool bSel = false;
			if (i == CurSelBtn)
			{
				bSel = true;
			}
			_weaponBtns[i].WeaponSel(bSel);
		}
	}

	private void Start()
	{
		CurSelBtn = -1;
	}

	private new void Update()
	{
	}

	public bool IsValidityByIndex(int index)
	{
		if (index < 0 || index >= _timelimits.Length)
		{
			return true;
		}
		if (_timelimits[index].gameObject.activeSelf)
		{
			return false;
		}
		return true;
	}

	public void InitTimeLimit(double dbPresrvTime, double maxSpan, int nIndex)
	{
		_timelimits[nIndex].InitMaxSpan(maxSpan);
		bool flag = _timelimits[nIndex].SetTime(dbPresrvTime);
		_timelimits[nIndex].gameObject.SetActive(true);
	}

	public void HandleEventButton_weapon1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon1-CommandClick");
			if (IsValidityByIndex(0))
			{
				CurSelBtn = 0;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void NeedMoreCoin(string param)
	{
		EnterIAPUI("COMA_Scene_Fishing", null);
	}

	public void HandleEventButton_weapon2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType != 3)
		{
			return;
		}
		Debug.Log("Button_weapon2-CommandClick");
		if (IsValidityByIndex(1))
		{
			if (COMA_Pref.Instance.GetGold() < 2000)
			{
				Debug.Log("Go to IAP~");
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(105);
			}
			else
			{
				CurSelBtn = 1;
			}
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
	}

	public void HandleEventButton_weapon3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_weapon3-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 25)
			{
				Debug.Log("Go to IAP~");
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(106);
			}
			else
			{
				CurSelBtn = 2;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}
}
