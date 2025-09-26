using MessageID;

public class UIScene : UIEntity
{
	protected override void Load()
	{
	}

	protected override void UnLoad()
	{
	}

	private void Awake()
	{
	}

	private void Start()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_InitSceneEntityMap, this, this);
	}

	private void Destroy()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ClearSceneEntityMap, this, this);
	}

	protected override void Tick()
	{
	}
}
