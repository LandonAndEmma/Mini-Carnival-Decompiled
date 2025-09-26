using UnityEngine;

public class UI_GameEndMgr : MonoBehaviour
{
	public enum EState
	{
		ShowEnd = 0,
		RealEnd = 1
	}

	private static UI_GameEndMgr _instance;

	[SerializeField]
	private GameObject _objIconEnd;

	private float _fCurStateStartTime;

	private EState _curState = EState.RealEnd;

	public static UI_GameEndMgr Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public void EnterGameEnd()
	{
		_fCurStateStartTime = Time.time;
		_curState = EState.ShowEnd;
		_objIconEnd.SetActive(true);
		if (_objIconEnd.animation != null)
		{
			_objIconEnd.animation.Play();
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Finish);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_curState == EState.ShowEnd && Time.time - _fCurStateStartTime >= 1f)
		{
			_objIconEnd.SetActive(false);
			_curState = EState.RealEnd;
			COMA_Scene.Instance.runingGameOver = true;
			COMA_Scene.Instance.GameFinishByTime();
		}
	}
}
