using UnityEngine;

public class NGUI_WaitingBox : MonoBehaviour
{
	private string[] _frameIcons = new string[8] { "juhua1", "juhua2", "juhua3", "juhua4", "juhua5", "juhua6", "juhua7", "juhua8" };

	[SerializeField]
	private float _intervalTime = 0.3f;

	[SerializeField]
	private UISprite _icon;

	private float _fCurTime;

	private int nIndex;

	private bool _bStart;

	[SerializeField]
	private bool _bAutoStart;

	private void Start()
	{
		nIndex = 0;
		_fCurTime = Time.time;
		_icon.spriteName = _frameIcons[nIndex];
		if (_bAutoStart)
		{
			StartWaiting();
		}
	}

	private void Update()
	{
		if (_bStart && Time.time - _fCurTime >= _intervalTime)
		{
			_fCurTime = Time.time;
			nIndex++;
			if (nIndex >= _frameIcons.Length)
			{
				nIndex = 0;
			}
			_icon.spriteName = _frameIcons[nIndex];
		}
	}

	public void StartWaiting()
	{
		StartWaiting(Vector2.zero, new Vector2(600f, 600f), _intervalTime);
	}

	public void StartWaiting(Vector2 pos)
	{
		StartWaiting(pos, new Vector2(600f, 600f), _intervalTime);
	}

	public void StartWaiting(TUIControlImpl control)
	{
		Vector2 pos = control.transform.position;
		Vector2 size = control.size;
		StartWaiting(pos, size, _intervalTime);
	}

	public void StartWaiting(Vector2 pos, Vector2 coverArear)
	{
		StartWaiting(pos, coverArear, _intervalTime);
	}

	public void StartWaiting(Vector2 pos, Vector2 coverArear, float fIntervalTime)
	{
		_bStart = true;
		nIndex = 0;
		_fCurTime = Time.time;
		_icon.spriteName = _frameIcons[nIndex];
		_intervalTime = fIntervalTime;
	}

	public void EndWaiting()
	{
		Object.Destroy(base.gameObject);
	}
}
