using System.Collections.Generic;
using UnityEngine;

public class RPGTBuff : TBaseEntity
{
	[SerializeField]
	protected int _elapseBoutCount;

	[SerializeField]
	protected bool _isNeedShowBuffIcon = true;

	protected static int _nextValidId;

	[SerializeField]
	private int _id;

	[SerializeField]
	private int _confId;

	[SerializeField]
	protected int _mainType;

	[SerializeField]
	protected int _secondType;

	[SerializeField]
	protected bool _overlap;

	[SerializeField]
	protected int _boutCount = -1;

	[SerializeField]
	protected Texture2D _buffIcon;

	[SerializeField]
	protected RPGRoleAttr _offsetAttr = new RPGRoleAttr();

	[SerializeField]
	protected RPGEntity _rpgEntityOwner;

	private List<GameObject> _specEffectLst = new List<GameObject>();

	private List<GameObject> _specBuffEffectLst = new List<GameObject>();

	public bool IsNeedShowBuffIcon
	{
		get
		{
			return _isNeedShowBuffIcon;
		}
	}

	public int Id
	{
		get
		{
			return _id;
		}
	}

	public int ConfId
	{
		get
		{
			return _confId;
		}
		set
		{
			_confId = value;
		}
	}

	public int SecondType
	{
		get
		{
			return _secondType;
		}
		set
		{
			_secondType = value;
		}
	}

	public bool IsOverlap
	{
		get
		{
			return _overlap;
		}
		set
		{
			_overlap = value;
		}
	}

	public int BoutCount
	{
		get
		{
			return _boutCount;
		}
		set
		{
			_boutCount = value;
		}
	}

	public RPGRoleAttr OffsetAttr
	{
		get
		{
			return _offsetAttr;
		}
		set
		{
			_offsetAttr = value;
		}
	}

	public RPGEntity RPGEntityOwner
	{
		get
		{
			return _rpgEntityOwner;
		}
		set
		{
			_rpgEntityOwner = value;
			InitBufferEffect();
		}
	}

	public void SetHideBuffIconPermanent()
	{
		_isNeedShowBuffIcon = false;
	}

	protected void Awake()
	{
		_id = _nextValidId++;
	}

	public virtual RPGRoleAttr GetOffsetResult(RPGRoleAttr baseAttr)
	{
		RPGRoleAttr rPGRoleAttr = new RPGRoleAttr();
		rPGRoleAttr.Attrs[0] = 0f;
		rPGRoleAttr.Attrs[1] = 0f;
		rPGRoleAttr.Attrs[2] = 0f;
		for (int i = 3; i < 24; i++)
		{
			rPGRoleAttr.Attrs[i] = baseAttr.Attrs[i] * (OffsetAttr.Attrs[i] / 100f);
		}
		return rPGRoleAttr;
	}

	protected virtual void InitBufferEffect()
	{
		if (RPGGlobalData.Instance.BuffEffectPool._dict.ContainsKey(ConfId))
		{
			RPGBuffEffectUnit rPGBuffEffectUnit = RPGGlobalData.Instance.BuffEffectPool._dict[ConfId];
			Object obj = Resources.Load(rPGBuffEffectUnit.EffectPath);
			if (obj != null)
			{
				Debug.Log(rPGBuffEffectUnit.EffectPath);
				GameObject gameObject = Object.Instantiate(obj) as GameObject;
				gameObject.transform.parent = RPGEntityOwner.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				AddSpecBuffEffectObj(gameObject);
			}
			Object obj2 = Resources.Load(rPGBuffEffectUnit.EffectPath_cast);
			if (obj2 != null)
			{
				GameObject gameObject2 = Object.Instantiate(obj2) as GameObject;
				gameObject2.transform.parent = RPGEntityOwner.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
				Object.Destroy(gameObject2, GetEffectDurTime(gameObject2));
			}
		}
	}

	protected float GetEffectDurTime(GameObject effect)
	{
		List<float> list = new List<float>();
		ParticleSystem[] componentsInChildren = effect.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			list.Add(componentsInChildren[i].duration);
		}
		list.Sort();
		return (list.Count <= 0) ? 0f : list[list.Count - 1];
	}

	public virtual bool MatchBuffSkillAvtiveCond()
	{
		return false;
	}

	public virtual bool MatchBuffSkillUnavtiveCond()
	{
		return false;
	}

	public virtual int ActiveBuffSkill()
	{
		return 0;
	}

	public virtual int UnactiveBuffSkill()
	{
		return 0;
	}

	public virtual float NotifyBuffBegin_Bout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		_elapseBoutCount++;
		return 0f;
	}

	public virtual void NotifyBuffEnd_Bout()
	{
		if (_boutCount > 0 && _elapseBoutCount >= _boutCount)
		{
			if (RPGEntityOwner != null)
			{
				RPGEntityOwner.RemoveBuff(GetInstanceID());
			}
			Debug.Log("Buff:=" + base.name + " -------------Removed:" + Id);
			_boutCount = -1;
		}
	}

	public virtual void AttackAppend(RPGEntity enemy, int dam)
	{
	}

	public virtual void ActionLimit()
	{
	}

	public virtual void BeAttackAppend(RPGEntity enemy, int dam)
	{
	}

	public virtual float ReVerdictDeath()
	{
		return 0f;
	}

	public GameObject GetBuffEffect()
	{
		if (_specBuffEffectLst.Count <= 0)
		{
			return null;
		}
		return _specBuffEffectLst[0];
	}

	protected void AddSpecEffectObj(GameObject obj)
	{
		if (!_specEffectLst.Contains(obj))
		{
			_specEffectLst.Add(obj);
		}
	}

	protected void AddSpecBuffEffectObj(GameObject obj)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Buff_Common);
		if (!_specBuffEffectLst.Contains(obj))
		{
			_specBuffEffectLst.Add(obj);
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _specEffectLst.Count; i++)
		{
			Object.Destroy(_specEffectLst[i]);
		}
		for (int j = 0; j < _specBuffEffectLst.Count; j++)
		{
			Object.Destroy(_specBuffEffectLst[j]);
		}
	}

	protected new void Update()
	{
		base.Update();
	}
}
