using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIMarket_FittingRoomContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private List<UIMarket_FittingBoxData> _lstSelectedAvatar = new List<UIMarket_FittingBoxData>();

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIMarket_FittingRoomUnwearAvatarEnd, this, OnFittingRoomUnwearAvatarEnd);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIMarket_FittingRoomUnwearAvatarEnd, this);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Hide_Charc);
	}

	private bool OnFittingRoomUnwearAvatarEnd(TUITelegram msg)
	{
		for (int i = 0; i < _lstSelectedAvatar.Count; i++)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Wear, _lstSelectedAvatar[i]);
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Multi)
		{
			UIMarket_FittingBoxData uIMarket_FittingBoxData = box.BoxData as UIMarket_FittingBoxData;
			if (uIMarket_FittingBoxData != null)
			{
				if (IsExistInPreList(box))
				{
					loseSel = box;
					return false;
				}
				loseSel = null;
				return true;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		Debug.Log("Box Cann't Selected!");
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxSelected(box);
		if (!_lstSelectedAvatar.Contains((UIMarket_FittingBoxData)box.BoxData))
		{
			_lstSelectedAvatar.Add((UIMarket_FittingBoxData)box.BoxData);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Wear, box.BoxData);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		if (box != null)
		{
			if (_lstSelectedAvatar.Contains((UIMarket_FittingBoxData)box.BoxData))
			{
				_lstSelectedAvatar.Remove((UIMarket_FittingBoxData)box.BoxData);
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.UnWear, box.BoxData);
		}
	}

	public void InitFittingRoom(List<UIMarket_FittingBoxData> lst)
	{
		_lstSelectedAvatar.Clear();
		for (int i = 0; i < lst.Count; i++)
		{
			_lstSelectedAvatar.Add(lst[i]);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Init, lst);
	}
}
