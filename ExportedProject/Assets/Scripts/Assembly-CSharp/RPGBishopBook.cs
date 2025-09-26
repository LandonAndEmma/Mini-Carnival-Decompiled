using UnityEngine;

public class RPGBishopBook : TBaseEntity
{
	[SerializeField]
	private Animation _ani;

	[SerializeField]
	private Transform _transUICamera;

	private static RPGBishopBook _instance;

	public static RPGBishopBook Instance
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
		AnimationState animationState = _ani["Idle01"];
		animationState.layer = -1;
	}

	protected new void OnDisable()
	{
		base.OnDisable();
		_instance = null;
	}

	protected new void Update()
	{
	}

	public void PlayBuffAni()
	{
		_ani.Play("Buff01");
	}

	public void PlayAttackAni()
	{
		_ani.Play("Attack01");
	}

	public override bool HandleMessage(TTelegram msg)
	{
		bool result = false;
		switch (msg._nMsgId)
		{
		case 5025:
			result = true;
			_ani.Play("Buff01");
			break;
		case 5026:
			result = true;
			_ani.Play("Attack01");
			break;
		}
		return result;
	}
}
