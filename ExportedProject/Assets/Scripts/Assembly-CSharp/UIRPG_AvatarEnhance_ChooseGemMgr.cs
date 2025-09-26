using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseGemMgr : MonoBehaviour
{
	public enum ECaptionType
	{
		Defective = 1,
		Normal = 2,
		Flawless = 3,
		Perfect = 4,
		Bright = 5,
		Star = 6
	}

	[SerializeField]
	private ECaptionType _curActiveCatption = ECaptionType.Defective;

	[SerializeField]
	private UIRPG_AvatarEnhance_ChooseGemCaptionBtn[] _captionBtns;

	[SerializeField]
	private GameObject _contentPrefab;

	[SerializeField]
	private Transform _contentPrefabParent;

	private UIRPG_AvatarEnhance_ChooseGemBox[] _curContents = new UIRPG_AvatarEnhance_ChooseGemBox[RPGGlobalData.Instance.CompoundTableUnitPool._dict.Count];

	private UIRPG_AvatarEnhance_ChooseGemBoxData[,] _boxDatas = new UIRPG_AvatarEnhance_ChooseGemBoxData[7, RPGGlobalData.Instance.CompoundTableUnitPool._dict.Count];

	private int[] _chooseNums = new int[7];

	[SerializeField]
	private UILabel[] _allChooseNums;

	[SerializeField]
	private UIRPG_AvatarEnhanceMgr _avatarEnhanceMgr;

	public ECaptionType CurActiveCatption
	{
		get
		{
			return _curActiveCatption;
		}
		set
		{
			_curActiveCatption = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnEnable()
	{
		InitCaptionBtns();
		GetContentData();
		GetAllChooseNums();
		InitCurCaptionContent();
	}

	public void OnDisable()
	{
		if (_contentPrefabParent != null)
		{
			for (int i = 0; i < _contentPrefabParent.childCount; i++)
			{
				Object.Destroy(_contentPrefabParent.GetChild(i).gameObject);
			}
		}
	}

	public void InitCaptionBtns()
	{
		for (int i = 0; i < _captionBtns.Length; i++)
		{
			if (CurActiveCatption == _captionBtns[i].CaptionType)
			{
				_captionBtns[i].SetActiveBtn(true);
			}
			else
			{
				_captionBtns[i].SetActiveBtn(false);
			}
		}
	}

	public void InitCurCaptionContent()
	{
		int num = 0;
		for (num = 0; num < _curContents.Length; num++)
		{
			GameObject gameObject = Object.Instantiate(_contentPrefab) as GameObject;
			if (gameObject != null)
			{
				gameObject.name = ((num <= 9) ? ("UIRPG_AvatarEnhance_GemComposition_0" + num) : ("UIRPG_AvatarEnhance_GemComposition_" + num));
				gameObject.transform.parent = _contentPrefabParent;
				gameObject.transform.localScale = Vector3.one;
				_curContents[num] = gameObject.GetComponent<UIRPG_AvatarEnhance_ChooseGemBox>();
				RefreshSingleGemCompositionBox(_curContents[num], _boxDatas[(int)_curActiveCatption, num]);
			}
			else
			{
				Debug.LogError("Cann't Instantiate Object");
			}
		}
		_contentPrefabParent.GetComponent<UIGrid>().Reposition();
	}

	public void HandleClickBtn(ECaptionType typeCaption)
	{
		_curActiveCatption = typeCaption;
		InitCaptionBtns();
		if (_curContents != null && _boxDatas != null)
		{
			for (int i = 0; i < _curContents.Length; i++)
			{
				RefreshSingleGemCompositionBox(_curContents[i], _boxDatas[(int)_curActiveCatption, i]);
			}
		}
	}

	public void GetContentData()
	{
		int[] array = new int[5];
		for (int i = 1; i < _boxDatas.GetLength(0); i++)
		{
			for (int j = 1; j < array.Length; j++)
			{
				ushort key = (ushort)(j * 100 + i);
				array[j] = (int)(UIDataBufferCenter.Instance.RPGData.m_jewel_list.ContainsKey(key) ? UIDataBufferCenter.Instance.RPGData.m_jewel_list[key] : 0);
			}
			for (int k = 0; k < _boxDatas.GetLength(1); k++)
			{
				_boxDatas[i, k] = new UIRPG_AvatarEnhance_ChooseGemBoxData(array, i);
			}
			int num = 0;
			foreach (int key2 in RPGGlobalData.Instance.CompoundTableUnitPool._dict.Keys)
			{
				_boxDatas[i, num].GemComposition = key2;
				num++;
			}
		}
	}

	public void RefreshSingleGemCompositionBox(UIRPG_AvatarEnhance_ChooseGemBox box, UIRPG_AvatarEnhance_ChooseGemBoxData data)
	{
		if (box != null && data != null)
		{
			box.BoxData = data;
		}
	}

	public void GetAllChooseNums()
	{
		for (int i = 0; i < _chooseNums.Length; i++)
		{
			_chooseNums[i] = 0;
		}
		int[] array = new int[3];
		for (int j = 1; j < _chooseNums.Length; j++)
		{
			for (int k = 0; k < _curContents.Length; k++)
			{
				int num = _boxDatas[j, k].GemComposition;
				int num2 = 0;
				for (num2 = 0; num2 < array.Length; num2++)
				{
					array[num2] = num % 10;
					num /= 10;
				}
				UIRPG_AvatarEnhance_ChooseGemBoxData uIRPG_AvatarEnhance_ChooseGemBoxData = new UIRPG_AvatarEnhance_ChooseGemBoxData(_boxDatas[j, k].GemNums, j);
				for (num2 = 0; num2 < array.Length; num2++)
				{
					if (--uIRPG_AvatarEnhance_ChooseGemBoxData.GemNums[array[num2]] < 0)
					{
						break;
					}
				}
				if (num2 == array.Length)
				{
					_chooseNums[j]++;
					_boxDatas[j, k].IsSel = true;
				}
				else
				{
					_boxDatas[j, k].IsSel = false;
				}
				if (_avatarEnhanceMgr.CurSelGemType == _boxDatas[j, k].GemComposition && _avatarEnhanceMgr.CurSelGemLevel == _boxDatas[j, k].CurCaptionType)
				{
					_boxDatas[j, k].IsHasSel = true;
				}
				else
				{
					_boxDatas[j, k].IsHasSel = false;
				}
			}
		}
		for (int l = 0; l < _allChooseNums.Length; l++)
		{
			_allChooseNums[l].text = _chooseNums[l / 2 + 1].ToString();
		}
	}
}
