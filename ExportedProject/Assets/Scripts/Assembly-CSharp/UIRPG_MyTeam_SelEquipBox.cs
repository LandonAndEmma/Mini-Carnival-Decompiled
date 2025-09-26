using NGUI_COMUI;
using UnityEngine;

public class UIRPG_MyTeam_SelEquipBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private GameObject _takeOrPutBtnObj;

	[SerializeField]
	private GameObject _equipped;

	[SerializeField]
	private UILabel _takeOrPutLabel;

	[SerializeField]
	private UILabel _equipDesLabel;

	[SerializeField]
	private UISprite _colorSprite;

	public UILabel TakeOrPutLabel
	{
		get
		{
			return _takeOrPutLabel;
		}
	}

	public override void FormatBoxName(int i)
	{
		base.gameObject.name = ((i <= 9) ? ("UIRPG_MyTerm_SelectEquip_0" + i) : ("UIRPG_MyTerm_SelectEquip_" + i));
	}

	public override void BoxDataChanged()
	{
		UIRPG_MyTeam_SelEquipBoxData uIRPG_MyTeam_SelEquipBoxData = base.BoxData as UIRPG_MyTeam_SelEquipBoxData;
		_mainTex.mainTexture = uIRPG_MyTeam_SelEquipBoxData.Tex;
		if (_mainTex.mainTexture != null)
		{
			_mainTex.enabled = true;
		}
		else
		{
			_mainTex.enabled = false;
		}
		int num = RPGGlobalData.Instance.CompoundTableUnitPool._dict[uIRPG_MyTeam_SelEquipBoxData.EquipData.m_type]._apList[uIRPG_MyTeam_SelEquipBoxData.EquipData.m_level - 1];
		string apDes = RPGGlobalData.Instance.CompoundTableUnitPool._dict[uIRPG_MyTeam_SelEquipBoxData.EquipData.m_type]._apDes;
		_equipDesLabel.text = TUITool.StringFormat(Localization.instance.Get(apDes), num);
		_colorSprite.color = UIRPG_DataBufferCenter.GetCardColorByGrade(uIRPG_MyTeam_SelEquipBoxData.EquipData.m_level);
		_takeOrPutBtnObj.SetActive(true);
		if (uIRPG_MyTeam_SelEquipBoxData.IsEquip)
		{
			_equipped.SetActive(true);
		}
		else
		{
			_takeOrPutLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_anniu4"));
			_equipped.SetActive(false);
		}
		if (uIRPG_MyTeam_SelEquipBoxData.IsEquip && !uIRPG_MyTeam_SelEquipBoxData.IsEquipBySelf)
		{
			_takeOrPutBtnObj.SetActive(false);
		}
		else if (uIRPG_MyTeam_SelEquipBoxData.IsEquip && uIRPG_MyTeam_SelEquipBoxData.IsEquipBySelf)
		{
			_takeOrPutLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_anniu3"));
		}
	}
}
