using MessageID;
using UnityEngine;

public class UI_NGRPGTeam : UI_NGBoardMgr
{
	[SerializeField]
	private UISprite[] _arrows;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		RegisterMessage(EUIMessageID.UI_ASAniEnterEnd, this, ASAniEnterEnd);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterTeam);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
		UnregisterMessage(EUIMessageID.UI_ASAniEnterEnd, this);
	}

	private bool ASAniEnterEnd(TUITelegram msg)
	{
		Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@COMA_Pref.Instance.NG2_1_FirstEnterTeam=" + COMA_Pref.Instance.NG2_1_FirstEnterTeam);
		for (int i = 0; i < _arrows.Length; i++)
		{
			_arrows[i].gameObject.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterTeam);
		}
		OpenBoard(1);
		return true;
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 == 1 && _objNG.activeSelf)
		{
			CloseAllBoard();
			_objNG.SetActive(false);
			_actorCmp.Ani_Out_to_left();
			for (int i = 0; i < _arrows.Length; i++)
			{
				_arrows[i].gameObject.SetActive(false);
			}
			COMA_Pref.Instance.NG2_1_FirstInTeam = true;
			COMA_Pref.Instance.NG2_1_FirstEnterTeam = false;
			COMA_Pref.Instance.Save(true);
			UIDataBufferCenter.Instance.CurNGIndex = 7;
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
