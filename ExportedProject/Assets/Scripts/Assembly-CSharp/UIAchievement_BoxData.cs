using UnityEngine;

public class UIAchievement_BoxData : UI_BoxData
{
	[SerializeField]
	private int _nId;

	[SerializeField]
	private int _nState;

	[SerializeField]
	private int _nAwardNum;

	[SerializeField]
	private int _nCurProcessNum;

	[SerializeField]
	private int _nMaxProcessNum;

	private int[] awardNum = new int[35]
	{
		10, 0, 2000, 0, 0, 0, 10, 15, 2000, 2000,
		1000, 10000, 500, 300, 0, 0, 0, 2000, 0, 0,
		10, 0, 2000, 0, 0, 0, 0, 0, 0, 1000,
		5, 15, 1000, 5, 15
	};

	public int[] sortWeight = new int[35]
	{
		140, 160, 300, 305, 310, 320, 360, 340, 380, 400,
		420, 240, 100, 120, 180, 200, 220, 440, 460, 480,
		500, 520, 540, 560, 580, 600, 620, 640, 660, 260,
		270, 280, 285, 286, 287
	};

	public int CurProcessNum
	{
		get
		{
			return _nCurProcessNum;
		}
		set
		{
			_nCurProcessNum = value;
			DataChanged();
		}
	}

	public int MaxProcessNum
	{
		get
		{
			return _nMaxProcessNum;
		}
		set
		{
			_nMaxProcessNum = value;
			DataChanged();
		}
	}

	public int AwardNum
	{
		get
		{
			return _nAwardNum;
		}
		set
		{
			AwardNum = value;
			DataChanged();
		}
	}

	public int Id
	{
		get
		{
			return _nId;
		}
		set
		{
			_nId = value;
			DataChanged();
		}
	}

	public int ACState
	{
		get
		{
			return _nState;
		}
		set
		{
			bool flag = false;
			if (_nState == 1)
			{
				flag = true;
			}
			_nState = value;
			if (_nState == 1)
			{
				UI_GlobalData.Instance.CanGetACNum++;
			}
			else if (_nState == 2 && flag)
			{
				UI_GlobalData.Instance.CanGetACNum--;
			}
			DataChanged();
		}
	}

	public UIAchievement_BoxData(int id, int state, int curProNum, int maxProNum)
	{
		_nId = id;
		ACState = state;
		_nAwardNum = awardNum[id - 1];
		_nCurProcessNum = curProNum;
		_nMaxProcessNum = maxProNum;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
