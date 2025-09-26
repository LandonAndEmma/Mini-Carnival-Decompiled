using UnityEngine;

public class UIButton_AutoFight : MonoBehaviour
{
	[SerializeField]
	private UISprite _spriteCircle;

	[SerializeField]
	private UISprite _spriteCube;

	[SerializeField]
	private UISprite _spriteTriangle;

	[SerializeField]
	private RPGTeam _team;

	private void InitUI()
	{
		if (_team.AutoFight)
		{
			_spriteCircle.animation.Play();
			_spriteCube.enabled = true;
			_spriteTriangle.enabled = false;
		}
		else
		{
			_spriteCircle.animation.Stop();
			_spriteCube.enabled = false;
			_spriteTriangle.enabled = true;
		}
	}

	private void RefreshUI()
	{
		Debug.Log(_team.AutoFight);
		_team.AutoFight = !_team.AutoFight;
		Debug.Log(_team.AutoFight);
		InitUI();
	}

	private void Start()
	{
		InitUI();
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		RefreshUI();
	}
}
