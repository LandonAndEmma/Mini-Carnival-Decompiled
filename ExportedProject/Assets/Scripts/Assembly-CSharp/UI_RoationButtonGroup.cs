using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoationButtonGroup : MonoBehaviour
{
	private enum ERotationDirection
	{
		Clockwise = 0,
		Anticlockwise = 1
	}

	private const int _nDepthOfInterval = 10;

	[SerializeField]
	private GameObject[] _objBtns;

	[SerializeField]
	private Camera _camera3D;

	[SerializeField]
	private Camera _cameraTUI;

	[SerializeField]
	private bool leftLayoutPrior;

	[SerializeField]
	private float intervalDegree = 36f;

	private float intervalRadian;

	[SerializeField]
	private Vector3 widgetCenterPos = new Vector3(0f, -2.5f, 5.5f);

	[SerializeField]
	private Vector3 motionCenter = new Vector3(0f, -2.5f, 10f);

	private float motionRadius;

	private List<Vector3> _lstWidgetPos = new List<Vector3>();

	private int _curActiveIndex;

	private List<float> lstInitAccumulateRadian = new List<float>();

	[SerializeField]
	private UIMainMenu _uiMainMenu;

	[SerializeField]
	private bool testStep;

	[SerializeField]
	private int nStepNum;

	[SerializeField]
	private float fStepTime;

	[SerializeField]
	private ERotationDirection _RD = ERotationDirection.Anticlockwise;

	private float fTotalRadian;

	private float fAccer;

	private float fAngleVelocity0;

	private int nCurRotElapsedSlot;

	private float fPerSlotAccumulateTime;

	private int nAimActiveIndex;

	private int nAimRealSlotNum;

	private float fRotationTime;

	private float fTimeToNextSlot;

	private float fCurRotationRadianSum;

	private int nCurSlotCompareArgu;

	private int nSlotCount;

	public float curAngleVelocity;

	private void Awake()
	{
		Vector3 vector = _camera3D.WorldToScreenPoint(widgetCenterPos);
		Vector3 vector2 = _cameraTUI.ScreenToWorldPoint(vector);
		Debug.Log("widgetCenterScreen=" + vector);
		Debug.Log("widgetCenterTUI=" + vector2);
	}

	private void Start()
	{
		intervalRadian = intervalDegree * ((float)Math.PI / 180f);
		nSlotCount = (int)(360f / intervalDegree);
		motionRadius = Vector3.Distance(motionCenter, widgetCenterPos);
		_curActiveIndex = InitLayout();
	}

	private void CalcToNextSlotTime()
	{
		fPerSlotAccumulateTime = 0f;
		float num = Mathf.Sqrt(curAngleVelocity * curAngleVelocity - 4f * (0f - intervalRadian) * (-0.5f * fAccer));
		fTimeToNextSlot = (0f - Mathf.Abs(curAngleVelocity) + num) / (2f * (-0.5f * fAccer));
		Debug.Log("fTimeToNextSlot=" + fTimeToNextSlot);
	}

	private void NotifyRoation(float stepTime, int stepNum)
	{
		nAimRealSlotNum = SetRotateBySlot(stepTime, stepNum);
		Debug.Log("nAimRealSlotNum=" + nAimRealSlotNum);
		fTotalRadian = intervalRadian * (float)nAimRealSlotNum;
		fAccer = fTotalRadian * 2f / (fRotationTime * fRotationTime);
		fAngleVelocity0 = ((_RD != ERotationDirection.Anticlockwise) ? (0f - fAccer * fRotationTime) : (fAccer * fRotationTime));
		curAngleVelocity = fAngleVelocity0;
		CalcToNextSlotTime();
	}

	private void NotifyRoation()
	{
		nAimRealSlotNum = SetRotateBySlot(fStepTime, nStepNum);
		Debug.Log("nAimRealSlotNum=" + nAimRealSlotNum);
		fTotalRadian = intervalRadian * (float)nAimRealSlotNum;
		fAccer = fTotalRadian * 2f / (fRotationTime * fRotationTime);
		fAngleVelocity0 = ((_RD != ERotationDirection.Anticlockwise) ? (0f - fAccer * fRotationTime) : (fAccer * fRotationTime));
		curAngleVelocity = fAngleVelocity0;
		CalcToNextSlotTime();
	}

	private void Update()
	{
		if (testStep)
		{
			NotifyRoation();
			testStep = false;
		}
		if (nAimRealSlotNum != 0)
		{
			RotateWidgets();
		}
	}

	private int GetDepthZ(int n)
	{
		int num = nSlotCount / 2 + 1;
		int num2 = -15 * num;
		int num3 = ((n <= nSlotCount / 2) ? n : (nSlotCount - n));
		return num2 + 10 * num3;
	}

	private float GetDepthScale(int n)
	{
		int num = ((n <= nSlotCount / 2) ? n : (nSlotCount - n));
		return 1f - (float)num * 0.1f;
	}

	private float GetDepthAlpha(int n)
	{
		int num = ((n <= nSlotCount / 2) ? n : (nSlotCount - n));
		return 1f - (float)num * 0.15f;
	}

	private int InitLayout()
	{
		int num = _objBtns.Length;
		int num2 = ((!leftLayoutPrior) ? Mathf.FloorToInt((float)(num - 1) / 2f) : Mathf.CeilToInt((float)(num - 1) / 2f));
		_lstWidgetPos.Clear();
		for (int i = 0; i < nSlotCount; i++)
		{
			Vector3 position = widgetCenterPos;
			position.x = motionCenter.x + Mathf.Sin((float)i * intervalRadian) * motionRadius;
			position.z = motionCenter.z - Mathf.Cos((float)i * intervalRadian) * motionRadius;
			Vector3 position2 = _camera3D.WorldToScreenPoint(position);
			Vector3 item = _cameraTUI.ScreenToWorldPoint(position2);
			int depthZ = GetDepthZ(i);
			item.z = depthZ;
			_lstWidgetPos.Add(item);
		}
		for (int j = 0; j < num; j++)
		{
			int num3 = j - num2;
			num3 = ((num3 >= 0) ? num3 : (nSlotCount + num3));
			_objBtns[j].transform.position = _lstWidgetPos[num3];
			_objBtns[j].transform.localScale = new Vector3(GetDepthScale(num3), GetDepthScale(num3), 1f);
			_objBtns[j].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(GetDepthAlpha(num3));
			if (num3 != 0)
			{
				_objBtns[j].GetComponent<TUIButtonClick>().m_bDisable = true;
			}
			else
			{
				_objBtns[j].GetComponent<TUIButtonClick>().m_bDisable = false;
			}
		}
		return num2;
	}

	private int GetLstIndexByBtnsIndex(int nBtnsIndex)
	{
		int num = nBtnsIndex - _curActiveIndex;
		return (num < 0) ? (nSlotCount + num) : num;
	}

	private int ForecastNextIndex(int nBtnsIndex)
	{
		int curActiveIndex = _curActiveIndex;
		curActiveIndex = ((_RD != ERotationDirection.Anticlockwise) ? (curActiveIndex + 1) : (curActiveIndex - 1));
		curActiveIndex = ((curActiveIndex >= 0) ? curActiveIndex : (nSlotCount + curActiveIndex));
		int num = nBtnsIndex - curActiveIndex;
		return (num < 0) ? (nSlotCount + num) : num;
	}

	private int SetRotateBySlot(float fTime, int nSlotNum)
	{
		fRotationTime = fTime;
		int num = _objBtns.Length;
		nCurRotElapsedSlot = 0;
		fCurRotationRadianSum = 0f;
		nCurSlotCompareArgu = 1;
		lstInitAccumulateRadian.Clear();
		for (int i = 0; i < num; i++)
		{
			int num2 = i - _curActiveIndex;
			lstInitAccumulateRadian.Add((float)num2 * intervalRadian);
			_objBtns[i].GetComponent<TUIButtonClick>().m_bDisable = true;
			_objBtns[i].GetComponent<TUIButtonClick>().m_bPressed = false;
			_uiMainMenu.BtnCloseLight(_objBtns[i].GetComponent<TUIButtonClick>());
			_objBtns[i].GetComponent<TUIButtonClick>().Show();
		}
		int num3 = nSlotNum % nSlotCount;
		int num4 = num - 1 - _curActiveIndex;
		int num5 = num - 1 - num4;
		int num6 = ((_RD != ERotationDirection.Anticlockwise) ? num5 : num4);
		int num7 = ((_RD != ERotationDirection.Anticlockwise) ? num4 : num5);
		int num8 = 0;
		if (num3 <= num7)
		{
			nAimActiveIndex = ((_RD != ERotationDirection.Anticlockwise) ? (_curActiveIndex + num3) : (_curActiveIndex - num3));
			return nSlotNum;
		}
		if (num3 <= nSlotCount - num6 && num3 > num7)
		{
			num8 = ((_RD != ERotationDirection.Anticlockwise) ? (nSlotCount - num6 - num3) : (nSlotCount - num6 - num3));
			nAimActiveIndex = ((_RD == ERotationDirection.Anticlockwise) ? (num - 1) : 0);
			return nSlotNum + num8;
		}
		nAimActiveIndex = ((_RD != ERotationDirection.Anticlockwise) ? (num3 - (nSlotCount - num6)) : (num - 1 - (num3 - (nSlotCount - num6))));
		return nSlotNum;
	}

	private void RotateWidgets()
	{
		float num = curAngleVelocity * Time.deltaTime;
		fCurRotationRadianSum += num;
		if (Mathf.Abs(fCurRotationRadianSum) >= fTotalRadian)
		{
			num -= fCurRotationRadianSum - fTotalRadian;
			curAngleVelocity = 0f;
		}
		fPerSlotAccumulateTime += Time.deltaTime;
		for (int i = 0; i < _objBtns.Length; i++)
		{
			List<float> list2;
			List<float> list = (list2 = lstInitAccumulateRadian);
			int index2;
			int index = (index2 = i);
			float num2 = list2[index2];
			list[index] = num2 + num;
			Vector3 position = widgetCenterPos;
			position.x = motionCenter.x + Mathf.Sin(lstInitAccumulateRadian[i]) * motionRadius;
			position.z = motionCenter.z - Mathf.Cos(lstInitAccumulateRadian[i]) * motionRadius;
			Vector3 position2 = _camera3D.WorldToScreenPoint(position);
			Vector3 position3 = _cameraTUI.ScreenToWorldPoint(position2);
			int num3 = ForecastNextIndex(i);
			Debug.Log("nNextIndex=" + num3);
			float z = _lstWidgetPos[num3].z;
			float z2 = _lstWidgetPos[GetLstIndexByBtnsIndex(i)].z;
			float z3 = z2 + fPerSlotAccumulateTime / fTimeToNextSlot * (z - z2);
			position3.z = z3;
			_objBtns[i].transform.position = position3;
			float depthScale = GetDepthScale(num3);
			float depthScale2 = GetDepthScale(GetLstIndexByBtnsIndex(i));
			float num4 = depthScale2 + fPerSlotAccumulateTime / fTimeToNextSlot * (depthScale - depthScale2);
			_objBtns[i].transform.localScale = new Vector3(num4, num4, 1f);
			float depthAlpha = GetDepthAlpha(num3);
			float depthAlpha2 = GetDepthAlpha(GetLstIndexByBtnsIndex(i));
			float alpha = depthAlpha2 + fPerSlotAccumulateTime / fTimeToNextSlot * (depthAlpha - depthAlpha2);
			_objBtns[i].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(alpha);
		}
		int num5 = (int)(Mathf.Abs(fCurRotationRadianSum) / intervalRadian);
		if (num5 >= nCurSlotCompareArgu)
		{
			_curActiveIndex = ((_RD != ERotationDirection.Anticlockwise) ? (_curActiveIndex + 1) : (_curActiveIndex - 1));
			_curActiveIndex = ((_curActiveIndex >= 0) ? _curActiveIndex : (nSlotCount + _curActiveIndex));
			_curActiveIndex = ((_curActiveIndex <= nSlotCount - 1) ? _curActiveIndex : (_curActiveIndex % nSlotCount));
			nCurSlotCompareArgu++;
			for (int j = 0; j < _objBtns.Length; j++)
			{
				Vector3 position4 = _objBtns[j].transform.position;
				position4.z = _lstWidgetPos[GetLstIndexByBtnsIndex(j)].z;
				_objBtns[j].transform.position = position4;
			}
			for (int k = 0; k < _objBtns.Length; k++)
			{
				_objBtns[k].transform.localScale = new Vector3(GetDepthScale(GetLstIndexByBtnsIndex(k)), GetDepthScale(GetLstIndexByBtnsIndex(k)), 1f);
			}
			for (int l = 0; l < _objBtns.Length; l++)
			{
				_objBtns[l].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(GetDepthAlpha(GetLstIndexByBtnsIndex(l)));
			}
			if (curAngleVelocity != 0f)
			{
				CalcToNextSlotTime();
			}
			Debug.Log("-----------One Slot Pass!!");
		}
		if (_RD == ERotationDirection.Anticlockwise)
		{
			curAngleVelocity -= fAccer * Time.deltaTime;
		}
		else
		{
			curAngleVelocity += fAccer * Time.deltaTime;
		}
		if (!((_RD != ERotationDirection.Anticlockwise) ? (curAngleVelocity >= 0f) : (curAngleVelocity <= 0f)))
		{
			return;
		}
		_curActiveIndex = nAimActiveIndex;
		nAimRealSlotNum = 0;
		for (int m = 0; m < _objBtns.Length; m++)
		{
			_objBtns[m].transform.position = _lstWidgetPos[GetLstIndexByBtnsIndex(m)];
			if (_curActiveIndex != m)
			{
				_objBtns[m].GetComponent<TUIButtonClick>().m_bDisable = true;
			}
			else
			{
				_objBtns[m].GetComponent<TUIButtonClick>().m_bDisable = false;
			}
		}
		Debug.Log("_curActiveIndex=" + _curActiveIndex);
	}

	public void HandleEventRoation(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == TUIRotationGroup.CommandUp && nAimRealSlotNum == 0)
		{
			_RD = (ERotationDirection)wparam;
			NotifyRoation(0.8f, (int)lparam);
		}
	}
}
