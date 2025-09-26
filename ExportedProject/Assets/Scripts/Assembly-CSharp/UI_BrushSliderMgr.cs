using UnityEngine;

public class UI_BrushSliderMgr : MonoBehaviour
{
	[SerializeField]
	private float[] _fValue;

	[SerializeField]
	private Transform _btnPointer;

	[SerializeField]
	private GameObject _brushIcon;

	public int _nSize = 6;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangePointerPos(int nIndex)
	{
		if (nIndex < 0)
		{
			nIndex = 0;
		}
		if (nIndex >= _fValue.Length)
		{
			nIndex = _fValue.Length - 1;
		}
		if (nIndex >= 0 && nIndex < _fValue.Length)
		{
			Vector3 localPosition = _btnPointer.localPosition;
			localPosition.x = (_fValue[nIndex] - 0.5f) * 140f;
			_btnPointer.localPosition = localPosition;
			float num = nIndex + 1;
			float num2 = num * 1.5f / 6f;
			_brushIcon.transform.localScale = new Vector3(num2, num2, 1f);
			_nSize = nIndex + 1;
		}
	}

	private void RefreshPointerPos(float fX, float fY, TUIControl control)
	{
		fX = Mathf.Clamp01(fX);
		Vector3 localPosition = _btnPointer.localPosition;
		localPosition.x = (fX - 0.5f) * 140f;
		_btnPointer.localPosition = localPosition;
		int num = 0;
		float num2 = fX;
		for (int i = 1; i < _fValue.Length; i++)
		{
			if (Mathf.Abs(fX - _fValue[i]) < num2)
			{
				num = i;
				num2 = Mathf.Abs(fX - _fValue[i]);
			}
		}
		float num3 = 0f;
		float num4 = num2 / 0.2f;
		num3 = ((!(fX < _fValue[num])) ? ((float)(num + 1) + num4) : ((float)(num + 1) - num4));
		float num5 = num3 * 1.5f / 6f;
		_brushIcon.transform.localScale = new Vector3(num5, num5, 1f);
	}

	private void EndRefreshPointerPos(float fX, float fY, TUIControl control)
	{
		fX = Mathf.Clamp01(fX);
		Debug.Log("=========================fX=" + fX);
		int num = 0;
		float num2 = fX;
		for (int i = 1; i < _fValue.Length; i++)
		{
			if (Mathf.Abs(fX - _fValue[i]) < num2)
			{
				num = i;
				num2 = Mathf.Abs(fX - _fValue[i]);
			}
		}
		ChangePointerPos(num);
		UIDoodle1 component = base.transform.root.gameObject.GetComponent<UIDoodle1>();
		component.ChangePrePaintSize(num);
	}

	public void HandleEventButton_Slider(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			RefreshPointerPos(wparam, lparam, control);
			break;
		case 2:
			EndRefreshPointerPos(wparam, lparam, control);
			break;
		case 4:
			RefreshPointerPos(wparam, lparam, control);
			break;
		}
	}
}
