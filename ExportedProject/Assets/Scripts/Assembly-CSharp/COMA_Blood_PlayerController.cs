using UnityEngine;

public class COMA_Blood_PlayerController : TBaseEntityCenterController
{
	public enum EState
	{
		Idle = 0,
		Fire = 1,
		MaxCount = 2
	}

	public enum EChildTag
	{
		Buff = 0
	}

	private StateMachine<COMA_Blood_PlayerController> stateMachine;

	private TStateFactory<COMA_Blood_PlayerController>[] stateFactory;

	private TState<COMA_Blood_PlayerController>[] objState;

	public COMA_Blood_PlayerController(TBaseEntity own)
		: base(own)
	{
	}

	protected override int InitController()
	{
		stateMachine = new StateMachine<COMA_Blood_PlayerController>(this);
		stateFactory = new TStateFactory<COMA_Blood_PlayerController>[2];
		stateFactory[0] = new COMA_Blood_PlayerStateCreator_Idle();
		stateFactory[1] = new COMA_Blood_PlayerStateCreator_Fire();
		objState = new TState<COMA_Blood_PlayerController>[2];
		for (int i = 0; i < 2; i++)
		{
			objState[i] = stateFactory[i].CreateState();
		}
		stateMachine.Init(objState[0], new COMA_Blood_PlayerStateCreator_Global().CreateState());
		return 0;
	}

	public void ChangeState(EState curState)
	{
		Debug.Log(string.Concat("########Change STATE=", stateMachine.CurState, "   TO STATE=", objState[(int)curState]));
		if (curState != EState.MaxCount)
		{
			stateMachine.ChangeState(objState[(int)curState]);
		}
	}

	public StateMachine<COMA_Blood_PlayerController> GetStateMachine()
	{
		return stateMachine;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		base.HandleMessage(msg);
		return stateMachine.HandleMessage(msg);
	}

	public override void Tick()
	{
		stateMachine.Update();
	}

	public override void FixedTick()
	{
	}

	private TBaseEntityBuffController GetBuffCtrl()
	{
		int childSysTypeByTag = GetChildSysTypeByTag(0);
		TBaseEntityBuffController tBaseEntityBuffController = GetOwner().GetSystemCtrlByType(childSysTypeByTag) as TBaseEntityBuffController;
		if (tBaseEntityBuffController == null)
		{
			Debug.LogError("------------>Cann't find buffCtrl!!! ChildSysType=" + childSysTypeByTag);
		}
		return tBaseEntityBuffController;
	}
}
