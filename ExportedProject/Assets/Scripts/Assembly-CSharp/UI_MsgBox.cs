using System;
using System.Reflection;
using UnityEngine;

public class UI_MsgBox : MonoBehaviour
{
	public delegate void BeforeDestroyCallHandler(string param);

	protected int nMsgType;

	public string param = string.Empty;

	[SerializeField]
	protected TUILabel themeLabel;

	[SerializeField]
	protected TUILabel furtherLabel;

	[SerializeField]
	protected TUILabel singleLineLabel;

	protected event BeforeDestroyCallHandler ProceYesHandler;

	protected event BeforeDestroyCallHandler ProceNoHandler;

	public void AddProceYesHandler(BeforeDestroyCallHandler handler)
	{
		AddProceYesHandler(handler, true);
	}

	public void AddProceYesHandler(BeforeDestroyCallHandler handler, bool bClear)
	{
		if (bClear)
		{
			ClearProceYesHandler();
		}
		this.ProceYesHandler = (BeforeDestroyCallHandler)Delegate.Combine(this.ProceYesHandler, handler);
	}

	public void ClearProceYesHandler()
	{
		RemoveEvent(this, "ProceYesHandler");
	}

	public void AddProceNoHandler(BeforeDestroyCallHandler handler)
	{
		AddProceNoHandler(handler, true);
	}

	public void AddProceNoHandler(BeforeDestroyCallHandler handler, bool bClear)
	{
		if (bClear)
		{
			ClearProceNoHandler();
		}
		this.ProceNoHandler = (BeforeDestroyCallHandler)Delegate.Combine(this.ProceNoHandler, handler);
	}

	public void ClearProceNoHandler()
	{
		RemoveEvent(this, "ProceNoHandler");
	}

	public void CallYesHandler()
	{
		if (this.ProceYesHandler != null)
		{
			this.ProceYesHandler(string.Empty);
		}
	}

	public void CallNoHandler()
	{
		if (this.ProceNoHandler != null)
		{
			this.ProceNoHandler(string.Empty);
		}
	}

	public void CallYesHandler(string param)
	{
		if (this.ProceYesHandler != null)
		{
			this.ProceYesHandler(param);
		}
	}

	public void CallNoHandler(string param)
	{
		if (this.ProceNoHandler != null)
		{
			this.ProceNoHandler(param);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public virtual void MsgBox(string strTheme, string strFurther, int nId)
	{
		MsgBox(strTheme, strFurther, nId, string.Empty);
	}

	public virtual void MsgBox(string strTheme, string strFurther, int nId, string strParam)
	{
		if (themeLabel != null)
		{
			themeLabel.Text = strTheme;
		}
		if (furtherLabel != null)
		{
			furtherLabel.Text = strFurther;
		}
		if (strTheme.Length == 0 || strFurther.Length == 0)
		{
			if (themeLabel != null)
			{
				themeLabel.Text = string.Empty;
			}
			if (furtherLabel != null)
			{
				furtherLabel.Text = string.Empty;
			}
			if (singleLineLabel != null)
			{
				singleLineLabel.Text = ((strTheme.Length != 0) ? strTheme : strFurther);
			}
		}
		else if (singleLineLabel != null)
		{
			singleLineLabel.Text = string.Empty;
		}
		nMsgType = nId;
		if (base.animation != null)
		{
			base.animation.Play();
		}
	}

	public virtual void DestroyBox()
	{
		if (base.animation != null)
		{
			base.animation.Stop();
		}
		UnityEngine.Object.Destroy(base.gameObject);
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

	protected void BtnOpenLight(TUIControl control)
	{
		UI_ButtonLight component = control.gameObject.GetComponent<UI_ButtonLight>();
		if (component != null)
		{
			component.LightOn();
		}
	}

	protected void BtnCloseLight(TUIControl control)
	{
		UI_ButtonLight component = control.gameObject.GetComponent<UI_ButtonLight>();
		if (component != null)
		{
			component.LightOff();
		}
	}
}
