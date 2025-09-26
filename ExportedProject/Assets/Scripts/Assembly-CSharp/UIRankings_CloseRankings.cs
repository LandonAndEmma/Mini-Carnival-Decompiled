using MessageID;
using UnityEngine;

public class UIRankings_CloseRankings : UIEntity
{
	private UIRankings_ButtonCaptionType.ECaptionType type = UIRankings_ButtonCaptionType.ECaptionType.World_Unsel;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, this, CaptionTypeSwitched);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, this);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		if (type == UIRankings_ButtonCaptionType.ECaptionType.SearchFriend)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, null, UIBackpack_ButtonCaptionType.ECaptionType.Sell_Unsel);
			return;
		}
		Debug.Log("Load: UI.Square");
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRankings_GotoSquare, null, null);
	}

	private bool CaptionTypeSwitched(TUITelegram msg)
	{
		UIRankings_ButtonCaptionType.ECaptionType eCaptionType = (UIRankings_ButtonCaptionType.ECaptionType)(int)msg._pExtraInfo;
		if (eCaptionType != UIRankings_ButtonCaptionType.ECaptionType.SearchInput)
		{
			type = eCaptionType;
		}
		return true;
	}
}
