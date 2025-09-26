using UnityEngine;

public class UIMainMenu_BtnDecorate : MonoBehaviour
{
	[SerializeField]
	private GameObject normalDecorate;

	[SerializeField]
	private GameObject normalLabel;

	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject disableLabel;

	[SerializeField]
	private GameObject disable;

	[SerializeField]
	private GameObject pressDecorate;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BtnDown()
	{
		pressDecorate.active = true;
		normalDecorate.active = false;
	}

	public void BtnUp()
	{
		pressDecorate.active = false;
		normalDecorate.active = true;
	}

	public void SetAlpha(float fAlp)
	{
		Color color = normal.GetComponent<TUIMeshSprite>().color;
		color.a = fAlp;
		normal.GetComponent<TUIMeshSprite>().color = color;
		color = normalDecorate.GetComponent<TUIMeshSprite>().color;
		color.a = fAlp;
		normalDecorate.GetComponent<TUIMeshSprite>().color = color;
		color = normalLabel.GetComponent<TUILabel>().color;
		color.a = fAlp;
		normalLabel.GetComponent<TUILabel>().color = color;
		color = disable.GetComponent<TUIMeshSprite>().color;
		color.a = fAlp;
		disable.GetComponent<TUIMeshSprite>().color = color;
		color = disableLabel.GetComponent<TUILabel>().color;
		color.a = fAlp;
		disableLabel.GetComponent<TUILabel>().color = color;
	}
}
