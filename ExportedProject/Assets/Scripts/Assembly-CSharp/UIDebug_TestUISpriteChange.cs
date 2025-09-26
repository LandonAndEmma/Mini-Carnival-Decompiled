using UnityEngine;

public class UIDebug_TestUISpriteChange : MonoBehaviour
{
	[SerializeField]
	private UISprite _testSprite;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		_testSprite.spriteName = "CommonIconGold";
	}
}
