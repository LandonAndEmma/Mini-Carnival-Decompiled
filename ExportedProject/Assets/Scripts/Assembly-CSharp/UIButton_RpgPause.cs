using UnityEngine;

public class UIButton_RpgPause : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		Time.timeScale = ((Time.timeScale == 0f) ? 1 : 0);
	}
}
