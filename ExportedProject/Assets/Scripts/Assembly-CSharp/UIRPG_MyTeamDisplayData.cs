public class UIRPG_MyTeamDisplayData
{
	public enum UIRPG_MyTeamRoleAttr
	{
		STR = 0,
		DEX = 1,
		INT = 2,
		HP = 3,
		ATK = 4,
		DEF = 5,
		CRT = 6,
		AVD = 7,
		HIT = 8,
		CDMG = 9,
		AP = 10
	}

	private float[] _baseAttrs = new float[11];

	private int _cardId;

	public float[] BaseAttrs
	{
		get
		{
			return _baseAttrs;
		}
	}

	public UIRPG_MyTeamDisplayData(int cardId)
	{
		_cardId = cardId;
		RPGCareerUnit rPGCareerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[_cardId];
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		_baseAttrs[0] = (float)(4 + rpg_level) * rPGCareerUnit.AttrValue[0];
		_baseAttrs[1] = (float)(4 + rpg_level) * rPGCareerUnit.AttrValue[1];
		_baseAttrs[2] = (float)(4 + rpg_level) * rPGCareerUnit.AttrValue[2];
		_baseAttrs[3] = RPGEntity.GetHPByRoleAttr(_baseAttrs[0], _baseAttrs[1], _baseAttrs[2]);
		float num = 0f;
		for (int i = 0; i <= 2; i++)
		{
			if (i != (int)rPGCareerUnit.MainAttr)
			{
				num += _baseAttrs[i];
			}
		}
		_baseAttrs[4] = _baseAttrs[(int)rPGCareerUnit.MainAttr] * 1.5f + num;
		_baseAttrs[5] = 35f - 40000f / (1150f + _baseAttrs[1]) + (35f - 40000f / (1150f + _baseAttrs[2]));
		_baseAttrs[6] = 20f - 40000f / (2000f + _baseAttrs[1]);
		_baseAttrs[7] = 5f;
		_baseAttrs[8] = 100f;
		_baseAttrs[9] = 100f - 40000f / (_baseAttrs[2] + 400f);
		float num2 = rPGCareerUnit.AttrValue[0] + rPGCareerUnit.AttrValue[1] + rPGCareerUnit.AttrValue[2];
		_baseAttrs[10] = num2 * (float)rpg_level + _baseAttrs[4] + _baseAttrs[3] / _baseAttrs[5];
	}
}
