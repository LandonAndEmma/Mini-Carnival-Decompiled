using UnityEngine;

public class COMA_TankHp : MonoBehaviour
{
	public Material red;

	public Material blue;

	public Transform _hpContent;

	public Transform _fireContent;

	private float _fHpMaxLen = 1.249f;

	public MeshRenderer _bloodMeshRender;

	private float fRatio = 1f;

	private void Start()
	{
		_bloodMeshRender = _hpContent.GetComponent<MeshRenderer>();
	}

	public void setTeamMaterial(bool bAliance)
	{
		Debug.Log("------------bAliance:" + bAliance);
		_bloodMeshRender.material = ((!bAliance) ? red : blue);
	}

	public void setHpRatio(float fRatio)
	{
		Debug.Log("set hp ratio:" + fRatio);
		_hpContent.localScale = new Vector3(fRatio, 1f, 1f);
		_hpContent.localPosition = new Vector3((1f - fRatio) * 0.5f * _fHpMaxLen, 0f, 0f);
	}

	public void setFireRatio(float fRatio)
	{
		if (_fireContent != null)
		{
			_fireContent.localScale = new Vector3(fRatio, 1f, 0.6f);
			_fireContent.localPosition = new Vector3((1f - fRatio) * 0.5f * _fHpMaxLen, -0.1078031f, 0f);
		}
	}

	private void Update()
	{
		base.transform.forward = -Camera.main.transform.forward;
	}
}
