using UnityEngine;

public class BloodBar : MonoBehaviour
{
	public enum BarColor
	{
		Grey = 0,
		Red = 1,
		Blue = 2
	}

	private BarColor curClr;

	[SerializeField]
	private Transform adjustTrs;

	[SerializeField]
	private Transform greyBarTrs;

	[SerializeField]
	private Transform redBarTrs;

	[SerializeField]
	private Transform blueBarTrs;

	private Transform cam;

	public void InitBloodBar(BarColor color)
	{
		InitBloodBar(color, 1f, 1f);
	}

	public void InitBloodBar(BarColor color, float sizeScaleX, float sizeScaleY)
	{
		curClr = color;
		adjustTrs.localScale = new Vector3(adjustTrs.localScale.x * sizeScaleX, 1f, adjustTrs.localScale.z * sizeScaleY);
		adjustTrs.localPosition = new Vector3(adjustTrs.localScale.x * 0.5f, 0f, 0f);
		greyBarTrs.transform.localPosition = new Vector3(0f, -0.01f, 0f);
		switch (curClr)
		{
		case BarColor.Grey:
			redBarTrs.gameObject.SetActive(false);
			blueBarTrs.gameObject.SetActive(false);
			break;
		case BarColor.Red:
			redBarTrs.gameObject.SetActive(true);
			blueBarTrs.gameObject.SetActive(false);
			break;
		case BarColor.Blue:
			redBarTrs.gameObject.SetActive(false);
			blueBarTrs.gameObject.SetActive(true);
			break;
		}
	}

	public void SetBloodBar(float length)
	{
		length = Mathf.Clamp01(length);
		Debug.Log(curClr);
		Debug.Log(length);
		switch (curClr)
		{
		case BarColor.Red:
			redBarTrs.localScale = new Vector3(length, 1f, 1f);
			break;
		case BarColor.Blue:
			blueBarTrs.localScale = new Vector3(length, 1f, 1f);
			break;
		}
	}

	private void Start()
	{
		cam = GameObject.Find("Main Camera").transform;
	}

	private void Update()
	{
		if (Application.loadedLevelName.StartsWith("COMA_Scene_RPG"))
		{
			base.transform.rotation = Quaternion.Euler(0f - cam.rotation.eulerAngles.x, 0f, 0f);
		}
	}
}
