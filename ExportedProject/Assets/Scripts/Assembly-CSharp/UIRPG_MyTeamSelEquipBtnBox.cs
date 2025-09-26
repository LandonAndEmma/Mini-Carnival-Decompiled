using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UIRPG_MyTeamSelEquipBtnBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	[SerializeField]
	private UIRPG_MyTeamSelEquipBtnMgr _selEquipBtnMgr;

	[SerializeField]
	private UILabel _equipDesLabel;

	[SerializeField]
	private UISprite _colorSprite;

	[SerializeField]
	private UILabel _gemLabel;

	[SerializeField]
	private BagItem.Part _part;

	[SerializeField]
	private UISprite[] _displaySprite;

	public BagItem.Part CurPart
	{
		get
		{
			return _part;
		}
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		switch (_part)
		{
		case BagItem.Part.head:
			_selEquipBtnMgr.CurPos = 0;
			break;
		case BagItem.Part.body:
			_selEquipBtnMgr.CurPos = 1;
			break;
		case BagItem.Part.leg:
			_selEquipBtnMgr.CurPos = 2;
			break;
		}
		_myTeamMgr.PopUpSelEquipObj.SetActive(true);
	}

	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UIRPG_MyTeamSelEquipBtnBoxData uIRPG_MyTeamSelEquipBtnBoxData = base.BoxData as UIRPG_MyTeamSelEquipBtnBoxData;
		if (uIRPG_MyTeamSelEquipBtnBoxData == null)
		{
			return;
		}
		if (uIRPG_MyTeamSelEquipBtnBoxData.EquipData != null)
		{
			Debug.Log("EquipData != null");
			int gemCompoundValue = UIRPG_DataBufferCenter.GetGemCompoundValue(uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_type, uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_level);
			string apDes = RPGGlobalData.Instance.CompoundTableUnitPool._dict[uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_type]._apDes;
			string rpg = "FFC500";
			_equipDesLabel.color = Color.white;
			_equipDesLabel.text = TUITool.StringFormat(Localization.instance.Get(apDes), UIRPG_DataBufferCenter.GetColorStringByRPGAndValue(rpg, gemCompoundValue));
			_mainTex.mainTexture = uIRPG_MyTeamSelEquipBtnBoxData.Tex;
			_mainTex.gameObject.SetActive(true);
			Color cardColorByGrade = UIRPG_DataBufferCenter.GetCardColorByGrade(uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_level);
			_colorSprite.gameObject.SetActive(true);
			_colorSprite.color = cardColorByGrade;
			_gemLabel.text = TUITool.StringFormat(Localization.instance.Get("avatarfactory_anniu" + uIRPG_MyTeamSelEquipBtnBoxData.EquipData.m_level) + " Gems");
			_gemLabel.gameObject.SetActive(true);
			_displaySprite[0].gameObject.SetActive(false);
			_displaySprite[1].gameObject.SetActive(false);
			_displaySprite[2].gameObject.SetActive(true);
		}
		else
		{
			Debug.Log("EquipData == null");
			_mainTex.gameObject.SetActive(false);
			_colorSprite.gameObject.SetActive(false);
			_gemLabel.gameObject.SetActive(false);
			switch (_part)
			{
			case BagItem.Part.head:
				_equipDesLabel.text = TUITool.StringFormat(UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("666666", Localization.instance.Get("myteam_desc3")));
				break;
			case BagItem.Part.body:
				_equipDesLabel.text = TUITool.StringFormat(UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("666666", Localization.instance.Get("myteam_desc4")));
				break;
			case BagItem.Part.leg:
				_equipDesLabel.text = TUITool.StringFormat(UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("666666", Localization.instance.Get("myteam_desc5")));
				break;
			}
			_displaySprite[0].gameObject.SetActive(true);
			_displaySprite[1].gameObject.SetActive(true);
			_displaySprite[2].gameObject.SetActive(false);
		}
	}
}
