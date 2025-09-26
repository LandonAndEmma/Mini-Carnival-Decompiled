using UnityEngine;

public class UI_GameItemTurn : MonoBehaviour
{
	[SerializeField]
	private Animation _turn1;

	[SerializeField]
	private AnimationClip _turn1Clip;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Turn()
	{
		if (_turn1 != null)
		{
			if (_turn1Clip == null)
			{
				_turn1.Play();
			}
			else
			{
				_turn1.Play(_turn1Clip.name);
			}
		}
	}
}
