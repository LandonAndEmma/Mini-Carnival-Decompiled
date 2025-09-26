using UnityEngine;

public class MainCameraTrans : MonoBehaviour
{
	private CameraParam _cameraParam = new CameraParam();

	private void Start()
	{
		string text = null;
		text = ((!RPGRefree.Instance.IsPCLoadFromCurScene()) ? COMA_FileIO.LoadFile("Configs", "CameraParams.xml") : COMA_FileIO.ReadTextDirectly("Data/CameraParams.xml"));
		_cameraParam = COMA_Tools.DeserializeObject<CameraParam>(text) as CameraParam;
		base.transform.position = _cameraParam._vPos;
		base.transform.rotation = Quaternion.Euler(_cameraParam._vRot);
		if (Screen.height % 768 == 0)
		{
			base.transform.position = new Vector3(0f, 9f, 8.7f);
		}
	}

	private void Update()
	{
	}
}
