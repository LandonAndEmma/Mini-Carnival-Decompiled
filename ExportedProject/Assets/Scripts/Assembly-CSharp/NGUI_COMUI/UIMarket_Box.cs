using UIGlobal;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UIMarket_Box : UI_Box
	{
		[SerializeField]
		private UILabel _labelPrice;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UITexture _iconTex;

		[SerializeField]
		private UISprite _iconSprite;

		[SerializeField]
		private UISprite _iconSprite_gem;

		[SerializeField]
		private GameObject _objCrystalIcon;

		[SerializeField]
		private GameObject _objGoldIcon;

		[SerializeField]
		private UIMarketBoxRefreshMgr _boxRefreshMgr;

		[SerializeField]
		private GameObject _objAD;

		[SerializeField]
		private GameObject _objDiscount;

		[SerializeField]
		private UILabel _labelDiscount;

		public void SetLabelName(string name)
		{
			Debug.Log("name=" + name);
			string text = name;
			if (text != null && text.Length > 6)
			{
				text = text.Substring(0, 6) + "...";
			}
			_labelName.enabled = true;
			_labelName.text = text;
			_labelPrice.enabled = false;
			_objCrystalIcon.SetActive(false);
			_objGoldIcon.SetActive(false);
		}

		public string GetLabelName()
		{
			return _labelName.text;
		}

		private void SetPrice(ECurrencyType type, int num)
		{
			switch (type)
			{
			case ECurrencyType.Crystal:
				_objCrystalIcon.SetActive(true);
				_objGoldIcon.SetActive(false);
				break;
			case ECurrencyType.Gold:
				_objCrystalIcon.SetActive(false);
				_objGoldIcon.SetActive(true);
				break;
			default:
				_objCrystalIcon.SetActive(false);
				_objGoldIcon.SetActive(false);
				break;
			}
			_labelPrice.enabled = true;
			if (num < 10000)
			{
				_labelPrice.text = num.ToString();
			}
			else if (num >= 10000 && num < 1000000)
			{
				_labelPrice.text = num / 1000 + "K";
			}
			else
			{
				_labelPrice.text = num / 1000000 + "M";
			}
			_labelName.enabled = false;
		}

		private ECurrencyType GetPrice(out int num)
		{
			num = int.Parse(_labelPrice.text);
			return (!_objCrystalIcon.activeSelf) ? ECurrencyType.Gold : ECurrencyType.Crystal;
		}

		public override void FormatBoxName(int i)
		{
			if (i > 9)
			{
				base.gameObject.name = "UIMarketBox" + i;
			}
			else
			{
				base.gameObject.name = "UIMarketBox0" + i;
			}
		}

		public override void BoxDataChanged()
		{
			UIMarket_BoxData uIMarket_BoxData = base.BoxData as UIMarket_BoxData;
			if (uIMarket_BoxData == null)
			{
				_labelPrice.text = string.Empty;
				_objCrystalIcon.SetActive(false);
				_objGoldIcon.SetActive(false);
				_boxRefreshMgr.gameObject.SetActive(false);
				_boxRefreshMgr.enabled = false;
				_objDiscount.SetActive(false);
				_iconSprite_gem.enabled = false;
				SetLoseSelected();
				return;
			}
			_objDiscount.SetActive(uIMarket_BoxData.Unit == "HBL01");
			if (_objDiscount.activeSelf && _labelDiscount != null)
			{
				_labelDiscount.text = (100 - COMA_DataConfig.Instance._sysConfig.Shop.suit_discount).ToString();
			}
			_boxRefreshMgr.gameObject.SetActive(false);
			_boxRefreshMgr.enabled = false;
			if (uIMarket_BoxData.DataType == 1 || uIMarket_BoxData.DataType == 2 || uIMarket_BoxData.DataType == 4)
			{
				SetPrice(uIMarket_BoxData.CurrencyType, uIMarket_BoxData.Price);
			}
			else if (uIMarket_BoxData.DataType == 3)
			{
				SetLabelName(uIMarket_BoxData.Name);
			}
			else if (uIMarket_BoxData.DataType == 0)
			{
				_boxRefreshMgr.gameObject.SetActive(true);
				_boxRefreshMgr.enabled = true;
				_objCrystalIcon.SetActive(false);
				_objGoldIcon.SetActive(false);
				_labelPrice.enabled = false;
				_labelName.enabled = false;
			}
			_iconTex.enabled = false;
			if (_iconTex != null)
			{
				_iconTex.mainTexture = uIMarket_BoxData.Tex;
				_iconTex.enabled = ((uIMarket_BoxData.Tex != null) ? true : false);
			}
			_iconSprite.enabled = false;
			_iconSprite_gem.enabled = false;
			if (_iconSprite != null)
			{
				if (uIMarket_BoxData.DataType == 4)
				{
					_iconSprite_gem.spriteName = uIMarket_BoxData.SpriteName;
					_iconSprite_gem.enabled = ((uIMarket_BoxData.SpriteName != string.Empty) ? true : false);
					_iconSprite_gem.MakePixelPerfect();
					_iconSprite_gem.transform.localScale *= 2f;
				}
				else
				{
					_iconSprite.spriteName = uIMarket_BoxData.SpriteName;
					_iconSprite.enabled = ((uIMarket_BoxData.SpriteName != string.Empty) ? true : false);
				}
			}
			if (_objAD != null)
			{
				_objAD.SetActive(uIMarket_BoxData.IsAdItem);
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
}
