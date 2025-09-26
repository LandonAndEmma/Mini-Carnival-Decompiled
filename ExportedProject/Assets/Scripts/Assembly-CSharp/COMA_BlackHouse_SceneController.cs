using TNetSdk;
using UnityEngine;

public class COMA_BlackHouse_SceneController : TBaseEntity
{
	private static COMA_BlackHouse_SceneController _instance;

	private int vampireNumber;

	public static COMA_BlackHouse_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		_instance = this;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.AddEventListener(TNetEventRoom.ROOM_COMMENT_CHANGE, OnCommentChange);
		}
	}

	private new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.RemoveEventListener(TNetEventRoom.ROOM_COMMENT_CHANGE, OnCommentChange);
		}
	}

	private void OnCommentChange(TNetEventData tEvent)
	{
		int num = 0;
		string text = (string)tEvent.data["comment"];
		Debug.Log("OnCommentChange userId : " + num + " comment : " + text);
		vampireNumber = int.Parse(text);
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Scene_Castle);
		float lastTime = COMA_Camera.Instance.SceneStart_CameraAnim();
		SceneTimerInstance.Instance.Add(lastTime, CountToStart);
		COMA_Sys.Instance.bCoverUIInput = true;
	}

	public bool CountToStart()
	{
		COMA_Scene.Instance.CountToStart(6);
		return false;
	}

	private new void Update()
	{
	}

	public void AddRoomComment()
	{
		vampireNumber++;
		if (COMA_Network.Instance.TNetInstance != null)
		{
			COMA_Network.Instance.TNetInstance.Send(new SetRoomCommentRequest(COMA_Network.Instance.TNetInstance.CurRoom.Id, vampireNumber.ToString()));
		}
	}
}
