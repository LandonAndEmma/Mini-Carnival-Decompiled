using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Binary;
using Protocol.Mail.S2C;

public class UILobby_MailProtocolProcessor : UILobbyMessageHandler
{
	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(4, this);
			OnMessage(1, OnMailListResult);
			OnMessage(7, OnReadMailResult);
			OnMessage(9, OnGainMailResult);
			OnMessage(3, OnDelMailResult);
		}
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(4, this);
		}
	}

	private bool OnMailListResult(UnPacker unpacker)
	{
		DragMailListResultCmd dragMailListResultCmd = new DragMailListResultCmd();
		if (!dragMailListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		uint num_of_mail = dragMailListResultCmd.m_num_of_mail;
		uint num_of_new = dragMailListResultCmd.m_num_of_new;
		List<Email> mail_list = dragMailListResultCmd.m_mail_list;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemoteMailInfoArrived, this, dragMailListResultCmd);
		return true;
	}

	private bool OnReadMailResult(UnPacker unpacker)
	{
		ReadMailResultCmd readMailResultCmd = new ReadMailResultCmd();
		if (!readMailResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (readMailResultCmd.m_result == 0)
		{
			DragMailListResultCmd mailBufferInfo = UIDataBufferCenter.Instance.MailBufferInfo;
			for (int i = 0; i < mailBufferInfo.m_mail_list.Count; i++)
			{
				if (mailBufferInfo.m_mail_list[i].m_id == readMailResultCmd.m_mail_id)
				{
					if (mailBufferInfo.m_mail_list[i].m_status == 0)
					{
						mailBufferInfo.m_mail_list[i].m_status = 1;
						mailBufferInfo.m_num_of_new--;
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, mailBufferInfo);
					}
					break;
				}
			}
		}
		return true;
	}

	private bool OnGainMailResult(UnPacker unpacker)
	{
		GainMailResultCmd gainMailResultCmd = new GainMailResultCmd();
		if (!gainMailResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (gainMailResultCmd.m_result == 0)
		{
			DragMailListResultCmd mailBufferInfo = UIDataBufferCenter.Instance.MailBufferInfo;
			for (int i = 0; i < mailBufferInfo.m_mail_list.Count; i++)
			{
				if (mailBufferInfo.m_mail_list[i].m_id == gainMailResultCmd.m_mail_id)
				{
					mailBufferInfo.m_mail_list[i].m_status = 2;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, mailBufferInfo);
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, gainMailResultCmd.m_mail_id);
					break;
				}
			}
		}
		else if (gainMailResultCmd.m_result == 2)
		{
			string str = Localization.instance.Get("youxiang_desc4");
			UIGolbalStaticFun.PopupTipsBox(str);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, gainMailResultCmd.m_mail_id);
		}
		else if (gainMailResultCmd.m_result != 1)
		{
		}
		return true;
	}

	private bool OnDelMailResult(UnPacker unpacker)
	{
		DelMailResultCmd delMailResultCmd = new DelMailResultCmd();
		if (!delMailResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIAutoDelBlockOnlyMessageBoxMgr.Instance.ReleaseAutoDelBlockOnlyMessageBox();
		if (delMailResultCmd.m_result == 0)
		{
			DragMailListResultCmd mailBufferInfo = UIDataBufferCenter.Instance.MailBufferInfo;
			for (int i = 0; i < mailBufferInfo.m_mail_list.Count; i++)
			{
				if (mailBufferInfo.m_mail_list[i].m_id == delMailResultCmd.m_mail_id)
				{
					mailBufferInfo.m_mail_list.RemoveAt(i);
					mailBufferInfo.m_num_of_mail--;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, mailBufferInfo);
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_DelMailResult, this, delMailResultCmd.m_mail_id);
					break;
				}
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
}
