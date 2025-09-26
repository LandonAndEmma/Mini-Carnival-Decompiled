public class StateMachine<T>
{
	private TState<T> globalState;

	private TState<T> curState;

	private TState<T> preState;

	private T owner;

	public T Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	public TState<T> CurState
	{
		get
		{
			return curState;
		}
	}

	public TState<T> PreState
	{
		get
		{
			return preState;
		}
	}

	public StateMachine(T t)
	{
		owner = t;
		curState = null;
		preState = null;
		globalState = null;
	}

	public void Init(TState<T> initState)
	{
		Init(initState, null);
	}

	public void Init(TState<T> initState, TState<T> gState)
	{
		globalState = gState;
		curState = initState;
		preState = null;
	}

	public void Update()
	{
		if (globalState != null)
		{
			globalState.Update(owner);
		}
		if (curState != null)
		{
			curState.Update(owner);
		}
	}

	public void ChangeState(TState<T> newState)
	{
		preState = curState;
		if (curState != null)
		{
			curState.Exit(owner);
		}
		curState = newState;
		curState.Enter(owner);
	}

	public void ReverToPreviousState()
	{
		if (preState != null)
		{
			ChangeState(preState);
		}
	}

	public bool HandleMessage(TTelegram msg)
	{
		if (curState != null && curState.OnMessage(owner, msg))
		{
			return true;
		}
		if (globalState != null && globalState.OnMessage(owner, msg))
		{
			return true;
		}
		return false;
	}
}
