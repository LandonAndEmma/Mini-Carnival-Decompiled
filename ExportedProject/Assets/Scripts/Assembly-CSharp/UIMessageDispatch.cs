using System.Collections;
using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIMessageDispatch
{
	private static UIMessageDispatch _instance = null;

	private static readonly object _lock = new object();

	private Dictionary<string, int> _channelMap = new Dictionary<string, int>();

	private Dictionary<EUIMessageID, SortedList> _msgDispathTable = new Dictionary<EUIMessageID, SortedList>();

	public static UIMessageDispatch Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new UIMessageDispatch();
					}
				}
			}
			return _instance;
		}
	}

	private UIMessageDispatch()
	{
		UIChannelSet uIChannelSet = null;
		uIChannelSet = Resources.Load("Data/ChannelSet") as UIChannelSet;
		if (!(uIChannelSet != null))
		{
			return;
		}
		for (int i = 0; i < uIChannelSet._channelSet.Length; i++)
		{
			if (uIChannelSet._channelSet[i] != null && uIChannelSet._channelSet[i] != string.Empty)
			{
				_channelMap.Add(uIChannelSet._channelSet[i], 1 << i);
			}
		}
	}

	public int RegisterMessage(EUIMessageID id, UIEntity entity)
	{
		if (_msgDispathTable.ContainsKey(id))
		{
			SortedList sortedList = _msgDispathTable[id];
			if (sortedList == null)
			{
				sortedList = new SortedList();
			}
			sortedList.Add(entity, null);
		}
		else
		{
			SortedList sortedList2 = new SortedList();
			sortedList2.Add(entity, null);
			_msgDispathTable.Add(id, sortedList2);
		}
		return 0;
	}

	public int UnregisterMessage(EUIMessageID id, UIEntity entity)
	{
		if (_msgDispathTable.ContainsKey(id))
		{
			SortedList sortedList = _msgDispathTable[id];
			if (sortedList != null)
			{
				for (int i = 0; i < sortedList.Count; i++)
				{
					if ((UIEntity)sortedList.GetKey(i) == entity)
					{
						sortedList.RemoveAt(i);
						break;
					}
				}
			}
		}
		return 0;
	}

	public bool SendMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo)
	{
		return SendMessage(msgId, sender, ExtraInfo, null, null);
	}

	public bool SendMessage(EUIMessageID msgId, UIEntity sender, UIAcrossSceneAniController.UIAniLinkEvent ExtraInfo)
	{
		return SendMessage(msgId, sender, null, null, ExtraInfo);
	}

	public bool SendMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo, object ExtraInfo2)
	{
		return SendMessage(msgId, sender, ExtraInfo, ExtraInfo2, null);
	}

	public bool SendMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo, object ExtraInfo2, UIAcrossSceneAniController.UIAniLinkEvent ExtaraEvent)
	{
		bool result = false;
		if (!_msgDispathTable.ContainsKey(msgId))
		{
			return result;
		}
		SortedList sortedList = _msgDispathTable[msgId];
		if (sortedList == null)
		{
			return result;
		}
		TUITelegram msg = new TUITelegram(sender, -1, (int)msgId, TUITelegram.SEND_MSG_IMMEDIATELY, ExtraInfo, ExtraInfo2, ExtaraEvent);
		for (int i = 0; i < sortedList.Count; i++)
		{
			UIEntity uIEntity = (UIEntity)sortedList.GetKey(i);
			if (Discharge(uIEntity, msg))
			{
				result = true;
				if (uIEntity.BlockMsg == UIEntity.EBlockType.BlockAll || (uIEntity.BlockMsg == UIEntity.EBlockType.BlockLowerOnly && i + 1 < sortedList.Count && ((UIEntity)sortedList[i + 1]).Priority < uIEntity.Priority))
				{
					break;
				}
			}
		}
		return result;
	}

	public void PostMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo)
	{
		PostMessage(msgId, sender, ExtraInfo, null, false);
	}

	public void PostMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo, object ExtraInfo2, bool unconditionalAdd)
	{
		if (!_msgDispathTable.ContainsKey(msgId))
		{
			return;
		}
		SortedList sortedList = _msgDispathTable[msgId];
		if (sortedList == null)
		{
			return;
		}
		TUITelegram msg = new TUITelegram(sender, -1, (int)msgId, TUITelegram.SEND_MSG_IMMEDIATELY, ExtraInfo, ExtraInfo2);
		for (int i = 0; i < sortedList.Count; i++)
		{
			UIEntity uIEntity = (UIEntity)sortedList.GetKey(i);
			COMA_Square_Player cOMA_Square_Player = uIEntity as COMA_Square_Player;
			if (unconditionalAdd || (uIEntity.gameObject.activeInHierarchy && uIEntity.enabled))
			{
				uIEntity.AddMsgToQueue(msg);
			}
		}
	}

	public void BroadcastMessage(EUIMessageID msgId, UIEntity sender, object ExtraInfo)
	{
	}

	private bool Discharge(UIEntity receiver, TUITelegram msg)
	{
		return receiver.HandleMessage(msg);
	}
}
