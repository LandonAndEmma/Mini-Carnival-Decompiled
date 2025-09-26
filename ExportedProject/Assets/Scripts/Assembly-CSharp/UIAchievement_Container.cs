using System.Collections;
using UnityEngine;

public class UIAchievement_Container : UI_Container
{
	private int[] nShowId = new int[8] { 1, 2, 9, 10, 11, 12, 13, 14 };

	private int[] nPublicShowId = new int[11]
	{
		1, 2, 12, 13, 14, 15, 16, 17, 30, 31,
		32
	};

	private int[][] nModeACId = new int[11][];

	private ArrayList _showId = new ArrayList();

	private int nSimulateBoxCount = 14;

	private void Awake()
	{
		PreInit();
		objBoxPerfab = Resources.Load("UI/Achievement/AchievementBox") as GameObject;
	}

	private void SimulateCall_Init()
	{
		nSimulateBoxCount = _showId.Count;
		int num = CreateBox(nSimulateBoxCount, 0);
		for (int i = 0; i < nSimulateBoxCount; i++)
		{
			UI_Box uI_Box = lstBoxs[i];
			for (int j = 0; j < uI_Box.Slots.Length; j++)
			{
				UI_BoxSlot uI_BoxSlot = uI_Box.Slots[j];
				int achievementState = COMA_Achievement.Instance.GetAchievementState((int)_showId[i] - 1);
				int achievementNumCur = COMA_Achievement.Instance.GetAchievementNumCur((int)_showId[i] - 1);
				int achievementNumMax = COMA_Achievement.Instance.GetAchievementNumMax((int)_showId[i] - 1);
				UIAchievement_BoxData boxData = new UIAchievement_BoxData((int)_showId[i], achievementState, achievementNumCur, achievementNumMax);
				AddBoxData(uI_BoxSlot.GetID(), boxData);
			}
		}
		RefreshContainer();
	}

	private void Start()
	{
		nModeACId[1] = new int[2] { 9, 10 };
		nModeACId[2] = new int[2] { 3, 4 };
		nModeACId[3] = new int[2] { 7, 8 };
		nModeACId[4] = new int[2] { 5, 6 };
		nModeACId[5] = new int[1] { 11 };
		nModeACId[6] = new int[7] { 18, 19, 20, 21, 22, 23, 24 };
		nModeACId[7] = new int[3] { 25, 26, 27 };
		nModeACId[8] = new int[0];
		nModeACId[9] = new int[0];
		nModeACId[10] = new int[2] { 28, 29 };
		int num = COMA_Login.Instance.orders.Length;
		int[] array = new int[num];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			array[num2] = COMA_Login.Instance.orders[i];
			num2++;
		}
		_showId = new ArrayList();
		int[] array2 = nPublicShowId;
		foreach (int num3 in array2)
		{
			_showId.Add(num3);
		}
		for (int k = 0; k < num2; k++)
		{
			if (nModeACId[array[k]] != null)
			{
				int[] array3 = nModeACId[array[k]];
				foreach (int num4 in array3)
				{
					_showId.Add(num4);
				}
			}
		}
		SimulateCall_Init();
	}

	private new void Update()
	{
	}

	public override int RefreshContainer()
	{
		base.LstBoxDatas.Sort(CompareACData);
		return base.RefreshContainer();
	}

	protected static int CompareACData(UI_BoxData l, UI_BoxData r)
	{
		if (((UIAchievement_BoxData)l).ACState == 1)
		{
			l.SortNum = 1000000;
		}
		else if (((UIAchievement_BoxData)l).ACState == 0)
		{
			l.SortNum = 500000;
		}
		else if (((UIAchievement_BoxData)l).ACState == 2)
		{
			l.SortNum = 300000;
		}
		if (((UIAchievement_BoxData)r).ACState == 1)
		{
			r.SortNum = 1000000;
		}
		else if (((UIAchievement_BoxData)r).ACState == 0)
		{
			r.SortNum = 500000;
		}
		else if (((UIAchievement_BoxData)r).ACState == 2)
		{
			r.SortNum = 300000;
		}
		l.SortNum -= ((UIAchievement_BoxData)l).sortWeight[((UIAchievement_BoxData)l).Id - 1];
		r.SortNum -= ((UIAchievement_BoxData)r).sortWeight[((UIAchievement_BoxData)r).Id - 1];
		if (l.SortNum > r.SortNum)
		{
			return -1;
		}
		if (l.SortNum < r.SortNum)
		{
			return 1;
		}
		return 0;
	}
}
