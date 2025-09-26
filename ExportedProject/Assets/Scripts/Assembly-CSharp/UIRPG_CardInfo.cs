using UnityEngine;

public class UIRPG_CardInfo : MonoBehaviour
{
	private int _cardId;

	[SerializeField]
	private UITexture _mainTexture;

	[SerializeField]
	private UISprite _colorSprite;

	[SerializeField]
	private UISprite[] _gradeSprite;

	[SerializeField]
	private UILabel _nameLabel;

	[SerializeField]
	private UILabel _desLabel;

	[SerializeField]
	private UILabel[] _propertyLabel;

	public int CardId
	{
		get
		{
			return _cardId;
		}
		set
		{
			_cardId = value;
		}
	}

	public void DisplayData()
	{
		string path = "UI/RPGCardTextures/RPG_Big_" + _cardId;
		Texture texture = Resources.Load(path) as Texture;
		if (texture != null)
		{
			_mainTexture.mainTexture = texture;
			_mainTexture.enabled = true;
		}
		byte starGrade = RPGGlobalData.Instance.CareerUnitPool._dict[_cardId].StarGrade;
		for (byte b = 0; b < _gradeSprite.Length; b++)
		{
			_gradeSprite[b].enabled = ((starGrade > b) ? true : false);
		}
		_colorSprite.color = UIRPG_DataBufferCenter.GetCardColorByGrade(starGrade);
		_nameLabel.text = UIRPG_DataBufferCenter.GetCardCareerNameByCardId(_cardId);
		_desLabel.text = UIRPG_DataBufferCenter.GetCardCareerDesByCardId(_cardId);
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		int[] property = UIRPG_DataBufferCenter.GetProperty(_cardId, rpg_level, null, false, 0u);
		_propertyLabel[0].text = property[3].ToString();
		_propertyLabel[1].text = property[4].ToString();
		_propertyLabel[2].text = property[5].ToString();
		_propertyLabel[3].text = property[6] + "%";
		_propertyLabel[4].text = property[7] + "%";
		_propertyLabel[5].text = property[8].ToString();
		_propertyLabel[6].text = property[9] + "%";
		_propertyLabel[7].text = property[10].ToString();
	}
}
