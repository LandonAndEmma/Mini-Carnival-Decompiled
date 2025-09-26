using UnityEngine;

public class COMA_ServerUpdate : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		HttpClient.Instance().HandleResponse();
	}
}
