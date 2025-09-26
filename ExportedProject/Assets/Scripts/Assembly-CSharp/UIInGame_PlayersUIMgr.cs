using System.Collections.Generic;
using UnityEngine;

public class UIInGame_PlayersUIMgr : MonoBehaviour
{
	private Vector3[] _locPositions = new Vector3[4]
	{
		new Vector3(-200f, 117f, 0f),
		new Vector3(-200f, 117f, 0f),
		new Vector3(-200f, 117f, 0f),
		new Vector3(-200f, 117f, 0f)
	};

	[SerializeField]
	private UI_InGamePlayerInfo[] _inGamePlayerInfos;

	private List<UIPlayerInfo> _lstPlayerData = new List<UIPlayerInfo>();

	private bool _bEnableTest;

	[SerializeField]
	private bool _needSort = true;

	private static UIInGame_PlayersUIMgr _instance;

	public UIPlayerInfo[] _testData;

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	[SerializeField]
	private bool bTestChangeHp;

	[SerializeField]
	private int nIndex;

	[SerializeField]
	private float fNewHp;

	[SerializeField]
	private bool bTestChangeStar;

	[SerializeField]
	private int nNewStarNum;

	public static UIInGame_PlayersUIMgr Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Start()
	{
		TestInit();
	}

	private void Update()
	{
		if (_bEnableTest)
		{
			TestHpChanged();
			TestStarChanged();
		}
	}

	public UI_InGamePlayerInfo GetUIByData(UIPlayerInfo info)
	{
		int num = _lstPlayerData.IndexOf(info);
		if (num < 0 || num >= _lstPlayerData.Count)
		{
			return null;
		}
		return _inGamePlayerInfos[num];
	}

	public void DataChanged()
	{
		if (_needSort)
		{
			_lstPlayerData.Sort();
		}
		ConcentData();
	}

	public void Init(UIPlayerInfo[] infos)
	{
		foreach (UIPlayerInfo uIPlayerInfo in infos)
		{
			uIPlayerInfo.MGR = this;
		}
		_lstPlayerData.Clear();
		_lstPlayerData.AddRange(infos);
		DataChanged();
	}

	private void ConcentData()
	{
		int num = 0;
		foreach (UIPlayerInfo lstPlayerDatum in _lstPlayerData)
		{
			_inGamePlayerInfos[num]._playInfo = lstPlayerDatum;
			_inGamePlayerInfos[num].RefreshUI();
			num++;
		}
	}

	public void DelayConcentData(UIPlayerInfo info, Texture2D newTex2D)
	{
		int num = _lstPlayerData.IndexOf(info);
		if (num >= 0 && num < _lstPlayerData.Count)
		{
			_inGamePlayerInfos[num]._playInfo.Tex2D = newTex2D;
			_inGamePlayerInfos[num].RefreshHeadIconUI();
			Debug.Log("DelayConcentData");
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void TestInit()
	{
		for (int i = 0; i < _testData.Length; i++)
		{
			_testData[i].Tex2D = _modeToTex.GetTexById(i, _testData[i].DelayAssignment);
		}
		Init(_testData);
	}

	private void TestHpChanged()
	{
		if (bTestChangeHp)
		{
			_testData[nIndex].HP = fNewHp;
			bTestChangeHp = false;
		}
	}

	private void TestStarChanged()
	{
		if (bTestChangeStar)
		{
			_testData[nIndex].Num = nNewStarNum;
			bTestChangeStar = false;
		}
	}
}
