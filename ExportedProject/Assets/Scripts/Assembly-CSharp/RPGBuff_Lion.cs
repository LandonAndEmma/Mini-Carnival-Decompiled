using UnityEngine;

public class RPGBuff_Lion : RPGTBuff
{
	[SerializeField]
	private bool _bActiveNextEffect;

	[SerializeField]
	private float _fStartTime;

	[SerializeField]
	private float _fDelayTime;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 18;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5003 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}

	protected override void InitBufferEffect()
	{
		if (RPGGlobalData.Instance.BuffEffectPool._dict.ContainsKey(base.ConfId))
		{
			RPGBuffEffectUnit rPGBuffEffectUnit = RPGGlobalData.Instance.BuffEffectPool._dict[base.ConfId];
			Object obj = Resources.Load(rPGBuffEffectUnit.EffectPath);
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj) as GameObject;
				gameObject.transform.parent = base.RPGEntityOwner.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				AddSpecBuffEffectObj(gameObject);
			}
			Object obj2 = Resources.Load(rPGBuffEffectUnit.EffectPath_cast);
			if (obj2 != null)
			{
				GameObject gameObject2 = Object.Instantiate(obj2, base.RPGEntityOwner.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject2.transform.parent = base.RPGEntityOwner.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
				float effectDurTime = GetEffectDurTime(gameObject2);
				Object.Destroy(gameObject2, effectDurTime);
				_bActiveNextEffect = true;
				_fStartTime = Time.time;
				_fDelayTime = 0.3f;
				Debug.LogWarning(_bActiveNextEffect);
			}
		}
	}

	protected new void Update()
	{
		base.Update();
		if (!_bActiveNextEffect || !(Time.time - _fStartTime >= _fDelayTime))
		{
			return;
		}
		if (RPGGlobalData.Instance.BuffEffectPool._dict.ContainsKey(base.ConfId))
		{
			RPGBuffEffectUnit rPGBuffEffectUnit = RPGGlobalData.Instance.BuffEffectPool._dict[base.ConfId];
			Debug.LogWarning(rPGBuffEffectUnit.EffectPath_receive);
			Object obj = Resources.Load(rPGBuffEffectUnit.EffectPath_receive);
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj, base.RPGEntityOwner.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.parent = base.RPGEntityOwner.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				float effectDurTime = GetEffectDurTime(gameObject);
				Object.Destroy(gameObject, effectDurTime);
			}
		}
		_bActiveNextEffect = false;
	}
}
