using UnityEngine;

public class UIInGame_SettlementInfo : MonoBehaviour
{
	[SerializeField]
	private TUILabel _nameLabel;

	[SerializeField]
	private TUILabel _lvLabel;

	[SerializeField]
	private GameObject _objExp;

	[SerializeField]
	private float _fExpPicLen = 57f;

	[SerializeField]
	private TUILabel _starNumLabel;

	[SerializeField]
	private TUILabel _addExpNumLabel;

	[SerializeField]
	private TUILabel _goldNumLabel;

	[SerializeField]
	private TUILabel _gemNumLabel;

	[SerializeField]
	private TUIMeshSprite _playerTex;

	private UIInGame_SettlementMgr _mgr;

	public UISettlementInfo _settlementInfo = new UISettlementInfo();

	private bool _bEnableExpAni;

	private bool _bEnableOtherNumAni;

	private float _fUINumAniElapseTime;

	private float _fUIExpAniElapseTime;

	private float _fUIExpCurNeedAdd;

	private float _fUIExpCurDurTime;

	public void SetMgr(UIInGame_SettlementMgr mgr)
	{
		_mgr = mgr;
	}

	private void Start()
	{
		_objExp.transform.parent.parent.gameObject.SetActive(false);
		_objExp.SetActive(false);
		_lvLabel.transform.parent.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (_mgr != null && _mgr.IsEnableUINumericalAni())
		{
			UpdateExpAni();
			UpdateOtherNumAni();
		}
	}

	private void RefreshExp(float exp)
	{
		_lvLabel.transform.parent.gameObject.SetActive(false);
		_objExp.transform.parent.parent.gameObject.SetActive(false);
		_objExp.SetActive(false);
	}

	public void RefreshUI()
	{
		_nameLabel.Text = _settlementInfo.Name;
		_lvLabel.Text = _settlementInfo.LV.ToString();
		_lvLabel.transform.parent.gameObject.SetActive(false);
		float expRatio = _settlementInfo.ExpRatio;
		expRatio = Mathf.Clamp01(expRatio);
		RefreshExp(expRatio);
		_starNumLabel.Text = "0";
		_addExpNumLabel.Text = "0";
		_goldNumLabel.Text = "0";
		_gemNumLabel.Text = "0";
		if (_settlementInfo.Tex2D != null)
		{
			_playerTex.UseCustomize = true;
			_playerTex.CustomizeTexture = _settlementInfo.Tex2D;
			_playerTex.CustomizeRect = new Rect(0f, 0f, _settlementInfo.Tex2D.width, _settlementInfo.Tex2D.height);
		}
	}

	public void AnimationStart()
	{
		_fUINumAniElapseTime = 0f;
		_fUIExpAniElapseTime = 0f;
		_bEnableExpAni = true;
		_bEnableOtherNumAni = true;
		_fUIExpCurNeedAdd = _settlementInfo.AddExpRatio;
		_fUIExpCurDurTime = _mgr.GetUINumAniDurTime();
	}

	public void AnimationEnd()
	{
		if (_mgr != null)
		{
			_mgr.EnableUINumericalAni(true);
		}
	}

	public bool IsNumicalAniEnd()
	{
		return !_bEnableExpAni && !_bEnableOtherNumAni;
	}

	private void UpdateExpAni()
	{
		if (_bEnableExpAni)
		{
			_bEnableExpAni = false;
			_fUIExpAniElapseTime += Time.deltaTime;
			if (_fUIExpAniElapseTime >= _fUIExpCurDurTime)
			{
				_fUIExpAniElapseTime = _fUIExpCurDurTime;
				_bEnableExpAni = false;
			}
			float num = 0f;
			num = ((_fUIExpAniElapseTime != _fUIExpCurDurTime) ? (_fUIExpAniElapseTime * _fUIExpCurNeedAdd / _fUIExpCurDurTime) : _fUIExpCurNeedAdd);
			float expRatio = _settlementInfo.ExpRatio;
			float num2 = num + expRatio;
			if (num2 >= 1f)
			{
				_settlementInfo.LV++;
				float num3 = num2 - 1f;
				_settlementInfo.ExpRatio = num3;
				_fUIExpCurNeedAdd -= num;
				_fUIExpCurDurTime -= _fUIExpAniElapseTime;
				_fUIExpAniElapseTime = 0f;
				num2 = num3;
			}
			RefreshExp(num2);
		}
	}

	private void UpdateOtherNumAni()
	{
		if (!_bEnableOtherNumAni)
		{
			return;
		}
		_fUINumAniElapseTime += Time.deltaTime;
		if (_fUINumAniElapseTime >= _mgr.GetUINumAniDurTime())
		{
			_fUINumAniElapseTime = _mgr.GetUINumAniDurTime();
			if (COMA_CommonOperation.Instance.IsMode_Blood(Application.loadedLevelName))
			{
				_starNumLabel.Text = _settlementInfo.StarNum.ToString();
			}
			else
			{
				string text = ((_settlementInfo.StarNum >= 0) ? "+" : string.Empty);
				_starNumLabel.Text = text + _settlementInfo.StarNum;
			}
			_addExpNumLabel.Text = _settlementInfo.AddExpNum.ToString();
			_addExpNumLabel.Text = "0";
			_goldNumLabel.Text = _settlementInfo.GoldNum.ToString();
			_gemNumLabel.Text = _settlementInfo.GemNum.ToString();
			_bEnableOtherNumAni = false;
		}
		else
		{
			int num = (int)(_fUINumAniElapseTime * (float)_settlementInfo.StarNum / _mgr.GetUINumAniDurTime());
			_starNumLabel.Text = num.ToString();
			int num2 = (int)(_fUINumAniElapseTime * (float)_settlementInfo.AddExpNum / _mgr.GetUINumAniDurTime());
			_addExpNumLabel.Text = num2.ToString();
			_addExpNumLabel.Text = "0";
			int num3 = (int)(_fUINumAniElapseTime * (float)_settlementInfo.GoldNum / _mgr.GetUINumAniDurTime());
			_goldNumLabel.Text = num3.ToString();
			int num4 = (int)(_fUINumAniElapseTime * (float)_settlementInfo.GemNum / _mgr.GetUINumAniDurTime());
			_gemNumLabel.Text = num4.ToString();
		}
	}
}
