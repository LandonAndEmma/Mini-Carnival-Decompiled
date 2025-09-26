using MessageID;
using UnityEngine;

public class UI_NGActorController : UIEntity
{
	[SerializeField]
	private Camera _actorCamera;

	[SerializeField]
	private COMA_PlayerSelfCharacter _actorCmp;

	[SerializeField]
	private UITexture _texScreen;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquareCompleted, this, SquareCompleted);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquareCompleted, this);
	}

	private bool SquareCompleted(TUITelegram msg)
	{
		SetShow(false);
		COMA_Pref.Instance.NG2_1_FirstEnterSquare = false;
		COMA_Pref.Instance.Save(true);
		return true;
	}

	private void SetShow(bool b)
	{
		_actorCamera.enabled = b;
		_actorCmp.gameObject.SetActive(b);
		_texScreen.enabled = b;
	}

	private void Awake()
	{
		if (Application.loadedLevelName == "UI.RPG.MyTerm")
		{
			Debug.Log(COMA_Pref.Instance.NG2_1_FirstEnterTeam);
			SetShow(COMA_Pref.Instance.NG2_1_FirstEnterTeam);
		}
		else if (Application.loadedLevelName == "UI.RPG.Map")
		{
			if (COMA_Pref.Instance.NG2_1_FirstEnterMap)
			{
				SetShow(true);
			}
			else if (COMA_Pref.Instance.NG2_1_FirstBackToMap && UIDataBufferCenter.Instance.CurBattleLevelLV == -1 && UIDataBufferCenter.Instance.CurBattleLevelIndex != -1)
			{
				SetShow(true);
			}
			else
			{
				SetShow(false);
			}
		}
		else if (Application.loadedLevelName == "UI.Square" && UIDataBufferCenter.Instance.CurNGIndex == 7)
		{
			SetShow(true);
		}
		else
		{
			SetShow(COMA_Pref.Instance.NG2_1_FirstEnterSquare);
		}
	}

	private void Start()
	{
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			if (UIDataBufferCenter.Instance.CurNGIndex == 0)
			{
				_actorCmp.Enter();
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 4)
			{
				_actorCmp.Ani_In_from_right();
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 5)
			{
				_actorCmp.Ani_In_from_right();
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 6)
			{
				_actorCmp.Ani_In_from_right(4);
			}
		}
		if (Application.loadedLevelName == "UI.RPG.MyTerm" && COMA_Pref.Instance.NG2_1_FirstEnterTeam)
		{
			_actorCmp.Ani_In_from_left(3);
		}
		if (Application.loadedLevelName == "UI.RPG.Map" && COMA_Pref.Instance.NG2_1_FirstEnterMap)
		{
			_actorCmp.Ani_In_from_right();
		}
		if (Application.loadedLevelName == "UI.Square" && UIDataBufferCenter.Instance.CurNGIndex == 7)
		{
			_actorCmp.Ani_In_from_right();
		}
	}

	protected override void Tick()
	{
	}
}
