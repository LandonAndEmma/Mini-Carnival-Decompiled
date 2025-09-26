using UnityEngine;

public class ParkourProcessInfo
{
	private UI_ParkourProcessMgr _mgr;

	private float _fProcess;

	private Texture2D _tex;

	public Texture2D PlayerTex
	{
		get
		{
			return _tex;
		}
		set
		{
			_tex = value;
			if (_mgr != null)
			{
				_mgr.DataChanged();
			}
		}
	}

	public float PlayerProcess
	{
		get
		{
			return _fProcess;
		}
		set
		{
			_fProcess = value;
			if (_mgr != null)
			{
				_mgr.DataChanged();
			}
		}
	}

	public ParkourProcessInfo()
	{
		_fProcess = 0f;
		_tex = null;
	}

	public ParkourProcessInfo(float p, Texture2D tex)
	{
		_fProcess = p;
		_tex = tex;
	}

	public void SetMgr(UI_ParkourProcessMgr mgr)
	{
		_mgr = mgr;
	}

	public void DelayAssignment(Texture2D tex)
	{
		PlayerTex = tex;
	}
}
