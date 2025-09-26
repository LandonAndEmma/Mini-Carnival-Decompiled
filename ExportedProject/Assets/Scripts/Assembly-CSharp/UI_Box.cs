using UnityEngine;

public class UI_Box : MonoBehaviour
{
	[SerializeField]
	private UI_BoxSlot[] slots;

	public UI_BoxSlot[] Slots
	{
		get
		{
			if (slots == null)
			{
				slots = GetComponentsInChildren<UI_BoxSlot>();
			}
			return slots;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
