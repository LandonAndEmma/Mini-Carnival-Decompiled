using System;
using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIEntity : MonoBehaviour, IComparable
{
	[SerializeField]
	public enum EBlockType
	{
		NotBlock = 0,
		BlockAll = 1,
		BlockLowerOnly = 2
	}

	public delegate bool ProcessMessage(TUITelegram msg);

	[HideInInspector]
	[SerializeField]
	private int _nPriority;

	[SerializeField]
	[HideInInspector]
	private EBlockType _eBlockMsg;

	[SerializeField]
	[HideInInspector]
	private int _nChannel;

	private Queue<TUITelegram> _msgQueue = new Queue<TUITelegram>();

	private Dictionary<EUIMessageID, ProcessMessage> _msgProcessTable = new Dictionary<EUIMessageID, ProcessMessage>();

	public int Priority
	{
		get
		{
			return _nPriority;
		}
		set
		{
			_nPriority = value;
		}
	}

	public EBlockType BlockMsg
	{
		get
		{
			return _eBlockMsg;
		}
		set
		{
			_eBlockMsg = value;
		}
	}

	public int Channel
	{
		get
		{
			return _nChannel;
		}
		set
		{
			_nChannel = value;
		}
	}

	public int CompareTo(object obj)
	{
		UIEntity uIEntity = obj as UIEntity;
		if (uIEntity == null)
		{
			Debug.LogError("NULL UIEntiry");
		}
		if (uIEntity.GetInstanceID() == GetInstanceID())
		{
			return 0;
		}
		return (Priority - uIEntity.Priority < 0) ? 1 : (-1);
	}

	public int AddMsgToQueue(TUITelegram msg)
	{
		_msgQueue.Enqueue(msg);
		return 0;
	}

	public bool HandleMessage(TUITelegram msg)
	{
		if (_msgProcessTable.ContainsKey((EUIMessageID)msg._nMsgId))
		{
			return _msgProcessTable[(EUIMessageID)msg._nMsgId](msg);
		}
		return false;
	}

	protected int RegisterMessage(EUIMessageID id, UIEntity entity, ProcessMessage proc)
	{
		OnMessage(id, proc);
		return UIMessageDispatch.Instance.RegisterMessage(id, entity);
	}

	protected int UnregisterMessage(EUIMessageID id, UIEntity entity)
	{
		return UIMessageDispatch.Instance.UnregisterMessage(id, entity);
	}

	protected virtual void Load()
	{
	}

	protected virtual void UnLoad()
	{
	}

	protected virtual void Tick()
	{
	}

	protected void OnEnable()
	{
		Load();
	}

	protected void OnDisable()
	{
		UnLoad();
	}

	protected void OnDestroy()
	{
		if (base.gameObject.activeInHierarchy)
		{
			UnLoad();
		}
		_msgProcessTable.Clear();
	}

	private void OnMessage(EUIMessageID id, ProcessMessage proc)
	{
		if (_msgProcessTable.ContainsKey(id))
		{
			_msgProcessTable[id] = proc;
		}
		else
		{
			_msgProcessTable.Add(id, proc);
		}
	}

	private void Update()
	{
		if (_msgQueue.Count > 0)
		{
			TUITelegram msg = _msgQueue.Dequeue();
			HandleMessage(msg);
		}
		Tick();
	}
}
