using UnityEngine;

public class UI_Effect_Ink : MonoBehaviour
{
	[SerializeField]
	private GameObject _objAppear;

	[SerializeField]
	private GameObject _objDisappear;

	public bool _bTest;

	private float _fStartTime = -1f;

	private float _fEndTime = -1f;

	private void Start()
	{
		EndEffect();
	}

	private void Update()
	{
		if (_bTest)
		{
			PlayEffect();
			_bTest = false;
		}
		if (_fStartTime >= 0f && Time.time - _fStartTime >= 4f)
		{
			DisappearEvent();
		}
		if (_fEndTime >= 0f && Time.time - _fEndTime >= 1f)
		{
			EndEffect();
		}
	}

	public void PlayEffect()
	{
		Debug.Log("PlayEffect");
		_fEndTime = -1f;
		_objAppear.animation.Rewind();
		_objAppear.SetActive(true);
		_objDisappear.SetActive(false);
		_fStartTime = Time.time;
	}

	protected bool DisappearEvent()
	{
		_objDisappear.animation.Rewind();
		_objAppear.SetActive(false);
		_objDisappear.SetActive(true);
		_fEndTime = Time.time;
		_fStartTime = -1f;
		return false;
	}

	public bool EndEffect()
	{
		_objAppear.SetActive(false);
		_objDisappear.SetActive(false);
		_fStartTime = -1f;
		_fEndTime = -1f;
		return false;
	}
}
