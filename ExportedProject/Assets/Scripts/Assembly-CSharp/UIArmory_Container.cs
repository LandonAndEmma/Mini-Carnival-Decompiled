using UnityEngine;

public class UIArmory_Container : UI_Container
{
	public enum EEleType
	{
		Weapons = 0,
		Helmet = 1,
		Paint = 2,
		Ele_Max = 3
	}

	private GameObject[] objBoxPrefabs = new GameObject[3];

	private EEleType _curType = EEleType.Ele_Max;

	public EEleType CurEleType
	{
		get
		{
			return _curType;
		}
	}

	private void Awake()
	{
		PreInit();
		objBoxPrefabs[0] = Resources.Load("UI/Armory/WeaponBox") as GameObject;
		objBoxPrefabs[1] = Resources.Load("UI/Armory/HelmentBox") as GameObject;
		objBoxPrefabs[2] = Resources.Load("UI/Armory/PaintBox") as GameObject;
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	private void SimulateCall_Init()
	{
		CreateBox(10, 0);
		for (int i = 0; i < 10; i++)
		{
			Random.seed = ((int)Time.timeSinceLevelLoad + i) * 10245;
			UIArmory_WeaponBoxData boxData = new UIArmory_WeaponBoxData();
			AddBoxData(i, boxData);
		}
		RefreshContainer();
	}

	public override int CreateBox(int nCount, int nType)
	{
		objBoxPerfab = objBoxPrefabs[nType];
		return base.CreateBox(nCount, nType);
	}

	public override int ExitContainer()
	{
		objBoxPerfab = null;
		return base.ExitContainer();
	}
}
