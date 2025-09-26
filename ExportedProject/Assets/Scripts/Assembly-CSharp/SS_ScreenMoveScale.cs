using System.Collections.Generic;
using UnityEngine;

public class SS_ScreenMoveScale : MonoBehaviour
{
	private enum FingerState
	{
		None = 0,
		MapMoving = 1,
		MapScaling = 2,
		PlayerDirecting = 3,
		PlayerTracing = 4
	}

	public Camera m_cmr;

	private Dictionary<int, Vector2> m_dict_fingerPos = new Dictionary<int, Vector2>();

	public Rect m_cmrMoveRestrict = new Rect(-20f, -20f, 40f, 40f);

	private Matrix4x4 m_cmrStartMatrixWorldToLocal = default(Matrix4x4);

	private Matrix4x4 m_cmrStartMatrixLocalToWorld = default(Matrix4x4);

	private Vector3 m_cmrTargetPosition = Vector3.zero;

	private Vector3 m_lastStep = Vector2.zero;

	private float m_holdTime;

	private float m_damp = 0.96f;

	private bool m_autoMove;

	public Vector2 m_cmrScale = new Vector2(10f, 60f);

	public float m_cmrScaleMin = 5f;

	private float m_cmrDefaultSize;

	private float m_cmrTargetScale;

	private Vector3 m_cmrPosMid = Vector3.zero;

	public bool m_test;

	public Vector2 m_testFinger1 = new Vector2(100f, 300f);

	public UICamera m_ngui_cmr;

	private FingerState m_fingerState;

	private int m_groupID = -1;

	private List<Vector3> m_lst_linePos = new List<Vector3>();

	public MeshFilter m_drawLineMesh;

	[SerializeField]
	private float _imageWidth;

	[SerializeField]
	private float _imageHeight;

	[SerializeField]
	private UIRPG_Root _root;

	public UIRPG_Root Root
	{
		get
		{
			return _root;
		}
	}

	public void OnEnable()
	{
		FingerGestures.OnFingerDown += OnFingerDown;
		FingerGestures.OnFingerUp += OnFingerUp;
		FingerGestures.OnFingerDragMove += OnFingerDragMove;
		FingerGestures.OnFingerDragEnd += OnFingerDragEnd;
		FingerGestures.OnFingerStationary += OnFingerStationary;
	}

	public void OnDisable()
	{
		FingerGestures.OnFingerDown -= OnFingerDown;
		FingerGestures.OnFingerUp -= OnFingerUp;
		FingerGestures.OnFingerDragMove -= OnFingerDragMove;
		FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;
		FingerGestures.OnFingerStationary -= OnFingerStationary;
	}

	private void Start()
	{
		m_cmrStartMatrixWorldToLocal = m_cmr.transform.worldToLocalMatrix;
		m_cmrStartMatrixLocalToWorld = m_cmr.transform.localToWorldMatrix;
		m_cmrMoveRestrict.x = (0f - _imageWidth) / (float)_root.activeHeight;
		m_cmrMoveRestrict.y = (0f - _imageHeight) / (float)_root.activeHeight;
		m_cmrMoveRestrict.width = m_cmrMoveRestrict.x * -2f;
		m_cmrMoveRestrict.height = m_cmrMoveRestrict.y * -2f;
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = (float)_root.activeHeight * num;
		Debug.Log("Screen.width = " + Screen.width);
		Debug.Log("screenViewWidth = " + num2);
		float num3 = 1f;
		float num4 = 0f;
		float num5 = 0f;
		num3 = ((!(num2 <= _imageWidth)) ? (_imageWidth / num2 / 2f) : 0.5f);
		num4 = ((!(num3 >= 1f)) ? ((0f - (_imageWidth - num2 * num3)) / 2f / (float)(_root.activeHeight / 2)) : (0f - (_imageWidth - num2) / 2f / (float)(_root.activeHeight / 2) - (1f - num3)));
		num5 = 0f - (_imageHeight - (float)_root.activeHeight) / 2f / (float)(_root.activeHeight / 2) - (1f - num3);
		Vector3 vector = new Vector3(num4, num5, 0f);
		m_cmrScale.y = num3 * 2f;
		m_cmr.orthographicSize = num3;
		m_cmrDefaultSize = m_cmr.orthographicSize;
		m_cmrTargetScale = m_cmr.orthographicSize;
		Debug.Log("posZero = " + vector);
		if (UIDataBufferCenter.Instance.CurBattleLevelIndex != -1)
		{
			Debug.Log("if (UIDataBufferCenter.Instance.CurBattleLevelIndex != -1)");
			Vector3 v = UIRPG_DataBufferCenter.Instance.PosVector3[UIDataBufferCenter.Instance.CurBattleLevelIndex] - new Vector3(512f, 340f, 0f);
			v *= _root.transform.localScale.x;
			v = m_cmr.transform.localToWorldMatrix.MultiplyPoint3x4(v);
			m_cmrTargetPosition = v;
		}
		else
		{
			m_cmrTargetPosition = vector;
		}
	}

	public void SetTarPos(Vector3 pos)
	{
		m_cmrTargetPosition = pos;
	}

