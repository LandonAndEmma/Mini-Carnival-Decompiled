using UnityEngine;

public class UI_Point : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.2f);
		Gizmos.DrawSphere(base.transform.position, 1f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
		Gizmos.DrawSphere(base.transform.position, 1f);
	}
}
