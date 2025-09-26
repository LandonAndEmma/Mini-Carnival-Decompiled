using UnityEngine;

public class UI_GameStartMgr : MonoBehaviour
{
	public enum EState
	{
		None = 0,
		ShowModelName = 1,
		CountDown = 2,
		ShowStart = 3,
		RealStart = 4
	}

	[SerializeField]
	private GameObject _objIconStart;

	[SerializeField]
	private GameObject[] _objCountDownTime;

	[SerializeField]
	private GameObject[] _objGameModels;

	private float _fCurStateStartTime;

	private EState _curState;

	private int nMaxCountDownTime = 3;

	private void Awake()
	{
		_objIconStart.SetActive(false);
		GameObject[] objCountDownTime = _objCountDownTime;
		foreach (GameObject gameObject in objCountDownTime)
		{
			gameObject.SetActive(false);
		}
		GameObject[] objGameModels = _objGameModels;
		foreach (GameObject gameObject2 in objGameModels)
		{
			gameObject2.SetActive(false);
		}
	}

	private void ChangeState(EState state, int nParam)
	{
		_fCurStateStartTime = Time.time;
		_curState = state;
		ProceEnterState(nParam);
	}

	private void ProceEnterState(int nParam)
	{
		switch (_curState)
		{
		case EState.ShowModelName:
		{
			_objIconStart.SetActive(false);
			GameStart(nParam);
			for (int m = 0; m < 3; m++)
			{
				_objCountDownTime[m].SetActive(false);
			}
			break;
		}
		case EState.CountDown:
		{
			_objIconStart.SetActive(false);
			GameObject[] objGameModels2 = _objGameModels;
			foreach (GameObject gameObject2 in objGameModels2)
			{
				gameObject2.SetActive(false);
			}
			for (int l = 0; l < 3; l++)
			{
				_objCountDownTime[l].SetActive(false);
			}
			Debug.Log("nMaxCountDownTime:" + nMaxCountDownTime);
			if (nMaxCountDownTime == 3)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown3);
			}
			else if (nMaxCountDownTime == 2)
			{
				Debug.Log("------AudioCategory.Game_CountDown2");
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown2);
			}
			else if (nMaxCountDownTime == 1)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown1);
			}
			_objCountDownTime[nMaxCountDownTime - 1].SetActive(true);
			_objCountDownTime[nMaxCountDownTime - 1].animation.Play();
			break;
		}
		case EState.ShowStart:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDownStart);
			_objIconStart.SetActive(true);
			if (_objIconStart.animation != null)
			{
				_objIconStart.animation.Play();
			}
			GameObject[] objGameModels3 = _objGameModels;
			foreach (GameObject gameObject3 in objGameModels3)
			{
				gameObject3.SetActive(false);
			}
			for (int num = 0; num < 3; num++)
			{
				_objCountDownTime[num].SetActive(false);
			}
			break;
		}
		case EState.RealStart:
		{
			_objIconStart.SetActive(false);
			GameObject[] objGameModels = _objGameModels;
			foreach (GameObject gameObject in objGameModels)
			{
				gameObject.SetActive(false);
			}
			for (int j = 0; j < 3; j++)
			{
				_objCountDownTime[j].SetActive(false);
			}
			Debug.Log("Notify Real Started");
			COMA_Sys.Instance.bCoverUIInput = false;
			COMA_Sys.Instance.bGameCounting = false;
			COMA_Sys.Instance.bRealStartGame = true;
			COMA_Scene.Instance.ReadyToPlay();
			if (UI_GameCountDownMgr.Instance != null)
			{
				UI_GameCountDownMgr.Instance.StartGameCountDown(COMA_Scene.Instance.gameTime);
			}
			break;
		}
		}
	}

	private void GameStart(int nGameModel)
	{
		for (int i = 0; i < _objGameModels.Length; i++)
		{
			if (i == nGameModel)
			{
				_objGameModels[i].SetActive(true);
			}
			else
			{
				_objGameModels[i].SetActive(false);
			}
		}
	}

	public void ShowGameModel(int nMode)
	{
		Debug.Log("ShowGameModel");
		ChangeState(EState.ShowModelName, nMode);
	}

	public void EnterGameModel(int nMode)
	{
		Debug.Log("EnterGameModel " + nMode);
		ChangeState(EState.ShowModelName, nMode);
	}

	public void EnterGameModel()
	{
		Debug.Log("EnterGameModel");
		ChangeState(EState.CountDown, 0);
	}

	public void ForceToStartGameForSync()
	{
		ChangeState(EState.RealStart, 0);
	}

	private void Start()
	{
	}

	private void Update()
	{
		switch (_curState)
		{
		case EState.ShowModelName:
			if (Time.time - _fCurStateStartTime >= 2f)
			{
				nMaxCountDownTime = 3;
			}
			break;
		case EState.CountDown:
		{
			if (!(Time.time - _fCurStateStartTime >= 1f))
			{
				break;
			}
			_fCurStateStartTime = Time.time;
			nMaxCountDownTime--;
			if (nMaxCountDownTime <= 0)
			{
				ChangeState(EState.ShowStart, 0);
				break;
			}
			if (nMaxCountDownTime == 3)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown3);
			}
			else if (nMaxCountDownTime == 2)
			{
				Debug.Log("------AudioCategory.Game_CountDown2");
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown2);
			}
			else if (nMaxCountDownTime == 1)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.Game_CountDown1);
			}
			for (int i = 0; i < 3; i++)
			{
				_objCountDownTime[i].SetActive(false);
			}
			_objCountDownTime[nMaxCountDownTime - 1].SetActive(true);
			_objCountDownTime[nMaxCountDownTime - 1].animation.Play();
			break;
		}
		case EState.ShowStart:
			if (Time.time - _fCurStateStartTime >= 1f)
			{
				ChangeState(EState.RealStart, 0);
			}
			break;
		}
	}
}
