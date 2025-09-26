using UnityEngine;

namespace TUINameSpace
{
	public class UICursor : MonoBehaviour
	{
		[SerializeField]
		private MeshRenderer render;

		private float fPreShowTime;

		private float fFlashInterval = 0.5f;

		private float fStartPos;

		private bool bFocus;

		public bool Focus
		{
			get
			{
				return bFocus;
			}
			set
			{
				bFocus = value;
				if (bFocus)
				{
					fPreShowTime = Time.time;
					render.enabled = true;
				}
				else
				{
					render.enabled = false;
				}
			}
		}

		private void Start()
		{
			fPreShowTime = Time.time;
			fStartPos = base.transform.localPosition.x;
			render.enabled = false;
		}

		private void Update()
		{
			if (Focus && Time.time - fPreShowTime >= fFlashInterval)
			{
				render.enabled = !render.enabled;
				fPreShowTime = Time.time;
			}
		}

		public void RefreshPos(float fW)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = fStartPos;
			localPosition.x += fW;
			base.transform.localPosition = localPosition;
		}
	}
}
