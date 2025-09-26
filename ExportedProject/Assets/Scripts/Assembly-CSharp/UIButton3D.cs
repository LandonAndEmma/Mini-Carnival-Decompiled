using UnityEngine;

public class UIButton3D : MonoBehaviour
{
	[SerializeField]
	private Camera camera3D;

	private void Awake()
	{
		camera3D = GameObject.Find("Camera3D").GetComponent<Camera>();
	}

	private void Start()
	{
		Vector3 vector = camera3D.WorldToScreenPoint(base.transform.position);
		Debug.Log("ScreenPos" + vector);
	}

	private void Update()
	{
	}
}
