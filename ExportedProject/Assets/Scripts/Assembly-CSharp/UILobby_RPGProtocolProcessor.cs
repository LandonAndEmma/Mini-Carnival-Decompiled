using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Binary;
using Protocol.RPG.S2C;
using UnityEngine;

public class UILobby_RPGProtocolProcessor : UILobbyMessageHandler
{
	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(7, this);
			OnMessage(0, OnNotigyRPGData);
			OnMessage(2, PickCardsResult);
			OnMessage(3, NotifyCardAdd);
			OnMessage(4, NotifyCardDel);
			OnMessage(5, NotifyCouponNum);
			OnMessage(7, NotifyCombineCardResult);
			OnMessage(9, NotifyCombineGemResult);
			OnMessage(10, NotifyGemCountChanged);
			OnMessage(12, EnhanceAvatarResult);
			OnMessage(13, NotifyAvatarAdd);
			OnMessage(15, NotifyGetShopGemResult);
			OnMessage(17, NotifyChangeMemberResult);
			OnMessage(19, NotifyMountEquipResult);
			OnMessage(21, NotifyRPGBagCapacityResult);
			OnMessage(23, NotifyMapBattleResult);
			OnMessage(24, NotifyMapChanged);
			OnMessage(25, NotifyMobilityChanged);
			OnMessage(26, NotifyMedalChanged);
			OnMessage(27, NotifyExpChanged);
			OnMessage(29, NotifyRefreshPlayerLevelResult);
			OnMessage(31, NotifyGainGoldResult);
			OnMessage(33, NotifyReqBattleResult);
			OnMessage(35, NotifyReplacePlayerResult);
			OnMessage(37, NotifyRPGOtherPlayerData);
			OnMessage(38, NotifyRPGOtherPlayerDataError);
			OnMessage(40, DragMedalRankResult);
			OnMessage(42, GetMedalRankPosResult);
			OnMessage(44, DragFriendMedalRankResult);
			OnMessage(46, NotifyBuyMobilityResult);
			OnMessage(47, NotifyDayFirstLoginAward);
			OnMessage(49, BuyCouponsResult);
			OnMessage(51, DecomposeEquipResult);
			OnMessage(52, NotifyRPGAvatarDel);
			OnMessage(54, NotifyGameDropResult);
			OnMessage(56, NotifyDelEquipResult);
			OnMessage(62, NotifyFirstRPGFinishResult);
			OnMessage(65, NotifyGainAllGoldResult);
			OnMessage(59, NotifyReveMobilityError);
			OnMessage(60, NotifyReveTicketError);
		}
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(7, this);
		}
	}

	private bool OnNotigyRPGData(UnPacker unpacker)
	{
		NotifyRPGDataCmd notifyRPGDataCmd = new NotifyRPGDataCmd();
		if (!notifyRPGDataCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIDataBufferCenter.Instance.RPGData = notifyRPGDataCmd;
		Debug.LogWarning(notifyRPGDataCmd.m_mobility_time);
		Debug.LogWarning(notifyRPGDataCmd.m_coupon);
		Debug.LogWarning("Member --------------------------------------");
		for (int i = 0; i < 5; i++)
		{
			Debug.LogWarning("i=" + i + "--------------------------");
			Debug.LogWarning("m_member=" + notifyRPGDataCmd.m_member_slot[i].m_member);
			Debug.LogWarning("m_unqiue=" + notifyRPGDataCmd.m_member_slot[i].m_unqiue);
			Debug.LogWarning("m_head=" + notifyRPGDataCmd.m_member_slot[i].m_head);
			Debug.LogWarning("m_body=" + notifyRPGDataCmd.m_member_slot[i].m_body);
			Debug.LogWarning("m_leg=" + notifyRPGDataCmd.m_member_slot[i].m_leg);
		}
		Debug.LogWarning("CardBag Capacity:" + notifyRPGDataCmd.m_card_capacity);
		foreach (uint key in notifyRPGDataCmd.m_card_list.Keys)
		{
			Debug.LogWarning("Card ID:" + key);
			List<ulong> list = notifyRPGDataCmd.m_card_list[key];
			Debug.LogWarning("Card Count:" + list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				Debug.LogWarning("Card unique:" + list[j]);
			}
			Debug.LogWarning("-----------------------------------------------------");
		}
		Debug.LogWarning("-----------------------------------------------------m_jewel_capacity=" + notifyRPGDataCmd.m_jewel_capacity);
		foreach (ushort key2 in notifyRPGDataCmd.m_jewel_list.Keys)
		{
			Debug.LogWarning("Gem Key:" + key2);
			Debug.LogWarning("Gem Count:" + notifyRPGDataCmd.m_jewel_list[key2]);
		}
		Debug.LogWarning("m_equip_capacity Capacity:" + notifyRPGDataCmd.m_equip_capacity);
		foreach (ulong key3 in notifyRPGDataCmd.m_equip_bag.Keys)
		{
			Debug.LogWarning("Equip Key:" + key3);
			Equip equip = notifyRPGDataCmd.m_equip_bag[key3];
			Debug.LogWarning("m_md5:" + equip.m_md5);
			Debug.LogWarning("m_part:" + equip.m_part);
			Debug.LogWarning("m_type:" + equip.m_type);
			Debug.LogWarning("m_level:" + equip.m_level);
			Debug.LogWarning("m_id:" + equip.m_id);
		}
		return true;
	}

	private bool PickCardsResult(UnPacker unpacker)
	{
		RequestPickCardsResultCmd requestPickCardsResultCmd = new RequestPickCardsResultCmd();
		if (!requestPickCardsResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------Pick Cards Result!");
		Debug.LogWarning("Result:" + requestPickCardsResultCmd.m_result);
		Debug.LogWarning("Cards Num:" + requestPickCardsResultCmd.m_card_num);
		for (int i = 0; i < requestPickCardsResultCmd.m_card_list.Count; i++)
		{
			Debug.LogWarning("Cards ID:" + requestPickCardsResultCmd.m_card_list[i].m_card_id);
			Debug.LogWarning("Cards unique:" + requestPickCardsResultCmd.m_card_list[i].m_unique_id);
			Debug.LogWarning("Cards IsNew:" + requestPickCardsResultCmd.m_card_list[i].m_new);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyPickCardsResult, null, requestPickCardsResultCmd);
		return true;
	}

	private bool NotifyCardAdd(UnPacker unpacker)
	{
		NotifyCardAddCmd notifyCardAddCmd = new NotifyCardAddCmd();
		if (!notifyCardAddCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------Add Cards!");
		Debug.LogWarning("Cards ID:" + notifyCardAddCmd.m_card_id);
		Debug.LogWarning("Cards unique:" + notifyCardAddCmd.m_unique_id);
		Debug.LogWarning("Cards IsNew:" + notifyCardAddCmd.m_new);
		Dictionary<uint, List<ulong>> card_list = UIDataBufferCenter.Instance.RPGData.m_card_list;
		if (card_list.ContainsKey(notifyCardAddCmd.m_card_id))
		{
			List<ulong> list = card_list[notifyCardAddCmd.m_card_id];
			if (list == null)
			{
				list = new List<ulong>();
			}
			list.Add(notifyCardAddCmd.m_unique_id);
		}
		else
		{
			List<ulong> list2 = new List<ulong>();
			list2.Add(notifyCardAddCmd.m_unique_id);
			card_list.Add(notifyCardAddCmd.m_card_id, list2);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyAddCard, null, notifyCardAddCmd);
		return true;
	}

	private bool NotifyCardDel(UnPacker unpacker)
	{
		NotifyCardDelCmd notifyCardDelCmd = new NotifyCardDelCmd();
		if (!notifyCardDelCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------Del Cards!");
		Debug.LogWarning("Cards ID:" + notifyCardDelCmd.m_card_id);
		Debug.LogWarning("Cards unique:" + notifyCardDelCmd.m_unique_id);
		Dictionary<uint, List<ulong>> card_list = UIDataBufferCenter.Instance.RPGData.m_card_list;
		if (card_list.ContainsKey(notifyCardDelCmd.m_card_id))
		{
			List<ulong> list = card_list[notifyCardDelCmd.m_card_id];
			if (list != null && list.Contains(notifyCardDelCmd.m_unique_id))
			{
				list.Remove(notifyCardDelCmd.m_unique_id);
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyDelCard, null, notifyCardDelCmd);
		return true;
	}

	private bool NotifyCouponNum(UnPacker unpacker)
	{
		NotifyCouponsCmd notifyCouponsCmd = new NotifyCouponsCmd();
		if (!notifyCouponsCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------CouponNum!");
		Debug.LogWarning("Coupon Num:" + notifyCouponsCmd.m_coupons);
		UIDataBufferCenter.Instance.RPGData.m_coupon = notifyCouponsCmd.m_coupons;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_CouponNumChange, null, notifyCouponsCmd);
		return true;
	}

	private bool NotifyCombineCardResult(UnPacker unpacker)
	{
		CombineCardResultCmd combineCardResultCmd = new CombineCardResultCmd();
		if (!combineCardResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		Debug.LogWarning("-------------------------------------CombineCardResult!");
		Debug.LogWarning("m_result:" + combineCardResultCmd.m_result);
		Debug.LogWarning("m_card_id:" + combineCardResultCmd.m_card_id);
		Debug.LogWarning("m_unique_id:" + combineCardResultCmd.m_unique_id);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyCombineCardResult, null, combineCardResultCmd);
		return true;
	}

	private bool NotifyCombineGemResult(UnPacker unpacker)
	{
		CombineGemResultCmd combineGemResultCmd = new CombineGemResultCmd();
		if (!combineGemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------CombineGemResultCmd!");
		Debug.LogWarning("m_result:" + combineGemResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyCombineGemResult, null, combineGemResultCmd);
		return true;
	}

	private bool NotifyGemCountChanged(UnPacker unpacker)
	{
		NotifyGemCmd notifyGemCmd = new NotifyGemCmd();
		if (!notifyGemCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyGemCmd!");
		Debug.LogWarning("m_gem_id:" + notifyGemCmd.m_gem_id);
		Debug.LogWarning("m_num:" + notifyGemCmd.m_num);
		Dictionary<ushort, uint> jewel_list = UIDataBufferCenter.Instance.RPGData.m_jewel_list;
		if (jewel_list.ContainsKey(notifyGemCmd.m_gem_id))
		{
			jewel_list[notifyGemCmd.m_gem_id] = notifyGemCmd.m_num;
		}
		else
		{
			jewel_list.Add(notifyGemCmd.m_gem_id, notifyGemCmd.m_num);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyGemNumChanged, null, notifyGemCmd);
		return true;
	}

	private bool EnhanceAvatarResult(UnPacker unpacker)
	{
		EnhanceAvatarResultCmd enhanceAvatarResultCmd = new EnhanceAvatarResultCmd();
		if (!enhanceAvatarResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------EnhanceAvatarResultCmd!");
		Debug.LogWarning("m_result:" + enhanceAvatarResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyEnhanceAvatarResult, null, enhanceAvatarResultCmd);
		return true;
	}

	private bool NotifyAvatarAdd(UnPacker unpacker)
	{
		NotifyAvatarAddCmd notifyAvatarAddCmd = new NotifyAvatarAddCmd();
		if (!notifyAvatarAddCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyAvatarAdd!");
		Debug.LogWarning("m_unique_id:" + notifyAvatarAddCmd.m_unique_id);
		Debug.LogWarning("m_md5:" + notifyAvatarAddCmd.m_md5);
		Debug.LogWarning("m_part:" + notifyAvatarAddCmd.m_part);
		Debug.LogWarning("m_type:" + notifyAvatarAddCmd.m_type);
		Debug.LogWarning("m_level:" + notifyAvatarAddCmd.m_level);
		Dictionary<ulong, Equip> equip_bag = UIDataBufferCenter.Instance.RPGData.m_equip_bag;
		if (!equip_bag.ContainsKey(notifyAvatarAddCmd.m_unique_id))
		{
			Equip equip = new Equip();
			equip.m_id = notifyAvatarAddCmd.m_unique_id;
			equip.m_md5 = notifyAvatarAddCmd.m_md5;
			equip.m_part = notifyAvatarAddCmd.m_part;
			equip.m_type = notifyAvatarAddCmd.m_type;
			equip.m_level = notifyAvatarAddCmd.m_level;
			equip_bag.Add(notifyAvatarAddCmd.m_unique_id, equip);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyAddAvatar, null, notifyAvatarAddCmd);
		return true;
	}

	private bool NotifyGetShopGemResult(UnPacker unpacker)
	{
		BuyGemShopItemResultCmd buyGemShopItemResultCmd = new BuyGemShopItemResultCmd();
		if (!buyGemShopItemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		Debug.LogWarning("-------------------------------------NotifyGetShopGemResult!");
		Debug.LogWarning("m_result:" + buyGemShopItemResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyGetShopGemResult, null, buyGemShopItemResultCmd);
		return true;
	}

	private bool NotifyChangeMemberResult(UnPacker unpacker)
	{
		ChangeMemberResultCmd changeMemberResultCmd = new ChangeMemberResultCmd();
		if (!changeMemberResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ChangeMemberResultCmd!");
		Debug.LogWarning("m_result:" + changeMemberResultCmd.m_result);
		Debug.LogWarning("m_pos:" + changeMemberResultCmd.m_pos);
		Debug.LogWarning("New m_member_card:" + changeMemberResultCmd.m_member_card);
		Debug.LogWarning("New m_member_unique_id:" + changeMemberResultCmd.m_member_unique_id);
		Debug.LogWarning("Old m_member_card:" + changeMemberResultCmd.m_unmember_card);
		Debug.LogWarning("Old m_member_unique_id:" + changeMemberResultCmd.m_unmember_unique_id);
		if (changeMemberResultCmd.m_result == 0)
		{
			UIDataBufferCenter.Instance.RPGData.m_member_slot[changeMemberResultCmd.m_pos].m_member = changeMemberResultCmd.m_member_card;
			UIDataBufferCenter.Instance.RPGData.m_member_slot[changeMemberResultCmd.m_pos].m_unqiue = changeMemberResultCmd.m_member_unique_id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, null, UIDataBufferCenter.Instance.RPGData.m_member_slot[changeMemberResultCmd.m_pos].m_member, (int)changeMemberResultCmd.m_pos);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyChangeMemberResult, null, changeMemberResultCmd);
		return true;
	}

	private bool NotifyMountEquipResult(UnPacker unpacker)
	{
		MountMemberEquipResultCmd mountMemberEquipResultCmd = new MountMemberEquipResultCmd();
		if (!mountMemberEquipResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------MountMemberEquipResultCmd!");
		Debug.LogWarning("m_result:" + mountMemberEquipResultCmd.m_result);
		Debug.LogWarning("m_member_pos:" + mountMemberEquipResultCmd.m_member_pos);
		Debug.LogWarning("m_part:" + (BagItem.Part)mountMemberEquipResultCmd.m_part);
		Debug.LogWarning("New m_mount_equip:" + mountMemberEquipResultCmd.m_mount_equip);
		Debug.LogWarning("Old m_unmount_equip:" + mountMemberEquipResultCmd.m_unmount_equip);
		if (mountMemberEquipResultCmd.m_result == 0)
		{
			switch ((BagItem.Part)mountMemberEquipResultCmd.m_part)
			{
			case BagItem.Part.head:
				UIDataBufferCenter.Instance.RPGData.m_member_slot[mountMemberEquipResultCmd.m_member_pos].m_head = mountMemberEquipResultCmd.m_mount_equip;
				break;
			case BagItem.Part.body:
				UIDataBufferCenter.Instance.RPGData.m_member_slot[mountMemberEquipResultCmd.m_member_pos].m_body = mountMemberEquipResultCmd.m_mount_equip;
				break;
			case BagItem.Part.leg:
				UIDataBufferCenter.Instance.RPGData.m_member_slot[mountMemberEquipResultCmd.m_member_pos].m_leg = mountMemberEquipResultCmd.m_mount_equip;
				break;
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMyTeam_PreviewChanged, null, UIDataBufferCenter.Instance.RPGData.m_member_slot[mountMemberEquipResultCmd.m_member_pos].m_member, (int)mountMemberEquipResultCmd.m_member_pos);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMountEquipResult, null, mountMemberEquipResultCmd);
		return true;
	}

	private bool NotifyRPGBagCapacityResult(UnPacker unpacker)
	{
		BuyRpgBagCapacityResultCmd buyRpgBagCapacityResultCmd = new BuyRpgBagCapacityResultCmd();
		if (!buyRpgBagCapacityResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		Debug.LogWarning("-------------------------------------NotifyRPGBagCapacityResult!");
		Debug.LogWarning("m_result:" + buyRpgBagCapacityResultCmd.m_result);
		Debug.LogWarning("m_bag_type:" + buyRpgBagCapacityResultCmd.m_bag_type);
		Debug.LogWarning("New m_current_capacity:" + buyRpgBagCapacityResultCmd.m_current_capacity);
		if (buyRpgBagCapacityResultCmd.m_result == 0)
		{
			if (buyRpgBagCapacityResultCmd.m_bag_type == 0)
			{
				UIDataBufferCenter.Instance.RPGData.m_card_capacity = buyRpgBagCapacityResultCmd.m_current_capacity;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_CardCapacityChange, null, buyRpgBagCapacityResultCmd.m_current_capacity);
			}
			else if (buyRpgBagCapacityResultCmd.m_bag_type == 1)
			{
				UIDataBufferCenter.Instance.RPGData.m_equip_capacity = buyRpgBagCapacityResultCmd.m_current_capacity;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_AvatarCapacityChange, null, buyRpgBagCapacityResultCmd.m_current_capacity);
			}
		}
		else if (buyRpgBagCapacityResultCmd.m_result == 1)
		{
			if (buyRpgBagCapacityResultCmd.m_bag_type == 0)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_CardCapactiyChangeError, null, null);
			}
			else if (buyRpgBagCapacityResultCmd.m_bag_type == 1)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_AvatarCapacityChangeError, null, null);
			}
		}
		return true;
	}

	private bool NotifyMapBattleResult(UnPacker unpacker)
	{
		ReportMapBattleResultCmd reportMapBattleResultCmd = new ReportMapBattleResultCmd();
		if (!reportMapBattleResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ReportMapBattleResultCmd!");
		Debug.LogWarning("m_result:" + reportMapBattleResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMapBattleResult, null, reportMapBattleResultCmd);
		return true;
	}

	private bool NotifyMapChanged(UnPacker unpacker)
	{
		NotifyMapChangeCmd notifyMapChangeCmd = new NotifyMapChangeCmd();
		if (!notifyMapChangeCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyMapChangeCmd!");
		Debug.LogWarning("m_count:" + notifyMapChangeCmd.m_count);
		bool flag = false;
		if (UIDataBufferCenter.Instance.RPGData.m_last_refresh_time != notifyMapChangeCmd.m_last_refresh_time)
		{
			flag = true;
		}
		UIDataBufferCenter.Instance.RPGData.m_last_refresh_time = notifyMapChangeCmd.m_last_refresh_time;
		for (int i = 0; i < notifyMapChangeCmd.m_count; i++)
		{
			int index = notifyMapChangeCmd.m_points[i].m_index;
			UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_role_id = notifyMapChangeCmd.m_points[i].m_role_id;
			UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_status = notifyMapChangeCmd.m_points[i].m_status;
			Debug.Log("_______________--------------------point " + i + "  :" + notifyMapChangeCmd.m_points[i].m_start_time);
			UIDataBufferCenter.Instance.RPGData.m_mapPoint[index].m_start_time = notifyMapChangeCmd.m_points[i].m_start_time;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMapDataChanged, null, notifyMapChangeCmd);
		if (flag)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyLastRefreshTimeChanged, null, notifyMapChangeCmd);
		}
		return true;
	}

	private bool NotifyMobilityChanged(UnPacker unpacker)
	{
		NotifyMobilityCmd notifyMobilityCmd = new NotifyMobilityCmd();
		if (!notifyMobilityCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyMobilityCmd!");
		Debug.LogWarning("m_mobility_time:" + notifyMobilityCmd.m_mobility_time);
		UIDataBufferCenter.Instance.RPGData.m_mobility_time = notifyMobilityCmd.m_mobility_time;
		COMA_Server_Account.Instance.svrTime = notifyMobilityCmd.m_srv_time;
		RPGGlobalClock.Instance.InitClock(notifyMobilityCmd.m_srv_time);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMobilityChanged, null, null);
		return true;
	}

	private bool NotifyMedalChanged(UnPacker unpacker)
	{
		NotifyMedalCmd notifyMedalCmd = new NotifyMedalCmd();
		if (!notifyMedalCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyMedalCmd!");
		Debug.LogWarning("m_medal:" + notifyMedalCmd.m_medal);
		UIDataBufferCenter.Instance.RPGData.m_medal = notifyMedalCmd.m_medal;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMedalChanged, null, null);
		return true;
	}

	private bool NotifyExpChanged(UnPacker unpacker)
	{
		NotifyExpCmd notifyExpCmd = new NotifyExpCmd();
		if (!notifyExpCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyExpCmd!");
		Debug.LogWarning("m_rpg_level:" + notifyExpCmd.m_rpg_level);
		Debug.LogWarning("m_next_exp:" + notifyExpCmd.m_next_exp);
		UIDataBufferCenter.Instance.RPGData.m_rpg_level = notifyExpCmd.m_rpg_level;
		UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp = notifyExpCmd.m_next_exp;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyExpChanged, null, null);
		return true;
	}

	private bool NotifyRefreshPlayerLevelResult(UnPacker unpacker)
	{
		ReqRefreshPlayerLevelResultCmd reqRefreshPlayerLevelResultCmd = new ReqRefreshPlayerLevelResultCmd();
		if (!reqRefreshPlayerLevelResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ReqRefreshPlayerLevelResultCmd!");
		Debug.LogWarning("m_result:" + reqRefreshPlayerLevelResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyMapDataResult, null, reqRefreshPlayerLevelResultCmd);
		return true;
	}

	private bool NotifyGainGoldResult(UnPacker unpacker)
	{
		ReqGainGoldResultCmd reqGainGoldResultCmd = new ReqGainGoldResultCmd();
		if (!reqGainGoldResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIAutoDelBlockOnlyMessageBoxMgr.Instance.ReleaseAutoDelBlockOnlyMessageBox();
		Debug.LogWarning("-------------------------------------ReqGainGoldResultCmd!");
		Debug.LogWarning("m_result:" + reqGainGoldResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyGainGoldResult, null, reqGainGoldResultCmd);
		return true;
	}

	private bool NotifyReqBattleResult(UnPacker unpacker)
	{
		ReqBattleResultCmd reqBattleResultCmd = new ReqBattleResultCmd();
		if (!reqBattleResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ReqBattleResultCmd!");
		Debug.LogWarning("m_result:" + reqBattleResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyReqBattleResult, null, reqBattleResultCmd);
		return true;
	}

	private bool NotifyReplacePlayerResult(UnPacker unpacker)
	{
		ChangePlayerLevelResultCmd changePlayerLevelResultCmd = new ChangePlayerLevelResultCmd();
		if (!changePlayerLevelResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ChangePlayerLevelResultCmd!");
		Debug.LogWarning("m_result:" + changePlayerLevelResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyReplacePlayerResult, null, changePlayerLevelResultCmd);
		return true;
	}

	private bool NotifyRPGOtherPlayerDataError(UnPacker unpacker)
	{
		DragPlayerRpgDataErrorCmd dragPlayerRpgDataErrorCmd = new DragPlayerRpgDataErrorCmd();
		if (!dragPlayerRpgDataErrorCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------DragPlayerRpgDataErrorCmd!");
		Debug.LogWarning("m_role_id:" + dragPlayerRpgDataErrorCmd.m_role_id);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyOtherPlayerDataError, null, dragPlayerRpgDataErrorCmd);
		return true;
	}

	private bool NotifyRPGOtherPlayerData(UnPacker unpacker)
	{
		PlayerRpgDataCmd playerRpgDataCmd = new PlayerRpgDataCmd();
		if (!playerRpgDataCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIDataBufferCenter.Instance.RPGData_Enemy = playerRpgDataCmd;
		for (int i = 0; i < 5; i++)
		{
		}
		foreach (ulong key in playerRpgDataCmd.m_equip_bag.Keys)
		{
			Equip equip = playerRpgDataCmd.m_equip_bag[key];
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyOtherPlayerData, null, playerRpgDataCmd);
		return true;
	}

	private bool DragMedalRankResult(UnPacker unpacker)
	{
		DragMedalRankResultCmd dragMedalRankResultCmd = new DragMedalRankResultCmd();
		if (!dragMedalRankResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------DragMedalRankResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DragMedalRankResult, null, dragMedalRankResultCmd);
		return true;
	}

	private bool DragFriendMedalRankResult(UnPacker unpacker)
	{
		GetFriendMedalListResultCmd getFriendMedalListResultCmd = new GetFriendMedalListResultCmd();
		if (!getFriendMedalListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------GetFriendMedalListResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DragFriendsMedalRankResult, null, getFriendMedalListResultCmd);
		return true;
	}

	private bool GetMedalRankPosResult(UnPacker unpacker)
	{
		GetMedalRankPosResultCmd getMedalRankPosResultCmd = new GetMedalRankPosResultCmd();
		if (!getMedalRankPosResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------GetMedalRankPosResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_GetMedalRankPosResult, null, getMedalRankPosResultCmd);
		return true;
	}

	private bool NotifyBuyMobilityResult(UnPacker unpacker)
	{
		BuyMobilityResultCmd buyMobilityResultCmd = new BuyMobilityResultCmd();
		if (!buyMobilityResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------BuyMobilityResultCmd!");
		Debug.LogWarning("m_result:" + buyMobilityResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NofityBuyMobilityResult, null, buyMobilityResultCmd);
		return true;
	}

	private bool NotifyDayFirstLoginAward(UnPacker unpacker)
	{
		UIDataBufferCenter.Instance.RPGFirstLoginAward_PerDay = 0;
		Debug.LogWarning("---------------------------------------------------Get First Login Award!");
		return true;
	}

	private bool BuyCouponsResult(UnPacker unpacker)
	{
		BuyCouponsShopResultCmd buyCouponsShopResultCmd = new BuyCouponsShopResultCmd();
		if (!buyCouponsShopResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------BuyCouponsShopResultCmd!");
		Debug.LogWarning("m_result:" + buyCouponsShopResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyBuyCouponResult, null, buyCouponsShopResultCmd);
		return true;
	}

	private bool DecomposeEquipResult(UnPacker unpacker)
	{
		DecomposeEquipResultCmd decomposeEquipResultCmd = new DecomposeEquipResultCmd();
		if (!decomposeEquipResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------DecomposeEquipResultCmd!");
		Debug.LogWarning("m_result:" + decomposeEquipResultCmd.m_result);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_DecomposeEquipResult, null, decomposeEquipResultCmd);
		return true;
	}

	private bool NotifyRPGAvatarDel(UnPacker unpacker)
	{
		NotifyAvatarDelCmd notifyAvatarDelCmd = new NotifyAvatarDelCmd();
		if (!notifyAvatarDelCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------NotifyAvatarDelCmd!");
		Debug.LogWarning("m_unique_id:" + notifyAvatarDelCmd.m_unique_id);
		if (UIDataBufferCenter.Instance.RPGData.m_equip_bag.ContainsKey(notifyAvatarDelCmd.m_unique_id))
		{
			UIDataBufferCenter.Instance.RPGData.m_equip_bag.Remove(notifyAvatarDelCmd.m_unique_id);
		}
		return true;
	}

	private bool NotifyGameDropResult(UnPacker unpacker)
	{
		RequestGameDropResultCmd requestGameDropResultCmd = new RequestGameDropResultCmd();
		if (!requestGameDropResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------RequestGameDropResultCmd!");
		Debug.LogWarning("m_result:" + requestGameDropResultCmd.m_result);
		Debug.LogWarning("m_gem_id:" + requestGameDropResultCmd.m_gem_id);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyGameDropResult, null, requestGameDropResultCmd);
		return true;
	}

	private bool NotifyDelEquipResult(UnPacker unpacker)
	{
		DeleteEquipResultCmd deleteEquipResultCmd = new DeleteEquipResultCmd();
		if (!deleteEquipResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------DeleteEquipResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyAvatarDelResult, null, deleteEquipResultCmd);
		return true;
	}

	private bool NotifyFirstRPGFinishResult(UnPacker unpacker)
	{
		FirstRpgFinishResultCmd firstRpgFinishResultCmd = new FirstRpgFinishResultCmd();
		if (!firstRpgFinishResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------FirstRpgFinishResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NG_2_1_End, null, firstRpgFinishResultCmd);
		return true;
	}

	private bool NotifyGainAllGoldResult(UnPacker unpacker)
	{
		ReqGainAllGoldResultCmd reqGainAllGoldResultCmd = new ReqGainAllGoldResultCmd();
		if (!reqGainAllGoldResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------ReqGainAllGoldResultCmd!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_NotifyGainAllGoldResult, null, reqGainAllGoldResultCmd);
		return true;
	}

	private bool NotifyReveMobilityError(UnPacker unpacker)
	{
		RecvMobilityErrorCmd recvMobilityErrorCmd = new RecvMobilityErrorCmd();
		if (!recvMobilityErrorCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------RecvMobilityErrorCmd!");
		if (recvMobilityErrorCmd.m_result == 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc7"));
		}
		return true;
	}

	private bool NotifyReveTicketError(UnPacker unpacker)
	{
		RecvTicketErrorCmd recvTicketErrorCmd = new RecvTicketErrorCmd();
		if (!recvTicketErrorCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.LogWarning("-------------------------------------RecvTicketErrorCmd!");
		if (recvTicketErrorCmd.m_result == 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc7"));
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
