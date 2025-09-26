using UnityEngine;

public class UIMarket_AuthorDetailAniController : MonoBehaviour
{
	[SerializeField]
	private GameObject _aniLayer;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_aniLayer.activeSelf)
		{
			_aniLayer.animation.Play("UIMarket_AuthorInfoUnstretch");
		}
		else
		{
			_aniLayer.SetActive(true);
		}
	}
}
