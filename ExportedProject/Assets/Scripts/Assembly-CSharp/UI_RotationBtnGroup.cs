using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_RotationBtnGroup : MonoBehaviour
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

	private int _preActiveIndex;

	private List<float> lstInitAccumulateRadian = new List<float>();

	[SerializeField]
	private UIMainMenu _uiMainMenu;

	[SerializeField]
	private ERotationDirection _RD = ERotationDirection.Anticlockwise;

	[SerializeField]
	private UIMainMenu_ModeIcons _modeIcons;

	private int nSlotCount;

	private bool bCorrecting;

	private float fCorrectSpeed;

	private float fNeedOffestRadian;

	private float fCurOffestRadianSum;

	private bool bNeedMoveInit;

	private bool bMoveInited;

	private void Awake()
	{
		Vector3 position = _camera3D.WorldToScreenPoint(widgetCenterPos);
		Vector3 vector = _cameraTUI.ScreenToWorldPoint(position);
	}

	private void Start()
	{
		intervalRadian = intervalDegree * ((float)Math.PI / 180f);
		nSlotCount = (int)(360f / intervalDegree);
		motionRadius = Vector3.Distance(motionCenter, widgetCenterPos);
		_curActiveIndex = InitLayout();
		_preActiveIndex = _curActiveIndex;
	}

	private void Update()
	{
		if (bCorrecting)
		{
			RTRotateCorrection();
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
		return 1f - (float)num * 0.1f;
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

	private float GetCurRatioInSlotByCurRadian(float curRadian, ref int next, ref int ori)
	{
		int num = 0;
		curRadian %= (float)Math.PI * 2f;
		curRadian = ((!(curRadian < 0f)) ? curRadian : ((float)Math.PI * 2f + curRadian));
		num = (int)(curRadian / intervalRadian);
		if (_RD == ERotationDirection.Anticlockwise)
		{
			ori = num;
			next = ori + 1;
			next = ((next <= nSlotCount - 1) ? next : (next - nSlotCount));
			ori = ((ori <= nSlotCount - 1) ? ori : (ori - nSlotCount));
		}
		else
		{
			next = num;
			ori = num + 1;
			ori = ((ori <= nSlotCount - 1) ? ori : (ori - nSlotCount));
			next = ((next <= nSlotCount - 1) ? next : (next - nSlotCount));
		}
		return (_RD != ERotationDirection.Anticlockwise) ? ((intervalRadian * (float)(num + 1) - curRadian) / intervalRadian) : ((curRadian - intervalRadian * (float)num) / intervalRadian);
	}

	private void RTRotateWidgets(float fDelta)
	{
		float num = fDelta * ((float)Math.PI / 180f) / 12f;
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
			int next = 0;
			int ori = 0;
			float curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next, ref ori);
			float z = _lstWidgetPos[next].z;
			float z2 = _lstWidgetPos[ori].z;
			float z3 = z2 + curRatioInSlotByCurRadian * (z - z2);
			position3.z = z3;
			_objBtns[i].transform.position = position3;
			int next2 = 0;
			int ori2 = 0;
			curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next2, ref ori2);
			float depthScale = GetDepthScale(next2);
			float depthScale2 = GetDepthScale(ori2);
			float num3 = depthScale2 + curRatioInSlotByCurRadian * (depthScale - depthScale2);
			_objBtns[i].transform.localScale = new Vector3(num3, num3, 1f);
			int next3 = 0;
			int ori3 = 0;
			curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next3, ref ori3);
			float depthAlpha = GetDepthAlpha(next3);
			float depthAlpha2 = GetDepthAlpha(ori3);
			float alpha = depthAlpha2 + curRatioInSlotByCurRadian * (depthAlpha - depthAlpha2);
			_objBtns[i].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(alpha);
		}
	}

	private void StartRotateCorrect()
	{
		bCorrecting = true;
		float num = lstInitAccumulateRadian[0];
		float num2 = 0f;
		num %= (float)Math.PI * 2f;
		num = ((!(num < 0f)) ? num : ((float)Math.PI * 2f + num));
		num2 = num / intervalRadian;
		float num3 = 0.15f;
		int num4 = _objBtns.Length;
		int num5 = 0;
		float num6 = Mathf.Min(Mathf.Abs(lstInitAccumulateRadian[0]), Mathf.Abs(lstInitAccumulateRadian[0] - (float)Math.PI * 2f));
		for (int i = 1; i < num4; i++)
		{
			float num7 = Mathf.Min(Mathf.Abs(lstInitAccumulateRadian[i]), Mathf.Abs(lstInitAccumulateRadian[i] - (float)Math.PI * 2f));
			if (num7 < num6)
			{
				num6 = num7;
				num5 = i;
			}
		}
		Debug.Log("nNearestIndex=" + num5);
		float num8 = lstInitAccumulateRadian[num5];
		num8 %= (float)Math.PI * 2f;
		num8 = ((!(num8 < 0f)) ? num8 : ((float)Math.PI * 2f + num8));
		_preActiveIndex = _curActiveIndex;
		_curActiveIndex = num5;
		if (num5 == 0)
		{
			if (num6 >= num3 * intervalRadian)
			{
				if (num8 >= (float)Math.PI)
				{
					if (_RD == ERotationDirection.Clockwise)
					{
						_curActiveIndex = 1;
						num6 = intervalRadian - num6;
					}
				}
				else if (_RD == ERotationDirection.Clockwise)
				{
					_curActiveIndex = 0;
					_RD = ERotationDirection.Clockwise;
				}
				else
				{
					_curActiveIndex = num4 - 1;
					_RD = ERotationDirection.Anticlockwise;
					float num9 = lstInitAccumulateRadian[_curActiveIndex];
					num9 %= (float)Math.PI * 2f;
					num9 = ((!(num9 < 0f)) ? num9 : ((float)Math.PI * 2f + num9));
					num6 = num9;
					num6 = (float)Math.PI * 2f - num9;
				}
			}
			else if (num8 >= (float)Math.PI)
			{
				_curActiveIndex = 0;
				_RD = ERotationDirection.Anticlockwise;
			}
			else if (_RD == ERotationDirection.Clockwise)
			{
				_curActiveIndex = 0;
				_RD = ERotationDirection.Clockwise;
			}
			else
			{
				_curActiveIndex = num4 - 1;
				_RD = ERotationDirection.Anticlockwise;
				float num10 = lstInitAccumulateRadian[_curActiveIndex];
				num10 %= (float)Math.PI * 2f;
				num10 = ((!(num10 < 0f)) ? num10 : ((float)Math.PI * 2f + num10));
				num6 = num10;
				num6 = (float)Math.PI * 2f - num10;
			}
		}
		else if (num5 == num4 - 1)
		{
			if (num6 >= num3 * intervalRadian)
			{
				if (num8 >= (float)Math.PI)
				{
					if (_RD == ERotationDirection.Clockwise)
					{
						_curActiveIndex = 0;
						_RD = ERotationDirection.Clockwise;
						float num11 = lstInitAccumulateRadian[_curActiveIndex];
						num11 %= (float)Math.PI * 2f;
						num11 = ((!(num11 < 0f)) ? num11 : ((float)Math.PI * 2f + num11));
						num6 = num11;
					}
					else
					{
						_curActiveIndex = num4 - 1;
						_RD = ERotationDirection.Anticlockwise;
					}
				}
				else if (_RD == ERotationDirection.Anticlockwise)
				{
					_curActiveIndex--;
					num6 = intervalRadian - num6;
				}
			}
			else if (num8 >= (float)Math.PI)
			{
				if (_RD == ERotationDirection.Clockwise)
				{
					_curActiveIndex = 0;
					_RD = ERotationDirection.Clockwise;
					float num12 = lstInitAccumulateRadian[_curActiveIndex];
					num12 %= (float)Math.PI * 2f;
					num12 = ((!(num12 < 0f)) ? num12 : ((float)Math.PI * 2f + num12));
					num6 = num12;
				}
				else
				{
					_curActiveIndex = num4 - 1;
					_RD = ERotationDirection.Anticlockwise;
				}
			}
			else
			{
				_curActiveIndex = num4 - 1;
				_RD = ERotationDirection.Clockwise;
			}
		}
		else if (num6 >= num3 * intervalRadian)
		{
			if (_preActiveIndex == _curActiveIndex)
			{
				if (num8 >= (float)Math.PI)
				{
					_curActiveIndex++;
					_RD = ERotationDirection.Clockwise;
					num6 = intervalRadian - num6;
				}
				else
				{
					_curActiveIndex--;
					_RD = ERotationDirection.Anticlockwise;
					num6 = intervalRadian - num6;
				}
			}
			else if (num8 >= (float)Math.PI)
			{
				_RD = ERotationDirection.Anticlockwise;
			}
			else
			{
				_RD = ERotationDirection.Clockwise;
			}
		}
		else if (num8 >= (float)Math.PI)
		{
			_RD = ERotationDirection.Anticlockwise;
		}
		else
		{
			_RD = ERotationDirection.Clockwise;
		}
		Debug.Log("当前应该的_curActiveIndex=" + _curActiveIndex + "方向：" + _RD);
		fNeedOffestRadian = num6;
		fCorrectSpeed = 1f;
		fCurOffestRadianSum = 0f;
		if (_RD == ERotationDirection.Clockwise)
		{
			fCorrectSpeed *= -1f;
		}
		if (Mathf.Abs(fNeedOffestRadian) < 5f)
		{
			fCorrectSpeed *= Mathf.Abs(fNeedOffestRadian) * 2f;
		}
		else
		{
			fCorrectSpeed *= Mathf.Abs(fNeedOffestRadian);
		}
		Debug.Log("需要补偿的度数：" + 180f * fNeedOffestRadian / (float)Math.PI);
	}

	private void RTRotateCorrection()
	{
		float num = Time.deltaTime * fCorrectSpeed;
		fCurOffestRadianSum += num;
		if (Mathf.Abs(fCurOffestRadianSum) >= fNeedOffestRadian)
		{
			num = ((!(num < 0f)) ? (num - (Mathf.Abs(fCurOffestRadianSum) - fNeedOffestRadian)) : (num + (Mathf.Abs(fCurOffestRadianSum) - fNeedOffestRadian)));
			Debug.Log("最后一帧误差：" + (Mathf.Abs(fCurOffestRadianSum) - fNeedOffestRadian));
			Debug.Log("需要校正：" + 180f * fCurOffestRadianSum / (float)Math.PI);
			bCorrecting = false;
		}
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
			int next = 0;
			int ori = 0;
			float curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next, ref ori);
			float z = _lstWidgetPos[next].z;
			float z2 = _lstWidgetPos[ori].z;
			float z3 = z2 + curRatioInSlotByCurRadian * (z - z2);
			position3.z = z3;
			_objBtns[i].transform.position = position3;
			int next2 = 0;
			int ori2 = 0;
			curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next2, ref ori2);
			float depthScale = GetDepthScale(next2);
			float depthScale2 = GetDepthScale(ori2);
			float num3 = depthScale2 + curRatioInSlotByCurRadian * (depthScale - depthScale2);
			_objBtns[i].transform.localScale = new Vector3(num3, num3, 1f);
			int next3 = 0;
			int ori3 = 0;
			curRatioInSlotByCurRadian = GetCurRatioInSlotByCurRadian(lstInitAccumulateRadian[i], ref next3, ref ori3);
			float depthAlpha = GetDepthAlpha(next3);
			float depthAlpha2 = GetDepthAlpha(ori3);
			float alpha = depthAlpha2 + curRatioInSlotByCurRadian * (depthAlpha - depthAlpha2);
			_objBtns[i].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(alpha);
		}
		if (bCorrecting)
		{
			return;
		}
		Debug.Log("激活控件ID：" + _curActiveIndex);
		for (int j = 0; j < _objBtns.Length; j++)
		{
			_objBtns[j].transform.position = _lstWidgetPos[GetLstIndexByBtnsIndex(j)];
			if (_curActiveIndex != j)
			{
				_objBtns[j].GetComponent<TUIButtonClick>().m_bDisable = true;
			}
			else
			{
				_objBtns[j].GetComponent<TUIButtonClick>().m_bDisable = false;
			}
		}
		for (int k = 0; k < _objBtns.Length; k++)
		{
			_objBtns[k].transform.localScale = new Vector3(GetDepthScale(GetLstIndexByBtnsIndex(k)), GetDepthScale(GetLstIndexByBtnsIndex(k)), 1f);
		}
		for (int l = 0; l < _objBtns.Length; l++)
		{
			_objBtns[l].GetComponent<UIMainMenu_BtnDecorate>().SetAlpha(GetDepthAlpha(GetLstIndexByBtnsIndex(l)));
		}
		bMoveInited = false;
		if (_modeIcons != null)
		{
			_modeIcons.ShowIcon(_curActiveIndex);
		}
	}

	private bool MoveInit()
	{
		if (!bNeedMoveInit)
		{
			return false;
		}
		lstInitAccumulateRadian.Clear();
		Debug.Log("_curActiveIndex=" + _curActiveIndex);
		for (int i = 0; i < _objBtns.Length; i++)
		{
			int num = i - _curActiveIndex;
			lstInitAccumulateRadian.Add((float)num * intervalRadian);
			_objBtns[i].GetComponent<TUIButtonClick>().m_bDisable = true;
			_objBtns[i].GetComponent<TUIButtonClick>().m_bPressed = false;
			_uiMainMenu.BtnCloseLight(_objBtns[i].GetComponent<TUIButtonClick>());
			_objBtns[i].GetComponent<TUIButtonClick>().Show();
		}
		bNeedMoveInit = false;
		if (_modeIcons != null)
		{
			_modeIcons.HideIcon();
		}
		return true;
	}

	public void HandleEventRoationing(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (bCorrecting)
		{
			return;
		}
		if (eventType == TUIRotationGroup.CommandDown)
		{
			Debug.Log("----------------------TUIRotationGroup.CommandDown");
			bNeedMoveInit = true;
		}
		else if (eventType == TUIRotationGroup.CommandUp)
		{
			Debug.Log("----StartRotateCorrect");
			if (bMoveInited)
			{
				StartRotateCorrect();
			}
		}
		else if (eventType == TUIRotationGroup.CommandMove)
		{
			if (bNeedMoveInit)
			{
				bMoveInited = MoveInit();
			}
			if (bMoveInited)
			{
				_RD = ((wparam > 0f) ? ERotationDirection.Anticlockwise : ERotationDirection.Clockwise);
				RTRotateWidgets(wparam);
			}
		}
	}
}
