using MessageID;

public class COMA_DataExchange : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, PrefDataExchange);
		RegisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this, PrefDataExchange);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this);
	}

	private void Start()
	{
		base.Priority = 1000;
	}

	private void Update()
	{
	}

	private bool PrefDataExchange(TUITelegram msg)
	{
		UIDataBufferCenter.Instance.ParseSaveData();
		return true;
	}
}
