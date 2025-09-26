using UnityEngine;

public class Ammo_Fly : MonoBehaviour
{
	[SerializeField]
	protected bool _flying;

	[SerializeField]
	protected Transform _aim;

	protected float _startFlyTime;

	protected float _flySpeed;

	protected Vector3 _flyDir = Vector3.zero;

	protected float _flyDur = 1.13f;

	protected float _flyOffset;

	private Vector3 _offsetPos = new Vector3(0f, 0.8f, 0f);

	public void StartFly(Transform aim, float fDur, float fOffset)
	{
		_flying = true;
		_aim = aim;
		_startFlyTime = Time.time;
		_flyDur = fDur;
		_flyOffset = fOffset;
		_flyDir = aim.position + _offsetPos - base.transform.position;
		_flyDir.Normalize();
		_flySpeed = Vector3.Distance(aim.position + _offsetPos, base.transform.position) / _flyDur;
	}

	public void StartFly(Transform aim, float fDur)
	{
		StartFly(aim, fDur, 0f);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_flying)
		{
			if (Time.time - _startFlyTime <= _flyDur)
			{
				base.transform.position += _flyDir * _flySpeed * Time.deltaTime;
				return;
			}
			_flying = false;
			Object.Destroy(base.gameObject, 0.03f + _flyOffset);
		}
	}
}
