using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UISquare_ChatChannelMgr : UIEntity
{
	[SerializeField]
	private UILabel _label;

	private bool _bNeedCheckChatChannel;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UISquare_ChatChannelChanged, this, OnChatChannelChanged);
		_bNeedCheckChatChannel = true;
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UISquare_ChatChannelChanged, this);
		_bNeedCheckChatChannel = false;
	}

	private bool OnChatChannelChanged(TUITelegram msg)
	{
		UIDataBufferCenter.EShowChatChannel eShowChatChannel = (UIDataBufferCenter.EShowChatChannel)(int)msg._pExtraInfo;
		Debug.Log("OnChatChannelChanged=" + eShowChatChannel);
		switch (eShowChatChannel)
		{
		case UIDataBufferCenter.EShowChatChannel.All:
			_label.text = "pm";
			break;
		case UIDataBufferCenter.EShowChatChannel.PM:
			_label.text = "all";
			break;
		}
		return true;
	}

	private void OnClick()
	{
		if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.All)
		{
			UIDataBufferCenter.Instance.CurChatChannel = UIDataBufferCenter.EShowChatChannel.PM;
		}
		else if (UIDataBufferCenter.Instance.CurChatChannel == UIDataBufferCenter.EShowChatChannel.PM)
		{
			UIDataBufferCenter.Instance.CurChatChannel = UIDataBufferCenter.EShowChatChannel.All;
		}
		switch (UIDataBufferCenter.Instance.CurChatChannel)
		{
		case UIDataBufferCenter.EShowChatChannel.All:
		{
			string str2 = Localization.instance.Get("liaotianshi_desc4");
			UIGolbalStaticFun.PopupTipsBox(str2);
			break;
		}
		case UIDataBufferCenter.EShowChatChannel.PM:
		{
			string str = Localization.instance.Get("liaotianshi_desc5");
			UIGolbalStaticFun.PopupTipsBox(str);
			break;
		}
		}
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
		if (_bNeedCheckChatChannel)
		{
			if (UIDataBufferCenter.Instance.CurChatChannel != UIDataBufferCenter.Instance.PreChatChannel)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_ChatChannelChanged, this, UIDataBufferCenter.Instance.CurChatChannel);
			}
			_bNeedCheckChatChannel = false;
		}
	}
}
