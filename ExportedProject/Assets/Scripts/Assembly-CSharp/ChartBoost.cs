using UnityEngine;

public class ChartBoost : MonoBehaviour
{
	private static ChartBoost _instance;

	public static ChartBoost Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		Debug.Log("-------------ChartboostPlugin.StartSession---------------");
		ChartboostPlugin.StartSession("52132e1216ba47932e000003", "d8f9935dda1ef8ec0591efecf9e412727612b0a4");
	}
}
