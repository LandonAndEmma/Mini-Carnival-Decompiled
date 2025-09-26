using MC_UIToolKit;
using MessageID;
using Protocol.Shop.C2S;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UIMarket_Container : UI_Container
	{
		protected override void Load()
		{
			base.Load();
			RegisterMessage(EUIMessageID.UIMarket_BtnPraiseAvatarClick, this, BtnPraiseAvatarClick);
			RegisterMessage(EUIMessageID.UIMarket_CollectCurAvatar, this, CollectCurAvatar);
			RegisterMessage(EUIMessageID.UIMarket_UncollectCurAvatar, this, UncollectCurAvatar);
			RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_CollectLstChanged, this, CollectLstChanged);
			RegisterMessage(EUIMessageID.UIMarket_FollowPlayer, this, FollowPlayer);
			RegisterMessage(EUIMessageID.UIMarket_UnfollowPlayer, this, UnfollowPlayer);
			RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_FollowLstChanged, this, FollowLstChanged);
		}

		protected override void UnLoad()
		{
			base.UnLoad();
			UnregisterMessage(EUIMessageID.UIMarket_BtnPraiseAvatarClick, this);
			UnregisterMessage(EUIMessageID.UIMarket_CollectCurAvatar, this);
			UnregisterMessage(EUIMessageID.UIMarket_UncollectCurAvatar, this);
			UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_CollectLstChanged, this);
			UnregisterMessage(EUIMessageID.UIMarket_FollowPlayer, this);
			UnregisterMessage(EUIMessageID.UIMarket_UnfollowPlayer, this);
			UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_FollowLstChanged, this);
		}

		private bool BtnPraiseAvatarClick(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_curSelBox.BoxData;
				PraiseAvatarCmd praiseAvatarCmd = new PraiseAvatarCmd();
				praiseAvatarCmd.m_id = (uint)uIMarket_BoxData.ItemId;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, praiseAvatarCmd);
				UIGolbalStaticFun.PraiseAvatar(praiseAvatarCmd.m_id);
				uIMarket_BoxData.PraiseNum++;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, uIMarket_BoxData);
				Debug.Log("Praise Avatar:" + praiseAvatarCmd.m_id);
			}
			return true;
		}

		private bool CollectCurAvatar(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_curSelBox.BoxData;
				CollectAvatarCmd collectAvatarCmd = new CollectAvatarCmd();
				collectAvatarCmd.m_id = (uint)uIMarket_BoxData.ItemId;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, collectAvatarCmd);
				Debug.Log("CollectCurAvatar=" + collectAvatarCmd.m_id);
			}
			return true;
		}

		private bool UncollectCurAvatar(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_curSelBox.BoxData;
				UncollectAvatarCmd uncollectAvatarCmd = new UncollectAvatarCmd();
				uncollectAvatarCmd.m_id = (uint)uIMarket_BoxData.ItemId;
				uncollectAvatarCmd.m_param = 1;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, uncollectAvatarCmd);
				Debug.Log("UncollectCurAvatar=" + uncollectAvatarCmd.m_id);
			}
			return true;
		}

		private bool CollectLstChanged(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, _curSelBox.BoxData);
			}
			return true;
		}

		private bool FollowLstChanged(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, _curSelBox.BoxData);
			}
			return true;
		}

		private bool FollowPlayer(TUITelegram msg)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Follow);
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_curSelBox.BoxData;
				FollowRoleShopCmd followRoleShopCmd = new FollowRoleShopCmd();
				followRoleShopCmd.m_follow_id = uIMarket_BoxData.AuthorId;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, followRoleShopCmd);
				Debug.Log("FollowPlayer=" + followRoleShopCmd.m_follow_id);
			}
			else if (msg._pExtraInfo != null)
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				FollowRoleShopCmd followRoleShopCmd2 = new FollowRoleShopCmd();
				followRoleShopCmd2.m_follow_id = (uint)msg._pExtraInfo;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, followRoleShopCmd2);
				Debug.Log("FollowPlayer=" + followRoleShopCmd2.m_follow_id);
			}
			return true;
		}

		private bool UnfollowPlayer(TUITelegram msg)
		{
			if (_curSelBox != null && _curSelBox.BoxData != null)
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)_curSelBox.BoxData;
				UnfollowRoleShopCmd unfollowRoleShopCmd = new UnfollowRoleShopCmd();
				unfollowRoleShopCmd.m_follow_id = uIMarket_BoxData.AuthorId;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, unfollowRoleShopCmd);
				Debug.Log("UncollectCurAvatar=" + unfollowRoleShopCmd.m_follow_id);
			}
			else if (msg._pExtraInfo != null)
			{
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				UnfollowRoleShopCmd unfollowRoleShopCmd2 = new UnfollowRoleShopCmd();
				unfollowRoleShopCmd2.m_follow_id = (uint)msg._pExtraInfo;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, unfollowRoleShopCmd2);
				Debug.Log("UncollectCurAvatar=" + unfollowRoleShopCmd2.m_follow_id);
			}
			if (msg._pExtraInfo != null)
			{
			}
			return true;
		}

		private void Awake()
		{
		}

		protected override void Tick()
		{
		}

		protected override bool IsCanSelBox(UI_Box box, out UI_Box loseSel)
		{
			if (base.BoxSelType == EBoxSelType.Single)
			{
				if (box.BoxData != null)
				{
					if (box.BoxData.DataType != 0 && box != _curSelBox)
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

		protected override void ProcessBoxSelected(UI_Box box)
		{
			base.ProcessBoxSelected(box);
			UIMarket_BoxData uIMarket_BoxData = box.BoxData as UIMarket_BoxData;
			if (uIMarket_BoxData == null)
			{
				Debug.LogWarning("boxData is NULL!");
				return;
			}
			Debug.Log("Cur sEL bOX id:" + uIMarket_BoxData.ItemId);
			if (uIMarket_BoxData.DataType != 1 && uIMarket_BoxData.DataType != 2)
			{
				if (uIMarket_BoxData.DataType == 3)
				{
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_PreviewPersonalFavorites, this, uIMarket_BoxData);
				}
				if (uIMarket_BoxData.DataType != 4)
				{
				}
			}
			else
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_SelNewShoppingItem, this, uIMarket_BoxData);
			}
		}

		protected override void ProcessBoxLoseSelected(UI_Box box)
		{
			base.ProcessBoxLoseSelected(box);
		}

		protected override void ProcessBoxCanntSelected(UI_Box box)
		{
			if (box != null && box.BoxData.DataType == 0)
			{
				Debug.Log("Refresh Btn OnClick;!!");
			}
		}
	}
}
