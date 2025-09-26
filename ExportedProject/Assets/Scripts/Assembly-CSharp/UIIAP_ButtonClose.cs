using UnityEngine;

public class UIIAP_ButtonClose : MonoBehaviour
{
	[SerializeField]
	private GameObject _owner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		Object.Destroy(_owner);
	}
}
