using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UISquareChatTipsMgr : UIEntity
{
	[SerializeField]
	private UILabel _labelTips;

	private Queue<UISquare_ChatRecordBoxData> _lstChatRecord = new Queue<UISquare_ChatRecordBoxData>();

	private float _preShowTime = -100f;

	[SerializeField]
	private Animation _aniLayer;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this, ChatHistoryChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UISquare_ChatHistoryChanged, this);
	}

	protected override void Tick()
	{
		if (_lstChatRecord.Count > 0 && Time.time - _preShowTime >= 1.5f)
		{
			_preShowTime = Time.time;
			UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = _lstChatRecord.Dequeue();
			_labelTips.text = FromatTips(uISquare_ChatRecordBoxData);
			if (UIDataBufferCenter.Instance.IsPlayerIDInBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID))
			{
				_labelTips.text = string.Empty;
			}
			_aniLayer.Play();
		}
	}

	private bool ChatHistoryChanged(TUITelegram msg)
	{
		List<UISquare_ChatRecordBoxData> list = (List<UISquare_ChatRecordBoxData>)msg._pExtraInfo;
		if (list.Count > 0)
		{
			_lstChatRecord.Enqueue(list[list.Count - 1]);
		}
		return true;
	}

	private string FromatTips(UISquare_ChatRecordBoxData data)
	{
		string empty = string.Empty;
		empty = empty + "[99FF66]" + data.OtherPeopleName + ":\n";
		string text = data.OtherPeopleChatContent;
		if (text.Length > 15)
		{
			text = text.Substring(0, 15) + "...";
		}
		return empty + "[FFFFFF]" + text;
	}
}
