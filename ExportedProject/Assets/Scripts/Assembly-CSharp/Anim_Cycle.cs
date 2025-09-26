using UnityEngine;

public class Anim_Cycle : MonoBehaviour
{
	public float radius = 2f;

	public float speed = 1f;

	private float alpha;

	private float theta;

	private Vector3 oPosition = Vector3.zero;

	private void Start()
	{
		oPosition = base.transform.localPosition;
	}

	private void Update()
	{
		alpha += 5f * Time.deltaTime;
		float num = radius * (1f + Mathf.Cos(alpha) * 0.2f);
		base.transform.localPosition = oPosition + new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta)) * num;
		theta += speed * Time.deltaTime;
		if (theta > 6.28f)
		{
			theta -= 6.28f;
		}
	}
}
