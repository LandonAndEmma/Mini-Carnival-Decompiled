using System.Collections.Generic;
using MessageID;
using Protocol;
using Protocol.Role.S2C;
using UnityEngine;

public class UIRole : UIDataBufferDependency
{
	[SerializeField]
	private UILabel _labelLevelNum;

	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private UILabel _labelHeartNum;

	[SerializeField]
	private UILabel _labelGoldNum;

	[SerializeField]
	private UILabel _labelCtystalNum;

	[SerializeField]
	private UILabel _labelCouponNum;

	private List<CNumAni> _lstGoldNumAni = new List<CNumAni>();

	private List<CNumAni> _lstCrystalNumAni = new List<CNumAni>();

	public UILabel _labelLV;

	public GameObject objExpIndicate;

	private float fOriLocalX;

	[SerializeField]
	private float maxLength = 276f;

	protected override void Load()
	{
		_lstGoldNumAni.Clear();
		_lstCrystalNumAni.Clear();
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, RoleDataChanged);
		RegisterMessage(EUIMessageID.UI_RoleExpUpdate, this, RoleExpUpdate);
		RegisterMessage(EUIMessageID.UIRPG_NotifyExpChanged, this, RPGRoleExpUpdate);
		RegisterMessage(EUIMessageID.UIRPG_CouponNumChange, this, CouponNumChange);
		base.Load();
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this);
		UnregisterMessage(EUIMessageID.UI_RoleExpUpdate, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyExpChanged, this);
		UnregisterMessage(EUIMessageID.UIRPG_CouponNumChange, this);
		base.UnLoad();
	}

	private void DataToUI(RoleInfo info, bool bNeedAni)
	{
		if (info == null)
		{
			return;
		}
		RPGRoleExpUpdate(null);
		if (null != _labelLevelNum)
		{
			_labelLevelNum.text = UIDataBufferCenter.Instance.RPGData.m_rpg_level.ToString();
		}
		if (null != _labelName)
		{
			_labelName.text = info.m_name.ToString();
		}
		if (null != _labelHeartNum)
		{
			_labelHeartNum.text = info.m_heart.ToString();
		}
		if (null != _labelCouponNum)
		{
			_labelCouponNum.text = UIDataBufferCenter.Instance.RPGData.m_coupon.ToString();
			if (Application.loadedLevelName == "UI.RPG.CardManage" && COMA_Pref.Instance.NG2_1_FirstEnterSquare)
			{
				if (UIDataBufferCenter.Instance.CurNGIndex == 1)
				{
					_labelCouponNum.text = (UIDataBufferCenter.Instance.RPGData.m_coupon + 10).ToString();
				}
				else if (UIDataBufferCenter.Instance.CurNGIndex == 2)
				{
					_labelCouponNum.text = (UIDataBufferCenter.Instance.RPGData.m_coupon + 5).ToString();
				}
			}
		}
		if (null != _labelGoldNum)
		{
			if (bNeedAni)
			{
				_lstGoldNumAni.Add(new CNumAni(uint.Parse(_labelGoldNum.text), info.m_gold, Time.time, _labelGoldNum));
			}
			else
			{
				_labelGoldNum.text = info.m_gold.ToString();
			}
		}
		if (null != _labelCtystalNum)
		{
			if (bNeedAni)
			{
				Debug.Log("Add Crystal Ani: Aim=" + info.m_crystal + " Init=" + uint.Parse(_labelCtystalNum.text));
				_lstCrystalNumAni.Add(new CNumAni(uint.Parse(_labelCtystalNum.text), info.m_crystal, Time.time, _labelCtystalNum));
			}
			else
			{
				_labelCtystalNum.text = info.m_crystal.ToString();
			}
		}
	}

	private bool CouponNumChange(TUITelegram msg)
	{
		Debug.Log("CouponNumChange");
		if (null != _labelCouponNum)
		{
			_labelCouponNum.text = UIDataBufferCenter.Instance.RPGData.m_coupon.ToString();
		}
		return true;
	}

	private bool RoleDataChanged(TUITelegram msg)
	{
		Debug.Log("Role.Hm.RoleDataChanged");
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)msg._pExtraInfo;
		if (notifyRoleDataCmd != null)
		{
			DataToUI(notifyRoleDataCmd.m_info, true);
		}
		return true;
	}

	private bool RoleExpUpdate(TUITelegram msg)
	{
		RPGRoleExpUpdate(null);
		return true;
	}

	private bool RPGRoleExpUpdate(TUITelegram msg)
	{
		if (objExpIndicate == null)
		{
			return false;
		}
		if (_labelLV != null)
		{
			_labelLV.text = UIDataBufferCenter.Instance.RPGData.m_rpg_level.ToString();
		}
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		float num = 0f;
		for (int i = 0; i < RPGGlobalData.Instance.LstRPGMaxExp.Count; i++)
		{
			RPGMaxExp rPGMaxExp = RPGGlobalData.Instance.LstRPGMaxExp[i];
			if (rpg_level < rPGMaxExp._lv_max)
			{
				num = rPGMaxExp._exp;
				break;
			}
		}
		float num2 = (float)UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp / num;
		Debug.Log("UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp=" + UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp);
		Debug.Log("fMax=" + num);
		Debug.Log("$$$$$$$$$$$$$$$EXP RATE:" + num2);
		RefreshExp(num2);
		return true;
	}

	public void RefreshExp(float f)
	{
		f = Mathf.Clamp01(f);
		Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^f=" + f);
		Vector3 localScale = objExpIndicate.transform.localScale;
		localScale.x = f * maxLength;
		objExpIndicate.transform.localScale = localScale;
		float num = (1f - f) / 2f;
		float num2 = (0f - num) * maxLength;
		Vector3 localPosition = objExpIndicate.transform.localPosition;
		localPosition.x = fOriLocalX + num2;
		objExpIndicate.transform.localPosition = localPosition;
	}

	private new void Awake()
	{
		if (objExpIndicate != null)
		{
			fOriLocalX = objExpIndicate.transform.localPosition.x;
		}
	}

	private void Start()
	{
	}

	protected override void Tick()
	{
		if (_NeedGetNewData)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			if (notifyRoleDataCmd != null)
			{
				DataToUI(notifyRoleDataCmd.m_info, false);
			}
			_NeedGetNewData = false;
		}
		if (_lstGoldNumAni.Count > 0 && _lstGoldNumAni[0].UpdateAni())
		{
			_lstGoldNumAni.RemoveAt(0);
		}
		if (_lstCrystalNumAni.Count > 0 && _lstCrystalNumAni[0].UpdateAni())
		{
			_lstCrystalNumAni.RemoveAt(0);
		}
	}
}
