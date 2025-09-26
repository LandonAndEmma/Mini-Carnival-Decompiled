using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using UIGlobal;
using UnityEngine;

public class UIRPG_CardLibraryMgr : UIEntity
{
	[SerializeField]
	private UISprite _frame;

	[SerializeField]
	private UISprite[] _stars;

	[SerializeField]
	private UITexture _mainTex;

	[SerializeField]
	private UILabel _name;

	[SerializeField]
	private UILabel _des;

	[SerializeField]
	private UISprite[] _atk;

	[SerializeField]
	private UISprite[] _def;

	[SerializeField]
	private UISprite[] _hp;

	[SerializeField]
	private UIRPGCardManage_Container _cardMgrContainer;

	[SerializeField]
	private UILabel _maxCradNum;

	[SerializeField]
	private UILabel _ownCradNum;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICardLibary_CardClick, this, CardClick);
		RegisterMessage(EUIMessageID.UICardLibrary_CloseClick, this, CloseClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICardLibary_CardClick, this);
		UnregisterMessage(EUIMessageID.UICardLibrary_CloseClick, this);
	}

	private bool CardClick(TUITelegram msg)
	{
		Resources.UnloadUnusedAssets();
		UIRPG_CardMgr_Card_Box uIRPG_CardMgr_Card_Box = (UIRPG_CardMgr_Card_Box)msg._pExtraInfo;
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = uIRPG_CardMgr_Card_Box.BoxData as UIRPG_CardMgr_Card_BoxData;
		if (uIRPG_CardMgr_Card_BoxData != null)
		{
			_frame.color = UIRPG_DataBufferCenter.GetCardColorByGrade((byte)uIRPG_CardMgr_Card_BoxData.CardGrade);
			int cardGrade = uIRPG_CardMgr_Card_BoxData.CardGrade;
			for (int i = 0; i < _stars.Length; i++)
			{
				_stars[i].enabled = ((cardGrade > i) ? true : false);
			}
			string path = "UI/RPGCardTextures/RPG_Big_" + uIRPG_CardMgr_Card_BoxData.CardId;
			_mainTex.mainTexture = Resources.Load(path) as Texture;
			_name.text = UIRPG_DataBufferCenter.GetCardCareerNameByCardId(uIRPG_CardMgr_Card_BoxData.CardId);
			_des.text = UIRPG_DataBufferCenter.GetCardCareerDesByCardId(uIRPG_CardMgr_Card_BoxData.CardId);
			int aTKGradeByCardId = UIRPG_DataBufferCenter.GetATKGradeByCardId(uIRPG_CardMgr_Card_BoxData.CardId);
			for (int j = 0; j < _atk.Length; j++)
			{
				_atk[j].enabled = ((aTKGradeByCardId > j) ? true : false);
			}
			int dEFGradeByCardId = UIRPG_DataBufferCenter.GetDEFGradeByCardId(uIRPG_CardMgr_Card_BoxData.CardId);
			for (int k = 0; k < _def.Length; k++)
			{
				_def[k].enabled = ((dEFGradeByCardId > k) ? true : false);
			}
			int hPGradeByCardId = UIRPG_DataBufferCenter.GetHPGradeByCardId(uIRPG_CardMgr_Card_BoxData.CardId);
			for (int l = 0; l < _hp.Length; l++)
			{
				_hp[l].enabled = ((hPGradeByCardId > l) ? true : false);
			}
		}
		return true;
	}

	private bool GotoCardMgr(object obj)
	{
		Debug.Log("GotoCardMgr");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.CardManage", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool CloseClick(TUITelegram msg)
	{
		Resources.UnloadUnusedAssets();
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoCardMgr);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	public static bool IsOwnCard(int cardId)
	{
		return UIDataBufferCenter.Instance.RPGData.m_card_list.ContainsKey((uint)cardId);
	}

	private void Awake()
	{
		InitContainer();
	}

	private void Start()
	{
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIContainer_BoxOnClick, null, _cardMgrContainer.LstBoxs[0]);
	}

	protected override void Tick()
	{
	}

	private void InitContainer()
	{
		_cardMgrContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		int num = 0;
		int num2 = 0;
		foreach (int key in RPGGlobalData.Instance.CareerUnitPool._dict.Keys)
		{
			RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[key];
			if (rPGCareerUnit.CareerId < 500)
			{
				UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = new UIRPG_CardMgr_Card_BoxData(rPGCareerUnit.StarGrade, key, rPGCareerUnit.CareerName);
				uIRPG_CardMgr_Card_BoxData.ShowInfoBtn = false;
				if (IsOwnCard(uIRPG_CardMgr_Card_BoxData.CardId))
				{
					uIRPG_CardMgr_Card_BoxData.LimitSel = false;
					num2++;
				}
				else
				{
					uIRPG_CardMgr_Card_BoxData.LimitSel = true;
				}
				_cardMgrContainer.SetBoxData(_cardMgrContainer.AddBox(num), uIRPG_CardMgr_Card_BoxData);
				num++;
			}
		}
		_maxCradNum.text = num.ToString();
		_ownCradNum.text = num2.ToString();
	}
}
