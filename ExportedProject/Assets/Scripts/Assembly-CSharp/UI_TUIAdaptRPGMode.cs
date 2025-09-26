using UnityEngine;

public class UI_TUIAdaptRPGMode : MonoBehaviour
{
	[SerializeField]
	private GameObject _mask;

	private void Awake()
	{
		_mask.SetActive(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Mask()
	{
		_mask.SetActive(true);
	}
}
