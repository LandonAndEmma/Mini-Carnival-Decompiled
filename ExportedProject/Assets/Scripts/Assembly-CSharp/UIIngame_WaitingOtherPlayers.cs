using UnityEngine;

public class UIIngame_WaitingOtherPlayers : MonoBehaviour
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

	public void ForceEndCountDown()
	{
		state = ECountdownState.Idle;
		ActiveCountDownContr(false);
	}

	public int StartCountDown(float fTime)
	{
		ActiveCountDownContr(true);
		maxSeconds = fTime;
		state = ECountdownState.Timing;
		return 0;
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
		COMA_Scene.Instance.AllReadyAndCountDown();
		return 0;
	}
}
