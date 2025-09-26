using UnityEngine;

public class UIMessageBox_ButtonClose : MonoBehaviour
{
	[SerializeField]
	private GameObject _obj;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		Object.Destroy(_obj);
	}
}
