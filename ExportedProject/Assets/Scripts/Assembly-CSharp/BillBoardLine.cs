using UnityEngine;

public class BillBoardLine : MonoBehaviour
{
	public Camera cameraToLookAt;

	private void Start()
	{
	}

	private void Update()
	{
		if (cameraToLookAt != null)
		{
			base.transform.LookAt(cameraToLookAt.transform.position);
		}
	}
}
