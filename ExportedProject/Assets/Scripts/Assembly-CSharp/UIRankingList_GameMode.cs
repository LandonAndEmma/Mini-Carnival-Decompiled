using UnityEngine;

public class UIRankingList_GameMode : MonoBehaviour
{
	[SerializeField]
	private GameObject _selFrame;

	[SerializeField]
	private int _nModeIndex;

	public int ModeIndex
	{
		get
		{
			return _nModeIndex;
		}
	}

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
