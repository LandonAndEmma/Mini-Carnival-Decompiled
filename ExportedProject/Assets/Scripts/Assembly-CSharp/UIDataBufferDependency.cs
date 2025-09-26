public class UIDataBufferDependency : UIEntity
{
	protected bool _NeedGetNewData;

	protected override void Load()
	{
		_NeedGetNewData = true;
	}

	protected override void UnLoad()
	{
		_NeedGetNewData = false;
	}

	protected void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
