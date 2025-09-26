using UnityEngine;

public class UIDoodle_BrushSel : MonoBehaviour
{
	private int selBrush = 4;

	public int SelBrush
	{
		get
		{
			return selBrush;
		}
		set
		{
			selBrush = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	protected UIMessageHandler GetTUIMessageHandler()
	{
		Transform parent = base.gameObject.transform.parent;
		if (parent != null)
		{
			Transform parent2 = parent.parent;
			if (parent2 != null)
			{
				Transform parent3 = parent2.parent;
				if (parent3 != null)
				{
					return parent3.GetComponent<UIMessageHandler>();
				}
			}
		}
		return null;
	}

	public void HandleEventButton_Brush1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			SelBrush = 1;
			UIDoodle uIDoodle = (UIDoodle)GetTUIMessageHandler();
			uIDoodle.ProcessBrushChanged(SelBrush);
		}
	}

	public void HandleEventButton_Brush2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			SelBrush = 2;
			UIDoodle uIDoodle = (UIDoodle)GetTUIMessageHandler();
			uIDoodle.ProcessBrushChanged(SelBrush);
		}
	}

	public void HandleEventButton_Brush3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			SelBrush = 3;
			UIDoodle uIDoodle = (UIDoodle)GetTUIMessageHandler();
			uIDoodle.ProcessBrushChanged(SelBrush);
		}
	}

	public void HandleEventButton_Brush4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			SelBrush = 4;
			UIDoodle uIDoodle = (UIDoodle)GetTUIMessageHandler();
			uIDoodle.ProcessBrushChanged(SelBrush);
		}
	}
}
