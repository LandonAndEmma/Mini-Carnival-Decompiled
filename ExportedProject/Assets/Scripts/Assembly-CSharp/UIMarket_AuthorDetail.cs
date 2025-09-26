using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UIMarket_AuthorDetail : UIEntity
{
	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private UILabel _labelFansNum;

	[SerializeField]
	private UITexture _texAuthorIcon;

	[SerializeField]
	private GameObject _content;

	[SerializeField]
	private UIMarket_ButtonFollow _btnFollowCmp;

	[SerializeField]
	private bool _inAuthorShop;

	[SerializeField]
	private GameObject _aniLayer;

	[SerializeField]
	private uint _authorId;

	public uint CurAuthorId
	{
		get
		{
			return _authorId;
		}
	}

	protected override void Load()
	{
		_authorId = 0u;
		RegisterMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, ShopItemAuthorRefresh);
		RegisterMessage(EUIMessageID.UIMarket_NotifyEnterAuthorShop, this, NotifyEnterAuthorShop);
		if (_aniLayer != null)
		{
			_aniLayer.SetActive(false);
		}
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this);
		UnregisterMessage(EUIMessageID.UIMarket_NotifyEnterAuthorShop, this);
		if (_aniLayer != null)
		{
			_aniLayer.SetActive(false);
		}
	}

	private bool ShopItemAuthorRefresh(TUITelegram msg)
	{
		if (_inAuthorShop)
		{
			return true;
		}
		if (msg._pExtraInfo2 != null)
		{
			NGUI_COMUI.UIMarket.ETabLeftButtonsType eTabLeftButtonsType = (NGUI_COMUI.UIMarket.ETabLeftButtonsType)(int)msg._pExtraInfo2;
			if (eTabLeftButtonsType != NGUI_COMUI.UIMarket.ETabLeftButtonsType.Avatar_Best && eTabLeftButtonsType != NGUI_COMUI.UIMarket.ETabLeftButtonsType.Avatar)
			{
				return true;
			}
		}
		UIMarket_BoxData uIMarket_BoxData = msg._pExtraInfo as UIMarket_BoxData;
		if (uIMarket_BoxData == null)
		{
			if (_content != null)
			{
				_content.SetActive(false);
			}
			return true;
		}
		if (_content != null)
		{
			_content.SetActive(true);
		}
		uint authorId = uIMarket_BoxData.AuthorId;
		RefreshPlayerExtInfoByID(authorId);
		return true;
	}

	private bool NotifyEnterAuthorShop(TUITelegram msg)
	{
		if (_inAuthorShop)
		{
			RefreshPlayerExtInfoByID(_authorId = (uint)msg._pExtraInfo);
			return true;
		}
		return true;
	}

	private void RefreshPlayerExtInfoByID(uint authorId)
	{
		_btnFollowCmp.BtnState = (UIGolbalStaticFun.IsPlayerInFollowLst(authorId) ? UIMarket_ButtonFollow.State.Followed : UIMarket_ButtonFollow.State.UnFollow);
		UIDataBufferCenter.Instance.FetchPlayerProfile(authorId, delegate(WatchRoleInfo watchRoleInfo)
		{
			if (watchRoleInfo == null)
			{
				_labelName.text = string.Empty;
			}
			else
			{
				_labelName.text = watchRoleInfo.m_name;
			}
		});
		UIDataBufferCenter.Instance.FetchPlayerExtInfoByID(authorId, delegate(ExtInfo extInfo)
		{
			if (extInfo == null)
			{
				_labelFansNum.text = string.Empty;
			}
			else if (extInfo.m_fans_num < 100000)
			{
				_labelFansNum.text = extInfo.m_fans_num.ToString();
			}
			else
			{
				_labelFansNum.text = extInfo.m_fans_num / 1000 + "K";
			}
		});
		UIDataBufferCenter.Instance.FetchFacebookIconByTID(authorId, delegate(Texture2D tex2D)
		{
			_texAuthorIcon.mainTexture = tex2D;
		});
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
