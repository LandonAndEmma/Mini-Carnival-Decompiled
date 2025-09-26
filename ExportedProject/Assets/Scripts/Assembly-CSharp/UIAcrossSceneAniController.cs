using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIAcrossSceneAniController : UIEntity
{
	public delegate bool UIAniLinkEvent(object obj);

	private UIAniLinkEvent linkEvent;

	[SerializeField]
	protected List<Animation> ui_ACAniLst = new List<Animation>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_SetASAniEvent, this, SetASAniEvent);
		RegisterMessage(EUIMessageID.UI_OpenExitASAni, this, OpenExitASAni);
		RegisterMessage(EUIMessageID.UI_ASAniEnterEnd, this, ASAniEnterEnd);
		RegisterMessage(EUIMessageID.UI_ASAniExitEnd, this, ASAniExitEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_SetASAniEvent, this);
		UnregisterMessage(EUIMessageID.UI_OpenExitASAni, this);
		UnregisterMessage(EUIMessageID.UI_ASAniEnterEnd, this);
		UnregisterMessage(EUIMessageID.UI_ASAniExitEnd, this);
	}

	private bool SetASAniEvent(TUITelegram msg)
	{
		linkEvent = msg.linkEvent;
		return true;
	}

	private bool OpenExitASAni(TUITelegram msg)
	{
		for (int i = 0; i < ui_ACAniLst.Count; i++)
		{
			if (ui_ACAniLst[i].gameObject.activeInHierarchy)
			{
				ui_ACAniLst[i].Play(GetExitAnimationName(ui_ACAniLst[i]));
			}
		}
		return true;
	}

	private bool ASAniEnterEnd(TUITelegram msg)
	{
		if (linkEvent != null)
		{
			linkEvent(msg._pExtraInfo);
			linkEvent = null;
		}
		return true;
	}

	private bool ASAniExitEnd(TUITelegram msg)
	{
		if (linkEvent != null)
		{
			linkEvent(msg._pExtraInfo);
			linkEvent = null;
		}
		return true;
	}

	protected string GetEnterAnimationName(Animation ani)
	{
		int num = 0;
		foreach (AnimationState item in ani)
		{
			if (num == 1)
			{
				return item.name;
			}
			num++;
		}
		return string.Empty;
	}

	protected string GetExitAnimationName(Animation ani)
	{
		int num = 0;
		foreach (AnimationState item in ani)
		{
			if (num == 0)
			{
				return item.name;
			}
			num++;
		}
		return string.Empty;
	}
}
