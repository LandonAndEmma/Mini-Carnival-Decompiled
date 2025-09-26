using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIBackpack_SellItemsExtend : UIEntity
{
	[SerializeField]
	private UIBackpack_SellListPreviewContainer _sellListPreviewContainer;

	[SerializeField]
	private GameObject _objBtnPlus;

	[SerializeField]
	private GameObject _objBtnMinus;

	[SerializeField]
	private UILabel _sellPriceLabel;

	protected override void Load()
	{
		_sellListPreviewContainer.ClearContainer();
		RegisterMessage(EUIMessageID.UIContainer_BoxSelChanged, this, BoxSelChanged);
		RegisterMessage(EUIMessageID.UIBackpack_SellItemMinusPrice, this, SellItemMinusPrice);
		RegisterMessage(EUIMessageID.UIBackpack_SellItemPlusPrice, this, SellItemPlusPrice);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIContainer_BoxSelChanged, this);
		UnregisterMessage(EUIMessageID.UIBackpack_SellItemMinusPrice, this);
		UnregisterMessage(EUIMessageID.UIBackpack_SellItemPlusPrice, this);
	}

	private bool BoxSelChanged(TUITelegram msg)
	{
		List<NGUI_COMUI.UI_Box> list = msg._pExtraInfo as List<NGUI_COMUI.UI_Box>;
		if (list != null)
		{
			int count = list.Count;
			_sellListPreviewContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
			_sellListPreviewContainer.InitBoxs(count, true);
			for (int i = 0; i < count; i++)
			{
				UIBackpack_SellListPreviewBoxData uIBackpack_SellListPreviewBoxData = new UIBackpack_SellListPreviewBoxData();
				uIBackpack_SellListPreviewBoxData.DataType = list[i].BoxData.DataType;
				uIBackpack_SellListPreviewBoxData.Tex = ((UIBackpack_BoxData)list[i].BoxData).Tex;
				_sellListPreviewContainer.SetBoxData(i, uIBackpack_SellListPreviewBoxData);
			}
			Debug.Log("Preview count=" + count);
		}
		return true;
	}

	private bool SellItemMinusPrice(TUITelegram msg)
	{
		int num = int.Parse(_sellPriceLabel.text);
		num = ((msg._pExtraInfo == null) ? (num - 500) : ((int)msg._pExtraInfo));
		num = Mathf.Clamp(num, 1000, 60000);
		_sellPriceLabel.text = num.ToString();
		if (num <= 1000)
		{
			_objBtnMinus.SetActive(false);
			_objBtnPlus.SetActive(true);
		}
		if (num < 60000)
		{
			_objBtnPlus.SetActive(true);
		}
		return true;
	}

	private bool SellItemPlusPrice(TUITelegram msg)
	{
		int num = int.Parse(_sellPriceLabel.text);
		num += 500;
		num = Mathf.Clamp(num, 1000, 60000);
		_sellPriceLabel.text = num.ToString();
		if (num >= 60000)
		{
			_objBtnMinus.SetActive(true);
			_objBtnPlus.SetActive(false);
		}
		if (num > 1000)
		{
			_objBtnMinus.SetActive(true);
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
