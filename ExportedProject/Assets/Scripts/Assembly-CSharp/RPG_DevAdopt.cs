using UnityEngine;

public class RPG_DevAdopt : MonoBehaviour
{
	private void Start()
	{
		if (Screen.height % 768 == 0)
		{
			base.transform.position += new Vector3(0f, 0f, 2f);
		}
	}

	private void Update()
	{
	}
}
