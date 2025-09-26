using UnityEngine;

public class UI_MeshSpriteColorInit : MonoBehaviour
{
	private bool _bInited;

	private Shader _shader;

	private void Start()
	{
		_shader = Shader.Find("Triniti/SpriteC");
	}

	private void Update()
	{
		if (_bInited)
		{
			Object.DestroyObject(this);
			return;
		}
		base.renderer.materials[0].shader = _shader;
		base.renderer.materials[0].SetColor("_Color", new Color(1f, 1f, 1f, 0.5f));
		_bInited = true;
	}
}
