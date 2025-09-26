using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseGemBox : MonoBehaviour
{
	[SerializeField]
	private GameObject _chooseObj;

	[SerializeField]
	private UISprite[] _gemIcon;

	[SerializeField]
	private UILabel _desLabel;

	[SerializeField]
	private UILabel _isSelLabel;

	[SerializeField]
	private UILabel[] _gemNums;

	private UIRPG_AvatarEnhance_ChooseGemBoxData _data;

	public UIRPG_AvatarEnhance_ChooseGemBoxData BoxData
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

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshUI()
	{
		_chooseObj.SetActive(_data.IsSel ? true : false);
		_isSelLabel.gameObject.SetActive(false);
		if (_data.IsHasSel)
		{
			_chooseObj.SetActive(false);
			_isSelLabel.gameObject.SetActive(true);
		}
		Debug.Log("UIRPG_AvatarEnhance_ChooseGemBox RefreshUI()");
		NotifyRPGDataCmd rPGData = UIDataBufferCenter.Instance.RPGData;
		int[] array = new int[5];
		for (int i = 0; i < _gemIcon.Length; i++)
		{
			_gemNums[i].gameObject.SetActive(false);
			_gemIcon[i].spriteName = UIRPG_DataBufferCenter.GetSmallGemSpriteNameByTypeAndLevel(int.Parse(_data.GemComposition.ToString()[i].ToString()), _data.CurCaptionType);
			int num = int.Parse(_data.GemComposition.ToString()[i].ToString());
			array[num]++;
			int num2 = num * 100 + _data.CurCaptionType;
			uint num3 = (rPGData.m_jewel_list.ContainsKey((ushort)num2) ? rPGData.m_jewel_list[(ushort)num2] : 0u);
			if (num3 == 0 || array[num] > num3)
			{
				_gemNums[i].gameObject.SetActive(true);
			}
			_gemIcon[i].MakePixelPerfect();
		}
		int gemCompoundValue = UIRPG_DataBufferCenter.GetGemCompoundValue(_data.GemComposition, _data.CurCaptionType);
		string apDes = RPGGlobalData.Instance.CompoundTableUnitPool._dict[_data.GemComposition]._apDes;
		_desLabel.text = TUITool.StringFormat(Localization.instance.Get(apDes), gemCompoundValue);
	}
}
