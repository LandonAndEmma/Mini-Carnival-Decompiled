using System.Collections.Generic;
using UnityEngine;

public class COMA_TankModel : MonoBehaviour
{
	public delegate void TripleShotCallBack();

	public float _fMaxSpeed = 5f;

	public float _fMaxForce = 10f;

	public float _fDragForce = 5f;

	public Transform shadow;

	private static float fShadowHeight;

	public Transform _turret;

	public Transform _barrel;

	public Transform _tankBase;

	public Transform _characterMout;

	private float _forwardPriority = 30f;

	private Quaternion _lastRotation = Quaternion.identity;

	private Vector3 _vLastPos = Vector3.zero;

	public GameObject[] _tankModelTextuer;

	public bool _bTripleTank;

	private string _strLastDeadAnim;

	public TripleShotCallBack shotAnimCallback;

	public List<Material> _wheelMats;

	public SkinnedMeshRenderer _wheelRender;

	public static readonly float _fDeltaOffset = 0.1f;

	private float _fTimeToUseNextMat = _fDeltaOffset;

	private int _nCurIndex;

	public static void resetShadowHeight()
	{
		fShadowHeight = 0f;
	}

	public void HideAdd(float r, float g, float b, float alpha)
	{
		Color color = new Color(r, g, b, alpha);
		_tankModelTextuer[0].renderer.material.color = color;
		_tankModelTextuer[1].renderer.material.color = color;
		_tankModelTextuer[2].renderer.material.color = color;
		_tankModelTextuer[3].renderer.material.color = color;
	}

	private void Start()
	{
		shadow.transform.localPosition = new Vector3(0f, fShadowHeight, 0f);
		fShadowHeight += 0.01f;
		base.animation["Forward"].layer = 1;
		base.animation["Fire"].layer = 2;
		if (base.animation["Fire02"] != null)
		{
			base.animation["Fire02"].layer = 2;
		}
		base.animation["Death01"].layer = 7;
		base.animation["Death02"].layer = 7;
		base.animation["Death03"].layer = 7;
		base.animation["Death01"].wrapMode = WrapMode.ClampForever;
		base.animation["Death02"].wrapMode = WrapMode.ClampForever;
		base.animation["Death03"].wrapMode = WrapMode.ClampForever;
		_lastRotation = _tankBase.rotation;
	}

	protected void LateUpdate()
	{
		if (_vLastPos != Vector3.zero)
		{
			Vector3 vDelta = base.transform.position - _vLastPos;
			updateTankBaseForward(vDelta);
			if (vDelta.magnitude > 0.05f)
			{
				UpdateWheel();
			}
		}
		_vLastPos = base.transform.position;
	}

	public void updateTankBaseRotaion(Vector3 vDelta)
	{
		Quaternion quaternion = Quaternion.Euler(vDelta.normalized);
	}

	public void updateTankBaseForward(Vector3 vDelta)
	{
		if (vDelta.magnitude > 0f)
		{
			Vector3 forward = _tankBase.forward;
			Quaternion quaternion = Quaternion.FromToRotation(forward, vDelta);
			Quaternion quaternion2 = Quaternion.FromToRotation(vDelta, forward);
			Quaternion quaternion3 = Quaternion.FromToRotation(-forward, vDelta);
			Quaternion quaternion4 = Quaternion.FromToRotation(vDelta, -forward);
			float num = Mathf.Min(quaternion.eulerAngles.y, quaternion2.eulerAngles.y);
			float num2 = Mathf.Min(quaternion3.eulerAngles.y, quaternion4.eulerAngles.y);
			int num3 = ((num < num2 + _forwardPriority) ? 1 : (-1));
			_tankBase.forward = Vector3.Lerp(forward, num3 * vDelta.normalized, 0.3f);
			base.animation.CrossFade("Forward");
		}
		else
		{
			base.animation.CrossFade("Idle");
			_tankBase.rotation = _lastRotation;
		}
		Vector3 eulerAngles = _tankBase.rotation.eulerAngles;
		_tankBase.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
		_lastRotation = _tankBase.rotation;
	}

	public void playFireAnim(string strCharacterAnimName)
	{
		string text = ((!_bTripleTank) ? "Fire" : "Fire02");
		if (base.animation.IsPlaying(text))
		{
			base.animation.Rewind(text);
		}
		base.animation.CrossFade(text, 0.3f);
	}

	public void playDeadAnim(int nId, float fSpeed)
	{
		Debug.Log("play dead speed" + fSpeed);
		string strLastDeadAnim = "Death0" + nId;
		if (fSpeed == 0f)
		{
			base.animation.Stop(strLastDeadAnim);
			base.animation.CrossFade("Idle");
		}
		else
		{
			base.animation.CrossFade(strLastDeadAnim, 0.3f);
			_strLastDeadAnim = strLastDeadAnim;
		}
	}

	public void OnRelive()
	{
		Debug.Log("-----------------------------model reset anim" + _strLastDeadAnim);
		base.animation[_strLastDeadAnim].time = 0f;
		base.animation.Sample();
		base.animation.Stop(_strLastDeadAnim);
	}

	private void TripleShotCallback()
	{
		if (shotAnimCallback != null)
		{
			shotAnimCallback();
		}
	}

	private void UpdateWheel()
	{
		_fTimeToUseNextMat -= Time.deltaTime;
		if (_fTimeToUseNextMat < 0f)
		{
			_fTimeToUseNextMat = _fDeltaOffset;
			doUseNextMaterial();
		}
	}

	private void doUseNextMaterial()
	{
		if (++_nCurIndex >= _wheelMats.Count)
		{
			_nCurIndex = 0;
		}
		_wheelRender.material = _wheelMats[_nCurIndex];
	}
}
