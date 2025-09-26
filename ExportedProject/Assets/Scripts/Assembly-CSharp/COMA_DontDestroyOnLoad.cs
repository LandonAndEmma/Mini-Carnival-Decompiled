using UnityEngine;

public class COMA_DontDestroyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		if (base.name == "UNITY_IPHONE")
		{
			Object.DestroyObject(base.gameObject);
		}
		else
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void Start()
	{
	}
}
