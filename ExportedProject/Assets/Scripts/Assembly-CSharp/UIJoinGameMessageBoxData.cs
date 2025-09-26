public class UIJoinGameMessageBoxData : UIMessage_BoxData
{
	private string _hostName;

	private int _sceneID;

	private int _roomID;

	public string HostName
	{
		get
		{
			return _hostName;
		}
		set
		{
			_hostName = value;
		}
	}

	public int SceneID
	{
		get
		{
			return _sceneID;
		}
		set
		{
			_sceneID = value;
		}
	}

	public int RoomID
	{
		get
		{
			return _roomID;
		}
		set
		{
			_roomID = value;
		}
	}

	public UIJoinGameMessageBoxData(string hostName, int sceneID, int roomId)
	{
		_hostName = hostName;
		_sceneID = sceneID;
		_roomID = roomId;
		_type = UIMessageBoxMgr.EMessageBoxType.JoinGameBox;
		_layout = 0;
		_channel = (int)_type;
	}
}
