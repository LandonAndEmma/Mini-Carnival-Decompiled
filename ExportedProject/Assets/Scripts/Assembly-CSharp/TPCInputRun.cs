using UnityEngine;

public class TPCInputRun : MonoBehaviour
{
	private void Update()
	{
		if (TPCInputMgr.Instance.Closed)
		{
			return;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			int num = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
			int num2 = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
			Vector2 vector = new Vector2(num, num2);
			TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput_WASD, vector);
		}
		else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
		{
			Vector2 vector2 = new Vector2(0f, 0f);
			TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput_WASD, vector2);
		}
		float axis = Input.GetAxis("Mouse X");
		float axis2 = Input.GetAxis("Mouse Y");
		TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput_MouseMove, new Vector2(axis, axis2));
		int num3 = 3;
		for (int i = 0; i < num3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				TPCInputEvent tPCInputEvent = new TPCInputEvent();
				tPCInputEvent.code = (KeyCode)(323 + i);
				tPCInputEvent.type = EventType.KeyDown;
				TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput_MouseButton, tPCInputEvent);
			}
			else if (Input.GetMouseButtonUp(i))
			{
				TPCInputEvent tPCInputEvent2 = new TPCInputEvent();
				tPCInputEvent2.code = (KeyCode)(323 + i);
				tPCInputEvent2.type = EventType.KeyUp;
				TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput_MouseButton, tPCInputEvent2);
			}
		}
	}

	private void OnGUI()
	{
		if (!TPCInputMgr.Instance.Closed && Event.current.keyCode != KeyCode.None && Event.current.keyCode != KeyCode.W && Event.current.keyCode != KeyCode.A && Event.current.keyCode != KeyCode.S && Event.current.keyCode != KeyCode.D && (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp))
		{
			TPCInputEvent tPCInputEvent = new TPCInputEvent();
			tPCInputEvent.code = Event.current.keyCode;
			tPCInputEvent.type = Event.current.type;
			TPCInputMgr.Instance.Execute(COMA_MsgSec.PCInput, tPCInputEvent);
		}
	}
}
