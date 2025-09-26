using UnityEngine;

public class GUI_Menu_Scene : TBaseEntity
{
	private void Start()
	{
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput_WASD, this);
		TPCInputMgr.Instance.RegisterPCInput(COMA_MsgSec.PCInput, this);
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (msg._nMsgId == 30001)
		{
			Vector2 vector = (Vector2)msg._pExtraInfo;
			Debug.Log("Move: " + vector.x + " " + vector.y);
		}
		else if (msg._nMsgId == 30000)
		{
			TPCInputEvent tPCInputEvent = (TPCInputEvent)msg._pExtraInfo;
			Debug.Log(string.Concat("Key: ", tPCInputEvent.code, " ", tPCInputEvent.type));
		}
		return true;
	}
}
