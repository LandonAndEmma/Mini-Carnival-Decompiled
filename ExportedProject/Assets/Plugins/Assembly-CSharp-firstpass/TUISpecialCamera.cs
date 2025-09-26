using UnityEngine;

public class TUISpecialCamera : MonoBehaviour
{
	private void Start()
	{
		base.camera.rect = new Rect(0.5f, 0.2f, 1f, 1f);
	}

	private void Update()
	{
	}
}
