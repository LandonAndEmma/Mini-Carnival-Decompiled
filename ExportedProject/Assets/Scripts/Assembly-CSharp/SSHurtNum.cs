using System.Collections.Generic;
using SSFont;
using UnityEngine;

public class SSHurtNum : MonoBehaviour
{
	private struct HealStruct
	{
		public float _num;

		public Transform _trans;

		public float _startTime;

		public bool _bCri;

		public HealStruct(Transform tran, float num, float t)
		{
			_trans = tran;
			_startTime = t;
			_num = num;
			_bCri = false;
		}

		public HealStruct(Transform tran, float num, float t, bool bC)
		{
			_trans = tran;
			_startTime = t;
			_num = num;
			_bCri = bC;
		}
	}

	private static SSHurtNum _instance;

	public Material m_font_mat;

	public TextAsset m_font_cfg;

	public bool m_bTest;

	public Transform _camTrans;

	public AnimationClip _aniClip;

	public RuntimeAnimatorController _animatorCtr;

	public RuntimeAnimatorController _animatorCtr_Suck;

	public RuntimeAnimatorController _animatorCtr_Crit;

	private Dictionary<Transform, List<HealStruct>> _dictHeal = new Dictionary<Transform, List<HealStruct>>();

	private Dictionary<Transform, List<HealStruct>> _dictHit = new Dictionary<Transform, List<HealStruct>>();

	public static SSHurtNum Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void HitCriticalFont(float num, Transform trans)
	{
		HpChangeFont(num, trans, true, true);
	}

	public void HitNormalFont(float num, Transform trans)
	{
		List<HealStruct> list = null;
		if (_dictHit.ContainsKey(trans))
		{
			if (_dictHit[trans] == null)
			{
				_dictHit[trans] = new List<HealStruct>();
			}
			float t = Time.time;
			if (_dictHit[trans].Count > 0)
			{
				t = _dictHit[trans][_dictHit[trans].Count - 1]._startTime + 0.2f;
			}
			_dictHit[trans].Add(new HealStruct(trans, num, t));
		}
		else
		{
			list = new List<HealStruct>();
			list.Add(new HealStruct(trans, num, Time.time));
			_dictHit.Add(trans, list);
		}
	}

