using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPGCardManage_Container : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private int containerType;

	protected override void Load()
	{
		base.Load();
	}

	protected override void UnLoad()
	{
		base.UnLoad();
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null && box.BoxData.DataType > 2)
			{
				if (box != _curSelBox)
				{
					loseSel = _curSelBox;
					return true;
				}
				loseSel = null;
				return false;
			}
			loseSel = null;
			return false;
		}
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxSelected(box);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICardLibary_CardClick, null, box);
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		if (box.BoxData.DataType == 2 || box.BoxData.DataType == 1)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICardMgr_UnlockBtnClick, null, null);
		}
	}

	public override void DataSort()
	{
		List<NGUI_COMUI.UI_BoxData> list = new List<NGUI_COMUI.UI_BoxData>();
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = base.LstBoxs[i].BoxData as UIRPG_CardMgr_Card_BoxData;
			if (uIRPG_CardMgr_Card_BoxData != null && uIRPG_CardMgr_Card_BoxData.DataType == 3)
			{
				list.Add(base.LstBoxs[i].BoxData);
			}
		}
		list.Sort();
		Debug.Log(list.Count);
		if (containerType == 0)
		{
			for (int j = 0; j < list.Count; j++)
			{
				base.LstBoxs[j].BoxData = list[j];
			}
			return;
		}
		int num = 0;
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			base.LstBoxs[num++].BoxData = list[num2];
		}
	}
}
