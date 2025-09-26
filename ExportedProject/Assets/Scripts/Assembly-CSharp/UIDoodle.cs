using UnityEngine;

public class UIDoodle : UIMessageHandler
{
	public BodyPaint bodyPaint;

	private Texture2D[] texs;

	public UIDoodle_ColourContainer colourContainer;

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventContainer_Move(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == TUIScrollList.CommandMove)
		{
			Debug.Log("HandleEventContainer_Move-CommandMove");
			colourContainer.RefreshCurSelSlot(null, control);
		}
	}

	public void HandleEventButton_undo(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_undo-CommandClick");
			bodyPaint.Undo();
		}
	}

	public void HandleEventButton_brush(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_brush-CommandClick");
			bodyPaint.Full();
		}
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_aniControl.PlayExitAni("UI.DoodleMgr");
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void ProcessBrushChanged(int nCurBrushId)
	{
		Debug.Log("Selected Brush:" + nCurBrushId);
		bodyPaint.paintRadius = nCurBrushId;
	}

	public void ProcessColourChanged(TUIControl control, int eventType, float wparam, float lparam, object data, UIDoodle_ColourBoxSlot slot)
	{
		colourContainer.RefreshCurSelSlot(slot, control);
		Debug.Log("HandleEventButton_Colour-Sel Color:" + ((TUIButtonClick)control).m_NormalObj.GetComponent<TUIMeshSprite>().color);
	}
}