	public void HitMiss(Transform trans)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Miss);
		string content = "Miss";
		Color c = new Color(1f, 1f, 1f);
		HitFont(content, trans, c);
	}

	private void Update()
	{
		foreach (Transform key in _dictHeal.Keys)
		{
			List<HealStruct> list = _dictHeal[key];
			for (int i = 0; i < list.Count; i++)
			{
				HealStruct healStruct = list[i];
				if (!(Time.time >= healStruct._startTime))
				{
					continue;
				}
				HpChangeFont(healStruct._num, healStruct._trans, false, false);
				if (_dictHit.ContainsKey(healStruct._trans))
				{
					List<HealStruct> list2 = _dictHit[healStruct._trans];
					for (int j = 0; j < list2.Count; j++)
					{
						HealStruct value = list2[j];
						value._startTime += 0.2f;
						list2[j] = value;
					}
				}
				list.RemoveAt(i);
				break;
			}
		}
		foreach (Transform key2 in _dictHit.Keys)
		{
			List<HealStruct> list3 = _dictHit[key2];
			for (int k = 0; k < list3.Count; k++)
			{
				HealStruct healStruct2 = list3[k];
				if (Time.time >= healStruct2._startTime)
				{
					HpChangeFont(healStruct2._num, healStruct2._trans, false, true);
					list3.RemoveAt(k);
					break;
				}
			}
		}
	}

	public void HealingFont(float num, Transform trans, bool bEffect)
	{
		if (bEffect)
		{
			Object original = Resources.Load("Particle/effect/Skill/RPG_BUFF_Undead/RPG_BUFF_Undead_Hp");
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.parent = trans;
			gameObject.transform.localPosition = Vector3.zero;
			Object.DestroyObject(gameObject, 1f);
		}
		List<HealStruct> list = null;
		if (_dictHeal.ContainsKey(trans))
		{
			if (_dictHeal[trans] == null)
			{
				_dictHeal[trans] = new List<HealStruct>();
			}
			float t = Time.time;
			if (_dictHeal[trans].Count > 0)
			{
				t = _dictHeal[trans][_dictHeal[trans].Count - 1]._startTime + 0.2f;
			}
			_dictHeal[trans].Add(new HealStruct(trans, num, t));
		}
		else
		{
			list = new List<HealStruct>();
			list.Add(new HealStruct(trans, num, Time.time));
			_dictHeal.Add(trans, list);
		}
	}

	public void HealingFont(float num, Transform trans)
	{
		HealingFont(num, trans, true);
	}

	protected void HpChangeFont(float num, Transform trans, bool critical)
	{
		HpChangeFont(num, trans, critical, true);
	}

	protected void HpChangeFont(float num, Transform trans, bool critical, bool hurt)
	{
		string content = Mathf.FloorToInt(num).ToString();
		Color color = (hurt ? ((!critical) ? new Color(1f, 0f, 0f) : new Color(1f, 0f, 0f)) : new Color(0f, 1f, 0f));
		Debug.Log("hurt=" + hurt + "  c=" + color);
		HitFont(content, trans, color, critical);
	}

	protected void HitFont(string content, Transform trans, Color c, bool critical)
	{
		Debug.LogWarning("critical=" + critical);
		GameObject gameObject = new GameObject();
		gameObject.transform.position = trans.position;
		gameObject.transform.rotation = trans.rotation;
		gameObject.transform.localScale = trans.localScale;
		GameObject gameObject2 = new GameObject("Blood2");
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 2f, 0f);
		gameObject2.transform.rotation = _camTrans.rotation;
		SSFont3D sSFont3D = gameObject2.AddComponent<SSFont3D>();
		GameObject gameObject3 = null;
		GameObject gameObject4 = null;
		if (critical)
		{
			Object obj = Resources.Load("Particle/effect/Skill/RPG_BUFF_Crit/RPG_Crit");
			if (obj != null)
			{
				gameObject3 = Object.Instantiate(obj) as GameObject;
				Object.DestroyObject(gameObject3, 2f);
			}
			Object obj2 = Resources.Load("FBX/Common/Crit");
			if (obj2 != null)
			{
				gameObject4 = Object.Instantiate(obj2) as GameObject;
			}
		}
		sSFont3D.SetFont(m_font_mat, m_font_cfg);
		sSFont3D.SetColor(c);
		sSFont3D.SetString(content, -0.4f, 1f, SSFont.BoardType.HitNumberJump);
		GameObject gameObject5 = new GameObject("aniLayer");
		gameObject5.transform.parent = gameObject.transform;
		gameObject5.transform.localPosition = Vector3.zero;
		gameObject5.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject5.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject2.transform.parent = gameObject5.transform;
		if (gameObject3 != null)
		{
			gameObject3.transform.parent = gameObject.transform;
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.transform.localRotation = Quaternion.Euler(Vector3.zero);
			gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (gameObject4 != null)
		{
			gameObject4.transform.parent = gameObject5.transform;
			gameObject4.transform.localScale = new Vector3(content.Length / 2, 1f, 1f);
			gameObject4.transform.localPosition = new Vector3(0f, 2f, -0.1f);
			gameObject4.transform.rotation = _camTrans.rotation;
		}
		Animator animator = gameObject5.AddComponent<Animator>();
		gameObject5.AddComponent<RPGHurtAniCtrl>();
		animator.runtimeAnimatorController = ((!critical) ? _animatorCtr : _animatorCtr_Crit);
		if (c.r == 0f)
		{
			animator.runtimeAnimatorController = _animatorCtr_Suck;
		}
	}

	protected void HitFont(string content, Transform trans, Color c)
	{
		HitFont(content, trans, c, false);
	}

	private void OnDestroy()
	{
		Debug.Log("-----------------------------------------------OnDestroy");
	}
}
