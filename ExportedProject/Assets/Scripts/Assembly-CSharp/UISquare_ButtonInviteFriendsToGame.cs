using MC_UIToolKit;
using MessageID;
using Protocol.Role.C2S;
using UnityEngine;

public class UISquare_ButtonInviteFriendsToGame : MonoBehaviour
{
	public GameObject tuiBlock;

	public GameObject _objOwner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_objOwner != null)
		{
			_objOwner.SetActive(false);
		}
		else
		{
			base.transform.parent.gameObject.SetActive(false);
		}
		if (tuiBlock != null)
		{
			tuiBlock.SetActive(false);
		}
		int i = 0;
		for (int count = COMA_WaitingRoom_SceneController.Instance.friendListToInvite.Count; i < count; i++)
		{
			Debug.Log("Invite Friend : " + COMA_WaitingRoom_SceneController.Instance.friendListToInvite[i]);
			InviteRoleCmd inviteRoleCmd = new InviteRoleCmd();
			inviteRoleCmd.m_who = COMA_WaitingRoom_SceneController.Instance.friendListToInvite[i];
			inviteRoleCmd.m_param = COMA_Network.Instance.TNetInstance.CurRoom.Id + "|" + COMA_NetworkConnect.sceneId;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, inviteRoleCmd);
		}
		COMA_WaitingRoom_SceneController.Instance.friendListToInvite.Clear();
		if (i > 0)
		{
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("newroom_desc1"));
		}
	}
}
