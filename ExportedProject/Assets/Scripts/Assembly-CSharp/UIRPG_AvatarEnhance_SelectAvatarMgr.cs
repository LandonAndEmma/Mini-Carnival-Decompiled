using System.Collections.Generic;
using MC_UIToolKit;
using NGUI_COMUI;
using Protocol;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_AvatarEnhance_SelectAvatarMgr : UIEntity
{
	[SerializeField]
	private UIRPG_AvatarEnhance_SelectAvatarContainer _selectAvatarMgrContainer;

	[SerializeField]
	private UIRPG_AvatarEnhanceMgr _avatarMgr;

	[SerializeField]
	private UILabel _noAvatarLabel;

	[SerializeField]
	private UILabel _noAvatarNoticeLabel;

	[SerializeField]
	private UIDraggablePanel _draggablePanel;

	protected override void Tick()
	{
	}

	private bool IsCanEnhance(BagItem item)
	{
		if (item.m_unit == string.Empty || item.m_unit == "9a53aef61db65e1ed1298fca0cc15a3d" || item.m_unit == "54245d0a0b0c5c8305976247da71f59f" || item.m_unit == "6ba2377776d6c137ee29551baff81bb5")
		{
			return false;
		}
		return true;
	}

	public void InitContainer()
	{
		_selectAvatarMgrContainer.ClearContainer();
		_selectAvatarMgrContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		List<BagItem> bag_list = notifyRoleDataCmd.m_bag_data.m_bag_list;
		int num = 0;
		for (int i = 0; i < bag_list.Count; i++)
		{
			byte part = bag_list[i].m_part;
			if ((part == 1 || part == 2 || part == 3) && !UIGolbalStaticFun.IsItemEquiped(bag_list[i].m_unique_id) && IsCanEnhance(bag_list[i]))
			{
				Debug.Log(bag_list[i].m_unit);
				num++;
				UIRPG_AvatarEnhance_SelectAvatarBoxData uIRPG_AvatarEnhance_SelectAvatarBoxData = new UIRPG_AvatarEnhance_SelectAvatarBoxData(bag_list[i].m_unique_id, bag_list[i].m_unit);
				uIRPG_AvatarEnhance_SelectAvatarBoxData.BagItemData = bag_list[i];
				uIRPG_AvatarEnhance_SelectAvatarBoxData.IsSel = (_avatarMgr.PreSelAvatar.Contains(uIRPG_AvatarEnhance_SelectAvatarBoxData.ItemId) ? true : false);
				_selectAvatarMgrContainer.SetBoxData(_selectAvatarMgrContainer.AddBox(i), uIRPG_AvatarEnhance_SelectAvatarBoxData);
				UIGolbalStaticFun.GetAvatarPartTex((EAvatarPart)bag_list[i].m_part, bag_list[i].m_unit, uIRPG_AvatarEnhance_SelectAvatarBoxData);
			}
		}
		if (num == 0)
		{
			_noAvatarLabel.gameObject.SetActive(true);
			_noAvatarLabel.text = TUITool.StringFormat(Localization.instance.Get("avatarfactory_desc1"));
			_noAvatarNoticeLabel.gameObject.SetActive(true);
			_noAvatarNoticeLabel.text = TUITool.StringFormat(Localization.instance.Get("avatarfactory_desc1a"));
		}
		else
		{
			_noAvatarLabel.gameObject.SetActive(false);
			_noAvatarNoticeLabel.gameObject.SetActive(false);
		}
		if (num <= 18)
		{
			_draggablePanel.scale = Vector3.zero;
		}
		else
		{
			_draggablePanel.scale = new Vector3(0f, 1f, 0f);
		}
	}

	protected override void Load()
	{
		InitContainer();
	}

	protected override void UnLoad()
	{
	}
}
