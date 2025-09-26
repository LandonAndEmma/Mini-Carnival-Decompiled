using System;
using System.Collections.Generic;
using UnityEngine;

public class COMA_VideoController : MonoBehaviour
{
	private static COMA_VideoController _instance;

	private bool _bVideoStopPopView;

	[NonSerialized]
	public bool bRecorded;

	public bool bClickKamcord;

	[NonSerialized]
	public int nVideo;

	public static COMA_VideoController Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool bVideoPopView
	{
		get
		{
			return _bVideoStopPopView;
		}
		set
		{
			_bVideoStopPopView = value;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Transform transform = base.transform.FindChild("KamcordPrefab");
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartRecording()
	{
		if (nVideo != 1 && nVideo == 2)
		{
			Kamcord.StartRecording();
		}
	}

	public void StopRecording()
	{
		if (nVideo != 1 && nVideo == 2)
		{
			Kamcord.StopRecording();
		}
	}

	public void SetMetadata(string key, object val)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(key, val);
		SetMetadata(dictionary);
	}

	public void SetMetadata(Dictionary<string, object> metadata)
	{
		if (nVideo != 1 && nVideo == 2)
		{
			Kamcord.SetVideoTitle("Carnival Gameplay");
			Kamcord.SetVideoMetadata(metadata);
		}
	}

	public void PlayLastRecording()
	{
		if (nVideo != 1 && nVideo == 2)
		{
			Kamcord.ShowView();
		}
	}

	public void Show()
	{
		if (nVideo != 1 && nVideo == 2)
		{
			Kamcord.ShowWatchView();
		}
	}
}
