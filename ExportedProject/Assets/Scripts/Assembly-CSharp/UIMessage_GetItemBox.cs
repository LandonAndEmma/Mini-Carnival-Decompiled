using UnityEngine;

public class UIMessage_GetItemBox : UIMessage_Box
{
	[SerializeField]
	private GameObject _goldOrCrystal;

	[SerializeField]
	private GameObject _items;

	[SerializeField]
	private UISprite _goldSprite;

	[SerializeField]
	private UISprite _crystalSprite;

	[SerializeField]
	private UISprite _gemSprite;

	[SerializeField]
	private UISprite _rpgDecoSprite;

	[SerializeField]
	private UILabel _numLabel;

	[SerializeField]
	private UISprite _decoSprite;

	[SerializeField]
	private UITexture _otherTex;

	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UIGetItemBoxData uIGetItemBoxData = base.BoxData as UIGetItemBoxData;
		if (uIGetItemBoxData == null)
		{
			return;
		}
		_gemSprite.enabled = false;
		_rpgDecoSprite.enabled = false;
		switch (uIGetItemBoxData.DataType)
		{
		case 0:
			_goldOrCrystal.SetActive(true);
			_items.SetActive(false);
			_goldSprite.enabled = true;
			_crystalSprite.enabled = false;
			_numLabel.text = uIGetItemBoxData.GetNum.ToString();
			break;
		case 4:
		{
			_goldOrCrystal.SetActive(false);
			_items.SetActive(false);
			_goldSprite.enabled = false;
			_crystalSprite.enabled = false;
			_numLabel.text = string.Empty;
			_gemSprite.enabled = true;
			RPGGemDefineUnit.EGemColor gemColorByID = RPGGemDefineUnit.GetGemColorByID(uIGetItemBoxData.GemId);
			int gemGradeByID = RPGGemDefineUnit.GetGemGradeByID(uIGetItemBoxData.GemId);
			_gemSprite.spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel((int)gemColorByID, gemGradeByID);
			if (_gemSprite.spriteName != string.Empty)
			{
				_gemSprite.transform.localScale = new Vector3(_gemSprite.atlas.GetSprite(_gemSprite.spriteName).outer.width * 2f, _gemSprite.atlas.GetSprite(_gemSprite.spriteName).outer.height * 2f, 1f);
			}
			break;
		}
		case 1:
			_goldOrCrystal.SetActive(true);
			_items.SetActive(false);
			_goldSprite.enabled = false;
			_crystalSprite.enabled = true;
			_numLabel.text = uIGetItemBoxData.GetNum.ToString();
			break;
		case 2:
			_goldOrCrystal.SetActive(false);
			_items.SetActive(true);
			_decoSprite.enabled = true;
			_otherTex.enabled = false;
			_decoSprite.spriteName = uIGetItemBoxData.SpriteName;
			break;
		case 5:
			_goldOrCrystal.SetActive(false);
			_items.SetActive(true);
			_decoSprite.enabled = false;
			_otherTex.enabled = false;
			_rpgDecoSprite.enabled = true;
			_rpgDecoSprite.spriteName = uIGetItemBoxData.SpriteName;
			break;
		case 3:
			_goldOrCrystal.SetActive(false);
			_items.SetActive(true);
			_decoSprite.enabled = false;
			_otherTex.enabled = true;
			_otherTex.mainTexture = uIGetItemBoxData.Tex;
			break;
		}
	}
}
