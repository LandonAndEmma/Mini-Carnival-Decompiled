using UnityEngine;

public class COMA_Platform : MonoBehaviour
{
	private static COMA_Platform _instance;

	public GameObject playerNodeObj;

	public static COMA_Platform Instance
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
		if (_instance == null)
		{
			_instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	public void DestroyPlatform()
	{
		Object.DestroyObject(base.gameObject);
		_instance = null;
	}
}
