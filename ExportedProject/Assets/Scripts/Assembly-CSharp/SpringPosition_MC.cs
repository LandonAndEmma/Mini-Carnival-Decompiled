using UnityEngine;

public class SpringPosition_MC : IgnoreTimeScale
{
	public delegate void OnFinished(SpringPosition_MC spring);

	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	public bool worldSpace;

	public bool ignoreTimeScale;

	public GameObject eventReceiver;

	public string callWhenFinished;

	public OnFinished onFinished;

	private Transform mTrans;

	private float mThreshold;

	private float _fAmu;

	private void Start()
	{
		mTrans = base.transform;
	}

	private void Update()
	{
		float num = ((!ignoreTimeScale) ? Time.deltaTime : UpdateRealTimeDelta());
		if (worldSpace)
		{
			if (mThreshold == 0f)
			{
				mThreshold = (target - mTrans.position).magnitude * 0.001f;
			}
			float value = _fAmu / strength;
			value = Mathf.Clamp(value, 0f, 1f);
			mTrans.position = Vector3.Lerp(mTrans.position, target, value);
			if (mThreshold >= (target - mTrans.position).magnitude)
			{
				mTrans.position = target;
				if (onFinished != null)
				{
					onFinished(this);
				}
				if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
				{
					eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
		else
		{
			if (mThreshold == 0f)
			{
				mThreshold = (target - mTrans.localPosition).magnitude * 0.001f;
			}
			mTrans.localPosition = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, num);
			if (mThreshold >= (target - mTrans.localPosition).magnitude)
			{
				mTrans.localPosition = target;
				if (onFinished != null)
				{
					onFinished(this);
				}
				if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
				{
					eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				base.enabled = false;
			}
		}
		_fAmu += num;
	}

	public static SpringPosition_MC Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition_MC springPosition_MC = go.GetComponent<SpringPosition_MC>();
		if (springPosition_MC == null)
		{
			springPosition_MC = go.AddComponent<SpringPosition_MC>();
		}
		springPosition_MC.target = pos;
		springPosition_MC.strength = strength;
		springPosition_MC.onFinished = null;
		springPosition_MC._fAmu = 0f;
		if (!springPosition_MC.enabled)
		{
			springPosition_MC.mThreshold = 0f;
			springPosition_MC.enabled = true;
		}
		return springPosition_MC;
	}
}
