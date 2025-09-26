using UnityEngine;

public class UI_PageIndicate : MonoBehaviour
{
	[SerializeField]
	private GameObject curPage;

	[SerializeField]
	private GameObject nextPage;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetCurPage()
	{
		curPage.active = true;
	}

	public void SetNextPage()
	{
		curPage.active = false;
	}

	protected static int CompareIndicate(UI_PageIndicate l, UI_PageIndicate r)
	{
		if (l.transform.position.x < r.transform.position.x)
		{
			return -1;
		}
		if (l.transform.position.x > r.transform.position.x)
		{
			return 1;
		}
		return 0;
	}
}
