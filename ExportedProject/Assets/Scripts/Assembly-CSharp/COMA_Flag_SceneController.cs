using UnityEngine;

public class COMA_Flag_SceneController : MonoBehaviour
{
	private static COMA_Flag_SceneController _instance;

	public Transform flagNodeTrs;

	public Transform flagTrs;

	public static COMA_Flag_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Hunger);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(1);
		return false;
	}

	private void Update()
	{
	}

	public void FlagDisappear()
	{
		flagNodeTrs.gameObject.SetActive(false);
		SceneTimerInstance.Instance.Remove(FlagAppear);
		SceneTimerInstance.Instance.Add(COMA_TimeAtlas.Instance.flagRefresh, FlagAppear);
	}

	public bool FlagAppear()
	{
		flagNodeTrs.gameObject.SetActive(true);
		GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/restart/restart")) as GameObject;
		gameObject.transform.position = flagNodeTrs.position;
		Object.DestroyObject(gameObject, 3f);
		return false;
	}
}
