using UnityEngine;

public class CameraAgility : MonoBehaviour
{
	private float distanceToPlayer;

	private float distanceCurrent;

	private void Start()
	{
		ResetCameraProperty();
	}

	public void ResetCameraProperty()
	{
		distanceToPlayer = 0f - base.transform.localPosition.z;
		distanceCurrent = distanceToPlayer;
	}

	private void Update()
	{
		Ray ray = new Ray(base.transform.parent.position, -base.transform.forward);
		int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, distanceToPlayer, layerMask))
		{
			distanceCurrent = Mathf.Lerp(distanceCurrent, hitInfo.distance, 0.5f);
		}
		else
		{
			distanceCurrent = Mathf.Lerp(distanceCurrent, distanceToPlayer, 0.5f);
		}
		base.transform.localPosition = new Vector3(0f, 0f, 0f - distanceCurrent);
	}
}
