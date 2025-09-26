using UnityEngine;

public class UIInGame_LabyrinthUIMgr : MonoBehaviour
{
	[SerializeField]
	private UIInGame_LabyrinthPlayerInfo[] _inGamePlayerInfos;

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	private bool _bEnableTest;

	private static UIInGame_LabyrinthUIMgr _instance;

	public UILabyrinthPlayerInfo[] _testData;

	[SerializeField]
	private bool bTestChangeHp;

	[SerializeField]
	private int nIndex;

	[SerializeField]
	private float fNewHp;

	public static UIInGame_LabyrinthUIMgr Instance
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

	public void Init(UILabyrinthPlayerInfo[] infos)
	{
		int num = 0;
		foreach (UILabyrinthPlayerInfo uILabyrinthPlayerInfo in infos)
		{
			uILabyrinthPlayerInfo.MGR = this;
			uILabyrinthPlayerInfo.UIInfo = _inGamePlayerInfos[num];
			_inGamePlayerInfos[num]._playInfo = uILabyrinthPlayerInfo;
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
		UIInGame_LabyrinthPlayerInfo[] inGamePlayerInfos = _inGamePlayerInfos;
		foreach (UIInGame_LabyrinthPlayerInfo uIInGame_LabyrinthPlayerInfo in inGamePlayerInfos)
		{
			uIInGame_LabyrinthPlayerInfo.RefreshUI();
		}
	}

	public void DelayConcentData(UILabyrinthPlayerInfo info, Texture2D newTex2D)
	{
		UIInGame_LabyrinthPlayerInfo uIInfo = info.UIInfo;
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
