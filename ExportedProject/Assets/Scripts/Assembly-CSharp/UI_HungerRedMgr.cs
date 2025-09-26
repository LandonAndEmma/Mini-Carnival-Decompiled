using UnityEngine;

public class UI_HungerRedMgr : MonoBehaviour
{
	private Texture2D _tex;

	[SerializeField]
	private TUIMeshSprite_Avatar _meshSprite;

	private void Awake()
	{
		_tex = Resources.Load("UI/hunger_red") as Texture2D;
	}

	private void Start()
	{
		_meshSprite.UseCustomize = true;
		_meshSprite.CustomizeTexture = _tex;
		_meshSprite.CustomizeRect = new Rect(0f, 0f, _tex.width, _tex.height);
		Debug.Log("tex width:" + _tex.width + "   tex height:" + _tex.height);
	}

	private void Update()
	{
	}
}
