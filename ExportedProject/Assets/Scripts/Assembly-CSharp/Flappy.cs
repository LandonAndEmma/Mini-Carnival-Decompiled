using UnityEngine;

public class Flappy : MonoBehaviour
{
	private CharacterController cCtl;

	private float my;

	private void Start()
	{
		cCtl = base.gameObject.GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Flappy!");
			my = 10f;
		}
		float num = Mathf.Clamp(my, -10f, 10f);
		Debug.Log(num);
		num = (0f - num) * 7f + 10f;
		base.transform.localEulerAngles = new Vector3(num, 90f, 0f);
		my -= 25f * Time.deltaTime;
		Vector3 motion = new Vector3(0f, my, 0f) * Time.deltaTime;
		cCtl.Move(motion);
	}
}
