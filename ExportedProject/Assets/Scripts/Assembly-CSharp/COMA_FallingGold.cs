using UnityEngine;

public class COMA_FallingGold : MonoBehaviour
{
	public string ptl_path_meteor = string.Empty;

	public string ptl_path_meteorBlast = string.Empty;

	private Vector3 pos_start = Vector3.zero;

	public Vector3 pos_target = Vector3.zero;

	public float lerpTime = 1f;

	private float lerpStep;

	private float lerp;

	private void Start()
	{
		if (ptl_path_meteor != string.Empty)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/" + ptl_path_meteor), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
		}
		pos_start = base.transform.position;
		lerpStep = 1f / lerpTime;
	}

	private void Update()
	{
		lerp += lerpStep * Time.deltaTime;
		base.transform.position = Vector3.Lerp(pos_start, pos_target, lerp);
		if (lerp >= 1f)
		{
			Object.DestroyObject(base.gameObject);
			if (ptl_path_meteorBlast != string.Empty)
			{
				GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/" + ptl_path_meteorBlast), base.transform.position, base.transform.rotation) as GameObject;
				Object.DestroyObject(obj, 2f);
			}
		}
	}
}
