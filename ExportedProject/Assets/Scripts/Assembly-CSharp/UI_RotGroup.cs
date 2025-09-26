using System;
using UnityEngine;

public class UI_RotGroup : MonoBehaviour
{
	[SerializeField]
	private bool leftLayoutPrior;

	[SerializeField]
	private float intervalDegree = 30f;

	private float intervalRadian;

	[SerializeField]
	private float angularAccer = 0.06f;

	[SerializeField]
	private float minT = 12f;

	private float minAngleVelocity;

	[SerializeField]
	private float maxT = 36f;

	private float maxAngleVelocity;

	[SerializeField]
	private Vector3 widgetCenterPos = new Vector3(0f, -2.5f, 5.5f);

	[SerializeField]
	private GameObject motionCenter;

	[SerializeField]
	private GameObject widgetPrefab;

	public int widgetCount = 6;

	private float motionRadius;

	private Vector3[] widgetPos;

	private GameObject[] widgetObj;

	private float oriAngleVelocity;

	public float curAngleVelocity;

	private float fMotionAccumulateRadian;

	private int curLapNum;

	[SerializeField]
	private bool testStep;

	private void Awake()
	{
		InitLayout();
		widgetObj = new GameObject[widgetCount];
		for (int i = 0; i < widgetCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(widgetPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = widgetPos[i];
			gameObject.transform.rotation = Quaternion.Euler(new Vector3(90f, 180f, 0f));
			widgetObj[i] = gameObject;
		}
	}

	private void Start()
	{
		minAngleVelocity = (float)Math.PI * 2f / maxT;
		maxAngleVelocity = (float)Math.PI * 2f / minT;
		oriAngleVelocity = (minAngleVelocity + maxAngleVelocity) / 2f;
		curAngleVelocity = oriAngleVelocity;
		fMotionAccumulateRadian = 0f;
		curLapNum = 0;
	}

	private void Update()
	{
		if (testStep)
		{
			float num = curAngleVelocity * Time.deltaTime;
			fMotionAccumulateRadian += num;
			if (fMotionAccumulateRadian >= intervalRadian)
			{
				fMotionAccumulateRadian = intervalRadian;
				testStep = false;
			}
			Vector3 position = widgetCenterPos;
			position.x = motionCenter.transform.position.x + Mathf.Sin((float)curLapNum * intervalRadian + fMotionAccumulateRadian) * motionRadius;
			position.z = motionCenter.transform.position.z - Mathf.Cos((float)curLapNum * intervalRadian + fMotionAccumulateRadian) * motionRadius;
			base.gameObject.transform.position = position;
			curAngleVelocity -= angularAccer * Time.deltaTime;
			curAngleVelocity = ((!(curAngleVelocity <= 0f)) ? curAngleVelocity : 0.02f);
			if (!testStep)
			{
				fMotionAccumulateRadian = 0f;
				curAngleVelocity = oriAngleVelocity;
				curLapNum++;
				curLapNum = ((!((float)curLapNum > 360f / intervalDegree - 1f)) ? curLapNum : 0);
			}
		}
	}

	private void InitLayout()
	{
		intervalRadian = intervalDegree * ((float)Math.PI / 180f);
		motionRadius = Vector3.Distance(motionCenter.transform.position, widgetCenterPos);
		widgetPos = new Vector3[widgetCount];
		int num = ((!leftLayoutPrior) ? Mathf.FloorToInt((float)(widgetCount - 1) / 2f) : Mathf.CeilToInt((float)(widgetCount - 1) / 2f));
		for (int i = 0; i < widgetCount; i++)
		{
			int num2 = i - num;
			Vector3 vector = widgetCenterPos;
			vector.x = motionCenter.transform.position.x + Mathf.Sin((float)num2 * intervalRadian) * motionRadius;
			vector.z = motionCenter.transform.position.z - Mathf.Cos((float)num2 * intervalRadian) * motionRadius;
			widgetPos[i] = vector;
		}
	}

	private void RestoreParam(bool bPos)
	{
	}
}
