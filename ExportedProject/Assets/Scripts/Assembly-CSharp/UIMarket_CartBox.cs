using NGUI_COMUI;

public class UIMarket_CartBox : NGUI_COMUI.UI_Box
{
	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIMarketCartBox" + i;
		}
		else
		{
			base.gameObject.name = "UIMarketCartBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIMarket_CartBoxData uIMarket_CartBoxData = base.BoxData as UIMarket_CartBoxData;
		if (uIMarket_CartBoxData == null)
		{
			SetLoseSelected();
		}
		else if (uIMarket_CartBoxData.Tex != null)
		{
			_mainTex.enabled = true;
			_mainTex.mainTexture = uIMarket_CartBoxData.Tex;
			if (_mainSprite != null)
			{
				_mainSprite.enabled = false;
			}
		}
		else if (_mainSprite != null)
		{
			_mainTex.enabled = false;
			_mainSprite.enabled = true;
			_mainSprite.spriteName = "deco_" + uIMarket_CartBoxData.Unit;
		}
	}
}
