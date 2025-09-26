using UnityEngine;

public class UI_NewcomersWizardMgr : MonoBehaviour
{
	[SerializeField]
	private TUIRect[] _noMaskRect;

	[SerializeField]
	private GameObject[] _noMaskControls;

	private float[] _noMaskControlsZ;

	private Transform[] _noMaskControlsParents;

	[SerializeField]
	private Transform[] _noMaskControlsTempParents;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshUIControllers(float fz)
	{
		if (_wizardMask != null)
		{
			_wizardMask.ActiveExtraEffect();
		}
		if (_noMaskRect.Length > 0)
		{
			Rect[] array = new Rect[_noMaskRect.Length];
			int num = 0;
			TUIRect[] noMaskRect = _noMaskRect;
			foreach (TUIRect tUIRect in noMaskRect)
			{
				array[num].width = tUIRect.Size.x * 2f;
				array[num].height = tUIRect.Size.y * 2f;
				float x = tUIRect.gameObject.transform.position.x;
				float y = tUIRect.gameObject.transform.position.y;
				float num2 = x - tUIRect.Size.x / 2f + 240f;
				float num3 = y - tUIRect.Size.y / 2f + 160f;
				array[num].x = num2 * 2f;
				array[num].y = num3 * 2f;
				num++;
			}
			_wizardMask.AddNoMaskRect(array);
		}
		if (_noMaskControls.Length > 0)
		{
			_noMaskControlsZ = new float[_noMaskControls.Length];
			_noMaskControlsParents = new Transform[_noMaskControls.Length];
			int num4 = 0;
			GameObject[] noMaskControls = _noMaskControls;
			foreach (GameObject gameObject in noMaskControls)
			{
				_noMaskControlsZ[num4] = gameObject.transform.position.z;
				_noMaskControlsParents[num4] = gameObject.transform.parent;
				Vector3 position = gameObject.transform.position;
				position.z = fz;
				gameObject.transform.position = position;
				gameObject.transform.parent = _noMaskControlsTempParents[num4];
				num4++;
			}
		}
	}

	public void RefreshUIControllers()
	{
		RefreshUIControllers(-330f);
	}

	public void ResetUIControllersZ()
	{
		int num = 0;
		GameObject[] noMaskControls = _noMaskControls;
		foreach (GameObject gameObject in noMaskControls)
		{
			gameObject.transform.parent = _noMaskControlsParents[num];
			Vector3 position = gameObject.transform.position;
			position.z = _noMaskControlsZ[num++];
			gameObject.transform.position = position;
		}
		_wizardMask.ResetMask();
	}
}
