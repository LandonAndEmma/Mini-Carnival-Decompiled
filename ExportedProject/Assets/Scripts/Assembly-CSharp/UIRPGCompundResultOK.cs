using UnityEngine;

public class UIRPGCompundResultOK : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		base.transform.parent.gameObject.SetActive(false);
	}
}
