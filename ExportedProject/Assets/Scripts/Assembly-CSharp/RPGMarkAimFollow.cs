using UnityEngine;

public class RPGMarkAimFollow : MonoBehaviour
{
	private Transform MarkTrans;

	private void Start()
	{
		if (base.transform.parent.GetComponent<RPGEntity>() != null)
		{
			MarkTrans = base.transform.parent.GetComponent<RPGEntity>().MarkTrans;
		}
	}

	private void Update()
	{
		if (!(MarkTrans == null))
		{
			Vector3 position = base.transform.position;
			position.x = MarkTrans.position.x;
			position.z = MarkTrans.position.z;
			base.transform.position = position;
		}
	}
}
