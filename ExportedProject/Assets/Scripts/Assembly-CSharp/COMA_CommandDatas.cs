using TNetSdk;

public abstract class COMA_CommandDatas
{
	public TNetUser dataSender;

	public float timeStamp_Send;

	protected COMA_Command _command;

	public COMA_Command Command
	{
		get
		{
			return _command;
		}
		set
		{
			_command = value;
		}
	}

	public virtual void Package(ref SFSObject obj)
	{
		if (obj != null)
		{
			float val = 0f;
			if (COMA_Network.Instance.TNetInstance != null)
			{
				val = (float)COMA_Network.Instance.TNetInstance.TimeManager.NetworkTime;
			}
			obj.PutFloat("st", val);
			obj.PutShort("c", (short)Command);
		}
	}

	public virtual void Unpackage(SFSObject obj)
	{
		if (obj != null)
		{
			timeStamp_Send = obj.GetFloat("st");
		}
	}
}
