using UnityEngine;

public class UIRankingList_RankingArear : MonoBehaviour
{
	[SerializeField]
	private GameObject _selFrame;

	private void Awake()
	{
		NotifyGetFocus(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void NotifyGetFocus(bool bFocus)
	{
		if (_selFrame != null)
		{
			_selFrame.SetActive(bFocus);
		}
	}
}
