using UnityEngine;

namespace NGUI_COMUI
{
	public class UI_BoxData
	{
		protected ulong _itemId;

		protected string _unit;

		protected Texture2D _tex;

		protected string _spriteName;

		[SerializeField]
		protected int _dataType;

		private UI_Box _dataOwner;

		public ulong ItemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
			}
		}

		public string Unit
		{
			get
			{
				return _unit;
			}
			set
			{
				_unit = value;
			}
		}

		public Texture2D Tex
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

		public string SpriteName
		{
			get
			{
				return _spriteName;
			}
			set
			{
				_spriteName = value;
			}
		}

		public int DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
			}
		}

		public UI_Box DataOwner
		{
			get
			{
				return _dataOwner;
			}
		}

		public void SetDataOwner(UI_Box box)
		{
			_dataOwner = box;
		}

		public void SetDirty()
		{
			if (DataOwner != null)
			{
				DataOwner.BoxDataChanged();
			}
			else
			{
				Debug.LogWarning("DataOwner is NULL!!!!");
			}
		}
	}
}
