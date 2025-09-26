using UnityEngine;

public class UI_ScrollControlRefreshMgr : MonoBehaviour
{
	[SerializeField]
	private GameObject _tipsLabel;

	[SerializeField]
	private string[] _tipsDes;

	[SerializeField]
	private TUIScrollList _scrollList;

	[SerializeField]
	private GameObject[] _aniPic;

	private float fSelfX;

	private float fSelfY;

	private void Awake()
	{
		fSelfX = base.transform.position.x;
		fSelfY = base.transform.position.y;
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
		GameObject mover = _scrollList.Mover;
		if (mover != null)
		{
			Vector3 position = base.gameObject.transform.position;
			position.x = mover.transform.position.x + fSelfX - _scrollList.gameObject.transform.position.x;
			position.y = mover.transform.position.y + fSelfY - _scrollList.gameObject.transform.position.y;
			base.gameObject.transform.position = position;
		}
	}

	public void ActiveRefreshUI()
	{
		_tipsLabel.SetActive(true);
		_tipsLabel.GetComponent<TUILabel>().TextID = _tipsDes[0];
	}

	public void RefreshUIStateChanged(int nState)
	{
		_tipsLabel.GetComponent<TUILabel>().TextID = _tipsDes[nState];
		switch (nState)
		{
		case 2:
			_aniPic[1].SetActive(true);
			_aniPic[0].SetActive(false);
			break;
		case 1:
			_aniPic[0].SetActive(true);
			_aniPic[1].SetActive(false);
			break;
		case 0:
			_aniPic[0].SetActive(false);
			_aniPic[1].SetActive(false);
			break;
		}
	}
}
