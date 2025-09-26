using UnityEngine;

[ExecuteInEditMode]
public class UI_AnimationControl : MonoBehaviour
{
	[SerializeField]
	private UI_AnimationControlMgr _aniMgr;

	[SerializeField]
	protected SUIExit[] _exitUI;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AniEnterStart()
	{
		if (_aniMgr != null)
		{
			_aniMgr.EnterStart();
		}
	}

	public void AniEnterEnd()
	{
		if (_aniMgr != null)
		{
			_aniMgr.EnterEnd();
		}
	}

	public void AniExitStart()
	{
		if (_aniMgr != null)
		{
			_aniMgr.ExitStart();
		}
	}

	public void AniExitEnd()
	{
		if (_aniMgr != null)
		{
			_aniMgr.ExitEnd();
		}
	}

	public void PlayExitAni()
	{
		SUIExit[] exitUI = _exitUI;
		foreach (SUIExit sUIExit in exitUI)
		{
			sUIExit._obj.animation.Play(sUIExit._clip.name);
		}
	}

	public void PlayExitAni(string enterName)
	{
		_aniMgr._strEnterSceneName = enterName;
		PlayExitAni();
	}
}
