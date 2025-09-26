using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_MyTeam_SelEquipMgr : UIEntity
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	[SerializeField]
	private UIRPG_MyTeamSelEquipBtnMgr _selEquipBtnMgr;

	[SerializeField]
	private UIRPG_MyTeam_SelEquipContainer _selEquipContainer;

	[SerializeField]
	private UILabel _noEquipLabel;

	[SerializeField]
	private UIDraggablePanel _uiDraggablePanel;

	public UIRPG_MyTeam_SelEquipContainer SelEquipContainer
	{
		get
		{
			return _selEquipContainer;
		}
	}

	protected override void Load()
	{
		InitContainer();
	}

	protected override void UnLoad()
	{
	}

	public void InitContainer()
	{
		if (_selEquipContainer == null)
		{
			Debug.Log("if (_selEquipContainer == null)");
		}
		_selEquipContainer.ClearContainer();
		_selEquipContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		StartCoroutine(MultiFrameAddContainerBox());
	}

	private IEnumerator MultiFrameAddContainerBox()
	{
		NotifyRPGDataCmd rpgData = UIDataBufferCenter.Instance.RPGData;
		Dictionary<ulong, Equip> avatarEquitBag = rpgData.m_equip_bag;
		int i = 0;
		int maxFrameBox = 20;
		int curi = i;
		foreach (ulong key in avatarEquitBag.Keys)
		{
			Equip avatarUnit = UIDataBufferCenter.Instance.RPGData.m_equip_bag[key];
			if ((byte)_selEquipBtnMgr.BtnList[_selEquipBtnMgr.CurPos].CurPart == avatarUnit.m_part)
			{
				UIRPG_MyTeam_SelEquipBoxData data = new UIRPG_MyTeam_SelEquipBoxData();
				data.ItemId = avatarUnit.m_id;
				data.EquipData = avatarUnit;
				int pos;
				data.IsEquip = IsEquip(data.ItemId, out pos);
				data.IsEquipBySelf = ((pos == ((UIRPG_MyTeamBoxData)_myTeamMgr.MyTeamContainer.CurSelBox.BoxData).CurPos) ? true : false);
				_selEquipContainer.SetBoxData(_selEquipContainer.AddBox(i), data);
				UIGolbalStaticFun.GetAvatarPartTex((EAvatarPart)avatarUnit.m_part, avatarUnit.m_md5, data);
				i++;
				if (i - curi >= maxFrameBox)
				{
					curi = i;
					yield return 0;
				}
			}
		}
		if (i == 0)
		{
			_noEquipLabel.text = TUITool.StringFormat(Localization.instance.Get("myteam_desc1"));
			_noEquipLabel.gameObject.SetActive(true);
		}
		else
		{
			_noEquipLabel.gameObject.SetActive(false);
		}
		Debug.Log("jfkaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa i = " + i);
		if (i <= 4)
		{
			_uiDraggablePanel.scale = Vector3.zero;
		}
		else
		{
			_uiDraggablePanel.scale = new Vector3(0f, 1f, 0f);
		}
	}

	public bool IsEquip(ulong itemId, out int pos)
	{
		bool flag = false;
		pos = -1;
		MemberSlot[] member_slot = UIDataBufferCenter.Instance.RPGData.m_member_slot;
		for (int i = 0; i < member_slot.Length; i++)
		{
			switch ((int)_selEquipBtnMgr.BtnList[_selEquipBtnMgr.CurPos].CurPart)
			{
			case 1:
				if (member_slot[i].m_head == itemId)
				{
					flag = true;
					pos = i;
				}
				break;
			case 2:
				if (member_slot[i].m_body == itemId)
				{
					flag = true;
					pos = i;
				}
				break;
			case 3:
				if (member_slot[i].m_leg == itemId)
				{
					flag = true;
					pos = i;
				}
				break;
			}
			if (flag)
			{
				break;
			}
		}
		return flag;
	}
}
