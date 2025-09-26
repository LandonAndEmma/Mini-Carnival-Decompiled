using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class COMA_CommandHandler
{
	public delegate void HandlerFunc(COMA_CommandDatas commandDatas);

	private static COMA_CommandHandler _instance;

	private Dictionary<COMA_Command, List<HandlerFunc>> dicCommandHandlerFunctions = new Dictionary<COMA_Command, List<HandlerFunc>>();

	public static COMA_CommandHandler Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_CommandHandler();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void Init()
	{
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.OBJECT_MESSAGE, Receive);
		}
	}

	public void AddHandlerFunction(COMA_Command command, HandlerFunc func)
	{
		if (!dicCommandHandlerFunctions.ContainsKey(command))
		{
			dicCommandHandlerFunctions.Add(command, new List<HandlerFunc>());
		}
		List<HandlerFunc> list = dicCommandHandlerFunctions[command];
		if (!list.Contains(func))
		{
			list.Add(func);
		}
	}

	public void RemoveHandlerFunction(COMA_Command command, HandlerFunc func)
	{
		List<HandlerFunc> value;
		if (dicCommandHandlerFunctions.TryGetValue(command, out value))
		{
			value.Remove(func);
		}
	}

	public void Send(COMA_CommandDatas commandDatas)
	{
		SFSObject obj = new SFSObject();
		commandDatas.Package(ref obj);
		COMA_Network.Instance.TNetInstance.Send(new BroadcastMessageRequest(obj));
	}

	public void Send(COMA_CommandDatas commandDatas, int userID)
	{
		SFSObject obj = new SFSObject();
		commandDatas.Package(ref obj);
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Send(new ObjectMessageRequest(userID, obj));
		}
	}

	private void Receive(TNetEventData tEvent)
	{
		TNetUser tNetUser = (TNetUser)tEvent.data["user"];
		if (tNetUser == null)
		{
			return;
		}
		SFSObject sFSObject = tEvent.data["message"] as SFSObject;
		if (sFSObject == null)
		{
			return;
		}
		COMA_Command cOMA_Command = (COMA_Command)sFSObject.GetShort("c");
		COMA_CommandDatas cOMA_CommandDatas = COMA_CommandDatasFactory.CreateCommandDatas(cOMA_Command);
		cOMA_CommandDatas.dataSender = tNetUser;
		cOMA_CommandDatas.Unpackage(sFSObject);
		if (tNetUser.IsItMe && cOMA_Command != COMA_Command.PLAYER_CREATE)
		{
			return;
		}
		if (cOMA_Command != COMA_Command.PLAYER_TRANSFORM && cOMA_Command != COMA_Command.PLAYER_ANIMATION && cOMA_Command != COMA_Command.PLAYER_HPSET && cOMA_Command != COMA_Command.ENEMY_TRANSFORM && cOMA_Command != COMA_Command.ENEMY_ANIMATION && cOMA_Command != COMA_Command.PLAYER_HIT && cOMA_Command != COMA_Command.GAME_TIME)
		{
			Debug.Log(string.Concat("[==sendUser==] [Id : ", tNetUser.Id, "] [Name : ", tNetUser.Name, "] [IsItMe   : ", tNetUser.IsItMe, "] [SetIndex : ", tNetUser.SitIndex, "] [command : ", cOMA_Command, "]"));
		}
		if (!dicCommandHandlerFunctions.ContainsKey(cOMA_Command))
		{
			return;
		}
		List<HandlerFunc> list = dicCommandHandlerFunctions[cOMA_Command];
		if (list != null || list.Count > 0)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num](cOMA_CommandDatas);
			}
		}
	}

	private void Destroy()
	{
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.OBJECT_MESSAGE, Receive);
		}
		_instance = null;
	}
}
