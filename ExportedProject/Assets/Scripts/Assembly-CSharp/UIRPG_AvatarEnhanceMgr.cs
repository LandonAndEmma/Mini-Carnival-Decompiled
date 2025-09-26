using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_AvatarEnhanceMgr : UIEntity
{
	[SerializeField]
	private GameObject _popUpSelGemObj;

	[SerializeField]
	private GameObject _popUpSelAvatarpObj;

	[SerializeField]
	private UITexture _selAvatarTexture;

	[SerializeField]
	private UISprite _changeSprite;

	[SerializeField]
	private GameObject _hideLabelObj;

	[SerializeField]
	private GameObject _hideSpriteObj;

	[SerializeField]
	private GameObject _hideUITextureObj;

	[SerializeField]
	private GameObject _enhanceBtnObj;

	[SerializeField]
	private List<UIRPG_AvatarEnhanceChooseGemBtn> _chooseGemBtn;

	private List<ulong> _preSelAvatar = new List<ulong>();

	private UIRPG_AvatarEnhance_SelectAvatarBoxData _selAvatarData;

	private UIRPG_AvatarEnhance_ChooseGemBoxData _selGemData;

	[SerializeField]
	private UILabel _compoundFeeLabel;

	private int _compoundFee;

	[SerializeField]
	private UILabel _enhanceResultLabel;

	[SerializeField]
	private UISprite _goldSprite;

	[SerializeField]
	private GameObject _avatarEnhanceSucessPopupObj;

	[SerializeField]
	private UITexture _avatarTexture;

	private int _curSelGemType;

	private int _curSelGemLevel = -1;

	[SerializeField]
	private UISprite[] _spriteLights;

	public List<ulong> PreSelAvatar
	{
		get
		{
			return _preSelAvatar;
		}
	}

	public UIRPG_AvatarEnhance_SelectAvatarBoxData SelAvatarData
	{
		get
		{
			return _selAvatarData;
		}
	}

	public UIRPG_AvatarEnhance_ChooseGemBoxData SelGemData
	{
		get
		{
			return _selGemData;
		}
	}

	public int CompundFee
	{
		get
		{
			return _compoundFee;
		}
	}

	public GameObject AvatarEnhanceSuccessPopupObj
	{
		get
		{
			return _avatarEnhanceSucessPopupObj;
		}
	}

	public int CurSelGemType
	{
		get
		{
			return _curSelGemType;
		}
	}

	public int CurSelGemLevel
	{
		get
		{
			return _curSelGemLevel;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPGAvatar_SelGemCompoundClick, this, HandleSelGemCompoundClick);
		RegisterMessage(EUIMessageID.UIRPGAvatar_SelAvatarClick, this, HandleSelAvatarClick);
		RegisterMessage(EUIMessageID.UIRPG_NotifyEnhanceAvatarResult, this, HandleNotifyEnhanceAvatarResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, HandleRoleDataRoleInfoChanged);
		RegisterMessage(EUIMessageID.UIRPG_AvatarCapacityChange, this, HandleAvatarCapacityChange);
		RegisterMessage(EUIMessageID.UIRPG_AvatarCapacityChangeError, this, HandleAvatarCapacityChangeError);
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		RegisterMessage(EUIMessageID.UIAvatarEnhance_GotoSquare, this, GotoSquareClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPGAvatar_SelGemCompoundClick, this);
		UnregisterMessage(EUIMessageID.UIRPGAvatar_SelAvatarClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyEnhanceAvatarResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this);
		UnregisterMessage(EUIMessageID.UIRPG_AvatarCapacityChange, this);
		UnregisterMessage(EUIMessageID.UIRPG_AvatarCapacityChangeError, this);
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
		UnregisterMessage(EUIMessageID.UIAvatarEnhance_GotoSquare, this);
	}

	public bool HandleSelGemCompoundClick(TUITelegram msg)
	{
		_popUpSelGemObj.SetActive(false);
		_goldSprite.gameObject.SetActive(true);
		_compoundFeeLabel.gameObject.SetActive(true);
		_selGemData = msg._pExtraInfo as UIRPG_AvatarEnhance_ChooseGemBoxData;
		if (_selGemData != null)
		{
			_curSelGemType = _selGemData.GemComposition;
			_curSelGemLevel = _selGemData.CurCaptionType;
			for (int i = 0; i < _chooseGemBtn.Count; i++)
			{
				_chooseGemBtn[i].DisplayGemIcon(int.Parse(_selGemData.GemComposition.ToString()[i].ToString()), _selGemData.CurCaptionType);
			}
			RPGCompoundFeeUnit compoundFeePool = RPGGlobalData.Instance.CompoundFeePool;
			for (int j = 0; j < compoundFeePool._gemToAvatarList.Count; j++)
			{
				if (_selGemData.CurCaptionType == compoundFeePool._gemToAvatarList[j]._grade)
				{
					_compoundFee = compoundFeePool._gemToAvatarList[j]._fee;
					RefreshCompundFee();
				}
			}
		}
		else
		{
			Debug.Log("msg isn't UIRPG_AvatarEnhance_ChooseGemData");
		}
		DisplayEnhanceBtn();
		return true;
	}

	private void RefreshCompundFee()
	{
		uint gold = UIDataBufferCenter.Instance.playerInfo.m_gold;
		_compoundFeeLabel.text = _compoundFee.ToString();
		if (_compoundFee > gold)
		{
			_compoundFeeLabel.color = Color.red;
		}
		else
		{
			_compoundFeeLabel.color = Color.white;
		}
	}

	public bool HandleSelAvatarClick(TUITelegram msg)
	{
		_popUpSelAvatarpObj.SetActive(false);
		_selAvatarData = msg._pExtraInfo as UIRPG_AvatarEnhance_SelectAvatarBoxData;
		if (_selAvatarData != null)
		{
			_preSelAvatar.Clear();
			_preSelAvatar.Add(_selAvatarData.ItemId);
			_selAvatarTexture.mainTexture = _selAvatarData.Tex;
			_hideUITextureObj.SetActive(true);
			_hideLabelObj.SetActive(false);
			_hideSpriteObj.SetActive(false);
			_changeSprite.gameObject.SetActive(true);
		}
		DisplayEnhanceBtn();
		return true;
	}

	public bool HandleNotifyEnhanceAvatarResult(TUITelegram msg)
	{
		EnhanceAvatarResultCmd enhanceAvatarResultCmd = msg._pExtraInfo as EnhanceAvatarResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Card_upgrade);
		if (enhanceAvatarResultCmd != null && enhanceAvatarResultCmd.m_result == 0)
		{
			_avatarEnhanceSucessPopupObj.SetActive(true);
			_avatarTexture.mainTexture = _selAvatarData.Tex;
			_avatarTexture.MakePixelPerfect();
			_avatarTexture.transform.localScale = _avatarTexture.transform.localScale * 2f;
			_selAvatarData = null;
			_selGemData = null;
			RecoverDefault();
		}
		return true;
	}

	public void DisplayEnhanceBtn()
	{
		uint gold = UIDataBufferCenter.Instance.playerInfo.m_gold;
		if (_selAvatarData != null && _selGemData != null && gold >= _compoundFee)
		{
			_enhanceResultLabel.text = UIRPG_DataBufferCenter.GetDesByGemTypeAndLevel(_selGemData.GemComposition, _selGemData.CurCaptionType);
			_enhanceResultLabel.gameObject.SetActive(true);
		}
	}

	public void RecoverDefault()
	{
		_selAvatarTexture.mainTexture = null;
		_hideLabelObj.SetActive(true);
		_hideSpriteObj.SetActive(true);
		_hideUITextureObj.SetActive(false);
		_enhanceResultLabel.gameObject.SetActive(false);
		_compoundFeeLabel.gameObject.SetActive(false);
		_goldSprite.gameObject.SetActive(false);
		_changeSprite.gameObject.SetActive(false);
		for (int i = 0; i < _chooseGemBtn.Count; i++)
		{
			_chooseGemBtn[i].RecoverGemIcon();
		}
		_curSelGemLevel = -1;
		_curSelGemType = 0;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "Lack of Money":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		case "zhuangbeibeibao_desc1":
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			BuyRpgBagCapacityCmd buyRpgBagCapacityCmd = new BuyRpgBagCapacityCmd();
			buyRpgBagCapacityCmd.m_bag_type = 1;
			buyRpgBagCapacityCmd.m_buy_num = 1;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, buyRpgBagCapacityCmd);
			COMA_HTTP_DataCollect.Instance.SendUnlockRPGBackpackByGemCount("1");
			break;
		}
		}
		return true;
	}

	private bool HandleRoleDataRoleInfoChanged(TUITelegram msg)
	{
		RefreshCompundFee();
		return true;
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		case 0:
			_spriteLights[0].enabled = true;
			break;
		case 1:
			_spriteLights[0].enabled = false;
			_spriteLights[1].enabled = true;
			break;
		case 2:
			_spriteLights[0].enabled = false;
			_spriteLights[1].enabled = false;
			break;
		}
		return true;
	}

	public bool HandleAvatarCapacityChange(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		return true;
	}

	public bool HandleAvatarCapacityChangeError(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
		uIMessage_CommonBoxData.Mark = "Lack of Money";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	private bool GotoSquare(object obj)
	{
		Debug.Log("GotoSquare");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoSquareClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}
}
