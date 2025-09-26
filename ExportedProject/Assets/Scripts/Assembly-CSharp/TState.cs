public abstract class TState<T>
{
	protected int nCurFrame;

	public virtual void Enter(T t)
	{
	}

	public virtual void Update(T t)
	{
	}

	public virtual void Exit(T t)
	{
	}

	public virtual bool OnMessage(T t, TTelegram msg)
	{
		return false;
	}
}
