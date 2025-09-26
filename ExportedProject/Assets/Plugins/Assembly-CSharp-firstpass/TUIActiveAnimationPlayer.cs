using UnityEngine;

[RequireComponent(typeof(Animation))]
[AddComponentMenu("TUI/Control/ActiveAnimationPlayer")]
public class TUIActiveAnimationPlayer : MonoBehaviour
{
	public bool loop;

	public AnimationClip anim;

	public TUIDirection direction = TUIDirection.Forward;

	private TUIActiveAnimation activeAnimation;

	private bool animFinished;

	public void Play()
	{
		if (null != anim)
		{
			AnimationClip clip = base.animation.GetClip(anim.name);
			if (null == clip || clip != anim)
			{
				base.animation.AddClip(anim, anim.name);
			}
			animFinished = false;
			activeAnimation = TUIActiveAnimation.Play(base.animation, anim.name, direction);
		}
	}

	private void Start()
	{
		Play();
		activeAnimation.callWhenFinished = "OnActiveAnimationFinished";
	}

	private void LateUpdate()
	{
		if (loop && animFinished)
		{
			activeAnimation.Reset();
			Play();
		}
	}

	private void OnActiveAnimationFinished()
	{
		animFinished = true;
	}
}
