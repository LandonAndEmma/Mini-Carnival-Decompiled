using UnityEngine;

public class RPGCharacter : MonoBehaviour
{
	public enum ERPGPart
	{
		head = 1,
		body = 2,
		leg = 3,
		decoration = 4
	}

	public enum EDecorationPart
	{
		head_top = 0,
		head_front = 1,
		head_back = 2,
		head_left = 3,
		head_right = 4,
		chest_front = 5,
		chest_back = 6,
		hand_left = 7,
		hand_right = 8,
		MAX = 9
	}

	public enum EEquipRenderPart
	{
		head = 0,
		body = 1,
		legs = 2,
		MAX = 3
	}

	[SerializeField]
	private RPGEntity _entity;

	[SerializeField]
	private Transform[] _decorationNodes = new Transform[9];

	[SerializeField]
	private Renderer[] _equipRenders = new Renderer[3];

	public Transform GetDecoByPart(EDecorationPart part)
	{
		return _decorationNodes[(int)part];
	}

	public void EquipDecoration(EDecorationPart part, Transform decoTrans)
	{
		decoTrans.parent = _decorationNodes[(int)part];
		decoTrans.localPosition = Vector3.zero;
		decoTrans.localRotation = Quaternion.Euler(Vector3.zero);
		decoTrans.localScale = new Vector3(1f, 1f, 1f);
	}

	public void EquipTexture(ERPGPart part, Texture2D tex)
	{
		_equipRenders[(int)(part - 1)].material.mainTexture = tex;
	}

	public void TriggerAttackEffect_Launch()
	{
		RPGEntity curBoutingEntity = RPGRefree.Instance.GetCurBoutingEntity();
		if (curBoutingEntity != null && curBoutingEntity == _entity)
		{
			_entity.TriggerAttackEffect_Launch();
		}
	}

	public void TriggerAttackEffect()
	{
		RPGEntity curBoutingEntity = RPGRefree.Instance.GetCurBoutingEntity();
		if (curBoutingEntity != null && curBoutingEntity == _entity)
		{
			_entity.TriggerAttackEffect();
		}
	}

	public void TriggerRealAttack(float delay)
	{
		RPGEntity curBoutingEntity = RPGRefree.Instance.GetCurBoutingEntity();
		if (curBoutingEntity != null && curBoutingEntity == _entity)
		{
			_entity.TriggerAttack(delay);
		}
	}

	public void ShakeScreen()
	{
		TMessageDispatcher.Instance.DispatchMsg(-1, RPGGhostWarriorV.Instance.GetInstanceID(), 5024, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
