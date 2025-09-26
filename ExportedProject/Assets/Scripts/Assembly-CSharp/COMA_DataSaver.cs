using UnityEngine;

public class COMA_DataSaver : MonoBehaviour
{
	private static COMA_DataSaver _instance;

	public static COMA_DataSaver Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDisable()
	{
		_instance = null;
	}
}
