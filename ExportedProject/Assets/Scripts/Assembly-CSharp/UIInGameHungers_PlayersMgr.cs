using UnityEngine;

public class UIInGameHungers_PlayersMgr : MonoBehaviour
{
	[SerializeField]
	private UIInGame_HungersPlayerInfo[] _inGamePlayerInfos;

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	private bool _bEnableTest;

	private static UIInGameHungers_PlayersMgr _instance;

	public UIHungersPlayerInfo[] _testData;

	[SerializeField]
	private bool bTestChangeHp;

	[SerializeField]
	private int nIndex;

	[SerializeField]
	private float fNewHp;

	public static UIInGameHungers_PlayersMgr Instance
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
		}
	}

	public void Init(UIHungersPlayerInfo[] infos)
	{
		int num = 0;
		foreach (UIHungersPlayerInfo uIHungersPlayerInfo in infos)
		{
			uIHungersPlayerInfo.MGR = this;
			uIHungersPlayerInfo.UIInfo = _inGamePlayerInfos[num];
			_inGamePlayerInfos[num]._playInfo = uIHungersPlayerInfo;
			num++;
		}
		DataChanged();
	}

	public void DataChanged()
	{
		ConcentData();
	}

	private void ConcentData()
	{
		UIInGame_HungersPlayerInfo[] inGamePlayerInfos = _inGamePlayerInfos;
		foreach (UIInGame_HungersPlayerInfo uIInGame_HungersPlayerInfo in inGamePlayerInfos)
		{
			uIInGame_HungersPlayerInfo.RefreshUI();
		}
	}

	public void DelayConcentData(UIHungersPlayerInfo info, Texture2D newTex2D)
	{
		UIInGame_HungersPlayerInfo uIInfo = info.UIInfo;
		if (uIInfo != null)
		{
			uIInfo._playInfo.Tex2D = newTex2D;
			uIInfo.RefreshHeadIconUI();
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
}
