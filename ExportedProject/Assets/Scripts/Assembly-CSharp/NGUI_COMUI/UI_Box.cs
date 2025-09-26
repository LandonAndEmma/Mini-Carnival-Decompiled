using UnityEngine;

namespace NGUI_COMUI
{
	public class UI_Box : MonoBehaviour
	{
		[SerializeField]
		protected GameObject _objSlotSelIcon;

		[SerializeField]
		protected UITexture _mainTex;

		[SerializeField]
		protected UISprite _mainSprite;

		protected UI_BoxData _boxData;

		[SerializeField]
		protected bool _isAvailable = true;

		public UI_BoxData BoxData
		{
			get
			{
				return _boxData;
			}
			set
			{
				_boxData = value;
				if (_boxData != null)
				{
					_boxData.SetDataOwner(this);
				}
				BoxDataChanged();
			}
		}

		public bool IsAvailable
		{
			get
			{
				return _isAvailable;
			}
		}

		public void SetAvailable(bool available)
		{
			_isAvailable = available;
			base.gameObject.SetActive(available);
		}

		public void ClearBoxData()
		{
			BoxData = null;
			if (BoxData != null)
			{
				BoxData.SetDataOwner(null);
			}
		}

		public virtual void FormatBoxName(int i)
		{
			Debug.LogWarning("FormatBoxName No Realize!!!");
		}

		public virtual void BoxDataChanged()
		{
			Debug.LogWarning("BoxDataChanged  No Realize!!!");
		}

		public virtual void SetSelected()
		{
			if (_objSlotSelIcon != null)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Select);
				_objSlotSelIcon.SetActive(true);
			}
		}

		public virtual void SetSelectedGameMode()
		{
			if (_objSlotSelIcon != null)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
				_objSlotSelIcon.SetActive(true);
			}
		}

		public virtual void SetLoseSelected()
		{
			if (_objSlotSelIcon != null)
			{
				_objSlotSelIcon.SetActive(false);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
