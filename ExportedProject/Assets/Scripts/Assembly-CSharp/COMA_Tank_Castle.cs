using UnityEngine;

public class COMA_Tank_Castle : COMA_Tank_Breakable
{
	public int nScore = 10;

	public int teamIndex;

	public Transform[] subCastals;

	private float _fMaxHp;

	private int _nCurIndex;

	public COMA_TankHp _hpComp;

	private bool _hpColorSet;

	protected new void Start()
	{
		base.Start();
		_nDestoryScore = nScore;
		_fMaxHp = _tankModeHp;
	}

	protected override bool canbeDamaged(COMA_PlayerSelf from)
	{
		return TankCommon.getTeamIndex(from.sitIndex) != teamIndex;
	}

	protected override void onHpChange(float fNewHp)
	{
		if (fNewHp < _fMaxHp * 2f / 3f && _nCurIndex < 1)
		{
			showSubCastle(1);
			playCastleStageChangeEffect();
		}
		else if (fNewHp < _fMaxHp / 3f && _nCurIndex < 2)
		{
			showSubCastle(2);
			playCastleStageChangeEffect();
		}
		_hpComp.setHpRatio(fNewHp / _fMaxHp);
	}

	private void playCastleStageChangeEffect()
	{
		GameObject obj = Object.Instantiate(Resources.Load("Particle/effect/Tank_Effect/Castle_Broken/Castle_Broken_pfb"), base.transform.position, base.transform.rotation) as GameObject;
		Object.DestroyObject(obj, 3f);
	}

	protected override void onDestoryed(bool bFromNet)
	{
		playCastleStageChangeEffect();
		Object.DestroyObject(base.gameObject, 0.5f);
	}

	private void showSubCastle(int nIndex)
	{
		if (nIndex < subCastals.Length)
		{
			Transform[] array = subCastals;
			foreach (Transform transform in array)
			{
				transform.gameObject.SetActive(false);
			}
			subCastals[nIndex].gameObject.SetActive(true);
			_nCurIndex = nIndex;
		}
		else
		{
			Debug.LogError("nIndex error!");
		}
	}

	private new void Update()
	{
		base.Update();
		setHpColor();
	}

	private void setHpColor()
	{
		if (!_hpColorSet && COMA_PlayerSelf.Instance != null)
		{
			_hpComp.setTeamMaterial(teamIndex == TankCommon.getTeamIndex(COMA_PlayerSelf.Instance.sitIndex));
			_hpColorSet = true;
		}
	}
}
