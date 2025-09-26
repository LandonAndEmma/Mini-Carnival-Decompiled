using System.Collections.Generic;

public class COMA_InstanceManager
{
	private static COMA_InstanceManager _instance;

	private List<COMA_ResetInstance> lst = new List<COMA_ResetInstance>();

	public static COMA_InstanceManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_InstanceManager();
			}
			return _instance;
		}
	}

	public void RegistResetInstance(COMA_ResetInstance ins)
	{
		lst.Add(ins);
	}

	public void Reset()
	{
		foreach (COMA_ResetInstance item in lst)
		{
			item.ResetInstance();
		}
		lst.Clear();
	}
}
