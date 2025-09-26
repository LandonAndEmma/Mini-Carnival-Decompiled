using UnityEngine;

public class UIDonnotDestoryOnLoad : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
