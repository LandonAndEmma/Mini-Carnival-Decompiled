using UnityEngine;

public class UI_EditZoomSliderMgr : MonoBehaviour
{
	[SerializeField]
	private Transform _btnPointer;

	[SerializeField]
	private UIDoodle1 _msgProce;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void RefreshPointerPos(float fX, float fY, TUIControl control)
	{
		fY = Mathf.Clamp01(fY);
		Vector3 localPosition = _btnPointer.localPosition;
		localPosition.y = (fY - 0.5f) * 140f;
		_btnPointer.localPosition = localPosition;
		_msgProce.NotifyZoomMode(Mathf.Abs(fY - 1f));
	}

	public void InitPointerPos(float f)
	{
		f = Mathf.Clamp01(f);
		RefreshPointerPos(0f, Mathf.Abs(f - 1f), null);
	}

	public void HandleEventButton_Slider(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			RefreshPointerPos(wparam, lparam, control);
			break;
		case 4:
			RefreshPointerPos(wparam, lparam, control);
			break;
		}
	}
}
