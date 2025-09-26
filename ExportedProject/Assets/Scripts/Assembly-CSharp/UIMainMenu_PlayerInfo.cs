using UnityEngine;

public class UIMainMenu_PlayerInfo : MonoBehaviour
{
	public GameObject objExpIndicate;

	public TUILabel levelCom;

	public TUILabel nameCom;

	private float fOriLocalX;

	public bool bTest;

	public float fRate;

	private float maxLength = 128f;

	private void Awake()
	{
		if (objExpIndicate != null)
		{
			fOriLocalX = objExpIndicate.transform.localPosition.x;
		}
	}

	private void Update()
	{
		if (bTest)
		{
			RefreshExp(fRate);
			bTest = false;
		}
	}

	public void RefreshLevel(int lv)
	{
		levelCom.Text = lv.ToString();
	}

	public void RefreshName(string nickName)
	{
		nameCom.Text = nickName;
	}

	public void RefreshExp(float f)
	{
		f = Mathf.Clamp01(f);
		Vector3 localScale = objExpIndicate.transform.localScale;
		localScale.x = f;
		objExpIndicate.transform.localScale = localScale;
		float num = (1f - f) / 2f;
		float num2 = (0f - num) * maxLength;
		Vector3 localPosition = objExpIndicate.transform.localPosition;
		localPosition.x = fOriLocalX + num2;
		objExpIndicate.transform.localPosition = localPosition;
	}
}
