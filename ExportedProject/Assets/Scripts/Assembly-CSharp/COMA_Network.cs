using TNetSdk;
using UnityEngine;

public class COMA_Network
{
	private static COMA_Network _instance;

	private TNetObject _tNetObject;

	public static COMA_Network Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Network();
			}
			return _instance;
		}
	}

	public TNetObject TNetInstance
	{
		get
		{
			if (_tNetObject == null)
			{
				_tNetObject = new TNetObject();
				COMA_CommandHandler.Instance.Init();
			}
			return _tNetObject;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void StartGame()
	{
		if (_tNetObject != null)
		{
			Debug.Log("StartGame");
			_tNetObject.Send(new RoomStartRequest());
		}
	}

	public void ReadyGame()
	{
		if (_tNetObject != null)
		{
			Debug.Log("ReadyGame");
			_tNetObject.Send(new RoomReadyRequest());
		}
	}

	public void LockValue(string val)
	{
		if (_tNetObject != null)
		{
			Debug.Log("LockValue:" + val);
			_tNetObject.Send(new LockSthRequest(val));
		}
	}

	public void UnlockValue(string val)
	{
		if (_tNetObject != null)
		{
			Debug.Log("UnlockValue:" + val);
			_tNetObject.Send(new UnLockSthRequest(val));
		}
	}

	public bool IsConnected()
	{
		if (_tNetObject == null)
		{
			return false;
		}
		return _tNetObject.IsContected();
	}

	public bool IsRoomMaster(int id)
	{
		if (_tNetObject == null)
		{
			return false;
		}
		if (_tNetObject.CurRoom == null)
		{
			return false;
		}
		if (_tNetObject.CurRoom.RoomMasterID == id)
		{
			return true;
		}
		return false;
	}

	public bool IsMyself(int id)
	{
		if (_tNetObject == null)
		{
			return false;
		}
		if (_tNetObject.Myself == null)
		{
			return false;
		}
		if (_tNetObject.Myself.Id == id)
		{
			return true;
		}
		return false;
	}

	public void Disconnect()
	{
		if (_tNetObject != null)
		{
			_tNetObject.Close();
			_tNetObject.RemoveAllEventListeners();
		}
		_tNetObject = null;
	}
}
