using MessageID;
using UnityEngine;

public class UIRPGCardMgr_GotoSquare : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private UISprite _sprite_arrow;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 3)
		{
			_sprite_light.enabled = true;
			_sprite_arrow.enabled = true;
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = -150f;
			base.transform.localPosition = localPosition;
		}
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 3)
		{
			_sprite_light.enabled = false;
			_sprite_arrow.enabled = false;
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = 0f;
			base.transform.localPosition = localPosition;
			UIDataBufferCenter.Instance.CurNGIndex = 4;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RPG_BtnDown, null, 33);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_Ani_MyTeamBackToSquare, null, null);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		default:
			return true;
		}
	}
}
