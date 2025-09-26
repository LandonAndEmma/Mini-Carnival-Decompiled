using UnityEngine;

public class RPGPharaohAbsorb : MonoBehaviour
{
	private float fCloseSpeed;

	private Vector3 _closedir = Vector3.zero;

	private Vector3 _oriPos = Vector3.zero;

	public float gravity = 10f;

	public float vJump = 6f;

	private RPGEntity _aim;

	private float _fDur;

	private float fStartFlyTime;

	private bool bFlying;

	private float _hp;

	private void Start()
	{
	}

	public int ClosetoAim()
	{
		_closedir.y -= gravity * Time.deltaTime;
		base.transform.position += _closedir * Time.deltaTime;
		return 0;
	}

	private void Update()
	{
		if (bFlying)
		{
			if (Time.time - fStartFlyTime <= _fDur)
			{
				ClosetoAim();
				return;
			}
			GameObject original = Resources.Load("Particle/effect/Skill/RPG_Pharaoh/RPG_Pharaoh_Bf") as GameObject;
			Vector3 position = _aim.transform.position;
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.Euler(Vector3.zero)) as GameObject;
			gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			_aim.GetAttackSkill().AddOnlyIconBuff("Particle/effect/Skill/RPG_Icon_Attribute_bonus/RPG_Icon_Attribute_bonus");
			Object.DestroyObject(gameObject, 1f);
			bFlying = false;
			Object.DestroyObject(base.gameObject);
		}
	}

	public float FlyToAim(RPGEntity aim, float hp, float dur)
	{
		_aim = aim;
		_hp = hp;
		bFlying = true;
		fStartFlyTime = Time.time;
		_fDur = dur;
		Vector3 position = aim.transform.position;
		position = new Vector3(position.x, 1.5f, position.z);
		float num = Vector3.Distance(position, base.transform.position);
		_fDur = vJump * 2f / gravity;
		fCloseSpeed = num / _fDur;
		_closedir = position - base.transform.position;
		_closedir.Normalize();
		_closedir *= fCloseSpeed;
		_closedir.y = vJump;
		return _fDur;
	}
}
