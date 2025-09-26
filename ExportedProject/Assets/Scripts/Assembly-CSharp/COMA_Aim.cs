using System;
using UnityEngine;

public class COMA_Aim : MonoBehaviour
{
	private static COMA_Aim _instance;

	public Transform tTrs;

	public Transform bTrs;

	public Transform lTrs;

	public Transform rTrs;

	[NonSerialized]
	public float minSize = 3f;

	[NonSerialized]
	public float maxSize = 8f;

	[NonSerialized]
	public float recoverLerp = 2f;

	private float lerp;

	public static COMA_Aim Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Update()
	{
		float num = Mathf.Lerp(minSize, maxSize, lerp);
		if (tTrs != null)
		{
			tTrs.localPosition = new Vector3(0f, num, 0f);
		}
		if (bTrs != null)
		{
			bTrs.localPosition = new Vector3(0f, 0f - num, 0f);
		}
		if (lTrs != null)
		{
			lTrs.localPosition = new Vector3(0f - num, 0f, 0f);
		}
		if (rTrs != null)
		{
			rTrs.localPosition = new Vector3(num, 0f, 0f);
		}
		lerp = Mathf.Clamp01(lerp - recoverLerp * Time.deltaTime);
	}

	public void Magnify(float addLerp)
	{
		lerp = Mathf.Clamp01(lerp + addLerp);
	}

	public void SetVisible(bool bShow)
	{
		if (tTrs != null)
		{
			tTrs.gameObject.SetActive(bShow);
		}
		if (bTrs != null)
		{
			bTrs.gameObject.SetActive(bShow);
		}
		if (lTrs != null)
		{
			lTrs.gameObject.SetActive(bShow);
		}
		if (rTrs != null)
		{
			rTrs.gameObject.SetActive(bShow);
		}
	}
}
