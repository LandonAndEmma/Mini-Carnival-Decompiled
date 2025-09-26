using UnityEngine;

public class COMA_BlackHouse_Data : MonoBehaviour
{
	private static COMA_BlackHouse_Data _instance;

	public static COMA_BlackHouse_Data Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
