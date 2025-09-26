using UnityEngine;

public class UI_ButtonEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject _icon;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetAlpha(float f)
	{
		if (_icon != null)
		{
			_icon.renderer.material.color = new Color(1f, 1f, 1f, f);
		}
	}
}
