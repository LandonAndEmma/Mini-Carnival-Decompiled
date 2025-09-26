using UnityEngine;

public class UIRPG_GemCompound_TipBox : MonoBehaviour
{
	[SerializeField]
	private UISprite[] _gemSprites;

	[SerializeField]
	private UILabel _gemCompoundInfo;

	private UIRPG_GemCompound_TipBoxData _data;

	public UIRPG_GemCompound_TipBoxData BoxData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			RefreshUI();
		}
	}

	public void RefreshUI()
	{
		int gemCompoundValue = UIRPG_DataBufferCenter.GetGemCompoundValue(_data.GemCompoundType, _data.GemLevel + 1);
		string text = TUITool.StringFormat(Localization.instance.Get(RPGGlobalData.Instance.CompoundTableUnitPool._dict[_data.GemCompoundType]._apDes), gemCompoundValue);
		_gemCompoundInfo.text = text;
		char[] array = _data.GemCompoundType.ToString().ToCharArray();
		for (int i = 0; i < _gemSprites.Length; i++)
		{
			_gemSprites[i].spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel(int.Parse(array[i].ToString()), _data.GemLevel + 1);
			_gemSprites[i].MakePixelPerfect();
		}
	}
}
