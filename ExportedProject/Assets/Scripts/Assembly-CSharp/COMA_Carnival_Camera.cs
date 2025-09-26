using UnityEngine;

public class COMA_Carnival_Camera : MonoBehaviour
{
	private static COMA_Carnival_Camera _instance;

	private bool bMove;

	private bool bVersionReady;

	private Vector3 pos = new Vector3(0.1f, 1.9f, 20f);

	private Quaternion rot = new Quaternion(0f, 0.996f, -0.087f, 0f);

	private Vector3 pos_str = Vector3.zero;

	private Quaternion rot_str = Quaternion.identity;

	[SerializeField]
	private UIGameThemeIconMgr _titleCom;

	private int _curDataConfigArrived;

	private int _totleNeedDataConfig = 2;

	private bool startMoving;

	private float lerp;

	public static COMA_Carnival_Camera Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		pos = base.transform.position;
		rot = base.transform.rotation;
	}

	public void HideTitle()
	{
	}

	public void VersionReady()
	{
		startMoving = true;
	}

	public void ConfigReady(int totleNeed)
	{
		_curDataConfigArrived++;
		_totleNeedDataConfig = totleNeed;
	}

	public void RealCameraMove()
	{
	}

	private void Update()
	{
		if (startMoving && _curDataConfigArrived >= _totleNeedDataConfig)
		{
			lerp += Time.deltaTime * 0.6f;
			base.transform.position = Vector3.Lerp(pos_str, pos, lerp);
			base.transform.rotation = Quaternion.Lerp(rot_str, rot, lerp);
			base.camera.fieldOfView = Mathf.Lerp(base.camera.fieldOfView, 60f, lerp);
			Debug.Log("Camera Ready!!");
			SceneTimerInstance.Instance.Add(0.1f, UIIn);
			startMoving = false;
		}
	}

	public bool UIIn()
	{
		Debug.Log("切换场景");
		UIDataBufferCenter.Instance.PreSceneName = Application.loadedLevelName;
		Application.LoadLevel("UI.Square");
		ChartBoostAndroid.showInterstitial(null);
		OpenClikPlugin.Show(true);
		return false;
	}
}
