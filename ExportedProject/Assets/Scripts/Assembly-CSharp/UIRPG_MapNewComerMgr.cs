using MessageID;
using UnityEngine;

public class UIRPG_MapNewComerMgr : UIEntity
{
	[SerializeField]
	private GameObject[] _newComerObj;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		switch (num)
		{
		case 0:
			_newComerObj[num + 1].SetActive(true);
			break;
		case 1:
			_newComerObj[num].SetActive(false);
			_newComerObj[num + 1].SetActive(true);
			break;
		case 2:
			_newComerObj[num].SetActive(false);
			_newComerObj[num + 1].SetActive(true);
			break;
		case 3:
			_newComerObj[num].SetActive(false);
			break;
		}
		return true;
	}
}
