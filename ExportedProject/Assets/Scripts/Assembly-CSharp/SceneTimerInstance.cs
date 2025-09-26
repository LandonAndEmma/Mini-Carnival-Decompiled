using System.Collections.Generic;
using UnityEngine;

public class SceneTimerInstance : MonoBehaviour
{
	private class TimerInfo
	{
		public float startTime;

		public float lastTime;

		public TimerCall callBack;

		public int nCallCount;

		public bool bUseCallCount;
	}

	public delegate bool TimerCall();

	private List<TimerInfo> infoList = new List<TimerInfo>();

	private static SceneTimerInstance _instance;

	public static SceneTimerInstance Instance
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

	private void Update()
	{
		if (infoList.Count <= 0)
		{
			return;
		}
		for (int num = infoList.Count - 1; num >= 0; num--)
		{
			if (Time.time - infoList[num].startTime >= infoList[num].lastTime)
			{
				bool flag = infoList[num].callBack();
				infoList[num].nCallCount--;
				if (flag || (infoList[num].bUseCallCount && infoList[num].nCallCount > 0))
				{
					infoList[num].startTime = Time.time;
				}
				else
				{
					infoList.RemoveAt(num);
				}
			}
		}
	}

	public void Add(float lastTime, TimerCall callBack)
	{
		Add(lastTime, callBack, 1, false);
	}

	public void Add(float lastTime, TimerCall callBack, int nCount, bool bUseCount)
	{
		TimerInfo timerInfo = new TimerInfo();
		timerInfo.startTime = Time.time;
		timerInfo.lastTime = lastTime;
		timerInfo.callBack = callBack;
		timerInfo.nCallCount = nCount;
		timerInfo.bUseCallCount = bUseCount;
		infoList.Add(timerInfo);
	}

	public void Remove(TimerCall callBack)
	{
		for (int num = infoList.Count - 1; num >= 0; num--)
		{
			if (infoList[num].callBack == callBack)
			{
				infoList.RemoveAt(num);
			}
		}
	}

	public void Clear()
	{
		infoList.Clear();
	}
}
