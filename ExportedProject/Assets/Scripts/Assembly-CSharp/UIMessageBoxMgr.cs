using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIMessageBoxMgr : UIEntity
{
	public enum EMessageBoxType
	{
		CommonBox = 0,
		Achievement = 1,
		GetItems = 2,
		OnlyBlock = 3,
		IAPBlock = 4,
		OnlyTips = 5,
		BlockForTUI = 6,
		JoinGameBox = 7,
		FaceBook = 8
	}

	public enum ECommonMessageBoxLayout
	{
		YesAndNo = 0,
		OnlyYes = 1,
		RateAndLater = 2,
		None = 3
	}

	public enum EChannelState
	{
		Idle = 0,
		Showing = 1,
		Suspend = 2
	}

	private class UIMessageBoxChannelState
	{
		public EChannelState _state;

		public float _startSuspendTime;

		public UIMessageBoxChannelState()
		{
			_state = EChannelState.Idle;
			_startSuspendTime = 0f;
		}

		public UIMessageBoxChannelState(EChannelState state, float time)
		{
			_state = state;
			_startSuspendTime = time;
		}
	}

	public static UIMessageBoxMgr Instance;

	[SerializeField]
	private GameObject[] _messageBoxPrefab;

	private List<string> _lstCloseMarkedBlockBox = new List<string>();

	private float _fPreSendDelayCloseTime;

	private Dictionary<int, Queue<UIMessage_BoxData>> _dictChannelData = new Dictionary<int, Queue<UIMessage_BoxData>>();

	private Dictionary<int, UIMessageBoxChannelState> _dictChannelState = new Dictionary<int, UIMessageBoxChannelState>();

	public GameObject GetMessageBoxPrefabByType(int type)
	{
		return _messageBoxPrefab[type];
	}

	protected override void Load()
	{
		Instance = this;
		RegisterMessage(EUIMessageID.UI_MessageBoxDestory, this, MessageBoxDestory);
		RegisterMessage(EUIMessageID.UI_MessageBoxDelayClose, this, MessageBoxDelayClose);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_MessageBoxDestory, this);
		UnregisterMessage(EUIMessageID.UI_MessageBoxDelayClose, this);
		Instance = null;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	private void Start()
	{
	}

	private void FixUpdate()
	{
	}

	protected override void Tick()
	{
		Dictionary<int, UIMessageBoxChannelState>.KeyCollection keys = _dictChannelState.Keys;
		foreach (int item in keys)
		{
			if (_dictChannelState[item]._state == EChannelState.Showing)
			{
				continue;
			}
			if (_dictChannelState[item]._state == EChannelState.Suspend)
			{
				if (Time.time - _dictChannelState[item]._startSuspendTime >= 1f)
				{
					Queue<UIMessage_BoxData> queue = _dictChannelData[item];
					if (queue != null && queue.Count > 0 && UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_PopupMessageBox, this, queue.Peek()))
					{
						_dictChannelState[item]._state = EChannelState.Showing;
						queue.Dequeue();
					}
				}
			}
			else
			{
				if (_dictChannelState[item]._state != EChannelState.Idle)
				{
					continue;
				}
				Queue<UIMessage_BoxData> queue2 = _dictChannelData[item];
				if (queue2 != null && queue2.Count > 0)
				{
					if (UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_PopupMessageBox, this, queue2.Peek()))
					{
						_dictChannelState[item]._state = EChannelState.Showing;
						queue2.Dequeue();
					}
					else
					{
						_dictChannelState[item]._state = EChannelState.Suspend;
						_dictChannelState[item]._startSuspendTime = Time.time;
					}
				}
			}
		}
		if (_lstCloseMarkedBlockBox.Count > 0 && Time.time - _fPreSendDelayCloseTime >= 0.1f)
		{
			_fPreSendDelayCloseTime = Time.time;
			if (UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_CloseBlockBox, null, _lstCloseMarkedBlockBox[0]))
			{
				_lstCloseMarkedBlockBox.RemoveAt(0);
			}
		}
	}

	public int UIMessageBox(UIMessage_BoxData data)
	{
		Queue<UIMessage_BoxData> queue = null;
		if (_dictChannelData.ContainsKey(data.Channel))
		{
			queue = _dictChannelData[data.Channel];
		}
		else
		{
			queue = new Queue<UIMessage_BoxData>();
			_dictChannelData.Add(data.Channel, queue);
		}
		if ((data.Type == EMessageBoxType.BlockForTUI || data.Type == EMessageBoxType.OnlyBlock) && queue.Count > 0)
		{
			Debug.Log("Ignore Block!!!!!!!!");
			return 0;
		}
		Debug.Log("Add box:" + data.Channel);
		queue.Enqueue(data);
		if (!_dictChannelState.ContainsKey(data.Channel))
		{
			_dictChannelState.Add(data.Channel, new UIMessageBoxChannelState());
		}
		return 0;
	}

	private bool MessageBoxDestory(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		if (_dictChannelState[num]._state != EChannelState.Showing)
		{
			Debug.LogWarning("msg Channel = " + num + " Warning!!");
		}
		_dictChannelState[num]._state = EChannelState.Idle;
		return true;
	}

	private bool MessageBoxDelayClose(TUITelegram msg)
	{
		_lstCloseMarkedBlockBox.Add((string)msg._pExtraInfo);
		return true;
	}
}
