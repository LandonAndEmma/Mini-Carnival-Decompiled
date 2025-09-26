using System;
using System.Collections.Generic;

public class TPCInputMgr
{
	public enum TPCInputLevel
	{
		None = 0,
		Lv1 = 1,
		Lv2 = 2,
		Lv3 = 3,
		Lv4 = 4,
		Lv5 = 5,
		Lv6 = 6,
		Lv7 = 7,
		Lv8 = 8
	}

	private class EntityInfo : IComparable
	{
		public int objID = -1;

		public TPCInputLevel lv;

		public bool bCover;

		public int CompareTo(object obj)
		{
			EntityInfo entityInfo = obj as EntityInfo;
			if (lv == entityInfo.lv)
			{
				return 0;
			}
			return (lv <= entityInfo.lv) ? 1 : (-1);
		}
	}

	private static TPCInputMgr _instance = null;

	private static readonly object _lock = new object();

	private bool _bClosed;

	private Dictionary<COMA_MsgSec, List<EntityInfo>> dict = new Dictionary<COMA_MsgSec, List<EntityInfo>>();

	public static TPCInputMgr Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new TPCInputMgr();
					}
				}
			}
			return _instance;
		}
	}

	public bool Closed
	{
		get
		{
			return _bClosed;
		}
		set
		{
			_bClosed = value;
		}
	}

	private TPCInputMgr()
	{
	}

	public void ResetInstance()
	{
		_instance = null;
	}

	public void RegisterPCInput(COMA_MsgSec msgSec, TBaseEntity NewEntity)
	{
		RegisterPCInput(msgSec, NewEntity, TPCInputLevel.Lv1, false);
	}

	public void RegisterPCInput(COMA_MsgSec msgSec, TBaseEntity NewEntity, TPCInputLevel lv, bool bCover)
	{
		if (!dict.ContainsKey(msgSec))
		{
			List<EntityInfo> value = new List<EntityInfo>();
			dict.Add(msgSec, value);
		}
		EntityInfo entityInfo = new EntityInfo();
		entityInfo.objID = NewEntity.GetInstanceID();
		entityInfo.lv = lv;
		entityInfo.bCover = bCover;
		dict[msgSec].Add(entityInfo);
		dict[msgSec].Sort();
	}

	public void UnregisterPCInput(COMA_MsgSec msgSec, TBaseEntity NewEntity)
	{
		if (!dict.ContainsKey(msgSec))
		{
			return;
		}
		int instanceID = NewEntity.GetInstanceID();
		for (int num = dict[msgSec].Count - 1; num >= 0; num--)
		{
			if (dict[msgSec][num].objID == instanceID)
			{
				dict[msgSec].RemoveAt(num);
			}
		}
	}

	public void Execute(COMA_MsgSec msg, object param)
	{
		if (!dict.ContainsKey(msg))
		{
			return;
		}
		TPCInputLevel tPCInputLevel = TPCInputLevel.None;
		for (int i = 0; i < dict[msg].Count; i++)
		{
			if (dict[msg][i].lv >= tPCInputLevel)
			{
				TMessageDispatcher.Instance.DispatchMsg(-1, dict[msg][i].objID, (int)msg, TTelegram.SEND_MSG_IMMEDIATELY, param);
				if (dict[msg][i].bCover)
				{
					tPCInputLevel = dict[msg][i].lv;
				}
			}
		}
	}
}
