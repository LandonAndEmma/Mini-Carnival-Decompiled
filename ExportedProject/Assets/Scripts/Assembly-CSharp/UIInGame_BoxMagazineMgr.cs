using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGame_BoxMagazineMgr : MonoBehaviour
{
	public class UIBoxMagazine
	{
		public int maxBulletCount;

		public int curBulletNum;

		public UIBoxMagazine(int maxCount, int num)
		{
			maxBulletCount = maxCount;
			curBulletNum = num;
		}
	}

	private int curBoxIndex;

	private List<UIBoxMagazine> lstCartridges = new List<UIBoxMagazine>();

	[SerializeField]
	private GameObject[] objBullet;

	private bool _bInfinityMode;

	[SerializeField]
	private TUILabel bulletNum;

	[SerializeField]
	private GameObject objInfinityNum;

	[SerializeField]
	private TUILabel boxNum;

	private int _bulletAmount;

	private bool _bEnableTest;

	public bool bTest;

	public bool bTestChangeBox;

	public int nCurBulletNum;

	public int BulletNum
	{
		get
		{
			return int.Parse(bulletNum.Text);
		}
		set
		{
			bulletNum.Text = value.ToString();
		}
	}

	public int BoxNum
	{
		get
		{
			return int.Parse(boxNum.Text);
		}
		set
		{
			if (!(boxNum != null))
			{
				return;
			}
			int num = value;
			if (_bInfinityMode)
			{
				boxNum.Text = (-1).ToString();
				if (boxNum.gameObject.activeSelf)
				{
					boxNum.gameObject.SetActive(false);
				}
				if (!objInfinityNum.activeSelf)
				{
					objInfinityNum.SetActive(true);
				}
			}
			else
			{
				if (!boxNum.gameObject.activeSelf)
				{
					boxNum.gameObject.SetActive(true);
				}
				if (objInfinityNum.activeSelf)
				{
					objInfinityNum.SetActive(false);
				}
				boxNum.Text = value.ToString();
			}
		}
	}

	private void Start()
	{
		if (_bEnableTest)
		{
			UIBoxMagazine[] array = new UIBoxMagazine[2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new UIBoxMagazine(2, 2);
			}
			InitBoxMagazine(array, 0, false);
			UIBoxMagazine[] array2 = new UIBoxMagazine[1];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new UIBoxMagazine(20, 20);
			}
		}
	}

	private void Update()
	{
		if (_bEnableTest && bTest)
		{
			RefreshCurBulletNum(nCurBulletNum);
			bTest = false;
		}
		if (_bEnableTest && bTestChangeBox)
		{
			ChangeBoxMagazine(0);
			bTestChangeBox = false;
		}
	}

	public int InitBoxMagazine(UIBoxMagazine[] cartridges, int nCurIndex, bool bInfinityMode)
	{
		curBoxIndex = nCurIndex;
		_bInfinityMode = false;
		if (bInfinityMode)
		{
			_bInfinityMode = true;
		}
		else if (nCurIndex < 0 || nCurIndex >= cartridges.Length)
		{
			Debug.LogError("out of range:nCurIndex");
			return -1;
		}
		lstCartridges.Clear();
		lstCartridges.AddRange(cartridges);
		if (bInfinityMode)
		{
			BoxNum = -1;
		}
		else
		{
			BoxNum = lstCartridges.Count - 1;
		}
		RefreshCurBulletNum(lstCartridges[curBoxIndex].curBulletNum);
		int num = 0;
		for (int i = 0; i < lstCartridges.Count; i++)
		{
			num = ((i != 0) ? (num + lstCartridges[i].maxBulletCount) : (num + lstCartridges[0].curBulletNum));
		}
		_bulletAmount = num;
		return BoxNum;
	}

	public int RefreshCurBulletNum(int nCurNum)
	{
		return RefreshCurBulletNum(nCurNum, false);
	}

	public int RefreshCurBulletNum(int nCurNum, bool bAutoChange)
	{
		if (curBoxIndex < 0 || curBoxIndex >= lstCartridges.Count)
		{
			return -1;
		}
		lstCartridges[curBoxIndex].curBulletNum = nCurNum;
		BoxNum = lstCartridges.Count - 1;
		BulletNum = lstCartridges[curBoxIndex].curBulletNum;
		int num = nCurNum;
		for (int i = 1; i < lstCartridges.Count; i++)
		{
			num += lstCartridges[i].maxBulletCount;
		}
		BoxNum = num - nCurNum;
		if (nCurNum == 0)
		{
			if (_bInfinityMode)
			{
				lstCartridges[0].curBulletNum = lstCartridges[0].maxBulletCount;
			}
			else
			{
				lstCartridges.RemoveAt(curBoxIndex);
			}
			if (bAutoChange)
			{
				StartCoroutine(ChangeBoxMagazine());
			}
		}
		return nCurNum;
	}

	public void ChangeBoxMagazine(int nCur)
	{
		curBoxIndex = nCur;
		if (curBoxIndex >= 0 && curBoxIndex < lstCartridges.Count)
		{
			RefreshCurBulletNum(lstCartridges[curBoxIndex].curBulletNum);
		}
	}

	public IEnumerator ChangeBoxMagazine()
	{
		yield return 0;
		ChangeBoxMagazine(0);
	}
}
