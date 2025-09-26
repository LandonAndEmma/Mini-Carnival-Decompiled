using System.Collections.Generic;
using UnityEngine;

public class UI_PageIndicateMgr : MonoBehaviour
{
	private List<UI_PageIndicate> lstPageIndicate;

	[SerializeField]
	private float fSpace = 11f;

	[SerializeField]
	private int pagePerPrefabNum = 3;

	private int curPageIndex;

	private int maxPageIndex;

	[SerializeField]
	private GameObject pageIndicatePrefab;

	private void Awake()
	{
		if (pageIndicatePrefab == null)
		{
			pageIndicatePrefab = Resources.Load("UI/Misc/PageIndicate") as GameObject;
		}
		lstPageIndicate = new List<UI_PageIndicate>();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ClearIndicate()
	{
		foreach (UI_PageIndicate item in lstPageIndicate)
		{
			Object.Destroy(item.gameObject);
		}
		lstPageIndicate.Clear();
	}

	public void InitIndicates(int nNum)
	{
		maxPageIndex = nNum;
		ClearIndicate();
		float num = (float)(-(maxPageIndex / 2)) * fSpace;
		for (int i = 0; i < nNum; i++)
		{
			GameObject gameObject = Object.Instantiate(pageIndicatePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = new Vector3((float)i * fSpace + num, 0f, -1f);
			UI_PageIndicate component = gameObject.GetComponent<UI_PageIndicate>();
			lstPageIndicate.Add(component);
		}
		SetPage(1);
	}

	public int SetPage(int nPage)
	{
		if (nPage < 1 || nPage > maxPageIndex)
		{
			return -1;
		}
		for (int i = 0; i < maxPageIndex; i++)
		{
			UI_PageIndicate uI_PageIndicate = lstPageIndicate[i];
			if (nPage - 1 == i)
			{
				uI_PageIndicate.SetCurPage();
			}
			else
			{
				uI_PageIndicate.SetNextPage();
			}
		}
		return 0;
	}
}
