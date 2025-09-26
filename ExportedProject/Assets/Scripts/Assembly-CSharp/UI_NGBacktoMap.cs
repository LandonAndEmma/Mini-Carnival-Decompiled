using System.Collections;
using MessageID;
using UnityEngine;

public class UI_NGBacktoMap : UI_NGBoardMgr
{
	[SerializeField]
	private UITexture _playerTex;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		Debug.Log("Cur LV=" + UIDataBufferCenter.Instance.CurBattleLevelLV);
		Debug.Log("NG2_1_FirstBackToMap=" + COMA_Pref.Instance.NG2_1_FirstBackToMap);
		if (UIDataBufferCenter.Instance.CurBattleLevelLV == -1 && UIDataBufferCenter.Instance.CurBattleLevelIndex != -1)
		{
			_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstBackToMap);
			_actorCmp.Ani_In_from_right(5);
		}
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
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			_actorCmp.PointLeftup();
			break;
		case 1:
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			_actorCmp.PointLeftup();
			break;
		case 2:
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			_actorCmp.PointLeftup();
			break;
		case 3:
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			_actorCmp.CreateAccouterment("HT05");
			_actorCmp.CreateAccouterment("HDR1000");
			_actorCmp.FarmerAttackNG();
			break;
		case 4:
			Debug.Log(" End NG!!");
			COMA_Pref.Instance.NG2_1_FirstBackToMap = false;
			COMA_Pref.Instance.Save(true);
			_actorCmp.Ani_Out_to_right();
			CloseAllBoard();
			_objNG.SetActive(false);
			StartCoroutine(HidePlayer());
			break;
		}
		return true;
	}

	private IEnumerator HidePlayer()
	{
		yield return new WaitForSeconds(0.87f);
		_playerTex.enabled = false;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
