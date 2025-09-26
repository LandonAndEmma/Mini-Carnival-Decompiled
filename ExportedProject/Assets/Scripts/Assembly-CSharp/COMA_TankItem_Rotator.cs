using UnityEngine;

public class COMA_TankItem_Rotator : MonoBehaviour
{
	public Vector3 vAxis = Vector3.forward;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(vAxis, 1.5f);
	}
}
