using UnityEngine;

public class UI_3DModeToTUIMgr : MonoBehaviour
{
	private static UI_3DModeToTUIMgr _instance;

	public GameObject ptlFlowerObj;

	public GameObject[] sourceObjs;

	[SerializeField]
	private UI_3DModeToTUI[] _3dModeToTUI;

	public static UI_3DModeToTUIMgr Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public UI_3DModeToTUI Get3DModeToTUI(int index)
	{
		if (index < 0 || index >= _3dModeToTUI.Length)
		{
			return null;
		}
		return _3dModeToTUI[index];
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
