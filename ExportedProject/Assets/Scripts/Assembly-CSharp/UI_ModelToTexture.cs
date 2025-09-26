using System.Collections.Generic;
using UnityEngine;

public class UI_ModelToTexture : MonoBehaviour
{
	[SerializeField]
	public class UITexture
	{
		private RenderTexture _tex;

		private bool _bRT;

		private Transform _relativeTrans;

		private Camera _camera;

		private Vector2 _size;

		public RenderTexture Tex
		{
			get
			{
				return _tex;
			}
			set
			{
				_tex = value;
			}
		}

		public UITexture(bool bRT, Vector3 rPos, Vector3 rScale, Quaternion rRot, Camera cam, Vector2 size)
		{
			_bRT = bRT;
			_camera = cam;
			_size = size;
			_relativeTrans.localPosition = rPos;
			_relativeTrans.localScale = rScale;
			_relativeTrans.localRotation = rRot;
		}

		public void Create()
		{
		}
	}

	[SerializeField]
	private Camera _gCamera;

	private List<UITexture> _lstUITexs = new List<UITexture>();

	private void Start()
	{
	}

	private void Update()
	{
	}

	public RenderTexture GetTexByModel(GameObject obj, Vector2 size, bool bRT, Camera cam, Vector3 rPos, Vector3 rScale, Quaternion rRot)
	{
		UITexture uITexture = new UITexture(bRT, rPos, rScale, rRot, cam, size);
		uITexture.Create();
		return uITexture.Tex;
	}

	public RenderTexture GetTexByModel(GameObject obj, Vector2 size)
	{
		return GetTexByModel(obj, size, false, null, Vector3.zero, new Vector3(1f, 1f, 1f), Quaternion.EulerAngles(0f, 0f, 0f));
	}
}
