using UnityEngine;

public class COMA_Exhaust : MonoBehaviour
{
	private void Update()
	{
		base.transform.position += new Vector3(0f, 0f, -1.5f * Time.deltaTime);
	}
}
