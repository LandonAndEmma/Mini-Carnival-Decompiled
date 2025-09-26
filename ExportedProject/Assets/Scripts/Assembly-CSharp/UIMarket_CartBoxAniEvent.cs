using MC_UIToolKit;
using UnityEngine;

public class UIMarket_CartBoxAniEvent : MonoBehaviour
{
	public delegate void UIBoxDelEvent();

	private UIBoxDelEvent boxDelEvent;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartAni(UIBoxDelEvent boxEvent)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		boxDelEvent = boxEvent;
		base.animation.Play();
	}

	public void AniEnd()
	{
		if (boxDelEvent != null)
		{
			UIGolbalStaticFun.CloseBlockForTUIMessageBox();
			boxDelEvent();
			boxDelEvent = null;
		}
	}
}
