using UnityEngine;

public class COMA_Fishing_SceneController : TBaseEntity
{
	private static COMA_Fishing_SceneController _instance;

	private int needPlayers;

	public GameObject barObj;

	public UIWaitingRoomPlayerNum roomInfoPlayerNum;

	private bool bBoxLock;

	[SerializeField]
	private GameObject[] _boats;

	public float[] _fOnBoatTimes = new float[10];

	private float _fMaxPraiseTime = 120f;

	private float _fPrePraiseTime;

	[SerializeField]
	private GameObject _objBuoys;

	public static COMA_Fishing_SceneController Instance
	{
		get
		{
			return _instance;
		}
	}

	public string GetFormatBoatLeftTime(int i)
	{
		string empty = string.Empty;
		float boatLeftTime = GetBoatLeftTime(i);
		int num = (int)boatLeftTime / 60;
		int num2 = (int)boatLeftTime % 60;
		empty = string.Format("{0:00}", num);
		empty += ":";
		string text = string.Format("{0:00}", num2);
		return empty + text;
	}

	public float GetBoatLeftTime(int i)
	{
		float num = 300f;
		if (i < 0)
		{
			return -1f;
		}
		float num2 = num - Time.time + _fOnBoatTimes[i];
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		if (num2 > num)
		{
			num2 = num;
		}
		return num2;
	}

	public void SetBoatActive(bool bActive, int nIndex)
	{
		_boats[nIndex].SetActive(bActive);
		if (bActive)
		{
			int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1019, TTelegram.SEND_MSG_IMMEDIATELY, nIndex + 1);
		}
	}

	public bool IsBoatActive(int nIndex)
	{
		return _boats[nIndex].activeSelf;
	}

	private new void OnEnable()
	{
		base.OnEnable();
		_instance = this;
	}

	private new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
	}

	public void RefreshPraiseTime()
	{
		_fPrePraiseTime = Time.time;
	}

	public bool IsCanUsePraise()
	{
		return (Time.time - _fPrePraiseTime >= _fMaxPraiseTime) ? true : false;
	}

	public void ActiveBuoys(bool b)
	{
		_objBuoys.SetActive(b);
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.Amb_Island);
		if (COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
		needPlayers = COMA_CommonOperation.Instance.SceneNameToPlayerCount(COMA_NetworkConnect.sceneName);
		COMA_Sys.Instance.bCoverUpdate = false;
		COMA_Sys.Instance.bCoverUIInput = false;
		_fPrePraiseTime = -1f * _fMaxPraiseTime;
	}

	private void OnDestroy()
	{
		TFishingAddressBook.Instance.ResetInstance();
	}

	private new void Update()
	{
		if (!bBoxLock && COMA_Network.Instance.TNetInstance != null)
		{
			if (COMA_Network.Instance.TNetInstance.CurRoom == null)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(102);
				uI_MsgBox.AddProceYesHandler(GoBackToMainMenu);
				bBoxLock = true;
			}
			else
			{
				roomInfoPlayerNum.CurNum = COMA_Network.Instance.TNetInstance.CurRoom.UserCount;
				roomInfoPlayerNum.MaxNum = COMA_Network.Instance.TNetInstance.CurRoom.MaxUsers;
			}
		}
	}

	public void GoBackToMainMenu(string param)
	{
		COMA_NetworkConnect.Instance.BackFromScene();
	}
}
