using UnityEngine;

public class RPGCharacter_Effect : MonoBehaviour
{
	public enum ERPGPart
	{
		head = 1,
		body = 2,
		leg = 3,
		decoration = 4
	}

	public enum EDecorationPart
	{
		head_top = 0,
		head_front = 1,
		head_back = 2,
		head_left = 3,
		head_right = 4,
		chest_front = 5,
		chest_back = 6,
		hand_left = 7,
		hand_right = 8,
		MAX = 9
	}

	[SerializeField]
	private Transform[] _decorationNodes = new Transform[9];

	public Transform GetDecoByPart(EDecorationPart part)
	{
		return _decorationNodes[(int)part];
	}
}
