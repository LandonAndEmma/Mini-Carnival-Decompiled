using MC_UIToolKit;
using NGUI_COMUI;
using UnityEngine;

public class UIBackpack_Box : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _dbgLabelType;

	[SerializeField]
	private GameObject _editObj;

	[SerializeField]
	private GameObject _equipedObj;

	[SerializeField]
	private UISprite _lockSprite;

	[SerializeField]
	private UISprite _rpgSprite;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIBackpackBox" + i;
		}
		else
		{
			base.gameObject.name = "UIBackpackBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		_rpgSprite.enabled = false;
		UIBackpack_BoxData uIBackpack_BoxData = base.BoxData as UIBackpack_BoxData;
		if (uIBackpack_BoxData == null)
		{
			_mainTex.enabled = false;
			_editObj.SetActive(false);
			_equipedObj.SetActive(false);
			_mainSprite.enabled = false;
			_dbgLabelType.enabled = false;
			_lockSprite.enabled = false;
			SetLoseSelected();
			return;
		}
		_dbgLabelType.enabled = false;
		_lockSprite.enabled = false;
		_mainTex.enabled = true;
		_mainTex.mainTexture = uIBackpack_BoxData.Tex;
		if (uIBackpack_BoxData.DataType == 1)
		{
			_mainTex.enabled = false;
			_mainSprite.enabled = false;
		}
		else if (uIBackpack_BoxData.DataType == 5)
		{
			_mainTex.enabled = false;
			if (uIBackpack_BoxData.SpriteName.StartsWith("deco_AA"))
			{
				_rpgSprite.enabled = true;
				_rpgSprite.spriteName = uIBackpack_BoxData.SpriteName;
				_mainSprite.enabled = false;
			}
			else
			{
				_mainSprite.enabled = true;
				_mainSprite.spriteName = uIBackpack_BoxData.SpriteName;
			}
		}
		else if (uIBackpack_BoxData.DataType == 0)
		{
			_mainTex.enabled = false;
			_mainSprite.enabled = false;
			_lockSprite.enabled = true;
		}
		if (uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditCanSell || uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditNoSell)
		{
			_editObj.SetActive(true);
		}
		else
		{
			_editObj.SetActive(false);
		}
		_equipedObj.SetActive(UIGolbalStaticFun.IsItemEquiped(uIBackpack_BoxData.ItemId));
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
