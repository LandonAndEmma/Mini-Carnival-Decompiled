using System.Collections.Generic;
using UnityEngine;

public class TBaseEntity : MonoBehaviour
{
	public static readonly int MAX_SYSTEM_NUM = int.MaxValue;

	[SerializeField]
	private int _nType = -1;

	[SerializeField]
	private int _nCustomID = -1;

	[SerializeField]
	private bool _bTag;

	protected TResData _resData;

	private Dictionary<int, TBaseEntitySystemController> _mapSystem = new Dictionary<int, TBaseEntitySystemController>();

	[SerializeField]
	protected TBaseEntityCenterController _centerController;

	protected void OnEnable()
	{
		TEntityMgr.Instance.RegisterEntity(this);
	}

	protected void OnDisable()
	{
		TEntityMgr.Instance.UnRegisterEntity(this);
	}

	protected void Update()
	{
		if (_centerController != null)
		{
			_centerController.Tick();
		}
	}

	public virtual bool HandleMessage(TTelegram msg)
	{
		if (_centerController != null)
		{
			_centerController.HandleMessage(msg);
		}
		return true;
	}

	public void SetEntityType(int type)
	{
		_nType = type;
	}

	public int GetEntityType()
	{
		return _nType;
	}

	public void SetCustomID(int id)
	{
		_nCustomID = id;
	}

	public int GetCustomID()
	{
		return _nCustomID;
	}

	public void SetCenterController(TBaseEntityCenterController ctr)
	{
		_centerController = ctr;
	}

	public TBaseEntityCenterController GetCenterController()
	{
		return _centerController;
	}

	public int RegisterSystemCtrl(TBaseEntitySystemController systemCtrl)
	{
		if (_mapSystem.Count >= MAX_SYSTEM_NUM)
		{
			Debug.LogWarning("SystemController Num Overflow!");
			return -1;
		}
		if (_mapSystem.ContainsKey(systemCtrl.GetSysType()))
		{
			Debug.LogWarning("Exist same SystemController type:" + systemCtrl.GetType());
			return -2;
		}
		_mapSystem.Add(systemCtrl.GetSysType(), systemCtrl);
		return systemCtrl.GetSysType();
	}

	public int UnregisterSystemCtrl(int type)
	{
		if (_mapSystem.ContainsKey(type))
		{
			_mapSystem.Remove(type);
			return 0;
		}
		Debug.LogWarning("Unexist SystemController type:" + type + " in Map!");
		return -1;
	}

	public TBaseEntitySystemController GetSystemCtrlByType(int type)
	{
		if (_mapSystem.ContainsKey(type))
		{
			return _mapSystem[type];
		}
		return null;
	}

	public virtual int TransmitMsgToSystemCtrl(TTelegram msg)
	{
		foreach (KeyValuePair<int, TBaseEntitySystemController> item in _mapSystem)
		{
			_mapSystem[item.Key].HandleMessage(msg);
		}
		return -1;
	}

	public virtual void DumRes()
	{
	}
}
