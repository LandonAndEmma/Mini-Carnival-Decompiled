using NGUI_COMUI;
using UnityEngine;

public class UIRPG_BackPack_Avatar_Box : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private GameObject _unLockBtn;

	[SerializeField]
	private GameObject _lock_normal;

	[SerializeField]
	private GameObject _isEquipObj;

	[SerializeField]
	private UISprite _colorSprite;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIRPG_BackPack_Avatar" + i;
		}
		else
		{
			base.gameObject.name = "UIRPG_BackPack_Avatar0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRPG_BackPack_Avatar_BoxData uIRPG_BackPack_Avatar_BoxData = base.BoxData as UIRPG_BackPack_Avatar_BoxData;
		if (uIRPG_BackPack_Avatar_BoxData == null)
		{
			return;
		}
		_mainTex.gameObject.SetActive(false);
		_unLockBtn.SetActive(false);
		_lock_normal.SetActive(false);
		_isEquipObj.SetActive(false);
		_colorSprite.gameObject.SetActive(false);
		switch (uIRPG_BackPack_Avatar_BoxData.DataType)
		{
		case 0:
			_mainTex.mainTexture = null;
			break;
		case 1:
			_unLockBtn.SetActive(true);
			_lock_normal.SetActive(false);
			break;
		case 2:
			_unLockBtn.SetActive(false);
			_lock_normal.SetActive(true);
			break;
		case 3:
			_mainTex.mainTexture = uIRPG_BackPack_Avatar_BoxData.Tex;
			if (_mainTex.mainTexture != null)
			{
				_mainTex.enabled = true;
			}
			else
			{
				_mainTex.enabled = false;
			}
			_mainTex.gameObject.SetActive(true);
			_isEquipObj.SetActive(uIRPG_BackPack_Avatar_BoxData.IsHasEquip);
			_colorSprite.color = UIRPG_DataBufferCenter.GetCardColorByGrade(uIRPG_BackPack_Avatar_BoxData.EquipAvatar.m_level);
			_colorSprite.gameObject.SetActive(true);
			break;
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
