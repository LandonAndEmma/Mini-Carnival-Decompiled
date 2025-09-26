using UnityEngine;

public class RPGGhostWarriorV : TBaseEntity
{
	[SerializeField]
	private Animation _ani;

	[SerializeField]
	private Transform _transUICamera;

	private static RPGGhostWarriorV _instance;

	public static RPGGhostWarriorV Instance
	{
		get
		{
			return _instance;
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		_instance = this;
	}

	protected new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
	}

	protected new void Update()
	{
	}

	public override bool HandleMessage(TTelegram msg)
	{
		bool result = false;
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5024)
		{
			result = true;
			_ani.Play();
			Object obj = Resources.Load("Particle/effect/Skill/RPG_Samurai_Attack/RPG_Samurai_RED");
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj) as GameObject;
				gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				gameObject.transform.parent = _transUICamera;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				Object.DestroyObject(gameObject, 1f);
			}
		}
		return result;
	}
}
