using UnityEngine;

public class UIRoom_Countdown : MonoBehaviour
{
	public enum ECountdownState
	{
		Idle = 0,
		Timing = 1,
		End = 2
	}

	public TUILabel seconds;

	public GameObject descr;

	private ECountdownState state;

	private float maxSeconds = 59f;

	public bool bTestCountDown;

	private void Awake()
	{
		ActiveCountDownContr(false);
	}

	private void Update()
	{
		if (bTestCountDown)
		{
			StartCountDown(6f);
			bTestCountDown = false;
		}
		switch (state)
		{
		case ECountdownState.Idle:
			break;
		case ECountdownState.Timing:
			maxSeconds -= Time.deltaTime;
			ProcElapseSeconds();
			break;
		case ECountdownState.End:
			ProcEndCountDown();
			break;
		}
	}

	protected void ActiveCountDownContr(bool b)
	{
		descr.active = b;
		seconds.gameObject.active = b;
	}

	public int StartCountDown(float fTime)
	{
		ActiveCountDownContr(true);
		maxSeconds = fTime;
		state = ECountdownState.Timing;
		return 0;
	}

	public void EndCountDown()
	{
		ActiveCountDownContr(false);
		state = ECountdownState.Idle;
	}

	protected int ProcElapseSeconds()
	{
		maxSeconds = ((!((double)maxSeconds <= 1E-06)) ? maxSeconds : 0f);
		seconds.Text = ((int)maxSeconds).ToString();
		if (maxSeconds == 0f)
		{
			state = ECountdownState.End;
		}
		return 0;
	}

	protected int ProcEndCountDown()
	{
		Debug.Log("Time End!");
		ActiveCountDownContr(false);
		state = ECountdownState.Idle;
		Application.LoadLevel(COMA_NetworkConnect.sceneName);
		Debug.Log("COMA_NetworkConnect.sceneId : " + COMA_NetworkConnect.sceneId);
		COMA_Login.Instance.AddGameModeCount(COMA_NetworkConnect.sceneId, COMA_CommonOperation.Instance.isCreateRoom);
		COMA_CommonOperation.Instance.EnterMode(COMA_NetworkConnect.sceneName);
		if (COMA_CommonOperation.Instance.selectedWeaponPrice > 0)
		{
			COMA_Pref.Instance.AddGold(-COMA_CommonOperation.Instance.selectedWeaponPrice);
			COMA_Pref.Instance.Save(true);
			COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
		}
		else if (COMA_CommonOperation.Instance.selectedWeaponPrice < 0)
		{
			COMA_Pref.Instance.AddCrystal(COMA_CommonOperation.Instance.selectedWeaponPrice);
			COMA_Pref.Instance.Save(true);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
		}
		UIRoom component = base.gameObject.transform.root.GetComponent<UIRoom>();
		if (component != null)
		{
			UIInputChatBox inputChatBox = component.GetInputChatBox();
			if (inputChatBox != null)
			{
				inputChatBox.closedkeyboard();
			}
		}
		return 0;
	}
}
