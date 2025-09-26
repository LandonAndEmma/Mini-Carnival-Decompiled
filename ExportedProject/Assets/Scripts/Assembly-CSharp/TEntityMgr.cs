using System.Collections.Generic;

public class TEntityMgr
{
	private static TEntityMgr _instance = null;

	private static readonly object _lock = new object();

	private Dictionary<int, TBaseEntity> _EntiryrMap = new Dictionary<int, TBaseEntity>();

	public static TEntityMgr Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new TEntityMgr();
					}
				}
			}
			return _instance;
		}
	}

	private TEntityMgr()
	{
	}

	public void ResetInstance()
	{
		_instance = null;
	}

	public void RegisterEntity(TBaseEntity NewEntity)
	{
		_EntiryrMap.Add(NewEntity.GetInstanceID(), NewEntity);
	}

	public void UnRegisterEntity(TBaseEntity Entity)
	{
		_EntiryrMap.Remove(Entity.GetInstanceID());
	}

	public TBaseEntity GetEntityFromID(int id)
	{
		if (_EntiryrMap.ContainsKey(id))
		{
			return _EntiryrMap[id];
		}
		return null;
	}
}
