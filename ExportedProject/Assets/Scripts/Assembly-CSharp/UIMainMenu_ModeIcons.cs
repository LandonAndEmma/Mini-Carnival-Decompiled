using UnityEngine;

public class UIMainMenu_ModeIcons : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _icons;

	private int _curShowIndex = 2;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HideIcon()
	{
		_icons[_curShowIndex].SetActive(false);
	}

	public void ShowIcon(int nIndex)
	{
		_curShowIndex = nIndex;
		_icons[_curShowIndex].SetActive(true);
	}
}
