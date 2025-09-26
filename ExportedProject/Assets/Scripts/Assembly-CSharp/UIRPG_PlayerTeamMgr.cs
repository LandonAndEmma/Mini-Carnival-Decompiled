using MessageID;
using UnityEngine;

public class UIRPG_PlayerTeamMgr : UIEntity
{
	[SerializeField]
	private GameObject _show;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, this, DragTeamInfo);
		RegisterMessage(EUIMessageID.UIRPG_HidePlayerTeamInfo, this, HidePlayerTeamInfo);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_DragOtherPlayerTeamInfo, this);
		UnregisterMessage(EUIMessageID.UIRPG_HidePlayerTeamInfo, this);
	}

	private bool DragTeamInfo(TUITelegram msg)
	{
		_show.SetActive(true);
		return true;
	}

	private bool HidePlayerTeamInfo(TUITelegram msg)
	{
		_show.SetActive(false);
		return true;
	}

	private void Awake()
	{
		_show.SetActive(false);
	}

	protected override void Tick()
	{
	}
}
