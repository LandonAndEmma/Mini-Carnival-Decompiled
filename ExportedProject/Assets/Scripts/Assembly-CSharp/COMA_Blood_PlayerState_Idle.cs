using UnityEngine;

public class COMA_Blood_PlayerState_Idle : TState<COMA_Blood_PlayerController>
{
	private float fEnterTime;

	public override void Enter(COMA_Blood_PlayerController t)
	{
		fEnterTime = Time.time;
		Debug.Log("Enter Idle");
	}

	public override void Update(COMA_Blood_PlayerController t)
	{
	}

	public override void Exit(COMA_Blood_PlayerController t)
	{
	}

	public override bool OnMessage(COMA_Blood_PlayerController t, TTelegram msg)
	{
		bool result = false;
		if (msg._nMsgId == 0)
		{
			result = true;
		}
		return result;
	}
}
