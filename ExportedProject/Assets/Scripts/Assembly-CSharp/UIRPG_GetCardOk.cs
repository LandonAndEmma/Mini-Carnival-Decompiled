using UnityEngine;

public class UIRPG_GetCardOk : MonoBehaviour
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
		Object.DestroyObject(_owner);
	}
}
