using System.Collections;
using UnityEngine;

public class UISquareMiscMgr : MonoBehaviour
{
	[SerializeField]
	private GameObject _btn_OpenMiscContent;

	[SerializeField]
	private GameObject _uiMiscContent;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AniEnd()
	{
		StartCoroutine(DelayCloseMisc());
	}

	public IEnumerator DelayCloseMisc()
	{
		yield return new WaitForSeconds(0.001f);
		_btn_OpenMiscContent.SetActive(true);
		base.gameObject.SetActive(false);
	}
}
