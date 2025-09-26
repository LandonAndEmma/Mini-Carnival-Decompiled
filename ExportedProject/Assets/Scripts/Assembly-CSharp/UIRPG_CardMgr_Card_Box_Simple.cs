using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_CardMgr_Card_Box_Simple : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UISprite[] _grade;

	[SerializeField]
	private UILabel _cardId;

	[SerializeField]
	private UILabel _cardName;

	[SerializeField]
	private GameObject _lock;

	[SerializeField]
	private GameObject _info;

	[SerializeField]
	private UISprite _block;

	public override void BoxDataChanged()
	{
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.BoxData as UIRPG_CardMgr_Card_BoxData;
		if (uIRPG_CardMgr_Card_BoxData == null)
		{
			_cardId.text = string.Empty;
			_mainSprite.enabled = false;
		}
		else
		{
			_cardId.text = uIRPG_CardMgr_Card_BoxData.CardId.ToString();
			_mainSprite.spriteName = "RPG_Small_" + uIRPG_CardMgr_Card_BoxData.CardId;
			_mainSprite.enabled = true;
		}
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.BoxData as UIRPG_CardMgr_Card_BoxData;
		if (uIRPG_CardMgr_Card_BoxData != null)
		{
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRPG_CardCompoundWindowBoxClick, null, uIRPG_CardMgr_Card_BoxData.ItemId);
		}
	}
}
