using System.Collections;

public class UI_UsePropInfoDataQueue
{
	private ArrayList _dataQueue;

	private int _nMaxCount = 4;

	public int MaxQueueCount
	{
		get
		{
			return _nMaxCount;
		}
	}

	public int QueueCount
	{
		get
		{
			return _dataQueue.Count;
		}
	}

	public UI_UsePropInfoDataQueue()
	{
		_dataQueue = new ArrayList();
	}

	public UI_UsePropInfoData GetQueueEleAt(int nIndex)
	{
		if (nIndex < 0 || nIndex >= QueueCount)
		{
			return null;
		}
		return (UI_UsePropInfoData)_dataQueue[QueueCount - 1 - nIndex];
	}

	public void AddInfo(UI_UsePropInfoData data)
	{
		_dataQueue.Add(data);
		if (_dataQueue.Count > _nMaxCount)
		{
			RemoveInfo();
		}
	}

	protected void RemoveInfo()
	{
		if (_dataQueue.Count != 0)
		{
			_dataQueue.RemoveAt(0);
		}
	}
}
