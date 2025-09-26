using System;
using System.Reflection;
using UnityEngine;

public class UIInGame_ProgressBar : MonoBehaviour
{
	public delegate void ProgressEndHandle();

	[SerializeField]
	private GameObject _bk;

	[SerializeField]
	private GameObject _fore;

	private static UIInGame_ProgressBar _instance;

	[SerializeField]
	private float _fDur;

	[SerializeField]
	private bool _bTest;

	private bool _bEnableTest;

	private float _fStartTime;

	private float _fDurTime;

	private bool _bProgressing;

	private float fLen = 212f;

	public static UIInGame_ProgressBar Instance
	{
		get
		{
			return _instance;
		}
	}

	protected event ProgressEndHandle ProceProgressEnd;

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public void AddProceProgressEndHandler(ProgressEndHandle handler)
	{
		AddProceProgressEndHandler(handler, true);
	}

	public void AddProceProgressEndHandler(ProgressEndHandle handler, bool bClear)
	{
		if (bClear)
		{
			ClearProgressEndHandler();
		}
		this.ProceProgressEnd = (ProgressEndHandle)Delegate.Combine(this.ProceProgressEnd, handler);
	}

	public void ClearProgressEndHandler()
	{
		RemoveEvent(this, "ProceProgressEnd");
	}

	public static void RemoveEvent<T>(T c, string name)
	{
		Delegate[] objectEventList = GetObjectEventList(c, name);
		if (objectEventList != null)
		{
			Delegate[] array = objectEventList;
			foreach (Delegate handler in array)
			{
				typeof(T).GetEvent(name).RemoveEventHandler(c, handler);
			}
		}
	}

	public static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
	{
		FieldInfo field = p_Object.GetType().GetField(p_EventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (field == null)
		{
			return null;
		}
		object value = field.GetValue(p_Object);
		if (value != null && value is Delegate)
		{
			Delegate obj = (Delegate)value;
			return obj.GetInvocationList();
		}
		return null;
	}

	private void Awake()
	{
		_bk.SetActive(false);
		_fore.SetActive(false);
		_bProgressing = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartProgressBar()
	{
		_bk.SetActive(true);
		_fore.SetActive(true);
	}

	public void EndProgressBar()
	{
		_bk.SetActive(false);
		_fore.SetActive(false);
	}

	public int ProgressBar(float f)
	{
		if (_fore != null)
		{
			f = Mathf.Clamp01(f);
			float x = fLen * (1f - f) * -0.5f;
			Vector3 localPosition = _fore.transform.localPosition;
			localPosition.x = x;
			_fore.transform.localPosition = localPosition;
			Vector3 localScale = _fore.transform.localScale;
			localScale.x = f;
			_fore.transform.localScale = localScale;
			if (f >= 1f)
			{
				if (this.ProceProgressEnd != null)
				{
					Debug.Log("Call Progress End!");
					this.ProceProgressEnd();
				}
				return 1;
			}
		}
		return 0;
	}
}
