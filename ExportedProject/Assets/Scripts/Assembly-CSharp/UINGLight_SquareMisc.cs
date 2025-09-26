using MessageID;
using UnityEngine;

public class UINGLight_SquareMisc : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquarePointMisc, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_SquarePointMiscEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquarePointMisc, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointMiscEnd, this);
	}

	private bool IntroduceBtn(TUITelegram msg)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_OpenMiscContentButtonOnClick, null, null);
		LightOn();
		return true;
	}

	private bool IntroduceBtnEnd(TUITelegram msg)
	{
		Debug.Log("UING_SquarePointMiscEnd");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_CloseMiscContentButtonOnClick, null, null);
		LightOff();
		return true;
	}

	private new void Awake()
	{
		base.Awake();
	}

	protected override void Tick()
	{
	}
}
