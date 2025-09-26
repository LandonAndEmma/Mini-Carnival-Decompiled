using System.Collections;
using UnityEngine;

public class TMessageDispatcher : MonoBehaviour
{
	private static TMessageDispatcher _instance;

	private SortedList _msgPriorityQ = new SortedList();

	public static TMessageDispatcher Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
		DispatchDelayedMessages();
	}

	public void DispatchMsg(int sender, int receiver, int msg, float delay, object ExtraInfo, object ExtraInfo2)
	{
		TBaseEntity entityFromID = TEntityMgr.Instance.GetEntityFromID(receiver);
		if (entityFromID == null)
		{
			Debug.LogWarning("Id:" + receiver + " Cann't find in TEntityMgr!No registration or has been removed!");
			return;
		}
		TTelegram tTelegram = new TTelegram(sender, receiver, msg, delay, ExtraInfo, ExtraInfo2);
		if (delay <= 0f)
		{
			Discharge(entityFromID, tTelegram);
			return;
		}
		tTelegram._dDispathTime = delay + Time.time;
		if (!_msgPriorityQ.ContainsKey(tTelegram))
		{
			_msgPriorityQ.Add(tTelegram, null);
		}
		else
		{
			Debug.Log("same msg:" + tTelegram._nMsgId);
		}
	}

	public void DispatchMsg(int sender, int receiver, int msg, float delay, object ExtraInfo)
	{
		DispatchMsg(sender, receiver, msg, delay, ExtraInfo, null);
	}

	private void DispatchDelayedMessages()
	{
		float time = Time.time;
		if (_msgPriorityQ.Count != 0 && ((TTelegram)_msgPriorityQ.GetKey(0))._dDispathTime < time && ((TTelegram)_msgPriorityQ.GetKey(0))._dDispathTime > 0f)
		{
			TTelegram tTelegram = (TTelegram)_msgPriorityQ.GetKey(0);
			TBaseEntity entityFromID = TEntityMgr.Instance.GetEntityFromID(tTelegram._nReceive);
			Discharge(entityFromID, tTelegram);
			_msgPriorityQ.RemoveAt(0);
		}
	}

	private void Discharge(TBaseEntity receiver, TTelegram msg)
	{
		if (!receiver.HandleMessage(msg))
		{
			Debug.LogWarning(receiver.GetInstanceID() + ":" + receiver.name + " HandleMessage:" + msg._nMsgId + " Failure!");
		}
	}
}
