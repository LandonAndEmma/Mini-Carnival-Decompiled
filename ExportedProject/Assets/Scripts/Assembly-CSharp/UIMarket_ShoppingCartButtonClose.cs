using UnityEngine;

public class UIMarket_ShoppingCartButtonClose : MonoBehaviour
{
	[SerializeField]
	private GameObject _objOwner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		_objOwner.SetActive(false);
	}
}
