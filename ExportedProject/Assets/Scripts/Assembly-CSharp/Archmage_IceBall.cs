using UnityEngine;

public class Archmage_IceBall : MonoBehaviour
{
	[SerializeField]
	private bool _flying;

	[SerializeField]
	private Transform _aim;

	private float _startFlyTime;

	private float _flySpeed;

	private Vector3 _flyDir = Vector3.zero;

	private float _flyDur = 1.13f;

	[SerializeField]
	private bool _explode;

	private Vector3 _offsetPos = new Vector3(0f, 0.8f, 0f);

	public void StartFly(Transform aim)
	{
		_flying = true;
		_aim = aim;
		_startFlyTime = Time.time;
		_flyDir = aim.position + _offsetPos - base.transform.position;
		_flyDir.Normalize();
		_flySpeed = Vector3.Distance(aim.position + _offsetPos, base.transform.position) / _flyDur;
	}

	private void Start()
	{
		RPGAttackEffectUnit rPGAttackEffectUnit = RPGGlobalData.Instance.AttackEffectPool._dict[43];
		_flyDur = rPGAttackEffectUnit.EffectDurTime;
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
			_explode = true;
		}
		else if (_explode)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Skill/Ice/Ice_hit")) as GameObject;
			gameObject.transform.position = _aim.position + _offsetPos;
			Object.Destroy(gameObject, 1f);
			Object.Destroy(base.gameObject, 1.1f);
			_explode = false;
		}
	}
}
