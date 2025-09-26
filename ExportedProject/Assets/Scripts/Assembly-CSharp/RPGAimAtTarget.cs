using UnityEngine;

public class RPGAimAtTarget : MonoBehaviour
{
	[SerializeField]
	private Transform _rotaTrans;

	[SerializeField]
	private Transform _rotaKeepTrans;

	[SerializeField]
	public Transform _effectTriggerPos;

	private Vector3 _oriRot;

	private bool _startRotation;

	private bool _startRotationBack;

	[SerializeField]
	private Transform _tar;

	private float _durTime;

	private float _startRotationTime;

	private Vector3 _tarRot = Vector3.right;

	private void Start()
	{
	}

	private void Update()
	{
		if (_startRotation)
		{
			if (Time.time - _startRotationTime < _durTime)
			{
				_rotaTrans.localEulerAngles = Vector3.Lerp(_oriRot, _tarRot, (Time.time - _startRotationTime) / _durTime);
			}
			else
			{
				_rotaTrans.localEulerAngles = _tarRot;
				_startRotation = false;
			}
		}
		if (_startRotationBack)
		{
			if (Time.time - _startRotationTime < _durTime)
			{
				_rotaTrans.localEulerAngles = Vector3.Lerp(_tarRot, _oriRot, (Time.time - _startRotationTime) / _durTime);
				return;
			}
			_startRotationBack = false;
			_rotaTrans.localEulerAngles = _oriRot;
		}
	}

	public float AimToTarget(Transform tar, float fDur)
	{
		_oriRot = _rotaTrans.localEulerAngles;
		Vector3 vector = tar.position - _rotaTrans.position;
		vector.Normalize();
		vector = new Vector3(vector.x, 0f, vector.z);
		Vector3 forward = _rotaTrans.forward;
		forward = new Vector3(forward.x, 0f, forward.z);
		Vector3 vector2 = _rotaTrans.position + forward;
		float num = Vector3.Angle(vector, forward);
		if (Vector3.Cross(forward, vector).y < 0f)
		{
			num *= -1f;
		}
		_tarRot = new Vector3(0f, _oriRot.y + num, 0f);
		if (_rotaTrans.localEulerAngles.y == _tarRot.y)
		{
			return 0f;
		}
		Debug.Log("-------------------------TarRot.y=" + _tarRot.y);
		Debug.Log(forward);
		Debug.Log(vector);
		Debug.Log(_oriRot);
		Debug.Log(_tarRot);
		_startRotationTime = Time.time;
		_durTime = Mathf.Abs(_tarRot.y - _oriRot.y) / 20f;
		_tar = tar;
		_startRotation = true;
		_startRotationBack = false;
		return _durTime;
	}

	public void EndTarget(float fDur)
	{
		_startRotation = false;
		_startRotationBack = true;
		_startRotationTime = Time.time;
		_tarRot = _oriRot;
	}
}
