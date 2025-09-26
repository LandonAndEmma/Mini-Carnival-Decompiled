using UnityEngine;

public class UIGameThemeIconMgr : MonoBehaviour
{
	private void Start()
	{
		Enter();
	}

	private void Update()
	{
	}

	public void Enter()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Start_CarnivalIn);
		if ((bool)base.animation)
		{
			base.animation.Play("UI_GameTheme_Enter");
		}
	}

	public void Exit()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Start_CarnivalOut);
		if ((bool)base.animation)
		{
			base.animation.Play("UI_GameTheme_Exit");
		}
	}

	public void ExitEvent()
	{
	}
}
