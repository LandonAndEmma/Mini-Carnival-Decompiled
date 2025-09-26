using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class UIBackpack_Container : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private GameObject _desginBtn;

	[SerializeField]
	private GameObject _del_Backpack_Btn;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIBackpack_Design, this, DesignItem);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIBackpack_Design, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "UnlockBagCell":
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			BuyBagCellCmd buyBagCellCmd = new BuyBagCellCmd();
			buyBagCellCmd.m_buy_num = 1;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, buyBagCellCmd);
			break;
		}
		}
		return true;
	}

	private bool DesignItem(TUITelegram msg)
	{
		if (_curSelBox != null && _curSelBox.BoxData != null)
		{
			UIBackpack_BoxData uIBackpack_BoxData = (UIBackpack_BoxData)_curSelBox.BoxData;
			Debug.Log(string.Concat("current item state : ", uIBackpack_BoxData.DataState, "    item id : ", uIBackpack_BoxData.ItemId));
			if (uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditCanSell || uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditNoSell)
			{
				UIDataBufferCenter.Instance.SelectBoxDataForDesign = uIBackpack_BoxData;
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				Application.LoadLevelAdditive("UI.Doodle1");
				_curSelBox = null;
			}
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
		if (base.BoxSelType == EBoxSelType.Single)
		{
			if (box.BoxData != null)
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

	public void EnableDelBackpackBtn(bool b)
	{
		_del_Backpack_Btn.SetActive(b);
	}

	public void EnableDesginBtn(bool b)
	{
		Debug.Log("Btn Desgin:" + b);
		_desginBtn.SetActive(b);
		if (b && COMA_Pref.Instance.NG2_FirstEnterBackpackEdit)
		{
			Debug.Log("-------------------COMA_Pref.Instance.NG2_FirstEnterBackpackEdit=" + COMA_Pref.Instance.NG2_FirstEnterBackpackEdit);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstUseEditBtn, null, null);
		}
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
		Debug.Log("Cann't Selected .");
		if (!(_curSelBox == box) || !(_curSelBox != null) || _curSelBox.BoxData == null)
		{
			return;
		}
		UIBackpack_BoxData uIBackpack_BoxData = (UIBackpack_BoxData)_curSelBox.BoxData;
		Debug.Log(string.Concat("current item state : ", uIBackpack_BoxData.DataState, "    item id : ", uIBackpack_BoxData.ItemId));
		if (uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditCanSell || uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditNoSell)
		{
			EnableDesginBtn(true);
		}
		else
		{
			EnableDesginBtn(false);
		}
		if (uIBackpack_BoxData.DataType == 0)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("beibaojiemian_desc7"), 1, COMA_DataConfig.Instance._sysConfig.Bag.unlock_cost);
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "UnlockBagCell";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return;
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		if (uIBackpack_BoxData.DataType == 2)
		{
			if (info.m_head != 0L && info.m_head == uIBackpack_BoxData.ItemId)
			{
				info.m_head = 0uL;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
			}
			else if (info.m_head == 0L)
			{
				info.m_head = uIBackpack_BoxData.ItemId;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
			}
		}
		else if (uIBackpack_BoxData.DataType == 3)
		{
			if (info.m_body != 0L && info.m_body == uIBackpack_BoxData.ItemId)
			{
				info.m_body = 0uL;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
			}
			else if (info.m_body == 0L)
			{
				info.m_body = uIBackpack_BoxData.ItemId;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
			}
		}
		else if (uIBackpack_BoxData.DataType == 4)
		{
			if (info.m_leg != 0L && info.m_leg == uIBackpack_BoxData.ItemId)
			{
				info.m_leg = 0uL;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
			}
			else if (info.m_leg == 0L)
			{
				info.m_leg = uIBackpack_BoxData.ItemId;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
			}
		}
		else if (uIBackpack_BoxData.DataType == 5)
		{
			switch (COMA_PackageItem.NameToPart(uIBackpack_BoxData.Unit))
			{
			case 1:
				if (info.m_head_top != 0L && info.m_head_top == uIBackpack_BoxData.ItemId)
				{
					info.m_head_top = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_head_top == 0L)
				{
					info.m_head_top = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 2:
				if (info.m_head_front != 0L && info.m_head_front == uIBackpack_BoxData.ItemId)
				{
					info.m_head_front = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_head_front == 0L)
				{
					info.m_head_front = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 3:
				if (info.m_head_back != 0L && info.m_head_back == uIBackpack_BoxData.ItemId)
				{
					info.m_head_back = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_head_back == 0L)
				{
					info.m_head_back = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 4:
				if (info.m_head_left != 0L && info.m_head_left == uIBackpack_BoxData.ItemId)
				{
					info.m_head_left = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_head_left == 0L)
				{
					info.m_head_left = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 5:
				if (info.m_head_right != 0L && info.m_head_right == uIBackpack_BoxData.ItemId)
				{
					info.m_head_right = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_head_right == 0L)
				{
					info.m_head_right = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 6:
				if (info.m_chest_front != 0L && info.m_chest_front == uIBackpack_BoxData.ItemId)
				{
					info.m_chest_front = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_chest_front == 0L)
				{
					info.m_chest_front = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			case 7:
				if (info.m_chest_back != 0L && info.m_chest_back == uIBackpack_BoxData.ItemId)
				{
					info.m_chest_back = 0uL;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Unequipped);
				}
				else if (info.m_chest_back == 0L)
				{
					info.m_chest_back = uIBackpack_BoxData.ItemId;
					COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
				}
				break;
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
	}

	public void ClearCurSelBox()
	{
		ProcessBoxLoseSelected(_curSelBox);
		_curSelBox = null;
	}

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxSelected(box);
		if (!(_curSelBox != null) || _curSelBox.BoxData == null)
		{
			return;
		}
		UIBackpack_BoxData uIBackpack_BoxData = (UIBackpack_BoxData)_curSelBox.BoxData;
		Debug.Log(string.Concat("current item state : ", uIBackpack_BoxData.DataState, "    item id : ", uIBackpack_BoxData.ItemId));
		if (uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditCanSell || uIBackpack_BoxData.DataState == UIBackpack_BoxData.EDataState.CanEditNoSell)
		{
			EnableDesginBtn(true);
		}
		else
		{
			EnableDesginBtn(false);
		}
		if (uIBackpack_BoxData.DataType == 0)
		{
			EnableDelBackpackBtn(false);
			string des = TUITool.StringFormat(Localization.instance.Get("beibaojiemian_desc7"), 1, COMA_DataConfig.Instance._sysConfig.Bag.unlock_cost);
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "UnlockBagCell";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return;
		}
		EnableDelBackpackBtn(true);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Equipped);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		if (uIBackpack_BoxData.DataType == 2)
		{
			info.m_head = uIBackpack_BoxData.ItemId;
		}
		else if (uIBackpack_BoxData.DataType == 3)
		{
			info.m_body = uIBackpack_BoxData.ItemId;
		}
		else if (uIBackpack_BoxData.DataType == 4)
		{
			info.m_leg = uIBackpack_BoxData.ItemId;
		}
		else if (uIBackpack_BoxData.DataType == 5)
		{
			switch (COMA_PackageItem.NameToPart(uIBackpack_BoxData.Unit))
			{
			case 1:
				info.m_head_top = uIBackpack_BoxData.ItemId;
				break;
			case 2:
				info.m_head_front = uIBackpack_BoxData.ItemId;
				break;
			case 3:
				info.m_head_back = uIBackpack_BoxData.ItemId;
				break;
			case 4:
				info.m_head_left = uIBackpack_BoxData.ItemId;
				break;
			case 5:
				info.m_head_right = uIBackpack_BoxData.ItemId;
				break;
			case 6:
				info.m_chest_front = uIBackpack_BoxData.ItemId;
				break;
			case 7:
				info.m_chest_back = uIBackpack_BoxData.ItemId;
				break;
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
	}

	protected override void ProcessBoxLoseSelected(NGUI_COMUI.UI_Box box)
	{
		base.ProcessBoxLoseSelected(box);
		if (box != null)
		{
			EnableDelBackpackBtn(false);
		}
	}
}
