using System;
using UnityEngine;

public class COMA_Creation : TBaseEntity
{
	[NonSerialized]
	public CharacterController cCtl;

	[NonSerialized]
	public float bodyHeight = 1.6f;

	[NonSerialized]
	public CreationKind creationKind;

	[NonSerialized]
	public string gid = string.Empty;

	[NonSerialized]
	public int id;

	[NonSerialized]
	public int sitIndex;

	[NonSerialized]
	public GameObject rt3DObj;

	[NonSerialized]
	public string nickname = "player";

	[NonSerialized]
	public int lv;

	[NonSerialized]
	public int exp;

	[NonSerialized]
	public int rankscore;

	[NonSerialized]
	private int _score;

	[NonSerialized]
	public int expGet;

	[NonSerialized]
	public int goldGet;

	[NonSerialized]
	public int crystalGet;

	[NonSerialized]
	public float HP = 100f;

	private float m_hp;

	[NonSerialized]
	public float AP;

	private float m_ap;

	[NonSerialized]
	public float speedRun = 3.5f;

	[NonSerialized]
	public float speedJump = 4f;

	[NonSerialized]
	public Vector3 moveCur = Vector3.zero;

	[NonSerialized]
	public Vector3 movePsv = Vector3.zero;

	[NonSerialized]
	public int itemSelected;

	private bool isFrozen;

	protected int icefromID;

	protected float iceAP;

	protected float icePush;

	private bool isConfused;

	private bool isVenom;

	public bool IsInvincible;

	public bool IsGrounded;

	protected bool IsHidden;

	public bool IsInverted;

	public int score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			if (creationKind == CreationKind.Player)
			{
				if (UIIngame_DefendUI.Instance != null)
				{
					UIIngame_DefendUI.Instance._info.Num = score;
				}
				if (UIInGame_PlayersUIMgr.Instance != null)
				{
					UIInGame_PlayersUIMgr.Instance._testData[sitIndex].Num = _score;
				}
				if (UIInGame_LabyrinthUIMgr.Instance != null)
				{
					UIInGame_LabyrinthUIMgr.Instance._testData[sitIndex].GoldNum = _score;
				}
				if (UIIngame_BloodUI.Instance != null && id == COMA_Network.Instance.TNetInstance.Myself.Id)
				{
					UIIngame_BloodUI.Instance._info.NumKill = _score;
				}
			}
		}
	}

	public float hp
	{
		get
		{
			return m_hp;
		}
		set
		{
			if (isFrozen)
			{
				return;
			}
			m_hp = Mathf.Clamp(value, 0f, HP);
			if (creationKind == CreationKind.Player)
			{
				if (UIIngame_DefendUI.Instance != null)
				{
					UIIngame_DefendUI.Instance._info.HP = hp / HP;
				}
				if (UIInGameHungers_PlayersMgr.Instance != null)
				{
					UIInGameHungers_PlayersMgr.Instance._testData[sitIndex].HP = hp / HP;
				}
				if (UIIngame_BloodUI.Instance != null && id == COMA_Network.Instance.TNetInstance.Myself.Id)
				{
					UIIngame_BloodUI.Instance._info.HP = hp / HP;
				}
			}
		}
	}

	public bool IsDead
	{
		get
		{
			return m_hp < 1f;
		}
	}

	public float ap
	{
		get
		{
			return m_ap;
		}
		set
		{
			m_ap = value;
		}
	}

	public bool IsVenom
	{
		get
		{
			return isVenom;
		}
		set
		{
			isVenom = value;
		}
	}

	public bool IsConfused
	{
		get
		{
			return isConfused;
		}
		set
		{
			isConfused = value;
		}
	}

	public bool IsFrozen
	{
		get
		{
			return isFrozen;
		}
		set
		{
			isFrozen = value;
		}
	}

	protected void Awake()
	{
	}

	protected void Start()
	{
		cCtl = base.gameObject.GetComponent<CharacterController>();
		if (cCtl != null)
		{
			bodyHeight = cCtl.height;
		}
		hp = HP;
		ap = AP;
	}

	protected new void OnEnable()
	{
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	public virtual void OnHitOther(int fromID, int toID, string bulletName, float bulletAP, Vector3 push)
	{
	}

	public virtual void OnHurt(COMA_PlayerSelf from, string bulletName, float bulletAP, Vector3 push)
	{
	}

	public virtual bool ReceiveHurt(int fromID, int bulletID, float bulletAP, Vector3 push)
	{
		return false;
	}

	protected void InvincibleStart(float lastTime)
	{
		IsInvincible = true;
		SceneTimerInstance.Instance.Remove(InvincibleEnd);
		SceneTimerInstance.Instance.Add(lastTime, InvincibleEnd);
	}

	public bool InvincibleEnd()
	{
		IsInvincible = false;
		return false;
	}

	protected void GroundedStart(float lastTime)
	{
		IsGrounded = true;
		SceneTimerInstance.Instance.Add(lastTime, GroundedEnd);
	}

	public bool GroundedEnd()
	{
		IsGrounded = false;
		return false;
	}

	public bool VenomRecover()
	{
		IsVenom = false;
		return false;
	}

	public bool ConfusedRecover()
	{
		IsConfused = false;
		return false;
	}

	public virtual bool FrozenBroken()
	{
		IsFrozen = false;
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_Buff_IceBroken")) as GameObject;
		gameObject.transform.position = base.transform.position + Vector3.up * 0.5f;
		UnityEngine.Object.DestroyObject(gameObject, 2f);
		Transform playerNodeTrs = COMA_Scene.Instance.playerNodeTrs;
		for (int i = 0; i < playerNodeTrs.childCount; i++)
		{
			Transform child = playerNodeTrs.GetChild(i);
			COMA_Creation component = child.GetComponent<COMA_Creation>();
			if (!(component != null))
			{
				continue;
			}
			Vector3 vector = child.position + Vector3.up * component.bodyHeight * 0.5f;
			Vector3 vector2 = child.position + Vector3.up * component.bodyHeight * 0.7f;
			Vector3 vector3 = base.transform.position + base.transform.up * bodyHeight * 0.5f + base.transform.forward * 0.5f;
			if (Vector3.SqrMagnitude(vector - vector3) < 4f)
			{
				Vector3 push = (vector2 - vector3).normalized * icePush;
				if (component.creationKind == CreationKind.Player)
				{
					OnHitOther(icefromID, component.id, string.Empty, iceAP, push);
				}
				else if (component.creationKind == CreationKind.Enemy)
				{
					component.OnHurt(null, string.Empty, iceAP, push);
				}
			}
		}
		InvincibleStart(2f);
		return false;
	}

	public virtual void Transmission()
	{
		movePsv = Vector3.zero;
	}

	public bool InvertedRecover()
	{
		IsInverted = false;
		return false;
	}
}
