using System.Collections.Generic;
using UnityEngine;

public class RPGTTactic : MonoBehaviour
{
	[SerializeField]
	private int _priorityLevel;

	[SerializeField]
	private bool _spec;

	[SerializeField]
	protected List<int> _buffIdLst = new List<int>();

	[SerializeField]
	protected RPGTeam _team;

	[SerializeField]
	protected RPGRefree _refree;

	protected RPGTacticUnit _tacticUnit;

	protected List<RPGEntity> _tacticOwner = new List<RPGEntity>();

	public int PriorityLevel
	{
		get
		{
			return _priorityLevel;
		}
		set
		{
			_priorityLevel = value;
		}
	}

	public bool Spec
	{
		get
		{
			return _spec;
		}
		set
		{
			_spec = value;
		}
	}

	public RPGTacticUnit TacticUnit
	{
		get
		{
			return _tacticUnit;
		}
		set
		{
			_tacticUnit = value;
		}
	}

	public List<RPGEntity> TacticOwner
	{
		get
		{
			return _tacticOwner;
		}
	}

	public virtual float InitTactic()
	{
		Debug.Log("InitTactic");
		if (MatchAvtiveCondition())
		{
			Debug.Log("MatchAvtiveCondition!");
			return ActiveTactic();
		}
		return 0f;
	}

	public virtual void ConditionChanged()
	{
		if (MatchUnavtiveCondition())
		{
			UnactiveTactic();
		}
	}

	public virtual void ConditionChanged_Enemy()
	{
	}

	public virtual bool MatchAvtiveCondition()
	{
		return false;
	}

	public virtual bool MatchUnavtiveCondition()
	{
		return false;
	}

	public virtual float ActiveTactic()
	{
		return 0f;
	}

	public virtual int UnactiveTactic()
	{
		return 0;
	}

	public bool IsExistTacticOwner(RPGEntity entity)
	{
		return TacticOwner.Contains(entity);
	}

	protected void Awake()
	{
		if (_team == null)
		{
			_team = GetComponent<RPGTeam>();
		}
		if (_refree == null)
		{
			_refree = base.transform.parent.GetComponent<RPGRefree>();
		}
	}

	protected void OnEnable()
	{
	}

	protected void OnDisable()
	{
	}
}
