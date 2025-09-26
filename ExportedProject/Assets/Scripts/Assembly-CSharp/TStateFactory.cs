public abstract class TStateFactory<T>
{
	public virtual TState<T> CreateState()
	{
		return null;
	}
}
