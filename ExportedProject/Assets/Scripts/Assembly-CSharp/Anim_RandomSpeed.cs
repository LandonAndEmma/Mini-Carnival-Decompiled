using UnityEngine;

public class Anim_RandomSpeed : MonoBehaviour
{
	public string animName = string.Empty;

	public float animSpeed = 1f;

	private void Awake()
	{
		if (animName != string.Empty)
		{
			base.animation[animName].speed = animSpeed;
		}
	}
}
