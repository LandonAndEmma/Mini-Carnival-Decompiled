using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UI_NGSquare : UI_NGBoardMgr
{
	[SerializeField]
	private UITexture _texPlayer;

	private float _fOriAlpha;

	[SerializeField]
	private UISprite _blockSprite;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_MoveDownEnd, this, MoveDownEnd);
		RegisterMessage(EUIMessageID.UING_SquareChangeBoard, this, ChangeBoard);
		RegisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this, SquareBoardBtnDown);
		RegisterMessage(EUIMessageID.UING_SquareIntroduceChatBtnEnd, this, SquareIntroduceChatBtnEnd);
		RegisterMessage(EUIMessageID.UING_SquarePointMails, this, PointMails);
		RegisterMessage(EUIMessageID.UING_SquareMoveLeftEnd, this, MoveLeftEnd);
		RegisterMessage(EUIMessageID.UING_SquareBowEnd, this, BowEnd);
		RegisterMessage(EUIMessageID.UISquare_GotoCardMgrClick, this, GotoCardMgrClick);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			if (UIDataBufferCenter.Instance.CurNGIndex == 4)
			{
				OpenBoard(4);
			}
			if (UIDataBufferCenter.Instance.CurNGIndex == 6)
			{
				OpenBoard(5);
			}
		}
		if (UIDataBufferCenter.Instance.CurNGIndex == 7)
		{
			_objNG.SetActive(true);
			OpenBoard(6);
		}
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_MoveDownEnd, this);
		UnregisterMessage(EUIMessageID.UING_SquareChangeBoard, this);
		UnregisterMessage(EUIMessageID.UING_SquareBoardBtnDown, this);
		UnregisterMessage(EUIMessageID.UING_SquareIntroduceChatBtnEnd, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointMails, this);
		UnregisterMessage(EUIMessageID.UING_SquareMoveLeftEnd, this);
		UnregisterMessage(EUIMessageID.UING_SquareBowEnd, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoCardMgrClick, this);
	}

	private bool MoveDownEnd(TUITelegram msg)
	{
		OpenBoard(1);
		return true;
	}

	private bool ChangeBoard(TUITelegram msg)
	{
		OpenBoard((int)msg._pExtraInfo);
		if ((int)msg._pExtraInfo == 0 && COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 4 && _texPlayer != null)
		{
			_texPlayer.enabled = false;
		}
		return true;
	}

	private bool GotoCardMgrClick(TUITelegram msg)
	{
		if (_objNG.activeSelf)
		{
			_actorCmp.Ani_Out_to_right();
			OpenBoard(0);
		}
		return true;
	}

	private bool SquareBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		switch (num)
		{
		case 1:
		case 2:
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareChangeBoard, null, num + 1);
			UIDataBufferCenter.Instance.CurNGIndex = 1;
			break;
		case 55:
			CloseAllBoard();
			_objNG.SetActive(false);
			COMA_Pref.Instance.NG2_1_FirstEnterSquare = false;
			COMA_Pref.Instance.Save(true);
			_actorCmp.Ani_Out_to_right();
			break;
		case 6:
			return true;
		case 66:
			Debug.Log("6666666666666666666666666666666666666666666666666");
			OpenBoard(0);
			_actorCmp.Ani_Out_to_right();
			UIDataBufferCenter.Instance.CurNGIndex = 8;
			break;
		case 7:
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointGameModesEnd, null, null);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareChangeBoard, null, num + 1);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointMarket, null, null);
			_actorCmp.PointRightdown02();
			break;
		case 8:
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointMarketEnd, null, null);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareMoveLeftEnd, null, null);
			break;
		case 9:
			_actorCmp.Point_down02_again();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareChangeBoard, null, num + 1);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointBackpack, null, null);
			break;
		case 10:
			_actorCmp.Bow();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareChangeBoard, null, num + 1);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointMiscEnd, null, null);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointBackpackEnd, null, null);
			break;
		case 11:
			BowEnd(null);
			break;
		}
		return true;
	}

	private bool SquareIntroduceChatBtnEnd(TUITelegram msg)
	{
		OpenBoard(0);
		_actorCmp.MoveRight();
		return true;
	}

	private bool PointMails(TUITelegram msg)
	{
		OpenBoard(5);
		return true;
	}

	private bool MoveLeftEnd(TUITelegram msg)
	{
		_actorCmp.Point_down02();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareChangeBoard, null, 9);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointMisc, null, null);
		return true;
	}

	private bool BowEnd(TUITelegram msg)
	{
		CloseAllBoard();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareCompleted, null, null);
		_objNG.SetActive(false);
		UILoginFacebookMessageBoxData uILoginFacebookMessageBoxData = new UILoginFacebookMessageBoxData();
		uILoginFacebookMessageBoxData.Mark = "LoginFacebook";
		UIGolbalStaticFun.PopFacebookMessageBox(uILoginFacebookMessageBoxData);
		return true;
	}

	private void Awake()
	{
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterSquare);
	}

	protected override void Tick()
	{
	}
}
