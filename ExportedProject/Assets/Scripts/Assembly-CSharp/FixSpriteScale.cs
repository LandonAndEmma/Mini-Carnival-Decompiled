using UnityEngine;

public class FixSpriteScale : MonoBehaviour
{
	[SerializeField]
	private Vector2 scale_sprite = Vector2.zero;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		float num = 640f / (float)UIRoot.list[0].activeHeight;
		base.transform.localScale = new Vector3(scale_sprite.x / num, scale_sprite.y / num, 1f);
	}
}
