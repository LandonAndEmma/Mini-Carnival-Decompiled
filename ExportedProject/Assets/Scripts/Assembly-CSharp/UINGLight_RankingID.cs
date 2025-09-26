using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UINGLight_RankingID : UINGLight_Square
{
	protected List<Color> _oriColorLst = new List<Color>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RankingIntroduceID, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_RankingIntroduceIDEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RankingIntroduceID, this);
		UnregisterMessage(EUIMessageID.UING_RankingIntroduceIDEnd, this);
	}

	private bool IntroduceBtn(TUITelegram msg)
	{
		LightOn();
		_oriColorLst.Clear();
		for (int i = 0; i < _needLightsObj.Length; i++)
		{
			_oriColorLst.Add(_needLightsObj[i].color);
			_needLightsObj[i].color = Color.white;
		}
		return true;
	}

	private bool IntroduceBtnEnd(TUITelegram msg)
	{
		LightOff();
		for (int i = 0; i < _needLightsObj.Length; i++)
		{
			_needLightsObj[i].color = _oriColorLst[i];
		}
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