	private Vector3 MoveRestrict(Vector3 tarPosition, Matrix4x4 W2L, Matrix4x4 L2W, Rect cmrRestrict)
	{
		Vector3 v = W2L.MultiplyPoint3x4(tarPosition);
		float orthographicSize = m_cmr.orthographicSize;
		float num = orthographicSize * (float)Screen.width / (float)Screen.height;
		v.x = Mathf.Clamp(v.x, cmrRestrict.xMin + num, cmrRestrict.xMax - num);
		v.y = Mathf.Clamp(v.y, cmrRestrict.yMin + orthographicSize, cmrRestrict.yMax - orthographicSize);
		return L2W.MultiplyPoint3x4(v);
	}

	public void Update()
	{
		if (m_test)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				m_testFinger1 = Input.mousePosition;
				OnFingerDown(1, m_testFinger1);
			}
			else if (Input.GetKeyUp(KeyCode.Q))
			{
				OnFingerUp(1, m_testFinger1, 0f);
			}
		}
		if (m_dict_fingerPos.Count == 1)
		{
			if (m_cmrTargetScale < m_cmrScale.x)
			{
				m_cmrTargetScale = Mathf.Lerp(m_cmrTargetScale, m_cmrScale.x, 0.2f);
			}
		}
		else if (m_dict_fingerPos.Count != 2)
		{
			if (m_cmrTargetScale < m_cmrScale.x)
			{
				m_cmrTargetScale = Mathf.Lerp(m_cmrTargetScale, m_cmrScale.x, 0.2f);
			}
			if (m_autoMove)
			{
				m_cmrTargetPosition += m_lastStep;
				m_lastStep *= m_damp;
				if (m_lastStep.sqrMagnitude < 0.001f)
				{
					m_autoMove = false;
				}
			}
		}
		m_cmrTargetPosition = MoveRestrict(m_cmrTargetPosition, m_cmrStartMatrixWorldToLocal, m_cmrStartMatrixLocalToWorld, m_cmrMoveRestrict);
		m_cmr.transform.position = Vector3.Lerp(m_cmr.transform.position, m_cmrTargetPosition, 0.3f);
		m_cmr.orthographicSize = Mathf.Lerp(m_cmr.orthographicSize, m_cmrTargetScale, 0.3f);
	}

	private bool IsNGUIUsed()
	{
		return false;
	}

	private Vector3 ScreenPointToGroundPoint(Vector2 screenPoint)
	{
		Ray ray = m_cmr.ScreenPointToRay(screenPoint);
		int layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, layerMask))
		{
			return hitInfo.point;
		}
		Debug.LogWarning("No Point Touched!!");
		return Vector3.zero;
	}

	private void OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		if (!IsNGUIUsed() && fingerIndex < 2)
		{
			m_autoMove = false;
			m_dict_fingerPos[fingerIndex] = fingerPos;
			if (m_dict_fingerPos.Count == 2)
			{
				m_cmrPosMid = m_cmr.ScreenToWorldPoint(Vector2.Lerp(m_dict_fingerPos[0], m_dict_fingerPos[1], 0.5f));
			}
		}
	}

	private void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		if (!IsNGUIUsed() && fingerIndex < 2)
		{
			if (m_dict_fingerPos.ContainsKey(fingerIndex))
			{
				m_dict_fingerPos.Remove(fingerIndex);
			}
			m_fingerState = FingerState.None;
		}
	}

	private void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (IsNGUIUsed() || fingerIndex >= 2)
		{
			return;
		}
		if (m_dict_fingerPos.Count == 1)
		{
			m_holdTime = 0f;
			Vector3 vector = m_cmr.ScreenToWorldPoint(delta);
			Vector3 vector2 = m_cmr.ScreenToWorldPoint(Vector3.zero);
			Vector3 vector3 = vector - vector2;
			m_cmrTargetPosition -= vector3;
			m_lastStep = -vector3;
			m_fingerState = FingerState.MapMoving;
		}
		else if (m_dict_fingerPos.Count == 2)
		{
			Vector2 vector4 = m_dict_fingerPos[fingerIndex];
			Vector2 vector5 = m_dict_fingerPos[1 - fingerIndex];
			Vector2 lhs = vector4 - vector5;
			float num = 1f - Vector2.Dot(lhs, delta) / lhs.sqrMagnitude;
			float cmrTargetScale = Mathf.Clamp(m_cmrTargetScale * num, m_cmrScaleMin, m_cmrScale.y);
			m_cmrTargetScale = cmrTargetScale;
			if (m_cmrTargetScale > m_cmrScaleMin && m_cmrTargetScale < m_cmrScale.y)
			{
				Vector3 vector6 = m_cmr.ScreenToWorldPoint(delta);
				Vector3 vector7 = m_cmr.ScreenToWorldPoint(Vector3.zero);
				Vector3 vector8 = vector6 - vector7;
				m_cmrPosMid -= vector8 * 0.5f;
				m_cmrTargetPosition = m_cmrTargetPosition * num + m_cmrPosMid * (1f - num);
			}
			m_fingerState = FingerState.MapScaling;
		}
	}

	private void OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (!IsNGUIUsed() && fingerIndex < 2 && m_dict_fingerPos.Count == 0 && m_holdTime < 0.5f)
		{
			if (m_lastStep.sqrMagnitude > 4f)
			{
				m_lastStep = m_lastStep.normalized * 2f;
			}
			m_autoMove = true;
		}
	}

	private void OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		if (!IsNGUIUsed() && fingerIndex < 2)
		{
			m_holdTime += elapsedTime;
		}
	}
}
