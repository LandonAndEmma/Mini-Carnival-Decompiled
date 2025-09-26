using UnityEngine;

public class NotRotateWithParent : MonoBehaviour
{
	private Quaternion rot = Quaternion.identity;

	private void Start()
	{
		rot = base.transform.rotation;
	}

	private void Update()
	{
		base.transform.rotation = rot;
	}
}
