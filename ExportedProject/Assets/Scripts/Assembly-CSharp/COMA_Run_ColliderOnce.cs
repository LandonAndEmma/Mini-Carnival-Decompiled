using UnityEngine;

public class COMA_Run_ColliderOnce : COMA_Run_RoadCollider
{
	[SerializeField]
	private Animation _aniCmp;

	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Ground");
	}

	private void Update()
	{
	}

	public override void NotifyCollideHitPlayer()
	{
		if (!bProcessHitPlayer)
		{
			bProcessHitPlayer = true;
			if (_aniCmp != null)
			{
				_aniCmp.Play();
			}
			base.gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}
}
