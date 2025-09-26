using UnityEngine;

public class NGUI_SwitchMask : MonoBehaviour
{
	[SerializeField]
	private GameObject _mask;

	private void Awake()
	{
		_mask.SetActive(false);
	}

	private void Enable()
	{
		_mask.SetActive(false);
	}

	private void Start()
	{
		if ((UIDataBufferCenter.Instance.PreSceneName == "COMA_Login" && Application.loadedLevelName == "UI.Square") || (UIDataBufferCenter.Instance.PreSceneName == "UI.Square" && Application.loadedLevelName == "UI.RPG.Map"))
		{
			UIDataBufferCenter.Instance.PreSceneName = string.Empty;
			_mask.SetActive(true);
		}
		else
		{
			_mask.SetActive(false);
		}
	}

	private void Update()
	{
	}
}
