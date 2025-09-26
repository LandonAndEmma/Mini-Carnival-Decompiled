using MC_UIToolKit;
using UnityEngine;

public class UIGameRulesMsgBox : UI_MsgBox
{
	[SerializeField]
	private GameObject[] _maps;

	private UI_GameRuleWeaponSelGroup _weaponGroup;

	[SerializeField]
	private UI_GameRuleWeaponSelGroup _weaponGroup_Normal;

	[SerializeField]
	private UI_GameRuleWeaponSelGroup _weaponGroup_Tank;

	[SerializeField]
	private RuleBoxWeaponInfo[] _ruleBoxWeaponInfo;

	[SerializeField]
	private TUILabel _propSelDes;

	[SerializeField]
	private TUILabel _mode;

	private string _preMoneyInfo;

	private bool _bShowLimit;

	public UI_TimeLimit[] _timelimits;

	[SerializeField]
	private GameObject _objGemDrop;

	[SerializeField]
	private TUIMeshSprite[] _gems;

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
		_bShowLimit = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetRuleBoxWeaponInfo(int index, int[] money, Texture2D[] tex2D)
	{
		_ruleBoxWeaponInfo[index].SetMoney(money);
		_ruleBoxWeaponInfo[index].SetTex(tex2D);
		_weaponGroup.SetWeaponInfo(_ruleBoxWeaponInfo[index]);
		_weaponGroup.RefreshMoney(_preMoneyInfo);
	}

	public void SetRuleBoxWeaponInfo(int index, int[] money, string[] tex2D, string[] des)
	{
		_ruleBoxWeaponInfo[index].SetMoney(money);
		_ruleBoxWeaponInfo[index].SetTex(tex2D);
		_ruleBoxWeaponInfo[index].SetPropDes(des);
		_weaponGroup.SetWeaponInfo(_ruleBoxWeaponInfo[index]);
		_weaponGroup.RefreshMoney(_preMoneyInfo);
		if (_mode != null)
		{
			_mode.TextID = UI_GlobalData.Instance._strModeID[index];
		}
		switch (index)
		{
		case 6:
			_propSelDes.TextID = "tanchukuang_desc8";
			break;
		case 8:
			_propSelDes.TextID = "tank_choose";
			break;
		default:
			_propSelDes.TextID = "moshitanchukuang_desc14";
			break;
		}
		_weaponGroup.CurSelBtn = 0;
	}

	public override void MsgBox(string strTheme, string strFurther, int nId, string strParam)
	{
		if (nId == 8)
		{
			_weaponGroup = _weaponGroup_Tank;
		}
		else
		{
			_weaponGroup = _weaponGroup_Normal;
		}
		_weaponGroup.gameObject.SetActive(true);
		base.MsgBox(strTheme, strFurther, nId, strParam);
		for (int i = 0; i < _maps.Length; i++)
		{
			bool flag = false;
			if (i == nId)
			{
				flag = true;
			}
			if (_maps[i] != null)
			{
				_maps[i].SetActive(flag);
			}
		}
		_weaponGroup.SetWeaponInfo(_ruleBoxWeaponInfo[nId]);
		_weaponGroup.RefreshMoney(strParam);
		_preMoneyInfo = strParam;
		string key = COMA_CommonOperation.Instance.SceneIDToRankID(nId);
		if (RPGGlobalData.Instance.GemDropPool._dict.ContainsKey(key))
		{
			_objGemDrop.SetActive(true);
			RPGGemDropUnit rPGGemDropUnit = RPGGlobalData.Instance.GemDropPool._dict[key];
			RPGGemDefineUnit.EGemColor type = rPGGemDropUnit._dropList[0];
			for (int j = 0; j < 3; j++)
			{
				_gems[j].texture = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel((int)type, j + 1);
			}
		}
		else
		{
			_objGemDrop.SetActive(false);
		}
		Debug.Log("-------------------Param:" + strParam);
	}

	public void HandleEventButton_Create(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButton_Play-CommandClick----GameType:" + nMsgType);
			Debug.Log("CurSel=" + _weaponGroup.CurSelBtn);
			string empty = string.Empty;
			int curSelBtn = _weaponGroup.CurSelBtn;
			if (curSelBtn != -1)
			{
				UI_GameRuleWeaponSelBtn btn = _weaponGroup.GetBtn(curSelBtn);
				Debug.Log(string.Concat("---Type:", btn._tradeType, "    Num:", btn.Money));
				int tradeType = (int)btn._tradeType;
				empty = tradeType + "," + btn.Money;
				DeductMoney(empty);
				COMA_CommonOperation.Instance.selectedWeaponIndex = curSelBtn;
				COMA_CommonOperation.Instance.selectedWeaponValidity = true;
				switch (curSelBtn)
				{
				case 0:
					if (_timelimits[0].gameObject.activeSelf)
					{
						COMA_CommonOperation.Instance.selectedWeaponValidity = false;
					}
					break;
				case 1:
					if (_timelimits[1].gameObject.activeSelf)
					{
						COMA_CommonOperation.Instance.selectedWeaponValidity = false;
					}
					break;
				}
				COMA_NetworkConnect.Instance.TryToCreateRoom(COMA_CommonOperation.Instance.seletectGameModeIndex.ToString());
				DestroyBox();
			}
			else if (nMsgType == 8)
			{
				TUI_MsgBox.Instance.MessageBox(136);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_ModeStart);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Play(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButton_Play-CommandClick----GameType:" + nMsgType);
			Debug.Log("CurSel=" + _weaponGroup.CurSelBtn);
			string empty = string.Empty;
			int curSelBtn = _weaponGroup.CurSelBtn;
			if (curSelBtn != -1)
			{
				UI_GameRuleWeaponSelBtn btn = _weaponGroup.GetBtn(curSelBtn);
				Debug.Log(string.Concat("---Type:", btn._tradeType, "    Num:", btn.Money));
				int tradeType = (int)btn._tradeType;
				empty = tradeType + "," + btn.Money;
				DeductMoney(empty);
				COMA_CommonOperation.Instance.selectedWeaponIndex = curSelBtn;
				COMA_CommonOperation.Instance.selectedWeaponValidity = true;
				switch (curSelBtn)
				{
				case 0:
					if (_timelimits[0].gameObject.activeSelf)
					{
						COMA_CommonOperation.Instance.selectedWeaponValidity = false;
					}
					break;
				case 1:
					if (_timelimits[1].gameObject.activeSelf)
					{
						COMA_CommonOperation.Instance.selectedWeaponValidity = false;
					}
					break;
				}
				COMA_NetworkConnect.Instance.TryToEnterRoom(COMA_CommonOperation.Instance.seletectGameModeIndex.ToString());
				DestroyBox();
			}
			else if (nMsgType == 8)
			{
				TUI_MsgBox.Instance.MessageBox(136);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_ModeStart);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	private void DeductMoney(string param)
	{
		Debug.Log("扣钱：" + param);
		string[] array = param.Split(',');
		if (array.Length > 1)
		{
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			switch (num)
			{
			case 1:
				COMA_CommonOperation.Instance.selectedWeaponPrice = num2;
				break;
			case 2:
				COMA_CommonOperation.Instance.selectedWeaponPrice = -num2;
				break;
			}
		}
	}

	public void HandleEventButton_Close(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_Close-CommandClick");
			COMA_CommonOperation.Instance.bSelectModeLock = false;
			UIGolbalStaticFun.CloseBlockForTUIMessageBox();
			Object.DestroyObject(base.transform.root.gameObject);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Close);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
