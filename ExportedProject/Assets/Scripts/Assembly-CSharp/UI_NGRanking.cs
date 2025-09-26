using MessageID;
using UnityEngine;

public class UI_NGRanking : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RankingIntroduceID, this, IntroduceID);
		RegisterMessage(EUIMessageID.UING_RankingBoardBtnDown, this, RankingBoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_FirstEnterFriends);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RankingIntroduceID, this);
		UnregisterMessage(EUIMessageID.UING_RankingBoardBtnDown, this);
	}

	private bool RankingBoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		case 0:
			OpenBoard(1);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RankingIntroduceID, this, null);
			Debug.Log("First Board Click!!!");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FirstClickNG, this, null);
			break;
		case 1:
			COMA_Pref.Instance.NG2_FirstEnterFriends = false;
			COMA_Pref.Instance.Save(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RankingIntroduceIDEnd, this, null);
			CloseAllBoard();
			_objNG.SetActive(false);
			break;
		}
		return true;
	}

	private bool IntroduceID(TUITelegram msg)
	{
		return true;
	}

	private void Awake()
	{
		_objNG.SetActive(false);
	}

	protected override void Tick()
	{
	}
}
