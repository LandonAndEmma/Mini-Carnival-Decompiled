using UnityEngine;

public class UI_ButtonLight : MonoBehaviour
{
	[SerializeField]
	private GameObject _light;

	private void Awake()
	{
		if (!(_light == null))
		{
			return;
		}
		Transform transform = base.transform.FindChild("light");
		if (transform != null)
		{
			_light = transform.gameObject;
			return;
		}
		Transform transform2 = base.transform.FindChild("Light");
		if (transform2 != null)
		{
			_light = transform2.gameObject;
		}
	}

	private void Start()
	{
		if (_light != null)
		{
			_light.SetActive(false);
		}
	}

	private void Update()
	{
	}

	public void LightOn()
	{
		if (_light != null)
		{
			_light.SetActive(true);
		}
	}

	public void LightOff()
	{
		if (_light != null)
		{
			_light.SetActive(false);
		}
	}
}
