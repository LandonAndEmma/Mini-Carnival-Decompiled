using UnityEngine;

public class RPGEffectContr : MonoBehaviour
{
	[SerializeField]
	public float _activeEffectTime;

	[SerializeField]
	public float _effectDurationTime;

	[SerializeField]
	public float _triggHurtTime;

	[SerializeField]
	public float _effectFlyTime = -1f;

	[SerializeField]
	public float _effectFlyOffsetTime;

	[SerializeField]
	public string _aniName;

	[SerializeField]
	public string _mountNode;

	[SerializeField]
	public bool _rotateByModel;

	[SerializeField]
	public bool _launchFollowWeapon;

	[SerializeField]
	public bool _beAttackFollowWeapon;

	[SerializeField]
	public bool _isLoop;
}
