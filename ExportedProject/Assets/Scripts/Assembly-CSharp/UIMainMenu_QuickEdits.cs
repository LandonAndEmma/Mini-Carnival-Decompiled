using UnityEngine;

public class UIMainMenu_QuickEdits : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Exit()
	{
		base.animation.Play("UI_QuickEdit_Exit");
	}

	public void AniEndEvent()
	{
		base.gameObject.SetActive(false);
	}
}
