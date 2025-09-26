using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPGPlayerTeam_Container : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UILabel _labelPlayerName;

	protected override void Load()
	{
		base.Load();
		RegisterMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, this, DragOtherPlayerTeamInfo);
	}

	protected override void UnLoad()
	{
		base.UnLoad();
		UnregisterMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, this);
	}

	private string ColorString(string str)
	{
		return "[FFC500]" + str + "[-]";
	}

	private bool DragOtherPlayerTeamInfo(TUITelegram msg)
	{
		ClearContainer();
		uint num = (uint)(ulong)msg._pExtraInfo;
		Debug.Log("PlayerID=" + num);
		UIDataBufferCenter.Instance.FetchPlayerProfile(num, delegate(WatchRoleInfo playerInfo)
		{
			if (playerInfo != null)
			{
				_labelPlayerName.text = ColorString(playerInfo.m_name) + "'s team";
			}
		});
		UIDataBufferCenter.Instance.FetchPlayerRPGData(num, delegate(PlayerRpgDataCmd rpgInfo)
		{
			Debug.Log("FetchPlayerRPGData-----------Return!");
			MemberSlot[] member_slot = rpgInfo.m_member_slot;
			for (int i = 0; i < member_slot.Length; i++)
			{
				if (member_slot[i] != null && member_slot[i].m_member != 0)
				{
					int num2 = AddBox();
					UIRPG_BigCard uIRPG_BigCard = (UIRPG_BigCard)base.LstBoxs[num2];
					UIRPG_BigCardData data = new UIRPG_BigCardData((int)member_slot[i].m_member);
					SetBoxData(num2, data);
				}
			}
			Resources.UnloadUnusedAssets();
		});
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

	protected override void ProcessBoxSelected(NGUI_COMUI.UI_Box box)
	{
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}
}
