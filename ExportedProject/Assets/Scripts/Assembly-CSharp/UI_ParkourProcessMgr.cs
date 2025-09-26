using UnityEngine;

public class UI_ParkourProcessMgr : MonoBehaviour
{
	[SerializeField]
	private TUIMeshSprite[] _playes;

	public ParkourProcessInfo[] _infos = new ParkourProcessInfo[4];

	[SerializeField]
	private UI_3DModeToTexture _modeToTex;

	private void Awake()
	{
		for (int i = 0; i < 4; i++)
		{
			_infos[i] = new ParkourProcessInfo();
			_infos[i].SetMgr(this);
		}
	}

	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			_infos[i].PlayerTex = _modeToTex.GetTexById(i, _infos[i].DelayAssignment);
		}
	}

	private void Update()
	{
	}

	public void DataChanged()
	{
		int num = 0;
		TUIMeshSprite[] playes = _playes;
		foreach (TUIMeshSprite tUIMeshSprite in playes)
		{
			tUIMeshSprite.UseCustomize = true;
			tUIMeshSprite.CustomizeTexture = _infos[num].PlayerTex;
			if (tUIMeshSprite.CustomizeTexture != null)
			{
				tUIMeshSprite.CustomizeRect = new Rect(0f, 0f, tUIMeshSprite.CustomizeTexture.width, tUIMeshSprite.CustomizeTexture.height);
			}
			Vector3 localPosition = tUIMeshSprite.gameObject.transform.localPosition;
			localPosition.x = (_infos[num].PlayerProcess - 0.5f) * 160f;
			localPosition.z = 0f - _infos[num].PlayerProcess;
			tUIMeshSprite.gameObject.transform.localPosition = localPosition;
			num++;
		}
	}
}
