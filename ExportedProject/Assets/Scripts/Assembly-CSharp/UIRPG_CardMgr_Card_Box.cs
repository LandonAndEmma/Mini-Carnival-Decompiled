using NGUI_COMUI;
using UnityEngine;

public class UIRPG_CardMgr_Card_Box : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UISprite[] _grade;

	[SerializeField]
	private UILabel _cardId;

	[SerializeField]
	private UILabel _cardName;

	[SerializeField]
	private GameObject _lock_first;

	[SerializeField]
	private GameObject _lock_normal;

	[SerializeField]
	private GameObject _info;

	[SerializeField]
	private UISprite _block;

	[SerializeField]
	private UISprite _frame;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIRPG_CardMgr_Card" + i;
		}
		else
		{
			base.gameObject.name = "UIRPG_CardMgr_Card0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.BoxData as UIRPG_CardMgr_Card_BoxData;
		if (uIRPG_CardMgr_Card_BoxData != null)
		{
			_lock_first.SetActive(false);
			_lock_normal.SetActive(false);
			_block.enabled = uIRPG_CardMgr_Card_BoxData.LimitSel;
			_info.SetActive(uIRPG_CardMgr_Card_BoxData.ShowInfoBtn);
			switch (base.BoxData.DataType)
			{
			case 0:
				_cardId.text = string.Empty;
				_frame.enabled = false;
				_info.SetActive(false);
				_mainSprite.enabled = false;
				break;
			case 1:
				_lock_first.SetActive(true);
				_lock_normal.SetActive(false);
				_cardId.text = string.Empty;
				_frame.enabled = false;
				_info.SetActive(false);
				_mainSprite.enabled = false;
				break;
			case 2:
				_lock_first.SetActive(false);
				_lock_normal.SetActive(true);
				_cardId.text = string.Empty;
				_frame.enabled = false;
				_info.SetActive(false);
				_mainSprite.enabled = false;
				break;
			case 3:
				_info.SetActive(uIRPG_CardMgr_Card_BoxData.ShowInfoBtn);
				_cardId.text = uIRPG_CardMgr_Card_BoxData.CardId.ToString();
				_frame.enabled = true;
				_frame.color = UIRPG_DataBufferCenter.GetCardColorByGrade((byte)uIRPG_CardMgr_Card_BoxData.CardGrade);
				_mainSprite.spriteName = "RPG_Small_" + uIRPG_CardMgr_Card_BoxData.CardId;
				_mainSprite.enabled = true;
				break;
			}
		}
	}

	public override void SetSelected()
	{
		base.SetSelected();
	}

	public override void SetLoseSelected()
	{
		base.SetLoseSelected();
	}
}
