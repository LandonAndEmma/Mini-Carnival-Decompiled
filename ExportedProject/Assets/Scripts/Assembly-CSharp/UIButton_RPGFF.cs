using UnityEngine;

public class UIButton_RPGFF : MonoBehaviour
{
	[SerializeField]
	private UILabel _labelFF;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_labelFF.text == "1X")
		{
			Time.timeScale = 2f;
			_labelFF.text = "2X";
		}
		else
		{
			Time.timeScale = 1f;
			_labelFF.text = "1X";
		}
	}

	private void OnDestroy()
	{
		Time.timeScale = 1f;
		_labelFF.text = "1X";
	}
}
