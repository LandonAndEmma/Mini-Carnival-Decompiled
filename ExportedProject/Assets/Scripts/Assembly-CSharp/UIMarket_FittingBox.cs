using NGUI_COMUI;

public class UIMarket_FittingBox : NGUI_COMUI.UI_Box
{
	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIMarketFittingBox" + i;
		}
		else
		{
			base.gameObject.name = "UIMarketFittingBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIMarket_FittingBoxData uIMarket_FittingBoxData = base.BoxData as UIMarket_FittingBoxData;
		if (uIMarket_FittingBoxData == null)
		{
			SetLoseSelected();
		}
		else if (uIMarket_FittingBoxData.Tex != null)
		{
			_mainTex.enabled = true;
			_mainTex.mainTexture = uIMarket_FittingBoxData.Tex;
			if (_mainSprite != null)
			{
				_mainSprite.enabled = false;
			}
		}
		else if (_mainSprite != null)
		{
			_mainTex.enabled = false;
			_mainSprite.enabled = true;
			_mainSprite.spriteName = "deco_" + uIMarket_FittingBoxData.Unit;
		}
	}
}
