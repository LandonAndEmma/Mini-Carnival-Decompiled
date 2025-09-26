using MC_UIToolKit;
using MessageID;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_GemCompoundMgr : UIEntity
{
	public enum ECaption
	{
		Red = 1,
		Yellow = 2,
		Blue = 3,
		Purple = 4
	}

	[SerializeField]
	private ECaption _curActiveCaption = ECaption.Red;

	private int _curLevel;

	[SerializeField]
	private UIRPG_GemCompound_CaptionBtn[] _captionBtns = new UIRPG_GemCompound_CaptionBtn[4];

	[SerializeField]
	private GameObject _contentPrefab;

	[SerializeField]
	private Transform _contentPrefabParent;

	[SerializeField]
	private GameObject _popupObj;

	private UIRPG_GemCompoundBox[] _curContents = new UIRPG_GemCompoundBox[5];

	private int _compoundBtnIndex;

	private UIRPG_GemCompoundBoxData[] _data = new UIRPG_GemCompoundBoxData[5];

	[SerializeField]
	private UILabel[] _gemRandCounts;

	[SerializeField]
	private UISprite[] _gemSprites;

	[SerializeField]
	private UILabel _gemCompoundTipLabel;

	[SerializeField]
	private GameObject _lightPrefabObj;

	public ECaption CurActiveCaption
	{
		get
		{
			return _curActiveCaption;
		}
		set
		{
			_curActiveCaption = value;
		}
	}

	public int CurLevel
	{
		get
		{
			return _curLevel;
		}
		set
		{
			_curLevel = value;
		}
	}

	public GameObject PopupObj
	{
		get
		{
			return _popupObj;
		}
	}

	public int CompoundBtnIndex
	{
		get
		{
			return _compoundBtnIndex;
		}
		set
		{
			_compoundBtnIndex = value;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this, GemNumChanged);
		RegisterMessage(EUIMessageID.UIRPG_NotifyCombineGemResult, this, HandleNotifyCombineGemResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyCombineGemResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	public bool GemNumChanged(TUITelegram msg)
	{
		for (int i = 0; i < _gemRandCounts.Length; i++)
		{
			int key = TransECaptionToGemID(CurActiveCaption, i + 1);
			RefreshGetGemNumData(i, key);
		}
		return true;
	}

	public bool HandleNotifyCombineGemResult(TUITelegram msg)
	{
		CombineGemResultCmd combineGemResultCmd = msg._pExtraInfo as CombineGemResultCmd;
		if (combineGemResultCmd.m_result == 0)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Gem_upgrade);
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/RPG_UI_gem/RPG_UI_gem")) as GameObject;
			gameObject.transform.parent = _curContents[_compoundBtnIndex].gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			Object.Destroy(gameObject, 1.2f);
		}
		else if (combineGemResultCmd.m_result == 1)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "LackOfMoney";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		return true;
	}

	public bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "LackOfMoney":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		InitPanel();
	}

	protected override void Tick()
	{
	}

	private void InitPanel()
	{
		_gemCompoundTipLabel.text = TUITool.StringFormat(Localization.instance.Get("gemfactory_desc1"), RPGGlobalData.Instance.RpgMiscUnit._gemCompoundConsumeCount);
		InitCaptionBtns();
		InitCurCaptionContent();
	}

	private void InitCaptionBtns()
	{
		for (int i = 0; i < _captionBtns.Length; i++)
		{
			if (_captionBtns[i].TypeCaption == CurActiveCaption)
			{
				_captionBtns[i].SetActiveBtn(true);
			}
			else
			{
				_captionBtns[i].SetActiveBtn(false);
			}
		}
	}

	private void InitCurCaptionContent()
	{
		GetGemCombinData();
		for (int i = 0; i < _curContents.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(_contentPrefab) as GameObject;
			gameObject.name = i + "_UIRPG_GemCombin";
			if (gameObject != null)
			{
				gameObject.transform.parent = _contentPrefabParent;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				_curContents[i] = gameObject.GetComponent<UIRPG_GemCompoundBox>();
				_curContents[i].CurIndex = i;
				_curContents[i].MGR = this;
				_curContents[i].GemCompoundInfo.GemLevel = i + 1;
				Debug.Log("_curContents[i].GemCompoundInfo.GemLevel = " + _curContents[i].GemCompoundInfo.GemLevel);
				if (_curContents[i] != null)
				{
					RefreshSingleGemCombinBox(_curContents[i], _data[i]);
				}
				else
				{
					Debug.LogError("Cann't Find UIRPG_GemCombinBox:" + gameObject.name);
				}
			}
			else
			{
				Debug.LogError("Cann't Instantiate Object");
			}
		}
		_contentPrefabParent.GetComponent<UIGrid>().Reposition();
	}

	public void HandleBtnClick(ECaption typeCaption)
	{
		CurActiveCaption = typeCaption;
		InitCaptionBtns();
		GetGemCombinData();
		if (_curContents != null && _data != null)
		{
			for (int i = 0; i < _curContents.Length; i++)
			{
				RefreshSingleGemCombinBox(_curContents[i], _data[i]);
			}
		}
	}

	public void GetGemCombinData()
	{
		for (int i = 0; i < _gemRandCounts.Length; i++)
		{
			int num = TransECaptionToGemID(CurActiveCaption, i + 1);
			if (i < _curContents.Length)
			{
				string gemName = RPGGlobalData.Instance.GemDefineUnitPool._dict[num + 1].GemName;
				string arg = RPGGlobalData.Instance.CompoundFeePool._gemToGemList[i]._fee.ToString();
				_data[i] = new UIRPG_GemCompoundBoxData(gemName, arg, num);
			}
			RefreshGetGemNumData(i, num);
			RefreshGemIcon();
		}
	}

	public void RefreshGetGemNumData(int i, int key)
	{
		NotifyRPGDataCmd rPGData = UIDataBufferCenter.Instance.RPGData;
		_gemRandCounts[i].text = ((!rPGData.m_jewel_list.ContainsKey((ushort)key)) ? "0" : rPGData.m_jewel_list[(ushort)key].ToString());
	}

	public void RefreshGemIcon()
	{
		for (int i = 0; i < _gemSprites.Length; i++)
		{
			_gemSprites[i].spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel((int)CurActiveCaption, i + 1);
			_gemSprites[i].MakePixelPerfect();
		}
	}

	public int TransECaptionToGemID(ECaption obj, int GemRank)
	{
		return (int)obj * 100 + GemRank;
	}

	public void RefreshSingleGemCombinBox(UIRPG_GemCompoundBox box, UIRPG_GemCompoundBoxData data)
	{
		if (box != null && data != null)
		{
			box.BOXData = data;
			box.HandleNotifyGemNumChanged(null);
		}
	}
}
