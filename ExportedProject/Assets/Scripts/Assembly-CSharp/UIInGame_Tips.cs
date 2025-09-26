using UnityEngine;

public class UIInGame_Tips : MonoBehaviour
{
	[SerializeField]
	private GameObject _objTips;

	public bool _bTest;

	private void Awake()
	{
		_objTips.SetActive(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_bTest)
		{
			OpenInGameTips();
			_bTest = false;
		}
	}

	public void OpenInGameTips()
	{
		_objTips.SetActive(true);
		if (base.animation != null)
		{
			base.animation.Play();
			SceneTimerInstance.Instance.Add(base.animation["UIInGame_CountDown2"].length + 0.3f, CloseInGameTips);
		}
	}

	protected bool CloseInGameTips()
	{
		_objTips.SetActive(false);
		return false;
	}
}
