using UnityEngine;

public class RPGBuffEffectFlyToTeam : MonoBehaviour
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

	protected string _endEffect = string.Empty;

	public void StartFly(Transform aim, float fDur, float fOffset, string endEffect)
	{
		_flying = true;
		_aim = aim;
		_startFlyTime = Time.time;
		_flyDur = fDur;
		_flyOffset = fOffset;
		_flyDir = aim.position + _offsetPos - base.transform.position;
		_flyDir.Normalize();
		_flySpeed = Vector3.Distance(aim.position + _offsetPos, base.transform.position) / _flyDur;
		_endEffect = endEffect;
	}

	public void StartFly(Transform aim, float fDur)
	{
		StartFly(aim, fDur, 0f, null);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!_flying)
		{
			return;
		}
		if (Time.time - _startFlyTime <= _flyDur)
		{
			base.transform.position += _flyDir * _flySpeed * Time.deltaTime;
			return;
		}
		Debug.LogWarning(_endEffect);
		Object obj = Resources.Load(_endEffect);
		if (obj != null)
		{
			Debug.LogWarning(_endEffect);
			GameObject gameObject = Object.Instantiate(obj) as GameObject;
			gameObject.transform.parent = _aim.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			Object.Destroy(gameObject, 1.5f);
		}
		_flying = false;
		Object.Destroy(base.gameObject, 0.03f + _flyOffset);
	}
}
