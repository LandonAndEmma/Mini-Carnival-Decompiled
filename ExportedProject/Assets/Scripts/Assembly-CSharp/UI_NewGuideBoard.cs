using System;
using System.Reflection;
using UnityEngine;

public class UI_NewGuideBoard : MonoBehaviour
{
	public delegate void BtnClickProcess();

	[SerializeField]
	private TUILabel _labelDes;

	[SerializeField]
	private Animation _aniCmp;

	[SerializeField]
	private Transform _transArrow;

	public event BtnClickProcess ProcessBtnClick;

	public void ProcessEvent()
	{
		if (this.ProcessBtnClick != null)
		{
			this.ProcessBtnClick();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetArrow(Vector2 pos, Vector3 rot)
	{
		if (_transArrow != null)
		{
			_transArrow.localPosition = new Vector3(pos.x, pos.y, 0f);
			_transArrow.localRotation = Quaternion.Euler(rot);
		}
	}

	public void InitLabel(Vector2 pos, string strId)
	{
		base.gameObject.SetActive(true);
		_labelDes.TextID = strId;
		base.transform.localPosition = new Vector3(pos.x, pos.y, -400f);
		_aniCmp.Play();
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
	}

	public void EndLabel()
	{
		base.gameObject.SetActive(false);
	}

	public void AddProceHandler(BtnClickProcess handler)
	{
		AddProceHandler(handler, true);
	}

	public void AddProceHandler(BtnClickProcess handler, bool bClear)
	{
		if (bClear)
		{
			ClearProceHandler();
		}
		this.ProcessBtnClick = (BtnClickProcess)Delegate.Combine(this.ProcessBtnClick, handler);
	}

	public void ClearProceHandler()
	{
		RemoveEvent(this, "ProcessBtnClick");
	}

	public static void RemoveEvent<T>(T c, string name)
	{
		Delegate[] objectEventList = GetObjectEventList(c, name);
		if (objectEventList == null)
		{
			return;
		}
		Delegate[] array = objectEventList;
		foreach (Delegate handler in array)
		{
			if (typeof(T).GetEvent(name) != null)
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
}
