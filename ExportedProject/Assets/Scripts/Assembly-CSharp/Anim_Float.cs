using UnityEngine;

public class Anim_Float : MonoBehaviour
{
	public float extent = 0.5f;

	private Vector3 posStr = Vector3.zero;

	private Vector3 posEnd = Vector3.zero;

	private float lerp = 0.5f;

	public float lerpAdd = 0.5f;

	private void Start()
	{
		posStr = base.transform.localPosition - Vector3.up * extent;
		posEnd = base.transform.localPosition + Vector3.up * extent;
	}

	private void Update()
	{
		base.transform.localPosition = Vector3.Lerp(posStr, posEnd, lerp);
		lerp += lerpAdd * Time.deltaTime;
		if (lerp >= 1f || lerp <= 0f)
		{
			lerpAdd = 0f - lerpAdd;
		}
	}
}
