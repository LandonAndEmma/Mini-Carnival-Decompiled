using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol.Shop.C2S;
using UnityEngine;

public class UIBackpack_SellItemsContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UILabel _labelSellPrice;

	[SerializeField]
	private UICheckbox _isAdvertise;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIBackpack_SellItems, this, SellItems);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIBackpack_SellItems, this);
	}

	private bool SellItems(TUITelegram msg)
	{
		int count = _preSelBoxLst.Count;
		if (count <= 0)
		{
			Debug.LogWarning("No Item Can Sell");
			string str = TUITool.StringFormat(Localization.instance.Get("jiaoyijiemian_desc24"));
			UIGolbalStaticFun.PopupTipsBox(str);
			return false;
		}
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SellItem);
		SellAvatarCmd sellAvatarCmd = new SellAvatarCmd();
		sellAvatarCmd.m_price = uint.Parse(_labelSellPrice.text);
		sellAvatarCmd.m_price_type = 1;
		sellAvatarCmd.m_remain_num = (byte)COMA_DataConfig.Instance._sysConfig.Shop.item_num;
		sellAvatarCmd.m_is_ad = (byte)(_isAdvertise.isChecked ? 1u : 0u);
		sellAvatarCmd.m_head_id = 0uL;
		sellAvatarCmd.m_body_id = 0uL;
		sellAvatarCmd.m_leg_id = 0uL;
		for (int i = 0; i < count; i++)
		{
			UIBackpack_BoxData uIBackpack_BoxData = (UIBackpack_BoxData)_preSelBoxLst[i].BoxData;
			if (uIBackpack_BoxData.DataType == 2)
			{
				sellAvatarCmd.m_head_id = uIBackpack_BoxData.ItemId;
			}
			else if (uIBackpack_BoxData.DataType == 3)
			{
				sellAvatarCmd.m_body_id = uIBackpack_BoxData.ItemId;
			}
			else if (uIBackpack_BoxData.DataType == 4)
			{
				sellAvatarCmd.m_leg_id = uIBackpack_BoxData.ItemId;
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, sellAvatarCmd);
		Debug.Log("===============================================\n");
		Debug.Log("Sell Items : head id = " + sellAvatarCmd.m_head_id + " body id=" + sellAvatarCmd.m_body_id + " leg id=" + sellAvatarCmd.m_leg_id + " Price=" + sellAvatarCmd.m_price);
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
			UIBackpack_BoxData uIBackpack_BoxData = box.BoxData as UIBackpack_BoxData;
			if (uIBackpack_BoxData != null)
			{
				if (box != _curSelBox && uIBackpack_BoxData.DataType != 1 && !IsExistInPreList(box))
				{
					NGUI_COMUI.UI_Box uI_Box = IsExistSameTypeInPreList(box);
					loseSel = uI_Box;
					return true;
				}
				if (IsExistInPreList(box))
				{
					loseSel = box;
					return false;
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

	public bool IsInSellList(ulong id)
	{
		for (int i = 0; i < _preSelBoxLst.Count; i++)
		{
			UIBackpack_BoxData uIBackpack_BoxData = _preSelBoxLst[i].BoxData as UIBackpack_BoxData;
			if (uIBackpack_BoxData.ItemId == id)
			{
				return true;
			}
		}
		return false;
	}
}
