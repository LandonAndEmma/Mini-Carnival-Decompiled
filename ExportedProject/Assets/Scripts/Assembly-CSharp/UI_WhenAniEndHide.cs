using System.Collections;
using UnityEngine;

public class UI_WhenAniEndHide : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HideSelf()
	{
		StartCoroutine(HideFun());
	}

	private IEnumerator HideFun()
	{
		yield return 0;
		base.gameObject.SetActive(false);
	}
}
