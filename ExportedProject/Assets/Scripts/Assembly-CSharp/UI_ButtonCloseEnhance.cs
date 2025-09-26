using UnityEngine;

public class UI_ButtonCloseEnhance : MonoBehaviour
{
	[SerializeField]
	private GameObject _owner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		_owner.SetActive(false);
	}
}
