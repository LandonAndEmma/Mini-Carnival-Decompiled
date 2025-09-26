using UnityEngine;

public class UIInGame_CrystalVolumeMgr : MonoBehaviour
{
	[SerializeField]
	private GameObject _objCrystalVolume;

	private float _fVolume;

	[SerializeField]
	private float _fVolumePicLen = 65f;

	public bool _bTest;

	public float _fTest;

	private bool _bEnableTest;

	public float CrystalVolume
	{
		get
		{
			return _fVolume;
		}
		set
		{
			_fVolume = value;
			_fVolume = Mathf.Clamp01(_fVolume);
			if (_objCrystalVolume != null)
			{
				float x = _fVolumePicLen * (1f - _fVolume) * -0.5f;
				Vector3 localPosition = _objCrystalVolume.transform.localPosition;
				localPosition.x = x;
				_objCrystalVolume.transform.localPosition = localPosition;
				Vector3 localScale = _objCrystalVolume.transform.localScale;
				localScale.x = _fVolume;
				_objCrystalVolume.transform.localScale = localScale;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (_bEnableTest && _bTest)
		{
			CrystalVolume = _fTest;
			_bTest = false;
		}
	}
}
