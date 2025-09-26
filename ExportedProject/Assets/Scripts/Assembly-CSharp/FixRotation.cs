using UnityEngine;

public class FixRotation : MonoBehaviour
{
	[SerializeField]
	private Vector3 _vRot;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(_vRot);
	}
}
