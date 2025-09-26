using UnityEngine;

public class UI_UsePropInfoMgr : MonoBehaviour
{
	[SerializeField]
	private UI_UsePropRecord[] _propRecords;

	private UI_UsePropInfoDataQueue _propInfoQueue = new UI_UsePropInfoDataQueue();

	public void AddUsePropInfo(UI_UsePropInfoData data)
	{
		_propInfoQueue.AddInfo(data);
		RefreshUI();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	protected void RefreshUI()
	{
		int queueCount = _propInfoQueue.QueueCount;
		if (queueCount > 0)
		{
			for (int i = 0; i < queueCount; i++)
			{
				UI_UsePropInfoData queueEleAt = _propInfoQueue.GetQueueEleAt(i);
				_propRecords[i].RefreshUIByData(queueEleAt);
			}
			for (int j = queueCount; j < _propInfoQueue.MaxQueueCount; j++)
			{
				_propRecords[j].RefreshUIByData(null);
			}
		}
	}
}
