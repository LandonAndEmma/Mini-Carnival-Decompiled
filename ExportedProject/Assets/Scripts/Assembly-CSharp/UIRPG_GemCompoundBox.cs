using MessageID;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_GemCompoundBox : UIEntity
{
	[SerializeField]
	private GameObject _gemCompoundBtn;

	[SerializeField]
	private UILabel _gemDefineLabel;

	[SerializeField]
	private UIRPG_GemCompoundMgr _mgr;

	[SerializeField]
	private UILabel _gemCompoundPriceLabel;

	[SerializeField]
	private UIRPG_GemCompoundInfo _gemCompoundInfo;

	[SerializeField]
	private UISprite[] _gemSprites;

	[SerializeField]
	private UILabel _compoundNum;

	[SerializeField]
	private UILabel _lackGemsLabel;

	private int _curIndex;

	private UIRPG_GemCompoundBoxData _data;

	public UIRPG_GemCompoundMgr MGR
	{
		get
		{
			return _mgr;
		}
		set
		{
			_mgr = value;
		}
	}

	public UIRPG_GemCompoundInfo GemCompoundInfo
	{
		get
		{
			return _gemCompoundInfo;
		}
		set
		{
			_gemCompoundInfo = value;
		}
	}

	public int CurIndex
	{
		get
		{
			return _curIndex;
		}
		set
		{
			_curIndex = value;
		}
	}

	public UIRPG_GemCompoundBoxData BOXData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			RefreshUI();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void RefreshUI()
	{
		if (_data != null)
		{
			_gemDefineLabel.text = TUITool.StringFormat(Localization.instance.Get(_data.GemDefineLabel));
			_gemCompoundPriceLabel.text = _data.GemCompoundPriceLabel;
			_compoundNum.text = RPGGlobalData.Instance.RpgMiscUnit._gemCompoundConsumeCount.ToString();
			RefreshGemIcon();
		}
	}

	private void RefreshGemIcon()
	{
		int type = _data.GemId / 100;
		int num = _data.GemId % 100;
		_gemSprites[0].spriteName = UIRPG_DataBufferCenter.GetBigGemSpriteNameByTypeAndLevel(type, num + 1);
		_gemSprites[0].MakePixelPerfect();
		_gemSprites[1].spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel(type, num);
		_gemSprites[1].MakePixelPerfect();
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this, HandleNotifyGemNumChanged);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, HaneleRoleInfoChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this);
	}

	protected override void Tick()
	{
	}

	public bool HandleNotifyGemNumChanged(TUITelegram msg)
	{
		UIRPG_GemCompoundBoxData bOXData = BOXData;
		uint gold = UIDataBufferCenter.Instance.playerInfo.m_gold;
		Debug.Log("HandleNotifyGemNumChanged goldNum : " + gold);
		NotifyRPGDataCmd rPGData = UIDataBufferCenter.Instance.RPGData;
		uint fee = (uint)RPGGlobalData.Instance.CompoundFeePool._gemToGemList[RPGGemDefineUnit.GetGemGradeByID(bOXData.GemId) - 1]._fee;
		int num = (int)(rPGData.m_jewel_list.ContainsKey((ushort)bOXData.GemId) ? rPGData.m_jewel_list[(ushort)bOXData.GemId] : 0);
		int gemCompoundConsumeCount = RPGGlobalData.Instance.RpgMiscUnit._gemCompoundConsumeCount;
		if (fee <= gold && gemCompoundConsumeCount <= num)
		{
			_gemCompoundPriceLabel.color = Color.white;
			_lackGemsLabel.gameObject.SetActive(false);
			_gemCompoundBtn.SetActive(true);
		}
		else if (fee <= gold && gemCompoundConsumeCount > num)
		{
			_gemCompoundPriceLabel.color = Color.white;
			_lackGemsLabel.gameObject.SetActive(true);
			_gemCompoundBtn.SetActive(false);
		}
		else if (fee >= gold && gemCompoundConsumeCount <= num)
		{
			_gemCompoundPriceLabel.color = Color.red;
			_lackGemsLabel.gameObject.SetActive(false);
			_gemCompoundBtn.SetActive(true);
		}
		else
		{
			_gemCompoundPriceLabel.color = Color.red;
			_lackGemsLabel.gameObject.SetActive(true);
			_gemCompoundBtn.SetActive(false);
		}
		return true;
	}

	public bool HaneleRoleInfoChanged(TUITelegram msg)
	{
		return HandleNotifyGemNumChanged(null);
	}
}
