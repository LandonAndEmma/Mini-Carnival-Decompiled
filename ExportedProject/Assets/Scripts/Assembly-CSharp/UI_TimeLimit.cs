using UnityEngine;

public class UI_TimeLimit : MonoBehaviour
{
	[SerializeField]
	private TUILabel _timeLabel;

	[SerializeField]
	private int _nIndex;

	[SerializeField]
	private UI_GameRuleWeaponSelGroup _selGroup;

	private float _fStartTime;

	private double _dbSpan;

	private double _dbMaxSpan;

	private bool _bNeedUpdate;

	public void InitMaxSpan(double dbMax)
	{
		_dbMaxSpan = dbMax;
	}

	public bool SetTime(double dbPreSrvTime)
	{
		_bNeedUpdate = true;
		double serverTime = COMA_Server_Account.Instance.GetServerTime();
		_dbSpan = serverTime - dbPreSrvTime;
		if (_dbSpan < 0.0)
		{
			_dbSpan = 0.0;
		}
		if (_dbMaxSpan - _dbSpan <= 0.0)
		{
			return true;
		}
		return false;
	}

	private string ConvertDoubleTimeToFormatStr(double db)
	{
		double num = db / 3600.0;
		double num2 = (num - (double)(int)num) * 60.0;
		string text = string.Empty;
		if ((int)num < 10)
		{
			text += "0";
		}
		text += (int)num;
		text += ":";
		if ((int)num2 < 10)
		{
			text += "0";
		}
		return text + (int)num2;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!_bNeedUpdate)
		{
			return;
		}
		double num = _dbMaxSpan - _dbSpan;
		if (num <= 0.0)
		{
			num = 0.0;
			base.gameObject.SetActive(false);
			Debug.Log("SetPropSelDesExtraActive           false!!!---_dbMaxSpan=" + _dbMaxSpan + " _dbSpan=" + _dbSpan);
			if (_selGroup != null && _nIndex == _selGroup.CurSelBtn)
			{
				_selGroup.SetPropSelDesExtraActive(false);
			}
		}
		_timeLabel.Text = ConvertDoubleTimeToFormatStr(num);
		_dbSpan += Time.deltaTime;
	}
}
